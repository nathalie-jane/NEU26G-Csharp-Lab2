using DungeonCrawler.Coordinates;
using DungeonCrawler.Systems.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities.Characters.Enemies
{
    // Shared properties and behavior for all enemies
    internal abstract class Enemy : Character
    {
        public string Species { get; }

        // Initialise properties shared by enemies
        protected Enemy(Position position, char displaySymbol, ConsoleColor displayColor, string species, int healthPoints, DiceConfiguration attack, DiceConfiguration defense) : base(position, displaySymbol, displayColor, healthPoints, attack, defense)
        {
            Species = species;
        }

        // Implement specific behavior for each enemy type
        public abstract void PerformAction();
    }
}
