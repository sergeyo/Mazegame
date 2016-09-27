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
    public class ListItemsCommandTests
    {
        class NonWearableItem : Item { }

        private Weapon weapon = new Weapon() { Description = "weapon", Weight = 1 };
        private Armor armor = new Armor() { Description = "armor", Weight = 2 };
        private Shield shield = new Shield() { Description = "shield", Weight = 3 };
        private NonWearableItem item = new NonWearableItem() { Description = "item", Weight = 5 };

        private GameContextStub CreateGameContext(params Item[] items)
        {
            return new GameContextStub()
            {
                Player = new Player("", 1, 1, 10)
                {
                    Backpack = items.ToList()
                }
            };
        }

        [TestMethod]
        public void ListItemsIsAvailable_WhenInCombat_ReturnsFalse()
        {
            var context = new GameContextStub() { IsInCombatMode = true };
            var command = new ListItemsCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsFalse(available);
        }

        [TestMethod]
        public void ListItemsIsAvailable_WhenNotInCombat_ReturnsTrue()
        {
            var context = new GameContextStub() { IsInCombatMode = false };
            var command = new ListItemsCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsTrue(available);
        }

        [TestMethod]
        public void ListItemsExecute_WhenBackpackIsEmpty_ShouldReturnMessage()
        {
            var context = CreateGameContext();
            var command = new ListItemsCommand();

            var response = command.Execute(context, null);

            StringAssert.Contains(response, "Your backpack is empty");
        }

        [TestMethod]
        public void ListItemsExecute_WhenBackpackContainsWeapon_ShouldReturnWeaponDescription()
        {
            var context = CreateGameContext(weapon);
            var command = new ListItemsCommand();

            var response = command.Execute(context, null);

            StringAssert.Contains(response, "Here is your backpack content (weight: 1/20)");
            StringAssert.Contains(response, weapon.Description + " (weight 1)");
            Assert.IsFalse(response.Contains(armor.Description));
            Assert.IsFalse(response.Contains(shield.Description));
            Assert.IsFalse(response.Contains(item.Description));
        }

        [TestMethod]
        public void ListItemsExecute_WhenBackpackContains4tems_ShouldReturnTheirDescriptions()
        {
            var context = CreateGameContext(weapon, armor, shield, item);
            var command = new ListItemsCommand();

            var response = command.Execute(context, null);

            StringAssert.Contains(response, "Here is your backpack content (weight: 11/20)");
            StringAssert.Contains(response, weapon.Description + " (weight 1)");
            StringAssert.Contains(response, armor.Description + " (weight 2)");
            StringAssert.Contains(response, shield.Description + " (weight 3)");
            StringAssert.Contains(response, item.Description + " (weight 5)");
        }
    }
}
