using DungeonCrawler.Coordinates;
using DungeonCrawler.Systems.Combat;
using DungeonCrawler.Systems.Movement;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities.Characters.Enemies
{
    // Extend Enemy class with predefined values for rats
    internal class Rat : Enemy
    {
        // Initialize properties specific to rats
        public Rat(Position position)
            : base(position, 'r', ConsoleColor.Yellow, "Rat", 10, attack: new DiceConfiguration(1, 6, 3), defense: new DiceConfiguration(1, 6, 1)) { }

        // Handle rat movement behavior
        public override void PerformAction(Player player, IReadOnlyList<LevelEntity> entities)
        {
            (int XAxis, int YAxis) ratMovement = MovementSystem.GetRatMovement();

            MovementSystem.TryEnemyMovement(this, ratMovement, player, entities);
        }
    }
}
