using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;

namespace Mazegame.Commands
{
    public class RunawayCommand : ICommand
    {
        public string Name => "runaway";
        public string Usage => "Usage: runaway\nFlee from hostile creature to previous location. This command is available only in battle, wich occurs when you enters to the location with hostile creature.";

        public string Execute(IGameContext context, string argument)
        {
            context.Player.Location = context.Player.PreviousLocation;
            context.Player.PreviousLocation = null;

            return "You returned to previous location.\n" + context.Player.Location.GetLongDescription();
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return context.IsInCombat();
        }
    }
}
