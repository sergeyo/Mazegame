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
    public class DropItemCommandTests
    {
        private Weapon weapon = new Weapon() { Description = "weapon" };
        private Armor armor = new Armor() { Description = "armor" };
        private Shield shield = new Shield() { Description = "shield" };
        private NonWearableItem item = new NonWearableItem() { Description = "item" };

        private GameContextStub CreateGameContext()
        {
            return new GameContextStub()
            {
                Player = new Player("", 1, 1, 1)
                {
                    Backpack = new List<Item> {
                        weapon,
                        armor,
                    },
                    Location = new Location("", "")
                    {
                        CollectableItems = new List<Item>()
                        {
                            shield,
                            item
                        }
                    }
                }
            };
        }


        [TestMethod]
        public void DropItemIsAvailable_WhenInCombatMode_ShouldReturnFalse()
        {
            var context = new GameContextStub() { IsInCombatMode = true };
            var command = new DropItemCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsFalse(available);
        }

        [TestMethod]
        public void DropItemIsAvailable_WhenNotInCombatMode_ShouldReturnTrue()
        {
            var context = new GameContextStub() { IsInCombatMode = false };
            var command = new DropItemCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsTrue(available);
        }

        [TestMethod]
        public void DropItemExecute_WhenNoArgumentsSpecified_ShouldReturnBackpackContent()
        {
            var context = CreateGameContext();
            var command = new DropItemCommand();

            var response = command.Execute(context, null);

            CollectionAssert.Contains(context.Player.Backpack, weapon);
            CollectionAssert.Contains(context.Player.Backpack, armor);
            CollectionAssert.DoesNotContain(context.Player.Backpack, shield);
            CollectionAssert.DoesNotContain(context.Player.Backpack, item);

            CollectionAssert.DoesNotContain(context.Player.Location.CollectableItems, weapon);
            CollectionAssert.DoesNotContain(context.Player.Location.CollectableItems, armor);
            CollectionAssert.Contains(context.Player.Location.CollectableItems, shield);
            CollectionAssert.Contains(context.Player.Location.CollectableItems, item);

            StringAssert.Contains(response, "You can drop something from your backpack:");
            StringAssert.Contains(response, weapon.Description);
            StringAssert.Contains(response, armor.Description);
            Assert.IsFalse(response.Contains(shield.Description));
            Assert.IsFalse(response.Contains(item.Description));
        }

        [TestMethod]
        public void DropItemExecute_WhenArgumentsIs0_ShouldDropWeapon()
        {
            var context = CreateGameContext();
            var command = new DropItemCommand();

            var response = command.Execute(context, "0");

            CollectionAssert.DoesNotContain(context.Player.Backpack, weapon);
            CollectionAssert.Contains(context.Player.Backpack, armor);
            CollectionAssert.DoesNotContain(context.Player.Backpack, shield);
            CollectionAssert.DoesNotContain(context.Player.Backpack, item);

            CollectionAssert.Contains(context.Player.Location.CollectableItems, weapon);
            CollectionAssert.DoesNotContain(context.Player.Location.CollectableItems, armor);
            CollectionAssert.Contains(context.Player.Location.CollectableItems, shield);
            CollectionAssert.Contains(context.Player.Location.CollectableItems, item);

            StringAssert.Contains(response, "You dropped");
            StringAssert.Contains(response, weapon.Description);
            Assert.IsFalse(response.Contains(armor.Description));
            Assert.IsFalse(response.Contains(shield.Description));
            Assert.IsFalse(response.Contains(item.Description));
        }

        [TestMethod]
        public void DropItemExecute_WhenArgumentsIs1_ShouldDropArmor()
        {
            var context = CreateGameContext();
            var command = new DropItemCommand();

            var response = command.Execute(context, "1");

            CollectionAssert.Contains(context.Player.Backpack, weapon);
            CollectionAssert.DoesNotContain(context.Player.Backpack, armor);
            CollectionAssert.DoesNotContain(context.Player.Backpack, shield);
            CollectionAssert.DoesNotContain(context.Player.Backpack, item);

            CollectionAssert.DoesNotContain(context.Player.Location.CollectableItems, weapon);
            CollectionAssert.Contains(context.Player.Location.CollectableItems, armor);
            CollectionAssert.Contains(context.Player.Location.CollectableItems, shield);
            CollectionAssert.Contains(context.Player.Location.CollectableItems, item);

            StringAssert.Contains(response, "You dropped");
            Assert.IsFalse(response.Contains(weapon.Description));
            StringAssert.Contains(response, armor.Description);
            Assert.IsFalse(response.Contains(shield.Description));
            Assert.IsFalse(response.Contains(item.Description));
        }
    }
}
