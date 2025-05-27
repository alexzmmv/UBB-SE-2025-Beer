namespace WinUIApp.WebAPI.Repositories
{
    using System.Collections.Generic;
    using WinUIApp.WebAPI.Models;

    public interface IDrinkRepository
    {
        List<DrinkDTO> GetDrinks();

        DrinkDTO? GetDrinkById(int drinkId);

        void AddDrink(string drinkName, string drinkUrl, List<CategoryDTO> categories, string brandName, float alcoholContent);

        void UpdateDrink(DrinkDTO drinkDto);

        void DeleteDrink(int drinkId);

        DrinkDTO GetDrinkOfTheDay();

        void ResetDrinkOfTheDay();

        void VoteDrinkOfTheDay(Guid userId, int drinkId);

        List<DrinkDTO> GetPersonalDrinkList(Guid userId);

        bool IsDrinkInPersonalList(Guid userId, int drinkId);

        bool AddToPersonalDrinkList(Guid userId, int drinkId);

        bool DeleteFromPersonalDrinkList(Guid userId, int drinkId);

        int GetCurrentTopVotedDrink();

        int GetRandomDrinkId();

        List<CategoryDTO> GetDrinkCategories();

        List<CategoryDTO> GetDrinkCategoriesById(int drinkId);

        List<BrandDTO> GetDrinkBrands();

        BrandDTO GetBrandById(int drinkId);

        bool IsBrandInDatabase(string brandName);

        void AddBrand(string brandName);
    }
}
