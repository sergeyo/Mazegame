using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;

namespace Mazegame.Commands.ItemCommands
{
    public class ListItemsCommand : ICommand
    {
        public string Name => "listItems";
        public string Usage => "Usage: listItems\nShows items list you have in your backpack";

        public string Execute(IGameContext context, string argument)
        {
            if (context.Player.Backpack.Any())
            {
                return string.Format("Here is your backpack content (weight: {0}/{1}):\n{2}",
                    context.Player.Backpack.Sum(item => item.Weight),
                    context.Player.MaxWeight,
                    string.Join("\n", context.Player.Backpack.Select(item => $"{item.Description} (weight {item.Weight})" ))
                );
            }
            else
            {
                return "Your backpack is empty.";
            }
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return !context.IsInCombat();
        }
    }
}
