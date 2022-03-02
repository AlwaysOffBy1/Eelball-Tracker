using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;


namespace EELBALL_TRACKER
{
    internal class IsDatabaseSaving : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string output = (bool)value? "Working...": "Ready";
            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class IsDatabaseSavingBrush : IValueConverter
    {
        private Brush Hex2Brush(string hex)
        {
            return new SolidColorBrush(Color.FromRgb
                (
                    System.Convert.ToByte(hex.Substring(0,2), 16),
                    System.Convert.ToByte(hex.Substring(2,2), 16),
                    System.Convert.ToByte(hex.Substring(4,2), 16)
                )
            );
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush brush = (bool)value ? Hex2Brush("FF725F") : Hex2Brush("8CFF5F");
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class ResultToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "Miss":
                    return Brushes.Red;
                case "In":
                    return Brushes.Green;
                case "Assassin":
                    return Brushes.White;

            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class InvertBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
