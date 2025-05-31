namespace DrinkDb_Auth.View
{
    using System;
    using System.ComponentModel;
    using DataAccess.Model.AdminDashboard;
    using DataAccess.Model.Authentication;
    using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
    using DrinkDb_Auth.ViewModel.AdminDashboard;
    using Microsoft.UI.Text;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Controls.Primitives;
    using Microsoft.Extensions.DependencyInjection;
    using DataAccess.AutoChecker;
    using DataAccess.Service;
    using DataAccess.Service.Interfaces;
    using WinUIApp;
    using WinUiApp.Data.Data;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using DataAccess.DTOModels;
    using Microsoft.UI.Xaml.Navigation;
    using WinUIApplication.Views.Components;

    public sealed partial class AdminPage : Page
    {
        public MainPageViewModel ViewModel { get; }
        private readonly IUserService userService;
        private readonly IDrinkService drinkService;
        private readonly IReviewService reviewService;
        private readonly IUpgradeRequestsService upgradeRequestsService;

        public MainPage()
        {
            this.InitializeComponent();
            this.userService = App.Host.Services.GetRequiredService<IUserService>();
            this.drinkService = App.Host.Services.GetRequiredService<IDrinkService>();
            this.reviewService = App.Host.Services.GetRequiredService<IReviewService>();
            this.upgradeRequestsService = App.Host.Services.GetRequiredService<IUpgradeRequestsService>();

            ICheckersService checkersService = App.Host.Services.GetRequiredService<ICheckersService>();
            IAutoCheck autoCheck = App.Host.Services.GetRequiredService<IAutoCheck>();

            this.ViewModel = new MainPageViewModel(this.reviewService, this.userService, this.upgradeRequestsService, checkersService);
            this.DataContext = ViewModel;

            this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            this.Unloaded += MainPage_Unloaded;

            // Load data after initialization
            this.Loaded += async (s, e) => await LoadInitialData();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (this.Frame != null)
            {
                NavMenu.SetNavigationFrame(this.Frame);
            }
            LoadDashboardData();
        }

        private async Task LoadInitialData()
        {
            try
            {
                await this.ViewModel.LoadAllData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading initial data: {ex.Message}");
                // Show error to user if needed
            }
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == nameof(ViewModel.IsWordListVisible))
            {
                this.WordListPopup.Visibility = ViewModel.IsWordListVisible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void AppealsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is User selectedUser)
            {
                ViewModel.SelectedAppealUser = selectedUser;
                ShowAppealDetailsUI(sender);
            }
        }

        private void ShowAppealDetailsUI(object anchor)
        {
            Flyout flyout = new Flyout();
            StackPanel panel = new StackPanel { Padding = new Thickness(10) };
            TextBlock userInfo = new TextBlock
            {
                FontSize = 18,
            };
            userInfo.SetBinding(TextBlock.TextProperty, new Microsoft.UI.Xaml.Data.Binding
            {
                Path = new PropertyPath("UserStatusDisplay"),
                Source = ViewModel,
                Mode = Microsoft.UI.Xaml.Data.BindingMode.OneWay,
            });

            TextBlock reviewsHeader = new TextBlock
            {
                Text = "User Reviews:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 10, 0, 5),
            };

            ListView reviewsList = new ListView
            {
                MaxHeight = 200,
            };
            reviewsList.SetBinding(ItemsControl.ItemsSourceProperty, new Microsoft.UI.Xaml.Data.Binding
            {
                Path = new PropertyPath("UserReviewsFormatted"),
                Source = ViewModel,
                Mode = Microsoft.UI.Xaml.Data.BindingMode.OneWay,
            });

            Button banButton = new Button
            {
                Content = "Keep Ban",
                Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red),
                Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Command = ViewModel.KeepBanCommand,
            };

            Button appealButton = new Button
            {
                Content = "Accept Appeal",
                Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green),
                Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Command = ViewModel.AcceptAppealCommand,
            };

            Button closeButton = new Button
            {
                Content = "Close Appeal Case",
                HorizontalAlignment = HorizontalAlignment.Center,
                Command = ViewModel.CloseAppealCaseCommand,
            };

            closeButton.Click += (s, args) => { flyout.Hide(); };

            panel.Children.Add(userInfo);

            panel.Children.Add(reviewsHeader);
            panel.Children.Add(reviewsList);
            panel.Children.Add(banButton);
            panel.Children.Add(appealButton);
            panel.Children.Add(closeButton);

            flyout.Content = panel;
            flyout.Placement = FlyoutPlacementMode.Left;
            flyout.ShowAt((FrameworkElement)anchor);
        }
        private async void AcceptUpgradeRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int requestId)
            {
                await ViewModel.HandleUpgradeRequest(true, requestId);
            }
        }

        private async void DeclineUpgradeRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int requestId)
            {
                await ViewModel.HandleUpgradeRequest(false, requestId);
            }
        }

        private async void ReviewSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            await ViewModel.FilterReviews(this.ReviewSearchTextBox.Text);
        }

        private async void BannedUserSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            await ViewModel.FilterAppeals(this.BannedUserSearchTextBox.Text);
        }

        private void MenuFlyoutAllowReview_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.DataContext is Review review)
            {
                ViewModel.ResetReviewFlags(review.ReviewId);
            }
        }

        private void MenuFlyoutHideReview_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.DataContext is Review review)
            {
                ViewModel.HideReview(review.ReviewId);
            }
        }

        private async void MenuFlyoutAICheck_Click_2(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.DataContext is ReviewDTO review)
            {
                await ViewModel.RunAICheck(review);
            }
        }

        private async void AddWord_Click(object sender, RoutedEventArgs e)
        {
            TextBox input = new TextBox { PlaceholderText = "Enter new word..." };

            ContentDialog dialog = new ContentDialog
            {
                Title = "Add New Word",
                Content = input,
                PrimaryButtonText = "Add Word",
                CloseButtonText = "Cancel",
                XamlRoot = XamlRoot,
            };

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                string newWord = input.Text.Trim();
                ViewModel.AddOffensiveWord(newWord);
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ProfilePage));
            }
        }

        private async void BanUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Guid userId)
            {
                await this.ViewModel.BanUser(userId);
            }
        }

        private void LoadDashboardData()
        {
            // Implementation of LoadDashboardData method
        }
    }
}