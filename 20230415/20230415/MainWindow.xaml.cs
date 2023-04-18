using _20230415;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
//RGBとHSVの相互変換
//依存関係プロパティは、ARGBがbyte型、HSVはdouble型、Color型とHSV型
//RGBのどれかを変更したらHSVを再計算
//HSVのどれかを変更したらRGBを再計算
//このとき無限ループにならないようにフラグで管理

namespace _20230415
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Picker Picker;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            //this.Picker = new(Colors.AliceBlue);
            this.Picker = new();
            Left = 100;
            Top = 100;
            //MySVImage.Source = GetSVImage2(10, 100);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mx = Picker.Marker.Saturation;
            var www = Picker.Marker.Width;
            var ww = Picker.MyImageSV.Width;
            var str = Picker.MyImageSV.Stretch;
            Picker.SetColor(Color.FromArgb(200, 200, 2, 0));
        }

        private void MyButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            Picker.Show();
            Picker.Owner = this;

        }
        private BitmapSource GetSVImage2(double hue, int size)
        {
            var wb = new WriteableBitmap(size, size, 96, 96, PixelFormats.Rgb24, null);
            int stride = (size * wb.Format.BitsPerPixel + 7) / 8;
            var pixels = new byte[size * stride];
            wb.CopyPixels(pixels, stride, 0);
            int p = 0;
            Parallel.For(0, size, y =>
            {
                ParallelImageSV(p, y, stride, pixels, hue, size, size);
            });

            wb.WritePixels(new Int32Rect(0, 0, size, size), pixels, stride, 0);
            return wb;
        }
        private void ParallelImageSV(int p, int y, int stride, byte[] pixels, double hue, int w, int h)
        {
            for (int x = 0; x < w; ++x)
            {
                p = y * stride + (x * 3);
                Color svColor = MathHSV.HSV2Color(hue, x / (double)w, y / (double)h);
                pixels[p] = svColor.R;
                pixels[p + 1] = svColor.G;
                pixels[p + 2] = svColor.B;
            }
        }
    }


    public class ConverterARGB2SolidColorBrush : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            byte a = (byte)values[0];
            byte r = (byte)values[1];
            byte g = (byte)values[2];
            byte b = (byte)values[3];
            return new SolidColorBrush(Color.FromArgb(a, r, g, b));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            SolidColorBrush cb = (SolidColorBrush)value;
            object[] result = new object[4];
            result[0] = cb.Color.A;
            result[1] = cb.Color.R;
            result[2] = cb.Color.G;
            result[3] = cb.Color.B;
            return result;
        }
    }

    public class ConverterARGB2Color : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            byte a = (byte)values[0];
            byte r = (byte)values[1];
            byte g = (byte)values[2];
            byte b = (byte)values[3];
            return Color.FromArgb(a, r, g, b);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            Color cb = (Color)value;
            object[] result = new object[4];
            result[0] = cb.A;
            result[1] = cb.R;
            result[2] = cb.G;
            result[3] = cb.B;
            return result;
        }
    }

    public class ConverterRR : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RGB rGB = (RGB)value;
            return rGB.R;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConverterGG : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RGB rGB = (RGB)value;
            return rGB.G;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConverterBB : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RGB rGB = (RGB)value;
            return rGB.B;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterHH : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HSV hsv = (HSV)value;
            return hsv.H;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterSS : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HSV hsv = (HSV)value;
            return hsv.S;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterVV : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HSV hsv = (HSV)value;
            return hsv.V;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterRGB2RGB : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            byte r = (byte)values[0];
            byte g = (byte)values[1];
            byte b = (byte)values[2];
            RGB rgb = new RGB(r, g, b);
            return rgb;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            RGB rGB = (RGB)value;
            object[] result = new object[3];
            result[0] = rGB.R;
            result[1] = rGB.G;
            result[2] = rGB.B;
            return result;
        }
    }

    public class ConverterR : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HSV hsv = (HSV)value;
            var (r, g, b) = MathHSV.HSV2rgb(hsv);
            return r;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConverterG : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HSV hsv = (HSV)value;
            var (r, g, b) = MathHSV.HSV2rgb(hsv);
            return g;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConverterB : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HSV hsv = (HSV)value;
            var (r, g, b) = MathHSV.HSV2rgb(hsv);
            return b;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterH : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RGB rgb = (RGB)value;
            var (h, s, v) = MathHSV.RGB2hsv(rgb);
            return h;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConverterS : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RGB rgb = (RGB)value;
            var (h, s, v) = MathHSV.RGB2hsv(rgb);
            return s;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ConverterV : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RGB rgb = (RGB)value;
            var (h, s, v) = MathHSV.RGB2hsv(rgb);
            return v;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConverterColorHSV2ARGBHSV : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Color c = (Color)values[0];
            HSV h = (HSV)values[1];
            return new ARGBHSV(c.A, c.R, c.G, c.B, h.H, h.S, h.V);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            ARGBHSV vs = (ARGBHSV)value;
            object[] objects = new object[2];
            objects[0] = Color.FromArgb(vs.A, vs.R, vs.G, vs.B);
            objects[1] = new HSV(vs.H, vs.S, vs.V);
            return objects;
        }
    }


    public class ConverterMyARGBHSV : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            byte a = (byte)values[0];
            RGB rgb = (RGB)values[1];
            HSV hsv = (HSV)values[2];
            ARGBHSV vs = new(a, rgb.R, rgb.G, rgb.B, hsv.H, hsv.S, hsv.V);
            return vs;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            ARGBHSV vs = (ARGBHSV)value;
            object[] result = new object[3];
            result[0] = vs.A;
            result[1] = new RGB(vs.R, vs.G, vs.B);
            result[2] = new HSV(vs.H, vs.S, vs.V);
            return result;
        }
    }
    public class ConverterHSV2Color : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HSV hsv = (HSV)value;
            Color c = MathHSV.HSV2Color(hsv);
            return c;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color c = (Color)value;
            var hsv = MathHSV.Color2HSV2(c);
            return hsv;
        }
    }

    public class ConverterARGBHSVSolidBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            ARGBHSV2 vs = (ARGBHSV2)value;
            return new SolidColorBrush(Color.FromArgb(vs.A, vs.R, vs.G, vs.B));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush scb = (SolidColorBrush)value;
            Color c = scb.Color;
            var (h, s, v) = MathHSV.Color2HSV(c);
            return new ARGBHSV2(c.A, c.R, c.G, c.B, h, s, v);
        }
    }

    public class ConverterARGBHSV : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            byte a = (byte)values[0];
            byte r = (byte)values[1];
            byte g = (byte)values[2];
            byte b = (byte)values[3];
            double h = (double)values[4];
            double s = (double)values[5];
            double v = (double)values[6];
            ARGBHSV vs = new(a, r, g, b, h, s, v);
            return vs;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            ARGBHSV vs = (ARGBHSV)value;
            object[] result = new object[7];
            result[0] = vs.A; result[1] = vs.R; result[2] = vs.G; result[3] = vs.B;
            result[4] = vs.H; result[5] = vs.S; result[6] = vs.V;
            return result;
        }
    }
    public class ConverterRGB2HSV : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            byte r = (byte)values[0];
            byte g = (byte)values[1];
            byte b = (byte)values[2];
            HSV hSV = MathHSV.Rgb2HSV(r, g, b);
            return hSV;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            HSV hSV = (HSV)value;
            var (r, g, b) = MathHSV.HSV2rgb(hSV);
            object[] result = new object[3];
            result[0] = r;
            result[1] = g;
            result[2] = b;
            return result;
        }
    }

    public class ConverterHSV2HSV : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double h = (double)values[0];
            double s = (double)values[1];
            double v = (double)values[2];
            return new HSV(h, s, v);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            HSV hsv = (HSV)value;
            object[] result = new object[3];
            result[0] = hsv.H;
            result[1] = hsv.S;
            result[2] = hsv.V;
            return result;
        }
    }
    public class ConverterHSV2RGB : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HSV neko = (HSV)value;
            return MathHSV.HSV2RGB(neko);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HSV neko = (HSV)value;
            return MathHSV.HSV2RGB(neko);
        }
    }

    public class ConverterHsv2RGB : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double h = (double)values[0];
            double s = (double)values[1];
            double v = (double)values[2];
            RGB rgb = MathHSV.Hsv2RGB(h, s, v);
            return rgb;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            RGB rgb = (RGB)value;
            var (h, s, v) = MathHSV.RGB2hsv(rgb);
            object[] result = new object[3];
            result[0] = h;
            result[1] = s;
            result[2] = v;
            return result;
        }
    }

    //public class ConverterARGB2Color : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        byte a = (byte)values[0];
    //        byte r = (byte)values[1];
    //        byte g = (byte)values[2];
    //        byte b = (byte)values[3];
    //        Color c = Color.FromArgb(a, r, g, b);
    //        return c;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        Color c = (Color)value;

    //        object[] result = new object[4];
    //        result[0] = c.A;
    //        result[1] = c.R;
    //        result[2] = c.G;
    //        result[3] = c.B;
    //        return result;
    //    }
    //}
    public class ConverterColor2Brush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color c = (Color)value;
            return new SolidColorBrush(c);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
