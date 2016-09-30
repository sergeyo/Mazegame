using Mazegame.Commands;
using Mazegame.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazegameTest
{
    [TestClass]
    public class FightCommandTests
    {
        private NonPlayerCharacter CreateParty(int damage, int AC)
        {
            const int strength = 14;

            return new NonPlayerCharacter("Party", AC, 100, strength, false)
            {
                Weapon = CreateConstWeapon(damage - strength)
            };
        }

        private Dice CreateConstDice(int rollResult)
        {
            return new Dice(rollResult, 1);
        }

        private Weapon CreateConstWeapon(int damage)
        {
            return new Weapon()
            {
                Description = "sword",
                Dice = CreateConstDice(damage)
            };
        }

        private GameContextStub CreateGameContext()
        {
            var location = new Location("", "")
            {
                NPC = new NonPlayerCharacter("NPC", 10, 100, 10, true) // AC = 17
                {
                    Weapon = CreateConstWeapon(15),
                    Armor = new Armor() { Bonus = 5 },
                    Shield = new Shield() { Bonus = 2 }
                }
            };

            return new GameContextStub()
            {
                Player = new Player("Player", 10, 100, 10) //AC = 17
                {
                    Location = location,
                    Weapon = CreateConstWeapon(25),
                    Armor = new Armor() { Bonus = 5 },
                    Shield = new Shield() { Bonus = 2 }
                },
                CurrentLocationEnemy = location.NPC
            };
        }

        [TestMethod]
        public void FightIsAvailable_WhenInCombat_ReturnsTrue()
        {
            var context = new GameContextStub() { IsInCombatMode = true };
            var command = new FightCommand(CreateConstDice(1));

            var available = command.IsAvailableInContext(context);

            Assert.IsTrue(available);
        }

        [TestMethod]
        public void FightIsAvailable_WhenNotInCombat_ReturnsFalse()
        {
            var context = new GameContextStub() { IsInCombatMode = false };
            var command = new FightCommand(CreateConstDice(1));

            var available = command.IsAvailableInContext(context);

            Assert.IsFalse(available);
        }

        [TestMethod]
        public void FightExecute_WhenAttackEnrollmentDiceIs1_ReturnsBothMisses()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(1));

            var response = command.Execute(context, null);

            Assert.AreEqual(100, context.Player.LifePoints);
            Assert.AreEqual(100, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(response, "Player misses");
            StringAssert.Contains(response, "NPC misses");
            Assert.IsNotNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenAttackEnrollmentPlusStrengthGreaterThanAC_ReturnsBothHits()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(6));

            var response = command.Execute(context, null);

            Assert.AreEqual(100, context.Player.LifePoints);
            Assert.AreEqual(100, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(response, "Player misses");
            StringAssert.Contains(response, "NPC misses");
            Assert.IsNotNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenAttackEnrollmentPlusStrengthEqualsAC_ReturnsBothHits()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(7));

            var response = command.Execute(context, null);

            Assert.AreEqual(75, context.Player.LifePoints);
            Assert.AreEqual(65, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(response, "Player hits NPC");
            StringAssert.Contains(response, "NPC hits Player");
            Assert.IsNotNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenAttackEnrollmentIs20_ReturnsBothCriticalHits()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(20));

            var response = command.Execute(context, null);

            Assert.AreEqual(50, context.Player.LifePoints);
            Assert.AreEqual(30, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(response, "Player critically hits NPC");
            StringAssert.Contains(response, "NPC critically hits Player");
            Assert.IsNotNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenDamageGreaterThanNPCLifepoints_ReturnsNPCDiesAndNotHitsPlayer()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(20));

            context.Player.Weapon = CreateConstWeapon(50);

            var response = command.Execute(context, null);

            Assert.AreEqual(100, context.Player.LifePoints);
            Assert.AreEqual(-20, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(response, "Player critically hits NPC");
            StringAssert.Contains(response, "You just killed NPC");
            Assert.IsNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenDamageGreaterThanPlayerLifepoints_ReturnsPlayerDied()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(20));

            context.CurrentLocationEnemy.Weapon = CreateConstWeapon(50);

            var response = command.Execute(context, null);

            Assert.AreEqual(-20, context.Player.LifePoints);
            Assert.AreEqual(30, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(response, "Player critically hits NPC");
            StringAssert.Contains(response, "NPC critically hits Player");
            StringAssert.Contains(response, "You died");
            Assert.IsNotNull(context.Player.Location.NPC);
        }

        //Now firht with party
        [TestMethod]
        public void FightExecute_WhenHasPartyAttackEnrollmentDiceIs1_ReturnsBothMisses()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(1));
            context.Player.Party.Character = CreateParty(17, 12);

            var response = command.Execute(context, null);

            Assert.AreEqual(100, context.Player.LifePoints);
            Assert.AreEqual(100, context.Player.Party.Character.LifePoints);
            Assert.AreEqual(100, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(response, "Player misses");
            StringAssert.Contains(response, "Party misses");
            StringAssert.Contains(response, "NPC misses");
            Assert.IsNotNull(context.Player.Party.Character);
            Assert.IsNotNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenHasPartyAttackEnrollmentDiceIs2_ReturnsNPCHitsParty()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(2));
            context.Player.Party.Character = CreateParty(17, 12);

            var response = command.Execute(context, null);

            Assert.AreEqual(100, context.Player.LifePoints);
            Assert.AreEqual(75, context.Player.Party.Character.LifePoints);
            Assert.AreEqual(100, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(response, "Player misses");
            StringAssert.Contains(response, "Party misses");
            StringAssert.Contains(response, "NPC hits Party");
            Assert.IsNotNull(context.Player.Party.Character);
            Assert.IsNotNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenHasPartyAttackEnrollmentDiceIs3_ReturnsNPCHitsPartyAndReverse()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(3));
            context.Player.Party.Character = CreateParty(17, 12);

            var response = command.Execute(context, null);

            Assert.AreEqual(100, context.Player.LifePoints);
            Assert.AreEqual(75, context.Player.Party.Character.LifePoints);
            Assert.AreEqual(83, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(response, "Player misses");
            StringAssert.Contains(response, "Party hits NPC");
            StringAssert.Contains(response, "NPC hits Party");
            Assert.IsNotNull(context.Player.Party.Character);
            Assert.IsNotNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenHasPartyAttackEnrollmentDiceIs7_ReturnsNPCHitsPartyAndReverseAndPlayerHitsNPC()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(7));
            context.Player.Party.Character = CreateParty(17, 12);

            var response = command.Execute(context, null);

            Assert.AreEqual(100, context.Player.LifePoints);
            Assert.AreEqual(75, context.Player.Party.Character.LifePoints);
            Assert.AreEqual(100 - 17 - 35, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(response, "Player hits NPC");
            StringAssert.Contains(response, "Party hits NPC");
            StringAssert.Contains(response, "NPC hits Party");
            Assert.IsNotNull(context.Player.Party.Character);
            Assert.IsNotNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenHasPartyAttackEnrollmentDiceIs20_ReturnsEveryoneCirticalHits()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(20));
            context.Player.Party.Character = CreateParty(15, 12);

            context.Player.Weapon.Dice = CreateConstDice(10);

            var response = command.Execute(context, null);

            Assert.AreEqual(100, context.Player.LifePoints);
            Assert.AreEqual(50, context.Player.Party.Character.LifePoints);
            Assert.AreEqual(100 - 15 * 2 - 20 * 2, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(response, "Player critically hits NPC");
            StringAssert.Contains(response, "Party critically hits NPC");
            StringAssert.Contains(response, "NPC critically hits Party");
            Assert.IsNotNull(context.Player.Party.Character);
            Assert.IsNotNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenHasPartyAttackEnrollmentDiceIs20_Returns2CriticalsAndEnemyDiesAndDoesNotStrikesBack()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(20));
            context.Player.Party.Character = CreateParty(17, 12);

            var response = command.Execute(context, null);

            Assert.AreEqual(100, context.Player.LifePoints);
            Assert.AreEqual(100, context.Player.Party.Character.LifePoints);
            Assert.AreEqual(100 - 17 * 2 - 35 * 2, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(response, "Player critically hits NPC");
            StringAssert.Contains(response, "Party critically hits NPC");
            StringAssert.Contains(response, "You just killed NPC");
            Assert.IsNotNull(context.Player.Party.Character);
            Assert.IsNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenHasPartyAttackEnrollmentDiceIs7_Returns3HitsAndPartyDies()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(7));
            context.Player.Party.Character = CreateParty(17, 12);

            context.CurrentLocationEnemy.Weapon = CreateConstWeapon(110);

            var response = command.Execute(context, null);

            Assert.AreEqual(100, context.Player.LifePoints);
            //Assert.AreEqual(-20, context.Player.Party.Character.LifePoints);
            Assert.AreEqual(100 - 17 - 35, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(response, "Player hits NPC");
            StringAssert.Contains(response, "Party hits NPC");
            StringAssert.Contains(response, "NPC hits Party");
            StringAssert.Contains(response, "Party died");
            Assert.IsNull(context.Player.Party.Character);
            Assert.IsNotNull(context.Player.Location.NPC);
        }


        [TestMethod]
        public void FightExecute_WhenPlayerKillsEnemy_ReturnsPartyAndNPCDoestNotStrikes()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(7));
            context.Player.Party.Character = CreateParty(17, 12);

            context.Player.Weapon = CreateConstWeapon(110);

            var response = command.Execute(context, null);

            Assert.AreEqual(100, context.Player.LifePoints);
            Assert.AreEqual(100, context.Player.Party.Character.LifePoints);
            Assert.AreEqual(-20, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(response, "Player hits NPC");
            StringAssert.Contains(response, "You just killed NPC");
            Assert.IsFalse(response.Contains("Party"));
            
            Assert.IsNotNull(context.Player.Party.Character);
            Assert.IsNull(context.Player.Location.NPC);
        }
    }
}
