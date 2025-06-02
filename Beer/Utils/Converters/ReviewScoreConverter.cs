namespace WinUIApp.Utils.Converters
{
    using System;
    using Microsoft.UI.Xaml.Data;

    public class ReviewScoreConverter : IValueConverter
    {
        private const string SCORE_FORMAT = "{0:F1}/5";
        private const string DEFAULT_SCORE_DISPLAY = "N/A";

        public object Convert(object reviewScoreSourceValue, Type destinationType, object converterParameter, string formattingCulture)
        {
            if (reviewScoreSourceValue is float score)
            {
                return string.Format(ReviewScoreConverter.SCORE_FORMAT, score);
            }

            return ReviewScoreConverter.DEFAULT_SCORE_DISPLAY;
        }

        public object ConvertBack(object displayedreviewScoreValue, Type sourcePropertyType, object converterParameter, string formattingCulture)
        {
            throw new NotImplementedException("Converting from string to review score is not supported.");
        }
    }
}