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
//WPF、Canvasの中に画像として保存したい要素が回転や拡大など変形されていてもOKな方法 - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/14977600
//WPF、Canvas全体やCanvasに配置した要素を画像(png)ファイルにする。回転や拡大変換要素にも対応版 - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2023/01/02/140519

//Polylineを画像として取得するテスト、
//今までのActualWidthやRenderSizeを使った方法では正しく取得できなかったので、
//VisualTreeHelper.GetDrawingを使ってみたら、だいぶまともになった。
//それでも少し大きめのサイズになってしまう
namespace _20230222_trueRect_Size_Bound
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

        private BitmapSource ElementToBitmap(FrameworkElement element)
        {
            Rect myRect = VisualTreeHelper.GetDrawing(element).Bounds;
            myRect.Offset(-myRect.X, -myRect.Y);
            VisualBrush brush = new(element);
            brush.Stretch = Stretch.None;
            DrawingVisual visual = new();
            using (var context = visual.RenderOpen())
            {
                context.DrawRectangle(brush, null, myRect);
            }
            RenderTargetBitmap render = new((int)myRect.Width, (int)myRect.Height, 96, 96, PixelFormats.Pbgra32);
            render.Render(visual);
            return render;
        }
        //private BitmapSource ElementToBitmap2(FrameworkElement element)
        //{
        //    Rect myRect = element.TransformToVisual(MyCanvas1).TransformBounds(new Rect(element.RenderSize));
        //    //Rect rect = element.TransformToVisual(element).TransformBounds(new Rect(element.RenderSize));
        //    //rect.Offset(-rect.X, -rect.Y);
        //    VisualBrush brush = new(element);
        //    brush.Stretch = Stretch.None;
        //    DrawingVisual visual = new();
        //    using (var context = visual.RenderOpen())
        //    {
        //        context.DrawRectangle(brush, null, myRect);
        //    }
        //    RenderTargetBitmap render = new((int)myRect.Width, (int)myRect.Height, 96, 96, PixelFormats.Pbgra32);
        //    render.Render(visual);
        //    return render;
        //}
        //private BitmapSource ElementToBitmap3(FrameworkElement element)
        //{
        //    Rect myR = VisualTreeHelper.GetDrawing(element).Bounds;
        //    //myR.Offset(-myR.X, -myR.Y);
        //    myR.X = 0; myR.Y = 0;
        //    Rect myRect = element.TransformToVisual(MyCanvas1).TransformBounds(myR);
        //    myRect.X = 0;
        //    myRect.Y = 0;
        //    VisualBrush brush = new(element);
        //    brush.Stretch = Stretch.None;
        //    DrawingVisual visual = new();
        //    using (var context = visual.RenderOpen())
        //    {
        //        context.DrawRectangle(brush, null, myRect);
        //    }
        //    RenderTargetBitmap render = new((int)myRect.Width, (int)myRect.Height, 96, 96, PixelFormats.Pbgra32);
        //    render.Render(visual);
        //    return render;
        //}
        /// <summary>
        /// 要素の描画サイズ取得。PolylineやPath系図形のサイズも概ね正しく取得できる。
        /// 座標が必要なければこれ、必要なら2
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private Rect GetDrawingBounds1(FrameworkElement element)
        {
            Rect rect0 = element.RenderTransform.
                TransformBounds(VisualTreeHelper.GetDrawing(element).Bounds);
            rect0.X = 0; rect0.Y = 0;
            return rect0;
            //以下でも同じ
            //Rect rect1 = element.TransformToVisual(parent).TransformBounds(VisualTreeHelper.GetDrawing(element).Bounds);
            //rect1.X = 0; rect1.Y = 0;
            //return rect1;
        }
        /// <summary>
        /// 要素の描画サイズと座標取得。PolylineやPath系図形のサイズも概ね正しく取得できる。
        /// 座標は違っているかも
        /// </summary>
        /// <param name="element"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private Rect GetDrawingBounds2(FrameworkElement element, Panel parent)
        {
            Rect rect1 = element.TransformToVisual(parent).
                TransformBounds(VisualTreeHelper.GetDrawing(element).Bounds);
            rect1.X = 0; rect1.Y = 0;
            return rect1;
        }

        private BitmapSource GetElementBitmap(FrameworkElement element)
        {
            Rect rect = GetDrawingBounds1(element);
            VisualBrush brush = new(element);
            brush.Stretch = Stretch.None;
            DrawingVisual visual = new();
            using (var context = visual.RenderOpen())
            {
                context.DrawRectangle(brush, null, rect);
            }
            RenderTargetBitmap render = new((int)(rect.Width + 0.5), (int)(rect.Height + 0.5), 96, 96, PixelFormats.Pbgra32);
            render.Render(visual);
            return render;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //BitmapSource line0 = GetElementBitmap(MyPolyline0, MyCanvas0);
            BitmapSource line2 = GetElementBitmap(MyPolyline00);
        }
    }



}
