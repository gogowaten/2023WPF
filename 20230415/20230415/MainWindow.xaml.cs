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
//ARGBとHSVはSliderのValueにBinding、ValueChangeイベントで相手の値を変更、
//このとき無限ループにならないようにフラグを使用

namespace _20230415
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 依存関係プロパティ

        public ARGBHSV2 MyARGBHSV2
        {
            get { return (ARGBHSV2)GetValue(MyARGBHSV2Property); }
            set { SetValue(MyARGBHSV2Property, value); }
        }
        public static readonly DependencyProperty MyARGBHSV2Property =
            DependencyProperty.Register(nameof(MyARGBHSV2), typeof(ARGBHSV2), typeof(MainWindow),
                new FrameworkPropertyMetadata(new ARGBHSV2(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ARGBHSV MyARGBHSV
        {
            get { return (ARGBHSV)GetValue(MyARGBHSVProperty); }
            set { SetValue(MyARGBHSVProperty, value); }
        }
        public static readonly DependencyProperty MyARGBHSVProperty =
            DependencyProperty.Register(nameof(MyARGBHSV), typeof(ARGBHSV), typeof(MainWindow),
                new FrameworkPropertyMetadata(new ARGBHSV(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Color MyColor
        {
            get { return (Color)GetValue(MyColorProperty); }
            set { SetValue(MyColorProperty, value); }
        }
        public static readonly DependencyProperty MyColorProperty =
            DependencyProperty.Register(nameof(MyColor), typeof(Color), typeof(MainWindow),
                new FrameworkPropertyMetadata(Color.FromArgb(0, 0, 0, 0),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public RGB MyRGB
        {
            get { return (RGB)GetValue(MyRGBProperty); }
            set { SetValue(MyRGBProperty, value); }
        }
        public static readonly DependencyProperty MyRGBProperty =
            DependencyProperty.Register(nameof(MyRGB), typeof(RGB), typeof(MainWindow),
                new FrameworkPropertyMetadata(new RGB(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public HSV MyHSV
        {
            get { return (HSV)GetValue(MyHSVProperty); }
            set { SetValue(MyHSVProperty, value); }
        }
        public static readonly DependencyProperty MyHSVProperty =
            DependencyProperty.Register(nameof(MyHSV), typeof(HSV), typeof(MainWindow),
                new FrameworkPropertyMetadata(new HSV(),
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

        public double H
        {
            get { return (double)GetValue(HProperty); }
            set { SetValue(HProperty, value); }
        }
        public static readonly DependencyProperty HProperty =
            DependencyProperty.Register(nameof(H), typeof(double), typeof(MainWindow),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double S
        {
            get { return (double)GetValue(SProperty); }
            set { SetValue(SProperty, value); }
        }
        public static readonly DependencyProperty SProperty =
            DependencyProperty.Register(nameof(S), typeof(double), typeof(MainWindow),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double V
        {
            get { return (double)GetValue(VProperty); }
            set { SetValue(VProperty, value); }
        }
        public static readonly DependencyProperty VProperty =
            DependencyProperty.Register(nameof(V), typeof(double), typeof(MainWindow),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存関係プロパティ

        ////無限ループ防止用フラグ
        //private bool isRGBChangNow;
        //private bool isHSVChangNow;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            SetSliderBindings();
            
            SetMyBindings();
            Loaded += (s, e) => { MyARGBHSV2.A = 255; };
        }
        private void SetMyBindings2()
        {
            MultiBinding mb = new();
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(HProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(SProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(VProperty) });
            mb.Converter = new ConverterHSV2HSV();
            SetBinding(MyHSVProperty, mb);

            SetBinding(MyRGBProperty, new Binding() { Source = this, Path = new PropertyPath(MyHSVProperty), Converter = new ConverterHSV2RGB() });

            //MySliderR.ValueChanged += MySliderR_ValueChanged;
            SetBinding(RProperty, new Binding() { Source = this, Path = new PropertyPath(MyRGBProperty), Converter = new ConverterRR(), Mode = BindingMode.OneWay });
            SetBinding(GProperty, new Binding() { Source = this, Path = new PropertyPath(MyRGBProperty), Converter = new ConverterGG(), Mode = BindingMode.OneWay });
            SetBinding(BProperty, new Binding() { Source = this, Path = new PropertyPath(MyRGBProperty), Converter = new ConverterBB(), Mode = BindingMode.OneWay });
        }
        private void SetMybindings0()
        {
            SetBinding(HProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyRGBProperty),
                Mode = BindingMode.OneWay,
                Converter = new ConverterH()
            });
            SetBinding(SProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyRGBProperty),
                Mode = BindingMode.OneWay,
                Converter = new ConverterS()
            });
            SetBinding(VProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyRGBProperty),
                Mode = BindingMode.OneWay,
                Converter = new ConverterV()
            });

            MultiBinding mb = new();
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(HProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(SProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(VProperty) });
            mb.Converter = new ConverterHSV2HSV();

            SetBinding(MyHSVProperty, mb);

            SetBinding(RProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyHSVProperty),
                Mode = BindingMode.OneWay,
                Converter = new ConverterR()
            });
            SetBinding(GProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyHSVProperty),
                Mode = BindingMode.OneWay,
                Converter = new ConverterG()
            });
            SetBinding(BProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyHSVProperty),
                Mode = BindingMode.OneWay,
                Converter = new ConverterB()
            });

            mb = new();
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(RProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(GProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(BProperty) });
            mb.Converter = new ConverterRGB2RGB();

            SetBinding(MyRGBProperty, mb);

        }
        private void SetMyBindings1()
        {
            MultiBinding mb;
            mb = new();
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(RProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(GProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(BProperty) });
            mb.Converter = new ConverterRGB2HSV();
            SetBinding(MyHSVProperty, mb);

            SetBinding(HProperty, new Binding() { Source = this, Path = new PropertyPath(MyHSVProperty), Converter = new ConverterHH(), Mode = BindingMode.OneWay });
            SetBinding(SProperty, new Binding() { Source = this, Path = new PropertyPath(MyHSVProperty), Converter = new ConverterSS(), Mode = BindingMode.OneWay });
            SetBinding(VProperty, new Binding() { Source = this, Path = new PropertyPath(MyHSVProperty), Converter = new ConverterVV(), Mode = BindingMode.OneWay });

            mb = new();
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(HProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(SProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(VProperty) });
            mb.Converter = new ConverterHsv2RGB();
            SetBinding(MyRGBProperty, mb);

            SetBinding(RProperty, new Binding() { Source = this, Path = new PropertyPath(MyRGBProperty), Converter = new ConverterRR(), Mode = BindingMode.OneWay });
            SetBinding(GProperty, new Binding() { Source = this, Path = new PropertyPath(MyRGBProperty), Converter = new ConverterGG(), Mode = BindingMode.OneWay });
            SetBinding(BProperty, new Binding() { Source = this, Path = new PropertyPath(MyRGBProperty), Converter = new ConverterBB(), Mode = BindingMode.OneWay });

        }
        private void SetMyBindings()
        {
            SetBinding(AProperty, new Binding(nameof(MyARGBHSV2.A)) { Source = MyARGBHSV2 });
            SetBinding(RProperty, new Binding(nameof(MyARGBHSV2.R)) { Source = MyARGBHSV2 });
            SetBinding(GProperty, new Binding(nameof(MyARGBHSV2.G)) { Source = MyARGBHSV2 });
            SetBinding(BProperty, new Binding(nameof(MyARGBHSV2.B)) { Source = MyARGBHSV2 });
            SetBinding(HProperty, new Binding(nameof(MyARGBHSV2.H)) { Source = MyARGBHSV2 });
            SetBinding(SProperty, new Binding(nameof(MyARGBHSV2.S)) { Source = MyARGBHSV2 });
            SetBinding(VProperty, new Binding(nameof(MyARGBHSV2.V)) { Source = MyARGBHSV2 });

            MultiBinding mb = new();
            mb.Bindings.Add(new Binding(nameof(MyARGBHSV2.A)) { Source = MyARGBHSV2 });
            mb.Bindings.Add(new Binding(nameof(MyARGBHSV2.R)) { Source = MyARGBHSV2 });
            mb.Bindings.Add(new Binding(nameof(MyARGBHSV2.G)) { Source = MyARGBHSV2 });
            mb.Bindings.Add(new Binding(nameof(MyARGBHSV2.B)) { Source = MyARGBHSV2 });
            mb.Converter = new ConverterARGB2Color();
            SetBinding(MyColorProperty, mb);

            mb = new();
            mb.Bindings.Add(new Binding(nameof(MyARGBHSV2.A)) { Source = MyARGBHSV2 });
            mb.Bindings.Add(new Binding(nameof(MyARGBHSV2.R)) { Source = MyARGBHSV2 });
            mb.Bindings.Add(new Binding(nameof(MyARGBHSV2.G)) { Source = MyARGBHSV2 });
            mb.Bindings.Add(new Binding(nameof(MyARGBHSV2.B)) { Source = MyARGBHSV2 });
            mb.Converter = new ConverterARGB2SolidColorBrush();
            MyBorderColor.SetBinding(BackgroundProperty, mb);
        }

       
        //private void MySetARGBHSVBindings3()
        //{
        //    MultiBinding mb = new();
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(AProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyRGBProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyHSVProperty) });
        //    mb.Converter = new ConverterARGB2Color();
        //    SetBinding(MyARGBHSVProperty, mb);

        //}
        //private void MySetARGBHSVBindings2()
        //{
        //    MultiBinding mb = new();
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(AProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(RProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(GProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(BProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(HProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(SProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(VProperty) });
        //    mb.Converter = new ConverterARGBHSV();
        //    SetBinding(MyARGBHSVProperty, mb);

        //}

        //private void MySetARGBHSVBindings1()
        //{
        //    MultiBinding mb = new();
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyColorProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyHSVProperty) });
        //    mb.Converter = new ConverterColorHSV2ARGBHSV();
        //    SetBinding(MyARGBHSVProperty, mb);

        //}
        //private void MyRGBFromHSVBindings()
        //{
        //    MultiBinding mb = new();
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(HProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(SProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(VProperty) });
        //    mb.Converter = new ConverterHsv2RGB();
        //    SetBinding(MyRGBProperty, mb);
        //}

        //private void MyHSVFromRGBBindings()
        //{
        //    MultiBinding mb = new();
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(RProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(GProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(BProperty) });
        //    mb.Converter = new ConverterRGB2HSV();
        //    SetBinding(MyHSVProperty, mb);
        //}
        //private void MySetMyHSVBindings()
        //{
        //    MultiBinding mb = new();
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(HProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(SProperty) });
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(VProperty) });
        //    mb.Converter = new ConverterHSV2HSV();
        //    SetBinding(MyHSVProperty, mb);
        //}


        private void SetSliderBindings()
        {
            MySliderA.SetBinding(Slider.ValueProperty, new Binding(nameof(ARGBHSV2.A)) { Source = MyARGBHSV2, Mode = BindingMode.TwoWay });
            MySliderR.SetBinding(Slider.ValueProperty, new Binding(nameof(ARGBHSV2.R)) { Source = MyARGBHSV2, Mode = BindingMode.TwoWay });
            MySliderG.SetBinding(Slider.ValueProperty, new Binding(nameof(ARGBHSV2.G)) { Source = MyARGBHSV2, Mode = BindingMode.TwoWay });
            MySliderB.SetBinding(Slider.ValueProperty, new Binding(nameof(ARGBHSV2.B)) { Source = MyARGBHSV2, Mode = BindingMode.TwoWay });
            MySliderH.SetBinding(Slider.ValueProperty, new Binding(nameof(ARGBHSV2.H)) { Source = MyARGBHSV2, Mode = BindingMode.TwoWay });
            MySliderS.SetBinding(Slider.ValueProperty, new Binding(nameof(ARGBHSV2.S)) { Source = MyARGBHSV2, Mode = BindingMode.TwoWay });
            MySliderV.SetBinding(Slider.ValueProperty, new Binding(nameof(ARGBHSV2.V)) { Source = MyARGBHSV2, Mode = BindingMode.TwoWay });

        }

        //private void SetSliderBindings()
        //{
        //    MySliderA.SetBinding(Slider.ValueProperty, new Binding() { Source = this, Path = new PropertyPath(AProperty) });
        //    MySliderR.SetBinding(Slider.ValueProperty, new Binding() { Source = this, Path = new PropertyPath(RProperty) });
        //    MySliderG.SetBinding(Slider.ValueProperty, new Binding() { Source = this, Path = new PropertyPath(GProperty) });
        //    MySliderB.SetBinding(Slider.ValueProperty, new Binding() { Source = this, Path = new PropertyPath(BProperty) });
        //    MySliderH.SetBinding(Slider.ValueProperty, new Binding() { Source = this, Path = new PropertyPath(HProperty) });
        //    MySliderS.SetBinding(Slider.ValueProperty, new Binding() { Source = this, Path = new PropertyPath(SProperty) });
        //    MySliderV.SetBinding(Slider.ValueProperty, new Binding() { Source = this, Path = new PropertyPath(VProperty) });

        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var myc = MyColor;
            var all = MyARGBHSV2;
            MyARGBHSV2.V = 0.5;
            R = 200;
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
