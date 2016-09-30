using Mazegame.Commands;
using Mazegame.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MazegameTest
{
    [TestClass]
    public class GroupCommandTests
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
        public void GroupIsAvailable_WhenNoNPCNoPartyAndInCombat_ShouldReturnFalse()
        {
            var context = CreateGameContext(false, false, true);
            var command = new GroupCommand();

            var isAvailable = command.IsAvailableInContext(context);

            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public void GroupIsAvailable_WhenHasNPCNoPartyAndInCombat_ShouldReturnFalse()
        {
            var context = CreateGameContext(true, false, true);
            var command = new GroupCommand();

            var isAvailable = command.IsAvailableInContext(context);

            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public void GroupIsAvailable_WhenNoNPCHasPartyAndInCombat_ShouldReturnFalse()
        {
            var context = CreateGameContext(false, true, true);
            var command = new GroupCommand();

            var isAvailable = command.IsAvailableInContext(context);

            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public void GroupIsAvailable_WhenNoNPCNoParty_ShouldReturnFalse()
        {
            var context = CreateGameContext(false, false);
            var command = new GroupCommand();

            var isAvailable = command.IsAvailableInContext(context);

            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public void GroupIsAvailable_WhenHasNPCNoParty_ShouldReturnFalse()
        {
            var context = CreateGameContext(true, false);
            var command = new GroupCommand();

            var isAvailable = command.IsAvailableInContext(context);

            Assert.IsTrue(isAvailable);
        }

        [TestMethod]
        public void GroupIsAvailable_WhenNoNPCHasParty_ShouldReturnFalse()
        {
            var context = CreateGameContext(false, true);
            var command = new GroupCommand();

            var isAvailable = command.IsAvailableInContext(context);

            Assert.IsFalse(isAvailable);
        }

        [TestMethod]
        public void GroupIsAvailable_WhenHasNPCHasParty_ShouldReturnFalse()
        {
            var context = CreateGameContext(true, true);
            var command = new GroupCommand();

            var isAvailable = command.IsAvailableInContext(context);

            Assert.IsTrue(isAvailable);
        }

        [TestMethod]
        public void GroupExecute_WhenHasNPCHasParty_ShouldReturnError()
        {
            var context = CreateGameContext(true, true);
            var command = new GroupCommand();

            var response = command.Execute(context, null);

            StringAssert.Contains(response, "you already have a company");
            Assert.AreEqual(party, context.Player.Party.Character);
            Assert.AreEqual(npc, context.Player.Location.NPC);
        }

        [TestMethod]
        public void GroupExecute_WhenHasNPCNoParty_ShouldMovePartyToLocation()
        {
            var context = CreateGameContext(true, false);
            var command = new GroupCommand();

            var response = command.Execute(context, null);

            StringAssert.Contains(response, "NPC (AC: 1, Life points: 1) has joined you");
            Assert.AreEqual(npc, context.Player.Party.Character);
            Assert.AreEqual(null, context.Player.Location.NPC);
        }
    }
}
