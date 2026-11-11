using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Characters;
using DungeonCrawler.Entities.Characters.Enemies;
using DungeonCrawler.Levels;
using DungeonCrawler.Systems.Combat;
using DungeonCrawler.Systems.Movement;
using DungeonCrawler.Systems.Vision;
using DungeonCrawler.UI;

namespace DungeonCrawler.Core
{
    internal class Game
    {
        public readonly LevelData levelData;
        public Player? player;
        
        public Player ActivePlayer
        {
            get
            {
                if (player is null)
                {
                    throw new InvalidOperationException("Active player not found.");
                }

                return player;
            }
        }
        
        public Game()
        {
            levelData = new();
        }

        public void RunGame()
        {
            SetUpLevel();

            Console.CursorVisible = false;

            VisionSystem.UpdateDiscoveredWalls(ActivePlayer.Position, levelData.Entities);

            GameRenderer.RenderLevel(ActivePlayer, levelData.Entities);

            HUD.RenderHUD(ActivePlayer.HealthPoints, ActivePlayer.PlayerMoves);

            bool isGameRunning = true;

            while (isGameRunning)
            {
                ConsoleKey keyInput = MovementSystem.GetKeyInput();

                if (keyInput is ConsoleKey.Escape)
                {
                    isGameRunning = false;
                }
                else
                {
                    Dictionary<Character, Position> previousPositions = new();
                    previousPositions.Add(ActivePlayer, ActivePlayer.Position);
                        
                    foreach (LevelEntity entity in levelData.Entities)
                    {
                        if (entity is Enemy enemy)
                        {
                            previousPositions.Add(enemy, enemy.Position);
                        }
                    }

                    if (MovementSystem.IsKeyInputValidMovement(keyInput))
                    {
                        ActivePlayer.UpdatePlayerMoves();
                    }

                    MovementSystem.TryPlayerMovement(ActivePlayer, keyInput, levelData.Entities);

                    if (ActivePlayer.HealthPoints <= 0)
                    {
                        isGameRunning = false;

                        break;
                    }

                    levelData.RemoveDefeatedEnemies();

                    foreach (LevelEntity entity in levelData.Entities)
                    {
                        if (entity is Enemy enemy)
                        {
                            enemy.PerformAction(ActivePlayer, levelData.Entities);

                            if (ActivePlayer.HealthPoints <= 0)
                            {
                                isGameRunning = false;

                                break;
                            }
                        }
                    }

                    VisionSystem.UpdateDiscoveredWalls(ActivePlayer.Position, levelData.Entities);
                    GameRenderer.RenderVisibleWalls(ActivePlayer.Position, previousPositions[ActivePlayer], levelData.Entities);

                    GameRenderer.UpdateRenderedCharacterPositions(ActivePlayer, previousPositions);

                    HUD.RenderHUD(ActivePlayer.HealthPoints, ActivePlayer.PlayerMoves);

                    List<string> combatLog = CombatSystem.HandleCombatLog();
                    HUD.RenderCombatLog(combatLog);
                }
            }

            if (ActivePlayer.HealthPoints <= 0)
            {
                Console.Clear();

                string gameOverText = "GAME OVER";
                int textLength = gameOverText.Length;
                int textPositionX = (Console.WindowWidth - textLength) / 2;
                int textPositionY = Console.WindowHeight / 2;

                Console.SetCursorPosition(textPositionX, textPositionY);

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(gameOverText);
                Console.ResetColor();

                MovementSystem.GetKeyInput();
            }
        }

        public void SetUpLevel()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string relativePath = Path.Combine("Levels", "Maps", "Level1.txt");
            string filePath = Path.Combine(baseDirectory, relativePath);

            levelData.LoadLevel(filePath);

            Position? playerStartPosition = levelData.PlayerStartPosition;
            
            // Prevent using default position (0, 0) if no starting position for player is found
            if (playerStartPosition is null)
            {
                throw new InvalidOperationException("Player start position was not found");
            }
            else
            {
                player = new Player(playerStartPosition.Value);
            }
        }
    }
}