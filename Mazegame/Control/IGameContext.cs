using Mazegame.Entity;

namespace Mazegame.Control
{
    public interface IGameContext
    {
        Location CurrentLocation { get; set; }
        Player Player { get; }
        Location PreviousLocation { get; set; }

        bool IsInCombat();
    }
}