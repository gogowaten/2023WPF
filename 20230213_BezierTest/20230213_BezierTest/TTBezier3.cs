using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace _20230213_BezierTest
{
    class TTBezier3 : Shape
    {
        public ObservableCollection<Point> Points { get; set; } = new();
        public ObservableCollection<Point> BezierPoints { get; set; } = new();

        public TTBezier3()
        {
            Points.Add(new Point(0, 0));
            Points.Add(new Point(20, 50));
            Points.Add(new Point(100, 20));
            //MyPoints.Add(new Point(100, 00));
            MakePoints();
            Points.CollectionChanged += Points_CollectionChanged;
        }

        private void Points_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            var neko = Points;
            MakePoints();
            InvalidateVisual();
        }

        private void MakePoints()
        {
            BezierPoints.Clear();
            BezierPoints.Add(Points[0]);
            for (int i = 1; i < Points.Count-1; i++)
            {
                BezierPoints.Add(Points[i]);
                BezierPoints.Add(Points[i]);
                BezierPoints.Add(Points[i]);
            }
            BezierPoints.Add(Points[^1]);
            BezierPoints.Add(Points[^1]);
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new();
                geometry.FillRule = FillRule.Nonzero;
                using (var context = geometry.Open())
                {
                    Draw(context);
                }
                geometry.Freeze();
                return geometry;

            }
        }
        private void Draw(StreamGeometryContext context)
        {
            context.BeginFigure(Points[0],
                false, false);//isFill,isClose
            context.PolyBezierTo(BezierPoints, true, false);
            //context.PolyBezierTo(new List<Point>()
            //{
            //    MyPoints[1],
            //    MyPoints[1],MyPoints[1],MyPoints[1],
            //    MyPoints[1],MyPoints[1],
            //}
            //, true, false);//isStroke, isSmoothJoin
        }

    }
}
