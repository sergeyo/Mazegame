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
    public class PurchaseItemCommandTests
    {
        private Weapon weapon = new Weapon() { Description = "weapon" , Worth = 10, Weight = 5 };
        private Armor armor = new Armor() { Description = "armor", Worth = 20, Weight = 10 };
        private NonWearableItem expensiveItem = new NonWearableItem() { Description = "expensiveItem", Worth = 500, Weight = 1 };
        private NonWearableItem heavyItem = new NonWearableItem() { Description = "heavyItem", Worth = 5, Weight = 500 };

        private GameContextStub CreateGameContext()
        {
            return new GameContextStub()
            {
                Player = new Player("", 1, 1, 10)
                {
                    Location = new Shop("shopDescription", "shop")
                    {
                        Store = new List<Item>()
                        {
                            weapon,
                            armor,
                            expensiveItem,
                            heavyItem
                        }
                    },
                    Gold = 100
                }
            };
        }

        [TestMethod]
        public void PurchaseItemIsAvailable_WhenInShopLocation_ReturnsTrue()
        {
            var context = new GameContextStub
            {
                Player = new Player("", 1, 1, 1) { Location = new Shop("shopDescription", "shop") }
            };
            var command = new PurchaseItemCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsTrue(available);
        }

        [TestMethod]
        public void PurchaseItemIsAvailable_WhenInCommonLocation_ReturnsFalse()
        {
            var context = new GameContextStub
            {
                Player = new Player("", 1, 1, 1) { Location = new Location("shopDescription", "shop") }
            };
            var command = new PurchaseItemCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsFalse(available);
        }

        [TestMethod]
        public void PurchaseItemExecute_WhenNoArgument_ShouldListShopItemsWithCosts()
        {
            var context = CreateGameContext();
            var command = new PurchaseItemCommand();

            var responce = command.Execute(context, null);

            StringAssert.Contains(responce, "There are some items you can purchase in store");
            StringAssert.Contains(responce, "0: " + weapon.Description + " (worth 10)");
            StringAssert.Contains(responce, "1: " + armor.Description + " (worth 20)");
            StringAssert.Contains(responce, "2: " + expensiveItem.Description + " (worth 500)");
            StringAssert.Contains(responce, "3: " + heavyItem.Description + " (worth 5)");
        }

        [TestMethod]
        public void PurchaseItemExecute_WhenArgumentIs0_ShouldPurchaseWeapon()
        {
            var context = CreateGameContext();
            var command = new PurchaseItemCommand();

            var responce = command.Execute(context, "0");

            StringAssert.Contains(responce, "You purchased " + weapon.Description);

            Assert.AreEqual(context.Player.Gold, 90);

            CollectionAssert.Contains(context.Player.Backpack, weapon);
            CollectionAssert.DoesNotContain(context.Player.Backpack, armor);
            CollectionAssert.DoesNotContain(context.Player.Backpack, expensiveItem);
            CollectionAssert.DoesNotContain(context.Player.Backpack, heavyItem);

            var shop = (Shop)context.Player.Location;
            CollectionAssert.DoesNotContain(shop.Store, weapon);
            CollectionAssert.Contains(shop.Store, armor);
            CollectionAssert.Contains(shop.Store, expensiveItem);
            CollectionAssert.Contains(shop.Store, heavyItem);
        }

        [TestMethod]
        public void PurchaseItemExecute_WhenArgumentIs1_ShouldPurchaseArmor()
        {
            var context = CreateGameContext();
            var command = new PurchaseItemCommand();

            var responce = command.Execute(context, "1");

            StringAssert.Contains(responce, "You purchased " + armor.Description);

            Assert.AreEqual(context.Player.Gold, 80);

            CollectionAssert.DoesNotContain(context.Player.Backpack, weapon);
            CollectionAssert.Contains(context.Player.Backpack, armor);
            CollectionAssert.DoesNotContain(context.Player.Backpack, expensiveItem);
            CollectionAssert.DoesNotContain(context.Player.Backpack, heavyItem);

            var shop = (Shop)context.Player.Location;
            CollectionAssert.Contains(shop.Store, weapon);
            CollectionAssert.DoesNotContain(shop.Store, armor);
            CollectionAssert.Contains(shop.Store, expensiveItem);
            CollectionAssert.Contains(shop.Store, heavyItem);
        }

        [TestMethod]
        public void PurchaseItemExecute_WhenHasNoMoney_ShouldReturnErrorMessage()
        {
            var context = CreateGameContext();
            var command = new PurchaseItemCommand();

            var responce = command.Execute(context, "2");

            StringAssert.Contains(responce, "You dont have enough gold to purchase this item");
            StringAssert.Contains(responce, "You have only 100 gold, but item's worht is 500 gold.");

            Assert.AreEqual(context.Player.Gold, 100);

            CollectionAssert.DoesNotContain(context.Player.Backpack, weapon);
            CollectionAssert.DoesNotContain(context.Player.Backpack, armor);
            CollectionAssert.DoesNotContain(context.Player.Backpack, expensiveItem);
            CollectionAssert.DoesNotContain(context.Player.Backpack, heavyItem);

            var shop = (Shop)context.Player.Location;
            CollectionAssert.Contains(shop.Store, weapon);
            CollectionAssert.Contains(shop.Store, armor);
            CollectionAssert.Contains(shop.Store, expensiveItem);
            CollectionAssert.Contains(shop.Store, heavyItem);
        }

        [TestMethod]
        public void PurchaseItemExecute_WhenCantCarryTooMuch_ShouldReturnErrorMessage()
        {
            var context = CreateGameContext();
            var command = new PurchaseItemCommand();

            var responce = command.Execute(context, "3");

            StringAssert.Contains(responce, "You can't carry this item, its too heavy.");
            StringAssert.Contains(responce, "Your item's total weight is 0, item's wweight is 500, and you can carry only 20");

            Assert.AreEqual(context.Player.Gold, 100);

            CollectionAssert.DoesNotContain(context.Player.Backpack, weapon);
            CollectionAssert.DoesNotContain(context.Player.Backpack, armor);
            CollectionAssert.DoesNotContain(context.Player.Backpack, expensiveItem);
            CollectionAssert.DoesNotContain(context.Player.Backpack, heavyItem);

            var shop = (Shop)context.Player.Location;
            CollectionAssert.Contains(shop.Store, weapon);
            CollectionAssert.Contains(shop.Store, armor);
            CollectionAssert.Contains(shop.Store, expensiveItem);
            CollectionAssert.Contains(shop.Store, heavyItem);
        }
    }
}
