namespace WinUIApp.Utils.Converters
{
    using System;
    using Microsoft.UI.Xaml.Data;

    public partial class ImagePathConverter(IBitmapImageFactory bitmapImageFactory) : IValueConverter
    {
        private const string FALL_BACK_IMAGE_PATH = "ms-appx:///Assets/DefaultDrink.jpg";
        private readonly IBitmapImageFactory bitmapImageFactory = bitmapImageFactory;

        public ImagePathConverter()
            : this(new DefaultBitmapImageFactory())
        {
        }

        public object Convert(object imagePathSourceValue, Type destinationType, object converterParameter, string formattingCulture)
        {
            if (imagePathSourceValue is string url && !string.IsNullOrEmpty(url))
            {
                try
                {
                    return this.bitmapImageFactory.Create(url);
                }
                catch
                {
                    return this.bitmapImageFactory.Create(ImagePathConverter.FALL_BACK_IMAGE_PATH);
                }
            }

            return this.bitmapImageFactory.Create(ImagePathConverter.FALL_BACK_IMAGE_PATH);
        }

        public object ConvertBack(object displayedImagePathValue, Type sourcePropertyType, object converterParameter, string formattingCulture)
        {
            throw new NotImplementedException("Converting from BitmapImage to URL string is not supported.");
        }
    }
}