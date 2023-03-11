using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace _20230310_Adorner
{
    public class GeoCanvas : Canvas
    {
        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeoCanvas),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyGeoAngle
        {
            get { return (double)GetValue(MyGeoAngleProperty); }
            set { SetValue(MyGeoAngleProperty, value); }
        }
        public static readonly DependencyProperty MyGeoAngleProperty =
            DependencyProperty.Register(nameof(MyGeoAngle), typeof(double), typeof(GeoCanvas),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double MyGeoScale
        {
            get { return (double)GetValue(MyGeoScaleProperty); }
            set { SetValue(MyGeoScaleProperty, value); }
        }
        public static readonly DependencyProperty MyGeoScaleProperty =
            DependencyProperty.Register(nameof(MyGeoScale), typeof(double), typeof(GeoCanvas),
                new FrameworkPropertyMetadata(1.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion 依存関係プロパティ


        public GeometryShape MyGeometryShape { get; protected set; }

        public GeoCanvas()
        {
            MyGeometryShape = new GeometryLine()
            {
                Stroke = Brushes.MediumAquamarine,
                StrokeThickness = 20
            };
            this.Children.Add(MyGeometryShape);
            SetLeft(MyGeometryShape, 0);
            SetTop(MyGeometryShape, 0);

            MySetBinding();
            Loaded += GeoCanvas_Loaded;
            //MyGeometryShape.SizeChanged += MyGeometryShape_SizeChanged;
            MyGeometryShape.LayoutUpdated += MyGeometryShape_LayoutUpdated;
        }


        public void RectCheck()
        {
            double minX = double.MaxValue;
            double minY = double.MaxValue;

            for (int i = 0; i < MyPoints.Count; i++)
            {
                Point pp = MyPoints[i];
                if (minX > pp.X) minX = pp.X;
                if (minY > pp.Y) minY = pp.Y;
            }
            var transform = MyGeometryShape.RenderTransform;
            var tfpoint = transform.Transform(new Point(minX, minY));
            var inverse = transform.Inverse;
            var inpoint = inverse.Transform(new Point(minX, minY));
        }

        //public void RectFix()
        //{
        //    double minX = double.MaxValue;
        //    double minY = double.MaxValue;

        //    for (int i = 0; i < MyPoints.Count; i++)
        //    {
        //        Point pp = MyPoints[i];
        //        if (minX > pp.X) minX = pp.X;
        //        if (minY > pp.Y) minY = pp.Y;
        //    }

        //    for (int i = 0; i < MyPoints.Count; i++)
        //    {
        //        Point pp = MyPoints[i];
        //        MyPoints[i] = new(pp.X - minX, pp.Y);
        //    }

        //    var left = GetLeft(this);
        //    left += minX;
        //    SetLeft(this, left);

        //    for (int i = 0; i < MyPoints.Count; i++)
        //    {
        //        Point pp = MyPoints[i];
        //        MyPoints[i] = new(pp.X, pp.Y - minY);
        //    }
        //    var top = GetTop(this);
        //    top += minY;
        //    SetTop(this, top);

        //}
        //public void RectFix2()
        //{
        //    double minX = double.MaxValue;
        //    double minY = double.MaxValue;

        //    for (int i = 0; i < MyPoints.Count; i++)
        //    {
        //        Point pp = MyPoints[i];
        //        if (minX > pp.X) minX = pp.X;
        //        if (minY > pp.Y) minY = pp.Y;
        //    }


        //    var locate = MyGeometryShape.RenderTransform.Inverse.Transform(new Point(minX, minY));
        //    for (int i = 0; i < MyPoints.Count; i++)
        //    {
        //        Point pp = MyPoints[i];
        //        MyPoints[i] = new(pp.X - locate.X, pp.Y - locate.Y);
        //        //MyPoints[i] = new(pp.X - minX, pp.Y - minY);

        //    }
        //    SetLeft(this, GetLeft(this) + locate.X);
        //    SetTop(this, GetTop(this) + locate.Y);
        //    //SetLeft(this, GetLeft(this) - locate.X);
        //    //SetTop(this, GetTop(this) - locate.Y);


        //}

        private void MyGeometryShape_LayoutUpdated(object? sender, EventArgs e)
        {
            //ShapeのLayoutUpdate時にCanvasのサイズ変更
            //本当は更新頻度の低い？SizeChangeで行いたいけど、
            //Thumbを動かしても稀にサイズ更新されない状態があるのでこっちで
            Rect bounds = VisualTreeHelper.GetDescendantBounds(MyGeometryShape);
            this.Width = bounds.Width;
            this.Height = bounds.Height;
        }
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            //Shapeの変形更新時、自身のサイズ変更
            Rect bounds = VisualTreeHelper.GetDescendantBounds(MyGeometryShape);
            if (!bounds.IsEmpty)
            {
                this.Width = bounds.Width;
                this.Height = bounds.Height;
            }

            return base.ArrangeOverride(arrangeSize);
        }

        private void GeoCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(this).Add(new GeoCanvasBoundsAdorner(this));
        }


        private void MySetBinding()
        {
            MyGeometryShape.SetBinding(GeometryShape.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });
            Binding b0 = new() { Source = this, Path = new PropertyPath(MyGeoAngleProperty) };
            Binding b1 = new() { Source = this, Path = new PropertyPath(MyGeoScaleProperty) };
            MultiBinding mb = new() { Converter = new MyConverterTransform() };
            mb.Bindings.Add(b0);
            mb.Bindings.Add(b1);
            MyGeometryShape.SetBinding(RenderTransformProperty, mb);
        }
    }





    public class GeoCanvasBoundsAdorner : Adorner
    {
        public VisualCollection MyVisuals { get; private set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];


        public Rectangle MyBoundsRed { get; private set; } = new() { Stroke = Brushes.Red, StrokeThickness = 1.0 };
        public Rectangle MyBoundsBlue { get; private set; } = new() { Stroke = Brushes.Blue, StrokeThickness = 1.0 };
        //public Rectangle MyBoundsGreen { get; private set; } = new() { Stroke = Brushes.Green, StrokeThickness = 1.0 };
        public GeometryShape MyTargetGeoShape { get; private set; }


        public GeoCanvasBoundsAdorner(GeoCanvas adornedElement) : base(adornedElement)
        {
            MyVisuals = new VisualCollection(this)
            {
                MyBoundsRed,
                MyBoundsBlue,
                //MyBoundsGreen,
            };
            MyTargetGeoShape = adornedElement.MyGeometryShape;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (MyTargetGeoShape.ActualHeight != 0)
            {
                MyBoundsRed.Arrange(MyTargetGeoShape.MyInternalBounds);
                MyBoundsBlue.Arrange(MyTargetGeoShape.MyExternalBounds);
            }
            return base.ArrangeOverride(finalSize);
        }
        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            MyBoundsRed.Arrange(MyTargetGeoShape.MyInternalBounds);
            MyBoundsBlue.Arrange(MyTargetGeoShape.MyExternalBounds);
            return base.GetLayoutClip(layoutSlotSize);
        }
    }


}
