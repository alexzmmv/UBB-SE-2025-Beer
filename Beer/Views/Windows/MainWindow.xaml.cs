namespace WinUIApp.Views
{
    using System;
    using global::Windows.Graphics;
    using Microsoft.UI;
    using Microsoft.UI.Windowing;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using WinUIApp.Views.Pages;
    using WinUIApp.Views.Components.HeaderComponents;
    using System.Reflection;

    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            MainWindow.CurrentPage = typeof(MainPage);
            this.InitializeComponent();
            this.SetFixedSize(1440, 900);
            AppMainFrame = this.MainFrame;
            this.MainFrame.Navigated += MainFrame_Navigated;
            this.MainFrame.Navigate(typeof(MainPage));
        }

        public static Frame AppMainFrame { get; private set; }

        public static Type PreviousPage { get; set; }

        public static Type CurrentPage { get; set; }

        private void SetFixedSize(int width, int height)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new SizeInt32(width, height));
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (this.HeaderComponent != null)
            {
                this.HeaderComponent.UpdateSearchBarVisibility(e.SourcePageType);
            }
        }
    }
}