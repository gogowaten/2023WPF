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

        public HSV MyHsv
        {
            get { return (HSV)GetValue(MyHsvProperty); }
            set { SetValue(MyHsvProperty, value); }
        }
        public static readonly DependencyProperty MyHsvProperty =
            DependencyProperty.Register(nameof(MyHsv), typeof(HSV), typeof(MainWindow),
                new FrameworkPropertyMetadata(new HSV(0, 0, 0),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));





        public MainWindow()
        {
            InitializeComponent();
            
            
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MyBorderHue.Background = new ImageBrush(GetHueImage(1, 6));
        }

        private BitmapSource GetSVImage( int w ,int h)
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
        
        private BitmapSource GetHueImage(int w ,int h)
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


    }
}
