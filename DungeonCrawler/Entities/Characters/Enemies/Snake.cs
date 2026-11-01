using DungeonCrawler.Coordinates;
using DungeonCrawler.Systems.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities.Characters.Enemies
{
    // Properties and behavior specific to snake type enemy
    internal class Snake : Enemy
    {
        // Initialise properties specific to snake type enemy
        public Snake(Position position) : base(position, 's', ConsoleColor.DarkGreen, "Snake", 25, attack: new DiceConfiguration(3, 4, 2), defense: new DiceConfiguration(1, 8, 5)) { }

        public override void PerformAction()
        {
            // TODO: Add behavior for snake type enemy
        }
    }
}
