namespace WinUIApp.Utils.Converters
{
    using Microsoft.UI.Xaml.Media.Imaging;

    public interface IBitmapImageFactory
    {
        BitmapImage Create(string uri);
    }
}