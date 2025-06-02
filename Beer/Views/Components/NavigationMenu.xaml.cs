using System;
using DataAccess.Constants;
using DataAccess.Service.Interfaces;
using DrinkDb_Auth;
using DrinkDb_Auth.View;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUIApp.Views.Components
{
    public sealed partial class NavigationMenu : UserControl
    {
        private readonly IUserService userService;
        private bool isAdmin;

        public bool IsAdmin
        {
            get { return (bool)GetValue(IsAdminProperty); }
            set { SetValue(IsAdminProperty, value); }
        }

        public static readonly DependencyProperty IsAdminProperty =
            DependencyProperty.Register("IsAdmin", typeof(bool), typeof(NavigationMenu), new PropertyMetadata(false));

        public NavigationMenu()
        {
            this.InitializeComponent();
            this.userService = App.Host.Services.GetRequiredService<IUserService>();
        }

        public async void Initialize()
        {
            if (App.CurrentUserId != Guid.Empty)
            {
                this.isAdmin = await userService.GetHighestRoleTypeForUser(App.CurrentUserId) == RoleType.Admin;
                this.IsAdmin = isAdmin;
            }
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationWindow.NavigationFrame.Navigate(typeof(ProfilePage));
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationWindow.NavigationFrame.Navigate(typeof(AdminPage));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationWindow.NavigationFrame.Navigate(typeof(ConfirmLogoutPage));
        }
    }
}