using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUIApp.Views;
using DataAccess.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using DataAccess.Constants;
using WinUIApp;
using DrinkDb_Auth.View;
using System;

namespace DrinkDb_Auth
{
    public sealed partial class SuccessPage : Page
    {
        private AuthenticationWindow mainWindow;
        private readonly IUserService userService;
        private bool isAdmin;

        public bool IsAdmin => isAdmin;

        public SuccessPage()
        {
            this.InitializeComponent();
            this.userService = App.Host.Services.GetRequiredService<IUserService>();
            InitializeAdminStatus();
        }

        private async void InitializeAdminStatus()
        {
            isAdmin = await userService.GetHighestRoleTypeForUser(App.CurrentUserId) == RoleType.Admin;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is AuthenticationWindow window)
            {
                this.mainWindow = window;
                // Set the navigation frame for the menu
                NavMenu.SetNavigationFrame(this.mainWindow.NavigationFrame);
            }
        }

        private async void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.mainWindow != null)
            {
                // Get current user's role
                var currentUser = await userService.GetUserById(App.CurrentUserId);
                if (currentUser != null && currentUser.AssignedRole == RoleType.Admin)
                {
                    try
                    {
                        this.mainWindow.NavigationFrame.Navigate(typeof(MainPage));
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Navigation to MainPage failed: {ex.Message}\n{ex.StackTrace}");
                        // Optionally, show a message dialog here
                    }
                }
                else
                {
                    // Navigate to regular user page
                    this.mainWindow.NavigationFrame.Navigate(typeof(UserPage));
                }
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Activate();
        }
    }
}