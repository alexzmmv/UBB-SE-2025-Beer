// <copyright file="Drink.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using WinUiApp.Data.Data;

    /// <summary>
    /// Represents a drink with associated brand, image, alcohol content, and categories.
    /// </summary>
    public class DrinkDTO
    {
        private const float MAXIMUM_ALCOHOOL_CONTENT = 100.0f;
        private const int MINIMUM_ALCOHOOL_CONTENT = 0;

        private string? drinkName;
        private string drinkImageUrl = string.Empty;
        private List<Category> categoryList;
        private float alcoholContent;
        private bool isRequestingApproval = false;

        public DrinkDTO() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkDTO"/> class.
        /// </summary>
        /// <param name="id">Unique identifier for the drink.</param>
        /// <param name="drinkName">Name of the drink.</param>
        /// <param name="imageUrl">URL of the drink image.</param>
        /// <param name="categories">Categories associated with the drink.</param>
        /// <param name="brandDto">Brand of the drink.</param>
        /// <param name="alcoholContent">Alcohol content percentage.</param>
        /// <exception cref="ArgumentNullException">Thrown when brand is null.</exception>
        public DrinkDTO(int id, string? drinkName, string imageUrl, List<Category> categories, Brand brandDto, float alcoholContent, bool isRequestingApproval)
        {
            this.DrinkId = id;
            this.DrinkName = drinkName;
            this.DrinkImageUrl = imageUrl;
            this.CategoryList = categories;
            this.DrinkBrand = brandDto ?? throw new ArgumentNullException(nameof(brandDto), "Brand cannot be null");
            this.AlcoholContent = alcoholContent;
            this.IsRequestingApproval = isRequestingApproval;
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
            get => this.drinkName;
            set => this.drinkName = value;
        }

        /// <summary>
        /// Gets or sets the URL of the drink image.
        /// </summary>
        public string DrinkImageUrl
        {
            get => this.drinkImageUrl;
            set => this.drinkImageUrl = value ?? string.Empty;
        }

        /// <summary>
        /// Gets or sets the list of categories associated with the drink.
        /// </summary>
        public List<Category> CategoryList
        {
            get => this.categoryList;
            set => this.categoryList = value;
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
            get => this.alcoholContent;
            set
            {
                if (value < MINIMUM_ALCOHOOL_CONTENT)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.AlcoholContent), "Alcohol content must be a positive value.");
                }

                if (value > MAXIMUM_ALCOHOOL_CONTENT)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.AlcoholContent), $"Alcohol content must not exceed {MAXIMUM_ALCOHOOL_CONTENT}.");
                }

                this.alcoholContent = value;
            }
        }

        public bool IsRequestingApproval { get; set; } = false;
    }
}
