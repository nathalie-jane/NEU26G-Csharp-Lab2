using DungeonCrawler.Coordinates;
using DungeonCrawler.Systems.Combat;

namespace DungeonCrawler.Entities.Characters.Enemies
{
    internal abstract class Enemy : Character
    {
        public string Species { get; }

        public Enemy(Position position, char displaySymbol, ConsoleColor displayColor, string species, int healthPoints, DiceConfiguration attack, DiceConfiguration defense)
            : base(position, displaySymbol, displayColor, healthPoints, attack, defense)
        {
            Species = species;
        }

        public void UpdatePosition(Position newPosition)
        {
            Position = newPosition;
        }

        public abstract void PerformAction(Player player, IReadOnlyList<LevelEntity> entities);
    }
}
