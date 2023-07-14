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
            (MyAppData.MyBitmap, MyPixels) = GetBitmapFromFilePath("D:\\ブログ用\\テスト用画像\\NEC_8041_2017_05_09_午後わてん_96dpi.jpg");

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


        //private static byte[] GetPixels(BitmapSource bmp)
        //{
        //    int w = bmp.PixelWidth;
        //    int h = bmp.PixelHeight;
        //    int stride = w * 4;
        //    byte[] pixels = new byte[h * stride];
        //    bmp.CopyPixels(pixels, stride, 0);
        //    return pixels;
        //}

        #endregion 画像を開く


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var neko = MyPixels;
            var appdata = MyAppData;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //MyAppData.MyBitmap = ChangeColor(MyAppData.MyBitmap);
            MyImage2.Source = ChangeColor(MyAppData.MyBitmap);
        }

        private BitmapSource? ChangeColor(BitmapSource? bmp)
        {
            if (bmp == null) return null;
            int wi = bmp.PixelWidth;
            int hi = bmp.PixelHeight;
            int stride = wi * 4;
            byte[] pixels = new byte[hi * stride];
            bmp.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                byte b = pixels[i];
                byte g = pixels[i + 1];
                byte r = pixels[i + 2];
                byte a = pixels[i + 3];
                if (a != 0)
                {
                    var (h, s, v) = MathHSV.RGB2hsv(r, g, b);
                    if (CheckIsArea(h, s, v))
                    {
                        //HSV hsv = new(GetNewHue(h), GetNewSat(s), GetNewLum(v));
                        //var rgb = MathHSV.HSV2rgb(hsv);                        
                        (byte rr, byte gg, byte bb) = MathHSV.Hsv2rgb(GetNewHue(h), GetNewSat(s), GetNewLum(v));
                        pixels[i] = bb;
                        pixels[i + 1] = gg;
                        pixels[i + 2] = rr;
                    }
                }
            }
            var bbb = BitmapSource.Create(wi, hi, 96.0, 96.0, bmp.Format, null, pixels, stride);
            return bbb;
        }

        private double GetNewHue(double hue)
        {
            if (MyAppData.IsHueAdd == true)
            {
                return FixHue(hue + MyAppData.HueChange);
            }
            else
            {
                return MyAppData.HueChange;
            }
        }
        private double GetNewSat(double sat)
        {
            if (MyAppData.IsSatAdd == true)
            {
                return FixSatOrLum(sat + MyAppData.SatChange);
            }
            else
            {
                return MyAppData.SatChange;
            }
        }
        private double GetNewLum(double lum)
        {
            if (MyAppData.IsLumAdd == true)
            {
                return FixSatOrLum(lum + MyAppData.LumChange);
            }
            else
            {
                return MyAppData.LumChange;
            }
        }



        public static double FixHue(double hue)
        {
            return hue < 0.0 ? 0.0 : hue > 360.0 ? 360.0 : hue;
        }
        public static double FixSatOrLum(double value)
        {
            return value < 0.0 ? 0.0 : value > 1.0 ? 1.0 : value;
        }

        /// <summary>
        /// 対象範囲判定
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool CheckIsArea(double h, double s, double v)
        {
            if (!CheckAreaHSV(MyAppData.HueMin, MyAppData.HueMax, h))
            {
                return false;
            }
            else if (!CheckAreaHSV(MyAppData.SatMin, MyAppData.SatMax, s))
            {
                return false;
            }
            else if (!CheckAreaHSV(MyAppData.LumMin, MyAppData.LumMax, v))
            {
                return false;
            }
            return true;

            bool CheckAreaHSV(double min, double max, double value)
            {
                if (min <= max)
                {
                    return min <= value && value <= max;
                }
                else
                {
                    return min <= value || value <= max;
                }
            }
        }

    }
}


