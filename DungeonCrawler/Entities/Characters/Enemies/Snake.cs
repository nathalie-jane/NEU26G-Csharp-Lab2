using DungeonCrawler.Coordinates;
using DungeonCrawler.Systems.Combat;
using DungeonCrawler.Systems.Movement;
using DungeonCrawler.Systems.Vision;

namespace DungeonCrawler.Entities.Characters.Enemies
{
    internal class Snake : Enemy
    {
        public Snake(Position position) 
            : base(position, 's', ConsoleColor.DarkGreen, "Snake", 25, attack: new DiceConfiguration(3, 4, 2), defense: new DiceConfiguration(1, 8, 5)) { }

        public override void PerformAction(Player player, IReadOnlyList<LevelEntity> entities)
        {
            bool isPlayerWithinRange = VisionSystem.IsSnakeWithinRange(player.Position, Position);

            if (isPlayerWithinRange)
            {
                (int XAxis, int YAxis) snakeMovement = MovementSystem.GetSnakeMovement(Position, player.Position);

                MovementSystem.TryEnemyMovement(this, snakeMovement, player, entities);
            }
        }
    }
}
