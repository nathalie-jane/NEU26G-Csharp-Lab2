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
    }
}
