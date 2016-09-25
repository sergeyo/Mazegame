using Mazegame.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazegameTest
{
    [TestClass]
    public class HelpCommandTests
    {
        class CommandInformationProviderStub : ICommandInformationProvider
        {
            public IEnumerable<string> GetCommandsList()
            {
                return new[] { "command1", "command2" };
            }

            public string GetCommandUsage(string command)
            {
                switch (command)
                {
                    case "command1": return "command1 usage";
                    case "command2": return "command2 usage";
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        private HelpCommand GetCommand()
        {
            return new HelpCommand(new CommandInformationProviderStub());
        }

        [TestMethod]
        public void HelpCommandIsAvailableInContext_Always_ReturnsTrue()
        {
            var command = GetCommand();

            var available = command.IsAvailableInContext(null);

            Assert.IsTrue(available);
        }

        [TestMethod]
        public void HelpCommandExecute_WhenNoArgumentProvided_ShouldReturnCommandsList()
        {
            var command = GetCommand();

            var responce = command.Execute(null, null);

            StringAssert.Contains(responce, "command1");
            StringAssert.Contains(responce, "command2");
        }

        [TestMethod]
        public void HelpCommandExecute_WhenArgumentIsCommand1_ShouldReturnCommand1Usage()
        {
            var command = GetCommand();

            var responce = command.Execute(null, "command1");

            StringAssert.Contains(responce, "command1 usage");
        }

        [TestMethod]
        public void HelpCommandExecute_WhenArgumentIsCommand2_ShouldReturnCommand2Usage()
        {
            var command = GetCommand();

            var responce = command.Execute(null, "command2");

            StringAssert.Contains(responce, "command2 usage");
        }

        [TestMethod]
        public void HelpCommandExecute_WhenArgumentIsUnknownCommand_ShouldReturnError()
        {
            var command = GetCommand();

            var responce = command.Execute(null, "unknownCommand");

            StringAssert.Contains(responce, "unknown");
        }
    }
}
