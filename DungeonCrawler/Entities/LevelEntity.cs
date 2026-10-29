using DungeonCrawler.Coordinates;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities
{
    // Shared properties and behavior for all game entities
    internal abstract class LevelEntity
    {
        
        public Position Position { get; protected set; } // Allow position to be modified only within LevelEntity class and its subclasses
        public char DisplaySymbol { get; }
        public ConsoleColor DisplayColor { get; }

        // Initialise properties shared by game entities
        protected LevelEntity(Position position, char displaySymbol, ConsoleColor displayColor)
        {
            Position = position;
            DisplaySymbol = displaySymbol;
            DisplayColor = displayColor;
        }

        // Render game entities on the console
        public void Render()
        {
            Console.SetCursorPosition(Position.XPosition, Position.YPosition); // Set position for game entities on the console

            Console.ForegroundColor = DisplayColor;
            Console.Write(DisplaySymbol);

            Console.ResetColor();
        }
    }
}
