using DungeonCrawler.Coordinates;

namespace DungeonCrawler.Entities
{
    internal abstract class LevelEntity
    {
        public Position Position { get; set; }
        public char DisplaySymbol { get; }
        public ConsoleColor DisplayColor { get; }
        public static int CenterMapXAxis { get; set; }
        public static int CenterMapYAxis { get; set; }

        public LevelEntity(Position position, char displaySymbol, ConsoleColor displayColor)
        {
            Position = position;
            DisplaySymbol = displaySymbol;
            DisplayColor = displayColor;
        }

        public void RenderEntity()
        {
            Console.SetCursorPosition((Position.XPosition + CenterMapXAxis), (Position.YPosition + CenterMapYAxis));

            Console.ForegroundColor = DisplayColor;
            Console.Write(DisplaySymbol);

            Console.ResetColor();
        }
    }
}
