using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Characters;
using DungeonCrawler.Entities.Characters.Enemies;
using DungeonCrawler.Levels;
using DungeonCrawler.Systems.Combat;
using DungeonCrawler.Systems.Movement;
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
        private readonly HUD headsUpDisplay;
        
        // Initialize level data and HUD
        public Game()
        {
            levelData = new();
            headsUpDisplay = new();
        }

        // Game loop
        public void RunGame()
        {
            SetUpLevel();

            Console.CursorVisible = false;

            RenderLevel();

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
                    if (player is not null)
                    {
                        // Store previous positions for characters to avoid re-rendering the entire level
                        Dictionary<Character, Position> previousPositions = new();
                        previousPositions.Add(player, player.Position);
                        
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
                            player.UpdatePlayerMoves();
                        }

                        MovementSystem.TryPlayerMovement(player, keyInput, levelData.Entities);

                        // End game if player is defeated
                        if (player.HealthPoints <= 0)
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
                                enemy.PerformAction(player, levelData.Entities);

                                // End game if player is defeated
                                if (player.HealthPoints <= 0)
                                {
                                    isGameRunning = false;

                                    break;
                                }
                            }
                        }

                        UpdateRenderedCharacterPositions(previousPositions);

                        headsUpDisplay.RenderHUD(player.HealthPoints, player.PlayerMoves);

                        List<string> combatLog = CombatSystem.HandleCombatLog();

                        headsUpDisplay.RenderCombatLog(combatLog);
                    }
                }
            }

            // End game if player is defeated
            if (player is not null && player.HealthPoints <= 0)
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

        // Display game level with all entities in the console window
        public void RenderLevel()
        {
            Console.Clear();

            CenterMap();

            foreach (LevelEntity entity in levelData.Entities)
            {
                entity.RenderEntity();
            }

            if (player is not null)
            {
                player.RenderEntity();

                headsUpDisplay.RenderHUD(player.HealthPoints, player.PlayerMoves);
            }
        }

        // Update character positions without re-rendering the entire level to prevent flickering in the console window
        private void UpdateRenderedCharacterPositions(Dictionary<Character, Position> previousPositions)
        {
            // Clear previous positions for characters that have moved or been defeated
            foreach (KeyValuePair<Character, Position> characterPosition in previousPositions)
            {
                Character character = characterPosition.Key;
                Position previousPosition = characterPosition.Value;

                if (previousPosition != character.Position || (character is Enemy &&  character.HealthPoints <= 0))
                {
                    Console.SetCursorPosition((previousPosition.XPosition + LevelEntity.CenterMapXAxis), (previousPosition.YPosition + LevelEntity.CenterMapYAxis));
                    Console.Write(' ');
                }
            }

            // Render characters that have moved to new positions
            foreach (KeyValuePair<Character, Position> characterPosition in previousPositions)
            {
                Character character = characterPosition.Key;
                Position previousPosition = characterPosition.Value;

                // Prevent defeated enemies from being re-rendered
                if (character is Enemy && character.HealthPoints <= 0)
                {
                    continue;
                }

                // Render character only if its position has changed
                if (previousPosition != character.Position)
                {
                    character.RenderEntity();
                }
            }
        }

        // Center game level map in console window
        private void CenterMap()
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
    }
}