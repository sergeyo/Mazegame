using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;
using Mazegame.Entity;

namespace Mazegame.Commands.ItemCommands
{
    public class DropItemCommand : ICommand
    {
        public string Name => "dropItem";
        public string Usage => "Usage: dropItem <itemNumber>\nDrop specified item from backpack to current location. Use without argument to see backpack items list";

        public string Execute(IGameContext context, string argument)
        {
            var backpack = context.Player.Backpack;

            if (!backpack.Any())
            {
                return "There is no items you can drop from your backpack.";
            }

            if (string.IsNullOrWhiteSpace(argument))
            {
                return "You can drop something from your backpack:\n"
                     + backpack.GetItemsIndexedList();
            }

            int itemNumber;
            if (!int.TryParse(argument, out itemNumber))
            {
                return "You should specify correct item number to drop:\n"
                    + backpack.GetItemsIndexedList();
            }

            if (itemNumber < 0 || itemNumber > backpack.Count)
            {
                return "You should specify correct item number to drop:\n"
                    + backpack.GetItemsIndexedList();
            }

            var item = backpack[itemNumber];

            backpack.Remove(item);
            context.Player.Location.CollectableItems.Add(item);

            return "You dropped " + item.Description;
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return !context.IsInCombat();
        }
    }
}
