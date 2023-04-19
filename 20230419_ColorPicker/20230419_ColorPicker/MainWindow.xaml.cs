using System;
using System.Collections.Generic;
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

namespace _20230419_ColorPicker
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
}
