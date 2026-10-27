using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Coordinates
{
    // X and Y coordinates for game elements
    internal readonly record struct Position
    {
        public int XPosition { get; }
        public int YPosition { get; }

        // Initialise X and Y coordinates for game elements
        public Position(int xPosition, int yPosition)
        {
            XPosition = xPosition;
            YPosition = yPosition;
        }
    }
}
