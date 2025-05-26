namespace WinUIApp.Views.Components
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    public sealed partial class VoteDrinkOfTheDayButton : UserControl
    {
        public static readonly DependencyProperty DrinkIdProperty =
            DependencyProperty.Register(nameof(DrinkId), typeof(int), typeof(VoteDrinkOfTheDayButton), new PropertyMetadata(DefaultIntValue));

        private const int DefaultIntValue = 0;

        public VoteDrinkOfTheDayButton()
        {
            this.InitializeComponent();
        }

        public int DrinkId
        {
            get => (int)this.GetValue(DrinkIdProperty);
            set => this.SetValue(DrinkIdProperty, value);
        }
    }
}