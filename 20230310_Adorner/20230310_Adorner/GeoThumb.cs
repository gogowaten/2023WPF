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
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace _20230310_Adorner
{
    public class GeoThumb : Thumb
    {
        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeoThumb),
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
            DependencyProperty.Register(nameof(MyGeoAngle), typeof(double), typeof(GeoThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double MyGeoScale
        {
            get { return (double)GetValue(MyGeoScaleProperty); }
            set { SetValue(MyGeoScaleProperty, value); }
        }
        public static readonly DependencyProperty MyGeoScaleProperty =
            DependencyProperty.Register(nameof(MyGeoScale), typeof(double), typeof(GeoThumb),
                new FrameworkPropertyMetadata(1.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion 依存関係プロパティ

        public GeometryShape MyGeometryShape { get; protected set; }
        public GeoThumb()
        {
            MyGeometryShape = SetTemplate();
            MyGeometryShape.Stroke = Brushes.Crimson;

            MySetBinding();
            Loaded += GeoThumb_Loaded;
            DragDelta += GeoThumb_DragDelta;
        }

        public BitmapSource GetBitmap()
        {
            Pen pen = new(MyGeometryShape.Stroke, MyGeometryShape.StrokeThickness);
            Geometry geo = MyGeometryShape.MyGeometry.Clone();
            PathGeometry widen = geo.GetWidenedPathGeometry(pen);
            widen.Transform = MyGeometryShape.RenderTransform;
            Rect rect = widen.Bounds;
            DrawingVisual dv = new() { Offset = new Vector(-rect.X, -rect.Y) };
            using (var context = dv.RenderOpen())
            {
                context.DrawGeometry(MyGeometryShape.Stroke, null, widen);
            }
            RenderTargetBitmap bitmap = new((int)(rect.Width + 1), (int)(rect.Height + 1), 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
            return bitmap;
        }

        private void GeoThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
        }

        private GeometryShape SetTemplate()
        {
            FrameworkElementFactory factory = new(typeof(GeometryLine), "nemo");
            this.Template = new ControlTemplate() { VisualTree = factory };
            this.ApplyTemplate();
            if (this.Template.FindName("nemo", this) is GeometryLine shape)
            {
                return shape;
            }
            else
            {
                throw new Exception();
            }
        }

        private void GeoThumb_Loaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(this).Add(new GeoThumbBoundsAdorner(this));
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



    public class GeoThumbBoundsAdorner : Adorner
    {
        public VisualCollection MyVisuals { get; private set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];

        public Rectangle MyRectangleBlue { get; private set; } = new() { Stroke = Brushes.Blue, StrokeThickness = 1.0 };

        public GeometryShape MyTargetGeoShape { get; private set; }


        public GeoThumbBoundsAdorner(GeoThumb adornedElement) : base(adornedElement)
        {
            MyVisuals = new VisualCollection(this) { MyRectangleBlue, };
            MyTargetGeoShape = adornedElement.MyGeometryShape;
            //MySetBiding();
        }

        //Bindingでは表示されない
        //private void MySetBiding()
        //{
        //    MyRectangleBlue.SetBinding(WidthProperty, new Binding(nameof(GeometryShape.MyTFWidth)) { Source = MyTargetGeoShape });
        //    MyRectangleBlue.SetBinding(HeightProperty, new Binding(nameof(GeometryShape.MyTFHeight)) { Source = MyTargetGeoShape });

        //}

        //ArrangeOverrideとGetLayoutClipの両方必要
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (MyTargetGeoShape.ActualHeight != 0)
            {
                MyRectangleBlue.Arrange(MyTargetGeoShape.MyExternalBounds);
            }
            return base.ArrangeOverride(finalSize);
        }
        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            MyRectangleBlue.Arrange(MyTargetGeoShape.MyExternalBounds);
            return base.GetLayoutClip(layoutSlotSize);
        }
    }

}
