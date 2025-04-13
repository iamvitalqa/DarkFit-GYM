using System;
using System.Globalization;
using Xamarin.Forms;

namespace DarkFit_app.Helpers
{
    public class ItemsHeightConverter : IValueConverter
    {
        private const int ItemHeight = 70; // Высота одного элемента в пикселях
        private const int MaxHeight = 350; // Максимальная высота списка

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                int totalHeight = count * ItemHeight;
                return totalHeight > MaxHeight ? MaxHeight : totalHeight; // Ограничиваем высоту
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
