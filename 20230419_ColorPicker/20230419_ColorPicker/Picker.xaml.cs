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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _20230419_ColorPicker
{
    /// <summary>
    /// Picker.xaml の相互作用ロジック
    /// </summary>
    public partial class Picker : Window
    {

        #region 依存関係プロパティ

        public Color PickColor
        {
            get { return (Color)GetValue(PickColorProperty); }
            set { SetValue(PickColorProperty, value); }
        }
        public static readonly DependencyProperty PickColorProperty =
            DependencyProperty.Register(nameof(PickColor), typeof(Color), typeof(Picker),
                new FrameworkPropertyMetadata(Color.FromArgb(0, 0, 0, 0),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public RGB MyRGB
        //{
        //    get { return (RGB)GetValue(MyRGBProperty); }
        //    set { SetValue(MyRGBProperty, value); }
        //}
        //public static readonly DependencyProperty MyRGBProperty =
        //    DependencyProperty.Register(nameof(MyRGB), typeof(RGB), typeof(Picker),
        //        new FrameworkPropertyMetadata(new RGB(),
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public HSV MyHSV
        //{
        //    get { return (HSV)GetValue(MyHSVProperty); }
        //    set { SetValue(MyHSVProperty, value); }
        //}
        //public static readonly DependencyProperty MyHSVProperty =
        //    DependencyProperty.Register(nameof(MyHSV), typeof(HSV), typeof(Picker),
        //        new FrameworkPropertyMetadata(new HSV(),
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// HSVを再計算、RGB変更時に使用
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnRGB(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Picker mw)
            {
                if (mw.IsHSVChangNow) return;
                mw.IsRGBChangNow = true;
                (mw.H, mw.S, mw.V) = MathHSV.RGB2hsv(mw.R, mw.G, mw.B);
                mw.MyImageSV.Source = mw.GetSVImage2(mw.H, mw.SVBitmapSize);
                mw.IsRGBChangNow = false;
            }
        }

        /// <summary>
        /// RGBを再計算、HSV変更時に使用
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnHSV(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Picker mw)
            {
                if (mw.IsRGBChangNow) return;
                mw.IsHSVChangNow = true;
                (mw.R, mw.G, mw.B) = MathHSV.Hsv2rgb(mw.H, mw.S, mw.V);
                mw.IsHSVChangNow = false;
            }
        }
        private static void OnHue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Picker mw)
            {
                if (mw.IsRGBChangNow) return;
                mw.IsHSVChangNow = true;
                (mw.R, mw.G, mw.B) = MathHSV.Hsv2rgb(mw.H, mw.S, mw.V);
                mw.MyImageSV.Source = mw.GetSVImage2(mw.H, mw.SVBitmapSize);
                mw.IsHSVChangNow = false;
            }
        }

        public byte R
        {
            get { return (byte)GetValue(RProperty); }
            set { SetValue(RProperty, value); }
        }
        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register(nameof(R), typeof(byte), typeof(Picker),
                new FrameworkPropertyMetadata(byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnRGB)));


        public byte G
        {
            get { return (byte)GetValue(GProperty); }
            set { SetValue(GProperty, value); }
        }
        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register(nameof(G), typeof(byte), typeof(Picker),
                new FrameworkPropertyMetadata(byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnRGB)));

        public byte B
        {
            get { return (byte)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register(nameof(B), typeof(byte), typeof(Picker),
                new FrameworkPropertyMetadata(byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnRGB)));

        public byte A
        {
            get { return (byte)GetValue(AProperty); }
            set { SetValue(AProperty, value); }
        }
        public static readonly DependencyProperty AProperty =
            DependencyProperty.Register(nameof(A), typeof(byte), typeof(Picker),
                new FrameworkPropertyMetadata(byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double H
        {
            get { return (double)GetValue(HProperty); }
            set { SetValue(HProperty, value); }
        }
        public static readonly DependencyProperty HProperty =
            DependencyProperty.Register(nameof(H), typeof(double), typeof(Picker),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnHue)));
        public double S
        {
            get { return (double)GetValue(SProperty); }
            set { SetValue(SProperty, value); }
        }
        public static readonly DependencyProperty SProperty =
            DependencyProperty.Register(nameof(S), typeof(double), typeof(Picker),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnHSV)));

        public double V
        {
            get { return (double)GetValue(VProperty); }
            set { SetValue(VProperty, value); }
        }
        public static readonly DependencyProperty VProperty =
            DependencyProperty.Register(nameof(V), typeof(double), typeof(Picker),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnHSV)));


        public double MarkerSize
        {
            get { return (double)GetValue(MarkerSizeProperty); }
            set { SetValue(MarkerSizeProperty, value); }
        }
        public static readonly DependencyProperty MarkerSizeProperty =
            DependencyProperty.Register(nameof(MarkerSize), typeof(double), typeof(Picker),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ

        ////無限ループ防止用フラグ
        private bool IsRGBChangNow;
        private bool IsHSVChangNow;
        //SVimageのBitmapSourceのサイズは32あれば十分？
        private readonly int SVBitmapSize = 32;
        public Marker Marker { get; set; }

        #region コンストラクタ

        public Picker()
        {
            InitializeComponent();

            DataContext = this;
            SetSliderBindings();

            SetMyBindings();
            Marker = new Marker(MyImageSV);
            MyImageSV.Stretch = Stretch.Fill;
            MultiBinding mb = new();
            PickColor = Color.FromArgb(200, 100, 202, 52);

            Loaded += Picker_Loaded;
            Closing += Picker_Closing;

        }


        //色指定あり
        public Picker(Color color) : this()
        {
            PickColor = color;
            //A = color.A; R = color.R; G = color.G; B = color.B;
            //(H, S, V) = MathHSV.Color2HSV(color);
        }
        #endregion コンストラクタ

        //外からの色の指定
        public void SetColor(Color color)
        {
            PickColor = color;
        }

        //Windowは閉じないで非表示
        private void Picker_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }



        private void Picker_Loaded(object sender, RoutedEventArgs e)
        {
            if (AdornerLayer.GetAdornerLayer(MyImageSV) is AdornerLayer layer)
            {
                layer.Add(Marker);
            }
            //PickColor = Color.FromArgb(255, 255, 255, 255);
            //
            SetMarkerBinding();
        }

        private void SetMarkerBinding()
        {
            SetBinding(SProperty, new Binding() { Source = Marker, Path = new PropertyPath(Marker.SaturationProperty) });
            SetBinding(VProperty, new Binding() { Source = Marker, Path = new PropertyPath(Marker.ValueProperty) });

            Marker.SetBinding(Marker.MarkerSizeProperty, new Binding() { Source = this, Path = new PropertyPath(MarkerSizeProperty) });
            MarkerSize = 40;
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

        private void SetMyBindings()
        {
            MultiBinding mb = new();
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(AProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(RProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(GProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(BProperty) });
            mb.Converter = new ConverterARGB2Color();
            SetBinding(PickColorProperty, mb);

            MyBorderPickColorSample.SetBinding(BackgroundProperty, new Binding() { Source = this, Path = new PropertyPath(PickColorProperty), Converter = new ConverterColor2Brush() }); ;
            //MyImageSV.SetBinding(BackgroundProperty, new Binding() { Source = this, Path = new PropertyPath(PickColorProperty), Converter = new ConverterColor2Brush() });
        }

        private void SetSliderBindings()
        {
            MySliderA.SetBinding(Slider.ValueProperty, new Binding() { Source = this, Path = new PropertyPath(AProperty) });
            MySliderR.SetBinding(Slider.ValueProperty, new Binding() { Source = this, Path = new PropertyPath(RProperty) });
            MySliderG.SetBinding(Slider.ValueProperty, new Binding() { Source = this, Path = new PropertyPath(GProperty) });
            MySliderB.SetBinding(Slider.ValueProperty, new Binding() { Source = this, Path = new PropertyPath(BProperty) });
            MySliderH.SetBinding(Slider.ValueProperty, new Binding() { Source = this, Path = new PropertyPath(HProperty) });
            MySliderS.SetBinding(Slider.ValueProperty, new Binding() { Source = this, Path = new PropertyPath(SProperty) });
            MySliderV.SetBinding(Slider.ValueProperty, new Binding() { Source = this, Path = new PropertyPath(VProperty) });

        }


    }

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
}
