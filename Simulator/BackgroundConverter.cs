 using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Simulator
{
    class BackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
            {
                throw new ArgumentException("Target must be a Brush");
            }

            if (value == null)
            {
                return false;
            }

            int convertedValue;
            if (!int.TryParse(value.ToString(), out convertedValue))
            {
                throw new InvalidOperationException("The Value can not be converted to a int");
            }

            var color = convertedValue == -1 ? Colors.PaleGreen : Colors.Orange;
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new NotSupportedException();
        }
    }
}