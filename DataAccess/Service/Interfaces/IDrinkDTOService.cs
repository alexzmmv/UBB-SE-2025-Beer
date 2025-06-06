﻿// <copyright file="IDrinkService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.WebAPI.Services
{
    using System;
    using System.Collections.Generic;
    using WinUiApp.Data.Data;
    using WinUIApp.WebAPI.Models;

    /// <summary>
    /// Interface for managing drink-related operations.
    /// </summary>
    public interface IDrinkDTOService
    {
        /// <summary>
        /// Adds a drink to the database.
        /// </summary>
        /// <param name="inputtedDrinkName"> Name. </param>
        /// <param name="inputtedDrinkPath"> ImagePath. </param>
        /// <param name="inputtedDrinkCategories"> Categories. </param>
        /// <param name="inputtedDrinkBrandName"> Brand. </param>
        /// <param name="inputtedAlcoholPercentage"> Alcohol. </param>
        /// <exception cref="Exception"> Any issues. </exception>
        void AddDrink(string inputtedDrinkName, string inputtedDrinkPath, List<Category> inputtedDrinkCategories, string inputtedDrinkBrandName, float inputtedAlcoholPercentage);

        /// <summary>
        /// Adds a drink to the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if successfull, false otherwise. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        bool AddToUserPersonalDrinkList(Guid userId, int drinkId);

        /// <summary>
        /// Deletes a drink from the database.
        /// </summary>
        /// <param name="drinkId"> Drink id. </param>
        /// <exception cref="Exception"> Any issues. </exception>
        void DeleteDrink(int drinkId);

        /// <summary>
        /// Deletes a drink from the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if successfull, false otherwise. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        bool DeleteFromUserPersonalDrinkList(Guid userId, int drinkId);

        /// <summary>
        /// Retrieves a list of drink brands.
        /// </summary>
        /// <returns> List of brands. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        List<Brand> GetDrinkBrandNames();

        /// <summary>
        /// Gets the drink by ID.
        /// </summary>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> The drink. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        DrinkDTO? GetDrinkById(int drinkId);

        /// <summary>
        /// Retrieves a random drink ID from the database.
        /// </summary>
        /// <returns> List of categories. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        List<Category> GetDrinkCategories();

        /// <summary>
        /// Retrieves the drink of the day.
        /// </summary>
        /// <returns> The drink. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        DrinkDTO GetDrinkOfTheDay();

        /// <summary>
        /// Gets drinks based on various filters and ordering criteria.
        /// </summary>
        /// <param name="searchKeyword"> search term. </param>
        /// <param name="drinkBrandNameFilter"> brand filter. </param>
        /// <param name="drinkCategoryFilter"> category filter. </param>
        /// <param name="minimumAlcoholPercentage"> minimum alcohol content. </param>
        /// <param name="maximumAlcoholPercentage"> maximum alcohol content. </param>
        /// <param name="orderingCriteria"> order criteria. </param>
        /// <returns> List of drinks. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        List<DrinkDTO> GetDrinks(string? searchKeyword, List<string>? drinkBrandNameFilter, List<string>? drinkCategoryFilter, float? minimumAlcoholPercentage, float? maximumAlcoholPercentage, Dictionary<string, bool>? orderingCriteria);

        /// <summary>
        /// Retrieves a random drink ID from the database.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="maximumDrinkCount"> Not sure. </param>
        /// <returns> Personal list. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        List<DrinkDTO> GetUserPersonalDrinkList(Guid userId, int maximumDrinkCount = 1);

        /// <summary>
        /// Checks if a drink is already in the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> true, if yes, false otherwise. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        bool IsDrinkInUserPersonalList(Guid userId, int drinkId);

        /// <summary>
        /// Updates a drink in the database.
        /// </summary>
        /// <param name="drinkDto"> Drink. </param>
        /// <exception cref="Exception"> Any issues. </exception>
        void UpdateDrink(DrinkDTO drinkDto);

        /// <summary>
        /// Votes for the drink of the day.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> The drink. </returns>
        /// <exception cref="Exception"> Any issues. </exception>
        DrinkDTO VoteDrinkOfTheDay(Guid userId, int drinkId);
    }
}
