namespace DungeonCrawler.Systems.Combat
{
    internal class DiceConfiguration
    {
        public int Quantity { get; }
        public int Sides { get; }
        public int Modifier { get; }

        public DiceConfiguration(int quantity, int sides, int modifier)
        {
            Quantity = quantity;
            Sides = sides;
            Modifier = modifier;
        }

        public int RollDice()
        {
            int diceSum = 0;

            for (int diceRollCount = 0; diceRollCount < Quantity; diceRollCount++)
            {
                int diceRollValue = Random.Shared.Next(1, Sides + 1);

                diceSum += diceRollValue;
            }

            diceSum += Modifier;

            return diceSum;
        }

        public override string ToString()
        {
            string diceNotation = $"{Quantity}d{Sides}+{Modifier}";

            return diceNotation;
        }
    }
}
