namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using WinUiApp.Data.Data;

    public class DrinkDisplayItem
    {
        public DrinkDisplayItem(Drink drink, float averageReviewScore)
        {
            this.Drink = drink ?? throw new ArgumentNullException(nameof(drink), "Drink cannot be null.");
            this.AverageReviewScore = averageReviewScore;
        }

        public Drink Drink { get; }

        public float AverageReviewScore { get; }
    }
}