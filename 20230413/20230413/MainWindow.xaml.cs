using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlLibraryCore20200620;

namespace _20230413
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 依存関係プロパティ

        public Color PickupColor
        {
            get { return (Color)GetValue(PickupColorProperty); }
            set { SetValue(PickupColorProperty, value); }
        }
        public static readonly DependencyProperty PickupColorProperty =
            DependencyProperty.Register(nameof(PickupColor), typeof(Color), typeof(MainWindow),
                new FrameworkPropertyMetadata(Color.FromArgb(0, 0, 0, 0),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public byte R
        {
            get { return (byte)GetValue(RProperty); }
            set { SetValue(RProperty, value); }
        }
        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register(nameof(R), typeof(byte), typeof(MainWindow),
                new FrameworkPropertyMetadata(byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public byte G
        {
            get { return (byte)GetValue(GProperty); }
            set { SetValue(GProperty, value); }
        }
        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register(nameof(G), typeof(byte), typeof(MainWindow),
                new FrameworkPropertyMetadata(byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public byte B
        {
            get { return (byte)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register(nameof(B), typeof(byte), typeof(MainWindow),
                new FrameworkPropertyMetadata(byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public byte A
        {
            get { return (byte)GetValue(AProperty); }
            set { SetValue(AProperty, value); }
        }
        public static readonly DependencyProperty AProperty =
            DependencyProperty.Register(nameof(A), typeof(byte), typeof(MainWindow),
                new FrameworkPropertyMetadata(byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public HSV PickupHSV
        {
            get { return (HSV)GetValue(PickupHSVProperty); }
            set { SetValue(PickupHSVProperty, value); }
        }
        public static readonly DependencyProperty PickupHSVProperty =
            DependencyProperty.Register(nameof(PickupHSV), typeof(HSV), typeof(MainWindow),
                new FrameworkPropertyMetadata(new HSV(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int SVImageSize
        {
            get { return (int)GetValue(SVImageSizeProperty); }
            set { SetValue(SVImageSizeProperty, value); }
        }
        public static readonly DependencyProperty SVImageSizeProperty =
            DependencyProperty.Register(nameof(SVImageSize), typeof(int), typeof(MainWindow),
                new FrameworkPropertyMetadata(200,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int MarkerSize
        {
            get { return (int)GetValue(MarkerSizeProperty); }
            set { SetValue(MarkerSizeProperty, value); }
        }
        public static readonly DependencyProperty MarkerSizeProperty =
            DependencyProperty.Register(nameof(MarkerSize), typeof(int), typeof(MainWindow),
                new FrameworkPropertyMetadata(20,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        //public double Hue
        //{
        //    get { return (double)GetValue(HueProperty); }
        //    set { SetValue(HueProperty, value); }
        //}
        //public static readonly DependencyProperty HueProperty =
        //    DependencyProperty.Register(nameof(Hue), typeof(double), typeof(MainWindow),
        //        new FrameworkPropertyMetadata(0.0,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public double Saturation
        //{
        //    get { return (double)GetValue(SaturationProperty); }
        //    set { SetValue(SaturationProperty, value); }
        //}
        //public static readonly DependencyProperty SaturationProperty =
        //    DependencyProperty.Register(nameof(Saturation), typeof(double), typeof(MainWindow),
        //        new FrameworkPropertyMetadata(0.0,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public double Value
        //{
        //    get { return (double)GetValue(ValueProperty); }
        //    set { SetValue(ValueProperty, value); }
        //}
        //public static readonly DependencyProperty ValueProperty =
        //    DependencyProperty.Register(nameof(Value), typeof(double), typeof(MainWindow),
        //        new FrameworkPropertyMetadata(0.0,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        #endregion 依存関係プロパティ

        //private int ImageSize = 200;//100以外にすると表示が崩れる
        private bool IsHsvChanging = false;
        private bool IsRgbChanging = false;
        //private double ThumbSize = 20;
        private MarkerAdorner Marker;

        public MainWindow()
        {
            InitializeComponent();

            Marker = new MarkerAdorner(ImageSV);
            MySetBinting();
            MySetEvents();
            MyInitialize();

            R = 255;
            G = 0;
            B = 0;
            A = 255;

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (AdornerLayer.GetAdornerLayer(ImageSV) is AdornerLayer layer)
            {
                layer.Add(Marker);
            }
        }

        private void MySetEvents()
        {
            UpDownR.MyValueChanged += UpDownRGB_MyValueChanged;
            UpDownG.MyValueChanged += UpDownRGB_MyValueChanged;
            UpDownB.MyValueChanged += UpDownRGB_MyValueChanged;

            UpDownH.MyValueChanged += UpDownHSV_MyValueChanged;
            UpDownS.MyValueChanged += UpDownHSV_MyValueChanged;
            UpDownV.MyValueChanged += UpDownHSV_MyValueChanged;

            
        }

        //HSV変更時
        private void UpDownHSV_MyValueChanged(object sender, MyValuechangedEventArgs e)
        {
            IsHsvChanging = true;
            if (IsRgbChanging != true)
            {
                NumericUpDown ud = (NumericUpDown)sender;
                if (ud == UpDownH) { SetImageSV((double)UpDownH.MyValue); }

                var s = (double)UpDownS.MyValue;
                var v = (double)UpDownV.MyValue;

                (R, G, B) = MathHSV.Hsv2rgb((double)UpDownH.MyValue, s, v);

            }
            IsHsvChanging = false;
        }


        //RGB変更時
        private void UpDownRGB_MyValueChanged(object sender, MyValuechangedEventArgs e)
        {
            IsRgbChanging = true;
            if (IsHsvChanging != true)
            {
                (double h, double s, double v) = MathHSV.RGB2hsv(R, G, B);
                if (UpDownH.MyValue != (int)h)
                {
                    UpDownH.MyValue = (int)h;
                    SetImageSV(h);
                }
                UpDownS.MyValue = (decimal)s;
                UpDownV.MyValue = (decimal)v;
            }
            IsRgbChanging = false;
        }

      
        private void MySetBinting()
        {
            //HSV -> Color
            SetBinding(PickupHSVProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(PickupColorProperty),
                Converter = new ConverterColor2HSV(),
            });

            //Color -> R,G,B,A
            Binding b0 = new() { Source = this, Path = new PropertyPath(RProperty) };
            Binding b1 = new() { Source = this, Path = new PropertyPath(GProperty) };
            Binding b2 = new() { Source = this, Path = new PropertyPath(BProperty) };
            Binding b3 = new() { Source = this, Path = new PropertyPath(AProperty) };
            MultiBinding mb = new();
            mb.Bindings.Add(b0);
            mb.Bindings.Add(b1);
            mb.Bindings.Add(b2);
            mb.Bindings.Add(b3);
            mb.Converter = new ConverterRGB2Color();
            SetBinding(PickupColorProperty, mb);

            //NumeR,G,B,A -> R,G,B,A
            UpDownR.SetBinding(NumericUpDown.MyValueProperty, new Binding() { Source = this, Path = new PropertyPath(RProperty) });
            UpDownG.SetBinding(NumericUpDown.MyValueProperty, new Binding() { Source = this, Path = new PropertyPath(GProperty) });
            UpDownB.SetBinding(NumericUpDown.MyValueProperty, new Binding() { Source = this, Path = new PropertyPath(BProperty) });
            UpDownA.SetBinding(NumericUpDown.MyValueProperty, new Binding() { Source = this, Path = new PropertyPath(AProperty) });

            SliderHue.SetBinding(Slider.ValueProperty, new Binding() { Source = UpDownH, Path = new PropertyPath(NumericUpDown.MyValueProperty) });

            
        }


        private void MyInitialize()
        {
            MyGrid.Margin = new Thickness(MarkerSize / 2, MarkerSize / 2, 0, 0);
            ImageHue.Source = GetImageHue((int)(SVImageSize / 2.0), (int)SVImageSize);
            ImageHue.Width = ImageHue.Source.Width;

            SliderHue.RenderTransform = new RotateTransform(180);

            SetImageSV(0);
            SetImageAlphaSource();

        }



        //Hue画像作成、起動時のみ使用。使用側でStretchすればサイズは幅1、高さ6でも実用できる
        private BitmapSource GetImageHue(int w, int h)
        {
            var wb = new WriteableBitmap(w, h, 96, 96, PixelFormats.Rgb24, null);
            int stride = wb.BackBufferStride;
            var pixels = new byte[h * stride];
            wb.CopyPixels(pixels, stride, 0);
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    int p = y * stride + (x * 3);
                    (pixels[p], pixels[p + 1], pixels[p + 2])
                        = MathHSV.Hsv2rgb(y / (double)h * 360.0, 1.0, 1.0);
                }
            }
            wb.WritePixels(new Int32Rect(0, 0, w, h), pixels, stride, 0);
            return wb;
        }

        //SV画像作成
        private void SetImageSV(double hue)
        {
            int size = 20;
            var wb = new WriteableBitmap(size, size, 96, 96, PixelFormats.Rgb24, null);
            int stride = wb.BackBufferStride;
            var pixels = new byte[size * stride];
            wb.CopyPixels(pixels, stride, 0);
            int p = 0;
            Parallel.For(0, size, y =>
            {
                ParallelImageSV(p, y, stride, pixels, hue, size, size);
            });

            wb.WritePixels(new Int32Rect(0, 0, size, size), pixels, stride, 0);

            ImageBrush ib = new(wb);
            ib.Stretch = Stretch.Uniform;
            ImageSV.Fill = ib;
        }
        private void ParallelImageSV(int p, int y, int stride, byte[] pixels, double hue, int w, int h)
        {
            for (int x = 0; x < w; ++x)
            {
                p = y * stride + (x * 3);
                (pixels[p], pixels[p + 1], pixels[p + 2])
                    = MathHSV.Hsv2rgb(hue, x / (double)w, y / (double)h);
            }
        }

        //市松模様画像
        private void SetImageAlphaSource()
        {
            int w = (int)ImageAlpha.Width;
            int h = (int)ImageAlpha.Height;
            var wb = new WriteableBitmap(w, h, 96, 96, PixelFormats.Bgr24, null);
            int stride = wb.BackBufferStride;
            var pixels = new byte[SVImageSize * stride];
            wb.CopyPixels(pixels, stride, 0);
            int x, y;
            for (int i = 0; i < pixels.Length; i += 3)
            {
                x = (i / (10 * 3)) % 2;
                y = (i / 10 / stride) % 2;

                byte color;
                if ((x == 0 && y == 0) || (x == 1 && y == 1))
                {
                    color = 255;
                }
                else
                {
                    color = 200;
                }
                pixels[i] = color;
                pixels[i + 1] = color;
                pixels[i + 2] = color;
            }
            wb.WritePixels(new Int32Rect(0, 0, w, h), pixels, stride, 0);
            ImageAlpha.Source = wb;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var color = PickupColor;
            var hsv = PickupHSV;
            //PickupColor = Color.FromArgb(200, 200, 0, 0);
            R = 200;
            G = 200;
            var mark = Marker.X;
            Marker.X = 50;
        }
    }

    internal class ConverterSV : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            HSV hsv= (HSV)values[0];//0.0 to 1.0
            int marker = (int)values[1];
            int image= (int)values[2];
            var saturation = image * hsv.S - (marker / 2.0);
            return saturation;//Left
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new Exception();
        }
    }
    internal class ConverterColor2HSV : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color c = (Color)value;
            return MathHSV.Color2HSV2(c);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HSV hsv = (HSV)value;
            return MathHSV.HSV2Color(hsv);
        }
    }

    internal class ConverterRGB2Color : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            byte r = (byte)values[0];
            byte g = (byte)values[1];
            byte b = (byte)values[2];
            byte a = (byte)values[3];
            return Color.FromArgb(a, r, g, b);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            Color c = (Color)value;
            return new object[] { c.R, c.G, c.B, c.A };
        }
    }

    internal class ConverterColor2SolidBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color c = (Color)value;
            return new SolidColorBrush(c);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = (SolidColorBrush)value;
            return brush.Color;
        }
    }
}
