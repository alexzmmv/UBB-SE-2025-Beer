using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.ViewModels
{
    internal class ProfilePageViewModel
    {
        private IDrinkService drinkService;

        public event PropertyChangedEventHandler PropertyChanged;

        public ProfilePageViewModel()
        {
            this.drinkService = App.Host.Services.GetRequiredService<IDrinkService>();
            this.LoadPersonalDrinkListData();
        }

        public List<DrinkDTO> PersonalDrinks { get; set; } = new List<DrinkDTO>();

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void LoadPersonalDrinkListData()
        {
            Guid userId = App.CurrentUserId;
            this.PersonalDrinks = this.drinkService.GetUserPersonalDrinkList(userId);
        }
    }
}
