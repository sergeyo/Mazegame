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
    public class EquipItemTests
    {
        private Weapon weapon = new Weapon() { Description = "weapon" };
        private Weapon weapon2 = new Weapon() { Description = "weapon2" };
        private Armor armor = new Armor() { Description = "armor" };
        private Armor armor2 = new Armor() { Description = "armor2" };
        private Shield shield = new Shield() { Description = "shield" };
        private Shield shield2 = new Shield() { Description = "shield2" };
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
                        shield,
                        item
                    }
                }
            };
        }

        [TestMethod]
        public void EquipIsAvailable_WhenInCombat_ReturnsFalse()
        {
            var context = new GameContextStub() { IsInCombatMode = true };
            var command = new EquipItemCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsFalse(available);
        }

        [TestMethod]
        public void EquipIsAvailable_WhenNotInCombat_ReturnsTrue()
        {
            var context = new GameContextStub() { IsInCombatMode = false };
            var command = new EquipItemCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsTrue(available);
        }

        [TestMethod]
        public void EquipExecute_WhenNoArgumentsProvided_ShouldShowBackpackItems()
        {
            var context = CreateGameContext();
            var command = new EquipItemCommand();

            var response = command.Execute(context, null);

            Assert.IsNull(context.Player.Weapon);
            Assert.IsNull(context.Player.Armor);
            Assert.IsNull(context.Player.Shield);
            Assert.IsFalse(response.Contains("Now you are equipped with"));
            StringAssert.Contains(response, "You should specify item number to equip");
            StringAssert.Contains(response, "0: " + weapon.Description);
            StringAssert.Contains(response, "1: " + armor.Description);
            StringAssert.Contains(response, "2: " + shield.Description);
            StringAssert.Contains(response, "3: " + item.Description);
        }

        [TestMethod]
        public void EquipExecute_WhenNoArgumentsIsNAN_ShouldShowBackpackItems()
        {
            var context = CreateGameContext();
            var command = new EquipItemCommand();

            var response = command.Execute(context, "sadf");

            Assert.IsNull(context.Player.Weapon);
            Assert.IsNull(context.Player.Armor);
            Assert.IsNull(context.Player.Shield);
            Assert.IsFalse(response.Contains("Now you are equipped with"));
            StringAssert.Contains(response, "You should specify correct item number to equip");
            StringAssert.Contains(response, "0: " + weapon.Description);
            StringAssert.Contains(response, "1: " + armor.Description);
            StringAssert.Contains(response, "2: " + shield.Description);
            StringAssert.Contains(response, "3: " + item.Description);
        }

        [TestMethod]
        public void EquipExecute_WhenNoArgumentsIsOutOfBounds_ShouldShowBackpackItems()
        {
            var context = CreateGameContext();
            var command = new EquipItemCommand();

            var response = command.Execute(context, "999");

            Assert.IsNull(context.Player.Weapon);
            Assert.IsNull(context.Player.Armor);
            Assert.IsNull(context.Player.Shield);
            Assert.IsFalse(response.Contains("Now you are equipped with"));
            StringAssert.Contains(response, "You should specify correct item number to equip");
            StringAssert.Contains(response, "0: " + weapon.Description);
            StringAssert.Contains(response, "1: " + armor.Description);
            StringAssert.Contains(response, "2: " + shield.Description);
            StringAssert.Contains(response, "3: " + item.Description);
        }

        [TestMethod]
        public void EquipExecute_WhenNoArgumentsIs0AndHasNoWeapon_ShouldEquipWeapon()
        {
            var context = CreateGameContext();
            var command = new EquipItemCommand();

            var response = command.Execute(context, "0");

            Assert.AreEqual(weapon, context.Player.Weapon);
            Assert.IsNull(context.Player.Armor);
            Assert.IsNull(context.Player.Shield);
            CollectionAssert.DoesNotContain(context.Player.Backpack, weapon);
            Assert.AreEqual(3, context.Player.Backpack.Count);
            StringAssert.Contains(response, "Now you are equipped with " + weapon.Description);
        }

        [TestMethod]
        public void EquipExecute_WhenNoArgumentsIs0AndHasOtherWeapon_ShouldEquipWeaponAndReturnOldToBackpack()
        {
            var context = CreateGameContext();
            var command = new EquipItemCommand();

            context.Player.Weapon = weapon2;

            var response = command.Execute(context, "0");

            Assert.AreEqual(weapon, context.Player.Weapon);
            Assert.IsNull(context.Player.Armor);
            Assert.IsNull(context.Player.Shield);
            CollectionAssert.DoesNotContain(context.Player.Backpack, weapon);
            CollectionAssert.Contains(context.Player.Backpack, weapon2);
            Assert.AreEqual(4, context.Player.Backpack.Count);
            StringAssert.Contains(response, "Now you are equipped with " + weapon.Description);
        }

        [TestMethod]
        public void EquipExecute_WhenNoArgumentsIs1AndHasNoArmor_ShouldEquipWeapon()
        {
            var context = CreateGameContext();
            var command = new EquipItemCommand();

            var response = command.Execute(context, "1");

            Assert.IsNull(context.Player.Weapon);
            Assert.AreEqual(armor, context.Player.Armor);
            Assert.IsNull(context.Player.Shield);
            CollectionAssert.DoesNotContain(context.Player.Backpack, armor);
            Assert.AreEqual(3, context.Player.Backpack.Count);
            StringAssert.Contains(response, "Now you are equipped with " + armor.Description);
        }

        [TestMethod]
        public void EquipExecute_WhenNoArgumentsIs1AndHasOtherArmor_ShouldEquipWeaponAndReturnOldToBackpack()
        {
            var context = CreateGameContext();
            var command = new EquipItemCommand();

            context.Player.Armor = armor2;

            var response = command.Execute(context, "1");

            Assert.IsNull(context.Player.Weapon);
            Assert.AreEqual(armor, context.Player.Armor);
            Assert.IsNull(context.Player.Shield);
            CollectionAssert.DoesNotContain(context.Player.Backpack, armor);
            CollectionAssert.Contains(context.Player.Backpack, armor2);
            Assert.AreEqual(4, context.Player.Backpack.Count);
            StringAssert.Contains(response, "Now you are equipped with " + armor.Description);
        }

        [TestMethod]
        public void EquipExecute_WhenNoArgumentsIs2AndHasNoShield_ShouldEquipWeapon()
        {
            var context = CreateGameContext();
            var command = new EquipItemCommand();

            var response = command.Execute(context, "2");

            Assert.IsNull(context.Player.Weapon);
            Assert.IsNull(context.Player.Armor);
            Assert.AreEqual(shield, context.Player.Shield);
            CollectionAssert.DoesNotContain(context.Player.Backpack, shield);
            Assert.AreEqual(3, context.Player.Backpack.Count);
            StringAssert.Contains(response, "Now you are equipped with " + shield.Description);
        }

        [TestMethod]
        public void EquipExecute_WhenNoArgumentsIs2AndHasOtherShield_ShouldEquipWeaponAndReturnOldToBackpack()
        {
            var context = CreateGameContext();
            var command = new EquipItemCommand();

            context.Player.Shield = shield2;

            var response = command.Execute(context, "2");

            Assert.IsNull(context.Player.Weapon);
            Assert.IsNull(context.Player.Armor);
            Assert.AreEqual(shield, context.Player.Shield);
            CollectionAssert.DoesNotContain(context.Player.Backpack, shield);
            CollectionAssert.Contains(context.Player.Backpack, shield2);
            Assert.AreEqual(4, context.Player.Backpack.Count);
            StringAssert.Contains(response, "Now you are equipped with " + shield.Description);
        }

        [TestMethod]
        public void EquipExecute_WhenNoArgumentsPointsToUnwearableItem_ShouldReturnError()
        {
            var context = CreateGameContext();
            var command = new EquipItemCommand();
            
            var response = command.Execute(context, "3");

            Assert.IsNull(context.Player.Weapon);
            Assert.IsNull(context.Player.Armor);
            Assert.IsNull(context.Player.Shield);
            Assert.IsFalse(response.Contains("Now you are equipped with"));
            StringAssert.Contains(response, "This item cannot be equipped");
        }
    }
}
