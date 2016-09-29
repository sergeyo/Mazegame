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
                var npc = Player.Location.NPC;
                return npc.Hostile ? npc : null;
            }
        }

        public bool IsInCombat() { return CurrentLocationEnemy != null; }
    }
}
