using System;
using System.Collections.Generic;
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


namespace _20230411_ColorPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public Color MyColor
        {
            get { return (Color)GetValue(MyColorProperty); }
            set { SetValue(MyColorProperty, value); }
        }
        public static readonly DependencyProperty MyColorProperty =
            DependencyProperty.Register("MyColor", typeof(Color), typeof(MainWindow),
                new FrameworkPropertyMetadata(Color.FromArgb(0, 0, 0, 0),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public HSV MyHSV
        {
            get { return (HSV)GetValue(MyHSVProperty); }
            set { SetValue(MyHSVProperty, value); }
        }
        public static readonly DependencyProperty MyHSVProperty =
            DependencyProperty.Register(nameof(MyHSV), typeof(HSV), typeof(MainWindow),
                new FrameworkPropertyMetadata(new HSV(0, 0, 0),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        private MarkerAdorner MarkerAdorner { get; set; }
        private bool IsHsvChanging = false;
        private bool IsRGBChanging = false;


        public MainWindow()
        {
            InitializeComponent();
            MarkerAdorner = new(MyBorder);

            Loaded += MainWindow_Loaded;
            WriteableBitmap bitmap = new(2, 2, 96, 96, PixelFormats.Rgb24, null);
            SetMyBinding();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MyBorderHue.Background = new ImageBrush(GetHueImage(1, 6));
            MyBorder.Background = new ImageBrush(GetSVImage2(30, 32));

            MyScrollBarR.ValueChanged += MyScrollBarR_ValueChanged;
            MyScrollBarG.ValueChanged += MyScrollBarR_ValueChanged;
            MyScrollBarB.ValueChanged += MyScrollBarR_ValueChanged;

            MyScrollBarH.ValueChanged += MyScrollBarH_ValueChanged;
            MyScrollBarS.ValueChanged += MyScrollBarS_ValueChanged;
            MyScrollBarV.ValueChanged += MyScrollBarS_ValueChanged;

            if (AdornerLayer.GetAdornerLayer(MyBorder) is AdornerLayer layer)
            {
                layer.Add(MarkerAdorner);
            }
            MarkerAdorner.SetBinding(MarkerAdorner.XProperty, new Binding()
            {
                Source = MyScrollBarS,
                Path = new PropertyPath(ScrollBar.ValueProperty)
            });
            MarkerAdorner.SetBinding(MarkerAdorner.YProperty, new Binding()
            {
                Source = MyScrollBarV,
                Path = new PropertyPath(ScrollBar.ValueProperty)
            });

            MyEllipse.SetBinding(Ellipse.FillProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyColorProperty),
                Converter =new ConverterSolidBruhs(),
            });
        }

        private void MyScrollBarS_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsRGBChanging) return;
            IsHsvChanging = true;
            SetRGBScrollBarValue();
            //MyHSV = new(MyScrollBarH.Value, MyScrollBarS.Value, MyScrollBarV.Value);
            //var (r, g, b) = MathHSV.HSV2RGB(MyHSV);
            //MyScrollBarR.Value = r;
            //MyScrollBarG.Value = g;
            //MyScrollBarB.Value = b;
            IsHsvChanging = false;
        }

        private void MyScrollBarH_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsRGBChanging) return;
            IsHsvChanging = true;
            SetRGBScrollBarValue();
            //MyHSV = new(MyScrollBarH.Value, MyScrollBarS.Value, MyScrollBarV.Value);
            MyBorder.Background = new ImageBrush(GetSVImage2(MyHSV.H, 32));
            //var (r, g, b) = MathHSV.HSV2RGB(MyHSV);
            //MyScrollBarR.Value = r;
            //MyScrollBarG.Value = g;
            //MyScrollBarB.Value = b;
            IsHsvChanging = false;
        }
        private void SetRGBScrollBarValue()
        {
            MyHSV = new(MyScrollBarH.Value, MyScrollBarS.Value, MyScrollBarV.Value);
            var (r, g, b) = MathHSV.HSV2RGB(MyHSV);
            MyScrollBarR.Value = r;
            MyScrollBarG.Value = g;
            MyScrollBarB.Value = b;
        }
        private void MyScrollBarR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            IsRGBChanging = true;
            if (IsHsvChanging == false)
            {
                MyColor = Color.FromArgb(255, (byte)MyScrollBarR.Value, (byte)MyScrollBarG.Value, (byte)MyScrollBarB.Value);
                MyBorder.Background = new ImageBrush(GetSVImage2(MyHSV.H, 32));
                var (h, s, v) = MathHSV.Color2HSV(MyColor);
                MyScrollBarH.Value = h;
                MyScrollBarS.Value = s;
                MyScrollBarV.Value = v;
            }
            IsRGBChanging = false;
        }

        private void SetMyBinding()
        {
            SetBinding(MyColorProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyHSVProperty),
                Mode = BindingMode.TwoWay,
                Converter = new ConverterHSV2Color()
            });
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



        private BitmapSource GetSVImage(int w, int h)
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
                    Color hue = MathHSV.HSV2Color(y / (float)h * 360f, 1f, 0.9f);
                    pixels[p] = hue.R;
                    pixels[p + 1] = hue.G;
                    pixels[p + 2] = hue.B;
                }
            }
            wb.WritePixels(new Int32Rect(0, 0, w, h), pixels, stride, 0);
            return wb;
        }

        private BitmapSource GetHueImage(int w, int h)
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
                    Color hue = MathHSV.HSV2Color(y / (float)h * 360f, 1f, 0.9f);
                    pixels[p] = hue.R;
                    pixels[p + 1] = hue.G;
                    pixels[p + 2] = hue.B;
                }
            }
            wb.WritePixels(new Int32Rect(0, 0, w, h), pixels, stride, 0);
            return wb;
        }

        private void ScrollRGBBinding()
        {
            MyScrollBarR.SetBinding(ScrollBar.ValueProperty, new Binding()
            {
                Source = MyBorder,
                Path = new PropertyPath(BackgroundProperty),
                Converter = new ConverterR(),
                ConverterParameter = MyBorder.Background,
            });
            MyScrollBarG.SetBinding(ScrollBar.ValueProperty, new Binding()
            {
                Source = MyBorder,
                Path = new PropertyPath(BackgroundProperty),
                Converter = new ConverterG(),
                ConverterParameter = MyBorder.Background,
            });
            MyScrollBarB.SetBinding(ScrollBar.ValueProperty, new Binding()
            {
                Source = MyBorder,
                Path = new PropertyPath(BackgroundProperty),
                Converter = new ConverterB(),
                ConverterParameter = MyBorder.Background,
            });

        }

        private void MyScrollBarHsv_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //(byte r, byte g, byte b) = HSV.HSV2RGB(MyScrollBarH.Value, MyScrollBarS.Value, MyScrollBarV.Value);
            //MyScrollBarR.Value = r;
            //MyScrollBarG.Value = g;
            //MyScrollBarB.Value = b;
        }

        private void MyScrollBarRGB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            //(double h, double s, double v) = HSV.RGB2HSV(MyScrollBarR.Value, MyScrollBarG.Value, MyScrollBarB.Value);
            //MyScrollBarH.Value = h;
            //MyScrollBarS.Value = s;
            //MyScrollBarV.Value = v;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var neko = MyColor;
            var inu = MyHSV;
            MyBorder.Width = 150;
        }
    }
}
