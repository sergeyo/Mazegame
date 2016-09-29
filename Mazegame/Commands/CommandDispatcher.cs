using Mazegame.Commands.ItemCommands;
using Mazegame.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazegame.Commands
{
    public class CommandDispatcher : ICommandInformationProvider
    {
        private Dictionary<string, ICommand> commands;

        public CommandDispatcher()
        {
            var availableCommands = new ICommand[] {
                new HelpCommand(this),
                new GotoCommand(),
                new ListItemsCommand(),
                new EquipItemCommand(),
                new UnequipItemCommand(),
                new GetItemCommand(),
                new DropItemCommand(),
                new PurchaseItemCommand(),
                new SellItemCommand()
            };

            commands = availableCommands.ToDictionary(c => c.Name);
        }

        IEnumerable<string> ICommandInformationProvider.GetCommandsList()
        {
            return commands.Keys;
        }

        string ICommandInformationProvider.GetCommandUsage(string command)
        {
            return commands[command].Usage;
        }

        public string Execute(IGameContext context, string command, string argument)
        {
            if (!commands.ContainsKey(command))
            {
                return "Unknown command. Use 'help' to see known commands list.";
            }

            return commands[command].Execute(context, argument);
        }
    }
}
