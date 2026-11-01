using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Characters.Enemies;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Levels
{
    // Store and manage game entities from level file
    internal class LevelData
    {
        private readonly List<LevelEntity> entities = new(); // Store references to all game entities created from level file
        public IReadOnlyList<LevelEntity> Entities => entities; // Make entities list read-only to prevent external modification
        public Position? PlayerStartPosition { get; private set; } // Store starting position when the correct symbol is found in the level file

        // Load game entities and starting position for player from level file
        public void LoadLevel(string filename)
        {
            using (StreamReader reader = new(filename))
            {
                int mapRowIndex = 0;

                string? currentRow = reader.ReadLine();

                // Read and process each row from level file
                while (currentRow is not null)
                {
                    // Read and process each column to identify map symbols and create corresponding game entities
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
                                PlayerStartPosition = position; // Set starting position for player based on level file

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
    }
}
