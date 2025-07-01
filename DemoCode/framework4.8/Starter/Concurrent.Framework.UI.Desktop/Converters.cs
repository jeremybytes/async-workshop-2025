using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Concurrent.Framework.UI.Desktop
{
    public class DecadeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int year = ((DateTime)value).Year;
            return string.Format($"{year.ToString().Substring(0,3)}0s");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int rating = (int)value;
            return string.Format($"{rating}/10 Stars");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RatingStarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int rating = (int)value;
            return new string('*', rating);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DecadeBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int decade = (((DateTime)value).Year / 10) * 10;

            switch (decade)
            {
                case 1970: return new SolidColorBrush(Colors.Maroon);
                case 1980: return new SolidColorBrush(Colors.DarkGreen);
                case 1990: return new SolidColorBrush(Colors.DarkSlateBlue);
                case 2000: return new SolidColorBrush(Colors.CadetBlue);
                case 2010: return new SolidColorBrush(Colors.DarkMagenta);
                default: return new SolidColorBrush(Colors.DarkSlateGray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
