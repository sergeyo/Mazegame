using Mazegame.Commands.ItemCommands;
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
    public class UseItemCommandTests
    {
        private Weapon weapon = new Weapon() { Description = "weapon" };
        private HealthPotion healthPotion = new HealthPotion();

        private GameContextStub CreateGameContext()
        {
            var player = new Player("", 1, 100, 1)
            {
                Backpack = new List<Item> {
                        weapon,
                        healthPotion
                    }
            };

            player.RecieveDamage(50);

            return new GameContextStub()
            {
                Player = player
            };
        }

        [TestMethod]
        public void UseItemIsAvailable_WhenInCombatMode_ShouldReturnFalse()
        {
            var context = new GameContextStub() { IsInCombatMode = true };
            var command = new UseItemCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsFalse(available);
        }

        [TestMethod]
        public void UseItemIsAvailable_WhenNotInCombatMode_ShouldReturnTrue()
        {
            var context = new GameContextStub() { IsInCombatMode = false };
            var command = new UseItemCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsTrue(available);
        }

        [TestMethod]
        public void UseItemExecute_WhenNoArgument_ShouldListBackpackItems()
        {
            var context = CreateGameContext();
            var command = new UseItemCommand();

            var responce = command.Execute(context, null);

            Assert.AreEqual(50, context.Player.LifePoints);
            StringAssert.Contains(responce, "You can use something from your backpack");
            StringAssert.Contains(responce, "0: " + weapon.Description);
            StringAssert.Contains(responce, "1: " + healthPotion.Description);
        }

        [TestMethod]
        public void UseItemExecute_WhenArgumentIsOutOfRange_ShouldListBackpackItems()
        {
            var context = CreateGameContext();
            var command = new UseItemCommand();

            var responce = command.Execute(context, "2");

            Assert.AreEqual(50, context.Player.LifePoints);
            StringAssert.Contains(responce, "You should specify correct item number to use");
            StringAssert.Contains(responce, "0: " + weapon.Description);
            StringAssert.Contains(responce, "1: " + healthPotion.Description);
        }

        [TestMethod]
        public void UseItemExecute_WhenArgumentIs0_ShouldReturnCantUse()
        {
            var context = CreateGameContext();
            var command = new UseItemCommand();

            var responce = command.Execute(context, "0");

            Assert.AreEqual(50, context.Player.LifePoints);
            StringAssert.Contains(responce, "You can't use this Item");
        }

        [TestMethod]
        public void UseItemExecute_WhenArgumentIs1_ShouldRestoreSomeLifepoints()
        {
            var context = CreateGameContext();
            var command = new UseItemCommand();

            var responce = command.Execute(context, "1");

            var lifePointsRestored = context.Player.LifePoints - 50;
            Assert.IsTrue(2 <= lifePointsRestored && lifePointsRestored <= 12);
            StringAssert.Contains(responce, $"You have healed { lifePointsRestored } life points, now it's { context.Player.LifePoints }");
        }

        [TestMethod]
        public void UseItemExecute_WhenArgumentIs1AndHealtIsNearToMax_ShouldRestoreLifepointsToMax()
        {
            var context = CreateGameContext();
            var command = new UseItemCommand();

            context.Player.Heal(48);

            var responce = command.Execute(context, "1");

            Assert.AreEqual(100, context.Player.LifePoints);
            StringAssert.Contains(responce, $"You have healed 2 life points, now it's 100");
        }
    }
}
