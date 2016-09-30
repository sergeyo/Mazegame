using Mazegame.Control;

namespace Mazegame.Commands
{
    public class GroupCommand : ICommand
    {
        public string Name => "group";
        public string Usage => "Usage: group\nTake friendly NPC to your party. He will help you in your journey.";

        public string Execute(IGameContext context, string argument)
        {
            if (context.Player.Party.Character != null) {
                return $"You cant group with {context.Player.Location.NPC.Name} because you already have a company.";
            }

            var npc = context.Player.Location.NPC;

            context.Player.Party.Character = context.Player.Location.NPC;
            context.Player.Location.NPC = null;

            var dmgString = npc.Weapon != null
                ? $"Dmg: {npc.Weapon.Dice.ToString()}, "
                : "";

            return $"{ npc.Name} ({dmgString}AC: {npc.AC}, Life points: {npc.LifePoints}) has joined you!";
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return !context.IsInCombat()
                && context.Player.Location.NPC != null
                && context.Player.Location.NPC.Hostile == false;
        }
    }
}
