using DungeonCrawler.Coordinates;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities
{
    // Base class for all game entities
    internal abstract class LevelEntity
    {
        public Position Position { get; protected set; } // Allow position to be modified only within LevelEntity class and its subclasses
        public char DisplaySymbol { get; }
        public ConsoleColor DisplayColor { get; }

        // Initialize properties shared by game entities
        protected LevelEntity(Position position, char displaySymbol, ConsoleColor displayColor)
        {
            Position = position;
            DisplaySymbol = displaySymbol;
            DisplayColor = displayColor;
        }

        // Handle rendering of an individual game entity in the console window
        public void RenderEntity()
        {
            Console.SetCursorPosition(Position.XPosition, Position.YPosition);

            Console.ForegroundColor = DisplayColor;
            Console.Write(DisplaySymbol);

            Console.ResetColor();
        }
    }
}
