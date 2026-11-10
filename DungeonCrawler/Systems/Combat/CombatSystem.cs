using DungeonCrawler.Entities.Characters;
using DungeonCrawler.Entities.Characters.Enemies;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Systems.Combat
{
    // Handle combat between game characters
    internal class CombatSystem
    {
        // Store combat events
        private static readonly List<string> combatLog = new();

        // Handle attack and counterattack between player and enemy characters
        public static void InitiateCombat(Character attacker, Character target)
        {
            // Prevent combat if either character is already defeated
            if (attacker.HealthPoints <= 0 || target.HealthPoints <= 0)
            {
                return;
            }

            // Calculate damage using attacker's attack and target's defense dice rolls
            DiceConfiguration attackDice = attacker.Attack;
            DiceConfiguration defenseDice = target.Defense;
            int attackResult = attackDice.RollDice();
            int defenseResult = defenseDice.RollDice();
            int damage = attackResult - defenseResult;

            // Prevent negative damage when defense is higher than attack
            if (damage <= 0)
            {
                damage = 0;
            }
            else
            {
                target.HealthPoints -= damage;

                // Prevent health points from dropping below 0
                if (target.HealthPoints < 0)
                {
                    target.HealthPoints = 0;
                }
            }

            // Add event entries to combat log
            if (attacker is Player && target is Enemy targetEnemy)
            {
                string playerAttackMessage = $"You attacked {targetEnemy.Species}, dealing {damage} damage.";

                combatLog.Add(playerAttackMessage);
            }
            else if (attacker is Enemy attackingEnemy && target is Player)
            {
                string enemyAttackMessage = $"{attackingEnemy.Species} attacked you, dealing {damage} damage.";

                combatLog.Add(enemyAttackMessage);
            }

            // Initiate counterattack if target has remaining health points
            if (target.HealthPoints > 0)
            {
                // Calculate counterattack damage using target's attack and attacker's defense dice rolls
                DiceConfiguration counterAttackDice = target.Attack;
                DiceConfiguration counterDefenseDice = attacker.Defense;
                int counterAttackResult = counterAttackDice.RollDice();
                int counterDefenseResult = counterDefenseDice.RollDice();
                int counterDamage = counterAttackResult - counterDefenseResult;

                // Prevent negative counter damage when defense is higher than attack
                if (counterDamage <= 0)
                {
                    counterDamage = 0;
                }
                else
                {
                    attacker.HealthPoints -= counterDamage;

                    // Prevent health points from dropping below 0
                    if (attacker.HealthPoints < 0)
                    {
                        attacker.HealthPoints = 0;
                    }
                }

                // Add event entries to combat log
                if (target is Player && attacker is Enemy attackingEnemy)
                {
                    string playerCounterAttackMessage = $"You struck back, dealing {counterDamage} damage.";

                    combatLog.Add(playerCounterAttackMessage);
                }
                else if (target is Enemy counterAttackingEnemy && attacker is Player)
                {
                    string enemyCounterAttackMessage = $"{counterAttackingEnemy.Species} struck back, dealing {counterDamage} damage.";

                    combatLog.Add(enemyCounterAttackMessage);
                }
            }
        }

        // Get event entries from combat log
        public static List<string> HandleCombatLog()
        {
            List<string> combatEvents = new(combatLog);

            combatLog.Clear();

            return combatEvents;
        }
    }
}
