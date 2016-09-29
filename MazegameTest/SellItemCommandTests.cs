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
    public class SellItemCommandTests
    {
        private Weapon weapon = new Weapon() { Description = "weapon", Worth = 10, Weight = 5 };
        private Armor armor = new Armor() { Description = "armor", Worth = 20, Weight = 10 };

        private GameContextStub CreateGameContext()
        {
            return new GameContextStub()
            {
                Player = new Player("", 1, 1, 10)
                {
                    Location = new Shop("shopDescription", "shop"),
                    Backpack = new List<Item>()
                        {
                            weapon,
                            armor,
                    },
                    Gold = 100
                }
            };
        }

        [TestMethod]
        public void SellItemIsAvailable_WhenInShop_ReturnsTrue()
        {
            var context = new GameContextStub
            {
                Player = new Player("", 1, 1, 1) { Location = new Shop("shopDescription", "shop") }
            };
            var command = new SellItemCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsTrue(available);
        }

        [TestMethod]
        public void SellItemIsAvailable_WhenNotInShop_ReturnsFalse()
        {
            var context = new GameContextStub
            {
                Player = new Player("", 1, 1, 1) { Location = new Location("cave", "cave") }
            };
            var command = new SellItemCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsFalse(available);
        }

        [TestMethod]
        public void SellItemExecute_WhenArgumentIsNull_ShouldReturnBackpackItemsList()
        {
            var context = CreateGameContext();
            var command = new SellItemCommand();

            var response = command.Execute(context, null);

            StringAssert.Contains(response, "Chooze some item to sell from your backpack");
            StringAssert.Contains(response, weapon.Description + " (worth 10)");
            StringAssert.Contains(response, armor.Description + " (worth 20)");

            Assert.AreEqual(context.Player.Gold, 100);

            CollectionAssert.Contains(context.Player.Backpack, weapon);
            CollectionAssert.Contains(context.Player.Backpack, armor);

            var shop = (Shop)context.Player.Location;
            CollectionAssert.DoesNotContain(shop.Store, weapon);
            CollectionAssert.DoesNotContain(shop.Store, armor);
        }

        [TestMethod]
        public void SellItemExecute_WhenArgumentIs0_ShouldSellWeapon()
        {
            var context = CreateGameContext();
            var command = new SellItemCommand();

            var response = command.Execute(context, "0");

            StringAssert.Contains(response, "You sold " + weapon.Description);

            Assert.AreEqual(context.Player.Gold, 110);

            CollectionAssert.DoesNotContain(context.Player.Backpack, weapon);
            CollectionAssert.Contains(context.Player.Backpack, armor);

            var shop = (Shop)context.Player.Location;
            CollectionAssert.Contains(shop.Store, weapon);
            CollectionAssert.DoesNotContain(shop.Store, armor);
        }

        [TestMethod]
        public void SellItemExecute_WhenArgumentIs1_ShouldSellArmor()
        {
            var context = CreateGameContext();
            var command = new SellItemCommand();

            var response = command.Execute(context, "1");

            StringAssert.Contains(response, "You sold " + armor.Description);

            Assert.AreEqual(context.Player.Gold, 120);

            CollectionAssert.Contains(context.Player.Backpack, weapon);
            CollectionAssert.DoesNotContain(context.Player.Backpack, armor);

            var shop = (Shop)context.Player.Location;
            CollectionAssert.DoesNotContain(shop.Store, weapon);
            CollectionAssert.Contains(shop.Store, armor);
        }
    }
}
