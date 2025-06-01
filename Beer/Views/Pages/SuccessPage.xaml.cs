using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUIApp.Views.Pages;
using DataAccess.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using DataAccess.Constants;
using WinUIApp;

namespace DrinkDb_Auth
{
    public sealed partial class SuccessPage : Page
    {
        private readonly IUserService userService;
        private bool isAdmin;

        public bool IsAdmin => isAdmin;

        public SuccessPage()
        {
            this.InitializeComponent();
            Loaded += SuccessPage_Loaded;
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
            AuthenticationWindow.CurrentPage = typeof(SuccessPage);
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationWindow.NavigationFrame.Navigate(typeof(MainPage));
        }

        private void SuccessPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.CircleAnimation.Begin();
        }
    }
}