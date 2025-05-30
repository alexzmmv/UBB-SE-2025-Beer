using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;

namespace DataAccess.Extensions
{
    public static class DrinkExtensions
    {
        static public Drink ConvertDTOToEntity(DrinkDTO drinkDTO)
        {
            return new Drink
            {
                DrinkId = drinkDTO.DrinkId,
                DrinkName = drinkDTO.DrinkName,
                DrinkURL = drinkDTO.DrinkImageUrl,
                BrandId = drinkDTO.DrinkBrand.BrandId,
                AlcoholContent = (decimal)drinkDTO.AlcoholContent,
                IsRequestingApproval = drinkDTO.IsRequestingApproval,
                Brand = drinkDTO.DrinkBrand,
                DrinkCategories = [.. drinkDTO.CategoryList.Select((Category c) =>
                {
                    return new DrinkCategory
                    {
                        DrinkId = drinkDTO.DrinkId,
                        CategoryId = c.CategoryId,
                        Category = c,
                        Drink = null
                    };
                })]
            };
        }

        static public DrinkDTO ConvertEntityToDTO(Drink drink)
        {
            return new DrinkDTO(
                    drink.DrinkId,
                    drink.DrinkName,
                    drink.DrinkURL,
                    (drink.DrinkCategories ?? new List<DrinkCategory>())
                        .Select(drinkCategory => new Category
                        {
                            CategoryId = drinkCategory.Category!.CategoryId,
                            CategoryName = drinkCategory.Category.CategoryName
                        })
                        .ToList(),
                    new Brand
                    {
                        BrandId = drink.Brand!.BrandId,
                        BrandName = drink.Brand.BrandName
                    },
                    (float)drink.AlcoholContent,
                    drink.IsRequestingApproval
                );
        }
    }
}
