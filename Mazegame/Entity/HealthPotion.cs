using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazegame.Entity
{
    public class HealthPotion : Item
    {
        private static Dice dice = new Dice(2, 6);

        public HealthPotion()
        {
            Description = $"Health potion: restores {dice.ToString()} life points.";
            Worth = 10;
            Weight = 1;
        }

        public override string UseItem(Player player)
        {
            var healPoints = dice.GetRollResult();

            var originalLifePoints = player.LifePoints;

            player.Heal(healPoints);
            player.Backpack.Remove(this);

            return $"You have healed {player.LifePoints - originalLifePoints} life points, now it's {player.LifePoints}";
        }
    }
}
