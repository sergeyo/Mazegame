using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;

namespace Mazegame.Commands
{
    public class GotoCommand : ICommand
    {
        public string Usage
        {
            get
            {
                return "Usage: goto <exit>\nGo to another location through specified exit";
            }
        }

        public string Name { get { return "goto"; } }

        public string Execute(IGameContext context, string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                return Usage;
            }
            return "";
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return !context.IsInCombat();
        }
    }
}
