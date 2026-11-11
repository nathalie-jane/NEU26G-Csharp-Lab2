using DungeonCrawler.Coordinates;
using DungeonCrawler.Systems.Combat;

namespace DungeonCrawler.Entities.Characters
{
    internal abstract class Character : LevelEntity
    {
        public int HealthPoints { get; set; }
        public DiceConfiguration Attack { get; }
        public DiceConfiguration Defense { get; }

        public Character(Position position, char displaySymbol, ConsoleColor displayColor, int healthPoints, DiceConfiguration attack, DiceConfiguration defense)
            : base(position, displaySymbol, displayColor)
        {
            HealthPoints = healthPoints;
            Attack = attack;
            Defense = defense;
        }
    }
}
