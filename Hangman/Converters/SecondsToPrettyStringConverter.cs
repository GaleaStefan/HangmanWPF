using System;
using System.Globalization;
using System.Windows.Data;

namespace Hangman.Converters
{
    public class SecondsToPrettyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not int seconds)
                return null;

            var span = TimeSpan.FromSeconds(seconds);
            return span.ToString(@"mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}