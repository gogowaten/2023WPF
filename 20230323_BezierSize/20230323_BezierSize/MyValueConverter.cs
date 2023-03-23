using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace _20230323_BezierSize
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
   

}
