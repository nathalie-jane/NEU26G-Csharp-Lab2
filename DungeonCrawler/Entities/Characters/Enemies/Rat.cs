using DungeonCrawler.Coordinates;
using DungeonCrawler.Systems.Combat;
using DungeonCrawler.Systems.Movement;

namespace DungeonCrawler.Entities.Characters.Enemies
{
    internal class Rat : Enemy
    {
        public Rat(Position position)
            : base(position, 'r', ConsoleColor.Yellow, "Rat", 10, attack: new DiceConfiguration(1, 6, 3), defense: new DiceConfiguration(1, 6, 1)) { }

        public override void PerformAction(Player player, IReadOnlyList<LevelEntity> entities)
        {
            (int XAxis, int YAxis) ratMovement = MovementSystem.GetRatMovement();

            MovementSystem.TryEnemyMovement(this, ratMovement, player, entities);
        }
    }
}
