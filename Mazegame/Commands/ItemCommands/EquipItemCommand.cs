using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;
using Mazegame.Entity;

namespace Mazegame.Commands.ItemCommands
{
    public class EquipItemCommand : ICommand
    {
        public string Name => "equipItem";
        public string Usage => "Usage: equipItem <itemNumber>\nEquip specified item from backpack";

        public string Execute(IGameContext context, string argument)
        {
            var backpack = context.Player.Backpack;

            if (string.IsNullOrWhiteSpace(argument))
            {
                return "You should specify item number to equip:\n" + backpack.GetItemsIndexedList();
            }

            int itemNumber;
            if (!int.TryParse(argument, out itemNumber))
            {
                return "You should specify correct item number to equip:\n" + backpack.GetItemsIndexedList();
            }

            if (itemNumber < 0 || itemNumber > backpack.Count) {
                return "You should specify correct item number to equip:\n" + backpack.GetItemsIndexedList();
            }

            var item = backpack[itemNumber];

            if (item.Equip(context.Player))
            {
                return "Now you are equipped with " + item.Description;
            }
            else
            {
                return "This item cannot be equipped";
            }
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return !context.IsInCombat();
        }
    }
}
