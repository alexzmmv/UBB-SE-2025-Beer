namespace WinUIApp.Utils.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.UI.Xaml.Data;
    using WinUiApp.Data.Data;

    public class CategoriesConverter : IValueConverter
    {
        private const string DEFAULT_DRINK_CATEGORIES_DISPLAY = "N/A";
        private const string DRINK_CATEGORIES_SEPARATOR = ", ";

        public object Convert(object drinkCategoriesSourceValue, Type destinationType, object converterParameter, string formattingCulture)
        {
            if (drinkCategoriesSourceValue is List<Category> drinkCategories && drinkCategories.Count > 0)
            {
                return string.Join(CategoriesConverter.DRINK_CATEGORIES_SEPARATOR, drinkCategories.Select(category => category.CategoryName));
            }

            return CategoriesConverter.DEFAULT_DRINK_CATEGORIES_DISPLAY;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)

        {
            throw new NotImplementedException("Converting from a formatted categories string back to a list of Category objects is not supported.");
        }
    }
}