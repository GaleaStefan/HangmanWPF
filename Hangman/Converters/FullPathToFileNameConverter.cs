using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Hangman.Converters
{
    public class FullPathToFileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string path)
                return null;

            return Path.GetFileNameWithoutExtension(path);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}