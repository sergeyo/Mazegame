using Mazegame.Entity;

namespace Mazegame.Control
{
    public class GameContext : IGameContext
    {
        public GameContext(Location currentLocation, Player player)
        {
            CurrentLocation = currentLocation;
            Player = player;
        }

        public Location CurrentLocation { get; set; }
        public Location PreviousLocation { get; set; }
        public Player Player { get; private set; }

        public bool IsInCombat() { return false; }
    }
}
