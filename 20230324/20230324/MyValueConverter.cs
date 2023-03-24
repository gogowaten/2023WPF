using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Security.Cryptography.Xml;
using System.Windows.Media;

namespace _20230324
{
    class MyValueConverter
    {
    }
    public class MyConverterRectWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect r = (Rect)value;
            if (r.IsEmpty) return 0;
            return r.Width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConverterRectHeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect r = (Rect)value;
            if (r.IsEmpty) return 0;
            return r.Height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConverterRotateTransform : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double angle = (double)value;
            RotateTransform tra = new RotateTransform(angle);
            return tra;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RotateTransform tra = (RotateTransform)value;
            return tra.Angle;
        }
    }
}
