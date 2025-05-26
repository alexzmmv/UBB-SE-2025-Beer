// <copyright file="IDrinkRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.WebAPI.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WinUIApp.WebAPI.Models;

    /// <summary>
    /// Interface for the Drink repository.
    /// </summary>
    public interface IDrinkRepository
    {
        /// <summary>
        /// Retrieves a list of all drinks.
        /// </summary>
        /// <returns> List of drinks. </returns>
        List<DrinkDTO> GetDrinks();

        /// <summary>
        /// Retrieves a drink by its unique identifier.
        /// </summary>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> The drink. </returns>
        DrinkDTO? GetDrinkById(int drinkId);

        /// <summary>
        /// Adds a new drink to the database.
        /// </summary>
        /// <param name="drinkName"> Drink name. </param>
        /// <param name="drinkUrl"> Drink Url. </param>
        /// <param name="categories"> List of categories. </param>
        /// <param name="brandName"> Brand name. </param>
        /// <param name="alcoholContent"> Alcohol content. </param>
        void AddDrink(string drinkName, string drinkUrl, List<CategoryDTO> categories, string brandName, float alcoholContent);

        /// <summary>
        /// Updates the details of an existing drink in the database.
        /// </summary>
        /// <param name="drinkDto"> The drink with updated info. </param>
        void UpdateDrink(DrinkDTO drinkDto);

        /// <summary>
        /// Deletes a drink from the database.
        /// </summary>
        /// <param name="drinkId"> Drink id. </param>
        void DeleteDrink(int drinkId);

        /// <summary>
        /// Retrieves the drink of the day.
        /// </summary>
        /// <returns> Drink of the day. </returns>
        DrinkDTO GetDrinkOfTheDay();

        /// <summary>
        /// Resets the Drink of the Day to the new top-voted drink for today.
        /// </summary>
        void ResetDrinkOfTheDay();

        /// <summary>
        /// Votes for a drink of the day.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        void VoteDrinkOfTheDay(int userId, int drinkId);

        /// <summary>
        /// Retrieves a list of drinks based on the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <returns> The list of drinks for the user. </returns>
        List<DrinkDTO> GetPersonalDrinkList(int userId);

        /// <summary>
        /// Checks if a drink is already in the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if it is in the list, false otherwise. </returns>
        bool IsDrinkInPersonalList(int userId, int drinkId);

        /// <summary>
        /// Adds a drink to the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if successfull, false otherwise. </returns>
        bool AddToPersonalDrinkList(int userId, int drinkId);

        /// <summary>
        /// Deletes a drink from the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if successfull, false otherwise. </returns>
        bool DeleteFromPersonalDrinkList(int userId, int drinkId);

        /// <summary>
        /// Retrieves the current top voted drink.
        /// </summary>
        /// <returns> Id of the current top voted drink. </returns>
        int GetCurrentTopVotedDrink();

        /// <summary>
        /// Retrieves a random drink id from the database.
        /// </summary>
        /// <returns> Random drink id. </returns>
        int GetRandomDrinkId();

        /// <summary>
        /// Retrieves a list of all available drink categories.
        /// </summary>
        /// <returns> List of all categories. </returns>
        List<CategoryDTO> GetDrinkCategories();

        /// <summary>
        /// Retrieves a list of drink categories by drink id.
        /// </summary>
        /// <param name="drinkId"> Id of the drink. </param>
        /// <returns> Categories for the specific drink. </returns>
        List<CategoryDTO> GetDrinkCategoriesById(int drinkId);

        /// <summary>
        /// Retrieves a list of all available drink brands.
        /// </summary>
        /// <returns> List of all brands. </returns>
        List<BrandDTO> GetDrinkBrands();

        /// <summary>
        /// Retrieves the drink brand for the drink id.
        /// </summary>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> Brand. </returns>
        BrandDTO GetBrandById(int drinkId);

        /// <summary>
        /// Checks if a drink brand is already in the database.
        /// </summary>
        /// <param name="brandName"> Brand name. </param>
        /// <returns> True, if yes, false otherwise. </returns>
        bool IsBrandInDatabase(string brandName);

        /// <summary>
        /// Adds a new drink brand to the database.
        /// </summary>
        /// <param name="brandName"> Brand name. </param>
        void AddBrand(string brandName);
    }
}
