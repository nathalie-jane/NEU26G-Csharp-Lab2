using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.Levels
{
    internal class LevelData
    {
        private List<LevelElement> elements = new();

        public IReadOnlyList<LevelElement> Elements => elements;

        public void LoadLevel(string filename)
        {
            using (StreamReader reader = new(filename))
            {
                int mapRowIndex = 0;

                string? line = reader.ReadLine();

                while (line is not null)
                {
                    for (int mapColumnIndex = 0; mapColumnIndex < line.Length; mapColumnIndex++)
                    {
                        char displaySymbol = line[mapColumnIndex];

                        Position position = new(mapColumnIndex, mapRowIndex);
                    }

                    mapRowIndex++;

                    line = reader.ReadLine();
                }
            }
        }
    }
}
