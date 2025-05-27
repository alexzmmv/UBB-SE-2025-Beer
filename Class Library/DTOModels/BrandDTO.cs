// <copyright file="Brand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.WebAPI.Models
{
    using System;

    /// <summary>
    /// Represents a drink brand.
    /// </summary>
    public class BrandDTO
    {
        private string brandName;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrandDTO"/> class.
        /// </summary>
        /// <param name="brandId">Unique identifier for the brand.</param>
        /// <param name="brandName">Name of the brand.</param>
        /// <exception cref="ArgumentException">Thrown when brandName is null or empty.</exception>
        public BrandDTO(int brandId, string brandName)
        {
            this.BrandId = brandId;
            this.BrandName = brandName;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the brand.
        /// </summary>
        public int BrandId { get; set; }

        /// <summary>
        /// Gets or sets the name of the brand.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when value is null or empty.</exception>
        public string BrandName
        {
            get => this.brandName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Brand name cannot be null or empty.", nameof(this.BrandName));
                }

                this.brandName = value;
            }
        }
    }
}
