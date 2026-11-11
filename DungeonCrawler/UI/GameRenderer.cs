using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Characters;
using DungeonCrawler.Entities.Characters.Enemies;
using DungeonCrawler.Systems.Vision;

namespace DungeonCrawler.UI
{
    internal class GameRenderer
    {
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

        public static void RenderVisibleWalls(Position playerPosition, Position previousPlayerPosition, IReadOnlyList<LevelEntity> entities)
        {
            foreach (LevelEntity entity in entities)
            {
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

        public static void UpdateRenderedCharacterPositions(Player player, Dictionary<Character, Position> previousPositions)
        {

            Position previousPlayerPosition = previousPositions[player];

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

            foreach (KeyValuePair<Character, Position> characterPosition in previousPositions)
            {
                Character character = characterPosition.Key;
                Position previousPosition = characterPosition.Value;
                bool hasMoved = previousPosition != character.Position;

                if (character is Enemy && character.HealthPoints <= 0)
                {
                    continue;
                }

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

        public static void RenderLevel(Player player, IReadOnlyList<LevelEntity> entities)
        {
            Console.Clear();

            CenterMap();

            foreach (LevelEntity entity in entities)
            {
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
