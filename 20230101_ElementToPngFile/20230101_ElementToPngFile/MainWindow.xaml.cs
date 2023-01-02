using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

//WPF、Canvas全体やCanvasに配置した要素を画像(png)ファイルにする。回転や拡大変換要素にも対応版 - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2023/01/02/140519

namespace _20230101_ElementToPngFile
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 要素をBitmap画像に変換する。だたし、
        /// 要素の座標が0,0以外だった場合はできない。
        /// 回転などの変換がされていた場合もできない。
        /// ActualWidthとActualHeightに数値が入っていない場合もできない。
        /// スクロールバーなどで全体が表示されていないときもでいない。
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private BitmapSource MakeBitmap(FrameworkElement element)
        {
            RenderTargetBitmap bitmap = new((int)element.ActualWidth,
                                            (int)element.ActualHeight,
                                            96,
                                            96,
                                            PixelFormats.Pbgra32);
            bitmap.Render(element);
            return bitmap;
        }

        /// <summary>
        /// Canvas上に配置された要素をBitmap画像に変換する。
        /// 要素の座標が0,0以外、回転などの変換がされていても正しく変換する。
        /// ただし、ActualWidthとActualHeightに数値が入っていない要素の場合はできない。
        /// </summary>
        /// <param name="element">対象要素</param>
        /// <param name="parentCanvas">対象要素の親Canvas</param>
        /// <returns></returns>
        private BitmapSource MakeBitmap(FrameworkElement element, Canvas parentCanvas)
        {
            //対象要素がピッタリ収まるRect取得
            Rect bounds = element.TransformToVisual(parentCanvas)
                                 .TransformBounds(
                new Rect(0, 0, element.ActualWidth, element.ActualHeight));
            VisualBrush brush = new(element) { Stretch = Stretch.None };
            DrawingVisual dv = new();
            using (var context = dv.RenderOpen())
            {
                context.DrawRectangle(brush, null, new Rect(bounds.Size));
            }
            RenderTargetBitmap bitmap
                = new((int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
            return bitmap;
        }

        /// <summary>
        /// BitmapSourceをpngファイルにする
        /// </summary>
        /// <param name="bitmap">保存する画像</param>
        /// <param name="filePath">保存先パス(拡張子も付けたフルパス)</param>
        private void SaveBitmapToPng(BitmapSource bitmap, string filePath)
        {
            PngBitmapEncoder encoder = new();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using (var stream = File.Open(filePath, FileMode.Create))
            {
                encoder.Save(stream);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveBitmapToPng(MakeBitmap(MyRectangle2, MyCanvas), "E:element.png");
            //SaveBitmapToPng(MakeBitmap(MyRectangle1, MyCanvas), "E:elementImage.png");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveBitmapToPng(MakeBitmap(MyCanvas, MyCanvas), "E:Canvas.png");
            //SaveBitmapToPng(MakeBitmap(MyCanvas), "E:Canvas.png");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SaveBitmapToPng(MakeBitmap(MyCanvas2, MyCanvas), "E:Canvas2.png");
        }
    }
}
