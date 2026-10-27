using DungeonCrawler.Coordinates;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Entities
{
    // Shared properties and behavior for all game elements
    internal abstract class LevelElement
    {
        public Position Position { get; protected set; }
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
            Console.SetCursorPosition(Position.XPosition, Position.YPosition); // Set game element position on the console

            Console.ForegroundColor = EntityColor;
            Console.Write(Entity);

            Console.ResetColor();
        }
    }
}
