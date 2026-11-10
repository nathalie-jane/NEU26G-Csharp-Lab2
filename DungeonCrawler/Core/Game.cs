using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Characters;
using DungeonCrawler.Entities.Characters.Enemies;
using DungeonCrawler.Levels;
using DungeonCrawler.Systems.Combat;
using DungeonCrawler.Systems.Movement;
using DungeonCrawler.Systems.Vision;
using DungeonCrawler.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Core
{
    // Handle game loop
    internal class Game
    {
        private readonly LevelData levelData;
        private Player? player;
        
        private Player ActivePlayer
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
        
        // Initialize level data
        public Game()
        {
            levelData = new();
        }

        // Game loop
        public void RunGame()
        {
            SetUpLevel();

            Console.CursorVisible = false;

            VisionSystem.UpdateDiscoveredWalls(ActivePlayer.Position, levelData.Entities);

            GameRenderer.RenderLevel(ActivePlayer, levelData.Entities);

            HUD.RenderHUD(ActivePlayer.HealthPoints, ActivePlayer.PlayerMoves);

            bool isGameRunning = true;

            // Process key input for player movement and update game state
            while (isGameRunning)
            {
                ConsoleKey keyInput = MovementSystem.GetKeyInput();

                if (keyInput is ConsoleKey.Escape) // Exit game if Esc key is pressed
                {
                    isGameRunning = false;
                }
                else
                {
                    // Store previous positions for characters to avoid re-rendering the entire level
                    Dictionary<Character, Position> previousPositions = new();
                    previousPositions.Add(ActivePlayer, ActivePlayer.Position);
                        
                    foreach (LevelEntity entity in levelData.Entities)
                    {
                        if (entity is Enemy enemy)
                        {
                            previousPositions.Add(enemy, enemy.Position);
                        }
                    }

                    // Update movement count for player
                    if (MovementSystem.IsKeyInputValidMovement(keyInput))
                    {
                        ActivePlayer.UpdatePlayerMoves();
                    }

                    MovementSystem.TryPlayerMovement(ActivePlayer, keyInput, levelData.Entities);

                    // End game if player is defeated
                    if (ActivePlayer.HealthPoints <= 0)
                    {
                        isGameRunning = false;

                        break;
                    }

                    levelData.RemoveDefeatedEnemies();

                    // Update enemy positions after player movement
                    foreach (LevelEntity entity in levelData.Entities)
                    {
                        if (entity is Enemy enemy)
                        {
                            enemy.PerformAction(ActivePlayer, levelData.Entities);

                            // End game if player is defeated
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

            // End game if player is defeated
            if (ActivePlayer.HealthPoints <= 0)
            {
                Console.Clear();
                Console.WriteLine("Game Over");
            }
        }

        // Load game level and create starting position for player
        public void SetUpLevel()
        {
            // Create file path to level file by combining application directory with relative path
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