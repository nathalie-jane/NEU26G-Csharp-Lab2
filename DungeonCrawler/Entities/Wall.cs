using DungeonCrawler.Coordinates;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities
{
    internal class Wall : LevelEntity
    {
        public Wall(Position position)
            : base(position, '#', ConsoleColor.Gray) { }
    }
}
