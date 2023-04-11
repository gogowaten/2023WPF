using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace _20230411_ColorPicker
{
    public class ConverterHsv2H : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MyHsv hsv = (MyHsv)value;
            return hsv.H;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double para0 = (double)value;
            object[] ooo = (object[])parameter;
            double para1 = (double)ooo[0];
            double para2 = (double)ooo[1];
            return new MyHsv(para0, para1, para2);
        }
    }
    public class ConverterHsv2S : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MyHsv hsv = (MyHsv)value;
            return hsv.S;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double para0 = (double)value;
            object[] ooo = (object[])parameter;
            double para1 = (double)ooo[0];
            double para2 = (double)ooo[1];
            return new MyHsv(para1, para0, para2);
        }
    }
    public class ConverterHsv2V : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MyHsv hsv = (MyHsv)value;
            return hsv.V;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double para0 = (double)value;
            object[] ooo = (object[])parameter;
            double para1 = (double)ooo[0];
            double para2 = (double)ooo[1];
            return new MyHsv(para1, para2, para0);
        }
    }

    public class Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double r = (double)values[0];
            double g = (double)values[1];
            double b = (double)values[2];
            return new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = (SolidColorBrush)value;
            Color color = brush.Color;
            return new object[] { color.R, color.G, color.B };
        }
    }
    public class ConverterHSV : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double h = (double)values[0];
            double s = (double)values[1];
            double v = (double)values[2];
            Color c = HSV.HSV2Color(h, s, v);
            return new SolidColorBrush(c);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = (SolidColorBrush)value;
            Color color = brush.Color;
            var hsv = HSV.Color2HSV(color);
            return new object[] { hsv.h, hsv.s, hsv.v };
        }
    }

    public class ConverterHue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            return HSV.Color2HSV(color).h;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConverterSaturation : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            return HSV.Color2HSV(color).s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConverterValue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            return HSV.Color2HSV(color).v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConverterR : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush solid = (SolidColorBrush)parameter;
            Color color = solid.Color;
            solid = (SolidColorBrush)value;
            color = solid.Color;
            return color.R;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            SolidColorBrush solid = (SolidColorBrush)parameter;
            Color color = solid.Color;
            solid = new SolidColorBrush(Color.FromRgb((byte)Math.Round(val, MidpointRounding.AwayFromZero), color.G, color.B));
            return solid;
        }
    }
    public class ConverterG : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush solid = (SolidColorBrush)parameter;
            Color color = solid.Color;
            return color.G;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            SolidColorBrush solid = (SolidColorBrush)parameter;
            Color color = solid.Color;
            return new SolidColorBrush(Color.FromRgb(color.R, (byte)Math.Round(val, MidpointRounding.AwayFromZero), color.B));
        }
    }
    public class ConverterB : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush solid = (SolidColorBrush)parameter;
            Color color = solid.Color;
            return color.B;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            SolidColorBrush solid = (SolidColorBrush)parameter;
            Color color = solid.Color;
            return new SolidColorBrush(Color.FromRgb(color.R, color.G, (byte)Math.Round(val, MidpointRounding.AwayFromZero)));
        }
    }


    public class ConverterHsv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MyHsv hsv = (MyHsv)value;
            return HSV.HSV2Color(hsv.H, hsv.S, hsv.V);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            (double h, double s, double v) = HSV.Color2HSV(color);
            return new MyHsv(h, s, v);
        }
    }

    public class ConverterSolidBruhs : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color1 = (Color)value;
            return new SolidColorBrush(color1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterRGB : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            byte r = (byte)values[0];
            byte g = (byte)values[1];
            byte b = (byte)values[2];
            return Color.FromRgb(r, g, b);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterHSV2Solid : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
