using Mazegame.Control;

namespace Mazegame.Commands
{
    public class DisbandCommand : ICommand
    {
        public string Name => "disband";
        public string Usage => "Usage: disband\nLeave party member in current location.";

        public string Execute(IGameContext context, string argument)
        {
            var npc = context.Player.Party.Character;
            if (context.Player.Location.NPC != null)
            {
                return $"{npc.Name} can't leave you right now, he won't stay here with {context.Player.Location.NPC.Name}";
            }

            context.Player.Location.NPC = npc;
            context.Player.Party.Character = null;

            return $"{npc.Name} leaves you and stays in this location.";
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return !context.IsInCombat() 
                && context.Player.Party.Character != null;
        }
    }
}
