using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace _20230310_Adorner
{
    public class GeometryShapeBase : Shape
    {

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeometryShapeBase),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Geometry MyGeometry { get; protected set; } = new PathGeometry();
        public Rect MyExternalBounds { get; protected set; }//外観のRect
        public Rect MyInternalBounds { get; protected set; }//PointsだけのRect

        protected override Geometry DefiningGeometry => Geometry.Empty;

        //変形時にBoundsを更新、これは変形してもArrangeは無反応だから
        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            Geometry geo = MyGeometry.Clone();
            geo.Transform = RenderTransform;
            MyInternalBounds = geo.Bounds;
            MyExternalBounds = geo.GetWidenedPathGeometry(new Pen(Stroke, StrokeThickness)).Bounds;
            return base.GetLayoutClip(layoutSlotSize);
        }
    }

    public class GeometryLine : GeometryShapeBase
    {
        public GeometryLine()
        {

        }
    }


    public class GeometryPolygon : GeometryShapeBase { }
    public class GeometryBezier : GeometryShapeBase { }
    public class GeometryAdorner : Adorner
    {
        public VisualCollection MyVisuals { get; private set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];

        public List<Thumb> MyThumbs { get; private set; } = new();
        public Canvas MyCanvas { get; private set; } = new();
        public GeometryShapeBase MyTargetGeoShape { get; private set; }
        public GeometryAdorner(GeometryShapeBase adornedElement) : base(adornedElement)
        {
            MyVisuals = new VisualCollection(this);
            MyTargetGeoShape = adornedElement;
            MyVisuals.Add(MyCanvas);
            Loaded += GeometryAdorner_Loaded;
        }

        private void GeometryAdorner_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in MyTargetGeoShape.MyPoints)
            {
                Thumb thumb = new() { Width = 20, Height = 20, Background = Brushes.Red, Opacity = 0.5 };
                MyThumbs.Add(thumb);
                MyCanvas.Children.Add(thumb);
                SetLocate(thumb, item);
                thumb.DragDelta += Thumb_DragDelta;
            }
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                int i = MyThumbs.IndexOf(t);
                PointCollection points = MyTargetGeoShape.MyPoints;
                double x = points[i].X + e.HorizontalChange;
                double y = points[i].Y + e.VerticalChange;
                points[i] = new Point(x, y);
                SetLocate(t, points[i]);
            }
        }
        private void SetLocate(Thumb thumb, Point point)
        {
            Canvas.SetLeft(thumb, point.X);
            Canvas.SetTop(thumb, point.Y);
        }
    }
}
