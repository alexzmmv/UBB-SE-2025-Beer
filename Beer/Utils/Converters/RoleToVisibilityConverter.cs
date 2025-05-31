using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using DataAccess.Constants;

namespace WinUIApp.Utils.Converters
{
    public class RoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is RoleType roleType)
            {
                bool isInverse = parameter?.ToString() == "Inverse";
                if (isInverse)
                {
                    return roleType == RoleType.User ? Visibility.Visible : Visibility.Collapsed;
                }
                return roleType == RoleType.Admin ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility visibility)
            {
                bool isInverse = parameter?.ToString() == "Inverse";
                if (isInverse)
                {
                    return visibility == Visibility.Visible ? RoleType.User : RoleType.Admin;
                }
                return visibility == Visibility.Visible ? RoleType.Admin : RoleType.User;
            }
            return RoleType.User;
        }
    }
} 