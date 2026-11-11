namespace DungeonCrawler.Coordinates
{
    internal readonly record struct Position
    {
        public int XPosition { get; }
        public int YPosition { get; }

        public Position(int xPosition, int yPosition)
        {
            XPosition = xPosition;
            YPosition = yPosition;
        }
    }
}
