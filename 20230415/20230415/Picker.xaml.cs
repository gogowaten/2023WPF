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

namespace _20230415
{
    /// <summary>
    /// Picker.xaml の相互作用ロジック
    /// </summary>
    public partial class Picker : Window
    {

        #region 依存関係プロパティ

        public Color MainColor
        {
            get { return (Color)GetValue(MainColorProperty); }
            set { SetValue(MainColorProperty, value); }
        }
        public static readonly DependencyProperty MainColorProperty =
            DependencyProperty.Register(nameof(MainColor), typeof(Color), typeof(Picker),
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
            DependencyProperty.Register(nameof(MyRGB), typeof(RGB), typeof(Picker),
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
            DependencyProperty.Register(nameof(MyHSV), typeof(HSV), typeof(Picker),
                new FrameworkPropertyMetadata(new HSV(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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
                    new PropertyChangedCallback(OnHSV)));
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
        #endregion 依存関係プロパティ

        ////無限ループ防止用フラグ
        private bool IsRGBChangNow;
        private bool IsHSVChangNow;
        public Marker Marker { get; set; }
        public Picker()
        {
            InitializeComponent();

            DataContext = this;
            SetSliderBindings();

            SetMyBindings();
            Marker = new Marker(MyBorderColor);

            Loaded += Picker_Loaded;
        }

        private void Picker_Loaded(object sender, RoutedEventArgs e)
        {
            if (AdornerLayer.GetAdornerLayer(MyBorderColor) is AdornerLayer layer)
            {
                layer.Add(Marker);
            }
            MainColor = Color.FromArgb(255, 255, 255, 255);

            MarkerBinding();
        }

        private void MarkerBinding()
        {
            SetBinding(SProperty, new Binding() { Source = Marker, Path = new PropertyPath(Marker.SaturationProperty) });
            SetBinding(VProperty, new Binding() { Source = Marker, Path = new PropertyPath(Marker.ValueProperty) });

            //MultiBinding mb = new();
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(SProperty) });
            //mb.Bindings.Add(new Binding() { Source = Marker, Path = new PropertyPath(Marker.MarkerSizeProperty) });
            //mb.Bindings.Add(new Binding() { Source = MyBorderColor, Path = new PropertyPath(WidthProperty) });
            //mb.Converter = new ConverterSV2TopLeft();
            //Marker.MarkerThumb.SetBinding(Canvas.LeftProperty, mb);


            //mb = new();
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(VProperty) });
            //mb.Bindings.Add(new Binding() { Source = Marker, Path = new PropertyPath(Marker.MarkerSizeProperty) });
            //mb.Bindings.Add(new Binding() { Source = MyBorderColor, Path = new PropertyPath(HeightProperty) });
            //mb.Converter = new ConverterSV2TopLeft();
            //Marker.MarkerThumb.SetBinding(Canvas.TopProperty, mb);

        }
        //private void MarkerBinding()
        //{
        //    MultiBinding mb = new();
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(SProperty) });
        //    mb.Bindings.Add(new Binding() { Source = Marker, Path = new PropertyPath(Marker.MarkerSizeProperty) });
        //    mb.Bindings.Add(new Binding() { Source = MyBorderColor, Path = new PropertyPath(WidthProperty) });
        //    mb.Converter = new ConverterSV2TopLeft();
        //    Marker.MarkerThumb.SetBinding(Canvas.LeftProperty, mb);


        //    mb = new();
        //    mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(VProperty) });
        //    mb.Bindings.Add(new Binding() { Source = Marker, Path = new PropertyPath(Marker.MarkerSizeProperty) });
        //    mb.Bindings.Add(new Binding() { Source = MyBorderColor, Path = new PropertyPath(HeightProperty) });
        //    mb.Converter = new ConverterSV2TopLeft();
        //    Marker.MarkerThumb.SetBinding(Canvas.TopProperty, mb);

        //}

        private void SetMyBindings()
        {
            MultiBinding mb = new();
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(AProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(RProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(GProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(BProperty) });
            mb.Converter = new ConverterARGB2Color();
            SetBinding(MainColorProperty, mb);

            MyBorderColor.SetBinding(BackgroundProperty, new Binding() { Source = this, Path = new PropertyPath(MainColorProperty), Converter = new ConverterColor2Brush() });
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

    public class ConverterSV2TopLeft : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double sv = (double)values[0];
            double markerSize = (double)values[1];
            double targetSize = (double)values[2];
            double result = (sv * targetSize) - (markerSize / 2.0);
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
