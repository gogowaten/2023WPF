using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
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
        private const string MY_APP_NAME = "OpaOpaOpacity";
        public string MyDirectory { get; private set; } = "";
        public MainWindow()
        {
            InitializeComponent();
            MyButtonConvertAll.FontSize = FontSize * 1.5;
            MyButtonConvertSelected.FontSize = FontSize * 1.5;

            DataContext = this;
            Drop += MainWindow_Drop;
            MyListBox.SelectionChanged += MyListBox_SelectionChanged;
            ContextMenu MyListBoxContext = new();
            MenuItem item = new() { Header = "選択画像を変換" };
            item.Click += Item_Click;
            MyListBoxContext.Items.Add(item);
            MyListBox.ContextMenu = MyListBoxContext;

        }


        private void Item_Click(object sender, RoutedEventArgs e)
        {
            
            List<string> files = new();
            foreach (var item in MyListBox.SelectedItems)
            {
                string str = (string)item;
                files.Add(System.IO.Path.Combine(MyDirectory, str));
            }
            MyExe(files);
        }

        private void MyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyListBox.SelectedItem is string fileName)
            {
                string path = System.IO.Path.Combine(MyDirectory, fileName);
                var bmp = GetBitmap(path);
                MyImage.Source = bmp;
            }
        }

        private void MainWindow_Drop(object sender, DragEventArgs e)
        {
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
                    var dirr = System.IO.Path.GetDirectoryName(ff[0]);
                    if (Directory.Exists(dirr)) { dir = dirr; }
                }
                MyDirectory = dir;
                MyTextBlockDir.Text = dir;
                MyListBox.ItemsSource = GetFileNames(dir);
                //LINQは処理が遅い、体感できるくらい遅い
                //MyListBox.ItemsSource = Directory.GetFiles(dir, "*.png").Select(x => System.IO.Path.GetFileName(x));
            }
        }

        /// <summary>
        /// 指定フォルダの中のpngファイルだけのリストを返す
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>

        private List<string> GetFileNames(string dir)
        {
            List<string> names = new();
            foreach (var item in Directory.GetFiles(dir, "*.png"))
            {
                names.Add(System.IO.Path.GetFileName(item));
            }
            return names;
        }



        private void ButtonExe_Click(object sender, RoutedEventArgs e)
        {
            MyExe(Directory.GetFiles(MyDirectory, "*.png"));
        }

        /// <summary>
        /// 実行
        /// 対象フォルダにopaフォルダ作成、そこにAlpha値を変換した画像を
        /// png形式で保存
        /// </summary>
        private void MyExe(IEnumerable<string> files)
        {
            try
            {
                if (!string.IsNullOrEmpty(MyDirectory))
                {
                    string dir = MyDirectory + "\\opa";
                    _ = Directory.CreateDirectory(dir);
                    foreach (var file in files)
                    {
                        SaveExe(file,
                            dir + "\\" + System.IO.Path.GetFileName(file),
                            (byte)MySlider.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 画像ファイルを指定Alphaに変換して保存
        /// </summary>
        /// <param name="bmpPath"></param>
        /// <param name="savePath"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        private bool SaveExe(string bmpPath, string savePath, byte alpha)
        {
            if (GetBitmap(bmpPath) is BitmapSource bmp)
            {
                bmp = ChangeOpacity(bmp, alpha);
                SaveBitmapToPng(savePath, bmp);
                return true;
            }
            return false;
        }

        private void SaveBitmapToPng(string filename, BitmapSource bmp)
        {
            BitmapMetadata metadata = new("png");
            metadata.SetQuery("/tEXt/Software", MY_APP_NAME);
            PngBitmapEncoder encoder = new();
            encoder.Frames.Add(BitmapFrame.Create(bmp, null, metadata, null));
            using FileStream stream = new(filename, FileMode.Create, FileAccess.Write);
            encoder.Save(stream);
        }

        /// <summary>
        /// すべてのピクセルを指定Alphaに変換。元のAlpha値が0だった場合は変換しない
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        private BitmapSource ChangeOpacity(BitmapSource bmp, byte alpha)
        {
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int stride = w * 4;
            byte[] pixels = new byte[stride * h];
            bmp.CopyPixels(pixels, stride, 0);
            for (int i = 3; i < pixels.Length; i += 4)
            {
                if (pixels[i] != 0)
                {
                    pixels[i] = alpha;
                }
            }
            return BitmapSource.Create(w, h, bmp.DpiX, bmp.DpiY, PixelFormats.Bgra32, null, pixels, stride);
        }

        private BitmapSource? GetBitmap(string path)
        {
            if (!File.Exists(path)) return null;
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MySlider.Value = 0.0;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MySlider.Value = 128;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MySlider.Value = 255;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class MyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dd = (double)value;
            return dd / 255;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
