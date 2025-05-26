// <copyright file="Drink.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.ProxyServices.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a drink with associated brand, image, alcohol content, and categories.
    /// </summary>
    public class Drink
    {
        private const float MaximumAlcoholContent = 100.0f;
        private const int MinimumAlcohoolContent = 0;

        private string? drinkName;
        private string drinkImageUrl = string.Empty;
        private List<Category> categoryList;
        private float alcoholContent;

        public Drink() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Drink"/> class.
        /// </summary>
        /// <param name="id">Unique identifier for the drink.</param>
        /// <param name="drinkName">Name of the drink.</param>
        /// <param name="imageUrl">URL of the drink image.</param>
        /// <param name="categories">Categories associated with the drink.</param>
        /// <param name="brand">Brand of the drink.</param>
        /// <param name="alcoholContent">Alcohol content percentage.</param>
        /// <exception cref="ArgumentNullException">Thrown when brand is null.</exception>
        public Drink(int id, string? drinkName, string imageUrl, List<Category> categories, Brand brand, float alcoholContent)
        {
            DrinkId = id;
            DrinkName = drinkName;
            DrinkImageUrl = imageUrl;
            CategoryList = categories;
            DrinkBrand = brand ?? throw new ArgumentNullException(nameof(brand), "Brand cannot be null");
            AlcoholContent = alcoholContent;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the drink.
        /// </summary>
        public int DrinkId { get; set; }

        /// <summary>
        /// Gets or sets the name of the drink.
        /// </summary>
        public string? DrinkName
        {
            get => drinkName;
            set => drinkName = value;
        }

        /// <summary>
        /// Gets or sets the URL of the drink image.
        /// </summary>
        public string DrinkImageUrl
        {
            get => drinkImageUrl;
            set => drinkImageUrl = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the list of categories associated with the drink.
        /// </summary>
        public List<Category> CategoryList
        {
            get => categoryList;
            set => categoryList = value;
        }

        /// <summary>
        /// Gets or sets the brand of the drink.
        /// </summary>
        public Brand DrinkBrand { get; set; }

        /// <summary>
        /// Gets or sets the alcohol content of the drink as a percentage.
        /// </summary>
        public float AlcoholContent
        {
            get => alcoholContent;
            set
            {
                if (value < MinimumAlcohoolContent)
                {
                    throw new ArgumentOutOfRangeException(nameof(AlcoholContent), "Alcohol content must be a positive value.");
                }

                if (value > MaximumAlcoholContent)
                {
                    throw new ArgumentOutOfRangeException(nameof(AlcoholContent), $"Alcohol content must not exceed {MaximumAlcoholContent}.");
                }

                alcoholContent = value;
            }
        }
    }
}
