using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;

namespace DungeonCrawler.Systems.Vision
{
    internal class VisionSystem
    {
        public static readonly HashSet<Position> visibleWalls = new();

        public static double GetStraightLineDistance(Position position1, Position position2)
        {
            int deltaX = position1.XPosition - position2.XPosition;
            int deltaY = position1.YPosition - position2.YPosition;
            double straightLineDistance = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));

            return straightLineDistance;
        }

        public static bool IsEntityWithinVisionRange(Position playerPosition, Position entityPosition)
        {
            double distanceToEntity = GetStraightLineDistance(playerPosition, entityPosition);
            bool isWithinRange = distanceToEntity <= 5; // Player vision range is 5 units

            return isWithinRange;
        }

        public static bool IsSnakeWithinRange(Position playerPosition, Position snakePosition)
        {
            double distanceToSnake = GetStraightLineDistance(playerPosition, snakePosition);
            bool isWithinRange = distanceToSnake <= 2; // Snake detection range is 2 units

            return isWithinRange;
        }

        public static bool IsWallVisible(Position wallPosition)
        {
            bool isWallVisible = visibleWalls.Contains(wallPosition);

            return isWallVisible;
        }

        public static void UpdateDiscoveredWalls(Position playerPosition, IReadOnlyList<LevelEntity> entities)
        {
            foreach (LevelEntity entity in entities)
            {
                if (entity is Wall wall)
                {
                    bool isWithinVisionRange = IsEntityWithinVisionRange(playerPosition, wall.Position);

                    if (isWithinVisionRange)
                    {
                        visibleWalls.Add(wall.Position);
                    }
                }
            }
        }
    }
}
