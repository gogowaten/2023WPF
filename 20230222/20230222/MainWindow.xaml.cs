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

namespace _20230222
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var resize = MyPolyline.RenderSize;
            GeneralTransform tv = TransformToVisual(MyPolyline);
            var neko = tv.TransformBounds(new Rect());
            var rgeo = MyPolyline.RenderedGeometry;
            var tfv = MyPolyline.TransformToVisual(MyCanvas);
            var vsize = tfv.TransformBounds(new(MyPolyline.RenderSize));

            var help1 = VisualTreeHelper.GetDrawing(MyPolyline).Bounds;
            var help2 = VisualTreeHelper.GetDrawing(MyPolyline2).Bounds;
            var help3 = VisualTreeHelper.GetDrawing(MyPolyline3).Bounds;

            Rect rect = MyPolyline2.TransformToVisual(MyCanvas).TransformBounds(new Rect(MyPolyline2.RenderSize));
            VisualBrush brush = new(MyPolyline2);
            DrawingVisual drawing = new();
            using (var context = drawing.RenderOpen())
            {
                context.DrawRectangle(brush, null, rect);
            }
            RenderTargetBitmap render = new((int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Pbgra32);
            render.Render(drawing);

            rect = VisualTreeHelper.GetDrawing(MyPolyline2).Bounds;
            rect.Offset(-rect.X, -rect.Y);
            using (var context = drawing.RenderOpen())
            {
                context.DrawRectangle(brush, null, rect);
            }
            RenderTargetBitmap trueRender = new((int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Pbgra32);
            trueRender.Render(drawing);

        }
    }
}
