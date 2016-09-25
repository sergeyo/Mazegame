using Mazegame.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazegame.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Usage { get; }

        bool IsAvailableInContext(IGameContext context);
        string Execute(IGameContext context, string argument);
    }
}
