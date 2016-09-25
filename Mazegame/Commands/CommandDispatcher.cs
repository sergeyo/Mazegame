using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazegame.Commands
{
    public class CommandDispatcher : ICommandInformationProvider
    {
        private Dictionary<string, ICommand> commands;

        public CommandDispatcher()
        {
            var availableCommands = new ICommand[] {
                new GotoCommand(),
                new HelpCommand(this)
            };
        }

        public IEnumerable<string> GetCommandsList()
        {
            return commands.Keys;
        }

        public string GetCommandUsage(string command)
        {
            return commands[command].Usage;
        }
    }
}
