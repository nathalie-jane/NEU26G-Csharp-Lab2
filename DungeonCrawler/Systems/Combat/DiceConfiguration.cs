using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Systems.Combat
{
    // Dice configuration for combat mechanics
    internal class DiceConfiguration
    {
        // Read-only to prevent changes to dice configuration after initialization
        public int Quantity { get; }
        public int Sides { get; }
        public int Modifier { get; }

        // Initialize values for dice rolls
        public DiceConfiguration(int quantity, int sides, int modifier)
        {
            Quantity = quantity;
            Sides = sides;
            Modifier = modifier;
        }

        // Roll configured number of dice and return combined result with modifier
        public int RollDice()
        {
            int diceSum = 0;

            // Roll specified number of dice and add to sum
            for (int diceRollCount = 0; diceRollCount < Quantity; diceRollCount++)
            {
                int diceRollValue = Random.Shared.Next(1, 7);
                diceSum += diceRollValue;
            }

            diceSum += Modifier;

            return diceSum;
        }

        // Return dice configuration as a string
        public override string ToString()
        {
            string diceNotation = $"{Quantity}d{Sides}+{Modifier}";

            return diceNotation;
        }
    }
}
