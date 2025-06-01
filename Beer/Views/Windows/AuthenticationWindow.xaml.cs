namespace DrinkDb_Auth
{
    using System;
    using System.Threading.Tasks;
    using DataAccess.AuthProviders.Facebook;
    using DataAccess.AuthProviders.Github;
    using DataAccess.AuthProviders.LinkedIn;
    using DataAccess.AuthProviders.Twitter;
    using DataAccess.Model.Authentication;
    using DataAccess.OAuthProviders;
    using DataAccess.Service.Interfaces;
    using DataAccess.Service;
    using DataAccess.Service.Interfaces;
    using DrinkDb_Auth.AuthProviders.Google;
    using DrinkDb_Auth.ViewModel.Authentication;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.UI;
    using Microsoft.UI.Windowing;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Quartz;
    using Quartz.Impl;
    using Windows.Graphics;
    using WinUIApp;
    using WinUiApp.Data.Data;
    using Microsoft.UI.Xaml.Navigation;
    using WinUIApp.Views.Pages;
    using DrinkDb_Auth.View;

    public sealed partial class AuthenticationWindow : Window
    {
        private IAuthenticationService authenticationService;
        private ITwoFactorAuthenticationService twoFactorAuthentificationService;
        private IUserService userService;
        private TwitterOAuth2Provider twitterOAuth2Provider;
        private IGoogleOAuth2Provider googleOAuth2Provider;
        public static Frame NavigationFrame { get; private set; }

        public static Type PreviousPage { get; set; }

        public static Type CurrentPage { get; set; }

        public AuthenticationWindow(IAuthenticationService authenticationService, ITwoFactorAuthenticationService twoFactorAuthenticationService,
            IUserService userService, TwitterOAuth2Provider twitterOAuth2Provider, IGoogleOAuth2Provider googleOAuth2Provider)
        {
            this.authenticationService = authenticationService;
            this.twoFactorAuthentificationService = twoFactorAuthenticationService;
            this.InitializeComponent();
            AuthenticationWindow.NavigationFrame = this.MainFrame;
            this.userService = userService;
            this.twitterOAuth2Provider = twitterOAuth2Provider;
            this.googleOAuth2Provider = googleOAuth2Provider;
            this.MainFrame.Navigated += FrameNavigated;

            this.Title = "Beer";

            this.HeaderComponent.Visibility = Visibility.Collapsed;

            this.AppWindow.Resize(new SizeInt32
            {
                Width = DisplayArea.Primary.WorkArea.Width,
                Height = DisplayArea.Primary.WorkArea.Height,
            });
            this.AppWindow.Move(new PointInt32(0, 0));

            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            if (appWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.Maximize();
            }
        }

        private async Task<bool> AuthenticationComplete(AuthenticationResponse response)
        {
            if (response.AuthenticationSuccessful)
            {
                User? user = await this.authenticationService.GetUser(response.SessionId);

                if (user == null)
                {
                    return false;
                }

                bool twoFAresponse = false;
                bool firstTimeSetup = user.TwoFASecret.IsNullOrEmpty();
                this.twoFactorAuthentificationService.UserId = user.UserId;
                this.twoFactorAuthentificationService.IsFirstTimeSetup = firstTimeSetup;
                TwoFaGuiHelper twoFaGuiHelper = new TwoFaGuiHelper(this, this.userService);
                (User? currentUser, string uniformResourceIdentifier, byte[] twoFactorSecret) values = await this.twoFactorAuthentificationService.Get2FAValues();

                if (values.currentUser == null)
                {
                    return false;
                }

                twoFaGuiHelper.InitializeOtherComponents(firstTimeSetup, values.currentUser, values.uniformResourceIdentifier, values.twoFactorSecret);
                twoFAresponse = await twoFaGuiHelper.SetupOrVerifyTwoFactor();

                if (twoFAresponse)
                {
                    App.CurrentUserId = user.UserId;
                    App.CurrentSessionId = response.SessionId;

                    // Initialize the header after successful 2FA
                    this.HeaderComponent.Initialize();
                    this.HeaderComponent.Visibility = Visibility.Visible;

                    AuthenticationWindow.NavigationFrame.Navigate(typeof(SuccessPage), this);
                }

                return twoFAresponse;
            }
            else
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Authentication Failed",
                    Content = "Authentication was not successful. Please try again.",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot,
                };
                await errorDialog.ShowAsync();
            }
            return false;
        }

        public async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = this.UsernameTextBox.Text;
                string password = this.PasswordBox.Password;

                AuthenticationResponse response = await this.authenticationService.AuthWithUserPass(username, password);
                await this.AuthenticationComplete(response);
            }
            catch (Exception ex)
            {
                await this.ShowError("Authentication Error", ex.ToString());
            }
        }

        public async void GoogleSignInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GoogleGuiHelper googleGuiHelper = new GoogleGuiHelper(this, this.googleOAuth2Provider);
                AuthenticationResponse authenticationResponse = await googleGuiHelper.SignInWithGoogleAsync();
                await this.AuthenticationComplete(authenticationResponse);
            }
            catch (Exception ex)
            {
                await this.ShowError("Error", ex.Message);
            }
        }

        public async void GithubSignInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IGitHubOAuthHelper githubHelper = App.Host.Services.GetRequiredService<IGitHubOAuthHelper>();
                AuthenticationResponse? authResponse = null;
                try
                {
                    authResponse = await this.authenticationService.AuthWithOAuth(OAuthService.GitHub, githubHelper);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Exception in AuthWithOAuth: {ex}");
                    throw;
                }
                _ = this.AuthenticationComplete(authResponse);
            }
            catch (Exception ex)
            {
                await this.ShowError("Authentication Error", ex.ToString());
            }
        }
        public async void FacebookSignInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var facebookHelper = App.Host.Services.GetRequiredService<IFacebookOAuthHelper>();
                AuthenticationResponse authResponse = await this.authenticationService.AuthWithOAuth(OAuthService.Facebook, facebookHelper);
                await this.AuthenticationComplete(authResponse);
            }
            catch (Exception ex)
            {
                await this.ShowError("Authentication Error", ex.ToString());
            }
        }

        public async void XSignInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TwitterGuiHelper twitterGuiHelper = new TwitterGuiHelper(this, this.twitterOAuth2Provider);
                AuthenticationResponse authResponse = await twitterGuiHelper.SignInWithTwitterAsync();
                await this.AuthenticationComplete(authResponse);
            }
            catch (Exception ex)
            {
                await this.ShowError("Error", ex.Message);
            }
        }

        public async void LinkedInSignInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var linkedInHelper = App.Host.Services.GetRequiredService<ILinkedInOAuthHelper>();
                AuthenticationResponse authResponse = await this.authenticationService.AuthWithOAuth(OAuthService.LinkedIn, linkedInHelper);
                await this.AuthenticationComplete(authResponse);
            }
            catch (Exception ex)
            {
                await this.ShowError("Authentication Error", ex.ToString());
            }
        }

        private async Task ShowError(string title, string content)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot,
            };
            await errorDialog.ShowAsync();
        }

        private void FrameNavigated(object sender, NavigationEventArgs e)
        {
            if (this.HeaderComponent != null)
            {
                // Hide header for Success, Profile and Admin pages
                if (e.SourcePageType == typeof(SuccessPage) || e.SourcePageType == typeof(ProfilePage) || e.SourcePageType == typeof(AdminPage))
                {
                    this.HeaderComponent.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.HeaderComponent.Visibility = Visibility.Visible;
                    this.HeaderComponent.UpdateHeaderComponentsVisibility(e.SourcePageType);
                }
            }
        }

        public void HandleLogout()
        {
            // Hide the header
            this.HeaderComponent.Visibility = Visibility.Collapsed;
            
            // Reset the window title
            this.Title = "DrinkDb - Sign In";
            
            // Clear username and password fields
            this.UsernameTextBox.Text = string.Empty;
            this.PasswordBox.Password = string.Empty;

            // Clear the navigation stack and show the default content
            this.MainFrame.BackStack.Clear();
            this.MainFrame.Content = this.RootGrid;


        }
    }
}
