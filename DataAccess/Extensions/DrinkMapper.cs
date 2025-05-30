namespace DataAccess.Extensions
{
    using WinUiApp.Data.Data;
    using WinUIApp.WebAPI.Models;

    public static class DrinkMapper
    {
        public static DrinkDTO ToDTO(Drink drink)
        {
            return new DrinkDTO
            {
                DrinkId = drink.DrinkId,
                DrinkName = drink.DrinkName,
                DrinkImageUrl = drink.DrinkURL,
                CategoryList = drink.DrinkCategories
                    .Select(drinkCategory => new Category
                    {
                        CategoryId = drinkCategory.Category!.CategoryId,
                        CategoryName = drinkCategory.Category.CategoryName
                    })
                    .ToList(),
                DrinkBrand = new Brand
                {
                    BrandId = drink.Brand!.BrandId,
                    BrandName = drink.Brand.BrandName
                },
                AlcoholContent = (float)drink.AlcoholContent
            };
        }
    }
}
