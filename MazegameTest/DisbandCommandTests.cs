using Mazegame.Commands;
using Mazegame.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MazegameTest
{
    [TestClass]
    public class DisbandCommandTests
    {
        private NonPlayerCharacter npc = new NonPlayerCharacter("NPC", 1, 1, 1, false);
        private NonPlayerCharacter party = new NonPlayerCharacter("Party", 1, 1, 1, false);

        private GameContextStub CreateGameContext(bool hasNpcInLocation, bool hasParty, bool inCombat = false)
        {
            var context = new GameContextStub()
            {
                IsInCombatMode = inCombat,
                Player = new Player("Player", 1, 1, 1)
                {
                    Location = new Location("", "")
                }
            };
            if (hasNpcInLocation)
            {
                context.Player.Location.NPC = npc;
            }
            if (hasParty)
            {
                context.Player.Party.Character = party;
            }
            return context;
        }

        [TestMethod]
        public void DisbandIsAvailable_WhenNoNPCNoPartyAndInCombat_ShouldReturnFalse()
        {
            var context = CreateGameContext(false, false, true);
            var command = new DisbandCommand();

            var isAvailable = command.IsAvailableInContext(context);

            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public void DisbandIsAvailable_WhenHasNPCNoPartyAndInCombat_ShouldReturnFalse()
        {
            var context = CreateGameContext(true, false, true);
            var command = new DisbandCommand();

            var isAvailable = command.IsAvailableInContext(context);

            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public void DisbandIsAvailable_WhenNoNPCHasPartyAndInCombat_ShouldReturnFalse()
        {
            var context = CreateGameContext(false, true, true);
            var command = new DisbandCommand();

            var isAvailable = command.IsAvailableInContext(context);

            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public void DisbandIsAvailable_WhenNoNPCNoParty_ShouldReturnFalse()
        {
            var context = CreateGameContext(false, false);
            var command = new DisbandCommand();

            var isAvailable = command.IsAvailableInContext(context);

            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public void DisbandIsAvailable_WhenHasNPCNoParty_ShouldReturnFalse()
        {
            var context = CreateGameContext(true, false);
            var command = new DisbandCommand();

            var isAvailable = command.IsAvailableInContext(context);

            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public void DisbandIsAvailable_WhenNoNPCHasParty_ShouldReturnFalse()
        {
            var context = CreateGameContext(false, true);
            var command = new DisbandCommand();

            var isAvailable = command.IsAvailableInContext(context);

            Assert.IsTrue(isAvailable);
        }

        [TestMethod]
        public void DisbandIsAvailable_WhenHasNPCHasParty_ShouldReturnFalse()
        {
            var context = CreateGameContext(true, true);
            var command = new DisbandCommand();

            var isAvailable = command.IsAvailableInContext(context);

            Assert.IsTrue(isAvailable);
        }

        [TestMethod]
        public void DisbandExecute_WhenHasNPCHasParty_ShouldReturnError()
        {
            var context = CreateGameContext(true, true);
            var command = new DisbandCommand();

            var response = command.Execute(context, null);

            StringAssert.Contains(response, "Party can't leave you right now, he won't stay here with NPC");
            Assert.AreEqual(party, context.Player.Party.Character);
            Assert.AreEqual(npc, context.Player.Location.NPC);
        }

        [TestMethod]
        public void DisbandExecute_WhenHasNPCNoParty_ShouldMovePartyToLocation()
        {
            var context = CreateGameContext(false, true);
            var command = new DisbandCommand();

            var response = command.Execute(context, null);

            StringAssert.Contains(response, "Party leaves you and stays in this location.");
            Assert.AreEqual(null, context.Player.Party.Character);
            Assert.AreEqual(party, context.Player.Location.NPC);
        }
    }
}
