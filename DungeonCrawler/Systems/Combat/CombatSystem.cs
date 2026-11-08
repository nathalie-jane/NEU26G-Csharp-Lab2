using DungeonCrawler.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Systems.Combat
{
    // Handle combat between game characters
    internal class CombatSystem
    {
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
            }
        }
    }
}
