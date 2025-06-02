namespace WinUIApp.Utils.Converters
{
    using System;
    using Microsoft.UI.Xaml.Data;

    public partial class AlcoholContentConverter : IValueConverter
    {
        private const string DEFAULT_ALCOHOL_PERCENTAGE = "0%";
        private const string ALCOHOL_PERCENTAGE_FORMAT = "{0}%";

        public object Convert(object alcoholContentSourceValue, Type destinationType, object converterParameter, string formattingCulture)
        {
            if (alcoholContentSourceValue is float alcoholContent)
            {
                return string.Format(AlcoholContentConverter.ALCOHOL_PERCENTAGE_FORMAT, alcoholContent);
            }

            return AlcoholContentConverter.DEFAULT_ALCOHOL_PERCENTAGE;
        }

        public object ConvertBack(object displayedAlcoholContentValue, Type sourcePropertyType, object converterParameter, string formattingCulture)
        {
            throw new NotImplementedException("Converting from string to alcohol content is not supported.");
        }
    }
}