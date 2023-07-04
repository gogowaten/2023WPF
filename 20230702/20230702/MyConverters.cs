using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace _20230702
{

    public class MyConverterRectWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect rr = (Rect)value;
            return rr.IsEmpty ? 0 : rr.Width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterItems : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterItemsRect : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<TThumb> thumbList = (ObservableCollection<TThumb>)value;
            double left = double.MaxValue;
            double top = double.MaxValue;
            double right = double.MinValue;
            double bottom = double.MinValue;

            foreach (TThumb thumb in thumbList)
            {
                double minX = thumb.X; double minY = thumb.Y;
                if (left < minX) left = minX;
                if (top < minY) top = minY;
                if (right < minX + thumb.Width) right = minX + thumb.Width;
                if (bottom < minY + thumb.Height) bottom = minY + thumb.Height;
            }
            if (thumbList.Count > 0) { return new Rect(left, top, right - left, bottom - top); }
            return new Rect();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
