using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace _20230622_PolylineKoteiSizeThumb
{

    public class MyConverterPen : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Brush b = (Brush)values[0];
            double dd = (double)values[1];
            return new Pen(b, dd);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
