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
                imageSource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageSource)));
            }
        }

        public BottleAsset()
        {
            this.imageSource = string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}