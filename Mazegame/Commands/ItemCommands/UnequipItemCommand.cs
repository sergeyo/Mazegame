using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;

namespace Mazegame.Commands.ItemCommands
{
    public class UnequipItemCommand : ICommand
    {
        public string Name => "unequipItem";
        public string Usage => "Usage: unequipItem <weapon, armor or shield>\nUnequip specified item to backpack";

        public string Execute(IGameContext context, string argument)
        {
            var player = context.Player;
            switch (argument)
            {
                case "weapon":
                    if (player.Weapon == null) {
                        return "You have no " + argument + " equipped.";
                    }
                    player.Backpack.Add(player.Weapon);
                    player.Weapon = null;
                    return "You have unequiped " + argument;
                case "armor":
                    if (player.Armor == null)
                    {
                        return "You have no " + argument + " equipped.";
                    }
                    player.Backpack.Add(player.Armor);
                    player.Armor = null;
                    return "You have unequiped " + argument;
                case "shield":
                    if (player.Shield == null)
                    {
                        return "You have no " + argument + " equipped.";
                    }
                    player.Backpack.Add(player.Shield);
                    player.Shield = null;
                    return "You have unequiped " + argument;
                default:
                    return "You should specify what item do you want to unequip: weapon, armor or shield.";
            }
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return !context.IsInCombat();
        }
    }
}
