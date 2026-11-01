using DungeonCrawler.Coordinates;
using DungeonCrawler.Systems.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities.Characters.Enemies
{
    // Properties and behavior specific to rat type enemy
    internal class Rat : Enemy
    {
        // Initialise properties specific to rat type enemy
        public Rat(Position position) : base(position, 'r', ConsoleColor.Yellow, "Rat", 10, attack: new DiceConfiguration(1, 6, 3), defense: new DiceConfiguration(1, 6, 1)) { }

        public override void PerformAction()
        {
            // TODO: Add behavior for rat type enemy
        }
    }
}
