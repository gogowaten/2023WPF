using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


//WPF、PolylineをぴったりサイズのBitmapSourceとして取得できた！PolylineよりPath使った方がいい - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2023/03/08/144258



namespace _20230307_PathPolyline
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// PathをBitmapとして取得する
        /// </summary>
        /// <param name="path">Bitmapとして取得したいPath</param>
        /// <returns></returns>
        private BitmapSource MakeBitmapFromPathGeometry(Path path)
        {
            Pen pen = new(path.Stroke, path.StrokeThickness);
            Geometry geo = path.Data;
            //PathGeometry myPGeo = geo.GetOutlinedPathGeometry();//違う
            //PathGeometry myPGeo = geo.GetFlattenedPathGeometry();//違う
            PathGeometry myPGeo = geo.GetWidenedPathGeometry(pen);//これ！
            myPGeo.Transform = path.RenderTransform;

            Rect rect = myPGeo.Bounds;
            DrawingVisual dv = new() { Offset = new Vector(-rect.X, -rect.Y) };
            using (var context = dv.RenderOpen())
            {
                context.DrawGeometry(pen.Brush, null, myPGeo);
            }
            RenderTargetBitmap bitmap = new(
                (int)(rect.Width + 1.0), (int)(rect.Height + 1.0), 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
            return bitmap;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Test();
            BitmapSource bmp2 = Test2(MyPolyline);
            BitmapSource bmp3 = Test3(MyPolyline);
            BitmapSource bmp4 = Test4(MyPolyline);
            BitmapSource bmp = MakeBitmapFromPathGeometry(MyPathPolyline);


        }


        /// <summary>
        /// PolylineをBitmapにする
        /// </summary>
        private BitmapSource Test4(Polyline polyline)
        {
            PathFigure fig = new() { StartPoint = polyline.Points[0] };
            fig.Segments.Add(new PolyLineSegment(polyline.Points.Skip(1), true));
            PathGeometry fillPG = new();
            fillPG.Figures.Add(fig);

            Pen pen = new(polyline.Stroke, polyline.StrokeThickness);
            PathGeometry strokePG = fillPG.GetWidenedPathGeometry(pen);
            strokePG.Transform = polyline.RenderTransform;
            Rect rect = strokePG.Bounds;

            fillPG.Transform = polyline.RenderTransform;

            DrawingVisual dv = new() { Offset = new Vector(-rect.X, -rect.Y) };
            using (var context = dv.RenderOpen())
            {
                context.DrawGeometry(polyline.Fill, null, fillPG);
                context.DrawGeometry(pen.Brush, null, strokePG);
            }
            RenderTargetBitmap bitmap = new((int)(rect.Width + 1),
                                            (int)(rect.Height + 1),
                                            96,
                                            96,
                                            PixelFormats.Pbgra32);
            bitmap.Render(dv);
            return bitmap;
        }

        /// <summary>
        /// PolylineのFillをBitmapにする
        /// </summary>
        private BitmapSource Test3(Polyline polyline)
        {
            PathFigure fig = new()
            {
                IsFilled = true,
                IsClosed = true,
                StartPoint = polyline.Points[0]
            };
            fig.Segments.Add(new PolyLineSegment(polyline.Points.Skip(1), true));

            PathGeometry pg = new();
            pg.Figures.Add(fig);
            pg.Transform = polyline.RenderTransform;
            Rect rect = pg.Bounds;

            DrawingVisual dv = new();
            dv.Offset = new Vector(-rect.X, -rect.Y);
            using (var context = dv.RenderOpen())
            {
                context.DrawGeometry(polyline.Fill, null, pg);
            }
            RenderTargetBitmap bitmap = new((int)(rect.Width + 1), (int)(rect.Height + 1), 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
            return bitmap;
        }

        /// <summary>
        /// PolylineのStrokeをBitmapにする
        /// </summary>
        private BitmapSource Test2(Polyline polyline)
        {
            PathFigure fig = new() { StartPoint = polyline.Points[0] };
            fig.Segments.Add(new PolyLineSegment(polyline.Points.Skip(1), true));
            PathGeometry pg = new();
            pg.Figures.Add(fig);

            Pen pen = new(polyline.Stroke, polyline.StrokeThickness);
            PathGeometry rpg = pg.GetWidenedPathGeometry(pen);
            rpg.Transform = polyline.RenderTransform;
            Rect rect = rpg.Bounds;

            DrawingVisual dv = new();
            dv.Offset = new Vector(-rect.X, -rect.Y);
            using (var context = dv.RenderOpen())
            {
                context.DrawGeometry(pen.Brush, null, rpg);
            }
            RenderTargetBitmap bitmap = new((int)(rect.Width + 1), (int)(rect.Height + 1), 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
            return bitmap;
        }

        private void Test()
        {
            Rect rect = VisualTreeHelper.GetDescendantBounds(MyPolyline);
            rect = MyPolyline.RenderTransform.TransformBounds(rect);
            DrawingVisual dv = new();
            using (var context = dv.RenderOpen())
            {
                context.DrawRectangle(new VisualBrush(MyPolyline) { Stretch = Stretch.None }, null, new Rect(rect.Size));
            }
            RenderTargetBitmap bitmap = new((int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
        }
    }
}
