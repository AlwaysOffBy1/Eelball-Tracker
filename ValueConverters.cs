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
    //IsDatabaseSavingColor converts the bool of "isUsingIO" to a color
    internal class IsDatabaseSavingColor : IValueConverter
    {
        private Color Hex2BColor(string hex)
        {
            return Color.FromRgb
                (
                    System.Convert.ToByte(hex.Substring(0, 2), 16),
                    System.Convert.ToByte(hex.Substring(2, 2), 16),
                    System.Convert.ToByte(hex.Substring(4, 2), 16)
                );
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (bool)value ? Hex2BColor("FF725F") : Hex2BColor("8CFF5F");
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //IsDatabaseSavingBrush converts the bool of "isUsingIO" to a brush
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
    internal class ThrowToBrush : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Color c1;
            Color c2;
            switch (values[0].ToString())
            {
                case "EELBALL":
                    c2 = Color.FromRgb(255,150,60); break;
                case "SUBBALL":
                    c2 = Colors.LightGreen; break;
            }
            switch (values[1].ToString())
            {
                case "MISS":
                    c1 = Colors.Red;break;
                case "CHALICE":
                    c1 = Colors.LimeGreen;break;
                case "ASS":
                    c1 = Colors.Yellow;break;
                case "TODD":
                    c1 = Colors.Cyan;break;

            }
            
            return new LinearGradientBrush(c2,c1,0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
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
