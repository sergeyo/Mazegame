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
    public class UnequipItemCommandTests
    {
        private Weapon weapon = new Weapon() { Description = "weapon" };
        private Armor armor = new Armor() { Description = "armor" };
        private Shield shield = new Shield() { Description = "shield" };

        private GameContextStub CreateGameContext()
        {
            return new GameContextStub()
            {
                Player = new Player("", 1, 1, 1)
            };
        }

        [TestMethod]
        public void UnequipIsAvailable_WhenInCombat_ReturnsFalse()
        {
            var context = new GameContextStub() { IsInCombatMode = true };
            var command = new UnequipItemCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsFalse(available);
        }

        [TestMethod]
        public void UnequipIsAvailable_WhenNotInCombat_ReturnsTrue()
        {
            var context = new GameContextStub() { IsInCombatMode = false };
            var command = new UnequipItemCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsTrue(available);
        }

        [TestMethod]
        public void UnequipExecute_WhenArgumentIsWeaponAndHasWeapon_ShouldMoveWeaponToBackpack()
        {
            var context = CreateGameContext();
            var command = new UnequipItemCommand();

            context.Player.Weapon = weapon;

            var response = command.Execute(context, "weapon");

            Assert.IsNull(context.Player.Weapon);
            CollectionAssert.Contains(context.Player.Backpack, weapon);
            Assert.AreEqual(1, context.Player.Backpack.Count);
            StringAssert.Contains(response, "You have unequiped weapon");
        }

        [TestMethod]
        public void UnequipExecute_WhenArgumentIsWeaponAndHasNoWeapon_ShouldPrintError()
        {
            var context = CreateGameContext();
            var command = new UnequipItemCommand();

            context.Player.Weapon = null;

            var response = command.Execute(context, "weapon");

            Assert.IsNull(context.Player.Weapon);
            CollectionAssert.DoesNotContain(context.Player.Backpack, weapon);
            Assert.AreEqual(0, context.Player.Backpack.Count);
            StringAssert.Contains(response, "You have no weapon equipped.");
        }

        [TestMethod]
        public void UnequipExecute_WhenArgumentIsArmorAndHasArmor_ShouldMoveArmorToBackpack()
        {
            var context = CreateGameContext();
            var command = new UnequipItemCommand();

            context.Player.Armor = armor;

            var response = command.Execute(context, "armor");

            Assert.IsNull(context.Player.Armor);
            CollectionAssert.Contains(context.Player.Backpack, armor);
            Assert.AreEqual(1, context.Player.Backpack.Count);
            StringAssert.Contains(response, "You have unequiped armor");
        }

        [TestMethod]
        public void UnequipExecute_WhenArgumentIsArmorAndHasNoArmor_ShouldPrintError()
        {
            var context = CreateGameContext();
            var command = new UnequipItemCommand();

            context.Player.Armor = null;

            var response = command.Execute(context, "armor");

            Assert.IsNull(context.Player.Armor);
            CollectionAssert.DoesNotContain(context.Player.Backpack, armor);
            Assert.AreEqual(0, context.Player.Backpack.Count);
            StringAssert.Contains(response, "You have no armor equipped.");
        }

        [TestMethod]
        public void UnequipExecute_WhenArgumentIsShieldAndHasShield_ShouldMoveShieldToBackpack()
        {
            var context = CreateGameContext();
            var command = new UnequipItemCommand();

            context.Player.Shield = shield;

            var response = command.Execute(context, "shield");

            Assert.IsNull(context.Player.Shield);
            CollectionAssert.Contains(context.Player.Backpack, shield);
            Assert.AreEqual(1, context.Player.Backpack.Count);
            StringAssert.Contains(response, "You have unequiped shield");
        }

        [TestMethod]
        public void UnequipExecute_WhenArgumentIsShieldAndHasNoShield_ShouldPrintError()
        {
            var context = CreateGameContext();
            var command = new UnequipItemCommand();

            context.Player.Shield = null;

            var response = command.Execute(context, "shield");

            Assert.IsNull(context.Player.Shield);
            CollectionAssert.DoesNotContain(context.Player.Backpack, shield);
            Assert.AreEqual(0, context.Player.Backpack.Count);
            StringAssert.Contains(response, "You have no shield equipped.");
        }

        [TestMethod]
        public void UnequipExecute_WhenArgumentIsWrong_ShouldReturnError()
        {
            var context = CreateGameContext();
            var command = new UnequipItemCommand();

            var response = command.Execute(context, "googles");

            Assert.AreEqual(0, context.Player.Backpack.Count);
            StringAssert.Contains(response, "You should specify what item do you want to unequip: weapon, armor or shield");
        }
    }
}
