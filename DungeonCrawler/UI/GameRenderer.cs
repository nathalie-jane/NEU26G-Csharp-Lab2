using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Characters;
using DungeonCrawler.Entities.Characters.Enemies;
using DungeonCrawler.Levels;
using DungeonCrawler.Systems.Vision;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.UI
{
    internal class GameRenderer
    {
        // Center game level map in console window
        public static void CenterMap()
        {
            int mapWidth = 53;
            int mapHeight = 18;

            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            int centerMapXAxis = (consoleWidth - mapWidth) / 2;
            int centerMapYAxis = (consoleHeight - mapHeight) / 2;

            LevelEntity.CenterMapXAxis = Math.Max(0, centerMapXAxis);
            LevelEntity.CenterMapYAxis = Math.Max(0, centerMapYAxis);
        }

        // Render walls that have entered player's vision range
        public static void RenderVisibleWalls(Position playerPosition, Position previousPlayerPosition, IReadOnlyList<LevelEntity> entities)
        {
            foreach (LevelEntity entity in entities)
            {
                // Check if wall has entered player's vision range
                if (entity is Wall wall)
                {
                    bool isVisible = VisionSystem.IsEntityWithinVisionRange(playerPosition, wall.Position);
                    bool wasHidden = !VisionSystem.IsEntityWithinVisionRange(previousPlayerPosition, wall.Position);

                    if (isVisible && wasHidden)
                    {
                        wall.RenderEntity();
                    }
                }
            }
        }

        // Update character positions without re-rendering the entire level to prevent flickering in the console window
        public static void UpdateRenderedCharacterPositions(Player player, Dictionary<Character, Position> previousPositions)
        {

            Position previousPlayerPosition = previousPositions[player];

            // Clear previous positions for characters that have moved or been defeated
            foreach (KeyValuePair<Character, Position> characterPosition in previousPositions)
            {
                Character character = characterPosition.Key;
                Position previousPosition = characterPosition.Value;
                bool hasMoved = previousPosition != character.Position;

                if (character is Player)
                {
                    if (hasMoved)
                    {
                        Console.SetCursorPosition((previousPosition.XPosition + LevelEntity.CenterMapXAxis), (previousPosition.YPosition + LevelEntity.CenterMapYAxis));
                        Console.Write(' ');
                    }
                }
                else if (character is Enemy enemy)
                {
                    bool isDefeated = enemy.HealthPoints <= 0;
                    bool wasVisible = VisionSystem.IsEntityWithinVisionRange(previousPlayerPosition, previousPosition);
                    bool isHidden = !VisionSystem.IsEntityWithinVisionRange(player.Position, enemy.Position);

                    if (wasVisible && (hasMoved || isDefeated || isHidden))
                    {
                        Console.SetCursorPosition(previousPosition.XPosition + LevelEntity.CenterMapXAxis, previousPosition.YPosition + LevelEntity.CenterMapYAxis);

                        Console.Write(' ');
                    }
                }
            }

            // Render characters that have moved to new positions
            foreach (KeyValuePair<Character, Position> characterPosition in previousPositions)
            {
                Character character = characterPosition.Key;
                Position previousPosition = characterPosition.Value;
                bool hasMoved = previousPosition != character.Position;

                // Prevent defeated enemies from being re-rendered
                if (character is Enemy && character.HealthPoints <= 0)
                {
                    continue;
                }

                // Render player if its position has changed
                if (character is Player)
                {
                    if (hasMoved)
                    {
                        character.RenderEntity();
                    }
                }
                else if (character is Enemy enemy)
                {
                    bool isVisible = VisionSystem.IsEntityWithinVisionRange(player.Position, enemy.Position);
                    bool wasHidden = !VisionSystem.IsEntityWithinVisionRange(previousPlayerPosition, previousPosition);

                    if (isVisible && (hasMoved || wasHidden))
                    {
                        enemy.RenderEntity();
                    }
                }
            }
        }

        // Render discovered walls and enemies within player's vision range
        public static void RenderLevel(Player player, IReadOnlyList<LevelEntity> entities)
        {
            Console.Clear();

            CenterMap();

            foreach (LevelEntity entity in entities)
            {
                // Only render walls the player has discovered
                if (entity is Wall wall)
                {
                    if (VisionSystem.IsWallVisible(wall.Position))
                    {
                        wall.RenderEntity();
                    }
                }
                else if (entity is Enemy enemy)
                {
                    bool isVisible = VisionSystem.IsEntityWithinVisionRange(player.Position, enemy.Position);

                    if (isVisible)
                    {
                        enemy.RenderEntity();
                    }
                }
            }

            player.RenderEntity();
        }
    }
}
