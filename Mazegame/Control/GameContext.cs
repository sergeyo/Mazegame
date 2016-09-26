using System;
using Mazegame.Entity;

namespace Mazegame.Control
{
    public class GameContext : IGameContext
    {
        public GameContext(Player player)
        {
            Player = player;
        }

        public Player Player { get; private set; }

        public NonPlayerCharacter CurrentLocationEnemy
        {
            get
            {
                return null;
            }
        }

        public bool IsInCombat() { return false; }
    }
}
