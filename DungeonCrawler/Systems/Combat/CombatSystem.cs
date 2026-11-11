using DungeonCrawler.Entities.Characters;
using DungeonCrawler.Entities.Characters.Enemies;

namespace DungeonCrawler.Systems.Combat
{
    internal class CombatSystem
    {
        public static readonly List<string> combatLog = new();

        public static void InitiateCombat(Character attacker, Character target)
        {
            if (attacker.HealthPoints <= 0 || target.HealthPoints <= 0)
            {
                return;
            }

            int damage = CalculateDamage(attacker, target);

            AddCombatMessage(attacker, target, damage, false);

            if (target.HealthPoints > 0)
            {
                int counterDamage = CalculateDamage(target, attacker);

                AddCombatMessage(target, attacker, counterDamage, true);
            }
        }

        public static int CalculateDamage(Character attacker, Character target)
        {
            DiceConfiguration attackDice = attacker.Attack;
            DiceConfiguration defenseDice = target.Defense;

            int attackResult = attackDice.RollDice();
            int defenseResult = defenseDice.RollDice();

            int damage = attackResult - defenseResult;

            if (damage < 0)
            {
                damage = 0;
            }

            target.HealthPoints -= damage;

            if (target.HealthPoints < 0)
            {
                target.HealthPoints = 0;
            }

            return damage;
        }

        public static void AddCombatMessage(Character attacker, Character target, int damage, bool isCounterAttack)
        {
            if (isCounterAttack)
            {
                if (attacker is Player && target is Enemy)
                {
                    string playerCounterAttackMessage = $"You struck back, dealing {damage} damage.";

                    combatLog.Add(playerCounterAttackMessage);
                }
                else if (attacker is Enemy enemyAttacking && target is Player)
                {
                    string enemyCounterAttackMessage = $"{enemyAttacking.Species} struck back, dealing {damage} damage.";

                    combatLog.Add(enemyCounterAttackMessage);
                }
            }
            else
            {
                if (attacker is Player && target is Enemy enemyTarget)
                {
                    string playerAttackMessage = $"You attacked {enemyTarget.Species}, dealing {damage} damage.";

                    combatLog.Add(playerAttackMessage);
                }
                else if (attacker is Enemy enemyAttacker && target is Player)
                {
                    string enemyAttackMessage = $"{enemyAttacker.Species} attacked you, dealing {damage} damage.";

                    combatLog.Add(enemyAttackMessage);
                }
            }
        }

        public static List<string> HandleCombatLog()
        {
            List<string> combatEvents = new(combatLog);

            combatLog.Clear();

            return combatEvents;
        }
    }
}