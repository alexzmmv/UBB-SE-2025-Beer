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

namespace WinUIApplication.Views.Components
{
    public sealed partial class NavigationMenu : UserControl
    {
        private readonly IUserService userService;
        private bool isAdmin;
        private Frame navigationFrame;

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
            InitializeAdminStatus();
        }

        public void SetNavigationFrame(Frame frame)
        {
            this.navigationFrame = frame;
        }

        private async void InitializeAdminStatus()
        {
            isAdmin = await userService.GetHighestRoleTypeForUser(App.CurrentUserId) == RoleType.Admin;
            IsAdmin = isAdmin;
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (navigationFrame != null)
            {
                navigationFrame.Navigate(typeof(UserPage));
            }
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            if (navigationFrame != null)
            {
                navigationFrame.Navigate(typeof(DrinkDb_Auth.View.MainPage));
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the current user ID and session
            App.CurrentUserId = Guid.Empty;
            App.CurrentSessionId = Guid.Empty;
            
            // Reset the existing window to login view
            if (App.MainWindow is AuthenticationWindow authWindow)
            {
                authWindow.ResetToLoginView();
            }
        }
    }
} 