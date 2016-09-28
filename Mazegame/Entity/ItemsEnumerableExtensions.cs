using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazegame.Entity
{
    public static class ItemsEnumerableExtensions
    {
        public static string GetItemsIndexedList(this IEnumerable<Item> items)
        {
            return string.Join("\n", items.Select((item, index) => $"{index}: {item.Description}"));
        }

        public static string GetItemsListWithWeights(this IEnumerable<Item> items)
        {
            return string.Join("\n", items.Select(item => $"{item.Description} (weight {item.Weight})"));
        }

        public static string GetItemsIndexedListWithWeights(this IEnumerable<Item> items)
        {
            return string.Join("\n", items.Select((item, index) => $"{index}: {item.Description} (weight {item.Weight})"));
        }
        
    }
}
