using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using Wpf.UiUtil.Helper;

namespace MkDocsDatabaseGenerator.Converters
{
    public class BaseModuloConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertInternal(value, parameter);
        }

        protected object ConvertInternal(DateTime value, TextBlock textBlock, double? width)
        {
            if (width == null)
                return value.ToString("dd/MM");

            string toReturn = value.ToLongDateString();
            if (textBlock.MeasureString(candidate: toReturn).Width < width)
                return toReturn;
            toReturn = value.ToShortDateString();
            if (textBlock.MeasureString(candidate: toReturn).Width < width)
                return toReturn;

            return value.ToString("dd/MM");
        }

        protected object ConvertInternal(DateOnly value, TextBlock textBlock, double? width)
        {
            if (width == null)
                return value.ToString("dd/MM");

            string toReturn = value.ToLongDateString();
            if (textBlock.MeasureString(candidate: toReturn).Width < width)
                return toReturn;
            toReturn = value.ToShortDateString();
            if (textBlock.MeasureString(candidate: toReturn).Width < width)
                return toReturn;

            return value.ToString("dd/MM");
        }

        protected object ConvertInternal(object value, object parameter)
        {
            Type type = parameter.GetType();
            if (parameter != null && int.TryParse((string)parameter, out int parametercast))
            {
                if (value is int int_value)
                    return int_value % parametercast;
                if (value is double double_value)
                    return double_value % parametercast;
                if (value is float float_value)
                    return float_value % parametercast;
                if (value is long long_value)
                    return long_value % parametercast;
                if (value is decimal decimal_value)
                    return decimal_value % parametercast;
                if (value is string value_string)
                {
                    if (int.TryParse((string)value_string, out int int_value_string))
                        return int_value_string % parametercast;
                    if (double.TryParse((string)value_string, out double double_value_string))
                        return double_value_string % parametercast;
                    if (float.TryParse((string)value_string, out float float_value_string))
                        return float_value_string % parametercast;
                    if (long.TryParse((string)value_string, out long long_value_string))
                        return long_value_string % parametercast;
                    if (decimal.TryParse((string)value_string, out decimal decimal_value_string))
                        return decimal_value_string % parametercast;
                }
            }

            throw new NotImplementedException($"value {value} is not of implemented type");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => throw new NotImplementedException();
    }
}