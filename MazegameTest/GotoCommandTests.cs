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
        private IGameContext GetContextWith3Locations()
        {
            var startLocation = new Location("startLoaction", "start");
            var location1 = new Location("location1description", "loc1");
            var location2 = new Location("location2description", "loc2");
            startLocation.AddExit("exit1", new Exit("exit1description", location1));
            startLocation.AddExit("exit2", new Exit("exit2description", location2));

            return new GameContextStub() { CurrentLocation = startLocation, Player = new Player() { Location = startLocation } };
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
            
        }

        [TestMethod]
        public void GotpCommandExecute_WhenArgumentIsExit1_ShouldChangeLocationToLocation1_AndShowItsDescription()
        {

        }

        [TestMethod]
        public void GotpCommandExecute_WhenArgumentIsExit2_ShouldChangeLocationToLocation2_AndShowItsDescription()
        {

        }

        [TestMethod]
        public void GotpCommandExecute_WhenArgumentIsUnknownExit_ShouldReturnsExitsList()
        {

        }
    }
}
