using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;


//2023WPF/GeoThumb.cs at main · gogowaten/2023WPF
//https://github.com/gogowaten/2023WPF/blob/main/20230310_Adorner/20230310_Adorner/GeoThumb.cs
//上のコピペから改変

//ShapeをTemplateにしたThumb
//ドラッグ移動できる

//回転が難しい、左上を中心に回転するはいいんだけど、
//中央を中心にする場合ができない、表示自体はできるけど、回転後にThumbを動かすと
//全く違う方へ移動する

namespace _2023031213_GeoShapeThumb
{
    public class GeoShapeThumb : Thumb
    {
        #region 依存関係プロパティ


        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeoShapeThumb),
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
            DependencyProperty.Register(nameof(MyGeoAngle), typeof(double), typeof(GeoShapeThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double MyGeoScale
        {
            get { return (double)GetValue(MyGeoScaleProperty); }
            set { SetValue(MyGeoScaleProperty, value); }
        }
        public static readonly DependencyProperty MyGeoScaleProperty =
            DependencyProperty.Register(nameof(MyGeoScale), typeof(double), typeof(GeoShapeThumb),
                new FrameworkPropertyMetadata(1.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(GeoShapeThumb),
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
            DependencyProperty.Register(nameof(MySTrokeThickness), typeof(double), typeof(GeoShapeThumb),
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
            DependencyProperty.Register(nameof(MyFill), typeof(Brush), typeof(GeoShapeThumb),
                new FrameworkPropertyMetadata(Brushes.DodgerBlue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyLineSmoothJoin
        {
            get { return (bool)GetValue(MyLineSmoothJoinProperty); }
            set { SetValue(MyLineSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyLineSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyLineSmoothJoin), typeof(bool), typeof(GeoShapeThumb),
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
            DependencyProperty.Register(nameof(MyLineClose), typeof(bool), typeof(GeoShapeThumb),
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
            DependencyProperty.Register(nameof(MyThumbVisible), typeof(Visibility), typeof(GeoShapeThumb),
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
            DependencyProperty.Register(nameof(MyShapeType), typeof(ShapeType), typeof(GeoShapeThumb),
                new FrameworkPropertyMetadata(ShapeType.Line,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ

        public GeometryShape MyGeometryShape { get; protected set; }
        public ContextMenu MyMenu { get; protected set; } = new();

        public GeoShapeThumb()
        {
            MyGeometryShape = SetTemplate();

            Loaded += GeoThumb_Loaded;
            DragDelta += GeoThumb_DragDelta;

        }
        private GeometryShape SetTemplate()
        {
            FrameworkElementFactory factory = new(typeof(GeometryShape), "nemo");
            this.Template = new ControlTemplate() { VisualTree = factory };
            this.ApplyTemplate();
            if (this.Template.FindName("nemo", this) is GeometryShape shape)
            {
                return shape;
            }
            else { throw new Exception(); }
        }

        //protected T SetTemplate2<T>()
        //{
        //    FrameworkElementFactory factory = new(typeof(T), "nemo");
        //    this.Template = new ControlTemplate() { VisualTree = factory };
        //    this.ApplyTemplate();
        //    if (this.Template.FindName("nemo", this) is T shape)
        //    {
        //        return shape;
        //    }
        //    else { throw new Exception(); }
        //}

        public BitmapSource GetBitmap()
        {
            return MyGeometryShape.GetBitmap();
        }

        private void GeoThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
        }

        private void GeoThumb_Loaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(this).Add(new GeoThumbBoundsAdorner(this));
            //BindingはLoadedで行うとPointsとか正常にBindingされる、逆に起動時だとできない
            MySetBinding();

            //中心回転…にはなるけど、位置がずれまくる
            //MyGeometryShape.RenderTransformOrigin = new Point(0.5, 0.5);

            AdornerLayer.GetAdornerLayer(this).Add(new ThumbAdorner(this));
        }


        protected void MySetBinding()
        {
            MyGeometryShape.SetBinding(GeometryShape.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });

            //変形は回転、拡大のみ対応。変形させるのはThumb自身じゃなくて表示している図形
            Binding b0 = new() { Source = this, Path = new PropertyPath(MyGeoAngleProperty) };
            Binding b1 = new() { Source = this, Path = new PropertyPath(MyGeoScaleProperty) };

            //MultiBinding mb = new() { Converter = new MyConverterTransform2() };
            //mb.ConverterParameter = MyGeometryShape;

            MultiBinding mb = new() { Converter = new MyConverterTransform() };

            mb.Bindings.Add(b0);
            mb.Bindings.Add(b1);
            MyGeometryShape.SetBinding(RenderTransformProperty, mb);

            MyGeometryShape.SetBinding(Shape.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeProperty) });
            MyGeometryShape.SetBinding(Shape.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MySTrokeThicknessProperty) });
            MyGeometryShape.SetBinding(Shape.FillProperty, new Binding() { Source = this, Path = new PropertyPath(MyFillProperty) });

            MyGeometryShape.SetBinding(GeometryShape.MyLineSmoothJoinProperty, new Binding() { Source = this, Path = new PropertyPath(MyLineSmoothJoinProperty) });
            MyGeometryShape.SetBinding(GeometryShape.MyLineCloseProperty, new Binding() { Source = this, Path = new PropertyPath(MyLineCloseProperty) });
            MyGeometryShape.SetBinding(GeometryShape.MyAnchorVisibleProperty, new Binding() { Source = this, Path = new PropertyPath(MyThumbVisibleProperty) });
            MyGeometryShape.SetBinding(GeometryShape.MyShapeTypeProperty, new Binding() { Source = this, Path = new PropertyPath(MyShapeTypeProperty) });
            
        }

    }



    //public class GeoLineThumb : GeoShapeThumb
    //{
    //    public GeoLineThumb()
    //    {
    //        MyGeometryShape = SetTemplate2<GeometryShapeLine>();
    //    }
    //}
    //public class GeoBezierThumb : GeoShapeThumb
    //{
    //    public GeoBezierThumb()
    //    {
    //        MyGeometryShape = SetTemplate2<GeometryShapeBezier>();
    //    }
    //}
    //public class GeoFillThumb : GeoShapeThumb
    //{
    //    public GeoFillThumb()
    //    {
    //        MyGeometryShape = SetTemplate2<GeometryShapeFill>();
    //    }
    //}



    public class GeoThumbBoundsAdorner : Adorner
    {
        public VisualCollection MyVisuals { get; private set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];

        public Rectangle MyRectangleBlue { get; private set; } = new() { Stroke = new SolidColorBrush(Color.FromArgb(100,0,0,255)), StrokeThickness = 1.0 };
        
        public GeometryShape MyTargetGeoShape { get; private set; }


        public GeoThumbBoundsAdorner(GeoShapeThumb adornedElement) : base(adornedElement)
        {
            MyVisuals = new VisualCollection(this) { MyRectangleBlue};
            MyTargetGeoShape = adornedElement.MyGeometryShape;

            MyTargetGeoShape.LayoutUpdated += MyTargetGeoShape_LayoutUpdated;

            //MySetBiding();
        }

        //表示Rect更新はTargetのイベントで行う、これは自身のArrangeOverrideやGetLayoutClipでは
        //Shapeの変形時に反応しないから
        private void MyTargetGeoShape_LayoutUpdated(object? sender, EventArgs e)
        {
            if (MyTargetGeoShape.MyExternalBounds.IsEmpty == false)
            {
                MyRectangleBlue.Arrange(MyTargetGeoShape.MyExternalBounds);
            }
        }

        ////Bindingでは表示されない
        //private void MySetBiding()
        //{
        //    MyRectangleBlue.SetBinding(WidthProperty, new Binding(nameof(GeometryShape.MyTFWidth)) { Source = MyTargetGeoShape });
        //    MyRectangleBlue.SetBinding(HeightProperty, new Binding(nameof(GeometryShape.MyTFHeight)) { Source = MyTargetGeoShape });

        //}

        ////ArrangeOverrideとGetLayoutClipの両方必要
        //protected override Size ArrangeOverride(Size finalSize)
        //{
        //    if (MyTargetGeoShape.ActualHeight != 0)
        //    {
        //        MyRectangleBlue.Arrange(MyTargetGeoShape.MyExternalBounds);
        //    }
        //    return base.ArrangeOverride(finalSize);
        //}
        //protected override Geometry GetLayoutClip(Size layoutSlotSize)
        //{
        //    if (!MyTargetGeoShape.MyExternalBounds.IsEmpty)
        //    {
        //        MyRectangleBlue.Arrange(MyTargetGeoShape.MyExternalBounds);
        //    }

        //    return base.GetLayoutClip(layoutSlotSize);
        //}

    }

    //public class MyConverterTransform2 : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        double angle = (double)values[0];
    //        double scale = (double)values[1];
    //        GeometryShape gs = (GeometryShape)parameter;
    //        Rect r = gs.MyExternalBoundsNotTF;
    //        TransformGroup group = new();
    //        group.Children.Add(new RotateTransform(angle, r.Width / 2.0, r.Height / 2.0));
    //        group.Children.Add(new ScaleTransform(scale, scale));
    //        return group;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    public class ThumbAdorner : Adorner
    {
        public VisualCollection MyVisuals { get; private set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];


        public Rectangle MyRectangle {  get; private set; } = new() { Stroke=new SolidColorBrush(Color.FromArgb(100,255,0,0)), StrokeThickness = 1.0 };
        public ThumbAdorner(UIElement adornedElement) : base(adornedElement)
        {
            MyVisuals = new VisualCollection(this) { MyRectangle };
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect r = new(this.RenderSize);
            MyRectangle.Arrange(r);
            return base.ArrangeOverride(finalSize);
        }
    }


    public class MyConverterTransform : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double angle = (double)values[0];
            double scale = (double)values[1];
            TransformGroup group = new();
            group.Children.Add(new RotateTransform(angle));
            group.Children.Add(new ScaleTransform(scale, scale));
            return group;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
