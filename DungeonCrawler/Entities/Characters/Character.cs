using DungeonCrawler.Coordinates;
using DungeonCrawler.Systems.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities.Characters
{
    // Base class for all game characters
    internal abstract class Character : LevelEntity
    {
        public int HealthPoints { get; protected set; } // Allow health points to be modified only within Character class and its subclasses
        public DiceConfiguration Attack { get; }
        public DiceConfiguration Defense { get; }

        // Initialize properties shared by game characters
        protected Character(Position position, char displaySymbol, ConsoleColor displayColor, int healthPoints, DiceConfiguration attack, DiceConfiguration defense)
            : base(position, displaySymbol, displayColor)
        {
            HealthPoints = healthPoints;
            Attack = attack;
            Defense = defense;
        }
    }
}
