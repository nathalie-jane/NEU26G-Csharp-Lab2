using DungeonCrawler.Coordinates;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities
{
    // Shared properties and behavior for all game elements
    internal abstract class LevelElement
    {
        
        public Position Position { get; protected set; } // Allow position to be modified only within LevelElement class and its subclasses
        public char Entity { get; }
        public ConsoleColor EntityColor { get; }

        // Initialise properties shared by game elements
        protected LevelElement(Position position, char entity, ConsoleColor entityColor)
        {
            Position = position;
            Entity = entity;
            EntityColor = entityColor;
        }

        // Render game elements on the console
        public void Render()
        {
            Console.SetCursorPosition(Position.XPosition, Position.YPosition); // Set position for game elements on the console

            Console.ForegroundColor = EntityColor;
            Console.Write(Entity);

            Console.ResetColor();
        }
    }
}
