using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;
using Mazegame.Entity;

namespace Mazegame.Commands.ItemCommands
{
    public class SellItemCommand :ICommand
    {
        public string Name => "sellItem";
        public string Usage => "Usage: sellItem <itemNumber>\nSell item from your backpack. Works only if you are in Shop location.";

        public string Execute(IGameContext context, string argument)
        {
            var backpack = context.Player.Backpack;

            if (!backpack.Any())
            {
                return "There is no items you can sell.";
            }

            if (string.IsNullOrWhiteSpace(argument))
            {
                return "Chooze some item to sell from your backpack:\n"
                     + backpack.GetItemsIndexedListWithWorths();
            }

            int itemNumber;
            if (!int.TryParse(argument, out itemNumber))
            {
                return "You should specify correct item number to sell:\n"
                    + backpack.GetItemsIndexedListWithWorths();
            }

            if (itemNumber < 0 || itemNumber > backpack.Count - 1)
            {
                return "You should specify correct item number to sell:\n"
                    + backpack.GetItemsIndexedListWithWorths();
            }

            var item = backpack[itemNumber];

            backpack.Remove(item);
            ((Shop)context.Player.Location).Store.Add(item);

            context.Player.Gold += item.Worth;

            return "You sold " + item.Description;
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return context.Player.Location is Shop;
        }
    }
}
