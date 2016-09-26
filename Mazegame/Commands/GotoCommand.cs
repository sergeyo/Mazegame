using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;
using Mazegame.Entity;

namespace Mazegame.Commands
{
    public class GotoCommand : ICommand
    {
        public string Name => "goto";
        public string Usage => "Usage: goto <exit>\nGo to another location through specified exit";

        public string Execute(IGameContext context, string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                return "You should point an exit to go, choose one of the exits from this location:\n" + GetExitsList(context.Player.Location);
            }
            if (!context.Player.Location.Exits.ContainsKey(argument))
            {
                return "You pointed an unknown exit, choose one of the exits from this location:\n" + GetExitsList(context.Player.Location);
            }

            var exit = context.Player.Location.Exits[argument];
            context.Player.PreviousLocation = context.Player.Location;
            context.Player.Location = exit.Destination;

            return "You now entered next location\n"
                + context.Player.Location.Label + ": " + context.Player.Location.Description + "\n"
                + (context.IsInCombat() 
                  ? "You have spotted an enemy: " + context.CurrentLocationEnemy.Name + "! You can fight or runaway!"
                  : GetExitsList(context.Player.Location));
        }

        private string GetExitsList(Location location)
        {
            return string.Join("\n", location.Exits.Select(kv => kv.Key + ": " + kv.Value.Description));
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return !context.IsInCombat();
        }
    }
}
