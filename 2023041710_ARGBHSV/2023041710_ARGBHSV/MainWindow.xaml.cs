using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

//RGBとHSVの相互変換
//依存関係プロパティは、ARGBがbyte型、HSVはdouble型、Color型とHSV型
//RGBのどれかを変更したらHSVを再計算
//HSVのどれかを変更したらRGBを再計算
//このとき無限ループにならないようにフラグで管理
namespace _2023041710_ARGBHSV
{

    public partial class MainWindow : Window
    {
        #region 依存関係プロパティ

        public Color MainColor
        {
            get { return (Color)GetValue(MainColorProperty); }
            set { SetValue(MainColorProperty, value); }
        }
        public static readonly DependencyProperty MainColorProperty =
            DependencyProperty.Register(nameof(MainColor), typeof(Color), typeof(MainWindow),
                new FrameworkPropertyMetadata(Color.FromArgb(0, 0, 0, 0),
                    FrameworkPropertyMetadataOptions.AffectsRender |
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
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnRGB)));


        public byte G
        {
            get { return (byte)GetValue(GProperty); }
            set { SetValue(GProperty, value); }
        }
        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register(nameof(G), typeof(byte), typeof(MainWindow),
                new FrameworkPropertyMetadata(byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnRGB)));

        public byte B
        {
            get { return (byte)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register(nameof(B), typeof(byte), typeof(MainWindow),
                new FrameworkPropertyMetadata(byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnRGB)));

        public byte A
        {
            get { return (byte)GetValue(AProperty); }
            set { SetValue(AProperty, value); }
        }
        public static readonly DependencyProperty AProperty =
            DependencyProperty.Register(nameof(A), typeof(byte), typeof(MainWindow),
                new FrameworkPropertyMetadata(byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double H
        {
            get { return (double)GetValue(HProperty); }
            set { SetValue(HProperty, value); }
        }
        public static readonly DependencyProperty HProperty =
            DependencyProperty.Register(nameof(H), typeof(double), typeof(MainWindow),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnHSV)));
        public double S
        {
            get { return (double)GetValue(SProperty); }
            set { SetValue(SProperty, value); }
        }
        public static readonly DependencyProperty SProperty =
            DependencyProperty.Register(nameof(S), typeof(double), typeof(MainWindow),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnHSV)));

        public double V
        {
            get { return (double)GetValue(VProperty); }
            set { SetValue(VProperty, value); }
        }
        public static readonly DependencyProperty VProperty =
            DependencyProperty.Register(nameof(V), typeof(double), typeof(MainWindow),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnHSV)));

        /// <summary>
        /// HSVを再計算、RGB変更時に使用
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnRGB(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainWindow mw)
            {
                if (mw.isHSVChangNow) return;
                mw.isRGBChangNow = true;
                (mw.H, mw.S, mw.V) = MathHSV.RGB2hsv(mw.R, mw.G, mw.B);
                mw.isRGBChangNow = false;
            }
        }

        /// <summary>
        /// RGBを再計算、HSV変更時に使用
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnHSV(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainWindow mw)
            {
                if (mw.isRGBChangNow) return;
                mw.isHSVChangNow = true;
                (mw.R, mw.G, mw.B) = MathHSV.Hsv2rgb(mw.H, mw.S, mw.V);
                mw.isHSVChangNow = false;
            }
        }
        #endregion 依存関係プロパティ

        //無限ループ防止用フラグ
        private bool isRGBChangNow;
        private bool isHSVChangNow;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            SetSliderBindings();

            SetMyBindings();
            Loaded += (s, e) => { A = 255; };
        }


        private void SetMyBindings()
        {
            MultiBinding mb = new();
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(AProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(RProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(GProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(BProperty) });
            mb.Converter = new ConverterARGB2Color();
            SetBinding(MainColorProperty, mb);

            MyBorderColor.SetBinding(BackgroundProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MainColorProperty),
                Converter = new ConverterColor2Brush()
            });
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var myc = MainColor;
            R = 200;
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
