using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Hangman.Converters
{
    public class ContainsKeyToEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not HashSet<char> set)
                return false;

            if (parameter is not string param)
                return false;

            return !set.Contains(param[0]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}