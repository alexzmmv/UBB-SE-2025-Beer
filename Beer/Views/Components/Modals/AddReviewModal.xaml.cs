using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp.Views.Components.Modals
{
    public sealed partial class AddReviewModal : UserControl
    {

        public event EventHandler CloseRequested;
        public AddReviewModal()
        {
            this.InitializeComponent();
        }

        private void SaveReviewButton_Click(object sender, RoutedEventArgs e)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void CancelReviewButton_Click(object sender, RoutedEventArgs e)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
