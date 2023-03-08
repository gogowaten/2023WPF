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

namespace _20230308_PathToBitmap
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
        /// Pathの見た目通りのBitmapSource取得
        /// </summary>
        private BitmapSource Test(Path path)
        {
            Pen pen = new(path.Stroke, path.StrokeThickness);
            //Fill用のGeometryはCloneで取得、こうしないともとのPathが変形してしまう
            Geometry fillGeo = path.Data.Clone();
            fillGeo.Transform = path.RenderTransform;//変形
            PathGeometry strokeGeo = new();
            //見た目通りのRect取得
            //PathにStrokeが設定されているときはStrokeから取得
            //StrokeがないときはもとのGeometry(fill)から取得
            Rect rect;
            if (path.StrokeThickness > 0
                && path.Stroke != null
                && path.Stroke != Brushes.Transparent)
            {
                strokeGeo = fillGeo.GetWidenedPathGeometry(pen);//太さの分広げる
                strokeGeo.Transform = path.RenderTransform;//変形
                rect = strokeGeo.Bounds;
            }
            else
            {
                rect = fillGeo.Bounds;
            }

            DrawingVisual dv = new() { Offset = new Vector(-rect.X, -rect.Y) };
            using (var context = dv.RenderOpen())
            {
                context.DrawGeometry(path.Fill, null, fillGeo);
                context.DrawGeometry(path.Stroke, null, strokeGeo);
            }

            RenderTargetBitmap bitmap = new(
                (int)(rect.Width + 1),
                (int)(rect.Height + 1),
                96,
                96,
                PixelFormats.Pbgra32);
            bitmap.Render(dv);
            return bitmap;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource bmp = Test(MyBezier);
        }
    }
}
