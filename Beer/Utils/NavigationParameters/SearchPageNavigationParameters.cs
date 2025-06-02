namespace WinUIApp.Utils.NavigationParameters
{
    using System.Collections.Generic;
    using WinUiApp.Data.Data;

    public class SearchPageNavigationParameters
    {
        public List<Category>? SelectedCategoryFilters { get; set; }

        public string? InputSearchKeyword { get; set; }
    }
}