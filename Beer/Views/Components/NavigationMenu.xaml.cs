using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using DrinkDb_Auth;
using DataAccess.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using DataAccess.Constants;
using WinUIApp;
using System;
using DrinkDb_Auth.View;
using WinUIApp.Views.Pages;

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
                isAdmin = await userService.GetHighestRoleTypeForUser(App.CurrentUserId) == RoleType.Admin;
                IsAdmin = isAdmin;
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
            // Clear the current user ID and session
            App.CurrentUserId = Guid.Empty;
            App.CurrentSessionId = Guid.Empty;
            
            // Get the window from the XAML root
            var window = (AuthenticationWindow)App.MainWindow;
            window.HandleLogout();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationWindow.NavigationFrame.Navigate(typeof(MainPage));
        }
    }
} 