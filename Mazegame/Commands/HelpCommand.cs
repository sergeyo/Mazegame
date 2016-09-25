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
        ICommandInformationProvider _informationProvider;

        public HelpCommand(ICommandInformationProvider informationProvider)
        {
            _informationProvider = informationProvider;
        }

        public string Name => "help";
        public string Usage => "Help command provides information about all other commands";

        public bool IsAvailableInContext(IGameContext context) => true;

        public string Execute(IGameContext context, string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                return "Known commands list:\n" + string.Join(Environment.NewLine, _informationProvider.GetCommandsList());
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
