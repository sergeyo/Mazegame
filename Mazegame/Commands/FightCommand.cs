using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mazegame.Control;
using Mazegame.Entity;

namespace Mazegame.Commands
{
    public class FightCommand : ICommand
    {
        public string Name => "fight";
        public string Usage => "Usage: fight\nPerform an attack to hostile creature. This command is available only in battle, wich occurs when you enters to the location with hostile creature.";

        public FightCommand(Dice attackEnrollmentDice)
        {
            _attackEnrollmentDice = attackEnrollmentDice;
        }

        public string Execute(IGameContext context, string argument)
        {
            if (context.CurrentLocationEnemy == null)
            {
                return "There is no one to fight with.";
            }
            if (context.Player.Weapon == null)
            {
                return "You have no weapon to fight! Runaway!";
            }

            var sb = new StringBuilder();

            sb.AppendLine(PerformAttack(context.Player, context.CurrentLocationEnemy));

            if (context.Player.Party.Character != null && context.CurrentLocationEnemy.LifePoints > 0)
            {
                sb.AppendLine(PerformAttack(context.Player.Party.Character, context.CurrentLocationEnemy));
            }

            if (context.CurrentLocationEnemy.LifePoints <= 0)
            {
                sb.AppendLine("You just killed " + context.CurrentLocationEnemy.Name);
                context.Player.Location.NPC = null;

                sb.AppendLine("");
                sb.AppendLine("Now you can look around in this location:");
                sb.AppendLine(context.Player.Location.GetLongDescription());
            } else
            {
                Character victim = context.Player.Party.Character;
                sb.AppendLine(
                    PerformAttack(
                        context.CurrentLocationEnemy, 
                        (Character)context.Player.Party.Character ?? context.Player));
            }

            if (context.Player.LifePoints <= 0)
            {
                sb.AppendLine("You died");
            }
            if (context.Player.Party.Character?.LifePoints <= 0)
            {
                sb.AppendLine($"{context.Player.Party.Character.Name} died");
                context.Player.Party.Character = null;
            }

            return sb.ToString();
        }

        private Dice _attackEnrollmentDice;

        private enum HitMode { Miss, Hit, CriticalHit }

        private string PerformAttack(Character attacker, Character victim)
        {
            var attackEnrollmentDiceResult = _attackEnrollmentDice.GetRollResult();

            var hitMode = HitMode.Miss;

            switch (attackEnrollmentDiceResult)
            {
                case 1:
                    hitMode = HitMode.Miss;
                    break;
                case 20:
                    hitMode = HitMode.CriticalHit;
                    break;
                default:
                    hitMode = attackEnrollmentDiceResult + attacker.Strength >= victim.AC
                        ? HitMode.Hit
                        : HitMode.Miss;
                    break;
            }

            var damage = attacker.Weapon.Dice.GetRollResult() + attacker.Strength;
            if (hitMode == HitMode.CriticalHit)
            {
                damage *= 2;
            }

            switch (hitMode)
            {
                case HitMode.Miss:
                    return $"{attacker.Name} misses!";
                case HitMode.Hit:
                case HitMode.CriticalHit:
                    victim.RecieveDamage(damage);
                    var hitText = hitMode == HitMode.CriticalHit ? "critically hits" : "hits";
                    return $"{attacker.Name} {hitText} {victim.Name}, dealing damage {damage} life points.";
            }

            return "WAT?";
        }

        public bool IsAvailableInContext(IGameContext context)
        {
            return context.IsInCombat();
        }
    }
}
