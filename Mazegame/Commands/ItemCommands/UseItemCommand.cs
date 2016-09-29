using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;
using Mazegame.Entity;

namespace Mazegame.Commands.ItemCommands
{
    public class UseItemCommand : ICommand
    {
        public string Name => "useItem";
        public string Usage => "Usage: useItem <itemNumber>\nUse specified item from backpack to current location. This can be some usable thing like Health Potion";

        public string Execute(IGameContext context, string argument)
        {
            var backpack = context.Player.Backpack;

            if (!backpack.Any())
            {
                return "There is no items you can use from your backpack.";
            }

            if (string.IsNullOrWhiteSpace(argument))
            {
                return "You can use something from your backpack:\n"
                     + backpack.GetItemsIndexedList();
            }

            int itemNumber;
            if (!int.TryParse(argument, out itemNumber))
            {
                return "You should specify correct item number to use:\n"
                    + backpack.GetItemsIndexedList();
            }

            if (itemNumber < 0 || itemNumber > backpack.Count - 1)
            {
                return "You should specify correct item number to use:\n"
                    + backpack.GetItemsIndexedList();
            }

            return backpack[itemNumber].UseItem(context.Player);
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return !context.IsInCombat();
        }
    }
}
