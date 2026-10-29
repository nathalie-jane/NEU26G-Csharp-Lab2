using DungeonCrawler.Coordinates;
using DungeonCrawler.Systems.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities.Enemies
{
    // Shared properties and behavior for all enemies
    internal abstract class Enemy : LevelEntity
    {
        public string Species { get; }
        public int HealthPoints { get; protected set; } // Allow health points to be modified only within Enemy class and its subclasses
        public DiceConfiguration Attack { get; }
        public DiceConfiguration Defense { get; }

        // Initialise properties shared by enemies
        protected Enemy(Position position, char displaySymbol, ConsoleColor displayColor, string species, int healthPoints, DiceConfiguration attack, DiceConfiguration defense) : base(position, displaySymbol, displayColor)
        {
            Species = species;
            HealthPoints = healthPoints;
            Attack = attack;
            Defense = defense;
        }

        // Implement specific behavior for each enemy type
        public abstract void PerformAction();
    }
}
