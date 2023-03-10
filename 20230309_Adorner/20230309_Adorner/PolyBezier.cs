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
using System.Windows.Media.Media3D;

namespace _20230309_Adorner
{
    public class PolyGeoBase : Shape
    {

        public PointCollection Points
        {
            get { return (PointCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(PolyGeoBase),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Geometry MyGeometry { get; protected set; } = new PathGeometry();
        public Rect MyTransformedExternalBounds { get; protected set; }//変形＋太さ
        public Rect MyTransformedInternalBounds { get; protected set; }//変形

        protected override Geometry DefiningGeometry => Geometry.Empty;

        //Transform変更時、MyBounds更新
        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            var geo = MyGeometry.Clone();
            geo.Transform = RenderTransform;
            MyTransformedInternalBounds = geo.Bounds;
            var TFWide = geo.GetWidenedPathGeometry(new Pen(Stroke, StrokeThickness));
            MyTransformedExternalBounds = TFWide.Bounds;

            return base.GetLayoutClip(layoutSlotSize);
        }

    }

    public class PolyGeoLine : PolyGeoBase
    {
        public ExAdorner ExAdorner { get; private set; }


        public PolyGeoLine()
        {
            ExAdorner = new ExAdorner(this);
            Loaded += PolyGeoLine_Loaded;
        }

        private void PolyGeoLine_Loaded(object sender, RoutedEventArgs e)
        {
            if (AdornerLayer.GetAdornerLayer(this) is AdornerLayer layer)
            {
                layer.Add(ExAdorner);
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                if (Points.Count == 0) return Geometry.Empty;
                Stroke = Brushes.Orange;
                StrokeThickness = 20;

                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    context.BeginFigure(Points[0], false, false);//isfill,iscloce
                    context.PolyLineTo(Points.Skip(1).ToArray(), true, false);//isStroke, isSmooth
                }
                geometry.Freeze();
                MyGeometry = geometry;

                return geometry;
            }
        }

    }
    public class PolyBezier : PolyGeoBase
    {
        public ExAdorner ExAdorner { get; private set; }
        public PolyBezier()
        {
            ExAdorner = new ExAdorner(this);
            Loaded += PolyBezier_Loaded;
        }

        private void PolyBezier_Loaded(object sender, RoutedEventArgs e)
        {
            if (AdornerLayer.GetAdornerLayer(this) is AdornerLayer layer)
            {
                layer.Add(ExAdorner);
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                if (Points.Count == 0) return Geometry.Empty;
                Stroke = Brushes.Orange;
                StrokeThickness = 20;

                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    context.BeginFigure(Points[0], false, false);//isfill,iscloce
                    context.PolyBezierTo(Points.Skip(1).ToArray(), true, false);//isStroke, isSmooth
                }
                geometry.Freeze();
                MyGeometry = geometry;
                return geometry;
            }
        }
    }

    public class ExAdorner : Adorner
    {
        #region お約束

        public VisualCollection MyVisuals { get; private set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];
        #endregion お約束

        public List<Thumb> MyThumbs { get; private set; } = new();
        public Canvas MyCanvas { get; private set; } = new();
        public PolyGeoBase MyTarget { get; private set; }

        public ExAdorner(PolyGeoBase adornedElement) : base(adornedElement)
        {
            MyVisuals = new VisualCollection(this);
            MyTarget = adornedElement;

            MyVisuals.Add(MyCanvas);

            Loaded += ExAdorner_Loaded;

        }

        private void ExAdorner_Loaded(object sender, RoutedEventArgs e)
        {

            foreach (var item in MyTarget.Points)
            {
                Thumb t = new()
                {
                    Width = 20,
                    Height = 20,
                    Background = Brushes.Red,
                    Opacity = 0.5,
                };
                MyThumbs.Add(t);
                MyCanvas.Children.Add(t);
                Canvas.SetLeft(t, item.X);
                Canvas.SetTop(t, item.Y);
                t.DragDelta += Thumb_DragDelta;
            }

        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                int ii = MyThumbs.IndexOf(t);
                PointCollection pc = MyTarget.Points;
                double x = pc[ii].X + e.HorizontalChange;
                double y = pc[ii].Y + e.VerticalChange;
                Canvas.SetLeft(t, x);
                Canvas.SetTop(t, y);
                pc[ii] = new Point(x, y);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //return base.ArrangeOverride(finalSize);
            base.ArrangeOverride(finalSize);

            Rect canvasRect = VisualTreeHelper.GetDescendantBounds(MyCanvas);
            if (canvasRect.IsEmpty)
            {
                MyCanvas.Arrange(new Rect(finalSize));
            }
            else
            {
                MyCanvas.Arrange(canvasRect);
            }

            return finalSize;
        }

    }
}
