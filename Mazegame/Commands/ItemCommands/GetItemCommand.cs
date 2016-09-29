using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;
using Mazegame.Entity;

namespace Mazegame.Commands.ItemCommands
{
    public class GetItemCommand : ICommand
    {
        public string Name => "getItem";
        public string Usage => "Usage: getItem <itemNumber>\nGet specified item from current location to backpack. Use without argument to see Location items list";

        public string Execute(IGameContext context, string argument)
        {
            var collectableItems = context.Player.Location.CollectableItems;

            if (!collectableItems.Any())
            {
                return "There is no items you can collect in this location.";
            }

            if (string.IsNullOrWhiteSpace(argument))
            {
                return "You can see some items you can collect in this location:\n"
                     + collectableItems.GetItemsIndexedListWithWeights();
            }

            int itemNumber;
            if (!int.TryParse(argument, out itemNumber))
            {
                return "You should specify correct item number to collect:\n" 
                    + collectableItems.GetItemsIndexedListWithWeights();
            }

            if (itemNumber < 0 || itemNumber > collectableItems.Count - 1)
            {
                return "You should specify correct item number to collect:\n" 
                    + collectableItems.GetItemsIndexedListWithWeights();
            }

            var item = collectableItems[itemNumber];

            if (context.Player.GetCurrentWeight() + item.Weight > context.Player.MaxWeight)
            {
                return "You cant collect this item, it's too heavy.";
            }

            collectableItems.Remove(item);
            context.Player.AddItemToBackpack(item);

            return "You picked up " + item.Description;
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return !context.IsInCombat();
        }
    }
}
