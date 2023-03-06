using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _20230303
{
    public class PolyBezier : Shape
    {

        public PointCollection Points
        {
            get { return (PointCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(PolyBezier),
                new FrameworkPropertyMetadata(new PointCollection() { new Point(0, 0), new Point(100, 100) },
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Geometry MyGeometry { get; set; }
        public PolyBezier()
        {
            
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                if (Points.Count == 0) { return Geometry.Empty; }
                Stroke = Brushes.Red;
                StrokeThickness = 30;

                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    context.BeginFigure(Points[0], false, false);//isFill,isClose
                    var neko = Points.Skip(1).ToList();
                    context.PolyBezierTo(neko, true, false);
                }
                geometry.Freeze();
                MyGeometry = geometry;
                return geometry;
            }
        }
    }
}
