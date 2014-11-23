using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Simulator
{
    /// <summary>
    /// Converts a Process ID to a SolidColorBrush
    /// </summary>
    class BackgroundConverter : IValueConverter
    {

        /// <summary>
        /// Array of colors
        /// Generated from: http://tools.medialab.sciences-po.fr/iwanthue/
        /// 
        /// Colors for data scientists. Palettes of optimally distinct colors.
        /// </summary>
        private static readonly string[] COLORS = new string[] {
            "#F88644",
            "#5F54D2",
            "#06B26D",
            "#E0A2CA",
            "#663731",
            "#EF00CC",
            "#807D13",
            "#FD3A5D",
            "#93C441",
            "#CCBC6C"
        };

        /// <summary>
        /// Convert the process id to a color
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Ensure the target of the conversion is brush
            if (targetType != typeof(Brush))
            {
                throw new ArgumentException("Target must be a Brush");
            }

            if (value == null)
            {
                // Default to a black brush
                return new SolidColorBrush(Colors.Black);
            }

            // Convert the value to an int
            int convertedValue;
            if (!int.TryParse(value.ToString(), out convertedValue))
            {
                throw new InvalidOperationException("The Value can not be converted to a int");
            }


            // Default color for an unused memory allocation block
            Color selectedColor = Colors.DarkGray;

            
            if (convertedValue > -1)
            {
                // Pick a color from the color palette
                selectedColor = (Color)ColorConverter.ConvertFromString(COLORS[convertedValue % COLORS.Length]);
            }

            return new SolidColorBrush(selectedColor);
        }


        /// <summary>
        /// Not supporting back conversion
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new NotSupportedException();
        }
    }
}