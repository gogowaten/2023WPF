using System;
using System.Collections.Generic;
using System.IO;
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
using ControlLibraryCore20200620;

namespace _20230713_hue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AppData MyAppData { get; set; } = new();
        byte[]? MyPixels { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = MyAppData;
            MyTest();
        }

        private void MyTest()
        {
            (MyImage.Source, MyPixels) = GetBitmapFromFilePath("D:\\ブログ用\\テスト用画像\\NEC_8041_2017_05_09_午後わてん_96dpi.jpg");
        }

        #region 画像を開く

        /// <summary>
        /// 画像ファイルとして開いて返す、エラーの場合はnullを返す
        /// dpiは96に変換する、このときのピクセルフォーマットはbgra32
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private (BitmapSource?, byte[]?) GetBitmapFromFilePath(string path)
        {
            using FileStream stream = File.OpenRead(path);
            BitmapSource bmp;
            try
            {
                bmp = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                //BitmapSource bbb = ConverterBitmapFromatBgra32(bmp);
                return ConverterBitmapDipWithPixels(ConverterBitmapFromatBgra32(bmp));
                //return ConverterBitmapDpi96AndPixFormatBgra32(bmp);
            }
            catch (Exception)
            {
                return (null, null);
            }
        }

        ///// <summary>
        ///// BitmapSourceのdpiを96に変換する、ピクセルフォーマットもBgra32に変換する
        ///// </summary>
        ///// <param name="bmp"></param>
        ///// <returns></returns>
        //private (BitmapSource, byte[]?) ConverterBitmapDpi96AndPixFormatBgra32(BitmapSource bmp)
        //{
        //    //png画像はdpi95.98とかの場合もあるけど、
        //    //これは問題ないので変換しない
        //    byte[]? pixels = null;
        //    if (bmp.DpiX < 95.0 || 96.0 < bmp.DpiX)
        //    {
        //        FormatConvertedBitmap fc = new(bmp, PixelFormats.Bgra32, null, 0.0);
        //        int w = fc.PixelWidth;
        //        int h = fc.PixelHeight;
        //        int stride = w * 4;
        //        pixels = new byte[stride * h];
        //        fc.CopyPixels(pixels, stride, 0);
        //        bmp = BitmapSource.Create(w, h, 96.0, 96.0, fc.Format, null, pixels, stride);
        //    }

        //    return (bmp, pixels);
        //}

        /// <summary>
        /// BitmapSourceのピクセルフォーマットをBgra32に変換するだけ
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        private static BitmapSource ConverterBitmapFromatBgra32(BitmapSource bmp)
        {
            if (bmp.Format != PixelFormats.Bgra32)
            {
                FormatConvertedBitmap fc = new(bmp, PixelFormats.Bgra32, null, 0.0);
                return fc;
            }
            return bmp;
        }

        /// <summary>
        /// BitmapSourceのdpiを96に変換＋pixelsも返す、PixelFormatsBgra32専用
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        private (BitmapSource, byte[]) ConverterBitmapDipWithPixels(BitmapSource bmp)
        {
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int stride = w * 4;
            byte[] pixels = new byte[stride * h];
            bmp.CopyPixels(pixels, stride, 0);
            //png画像はdpi95.98とかの場合もあるけど、
            //これは問題ないので変換しない
            if (bmp.DpiX < 95.0 || 96.0 < bmp.DpiX)
            {
                bmp = BitmapSource.Create(w, h, 96.0, 96.0, bmp.Format, null, pixels, stride);
            }
            return (bmp, pixels);
        }

        #endregion 画像を開く


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var neko = MyPixels;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
