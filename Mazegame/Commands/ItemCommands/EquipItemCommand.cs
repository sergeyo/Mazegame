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
            if (string.IsNullOrWhiteSpace(argument))
            {
                return "You should specify item number to equip:\n" + GetBackpackItemsList(context.Player.Backpack);
            }

            int itemNumber;
            if (!int.TryParse(argument, out itemNumber))
            {
                return "You should specify correct item number to equip:\n" + GetBackpackItemsList(context.Player.Backpack);
            }

            if (itemNumber < 0 || itemNumber > context.Player.Backpack.Count) {
                return "You should specify correct item number to equip:\n" + GetBackpackItemsList(context.Player.Backpack);
            }

            var item = context.Player.Backpack[itemNumber];

            if (item is Shield)
            {
                if (context.Player.Shield != null)
                {
                    context.Player.Backpack.Add(context.Player.Shield);
                }
                context.Player.Shield = (Shield)item;
            }
            else {
                if (item is Armor)
                {
                    if (context.Player.Armor != null)
                    {
                        context.Player.Backpack.Add(context.Player.Armor);
                    }
                    context.Player.Armor = (Armor)item;
                }
                else
                {
                    if (item is Weapon)
                    {
                        if (context.Player.Weapon != null)
                        {
                            context.Player.Backpack.Add(context.Player.Weapon);
                        }
                        context.Player.Weapon = (Weapon)item;
                    }
                    else
                    {
                        return "This item cannot be equipped";
                    }
                } 
            }
            context.Player.Backpack.Remove(item);

            return "Now you are equipped with " + item.Description;
        }

        private string GetBackpackItemsList(IEnumerable<Item> items) {
            return string.Join("\n", items.Select((item, index) => string.Format("{0}: {1}", index, item.Description)));
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return !context.IsInCombat();
        }
    }
}
