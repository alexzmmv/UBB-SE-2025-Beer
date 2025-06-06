﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DataAccess.Service.Interfaces;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.ViewModels
{
    internal class MainPageViewModel
    {
        private IDrinkService drinkService;
        private string imageSource;
        private string drinkName;
        private string drinkBrand;
        private List<Category> drinkCategories;
        private decimal alcoholContent;

        public MainPageViewModel(IDrinkService drinkService)
        {
            this.drinkService = drinkService;
            this.LoadDrinkOfTheDayData();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string ImageSource
        {
            get => this.imageSource;
            set => this.SetField(ref this.imageSource, value);
        }

        public string DrinkName
        {
            get => this.drinkName;
            set => this.SetField(ref this.drinkName, value);
        }

        public string DrinkBrand
        {
            get => this.drinkBrand;
            set => this.SetField(ref this.drinkBrand, value);
        }

        public List<Category> DrinkCategories
        {
            get => this.drinkCategories;
            set => this.SetField(ref this.drinkCategories, value);
        }

        public decimal AlcoholContent
        {
            get => this.alcoholContent;
            set => this.SetField(ref this.alcoholContent, value);
        }

        public List<DrinkDTO> PersonalDrinks { get; set; } = new List<DrinkDTO>();

        public void LoadDrinkOfTheDayData()
        {
            DrinkDTO drink = this.drinkService.GetDrinkOfTheDay();

            this.ImageSource = drink.DrinkImageUrl;
            this.DrinkName = drink.DrinkName;
            this.DrinkBrand = drink.DrinkBrand.BrandName;
            this.DrinkCategories = drink.CategoryList.ToList();
            this.AlcoholContent = (decimal)drink.AlcoholContent;
        }

        public int GetDrinkOfTheDayId()
        {
            return this.drinkService.GetDrinkOfTheDay().DrinkId;
        }

        protected bool SetField<TProperty>(ref TProperty field, TProperty value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TProperty>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}