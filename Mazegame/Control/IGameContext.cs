using Mazegame.Entity;

namespace Mazegame.Control
{
    public interface IGameContext
    {
        Player Player { get; }

        bool IsInCombat();

        NonPlayerCharacter CurrentLocationEnemy { get; }
    }
}