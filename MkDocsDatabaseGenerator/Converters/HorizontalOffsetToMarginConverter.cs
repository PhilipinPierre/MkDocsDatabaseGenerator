using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MkDocsDatabaseGenerator.Converters
{
    public class HorizontalOffsetToMarginConverter : IMultiValueConverter
    {
        private static Thickness Thickness0 = new Thickness(0);

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Logging.Instance.Function(values, targetType, parameter, culture);
            if (values.Length == 5
                && values[0] is double horizontalOffset
                && values[1] is double column
                && values[2] is double columnWidth
                && values[3] is double ContentWidth
                && values[4] is double zoomLevel
                && ContentWidth > 0)
            {
                double min = column * columnWidth * zoomLevel;
                if (horizontalOffset > min)
                    return new Thickness((horizontalOffset - min) / zoomLevel, 0, 0, 0);
            }

            return Thickness0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}