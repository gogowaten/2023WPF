using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace _20230622Opacity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string MyDirectory { get; private set; } = "";
        public MainWindow()
        {
            InitializeComponent();
            Drop += MainWindow_Drop;
        }

        private void MainWindow_Drop(object sender, DragEventArgs e)
        {
            object fd = e.Data.GetData(DataFormats.FileDrop);
            string[] ff = (string[])e.Data.GetData(DataFormats.FileDrop);
            string dir = string.Empty;
            if (ff != null && ff.Length > 0)
            {
                if (File.GetAttributes(ff[0]).HasFlag(FileAttributes.Directory))
                {
                    dir = ff[0];
                }
                else
                {
                    var ext = System.IO.Path.GetExtension(ff[0]);
                    var dirr = System.IO.Path.GetDirectoryName(ff[0]);
                    if (Directory.Exists(dirr)) { dir = dirr; }
                }
                MyDirectory = dir;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MyDirectory))
            {
                Directory.CreateDirectory()
                var files = Directory.GetFiles(MyDirectory, "*.png");
                foreach (var file in files)
                {
                    BitmapSource bmp = GetBitmap(file);
                    bmp = ChangeOpacity(bmp, 255);
                }
            }
        }

        private BitmapSource ChangeOpacity(BitmapSource bmp, byte opa)
        {
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int stride = w * 4;
            byte[] pixels = new byte[stride * h];
            bmp.CopyPixels(pixels, stride, 0);
            for (int i = 3; i < pixels.Length; i += 4)
            {
                pixels[i] = opa;
            }
            return BitmapSource.Create(w, h, bmp.DpiX, bmp.DpiY, PixelFormats.Bgra32, null, pixels, stride);
        }

        private BitmapSource GetBitmap(string path)
        {
            PngBitmapDecoder decoder = new(new Uri(path), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            BitmapSource bmp = decoder.Frames[0];
            return ConverterBitmapFromatBgra32(bmp);
        }

        private static BitmapSource ConverterBitmapFromatBgra32(BitmapSource bmp)
        {
            if (bmp.Format != PixelFormats.Bgra32)
            {
                FormatConvertedBitmap fc = new(bmp, PixelFormats.Bgra32, null, 0.0);
                return fc;
            }
            return bmp;
        }
    }
}
