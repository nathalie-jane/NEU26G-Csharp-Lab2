using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Systems.Movement
{
    // Handle movement related logic for all game characters
    internal class MovementSystem
    {
        // Define possible movement directions for enemy characters
        private static readonly Dictionary<string, (int XAxis, int YAxis)> enemyMovements = new()
        {
            { "Up", (0, -1) },
            { "Right", (1, 0) },
            { "Down", (0, 1) },
            { "Left", (-1, 0) }
        };

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


        public static LevelEntity? GetMovementObstacle(Position targetPosition, IReadOnlyList<LevelEntity> entities)
        {
            foreach (LevelEntity entity in entities)
            {
                if (entity.Position == targetPosition)
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

                LevelEntity? levelObstacle = GetMovementObstacle(targetPlayerPosition, entities);

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

        // Get random movement direction for rat type enemies
        public static (int XAxis, int YAxis) GetRatMovement()
        {
            int movementsIndex = Random.Shared.Next(enemyMovements.Count);

            (int XAxis, int YAxis) ratMovement = enemyMovements.Values.ElementAt(movementsIndex);

            return ratMovement;
        }

        // Get movement direction for snake type enemies based on player position
        public static (int XAxis, int YAxis) GetSnakeMovement(Position snakePosition, Position playerPosition)
        {
            List<(int XAxis, int YAxis)> validMovements = new();
            {
                // Determine horizontal movement direction away from player
                if (snakePosition.XPosition > playerPosition.XPosition)
                {
                    validMovements.Add(enemyMovements["Right"]);
                }
                else if (snakePosition.XPosition < playerPosition.XPosition)
                {
                    validMovements.Add(enemyMovements["Left"]);
                }

                // Determine vertical movement direction away from player
                if (snakePosition.YPosition > playerPosition.YPosition)
                {
                    validMovements.Add(enemyMovements["Down"]);
                }
                else if (snakePosition.YPosition < playerPosition.YPosition)
                {
                    validMovements.Add(enemyMovements["Up"]);
                }

                // Prevent snake from moving if no movement leads away from player
                if (validMovements.Count == 0)
                {
                    return (0, 0);
                }
                else // Randomly select a valid movement direction when the player is diagonally positioned from the snake
                {
                    int movementsIndex = Random.Shared.Next(validMovements.Count);

                    (int XAxis, int YAxis) snakeMovement = validMovements.ElementAt(movementsIndex);

                    return snakeMovement;
                } 
            }
        }

        // Calculate where an enemy should move based on its current position and chosen movement direction
        public static Position GetEnemyTargetPosition(Position enemyPosition, (int XAxis, int YAxis) enemyMovement)
        {
            Position targetEnemyPosition = new((enemyPosition.XPosition + enemyMovement.XAxis), (enemyPosition.YPosition + enemyMovement.YAxis));

            return targetEnemyPosition;
        }
    }
}