using System;
using System.Globalization;
using System.Windows.Data;

namespace Hangman.Converters
{
    public class MistakesToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not int mistake)
                return null;

            return @"\Images\hangman" + (mistake + 1) + ".png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}