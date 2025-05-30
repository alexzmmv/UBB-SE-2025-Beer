namespace DataAccess.IRepository
{
    using System.Collections.Generic;
    using WinUiApp.Data.Data;
    using WinUIApp.WebAPI.Models;

    public interface IDrinkRepository
    {
        List<DrinkDTO> GetDrinks();

        DrinkDTO? GetDrinkById(int drinkId);

        void AddDrink(string drinkName, string drinkUrl, List<Category> categories, string brandName, float alcoholContent);

        void UpdateDrink(DrinkDTO drinkDto);

        void DeleteDrink(int drinkId);
        void DeleteRequestingApprovalDrink(int drinkId);

        DrinkDTO GetDrinkOfTheDay();

        void ResetDrinkOfTheDay();

        void VoteDrinkOfTheDay(Guid userId, int drinkId);

        List<DrinkDTO> GetPersonalDrinkList(Guid userId);

        bool IsDrinkInPersonalList(Guid userId, int drinkId);

        bool AddToPersonalDrinkList(Guid userId, int drinkId);

        bool DeleteFromPersonalDrinkList(Guid userId, int drinkId);

        int GetCurrentTopVotedDrink();

        int GetRandomDrinkId();

        List<Category> GetDrinkCategories();

        List<Category> GetDrinkCategoriesById(int drinkId);

        List<Brand> GetDrinkBrands();

        Brand GetBrandById(int drinkId);

        bool IsBrandInDatabase(string brandName);

        void AddBrand(string brandName);
    }
}
