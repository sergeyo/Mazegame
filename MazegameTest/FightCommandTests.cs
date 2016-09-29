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

            var responce = command.Execute(context, null);

            Assert.AreEqual(100, context.Player.LifePoints);
            Assert.AreEqual(100, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(responce, "Player misses");
            StringAssert.Contains(responce, "NPC misses");
            Assert.IsNotNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenAttackEnrollmentPlusStrengthGreaterThanAC_ReturnsBothHits()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(6));

            var responce = command.Execute(context, null);

            Assert.AreEqual(100, context.Player.LifePoints);
            Assert.AreEqual(100, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(responce, "Player misses");
            StringAssert.Contains(responce, "NPC misses");
            Assert.IsNotNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenAttackEnrollmentPlusStrengthEqualsAC_ReturnsBothHits()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(7));

            var responce = command.Execute(context, null);

            Assert.AreEqual(75, context.Player.LifePoints);
            Assert.AreEqual(65, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(responce, "Player hits NPC");
            StringAssert.Contains(responce, "NPC hits Player");
            Assert.IsNotNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenAttackEnrollmentIs20_ReturnsBothCriticalHits()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(20));

            var responce = command.Execute(context, null);

            Assert.AreEqual(50, context.Player.LifePoints);
            Assert.AreEqual(30, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(responce, "Player critically hits NPC");
            StringAssert.Contains(responce, "NPC critically hits Player");
            Assert.IsNotNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenDamageGreaterThanNPCLifepoints_ReturnsNPCDiesAndNotHitsPlayer()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(20));

            context.Player.Weapon = CreateConstWeapon(50);

            var responce = command.Execute(context, null);

            Assert.AreEqual(100, context.Player.LifePoints);
            Assert.AreEqual(-20, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(responce, "Player critically hits NPC");
            StringAssert.Contains(responce, "You just killed NPC");
            Assert.IsNull(context.Player.Location.NPC);
        }

        [TestMethod]
        public void FightExecute_WhenDamageGreaterThanPlayerLifepoints_ReturnsPlayerDied()
        {
            var context = CreateGameContext();
            var command = new FightCommand(CreateConstDice(20));

            context.CurrentLocationEnemy.Weapon = CreateConstWeapon(50);

            var responce = command.Execute(context, null);

            Assert.AreEqual(-20, context.Player.LifePoints);
            Assert.AreEqual(30, context.CurrentLocationEnemy.LifePoints);
            StringAssert.Contains(responce, "Player critically hits NPC");
            StringAssert.Contains(responce, "NPC critically hits Player");
            StringAssert.Contains(responce, "You died");
            Assert.IsNotNull(context.Player.Location.NPC);
        }
    }
}
