using DungeonCrawler.Coordinates;
using DungeonCrawler.Systems.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities.Characters
{
    // Extend Character class with predefined values for player character
    internal class Player : Character
    {
        // Initialize properties shared by player character
        public Player(Position position) 
            : base(position, '@', ConsoleColor.Cyan, 100, attack: new DiceConfiguration(2, 6, 2), defense: new DiceConfiguration(2, 6, 0)) { }
    }
}
