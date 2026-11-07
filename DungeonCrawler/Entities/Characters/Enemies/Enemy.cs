using DungeonCrawler.Coordinates;
using DungeonCrawler.Systems.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities.Characters.Enemies
{
    // Base class for all enemy types
    internal abstract class Enemy : Character
    {
        public string Species { get; }

        // Initialize properties shared by enemies
        protected Enemy(Position position, char displaySymbol, ConsoleColor displayColor, string species, int healthPoints, DiceConfiguration attack, DiceConfiguration defense)
            : base(position, displaySymbol, displayColor, healthPoints, attack, defense)
        {
            Species = species;
        }

        // Update enemy position after movement
        public void UpdatePosition(Position newPosition)
        {
            Position = newPosition;
        }

        // Implement specific behavior for each enemy type
        public abstract void PerformAction(Position playerPosition, IReadOnlyList<LevelEntity> entities);
    }
}
