using System;
using System.Globalization;
using Xamarin.Forms;

namespace DarkFit_app
{
    public class ZeroToVisibilityConverter : IValueConverter
    {
        // Если значение = 0, возвращаем false (не видно), иначе true (видно)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;

            if (value is int intValue)
                return intValue != 0;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
