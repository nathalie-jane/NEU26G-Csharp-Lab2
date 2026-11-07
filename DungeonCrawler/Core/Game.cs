using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Characters;
using DungeonCrawler.Entities.Characters.Enemies;
using DungeonCrawler.Levels;
using DungeonCrawler.Systems.Movement;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Core
{
    internal class Game
    {
        private readonly LevelData levelData;
        private Player? player;
        
        // Initialize level data
        public Game()
        {
            levelData = new();
        }

        public void RunGame()
        {
            SetUpLevel();

            Console.CursorVisible = false;

            RenderLevel();

            bool isGameRunning = true;

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

                        MovementSystem.TryPlayerMovement(player, keyInput, levelData.Entities);

                        // Update enemy positions after player movement
                        foreach (LevelEntity entity in levelData.Entities)
                        {
                            if (entity is Enemy enemy)
                            {
                                enemy.PerformAction(player.Position, levelData.Entities);
                            }
                        }

                        UpdateRenderedCharacterPositions(previousPositions);
                    }
                }
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

            foreach (LevelEntity entity in levelData.Entities)
            {
                entity.RenderEntity();
            }

            if (player is not null)
            {
                player.RenderEntity();
            }
        }

        // Update character positions without re-rendering the entire level to prevent flickering in the console window
        private void UpdateRenderedCharacterPositions(Dictionary<Character, Position> previousPositions)
        {
            // Clear old positions before rendering new ones
            foreach (KeyValuePair<Character, Position> characterPosition in previousPositions)
            {
                Character character = characterPosition.Key;
                Position previousPosition = characterPosition.Value;

                if (previousPosition != character.Position)
                {
                    Console.SetCursorPosition(previousPosition.XPosition, previousPosition.YPosition);
                    Console.Write(' ');
                }
            }

            // Render characters at their new positions
            foreach (KeyValuePair<Character, Position> characterPosition in previousPositions)
            {
                Character character = characterPosition.Key;
                Position previousPosition = characterPosition.Value;

                if (previousPosition != character.Position)
                {
                    character.RenderEntity();
                }
            }
        }
    }
}