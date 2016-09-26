using Mazegame.Commands;
using Mazegame.Control;
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
    public class GotoCommandTests
    {
        private Location startLocation;
        private Location location1;
        private Location location2;

        private GameContextStub GetContextWith3Locations()
        {
            startLocation = new Location("startLoaction", "start");
            location1 = new Location("location1description", "loc1");
            location2 = new Location("location2description", "loc2");
            startLocation.AddExit("exit1", new Exit("exit1description", location1));
            startLocation.AddExit("exit2", new Exit("exit2description", location2));
            location1.AddExit("location1_exit1", new Exit("location1_exit1description", startLocation));

            return new GameContextStub() { Player = new Player() { Location = startLocation } };
        }

        [TestMethod]
        public void GotpCommandIsAvailable_WhenNotInCombatMode_ShouldReturnTrue()
        {
            var context = new GameContextStub() { IsInCombatMode = false };
            var command = new GotoCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsTrue(available);
        }

        [TestMethod]
        public void GotpCommandIsAvailable_WhenIsInCombatMode_ShouldReturnFalse()
        {
            var context = new GameContextStub() { IsInCombatMode = true };
            var command = new GotoCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsFalse(available);
        }

        [TestMethod]
        public void GotpCommandExecute_WhenNoArgumentProvided_ShouldReturnExitsList()
        {
            var context = GetContextWith3Locations();
            var command = new GotoCommand();

            var response = command.Execute(context, null);

            Assert.AreEqual(startLocation, context.Player.Location);
            StringAssert.Contains(response, "You should point an exit to go");
            StringAssert.Contains(response, "exit1");
            StringAssert.Contains(response, "exit1description");
            StringAssert.Contains(response, "exit2");
            StringAssert.Contains(response, "exit2description");
        }

        [TestMethod]
        public void GotpCommandExecute_WhenArgumentIsUnknownExit_ShouldReturnsExitsList()
        {
            var context = GetContextWith3Locations();
            var command = new GotoCommand();

            var response = command.Execute(context, "unknownExit");

            Assert.AreEqual(startLocation, context.Player.Location);
            StringAssert.Contains(response, "unknown exit");
            StringAssert.Contains(response, "exit1");
            StringAssert.Contains(response, "exit1description");
            StringAssert.Contains(response, "exit2");
            StringAssert.Contains(response, "exit2description");

        }

        [TestMethod]
        public void GotpCommandExecute_WhenArgumentIsExit1_ShouldChangeLocationToLocation1_AndShowItsDescription()
        {
            var context = GetContextWith3Locations();
            var command = new GotoCommand();

            var response = command.Execute(context, "exit1");

            Assert.AreEqual(location1, context.Player.Location);
            Assert.AreEqual(startLocation, context.Player.PreviousLocation);
            StringAssert.Contains(response, location1.Description);
            StringAssert.Contains(response, "location1_exit1description");
        }

        [TestMethod]
        public void GotpCommandExecute_WhenArgumentIsExit2_ShouldChangeLocationToLocation2_AndShowItsDescription()
        {
            var context = GetContextWith3Locations();
            var command = new GotoCommand();

            var response = command.Execute(context, "exit2");

            Assert.AreEqual(location2, context.Player.Location);
            Assert.AreEqual(startLocation, context.Player.PreviousLocation);
            StringAssert.Contains(response, location2.Description);
        }

        [TestMethod]
        public void GotpCommandExecute_WhenArgumentIsExit1AndEnteredCombatMode_ShouldChangeLocationToLocation2_AndShowEnemyName()
        {
            var context = GetContextWith3Locations();
            context.IsInCombatMode = true;
            context.CurrentLocationEnemy = new NonPlayerCharacter() { Name = "enemy1" };

            var command = new GotoCommand();

            var response = command.Execute(context, "exit1");

            Assert.AreEqual(location1, context.Player.Location);
            Assert.AreEqual(startLocation, context.Player.PreviousLocation);
            Assert.IsFalse(response.Contains("location1_exit1description"));
            StringAssert.Contains(response, "You have spotted an enemy");
            StringAssert.Contains(response, context.CurrentLocationEnemy.Name);
        }
    }
}
