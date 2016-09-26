using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;

namespace Mazegame.Commands
{
    public class HelpCommand : ICommand
    {
        public string Name => "help";
        public string Usage => "Usage: help <commandName>\nHelp command provides information about all other commands";

        ICommandInformationProvider _informationProvider;

        public HelpCommand(ICommandInformationProvider informationProvider)
        {
            _informationProvider = informationProvider;
        }

        public bool IsAvailableInContext(IGameContext context) => true;

        public string Execute(IGameContext context, string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                return "Known commands list:\n" 
                       + string.Join(Environment.NewLine, _informationProvider.GetCommandsList())
                       + "\nUse help <commandName> to see command usage information";
            }
            else {
                if (!_informationProvider.GetCommandsList().Contains(argument))
                {
                    return "This command is unknown!";
                }
                return _informationProvider.GetCommandUsage(argument);
            }
        }
    }
}
