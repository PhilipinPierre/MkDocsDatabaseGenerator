using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using Wpf.UiUtil.Helper;

namespace MkDocsDatabaseGenerator.Converters
{
    public class ModuloConverter : BaseModuloConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = base.Convert(value: value, targetType: targetType, parameter: parameter, culture: culture);
            if (item != null)
                return "KW " + item;

            if (value is DateOnly dateOnly)
                return dateOnly.ToString("dd/MM");
            return value;
        }
    }
}