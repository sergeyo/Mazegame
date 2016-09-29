using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;
using Mazegame.Entity;

namespace Mazegame.Commands.ItemCommands
{
    public class PurchaseItemCommand : ICommand
    {
        public string Name => "purchaseItem";
        public string Usage => "Usage: purchaseItem <itemNumber>\nPurchase item from shop store. Works only if you are in Shop location. You must have enough money to purchase an item.";

        public string Execute(IGameContext context, string argument)
        {
            var shop = (Shop)context.Player.Location;
            var store = shop.Store;

            if (!store.Any())
            {
                return "There is no items you can purchase.";
            }

            if (string.IsNullOrWhiteSpace(argument))
            {
                return "There are some items you can purchase in store:\n"
                     + store.GetItemsIndexedListWithWorths()
                     + $"\nYou have {context.Player.Gold} gold";
            }

            int itemNumber;
            if (!int.TryParse(argument, out itemNumber))
            {
                return "You should specify correct item number to purchase:\n"
                    + store.GetItemsIndexedListWithWorths()
                     + $"\nYou have {context.Player.Gold} gold";
            }

            if (itemNumber < 0 || itemNumber > store.Count - 1)
            {
                return "You should specify correct item number to purchase:\n"
                    + store.GetItemsIndexedListWithWorths()
                     + $"\nYou have {context.Player.Gold} gold";
            }

            var item = store[itemNumber];

            if (context.Player.Gold < item.Worth) {
                return "You dont have enough gold to purchase this item.\n"
                     + $"\nYou have only {context.Player.Gold} gold, but item's worht is {item.Worth} gold.";
            }

            if (context.Player.GetCurrentWeight() + item.Weight > context.Player.MaxWeight)
            {
                return "You can't carry this item, its too heavy.\n"
                     + $"\nYour item's total weight is {context.Player.GetCurrentWeight()}, item's wweight is {item.Weight}, and you can carry only {context.Player.MaxWeight}.";
            }

            store.Remove(item);
            context.Player.AddItemToBackpack(item);

            context.Player.Gold -= item.Worth;

            return "You purchased " + item.Description;
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return context.Player.Location is Shop;
        }
    }
}
