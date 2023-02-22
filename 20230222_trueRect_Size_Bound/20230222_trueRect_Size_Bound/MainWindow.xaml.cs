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

            Rect rect = VisualTreeHelper.GetDrawing(element).Bounds;
            rect.X = 0; rect.Y = 0;
            rect = element.TransformToVisual(parent).TransformBounds(rect);
            rect.X = 0; rect.Y = 0;
            return rect;

        }
        private BitmapSource GetElementBitmap(FrameworkElement element,Panel parent)
        {
            Rect rect=GetDrawingBounds(element, parent);
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
            BitmapSource bitmap1 = ElementToBitmap(MyPolyline1);
            BitmapSource bitmap2 = ElementToBitmap2(MyPolyline1);
            BitmapSource bitmap3 = ElementToBitmap3(MyPolyline1);
            BitmapSource line0 = GetElementBitmap(MyPolyline0, MyCanvas0);
            BitmapSource line1 = GetElementBitmap(MyPolyline1, MyCanvas1);
            BitmapSource line2 = GetElementBitmap(MyPolyline2, MyCanvas1);
        }
    }
}
