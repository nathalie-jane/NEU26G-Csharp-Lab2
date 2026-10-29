using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Levels
{
    // Level data for tracking game entities and player start position on the map
    internal class LevelData
    {
        private List<LevelEntity> entities = new();
        public IReadOnlyList<LevelEntity> Entities => entities;

        public Position? PlayerStartPosition { get; private set; }

        
        public void LoadLevel(string filename)
        {
            using (StreamReader reader = new(filename))
            {
                int mapRowIndex = 0;

                string? line = reader.ReadLine();

                // Read and process each row from level map file
                while (line is not null)
                {
                    // Read and process each column to identify map symbols and create corresponding game entities
                    for (int mapColumnIndex = 0; mapColumnIndex < line.Length; mapColumnIndex++)
                    {
                        char displaySymbol = line[mapColumnIndex];

                        Position position = new(mapColumnIndex, mapRowIndex);

                        switch (displaySymbol)
                        {
                            case '#':
                                Wall wall = new(position);
                                entities.Add(wall);

                                break;

                            case '@':
                                PlayerStartPosition = position; // Set start position for player based on level map file

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

                    line = reader.ReadLine();
                }
            }
        }
    }
}
