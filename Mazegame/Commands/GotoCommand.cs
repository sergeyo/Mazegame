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
                return "You should point an exit to go, choose one of the exits from this location:\n" 
                    + context.Player.Location.GetExitsList();
            }
            if (!context.Player.Location.Exits.ContainsKey(argument))
            {
                return "You pointed an unknown exit, choose one of the exits from this location:\n"
                    + context.Player.Location.GetExitsList();
            }

            var exit = context.Player.Location.Exits[argument];
            context.Player.PreviousLocation = context.Player.Location;
            context.Player.Location = exit.Destination;

            if (context.IsInCombat())
            {
                var enemy = context.CurrentLocationEnemy;
                return "You now entered next location\n"
                    + context.Player.Location.Label + ": " + context.Player.Location.Description
                    + "\nYou have spotted an enemy: " 
                    + $"{enemy.Name} (Dmg: {enemy.Weapon.Dice.ToString()}+{enemy.Strength}, AC: {enemy.AC}, Life Points: {enemy.LifePoints}" 
                    + "\n! You can fight or runaway!";
            }

            return "You now entered next location\n" + context.Player.Location.GetLongDescription();
        }
               
        public bool IsAvailableInContext(IGameContext context)
        {
            return !context.IsInCombat();
        }
    }
}
