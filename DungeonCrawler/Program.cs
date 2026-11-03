using DungeonCrawler.Core;

// Test game level loading and rendering
Game game = new Game();

game.SetUpLevel();

game.RenderLevel();

Console.CursorVisible = false;
Console.ReadKey(true);