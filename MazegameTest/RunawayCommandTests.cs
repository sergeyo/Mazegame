using Mazegame.Commands;
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
    public class RunawayCommandTests
    {
        [TestMethod]
        public void RunawayIsAvailable_WhenInCombat_ReturnsTrue()
        {
            var context = new GameContextStub() { IsInCombatMode = true };
            var command = new RunawayCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsTrue(available);
        }

        [TestMethod]
        public void RunawayIsAvailable_WhenNotInCombat_ReturnsFalse()
        {
            var context = new GameContextStub() { IsInCombatMode = false };
            var command = new RunawayCommand();

            var available = command.IsAvailableInContext(context);

            Assert.IsFalse(available);
        }

        [TestMethod]
        public void RunawayExecute_WhenExecuted_ShouldReturnPlayerToPreviousLocation()
        {
            var currentLocation = new Location("", "");
            var previousLocation = new Location("", "");

            var context = new GameContextStub()
            {
                Player = new Player("", 1, 1, 1)
                {
                    Location = currentLocation,
                    PreviousLocation = previousLocation
                }
            };
            var command = new RunawayCommand();

            var response = command.Execute(context, null);

            Assert.AreEqual(previousLocation, context.Player.Location);
            StringAssert.Contains(response, context.Player.Location.GetLongDescription());
        }
    }
}
