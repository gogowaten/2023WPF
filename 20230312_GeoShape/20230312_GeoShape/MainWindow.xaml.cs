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

namespace _20230312_GeoShape
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GeometryLine MyLine;
        public MainWindow()
        {
            InitializeComponent();

            MyLine = new() { MyPoints = new PointCollection() { new(40, 0), new(0, 100), new(100, 0) } };
            MyCanvas.Children.Add(MyLine);
            Canvas.SetLeft(MyLine, 200);
            Canvas.SetTop(MyLine, 100);

            RotateTransform rotate = new(60);
            TransformGroup group = new();
            group.Children.Add(rotate);

            MyLine.RenderTransform = rotate;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Test1();
        }
        private void Test1()
        {
            BitmapSource bmp = MyGeo.GetBitmap();
        }
        private void Text0()
        {
            GeneralTransform tf = MyCanvas.TransformToVisual(MyLine);
            Rect rect = tf.TransformBounds(new Rect(MyLine.RenderSize));
            //Rect rect = MyLine.MyExternalBounds;
            VisualBrush vb = new(MyCanvas) { Stretch = Stretch.None };
            DrawingVisual dv = new();// { Offset = new Vector(rect.X,rect.Y)};
            using (var context = dv.RenderOpen())
            {
                context.DrawRectangle(vb, null, new Rect(0, 0, rect.Width, rect.Height));
            }
            RenderTargetBitmap bitmap = new((int)(rect.Width + 1), (int)(rect.Height + 1), 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
        }
    }
}
