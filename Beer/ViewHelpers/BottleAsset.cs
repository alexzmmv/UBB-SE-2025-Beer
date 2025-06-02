namespace WinUIApp.ViewHelpers
{
    using System.ComponentModel;
    public class BottleAsset : INotifyPropertyChanged
    {
        private string imageSource;

        public string ImageSource
        {
            get => imageSource;
            set
            {
                this.imageSource = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ImageSource)));
            }
        }

        public BottleAsset()
        {
            this.imageSource = string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}