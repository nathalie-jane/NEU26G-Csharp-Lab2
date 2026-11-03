using DungeonCrawler.Coordinates;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities
{
    // Extend LevelEntity with predefined values for walls
    internal class Wall : LevelEntity
    {
        // Initialize properties shared by walls
        public Wall(Position position) 
            : base(position, '#', ConsoleColor.Gray) { }
    }
}
