using Mazegame.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Entity;

namespace MazegameTest
{
    class GameContextStub : IGameContext
    {
        public Player Player { get; set; }

        public bool IsInCombatMode { get; set; }

        public NonPlayerCharacter CurrentLocationEnemy { get; set; }

        public bool IsInCombat()
        {
            return IsInCombatMode;
        }
    }
}
