using DungeonCrawler.Entities;

namespace DungeonCrawler.UI
{
    internal class HUD
    {
        public static void RenderHUD(int healthPoints, int playerMoves)
        {
            int hudXPosition = LevelEntity.CenterMapXAxis;
            int hudYPosition = LevelEntity.CenterMapYAxis + 19;

            // Set spacing and alignment for player stats
            string spacingXAxis = new(' ', 12);
            const int textAlignment = -3;

            Console.SetCursorPosition(hudXPosition, hudYPosition);

            Console.Write($"HP: {healthPoints,textAlignment} {spacingXAxis}Moves: {playerMoves}");
        }

        public static void RenderCombatLog(IReadOnlyList<string> combatLog)
        {
            int combatLogXPosition = LevelEntity.CenterMapXAxis;
            int combatLogYPosition = LevelEntity.CenterMapYAxis + 21;

            string spacingXAxis = new(' ', 53);
            const int spacingYAxis = 1;

            // Clear previous events before displaying new ones
            Console.SetCursorPosition(combatLogXPosition, combatLogYPosition);
            Console.Write(spacingXAxis);

            Console.SetCursorPosition(combatLogXPosition, combatLogYPosition + spacingYAxis);
            Console.Write(spacingXAxis);

            int totalLogEntries = combatLog.Count;
            const int maxEntries = 2;
            int logStartIndex = totalLogEntries - maxEntries;

            if (logStartIndex < 0)
            {
                logStartIndex = 0;
            }

            for (int combatLogIndex = logStartIndex; combatLogIndex < combatLog.Count; combatLogIndex++)
            {
                Console.SetCursorPosition(combatLogXPosition, combatLogYPosition);

                Console.Write(combatLog[combatLogIndex]);

                combatLogYPosition += spacingYAxis;
            }
        }
    }
}
