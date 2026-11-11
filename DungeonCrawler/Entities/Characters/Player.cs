using DungeonCrawler.Coordinates;
using DungeonCrawler.Systems.Combat;

namespace DungeonCrawler.Entities.Characters
{
    internal class Player : Character
    {
        public Player(Position position)
            : base(position, '@', ConsoleColor.Cyan, 100, attack: new DiceConfiguration(2, 6, 2), defense: new DiceConfiguration(2, 6, 0)) { }
        public int PlayerMoves { get; set; } = 0;

        public void UpdatePosition(Position newPosition)
        {
            Position = newPosition;
        }

        public void UpdatePlayerMoves()
        {
            PlayerMoves++;
        }
    }
}
