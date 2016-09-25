using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;

namespace Mazegame
{
    class Program
    {
        static void Main(string[] args)
        {
            DungeonMaster theDm = new DungeonMaster(new HardCodedData(), new SimpleConsoleClient());
            theDm.RunGame();
        }
    }
}
