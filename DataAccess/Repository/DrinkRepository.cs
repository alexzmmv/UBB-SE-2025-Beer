// <copyright file="DrinkRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WinUIApp.Tests")]

namespace DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DataAccess.Data;
    using DataAccess.Extensions;
    using DataAccess.IRepository;
    using Microsoft.EntityFrameworkCore;
    using WinUiApp.Data.Data;
    using WinUiApp.Data.Interfaces;
    using WinUIApp.WebAPI.Models;

    /// <summary>
    /// Repository for managing drink-related operations.
    /// </summary>
    public class DrinkRepository : IDrinkRepository
    {
        private const int NO_CATEGORIES_COUNT = 0;
        private IAppDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkRepository"/> class.
        /// </summary>
        /// <param name="dataBaseService"> The database service. </param>
        public DrinkRepository(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves a list of all drinks.
        /// </summary>
        /// <returns> List of drinks. </returns>
        public List<DrinkDTO> GetDrinks()
        {
            return dbContext.Drinks
                .Include(drink => drink.Brand)
                .Include(drink => drink.DrinkCategories)
                .ThenInclude(drinkCategory => drinkCategory.Category)
                .Where(drink => !drink.IsRequestingApproval)
                .Select(drink => DrinkExtensions.ConvertEntityToDTO(drink))
                .ToList();
        }

        /// <summary>
        /// Retrieves a drink by its unique identifier.
        /// </summary>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> The drink. </returns>
        public virtual DrinkDTO? GetDrinkById(int drinkId)
        {
            Drink? drink = dbContext.Drinks
                .Include(drink => drink.Brand)
                .Include(drink => drink.DrinkCategories)
                    .ThenInclude(drinkCategory => drinkCategory.Category)
                .FirstOrDefault(drink => drink.DrinkId == drinkId);

            return drink != null ? DrinkExtensions.ConvertEntityToDTO(drink) : null;
        }

        public virtual Brand RetrieveBrand(string brandName)
        {
            Brand? brand = dbContext.Brands
                                    .FirstOrDefault(brand => brand.BrandName == brandName);

            if (brand == null)
            {
                Brand newBrand = new Brand { BrandName = brandName };
                dbContext.Brands.Add(newBrand);

                dbContext.SaveChanges();
                brand = dbContext.Brands
                                    .FirstOrDefault(brand => brand.BrandName == brandName);
            }

            return brand;
        }

        public virtual Category RetrieveCategory(Category currentCategoryDto)
        {
            Category? dataCategory = dbContext.Categories
                    .FirstOrDefault(category => category.CategoryId == currentCategoryDto.CategoryId);

            if (dataCategory == null)
            {
                dataCategory = dbContext.Categories
                    .FirstOrDefault(category => category.CategoryName == currentCategoryDto.CategoryName);
            }

            if (dataCategory == null)
            {
                dataCategory = new Category
                {
                    CategoryName = currentCategoryDto.CategoryName
                };
                dbContext.Categories.Add(dataCategory);
                dbContext.SaveChanges();
            }

            return dataCategory;
        }

        /// <summary>
        /// Adds a new drink to the database.
        /// </summary>
        /// <param name="drinkName"> Drink name. </param>
        /// <param name="drinkUrl"> Drink Url. </param>
        /// <param name="categories"> List of categories. </param>
        /// <param name="brandName"> Brand name. </param>
        /// <param name="alcoholContent"> Alcohol content. </param>
        ///
        public DrinkDTO AddDrink(string drinkName, string drinkUrl, List<Category> categories, string brandName, float alcoholContent, bool isDrinkRequestingApproval = false)
        {
            Brand? brand = RetrieveBrand(brandName);

            Drink drink = new Drink
            {
                DrinkName = drinkName,
                DrinkURL = drinkUrl,
                AlcoholContent = (int)alcoholContent,
                BrandId = brand.BrandId,
                IsRequestingApproval = isDrinkRequestingApproval
            };

            dbContext.Drinks.Add(drink);
            dbContext.SaveChanges();

            foreach (Category category in categories)
            {
                Category dataCategory = RetrieveCategory(category);

                DrinkCategory drinkCategory = new DrinkCategory
                {
                    DrinkId = drink.DrinkId,
                    CategoryId = dataCategory.CategoryId
                };

                dbContext.DrinkCategories.Add(drinkCategory);
            }

            dbContext.SaveChanges();

            return DrinkExtensions.ConvertEntityToDTO(drink);
        }

        /// <summary>
        /// Updates the details of an existing drink in the database.
        /// </summary>
        /// <param name="drinkDto"> The drink with updated info. </param>
        public void UpdateDrink(DrinkDTO drinkDto)
        {
            try
            {
                Brand? brand = dbContext.Brands
                                     .FirstOrDefault(brand => brand.BrandName == drinkDto.DrinkBrand.BrandName);

                if (brand == null)
                {
                    brand = new Brand { BrandName = drinkDto.DrinkBrand.BrandName };
                    dbContext.Brands.Add(brand);
                    dbContext.SaveChanges();
                }

                Drink? existingDrink = dbContext.Drinks
                                             .Include(drink => drink.DrinkCategories)
                                             .FirstOrDefault(drink => drink.DrinkId == drinkDto.DrinkId) ?? throw new Exception("No drink found with the provided ID.");

                existingDrink.DrinkName = drinkDto.DrinkName ?? string.Empty;
                existingDrink.DrinkURL = drinkDto.DrinkImageUrl;
                existingDrink.AlcoholContent = (int)drinkDto.AlcoholContent;
                existingDrink.BrandId = brand.BrandId;
                existingDrink.IsRequestingApproval = drinkDto.IsRequestingApproval;

                List<DrinkCategory> oldCategories = dbContext.DrinkCategories
                    .Where(dc => dc.DrinkId == existingDrink.DrinkId)
                    .ToList();
                dbContext.DrinkCategories.RemoveRange(oldCategories);

                // Add new DrinkCategory rows
                foreach (Category category in drinkDto.CategoryList)
                {
                    // Ensure the category exists or add it
                    var existingCategory = dbContext.Categories.Find(category.CategoryId);

                    DrinkCategory drinkCategory = new DrinkCategory
                    {
                        DrinkId = existingDrink.DrinkId,
                        CategoryId = category.CategoryId
                    };

                    dbContext.DrinkCategories.Add(drinkCategory);
                }

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Deletes a drink from the database.
        /// </summary>
        /// <param name="drinkId"> Drink id. </param>
        public void DeleteDrink(int drinkId)
        {
            Drink? drink = dbContext.Drinks
                                    .Include(drink => drink.DrinkCategories)
                                    .FirstOrDefault(drink => drink.DrinkId == drinkId);


            if (drink == null)
            {
                throw new Exception("No drink found with the provided ID.");
            }

            List<DrinkModificationRequest> requests = dbContext.DrinkModificationRequests
                .Where(req => drink.DrinkId == req.NewDrinkId || drink.DrinkId == req.OldDrinkId)
                .ToList();

            foreach (DrinkModificationRequest req in requests)
            {
                req.OldDrink = null;
                req.NewDrink = null;
                req.OldDrinkId = null;
                req.NewDrinkId = null;
                dbContext.DrinkModificationRequests.Remove(req);
            }

            dbContext.DrinkCategories.RemoveRange(drink.DrinkCategories);

            dbContext.Drinks.Remove(drink);

            dbContext.SaveChanges();
        }

        /// <summary>
        /// Retrieves the drink of the day.
        /// </summary>
        /// <returns> Drink of the day. </returns>
        public DrinkDTO GetDrinkOfTheDay()
        {
            DateTime today = DateTime.UtcNow.Date;

            bool hasToday = dbContext.DrinkOfTheDays
                .Any(drink => drink.DrinkTime.Date == today);

            if (!hasToday)
            {
                ResetDrinkOfTheDay();
            }

            DrinkOfTheDay? drinkOfTheDay = dbContext.DrinkOfTheDays
                .AsNoTracking()
                .FirstOrDefault();

            if (drinkOfTheDay == null)
            {
                throw new Exception("DrinkOfTheDay table is empty.");
            }
            DrinkDTO? drink = GetDrinkById(drinkOfTheDay.DrinkId);
            if (drink is null)
            {
                throw new Exception($"Drink with ID {drinkOfTheDay.DrinkId} not found.");
            }

            return drink;
        }

        /// <summary>
        /// Resets the Drink of the Day to the new top-voted drink for today.
        /// </summary>
        public virtual void ResetDrinkOfTheDay()
        {
            List<DrinkOfTheDay> allEntries = dbContext.DrinkOfTheDays.ToList();
            dbContext.DrinkOfTheDays.RemoveRange(allEntries);

            int drinkId = GetCurrentTopVotedDrink();

            DrinkOfTheDay newDotd = new DrinkOfTheDay
            {
                DrinkId = drinkId,
                DrinkTime = DateTime.UtcNow
            };
            dbContext.DrinkOfTheDays.Add(newDotd);

            dbContext.SaveChanges();
        }

        /// <summary>
        /// Adds a new vote entry for the specified user and drink at the given time.
        /// </summary>
        /// <param name="userId">The unique ID of the user casting the vote.</param>
        /// <param name="drinkId">The ID of the drink being voted for.</param>
        /// <param name="voteTime">The timestamp when the vote is cast.</param>
        public void AddNewVote(Guid userId, int drinkId, DateTime voteTime)
        {
            Vote newVote = new Vote
            {
                UserId = userId,
                DrinkId = drinkId,
                VoteTime = voteTime
            };
            dbContext.Votes.Add(newVote);

            dbContext.SaveChanges();
        }

        /// <summary>
        /// Updates an existing vote to associate it with a new drink.
        /// </summary>
        /// <param name="existingVote">The existing vote record to update.</param>
        /// <param name="drinkId">The new drink ID to associate with the vote.</param>
        public void UpdateExistingVote(Vote existingVote, int drinkId)
        {
            existingVote.DrinkId = drinkId;
            dbContext.Votes.Update(existingVote);

            dbContext.SaveChanges();
        }

        /// <summary>
        /// Votes for a drink of the day.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        public void VoteDrinkOfTheDay(Guid userId, int drinkId)
        {
            DateTime voteTime = DateTime.UtcNow;

            Vote? existingVote = dbContext.Votes
                .FirstOrDefault(vote => vote.UserId == userId && vote.VoteTime.Date == voteTime.Date);

            if (existingVote == null)
            {
                AddNewVote(userId, drinkId, voteTime);
            }
            else
            {
                UpdateExistingVote(existingVote, drinkId);
            }

            dbContext.SaveChanges();
        }

        /// <summary>
        /// Retrieves a list of drinks based on the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <returns> The list of drinks for the user. </returns>
        public List<DrinkDTO> GetPersonalDrinkList(Guid userId)
        {
            List<int> drinkIds = dbContext.UserDrinks
            .Where(ud => ud.UserId == userId)
            .Select(ud => ud.DrinkId)
            .ToList();

            if (!drinkIds.Any())
            {
                return new List<DrinkDTO>();
            }

            List<Drink> drinkEntities = dbContext.Drinks
                .Include(drink => drink.Brand)
                .Include(drink => drink.DrinkCategories)
                .ThenInclude(drinkCategory => drinkCategory.Category)
                .Where(drink => drinkIds.Contains(drink.DrinkId) && !drink.IsRequestingApproval) // Only approved drinks
                .AsNoTracking()
                .ToList(); // materialize before projection
            List<DrinkDTO> drinks = drinkEntities.Select(drink => DrinkExtensions.ConvertEntityToDTO(drink))
                .ToList();

            return drinks;
        }

        /// <summary>
        /// Checks if a drink is already in the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if it is in the list, false otherwise. </returns>
        public bool IsDrinkInPersonalList(Guid userId, int drinkId)
        {
            return dbContext.UserDrinks
                .Any(userDrink =>
                    userDrink.UserId == userId &&
                    userDrink.DrinkId == drinkId);
        }

        /// <summary>
        /// Adds a drink to the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if successfull, false otherwise. </returns>
        public bool AddToPersonalDrinkList(Guid userId, int drinkId)
        {
            bool alreadyExists = dbContext.UserDrinks
            .Any(userDrink => userDrink.UserId == userId && userDrink.DrinkId == drinkId);
            if (alreadyExists)
            {
                return true;
            }

            UserDrink userDrink = new UserDrink
            {
                UserId = userId,
                DrinkId = drinkId
            };
            dbContext.UserDrinks.Add(userDrink);
            dbContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Deletes a drink from the user's personal drink list.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <param name="drinkId"> Drink id. </param>
        /// <returns> True, if successfull, false otherwise. </returns>
        public bool DeleteFromPersonalDrinkList(Guid userId, int drinkId)
        {
            UserDrink? userDrink = dbContext.UserDrinks
           .FirstOrDefault(userDrink => userDrink.UserId == userId && userDrink.DrinkId == drinkId);

            if (userDrink == null)
            {
                return true;
            }

            dbContext.UserDrinks.Remove(userDrink);
            dbContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Retrieves the current top voted drink.
        /// </summary>
        /// <returns> Id of the current top voted drink. </returns>
        public virtual int GetCurrentTopVotedDrink()
        {
            DateTime voteTime = DateTime.UtcNow.Date.AddDays(-1);

            var topVotedDrink = dbContext.Votes
                .Where(vote => vote.VoteTime >= voteTime)
                .Join(dbContext.Drinks, vote => vote.DrinkId, drink => drink.DrinkId, (vote, drink) => new { vote, drink })
                .Where(vd => !vd.drink.IsRequestingApproval) // Only consider approved drinks
                .GroupBy(vd => vd.vote.DrinkId)
                .Select(drinkVote => new { DrinkId = drinkVote.Key, VoteCount = drinkVote.Count() })
                .OrderByDescending(drinkVote => drinkVote.VoteCount)
                .FirstOrDefault();

            if (topVotedDrink == null)
            {
                return GetRandomDrinkId();
            }

            return topVotedDrink.DrinkId;
        }

        /// <summary>
        /// Retrieves a random drink id from the database.
        /// </summary>
        /// <returns> Random drink id. </returns>
        public virtual int GetRandomDrinkId()
        {
            Drink? randomDrink = dbContext.Drinks
            .Where(drink => !drink.IsRequestingApproval) // Only consider approved drinks
            .OrderBy(drink => Guid.NewGuid())
            .FirstOrDefault();

            if (randomDrink == null)
            {
                throw new Exception("No approved drink found in the database.");
            }

            return randomDrink.DrinkId;
        }

        /// <summary>
        /// Retrieves a list of all available drink categories.
        /// </summary>
        /// <returns> List of all categories. </returns>
        public List<Category> GetDrinkCategories()
        {
            return dbContext.Categories
            .OrderBy(category => category.CategoryId)
            .Select(category => new Category(
                category.CategoryId,
                category.CategoryName))
            .ToList();
        }

        /// <summary>
        /// Retrieves a list of all available drink brands.
        /// </summary>
        /// <returns> List of all brands. </returns>
        public List<Brand> GetDrinkBrands()
        {
            return dbContext.Brands
            .Select(brand => new Brand
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName
            })
            .ToList();
        }

        /// <summary>
        /// Checks if a drink brand is already in the database.
        /// </summary>
        /// <param name="brandName"> Brand name. </param>
        /// <returns> True, if yes, false otherwise. </returns>
        public bool IsBrandInDatabase(string brandName)
        {
            return dbContext.Brands
                .Any(brand => brand.BrandName == brandName);
        }

        /// <summary>
        /// Adds a new drink brand to the database.
        /// </summary>
        /// <param name="brandName"> Brand name. </param>
        public void AddBrand(string brandName)
        {
            Brand brand = new Brand
            {
                BrandName = brandName
            };
            dbContext.Brands.Add(brand);

            dbContext.SaveChanges();
        }
    }
}
