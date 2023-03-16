using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _2023031213_GeoShapeThumb
{
    public class GeoShapeThumb3 : GeoShapeThumb2
    {
        public GeoShapeThumb3()
        {
            Loaded += GeoShapeThumb3_Loaded;

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            //Test1();
            Test2();
            var rsize = RenderSize;
        }

        /// <summary>
        /// ShapeThumbの座標を0,0する＋Pointsにもとの座標を足すことで
        /// ShapeThumbのRenderSizeを頂点Thumbを含むサイズに広げる
        /// </summary>
        private void Test2()
        {
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);

            var left = Canvas.GetLeft(this);
            var top = Canvas.GetTop(this);
            for (int i = 0; i < MyPoints.Count; i++)
            {
                Point pp = MyPoints[i];
                MyPoints[i] = new Point(pp.X + left, pp.Y + top);
            }
            
            //頂点Thumbの位置ずれを修正
            MyGeometryShape.UpdateAdorner();

        }
        private void Test1()
        {
            //頂点Thumbすべてを含むRect取得はどれ？
            var ccsize = MyGeometryShape.MyAdorner.RenderSize;//違う
            var canw = MyGeometryShape.MyAdorner.MyCanvas.RenderSize;//サイズだけならこれ
            //var thisdraw = VisualTreeHelper.GetDrawing(this)?.Bounds;//null
            var shapedraw = VisualTreeHelper.GetDrawing(MyGeometryShape)?.Bounds;
            //var shapeAdonerdraw = VisualTreeHelper.GetDrawing(MyGeometryShape.MyAdorner)?.Bounds;//null
            //var ahapeAdonerCanvasdraw = VisualTreeHelper.GetDrawing(MyGeometryShape.MyAdorner.MyCanvas)?.Bounds;//null

            var thisbound = VisualTreeHelper.GetDescendantBounds(this);
            var shapebound = VisualTreeHelper.GetDescendantBounds(MyGeometryShape);
            var shapeAdobound = VisualTreeHelper.GetDescendantBounds(MyGeometryShape.MyAdorner);//Rectはこれ
            var ahapeAdoCanbound = VisualTreeHelper.GetDescendantBounds(MyGeometryShape.MyAdorner.MyCanvas);//Rectはこれ

            var left = Canvas.GetLeft(this);
            var top = Canvas.GetTop(this);
            var lefts = Canvas.GetLeft(MyGeometryShape);
            var tops = Canvas.GetTop(MyGeometryShape);
            var ssize = MyGeometryShape.RenderSize;
            //MyGeometryShape.Arrange(shapeAdobound);
            MyGeometryShape.Arrange(new Rect(shapeAdobound.Size));

        }
        private void GeoShapeThumb3_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(this).Add(new SizeAdorner(this));
        }
    }
}
