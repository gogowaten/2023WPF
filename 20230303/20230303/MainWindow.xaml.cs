using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace _20230303
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

           
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var geo = MyPolyBezierCanvas.MyPolyBezier.MyGeometry;
            var widen = geo.GetWidenedPathGeometry(new Pen(Brushes.Red, MyPolyBezierCanvas.MyPolyBezier.StrokeThickness));
            widen.Transform = MyPolyBezierCanvas.RenderTransform;
            Path myPath = new()
            {
                Fill = Brushes.Cyan,
                Data = widen
            };
            MyCanvas.Children.Add(myPath);

            //AdornerLayer.GetAdornerLayer(MyPolyline).Add(new BBAdor(MyPolyline));
            AdornerLayer.GetAdornerLayer(MyPolyline).Add(new CCAdor(MyPolyline));
            //AdornerLayer.GetAdornerLayer(MyPolyBezierCanvas).Add(new CCAdor(MyPolyBezierCanvas));
            //AdornerLayer.GetAdornerLayer(MyBorder).Add(new Ador(MyBorder));
            //AdornerLayer.GetAdornerLayer(MyButton).Add(new Ador(MyButton));

        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            MyPolyline.Points[3] = new Point(200, 200);
            var panelSize = MyCanvas.RenderSize;
            var targetSize = MyPolyline.RenderSize;
            var targetRect = VisualTreeHelper.GetContentBounds(MyPolyline);//empty
            var targetDeRect = VisualTreeHelper.GetDescendantBounds(MyPolyline);
            var targetTransBound = MyPolyline.RenderTransform.TransformBounds(new Rect(MyPolyline.RenderSize));
            VisualBrush vb = new(MyPolyline);
            DrawingVisual dv = new();
            using (var context = dv.RenderOpen())
            {
                context.DrawRectangle(vb, null, new Rect(targetDeRect.Size));
            }
            RenderTargetBitmap bitmap = new((int)targetDeRect.Width, (int)targetDeRect.Height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
        }
    }

    public class MyConverterShapeWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Canvas canvas= (Canvas)parameter;
            var neko = VisualTreeHelper.GetContentBounds(canvas);
            var inu=VisualTreeHelper.GetContentBounds(canvas);
            return VisualTreeHelper.GetDescendantBounds(canvas).Width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConverterShapeHeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Canvas canvas= (Canvas)parameter;
            return VisualTreeHelper.GetDescendantBounds(canvas).Height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
