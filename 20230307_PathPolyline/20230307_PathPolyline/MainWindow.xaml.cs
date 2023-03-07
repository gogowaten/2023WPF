using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            //Test2();
            //Test3(MyPolyline);
            Test4(MyPolyline);
            BitmapSource bmp = MakeBitmapFromPathGeometry(MyPathPolyline);


        }


        /// <summary>
        /// PolylineのStrokeをBitmapにする
        /// </summary>
        private BitmapSource Test4(Polyline polyline)
        {
            PathFigure fig = new() { StartPoint = polyline.Points[0] };
            fig.Segments.Add(new PolyLineSegment(polyline.Points.Skip(1), true));
            PathGeometry pg = new();
            pg.Figures.Add(fig);

            Pen pen = new(polyline.Stroke, polyline.StrokeThickness);
            PathGeometry rpg = pg.GetWidenedPathGeometry(pen);
            rpg.Transform = polyline.RenderTransform;
            Rect strokeRect = rpg.Bounds;

            pg.Transform = polyline.RenderTransform;

            DrawingVisual dv = new();
            dv.Offset = new Vector(-strokeRect.X, -strokeRect.Y);
            using (var context = dv.RenderOpen())
            {
                context.DrawGeometry(polyline.Fill, null, pg);
                context.DrawGeometry(pen.Brush, null, rpg);
            }
            RenderTargetBitmap bitmap = new((int)(strokeRect.Width + 1), (int)(strokeRect.Height + 1), 96, 96, PixelFormats.Pbgra32);
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
