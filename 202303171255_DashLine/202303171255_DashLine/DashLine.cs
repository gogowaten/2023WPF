using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace _202303171255_DashLine
{
    class DashLine : Shape
    {

        public Point P1
        {
            get { return (Point)GetValue(P1Property); }
            set { SetValue(P1Property, value); }
        }
        public static readonly DependencyProperty P1Property =
            DependencyProperty.Register(nameof(P1), typeof(Point), typeof(DashLine),
                new FrameworkPropertyMetadata(new Point(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Point P2
        {
            get { return (Point)GetValue(P2Property); }
            set { SetValue(P2Property, value); }
        }
        public static readonly DependencyProperty P2Property =
            DependencyProperty.Register(nameof(P2), typeof(Point), typeof(DashLine),
                new FrameworkPropertyMetadata(new Point(100, 100),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public DoubleCollection MyDoubleCollection
        {
            get { return (DoubleCollection)GetValue(MyDoubleCollectionProperty); }
            set { SetValue(MyDoubleCollectionProperty, value); }
        }
        public static readonly DependencyProperty MyDoubleCollectionProperty =
            DependencyProperty.Register(nameof(MyDoubleCollection), typeof(DoubleCollection), typeof(DashLine),
                new FrameworkPropertyMetadata(new DoubleCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        

        public DashLine()
        {   
            SetBinding(Shape.StrokeDashArrayProperty, new Binding() { Source = this, Path = new PropertyPath(MyDoubleCollectionProperty) });
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    context.BeginFigure(P1, false, false);
                    context.LineTo(P2, true, false);
                }
                geometry.Freeze();
                return geometry;
            }
        }
    }




}
