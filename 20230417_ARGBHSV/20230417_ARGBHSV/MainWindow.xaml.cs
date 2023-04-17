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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20230417_ARGBHSV
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
        private static void OnARGB(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
      
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
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            SetSliderBindings();

            SetMyBindings();
            Loaded += (s, e) => { MyARGBHSV2.A = 255; };
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var myc = MyColor;
            var all = MyARGBHSV2;
            MyARGBHSV2.V = 0.5;
            R = 200;
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



    }
}
