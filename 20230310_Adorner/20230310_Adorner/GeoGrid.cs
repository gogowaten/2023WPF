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

    //見た目通りのRect表示
    //頂点移動では追従するけど、回転では無視される
    public class GeoGrid : Grid
    {
        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeoGrid),
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
            DependencyProperty.Register(nameof(MyGeoAngle), typeof(double), typeof(GeoGrid),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double MyGeoScale
        {
            get { return (double)GetValue(MyGeoScaleProperty); }
            set { SetValue(MyGeoScaleProperty, value); }
        }
        public static readonly DependencyProperty MyGeoScaleProperty =
            DependencyProperty.Register(nameof(MyGeoScale), typeof(double), typeof(GeoGrid),
                new FrameworkPropertyMetadata(1.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion 依存関係プロパティ

        public GeometryShape MyGeometryShape { get; protected set; }

        public GeoGrid()
        {
            MyGeometryShape = new GeometryLine()
            {
                Stroke = Brushes.MediumAquamarine,
                StrokeThickness = 20
            };
            this.Children.Add(MyGeometryShape);

            MySetBinding();
            Loaded += GeoGrid_Loaded;

        }

        public void RectCheck()
        {
            var bounds = VisualTreeHelper.GetDescendantBounds(this);
            var shapeBounds = VisualTreeHelper.GetDescendantBounds(MyGeometryShape);
            var shapeBounds2 = MyGeometryShape.MyExternalTFBounds;

        }

        ////自身のサイズを決めるのは良くないのは、座標に依っては見切れてしまうから
        //protected override Size ArrangeOverride(Size arrangeSize)
        //{
        //    return base.ArrangeOverride(arrangeSize);
        //}

        //protected override Size MeasureOverride(Size constraint)
        //{
        //    return base.MeasureOverride(constraint);
        //}

        private void GeoGrid_Loaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(this).Add(new GeoGridBoundsAdorner(this));
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




    public class GeoGridBoundsAdorner : Adorner
    {
        public VisualCollection MyVisuals { get; private set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];

        public Rectangle MyRectangleBlue { get; private set; } = new() { Stroke = Brushes.Blue, StrokeThickness = 1.0 };

        public GeometryShape MyTargetGeoShape { get; private set; }


        public GeoGridBoundsAdorner(GeoGrid adornedElement) : base(adornedElement)
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
