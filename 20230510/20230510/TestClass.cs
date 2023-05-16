using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using System.CodeDom;

namespace _20230510
{



    /// <summary>
    /// CanvasベースのMoveThumbにShapeSize表示
    /// Thumb自身のサイズをShapeのMyBoundsにバインド
    /// Shapeの表示座標をMyBoundsを元にオフセット
    /// </summary>
    public class ShapeSizeCanvasThumb2 : ShapeSizeCanvasThumb
    {
        #region 依存関係プロパティ

        //public PointCollection MyPoints
        //{
        //    get { return (PointCollection)GetValue(MyPointsProperty); }
        //    set { SetValue(MyPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointsProperty =
        //    DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(ShapeSizeCanvasThumb),
        //        new FrameworkPropertyMetadata(null,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public double MyStrokeThickness
        //{
        //    get { return (double)GetValue(MyStrokeThicknessProperty); }
        //    set { SetValue(MyStrokeThicknessProperty, value); }
        //}
        //public static readonly DependencyProperty MyStrokeThicknessProperty =
        //    DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(ShapeSizeCanvasThumb),
        //        new FrameworkPropertyMetadata(10.0,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public Brush MyStroke
        //{
        //    get { return (Brush)GetValue(MyStrokeProperty); }
        //    set { SetValue(MyStrokeProperty, value); }
        //}
        //public static readonly DependencyProperty MyStrokeProperty =
        //    DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(ShapeSizeCanvasThumb),
        //        new FrameworkPropertyMetadata(Brushes.MediumAquamarine,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        //public GeoShapeSize MyGeoShape
        //{
        //    get { return (GeoShapeSize)GetValue(MyGeoShapeProperty); }
        //    set { SetValue(MyGeoShapeProperty, value); }
        //}
        //public static readonly DependencyProperty MyGeoShapeProperty =
        //    DependencyProperty.Register(nameof(MyGeoShape), typeof(GeoShapeSize), typeof(ShapeSizeCanvasThumb),
        //        new FrameworkPropertyMetadata(null,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ

        public ObservableCollection<Thumb> AnchorThumb { get; private set; } = new();
        public ShapeSizeCanvasThumb2()
        {
            //MyTemplate.Background = new SolidColorBrush(Color.FromArgb(10, 0, 0, 255));
            //MyGeoShape = new();
            //MyTemplate.Children.Add(MyGeoShape);
            //SetMyBindings();
            Loaded += ShapeSizeCanvasThumb2_Loaded;
        }

