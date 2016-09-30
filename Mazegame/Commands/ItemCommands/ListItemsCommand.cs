using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;
using Mazegame.Entity;

namespace Mazegame.Commands.ItemCommands
{
    public class ListItemsCommand : ICommand
    {
        public string Name => "listItems";
        public string Usage => "Usage: listItems\nShows items list you have in your backpack";

        public string Execute(IGameContext context, string argument)
        {
            var sb = new StringBuilder();

            var player = context.Player;

            sb.AppendLine($"Your weight is {player.GetCurrentWeight()} / {player.MaxWeight}");

            var equippedItems = new Item[] {
                    player.Weapon,
                    player.Armor,
                    player.Shield
                }.Where(item => item != null)
                 .ToList();
            if (equippedItems.Any()) {
                sb.AppendLine("You are equipped with:");
                sb.AppendLine(equippedItems.GetItemsListWithWeights());
            } else
            {
                sb.AppendLine("You have no equipped items.");
            }

            if (context.Player.Backpack.Any())
            {
                sb.AppendLine("Here is your backpack content");
                sb.AppendLine(context.Player.Backpack.GetItemsListWithWeights());
            }
            else
            {
                sb.AppendLine("Your backpack is empty.");
            }
            return sb.ToString();
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return !context.IsInCombat();
        }
    }
}
