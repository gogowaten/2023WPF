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

namespace _20230102_TextBlockToPng
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            var pchild = VisualTreeHelper.GetChild(parentCanvas, 0);
            var pcount = VisualTreeHelper.GetChildrenCount(parentCanvas);
            var pcbounds = VisualTreeHelper.GetContentBounds(parentCanvas);
            var pdebounds = VisualTreeHelper.GetDescendantBounds(parentCanvas);
            var edraw = VisualTreeHelper.GetDrawing(element);
            //RenderOptions.SetEdgeMode(element, EdgeMode.Aliased);
            var ebounds = VisualTreeHelper.GetDescendantBounds(element);
            var eedge = VisualTreeHelper.GetEdgeMode(element);
            var neko = element.ActualWidth;
            var wid = ebounds.X + ebounds.Width;
            var epare = VisualTreeHelper.GetParent(element);

            //対象要素がピッタリ収まるRect取得
            Rect bounds = element.TransformToVisual(parentCanvas)
                                 .TransformBounds(
                new Rect(0, 0, element.ActualWidth, element.ActualHeight));
            bounds = VisualTreeHelper.GetDescendantBounds(element);//余白なしのRectになるのでText表示系要素だとそのままでは使えない
            //bounds.X = -ebounds.X; bounds.Y = -ebounds.Y;
            VisualBrush brush = new(element) { Stretch = Stretch.None };
            DrawingVisual dv = new();
            using (var context = dv.RenderOpen())
            {
                context.DrawRectangle(brush, null, new Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height));
                //context.DrawRectangle(brush, null, new Rect(bounds.Size));
            }
            RenderTargetBitmap bitmap
                = new((int)bounds.Width + 1, (int)bounds.Height + 1, 96, 96, PixelFormats.Pbgra32);
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
            //SaveBitmapToPng(MakeBitmap(MyCanvas), "E:textresult.png");
            SaveBitmapToPng(MakeBitmap(MyCanvas, MyCanvas), "E:textresult.png");
            //SaveBitmapToPng(MakeBitmap(MyCanvas, ParentCanvas), "E:textresult.png");
            //SaveBitmapToPng(MakeBitmap(MyInternalGrid, MyCanvas), "E:textresult.png");
            //var can = MyCanvas;
            //var gri = MyInternalGrid;
            //var tex = MyText;
        }
    }
}
