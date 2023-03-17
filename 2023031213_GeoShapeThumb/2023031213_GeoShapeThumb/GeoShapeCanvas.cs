using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Input;

namespace _2023031213_GeoShapeThumb
{
    public class GeoShapeCanvas : Canvas
    {
        #region 依存関係プロパティ


        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeoShapeCanvas),
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
            DependencyProperty.Register(nameof(MyGeoAngle), typeof(double), typeof(GeoShapeCanvas),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double MyGeoScale
        {
            get { return (double)GetValue(MyGeoScaleProperty); }
            set { SetValue(MyGeoScaleProperty, value); }
        }
        public static readonly DependencyProperty MyGeoScaleProperty =
            DependencyProperty.Register(nameof(MyGeoScale), typeof(double), typeof(GeoShapeCanvas),
                new FrameworkPropertyMetadata(1.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(GeoShapeCanvas),
                new FrameworkPropertyMetadata(Brushes.Crimson,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MySTrokeThickness
        {
            get { return (double)GetValue(MySTrokeThicknessProperty); }
            set { SetValue(MySTrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MySTrokeThicknessProperty =
            DependencyProperty.Register(nameof(MySTrokeThickness), typeof(double), typeof(GeoShapeCanvas),
                new FrameworkPropertyMetadata(1.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Brush MyFill
        {
            get { return (Brush)GetValue(MyFillProperty); }
            set { SetValue(MyFillProperty, value); }
        }
        public static readonly DependencyProperty MyFillProperty =
            DependencyProperty.Register(nameof(MyFill), typeof(Brush), typeof(GeoShapeCanvas),
                new FrameworkPropertyMetadata(Brushes.DodgerBlue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyLineSmoothJoin
        {
            get { return (bool)GetValue(MyLineSmoothJoinProperty); }
            set { SetValue(MyLineSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyLineSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyLineSmoothJoin), typeof(bool), typeof(GeoShapeCanvas),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public bool MyLineClose
        {
            get { return (bool)GetValue(MyLineCloseProperty); }
            set { SetValue(MyLineCloseProperty, value); }
        }
        public static readonly DependencyProperty MyLineCloseProperty =
            DependencyProperty.Register(nameof(MyLineClose), typeof(bool), typeof(GeoShapeCanvas),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Visibility MyThumbVisible
        {
            get { return (Visibility)GetValue(MyThumbVisibleProperty); }
            set { SetValue(MyThumbVisibleProperty, value); }
        }
        public static readonly DependencyProperty MyThumbVisibleProperty =
            DependencyProperty.Register(nameof(MyThumbVisible), typeof(Visibility), typeof(GeoShapeCanvas),
                new FrameworkPropertyMetadata(Visibility.Visible,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public ShapeType MyShapeType
        {
            get { return (ShapeType)GetValue(MyShapeTypeProperty); }
            set { SetValue(MyShapeTypeProperty, value); }
        }
        public static readonly DependencyProperty MyShapeTypeProperty =
            DependencyProperty.Register(nameof(MyShapeType), typeof(ShapeType), typeof(GeoShapeCanvas),
                new FrameworkPropertyMetadata(ShapeType.Line,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ

        public GeometryShape MyShape { get; set; }
        public GeoShapeCanvas()
        {
            Background = Brushes.Gold;

            MyShape = new GeometryShape();
            Children.Add(MyShape);
            Loaded += GeoShapeCanvas_Loaded;
            LayoutUpdated += GeoShapeCanvas_LayoutUpdated;
        }

        private void GeoShapeCanvas_LayoutUpdated(object? sender, EventArgs e)
        {
            Test1();
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {            
            return base.ArrangeOverride(arrangeSize);
        }
        protected override Size MeasureOverride(Size constraint)
        {
            //Test1();
            return base.MeasureOverride(constraint);
        }
        private void Test1()
        {
            if (MyShape.MyExternalBounds != Rect.Empty)
            {
                Rect ex = MyShape.MyExternalBounds;
                //Rect rr = new(-r.X, -r.Y, r.Width, r.Height);
                //SetLeft(MyShape, -r.Left);
                //SetTop(MyShape, -r.Top);
                //MyShape.Arrange(rr);
                Rect rrr = new(0, 0, 100, 100);
                //MyShape.Arrange(rrr);//変化なし
                //Arrange(rrr);//over flow
                Canvas.SetLeft(MyShape, -ex.X);
                Canvas.SetTop(MyShape, -ex.Y);
                this.Width = ex.Width;
                this.Height = ex.Height;

            }

        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            var rsize = this.RenderSize;
            var ex = MyShape.MyExternalBounds;
            Pen pen = new Pen(MyShape.Stroke, MyShape.StrokeThickness);
            var geo = MyShape.MyGeometry.Clone();
            var widen = geo.GetWidenedPathGeometry(pen);
            var wex = widen.Bounds;

            base.OnMouseDown(e);
        }
        private void GeoShapeCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            MySetBinding();
        }
        protected void MySetBinding()
        {
            MyShape.SetBinding(GeometryShape.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });

            MyShape.SetBinding(Shape.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeProperty) });
            MyShape.SetBinding(Shape.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MySTrokeThicknessProperty) });
            MyShape.SetBinding(Shape.FillProperty, new Binding() { Source = this, Path = new PropertyPath(MyFillProperty) });

            MyShape.SetBinding(GeometryShape.MyLineSmoothJoinProperty, new Binding() { Source = this, Path = new PropertyPath(MyLineSmoothJoinProperty) });
            MyShape.SetBinding(GeometryShape.MyLineCloseProperty, new Binding() { Source = this, Path = new PropertyPath(MyLineCloseProperty) });
            MyShape.SetBinding(GeometryShape.MyAnchorVisibleProperty, new Binding() { Source = this, Path = new PropertyPath(MyThumbVisibleProperty) });
            MyShape.SetBinding(GeometryShape.MyShapeTypeProperty, new Binding() { Source = this, Path = new PropertyPath(MyShapeTypeProperty) });
            //変形は回転、拡大のみ対応。変形させるのはThumb自身じゃなくて表示している図形
            Binding b0 = new() { Source = this, Path = new PropertyPath(MyGeoAngleProperty) };
            Binding b1 = new() { Source = this, Path = new PropertyPath(MyGeoScaleProperty) };
            MultiBinding mb = new() { Converter = new MyConverterTransform() };
            mb.Bindings.Add(b0);
            mb.Bindings.Add(b1);
            MyShape.SetBinding(RenderTransformProperty, mb);

        }

    }
}
