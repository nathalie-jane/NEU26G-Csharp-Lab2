using DungeonCrawler.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonCrawler.UI
{
    // Handle HUD rendering for player stats and combat events
    internal class HUD
    {
        // Display player stats
        public void RenderHUD(int healthPoints, int playerMoves)
        {
            // Position HUD relative to game level map
            int hudXPosition = LevelEntity.CenterMapXAxis;
            int hudYPosition = LevelEntity.CenterMapYAxis + 19;

            // Set spacing and alignment for player stats
            string spacingXAxis = new(' ', 12);
            const int textAlignment = -3;

            Console.SetCursorPosition(hudXPosition, hudYPosition);

            Console.Write($"HP: {healthPoints,textAlignment} {spacingXAxis}Moves: {playerMoves}");
        }

        // Display combat events
        public void RenderCombatLog(IReadOnlyList<string> combatLog)
        {
            // Position combat events relative to game level map
            int combatLogXPosition = LevelEntity.CenterMapXAxis;
            int combatLogYPosition = LevelEntity.CenterMapYAxis + 21;

            string spacingXAxis = new(' ', 53);
            const int spacingYAxis = 1;

            // Clear previous events before displaying new ones
            Console.SetCursorPosition(combatLogXPosition, combatLogYPosition);
            Console.Write(spacingXAxis);

            Console.SetCursorPosition(combatLogXPosition, combatLogYPosition + spacingYAxis);
            Console.Write(spacingXAxis);

            // Display combat events on separate lines
            foreach (string combatEntry in combatLog)
            {
                Console.SetCursorPosition(combatLogXPosition, combatLogYPosition);

                Console.Write(combatEntry);

                combatLogYPosition += spacingYAxis;
            }
        }
    }
}
