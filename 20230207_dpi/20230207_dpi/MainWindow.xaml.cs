using System;
using System.Collections.Generic;
using System.Diagnostics;
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

//dpi96のBitmapSourceをPng形式で保存した画像のdpiが95.98...になってしまうのはpngの仕様みたい
//WPF Manipulating DPI of Images
//https://social.msdn.microsoft.com/Forums/vstudio/en-US/40431c54-01c7-4e74-9a02-7eaba5c40373/wpf-manipulating-dpi-of-images?forum=wpf

//保存後に画像の解像度が下がり、サイズが大きくなる - Paint.NET に関するディスカッションと質問 - paint.net Forum
//https://forums.getpaint.net/topic/113826-images-drop-resolution-and-increase-size-after-saving/

namespace _20230207_dpi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string MyPath1 = "D:\\ブログ用\\テスト用画像\\collection1.png";
        private string MyPath2 = "E:\\0204.jpg";
        private string MyPath3 = "C:\\Users\\waten\\Documents\\20230125_検索パフォーマンス.png";
        private string MyPath4 = "D:\\ブログ用\\テスト用画像\\NEC_9011_2015_07_23_スイートバジル収穫.jpg";
        private string SaveBitmapName = "E://dpiTest.png";
        public MainWindow()
        {
            InitializeComponent();
            //Test();
            SaveTest();
        }
        private void SaveTest()
        {
            BitmapSource source = GetBitmap0(MyPath4);
            SaveBitmap(source, SaveBitmapName);
        }
        private void SaveBitmap(BitmapSource source, string fileName)
        {
            PngBitmapEncoder encoder = new();
            using System.IO.FileStream stream = new(fileName, System.IO.FileMode.Create);
            encoder.Frames.Add(BitmapFrame.Create(source));
            var info = encoder.CodecInfo;
            var conte = encoder.ColorContexts;
            encoder.Save(stream);
        }
        private void Test()
        {
            string MyPath = MyPath1;
            //MyPanel.Children.Add(new Image() { Source = GetBitmap1(MyPath), Stretch = Stretch.None });
            //Debug.WriteLine(nameof(GetBitmap0) + "_" + GetBitmap0(MyPath).DpiX.ToString());
            Debug.WriteLine(nameof(GetBitmap1) + "_" + GetBitmap1(MyPath).DpiX.ToString());
            //Debug.WriteLine(nameof(GetBitmap2) + "_" + GetBitmap2(MyPath).DpiX.ToString());
            Debug.WriteLine(nameof(GetBitmap3) + "_" + GetBitmap3(MyPath).DpiX.ToString());
            Debug.WriteLine(nameof(GetBitmap4) + "_" + GetBitmap4(MyPath).DpiX.ToString());
            Debug.WriteLine(nameof(GetBitmap5) + "_" + GetBitmap5(MyPath).DpiX.ToString());

        }
        private BitmapSource GetBitmap0(string fileName)
        {
            //ファイルロックされる
            System.IO.FileStream stream = new(fileName, System.IO.FileMode.Open);
            BitmapImage img = new();
            img.BeginInit();
            img.StreamSource = stream;
            img.EndInit();
            return img;
        }
        private BitmapSource GetBitmap1(string fileName)
        {
            using System.IO.FileStream stream = new(fileName, System.IO.FileMode.Open);
            BitmapImage img = new();
            img.BeginInit();
            img.StreamSource = stream;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            return img;
        }

        private BitmapSource GetBitmap2(string fileName)
        {
            //ファイルロックされる
            BitmapImage img = new(new Uri(fileName));
            //img.CacheOption = BitmapCacheOption.OnLoad;//無意味？
            return img;
        }
        private BitmapSource GetBitmap3(string fileName)
        {
            using System.IO.FileStream stream = new(fileName, System.IO.FileMode.Open);
            BitmapImage img = new();
            img.BeginInit();
            img.StreamSource = stream;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            img.SourceRect = new Int32Rect(0, 0, img.PixelWidth, img.PixelHeight);
            return img;
        }
        private BitmapSource GetBitmap4(string fileName)
        {
            using System.IO.FileStream stream = System.IO.File.OpenRead(fileName);
            BitmapImage img = new();
            img.BeginInit();
            img.StreamSource = stream;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            return img;
        }
        private BitmapSource GetBitmap5(string fileName)
        {
            using System.IO.FileStream stream = System.IO.File.OpenRead(fileName);
            BitmapFrame bmp = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            return bmp;
        }

    }
}
