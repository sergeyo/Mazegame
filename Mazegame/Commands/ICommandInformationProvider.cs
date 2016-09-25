using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazegame.Commands
{
    public interface ICommandInformationProvider
    {
        IEnumerable<string> GetCommandsList();
        string GetCommandUsage(string command);
    }
}
