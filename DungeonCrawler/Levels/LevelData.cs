using DungeonCrawler.Coordinates;
using DungeonCrawler.Entities;
using DungeonCrawler.Entities.Characters.Enemies;

namespace DungeonCrawler.Levels
{
    internal class LevelData
    {
        public Position? PlayerStartPosition { get; set; }
        public readonly List<LevelEntity> entities = new();
        public IReadOnlyList<LevelEntity> Entities
        {
            get
            {
                return entities;
            }
        }

        public void LoadLevel(string filePath)
        {
            using (StreamReader reader = new(filePath))
            {
                int mapRowIndex = 0;

                string? currentRow = reader.ReadLine();

                while (currentRow is not null)
                {
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
                                PlayerStartPosition = position;

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
