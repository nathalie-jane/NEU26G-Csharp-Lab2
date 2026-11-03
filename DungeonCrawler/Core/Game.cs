using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Characters;
using DungeonCrawler.Levels;
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

        // Load game level and create starting position for player
        public void SetUpLevel()
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, "Levels", "Maps", "Level1.txt");

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

        // Display all game entities on the console
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