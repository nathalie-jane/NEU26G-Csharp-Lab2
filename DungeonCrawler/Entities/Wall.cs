using DungeonCrawler.Coordinates;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities
{
    // Wall element that inherits properties and behavior from LevelElement
    internal class Wall : LevelEntity
    {
        // Initialise wall element with X and Y coordinates, fixed character and color
        public Wall(Position position) : base(position, '#', ConsoleColor.Gray) { }
    }
}
