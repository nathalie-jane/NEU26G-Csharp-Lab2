using DungeonCrawler.Coordinates;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Systems.Vision
{
    // Manage vision ranges based on character positions
    internal class VisionSystem
    {
        // Calculate straight-line distance between two positions
        public static double GetStraightLineDistance(Position position1, Position position2)
        {
            int deltaX = position1.XPosition - position2.XPosition;
            int deltaY = position1.YPosition - position2.YPosition;
            double straightLineDistance = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));

            return straightLineDistance;
        }

        // Check if an entity is within player's vision range
        public static bool IsEntityWithinVisionRange(Position playerPosition, Position entityPosition)
        {
            double distanceToEntity = GetStraightLineDistance(playerPosition, entityPosition);
            bool isWithinRange = distanceToEntity <= 5; // Player vision range is 5 units

            return isWithinRange;
        }

        // Check if player is within snake detection range
        public static bool IsSnakeWithinRange(Position playerPosition, Position snakePosition)
        {
            double distanceToSnake = GetStraightLineDistance(playerPosition, snakePosition);
            bool isWithinRange = distanceToSnake <= 2; // Snake detection range is 2 units

            return isWithinRange;
        }
    }
}
