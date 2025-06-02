using System;
using Microsoft.UI.Xaml.Media.Imaging;

namespace WinUIApp.Utils.Converters
{
    public class DefaultBitmapImageFactory : IBitmapImageFactory
    {
        public BitmapImage Create(string uri)
        {
            return new BitmapImage(new Uri(uri));
        }
    }
}
