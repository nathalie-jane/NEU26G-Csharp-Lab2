using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Systems.Combat
{
    // Dice configuration for combat mechanics
    internal class DiceConfiguration
    {
        public int Quantity { get; }
        public int Sides { get; }
        public int Modifier { get; }

        // Initialise values for dice rolls
        public DiceConfiguration(int quantity, int sides, int modifier)
        {
            Quantity = quantity;
            Sides = sides;
            Modifier = modifier;
        }
    }
}
