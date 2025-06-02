using System;
using DrinkDb_Auth.View.Authentication.Interfaces;
using DrinkDb_Auth.ViewModel.AdminDashboard.Components;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace DrinkDb_Auth.View.Authentication
{
    public class AuthenticationCodeWindow : IDialog
    {
        private ContentDialog? contentDialog;
        private string title;
        private string cancelButtonText;
        private string primaryButtonText;
        private RelayCommand command;
        private Window? window;
        private object view;

        public ContentDialog? ContentDialog
        {
            get { return contentDialog; }
        }

        public RelayCommand? Command { get => command; set => command = value ?? new RelayCommand(new Action(() => { })); }

        public AuthenticationCodeWindow(string title, string cancelButtonText, string primaryButtonText, RelayCommand command, Window? window, object view)
        {
            this.title = title;
            this.cancelButtonText = cancelButtonText;
            this.primaryButtonText = primaryButtonText;
            this.command = command;
            this.window = window;
            this.view = view;
        }

        public ContentDialog? CreateContentDialog()
        {
            contentDialog = new ContentDialog
            {
                Title = title,
                CloseButtonText = cancelButtonText,
                PrimaryButtonText = primaryButtonText,
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonCommand = command,
                XamlRoot = window?.Content.XamlRoot,
                Content = view,
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 26, 26, 46)),
                Foreground = new SolidColorBrush(),
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 75, 108, 219)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8)
            };

            contentDialog.Resources["ContentDialogButtonBackground"] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 75, 108, 219));
            contentDialog.Resources["ContentDialogButtonBackgroundPointerOver"] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 65, 98, 209));
            contentDialog.Resources["ContentDialogButtonBackgroundPressed"] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 55, 88, 199));
            contentDialog.Resources["ContentDialogButtonForeground"] = new SolidColorBrush();
            contentDialog.Resources["ContentDialogButtonBorderBrush"] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 75, 205, 219));

            return contentDialog;
        }

        public async void ShowAsync()
        {
            if (this.contentDialog != null)
            {
                if (this.contentDialog.XamlRoot == null && this.window != null)
                {
                    this.contentDialog.XamlRoot = this.window.Content.XamlRoot;
                }
                await contentDialog.ShowAsync();
            }
        }

        public void Hide()
        {
            this.contentDialog?.Hide();
        }
    }
}