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
        private BitmapSource ElementToBitmap2(FrameworkElement element)
        {
            Rect myRect = element.TransformToVisual(MyCanvas1).TransformBounds(new Rect(element.RenderSize));
            //Rect rect = element.TransformToVisual(element).TransformBounds(new Rect(element.RenderSize));
            //rect.Offset(-rect.X, -rect.Y);
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
        private BitmapSource ElementToBitmap3(FrameworkElement element)
        {
            Rect myR = VisualTreeHelper.GetDrawing(element).Bounds;
            //myR.Offset(-myR.X, -myR.Y);
            myR.X = 0; myR.Y = 0;
            Rect myRect = element.TransformToVisual(MyCanvas1).TransformBounds(myR);
            myRect.X = 0;
            myRect.Y = 0;
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
        private Rect GetDrawingBounds(FrameworkElement element, Panel parent)
        {
            Rect rect0 = VisualTreeHelper.GetDrawing(element).Bounds;
           var neko = element.RenderTransform.TransformBounds(rect0);
            Rect ep = element.TransformToVisual(parent).TransformBounds(rect0);
            Rect ee = element.TransformToVisual(element).TransformBounds(rect0);
            Rect pe = parent.TransformToVisual(element).TransformBounds(rect0);
            Rect pp = parent.TransformToVisual(parent).TransformBounds(rect0);
            Rect rect1 = new(rect0.Size);
            Rect ep0 = element.TransformToVisual(parent).TransformBounds(rect1);//
            Rect ee0 = element.TransformToVisual(element).TransformBounds(rect1);
            Rect pe0 = parent.TransformToVisual(element).TransformBounds(rect1);//kore-
            Rect pp0 = parent.TransformToVisual(parent).TransformBounds(rect1);

            Rect rect2 = element.TransformToVisual(parent).TransformBounds(rect1);
            //rect2.Width += rect2.X;rect2.Height += rect2.Y;
            rect2.X = 0; rect2.Y = 0;
            //Rect rectpe0 = new(new Size(pe0.Width + pe0.X, pe0.Height + pe0.Y));
            //Rect rectpe0 = new(pe0.X, pe0.Y, pe0.Width + pe0.X, pe0.Height + pe0.Y);
            //Rect rectpe0 = new(-pe0.X, -pe0.Y, pe0.Width + pe0.X, pe0.Height + pe0.Y);
            //Rect rectpe0 = new(-pe0.X/2, -pe0.Y/2, pe0.Width + pe0.X, pe0.Height + pe0.Y);
            Rect rectpe0 = new(pe0.X/2, pe0.Y/2, pe0.Width + pe0.X, pe0.Height + pe0.Y);
            Rect nekorect=new(0,0,neko.Width+neko.X,neko.Height+neko.Y);
            return nekorect;
            return rectpe0;
            return rect2;
        }
        private Rect GetDrawingBounds2(FrameworkElement element, Panel parent)
        {
            Rect rect = element.TransformToVisual(parent).TransformBounds(new(element.RenderSize));
            rect.X = 0; rect.Y = 0;
            return rect;
        }

        private BitmapSource GetElementBitmap(FrameworkElement element, Panel parent)
        {
            Rect rect = GetDrawingBounds(element, parent);
            rect = new(0, 0, 100, 400);
            VisualBrush brush = new(element);
            brush.Stretch = Stretch.None;
            DrawingVisual visual = new();
            using (var context = visual.RenderOpen())
            {
                context.DrawRectangle(brush, null, rect);
            }
            RenderTargetBitmap render = new((int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Pbgra32);
            render.Render(visual);
            return render;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //BitmapSource bitmap0 = ElementToBitmap(MyPolyline0);
            BitmapSource bitmap1 = ElementToBitmap(MyPolyline2);
            BitmapSource bitmap2 = ElementToBitmap2(MyPolyline2);
            BitmapSource bitmap3 = ElementToBitmap3(MyPolyline2);
            BitmapSource line0 = GetElementBitmap(MyPolyline0, MyCanvas0);
            BitmapSource line1 = GetElementBitmap(MyPolyline1, MyCanvas1);
            BitmapSource line2 = GetElementBitmap(MyPolyline2, MyCanvas1);
        }
    }



}
