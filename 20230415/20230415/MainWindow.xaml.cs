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

namespace _20230415
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 依存関係プロパティ
        
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

        //無限ループ防止用フラグ
        private bool isRGBChangNow = false;
        private bool isHSVChangNow = false;
        public MainWindow()
        {
            InitializeComponent();
            SetMyBindings();

        }
        private void SetMyBindings()
        {

            MyBorderColor.SetBinding(BackgroundProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyColorProperty),
                Converter = new ConverterColor2Brush(),
            });

            SetARGBBindings();
            SetHSVBindings();
            SetSliderBindings();

            MySliderH.ValueChanged += MySliderH_ValueChanged;
            MySliderS.ValueChanged += MySliderH_ValueChanged;
            MySliderV.ValueChanged += MySliderH_ValueChanged;
            MySliderR.ValueChanged += MySliderR_ValueChanged;
            MySliderG.ValueChanged += MySliderR_ValueChanged;
            MySliderB.ValueChanged += MySliderR_ValueChanged;
        }

        private void MySliderR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //HSV変更中はRGBを変更しない
            if (isHSVChangNow) return;
            isRGBChangNow = true;
            (H, S, V) = MathHSV.RGB2hsv(R, G, B);
            isRGBChangNow = false;
        }

        private void MySliderH_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //RGB変更中はHSVを変更しない
            if (isRGBChangNow) return;
            isHSVChangNow = true;
            (R, G, B) = MathHSV.Hsv2rgb(H, S, V);
            isHSVChangNow = false;
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
        private void SetARGBBindings()
        {
            Binding b0 = new() { Source = this, Path = new PropertyPath(AProperty) };
            Binding b1 = new() { Source = this, Path = new PropertyPath(RProperty) };
            Binding b2 = new() { Source = this, Path = new PropertyPath(GProperty) };
            Binding b3 = new() { Source = this, Path = new PropertyPath(BProperty) };
            MultiBinding mb = new();
            mb.Converter = new ConverterARGB2Color();
            mb.Bindings.Add(b0);
            mb.Bindings.Add(b1);
            mb.Bindings.Add(b2);
            mb.Bindings.Add(b3);
            SetBinding(MyColorProperty, mb);
        }

        private void SetHSVBindings()
        {
            Binding b1 = new() { Source = this, Path = new PropertyPath(HProperty) };
            Binding b2 = new() { Source = this, Path = new PropertyPath(SProperty) };
            Binding b3 = new() { Source = this, Path = new PropertyPath(VProperty) };
            MultiBinding mb = new();
            mb.Bindings.Add(b1);
            mb.Bindings.Add(b2);
            mb.Bindings.Add(b3);
            mb.Converter = new ConverterHSV2HSV();
            SetBinding(MyHSVProperty, mb);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var neko = MyBorderColor.Background;
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
            object[] result = new object[2];
            result[0] = hsv.H;
            result[1] = hsv.S;
            result[2] = hsv.V;
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
            Color c = Color.FromArgb(a, r, g, b);
            return c;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            Color c = (Color)value;

            object[] result = new object[3];
            result[0] = c.A;
            result[1] = c.R;
            result[2] = c.G;
            result[3] = c.B;
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
