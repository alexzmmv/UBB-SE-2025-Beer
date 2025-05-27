namespace WinUIApp.Views.ViewModels
{
    using DataAccess.Service.Interfaces;
    using System.Collections.Generic;
    using WinUiApp.Data.Data;
    using WinUIApp.ProxyServices;
    using WinUIApp.ProxyServices.Models;

    internal class HeaderViewModel
    {
        private IDrinkService drinkService;

        public HeaderViewModel(IDrinkService drinkService)
        {
            this.drinkService = drinkService;
        }

        public List<Category> GetCategories()
        {
            return this.drinkService.GetDrinkCategories();
        }
    }
}