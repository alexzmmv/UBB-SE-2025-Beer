namespace WinUIApp.Views.Components.SearchPageComponents
{
    using System;
    using WinUiApp.Data.Data;
    using WinUIApp.WebAPI.Models;

    public class DrinkDisplayItem
    {
        public DrinkDisplayItem(DrinkDTO drink, float averageReviewScore)
        {
            this.Drink = drink ?? throw new ArgumentNullException(nameof(drink), "Drink cannot be null.");
            this.AverageReviewScore = averageReviewScore;
        }

        public DrinkDTO Drink { get; }

        public float AverageReviewScore { get; }
    }
}