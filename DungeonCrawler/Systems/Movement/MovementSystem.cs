using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Characters;
using DungeonCrawler.Entities.Characters.Enemies;
using DungeonCrawler.Systems.Combat;

namespace DungeonCrawler.Systems.Movement
{
    internal class MovementSystem
    {
        public static readonly Dictionary<string, (int XAxis, int YAxis)> enemyMovements = new()
        {
            { "Up", (0, -1) },
            { "Right", (1, 0) },
            { "Down", (0, 1) },
            { "Left", (-1, 0) }
        };

        public static ConsoleKey GetKeyInput()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
            ConsoleKey keyInput = keyInfo.Key;

            return keyInput;
        }

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

        public static bool TryPlayerMovement(Player player, ConsoleKey keyInput, IReadOnlyList<LevelEntity> entities)
        {
            if (!IsKeyInputValidMovement(keyInput))
            {
                return false;
            }
           
            Position targetPlayerPosition = GetPlayerTargetPosition(player.Position, keyInput);

            LevelEntity? levelObstacle = GetMovementObstacle(targetPlayerPosition, entities);

            if (levelObstacle is Enemy enemy)
            {
                CombatSystem.InitiateCombat(player, enemy);

                return false;
            }

            if (levelObstacle is not null)
            {
                return false;
            }
                
            player.UpdatePosition(targetPlayerPosition);

            return true;
        }

        public static bool IsPlayerAtTargetPosition(Position targetEnemyPosition, Position playerPosition)
        {
            bool isPlayerAtTargetPosition = targetEnemyPosition == playerPosition;

            return isPlayerAtTargetPosition;
        }

        public static (int XAxis, int YAxis) GetRatMovement()
        {
            int movementsIndex = Random.Shared.Next(enemyMovements.Count);

            (int XAxis, int YAxis) ratMovement = enemyMovements.Values.ElementAt(movementsIndex);

            return ratMovement;
        }

        public static (int XAxis, int YAxis) GetSnakeMovement(Position snakePosition, Position playerPosition)
        {
            List<(int XAxis, int YAxis)> validMovements = new();
            
            if (snakePosition.XPosition > playerPosition.XPosition)
            {
                validMovements.Add(enemyMovements["Right"]);
            }
            else if (snakePosition.XPosition < playerPosition.XPosition)
            {
                validMovements.Add(enemyMovements["Left"]);
            }

            if (snakePosition.YPosition > playerPosition.YPosition)
            {
                validMovements.Add(enemyMovements["Down"]);
            }
            else if (snakePosition.YPosition < playerPosition.YPosition)
            {
                validMovements.Add(enemyMovements["Up"]);
            }

            if (validMovements.Count == 0)
            {
                return (0, 0);
            }
            else
            {
                int movementsIndex = Random.Shared.Next(validMovements.Count);

                (int XAxis, int YAxis) snakeMovement = validMovements.ElementAt(movementsIndex);

                return snakeMovement;
            }
        }

        public static Position GetEnemyTargetPosition(Position enemyPosition, (int XAxis, int YAxis) enemyMovement)
        {
            Position targetEnemyPosition = new((enemyPosition.XPosition + enemyMovement.XAxis), (enemyPosition.YPosition + enemyMovement.YAxis));

            return targetEnemyPosition;
        }

        public static bool TryEnemyMovement(Enemy enemy, (int XAxis, int YAxis) enemyMovement, Player player, IReadOnlyList<LevelEntity> entities)
        {
            if (enemyMovement is (0, 0)) 
            {
                return false;
            }
            
            Position targetEnemyPosition = GetEnemyTargetPosition(enemy.Position, enemyMovement);

            LevelEntity? levelObstacle = GetMovementObstacle(targetEnemyPosition, entities);

            if (levelObstacle is not null)
            {
                return false;
            }
               
            bool isPlayerAtTargetPosition = IsPlayerAtTargetPosition(targetEnemyPosition, player.Position);

            if (isPlayerAtTargetPosition)
            {
                CombatSystem.InitiateCombat(enemy, player);

                return false;
            }
            else
            {
                enemy.UpdatePosition(targetEnemyPosition);

                return true;
            }      
        }
    }
}