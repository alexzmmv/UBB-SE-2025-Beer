namespace WinUIApp.Views.Components
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    public sealed partial class AddDrinkButton : UserControl
    {
        public AddDrinkButton()
        {
            this.InitializeComponent();
        }

        private void AddDrinkButton_Click(object sender, RoutedEventArgs eventArguments)
        {
            var userService = new WinUIApp.Services.DummyServices.UserService();
            var flyout = new Flyout
            {
                Content = new AddDrinkFlyout
                {
                    UserId = userService.GetCurrentUserId(),
                },
            };

            flyout.ShowAt(this.AddButton);
        }
    }
}