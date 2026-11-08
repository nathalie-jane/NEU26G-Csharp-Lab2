using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Characters.Enemies;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Levels
{
    // Read and process data from level file to create game entities and starting position for player
    internal class LevelData
    {
        private readonly List<LevelEntity> entities = new(); // Store references to all game entities
        public IReadOnlyList<LevelEntity> Entities => entities; // Make entities list read-only to prevent external modification
        public Position? PlayerStartPosition { get; private set; } // Store starting position when the correct symbol is found

        // Read and process data from level file
        public void LoadLevel(string filePath)
        {
            using (StreamReader reader = new(filePath))
            {
                int mapRowIndex = 0;

                string? currentRow = reader.ReadLine();

                // Process each row
                while (currentRow is not null)
                {
                    // Process each column to identify map symbols and create corresponding game entities
                    for (int mapColumnIndex = 0; mapColumnIndex < currentRow.Length; mapColumnIndex++)
                    {
                        char displaySymbol = currentRow[mapColumnIndex];

                        Position position = new(mapColumnIndex, mapRowIndex);

                        switch (displaySymbol)
                        {
                            case '#':
                                Wall wall = new(position);
                                entities.Add(wall);
                                break;

                            case '@':
                                PlayerStartPosition = position; // Set starting position for player
                                break;

                            case 'r':
                                Rat rat = new(position);
                                entities.Add(rat);
                                break;

                            case 's':
                                Snake snake = new(position);
                                entities.Add(snake);
                                break;
                        }
                    }

                    mapRowIndex++;

                    currentRow = reader.ReadLine();
                }
            }
        }

        // Remove defeated enemies from entities list
        public void RemoveDefeatedEnemies()
        {
            for (int entityListIndex = entities.Count - 1; entityListIndex >= 0; entityListIndex--)
            {
                if (entities[entityListIndex] is Enemy enemy)
                {
                    if (enemy.HealthPoints <= 0)
                    {
                        entities.RemoveAt(entityListIndex);
                    }
                }
            }
        }
    }
}
