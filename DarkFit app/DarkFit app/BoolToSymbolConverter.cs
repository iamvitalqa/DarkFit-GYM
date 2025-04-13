using System;
using System.Globalization;
using Xamarin.Forms;

namespace DarkFit_app
{
    public class BoolToSymbolConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isExpanded)
                return isExpanded ? "−" : "+"; // Минус и плюс
            return "+";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }



    }
}
