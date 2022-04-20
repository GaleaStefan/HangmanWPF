using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hangman.Converters
{
    public enum Comparer
    {
        Less,
        Greater,
        LessOrEquals,
        GreaterOrEquals,
        Equals
    }
    
    public class ComparatorToVisibilityConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is not int first)
                return null;
            
            if (values[1] is not int second)
                return null;

            if (parameter is not Comparer comparer)
                return null;

            return comparer switch
            {
                Comparer.Less => first < second ? Visibility.Visible : Visibility.Hidden,
                Comparer.LessOrEquals => first <= second ? Visibility.Visible : Visibility.Hidden,
                Comparer.Greater => first > second ? Visibility.Visible : Visibility.Hidden,
                Comparer.GreaterOrEquals => first >= second ? Visibility.Visible : Visibility.Hidden,
                Comparer.Equals => first == second ? Visibility.Visible : Visibility.Hidden,
                _ => null
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}