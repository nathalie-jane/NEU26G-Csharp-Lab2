using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Characters;
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

            Console.CursorVisible = false; // Hide cursor in console window

            bool isGameRunning = true;

            while (isGameRunning)
            {
                RenderLevel();

                ConsoleKey keyInput = MovementSystem.GetKeyInput();

                if (keyInput is ConsoleKey.Escape)
                {
                    isGameRunning = false;
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
    }
}