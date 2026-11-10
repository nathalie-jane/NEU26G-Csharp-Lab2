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
        private readonly HUD hud;
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
        
        // Initialize level data and HUD
        public Game()
        {
            levelData = new();
            hud = new();
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
                    RenderVisibleWalls(previousPositions[ActivePlayer]);

                    UpdateRenderedCharacterPositions(previousPositions);

                    HUD.RenderHUD(ActivePlayer.HealthPoints, ActivePlayer.PlayerMoves);

                    List<string> combatLog = CombatSystem.HandleCombatLog();
                    HUD.RenderCombatLog(combatLog);
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

        // Render discovered walls and enemies within player's vision range
        public void RenderLevel()
        {
            Console.Clear();

            CenterMap();

            VisionSystem.UpdateDiscoveredWalls(ActivePlayer.Position, levelData.Entities);

            foreach (LevelEntity entity in levelData.Entities)
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
                    bool isVisible = VisionSystem.IsEntityWithinVisionRange(ActivePlayer.Position, enemy.Position);

                    if (isVisible)
                    {
                        enemy.RenderEntity();
                    }
                }
            }

            ActivePlayer.RenderEntity();

            HUD.RenderHUD(ActivePlayer.HealthPoints, ActivePlayer.PlayerMoves);
        }

        // Update character positions without re-rendering the entire level to prevent flickering in the console window
        private void UpdateRenderedCharacterPositions(Dictionary<Character, Position> previousPositions)
        {

            Position previousPlayerPosition = previousPositions[ActivePlayer];

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
                    bool isHidden = !VisionSystem.IsEntityWithinVisionRange(ActivePlayer.Position, enemy.Position);

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
                    bool isVisible = VisionSystem.IsEntityWithinVisionRange(ActivePlayer.Position, enemy.Position);
                    bool wasHidden = !VisionSystem.IsEntityWithinVisionRange(previousPlayerPosition, previousPosition);

                    if (isVisible && (hasMoved || wasHidden))
                    {
                        enemy.RenderEntity();
                    }
                }
            }
        }

        // Render walls that have entered player's vision range
        private void RenderVisibleWalls(Position previousPlayerPosition)
        {
            foreach (LevelEntity entity in levelData.Entities)
            {
                // Check if wall has entered player's vision range
                if (entity is Wall wall)
                {
                    bool isVisible = VisionSystem.IsEntityWithinVisionRange(ActivePlayer.Position, wall.Position);
                    bool wasHidden = !VisionSystem.IsEntityWithinVisionRange(previousPlayerPosition, wall.Position);

                    if (isVisible && wasHidden)
                    {
                        wall.RenderEntity();
                    }
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