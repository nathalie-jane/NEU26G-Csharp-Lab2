using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Systems.Movement
{
    internal class MovementSystem
    {
        // Get user key press for player movement
        public static ConsoleKey GetKeyInput()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true); // Prevent key press from being displayed in console window
            ConsoleKey keyInput = keyInfo.Key;

            return keyInput;
        }

        // Check if key press is a valid movement direction for player
        public static bool IsKeyInputValidMovement(ConsoleKey keyInput)
        {
            switch (keyInput)
            {
                case ConsoleKey.UpArrow:
                    return true;

                case ConsoleKey.RightArrow:
                    return true;

                case ConsoleKey.DownArrow:
                    return true;

                case ConsoleKey.LeftArrow:
                    return true;

                default:
                    return false;
            }
        }

        // Get target position for player based on movement direction from key press
        public static Position GetPlayerTargetPosition(Position currentPlayerPosition, ConsoleKey keyInput)
        {
            Position targetPlayerPosition = currentPlayerPosition;

            switch (keyInput)
            {
                case ConsoleKey.UpArrow:
                    targetPlayerPosition = new(currentPlayerPosition.XPosition, (currentPlayerPosition.YPosition - 1));
                    break;

                case ConsoleKey.RightArrow:
                    targetPlayerPosition = new((currentPlayerPosition.XPosition + 1), currentPlayerPosition.YPosition);
                    break;

                case ConsoleKey.DownArrow:
                    targetPlayerPosition = new(currentPlayerPosition.XPosition, (currentPlayerPosition.YPosition + 1));
                    break;

                case ConsoleKey.LeftArrow:
                    targetPlayerPosition = new((currentPlayerPosition.XPosition - 1), currentPlayerPosition.YPosition);
                    break;

                default:
                    break;
            }

            return targetPlayerPosition;
        }

        // Check if a game entity is occupying the player's target position
        public static LevelEntity? GetPlayerObstacle(Position targetPlayerPosition, IReadOnlyList<LevelEntity> entities)
        {
            foreach (LevelEntity entity in entities)
            {
                if (entity.Position == targetPlayerPosition)
                {
                    return entity;
                }
            }

            return null;
        }

        // Update player position if movement direction is valid and no game entity is occupying the target position
        public static bool TryPlayerMovement(Player player, ConsoleKey keyInput, IReadOnlyList<LevelEntity> entities)
        {
            if (!IsKeyInputValidMovement(keyInput)) // Prevent player from moving if key press is not a movement direction
            {
                return false;
            }
            else
            {
                Position targetPlayerPosition = GetPlayerTargetPosition(player.Position, keyInput);

                LevelEntity? levelObstacle = GetPlayerObstacle(targetPlayerPosition, entities);

                if (levelObstacle is not null) // Prevent player from moving into an occupied position
                {
                    return false;
                }
                else
                {
                    player.UpdatePosition(targetPlayerPosition);

                    return true;
                }
            }
        }
    }
}