        private void ShapeSizeCanvasThumb2_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < MyPoints.Count; i++)
            {
                Thumb t = new() { Width = 20, Height = 20 };
                AnchorThumb.Add(t);
                MyTemplate.Children.Add(t);
                Canvas.SetLeft(t, MyPoints[i].X);
                Canvas.SetTop(t, MyPoints[i].Y);
                t.DragDelta += T_DragDelta;
            }
        }

        private void T_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                int ii = AnchorThumb.IndexOf(t);
                double x = Canvas.GetLeft(t) + e.HorizontalChange;
                double y = Canvas.GetTop(t) + e.VerticalChange;
                if (x < 0)
                {
                    for (int i = 0; i < MyPoints.Count; i++)
                    {
                        Point p = MyPoints[i];
                        MyPoints[i] = new Point(p.X + x, p.Y);
                    }
                }
                else { MyPoints[ii] = new Point(x, y); }
                Canvas.SetLeft(t, x);
                Canvas.SetTop(t, y);
                
            }
        }

        private void SetMyBindings()
        {
            MyGeoShape.SetBinding(GeoShapeSize.AnchorPointsProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyPointsProperty)
            });
            MyGeoShape.SetBinding(GeoShapeSize.StrokeProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyStrokeProperty)
            });
            MyGeoShape.SetBinding(GeoShapeSize.StrokeThicknessProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyStrokeThicknessProperty)
            });

            //Shapeの表示座標オフセット
            MyGeoShape.SetBinding(Canvas.LeftProperty, new Binding()
            {
                Source = MyGeoShape,
                Path = new PropertyPath(GeoShapeSize.MyBoundsProperty),
                Converter = new MyConverterBounds2LeftOffset()
            });
            MyGeoShape.SetBinding(Canvas.TopProperty, new Binding()
            {
                Source = MyGeoShape,
                Path = new PropertyPath(GeoShapeSize.MyBoundsProperty),
                Converter = new MyConverterBounds2TopOffset()
            });


            //自身のサイズをShapeのサイズにバインド
            SetBinding(WidthProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShapeSize.MyBoundsProperty), Converter = new MyConverterBounds2Width() });
            SetBinding(HeightProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShapeSize.MyBoundsProperty), Converter = new MyConverterBounds2Height() });

        }
    }

    /// <summary>
    /// CanvasベースのMoveThumbにShapeSize表示
    /// Thumb自身のサイズをShapeのMyBoundsにバインド
    /// Shapeの表示座標をMyBoundsを元にオフセット
    /// </summary>
    public class ShapeSizeCanvasThumb : CanvasThumb
    {
        //#region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(ShapeSizeCanvasThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(ShapeSizeCanvasThumb),
                new FrameworkPropertyMetadata(10.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(ShapeSizeCanvasThumb),
                new FrameworkPropertyMetadata(Brushes.MediumAquamarine,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public GeoShapeSize MyGeoShape
        {
            get { return (GeoShapeSize)GetValue(MyGeoShapeProperty); }
            set { SetValue(MyGeoShapeProperty, value); }
        }
        public static readonly DependencyProperty MyGeoShapeProperty =
            DependencyProperty.Register(nameof(MyGeoShape), typeof(GeoShapeSize), typeof(ShapeSizeCanvasThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //#endregion 依存関係プロパティ

        public ShapeSizeCanvasThumb()
        {
            MyTemplate.Background = new SolidColorBrush(Color.FromArgb(10, 0, 0, 255));
            MyGeoShape = new();
            MyTemplate.Children.Add(MyGeoShape);
            SetMyBindings();
        }

        private void SetMyBindings()
        {
            MyGeoShape.SetBinding(GeoShapeSize.AnchorPointsProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyPointsProperty)
            });
            MyGeoShape.SetBinding(GeoShapeSize.StrokeProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyStrokeProperty)
            });
            MyGeoShape.SetBinding(GeoShapeSize.StrokeThicknessProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyStrokeThicknessProperty)
            });

            //Shapeの表示座標オフセット
            MyGeoShape.SetBinding(Canvas.LeftProperty, new Binding()
            {
                Source = MyGeoShape,
                Path = new PropertyPath(GeoShapeSize.MyBoundsProperty),
                Converter = new MyConverterBounds2LeftOffset()
            });
            MyGeoShape.SetBinding(Canvas.TopProperty, new Binding()
            {
                Source = MyGeoShape,
                Path = new PropertyPath(GeoShapeSize.MyBoundsProperty),
                Converter = new MyConverterBounds2TopOffset()
            });


            //自身のサイズをShapeのサイズにバインド
            SetBinding(WidthProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShapeSize.MyBoundsProperty), Converter = new MyConverterBounds2Width() });
            SetBinding(HeightProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShapeSize.MyBoundsProperty), Converter = new MyConverterBounds2Height() });

        }
    }

    /// <summary>
    /// CanvasベースのMoveThumbにShape表示
    /// Gridとの違いはサイズがNaNになること
    /// </summary>
    public class ShapeCanvasThumb : CanvasThumb
    {
        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(ShapeCanvasThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(ShapeCanvasThumb),
                new FrameworkPropertyMetadata(10.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(ShapeCanvasThumb),
                new FrameworkPropertyMetadata(Brushes.MediumAquamarine,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public GeoShape MyGeoShape
        {
            get { return (GeoShape)GetValue(MyGeoShapeProperty); }
            set { SetValue(MyGeoShapeProperty, value); }
        }
        public static readonly DependencyProperty MyGeoShapeProperty =
            DependencyProperty.Register(nameof(MyGeoShape), typeof(GeoShape), typeof(ShapeCanvasThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存関係プロパティ

        public ShapeCanvasThumb()
        {
            MyGeoShape = new();
            MyTemplate.Children.Add(MyGeoShape);
            SetMyBindings();
        }

        private void SetMyBindings()
        {
            MyGeoShape.SetBinding(GeoShape.AnchorPointsProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyPointsProperty)
            });
            MyGeoShape.SetBinding(GeoShape.StrokeProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyStrokeProperty)
            });
            MyGeoShape.SetBinding(GeoShape.StrokeThicknessProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyStrokeThicknessProperty)
            });
        }
    }

    /// <summary>
    /// GridベースのMoveThumbにShape表示
    /// </summary>
    public class ShapeGridThumb : GridThumb
    {
        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(ShapeGridThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(ShapeGridThumb),
                new FrameworkPropertyMetadata(10.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(ShapeGridThumb),
                new FrameworkPropertyMetadata(Brushes.MediumAquamarine,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public GeoShape MyGeoShape
        {
            get { return (GeoShape)GetValue(MyGeoShapeProperty); }
            set { SetValue(MyGeoShapeProperty, value); }
        }
        public static readonly DependencyProperty MyGeoShapeProperty =
            DependencyProperty.Register(nameof(MyGeoShape), typeof(GeoShape), typeof(ShapeGridThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存関係プロパティ

        public ShapeGridThumb()
        {
            MyGeoShape = new();
            MyTemplate.Children.Add(MyGeoShape);
            SetMyBindings();
        }

        private void SetMyBindings()
        {
            MyGeoShape.SetBinding(GeoShape.AnchorPointsProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyPointsProperty)
            });
            MyGeoShape.SetBinding(GeoShape.StrokeProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyStrokeProperty)
            });
            MyGeoShape.SetBinding(GeoShape.StrokeThicknessProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyStrokeThicknessProperty)
            });
        }
    }

    public class CanvasThumb : MoveThumb
    {
        public Canvas MyTemplate { get; set; } = new();
        public CanvasThumb()
        {
            MyTemplate = SetMyTemplate<Canvas>(MyTemplate.GetType());
        }

        private T SetMyTemplate<T>(Type type)
        {
            FrameworkElementFactory factory = new(type, "nemo");
            this.Template = new ControlTemplate() { VisualTree = factory };
            ApplyTemplate();
            return (T)this.Template.FindName("nemo", this);
        }
    }

    public class GridThumb : MoveThumb
    {
        public Grid MyTemplate { get; set; } = new();
        public GridThumb()
        {
            MyTemplate = SetMyTemplate<Grid>(MyTemplate.GetType());

        }

        private T SetMyTemplate<T>(Type type)
        {
            FrameworkElementFactory factory = new(type, "nemo");
            this.Template = new ControlTemplate() { VisualTree = factory };
            ApplyTemplate();
            return (T)this.Template.FindName("nemo", this);
        }
    }

    public class MoveThumb : Thumb
    {

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(MoveThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(MoveThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public MoveThumb()
        {
            DragDelta += GridThumb_DragDelta;
            SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Mode = BindingMode.TwoWay, Path = new PropertyPath(XProperty) });
            SetBinding(Canvas.TopProperty, new Binding() { Source = this, Mode = BindingMode.TwoWay, Path = new PropertyPath(YProperty) });
        }
        private void GridThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (e.OriginalSource == e.Source)
            {
                X += e.HorizontalChange;
                Y += e.VerticalChange;
            }
            
        }
    }

    public class TestClassA : Thumb
    {
        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public TestClassA()
        {
            MyPoints = new PointCollection();
        }

        private void MyObPoints_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

    public class TThumb : Thumb
    {


        public TThumb()
        {

            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
            DragDelta += TThumb_DragDelta;
        }

        private void TThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                var left = Canvas.GetLeft(t);
                Canvas.SetLeft(t, Canvas.GetLeft(t) + e.HorizontalChange);
                Canvas.SetTop(t, Canvas.GetTop(t) + e.VerticalChange);
            }
        }
    }
}
