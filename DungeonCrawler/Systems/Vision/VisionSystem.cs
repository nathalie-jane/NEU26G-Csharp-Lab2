using DungeonCrawler.Coordinates;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Systems.Vision
{
    internal class VisionSystem
    {
        // Check if player is within snake detection range
        public static bool IsSnakeWithinRange(Position playerPosition, Position snakePosition)
        {
            // Calculate straight-line distance between player and snake
            int xPositionDifference = playerPosition.XPosition - snakePosition.XPosition;
            int yPositionDifference = playerPosition.YPosition - snakePosition.YPosition;
            double distanceToSnake = Math.Sqrt(Math.Pow(xPositionDifference, 2) + Math.Pow(yPositionDifference, 2));

            bool isWithinRange = distanceToSnake <= 2; // Snake detection range is 2 units

            return isWithinRange;
        }
    }
}
