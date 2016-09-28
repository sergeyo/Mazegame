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
    public class GetItemCommandTests
    {
        private Weapon weapon = new Weapon() { Description = "weapon", Weight = 7 };
        private Armor armor = new Armor() { Description = "armor", Weight = 7 };
        private Shield shield = new Shield() { Description = "shield", Weight = 7 };
        private NonWearableItem heavyItem = new NonWearableItem() { Description = "item", Weight = 19 };

        private GameContextStub CreateGameContext(params Item[] items)
        {
            return new GameContextStub()
            {
                Player = new Player("", 1, 1, 10)
                {
                    Location = new Location("location1description", "loc1")
                    {
                        CollectableItems = items.ToList()
                    }
                }
            };
        }

        [TestMethod]
        public void GetItemIsAvailable_WhenNotInCombatMode_ShouldReturnTrue()
        {
            var context = new GameContextStub() { IsInCombatMode = false };
            var command = new GetItemCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsTrue(available);
        }

        [TestMethod]
        public void GetItemIsAvailable_WhenInCombatMode_ShouldReturnFalse()
        {
            var context = new GameContextStub() { IsInCombatMode = true };
            var command = new GetItemCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsFalse(available);
        }

        [TestMethod]
        public void GetItemExecute_WhenNoArgumentProvidedAndHasNoCollectableItems_ShouldReturnErrorMessage()
        {
            var context = CreateGameContext();
            var command = new GetItemCommand();

            var responce = command.Execute(context, null);

            CollectionAssert.DoesNotContain(context.Player.Backpack, weapon);
            CollectionAssert.DoesNotContain(context.Player.Backpack, armor);
            CollectionAssert.DoesNotContain(context.Player.Backpack, shield);
            StringAssert.Contains(responce, "There is no items you can collect");
        }

        [TestMethod]
        public void GetItemExecute_WhenNoArgumentProvidedAndHasCollectableItems_ShouldReturnListOfCollectableItems()
        {
            var context = CreateGameContext(weapon, armor);
            var command = new GetItemCommand();

            var responce = command.Execute(context, null);

            CollectionAssert.Contains(context.Player.Location.CollectableItems, weapon);
            CollectionAssert.Contains(context.Player.Location.CollectableItems, armor);
            CollectionAssert.DoesNotContain(context.Player.Location.CollectableItems, shield);

            CollectionAssert.DoesNotContain(context.Player.Backpack, weapon);
            CollectionAssert.DoesNotContain(context.Player.Backpack, armor);
            CollectionAssert.DoesNotContain(context.Player.Backpack, shield);

            StringAssert.Contains(responce, "0: " + weapon.Description);
            StringAssert.Contains(responce, "1: " + armor.Description);
        }

        [TestMethod]
        public void GetItemExecute_WhenArgumentIs0_ShouldCollectWeapon()
        {
            var context = CreateGameContext(weapon, armor);
            var command = new GetItemCommand();

            var responce = command.Execute(context, "0");

            CollectionAssert.DoesNotContain(context.Player.Location.CollectableItems, weapon);
            CollectionAssert.Contains(context.Player.Location.CollectableItems, armor);
            CollectionAssert.DoesNotContain(context.Player.Location.CollectableItems, shield);

            CollectionAssert.Contains(context.Player.Backpack, weapon);
            CollectionAssert.DoesNotContain(context.Player.Backpack, armor);
            CollectionAssert.DoesNotContain(context.Player.Backpack, shield);

            StringAssert.Contains(responce, "You picked up");
            StringAssert.Contains(responce, weapon.Description);
        }

        [TestMethod]
        public void GetItemExecute_WhenArgumentIs1_ShouldCollectArmor()
        {
            var context = CreateGameContext(weapon, armor);
            var command = new GetItemCommand();

            var responce = command.Execute(context, "1");

            CollectionAssert.Contains(context.Player.Location.CollectableItems, weapon);
            CollectionAssert.DoesNotContain(context.Player.Location.CollectableItems, armor);
            CollectionAssert.DoesNotContain(context.Player.Location.CollectableItems, shield);

            CollectionAssert.DoesNotContain(context.Player.Backpack, weapon);
            CollectionAssert.Contains(context.Player.Backpack, armor);
            CollectionAssert.DoesNotContain(context.Player.Backpack, shield);

            StringAssert.Contains(responce, "You picked up");
            StringAssert.Contains(responce, armor.Description);
        }

        [TestMethod]
        public void GetItemExecute_WhenArgumentIsIncorrect_ShouldReturnErrorMessage()
        {
            var context = CreateGameContext(weapon, armor);
            var command = new GetItemCommand();

            var responce = command.Execute(context, "999");

            CollectionAssert.Contains(context.Player.Location.CollectableItems, weapon);
            CollectionAssert.Contains(context.Player.Location.CollectableItems, armor);
            CollectionAssert.DoesNotContain(context.Player.Location.CollectableItems, shield);

            CollectionAssert.DoesNotContain(context.Player.Backpack, weapon);
            CollectionAssert.DoesNotContain(context.Player.Backpack, armor);
            CollectionAssert.DoesNotContain(context.Player.Backpack, shield);

            StringAssert.Contains(responce, "You should specify correct item number");
        }

        [TestMethod]
        public void GetItemExecute_WhenArgumentIsCorrectButIsTooHeavy_ShouldReturnErrorMessage()
        {
            var context = CreateGameContext(weapon, armor);
            var command = new GetItemCommand();

            context.Player.AddItemToBackpack(heavyItem);

            var responce = command.Execute(context, "0");

            CollectionAssert.Contains(context.Player.Location.CollectableItems, weapon);
            CollectionAssert.Contains(context.Player.Location.CollectableItems, armor);
            CollectionAssert.DoesNotContain(context.Player.Location.CollectableItems, shield);

            CollectionAssert.DoesNotContain(context.Player.Backpack, weapon);
            CollectionAssert.DoesNotContain(context.Player.Backpack, armor);
            CollectionAssert.DoesNotContain(context.Player.Backpack, shield);

            StringAssert.Contains(responce, "too heavy");
        }
    }
}
