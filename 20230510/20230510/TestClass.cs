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
    /// いいね
    /// GridのTemplateを使ったThumbに
    /// ShapeCanvasを表示
    /// アンカー点のThumbはThumbからShapeCanvasに追加している理由は
    /// もしShapeCanvasクラスに持たせると、アンカー点変更時に本体の移動が加わったときに
    /// Canvasを移動することになって不都合だからだった気がする、ﾖｸ ｵﾎﾞｴﾃｲﾅｲﾉﾀﾞ
    /// Thumb
    ///     ┗Grid(Template)
    ///         ┗ShapeCanvas
    ///             ┗Shape
    ///             ┗AnchorThumbs
    /// </summary>
    public class ShapeSizeGridThumb3 : GridThumb
    {
        #region 依存関係プロパティ

        public PointCollection MyAnchorPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyAnchorPoints), typeof(PointCollection), typeof(ShapeSizeGridThumb3),
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
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(ShapeSizeGridThumb3),
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
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(ShapeSizeGridThumb3),
                new FrameworkPropertyMetadata(Brushes.MediumAquamarine,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public ShapeSizeCanvasWithoutAnchorThumb MyShapeCanvas
        {
            get { return (ShapeSizeCanvasWithoutAnchorThumb)GetValue(MyShapeCanvasProperty); }
            set { SetValue(MyShapeCanvasProperty, value); }
        }
        public static readonly DependencyProperty MyShapeCanvasProperty =
            DependencyProperty.Register(nameof(MyShapeCanvas), typeof(ShapeSizeCanvasWithoutAnchorThumb), typeof(ShapeSizeGridThumb3),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ
        public ObservableCollection<Thumb> MyAnchorThumbs { get; private set; } = new();

        public ShapeSizeGridThumb3()
        {
            MyShapeCanvas = new();
            MyTemplate.Children.Add(MyShapeCanvas);
            SetMyBindings();
            Loaded += ShapeSizeGridThumb3_Loaded;
        }

        private void ShapeSizeGridThumb3_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < MyAnchorPoints.Count; i++)
            {
                Thumb tt = new() { Width = 20, Height = 20 };
                MyAnchorThumbs.Add(tt);
                MyShapeCanvas.Children.Add(tt);
                Canvas.SetLeft(tt, MyAnchorPoints[i].X);
                Canvas.SetTop(tt, MyAnchorPoints[i].Y);
                tt.DragDelta += Tt_DragDelta;
            }
        }

        private void Tt_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                int ii = MyAnchorThumbs.IndexOf(t);
                double x = Canvas.GetLeft(t) + e.HorizontalChange;
                double y = Canvas.GetTop(t) + e.VerticalChange;
                MyAnchorPoints[ii] = new Point(x, y);
                Rect pointsRect = GetPointsRect(MyAnchorPoints);
                //全体(自身)が移動になる状況は
                //PointsRect座標が0,0以外になる場合、このときは
                //Pointsをオフセット＋自身を移動
                if (pointsRect.Top != 0 || pointsRect.Left != 0)
                {
                    for (int i = 0; i < MyAnchorPoints.Count; i++)
                    {
                        Point pp = MyAnchorPoints[i];
                        SetLocatePointAndAnchorThumb(i, pp.X - pointsRect.X, pp.Y - pointsRect.Y);
                    }
                    this.X += pointsRect.X;
                    this.Y += pointsRect.Y;
                }
                else
                {
                    Canvas.SetLeft(t, x);
                    Canvas.SetTop(t, y);
                }
            }
        }

        private void SetLocatePointAndAnchorThumb(int idx, double x, double y)
        {
            MyAnchorPoints[idx] = new Point(x, y);
            Canvas.SetLeft(MyAnchorThumbs[idx], x);
            Canvas.SetTop(MyAnchorThumbs[idx], y);
        }


        /// <summary>
        /// PointCollectionのPointがすべて収まるRect取得
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>        
        private static Rect GetPointsRect(IEnumerable<Point> points)
        {
            double minx = double.MaxValue;
            double miny = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            foreach (Point pp in points)
            {
                if (pp.X < minx) minx = pp.X;
                if (pp.Y < miny) miny = pp.Y;
                if (pp.X > maxX) maxX = pp.X;
                if (pp.Y > maxY) maxY = pp.Y;
            }
            return new Rect(minx, miny, maxX, maxY);
        }


        private void SetMyBindings()
        {
            MyShapeCanvas.SetBinding(ShapeSizeCanvasWithoutAnchorThumb.MyAnchorPointsProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyPointsProperty)
            });
            MyShapeCanvas.SetBinding(ShapeSizeCanvasWithoutAnchorThumb.MyStrokeProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyStrokeProperty)
            });
            MyShapeCanvas.SetBinding(ShapeSizeCanvasWithoutAnchorThumb.MyStrokeThicknessProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyStrokeThicknessProperty)
            });

            

            //Shapeの表示座標オフセット
            MyShapeCanvas.SetBinding(Canvas.LeftProperty, new Binding()
            {
                Source = MyShapeCanvas.MyGeoShape,
                Path = new PropertyPath(GeoShapeSize.MyBoundsProperty),
                Converter = new MyConverterBounds2LeftOffset()
            });
            MyShapeCanvas.SetBinding(Canvas.TopProperty, new Binding()
            {
                Source = MyShapeCanvas.MyGeoShape,
                Path = new PropertyPath(GeoShapeSize.MyBoundsProperty),
                Converter = new MyConverterBounds2TopOffset()
            });

            //自身の座標
            SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Mode = BindingMode.TwoWay, Path = new PropertyPath(XProperty) });
            SetBinding(Canvas.TopProperty, new Binding() { Source = this, Mode = BindingMode.TwoWay, Path = new PropertyPath(YProperty) });

            ////自身のサイズをShapeのサイズにバインド
            //SetBinding(WidthProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShapeSize.MyBoundsProperty), Converter = new MyConverterBounds2Width() });
            //SetBinding(HeightProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShapeSize.MyBoundsProperty), Converter = new MyConverterBounds2Height() });

        }


    }

    /// <summary>
    /// 失敗、なんか違う
    /// Thumb
    ///     ┗Grid(Template)
    ///         ┗Canvas(ShapeSizeCanvas)
    ///             ┗Shape
    /// </summary>
    public class ShapeSizeGridThumb2 : Thumb
    {
        #region 依存関係プロパティ

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(ShapeSizeGridThumb2),
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
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(ShapeSizeGridThumb2),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public PointCollection MyAnchorPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyAnchorPoints), typeof(PointCollection), typeof(ShapeSizeGridThumb2),
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
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(ShapeSizeGridThumb2),
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
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(ShapeSizeGridThumb2),
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
            DependencyProperty.Register(nameof(MyGeoShape), typeof(GeoShapeSize), typeof(ShapeSizeGridThumb2),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyAnchorThumbSize
        {
            get { return (double)GetValue(MyAnchorThumbSizeProperty); }
            set { SetValue(MyAnchorThumbSizeProperty, value); }
        }
        public static readonly DependencyProperty MyAnchorThumbSizeProperty =
            DependencyProperty.Register(nameof(MyAnchorThumbSize), typeof(double), typeof(ShapeSizeGridThumb2),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Visibility MyVisibleAnchorThumb
        {
            get { return (Visibility)GetValue(MyVisibleAnchorThumbProperty); }
            set { SetValue(MyVisibleAnchorThumbProperty, value); }
        }
        public static readonly DependencyProperty MyVisibleAnchorThumbProperty =
            DependencyProperty.Register(nameof(MyVisibleAnchorThumb), typeof(Visibility), typeof(ShapeSizeGridThumb2),
                new FrameworkPropertyMetadata(Visibility.Collapsed,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        //public Rect MyBounts
        //{
        //    get { return (Rect)GetValue(MyBountsProperty); }
        //    set { SetValue(MyBountsProperty, value); }
        //}
        //public static readonly DependencyProperty MyBountsProperty =
        //    DependencyProperty.Register(nameof(MyBounts), typeof(Rect), typeof(GeoShapeThumb),
        //        new FrameworkPropertyMetadata(new Rect(),
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ

        public Canvas MyTemplateCanvas { get; private set; }

        public ObservableCollection<Thumb> MyAnchorThumbs { get; private set; } = new();

        #region コンストラクタ

        public ShapeSizeGridThumb2()
        {
            MyTemplateCanvas = SetMyTemplate<Canvas>();
            MyGeoShape = new();
            MyTemplateCanvas.Children.Add(MyGeoShape);
            MyTemplateCanvas.Background = new SolidColorBrush(Color.FromArgb(10, 0, 0, 255));

            DragDelta += GeoShapeThumb_DragDelta;
            Loaded += GeoShapeThumb_Loaded;
            SetMyBindings();
        }
        #endregion コンストラクタ
        #region privateメソッド
        private T SetMyTemplate<T>()
        {
            FrameworkElementFactory factory = new(typeof(T), "nemo");
            this.Template = new ControlTemplate() { VisualTree = factory };
            ApplyTemplate();
            return (T)this.Template.FindName("nemo", this);
        }

        private void GeoShapeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            //Anchorの数だけAnchorThumbを追加
            for (int i = 0; i < MyAnchorPoints.Count; i++)
            {
                AddAnchorThumb(MyAnchorPoints[i]);
            }
        }

        private void AddAnchorThumb(Point pp)
        {
            Thumb t = new() { Opacity = 0.5 };
            t.SetBinding(VisibilityProperty, new Binding() { Source = this, Path = new PropertyPath(MyVisibleAnchorThumbProperty) });
            t.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(MyAnchorThumbSizeProperty) });
            t.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(MyAnchorThumbSizeProperty) });

            MyAnchorThumbs.Add(t);
            MyTemplateCanvas.Children.Add(t);
            Canvas.SetLeft(t, pp.X);
            Canvas.SetTop(t, pp.Y);
            t.DragDelta += TT_DragDelta;
        }

        private void TT_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                int ii = MyAnchorThumbs.IndexOf(t);
                double x = Canvas.GetLeft(t) + e.HorizontalChange;
                double y = Canvas.GetTop(t) + e.VerticalChange;
                MyAnchorPoints[ii] = new Point(x, y);
                Rect pointsRect = GetPointsRect(MyAnchorPoints);
                //全体(Thumb自身)が移動になる状況は
                //PointsRect座標が0,0以外になる場合、このときは
                //Pointsをオフセット＋自身を移動
                if (pointsRect.Top != 0 || pointsRect.Left != 0)
                {
                    for (int i = 0; i < MyAnchorPoints.Count; i++)
                    {
                        Point pp = MyAnchorPoints[i];
                        SetLocatePointAndAnchorThumb(i, pp.X - pointsRect.X, pp.Y - pointsRect.Y);
                    }
                    this.X += pointsRect.X;
                    this.Y += pointsRect.Y;
                }
                else
                {
                    Canvas.SetLeft(t, x);
                    Canvas.SetTop(t, y);
                }
            }
        }


        private void SetMyBindings()
        {

            //Shapeと自身のバインド
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

            //自身の座標
            SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Mode = BindingMode.TwoWay, Path = new PropertyPath(XProperty) });
            SetBinding(Canvas.TopProperty, new Binding() { Source = this, Mode = BindingMode.TwoWay, Path = new PropertyPath(YProperty) });

            //自身のサイズをShapeのサイズにバインド
            SetBinding(WidthProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShapeSize.MyBoundsProperty), Converter = new MyConverterBounds2Width() });
            SetBinding(HeightProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShapeSize.MyBoundsProperty), Converter = new MyConverterBounds2Height() });

        }

        private void GeoShapeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (e.OriginalSource == e.Source)
            {
                X += e.HorizontalChange;
                Y += e.VerticalChange;
            }
        }
        private void SetLocatePointAndAnchorThumb(int idx, double x, double y)
        {
            MyAnchorPoints[idx] = new Point(x, y);
            Canvas.SetLeft(MyAnchorThumbs[idx], x);
            Canvas.SetTop(MyAnchorThumbs[idx], y);
        }

        /// <summary>
        /// PointCollectionのPointがすべて収まるRect取得
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>        
        private static Rect GetPointsRect(IEnumerable<Point> points)
        {
            double minx = double.MaxValue;
            double miny = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            foreach (Point pp in points)
            {
                if (pp.X < minx) minx = pp.X;
                if (pp.Y < miny) miny = pp.Y;
                if (pp.X > maxX) maxX = pp.X;
                if (pp.Y > maxY) maxY = pp.Y;
            }
            return new Rect(minx, miny, maxX, maxY);
        }


        #endregion privateメソッド
        #region publicメソッド
        public void AddAnchorPoint(Point pp)
        {
            MyAnchorPoints.Add(pp);
            AddAnchorThumb(pp);
        }

        #endregion publicメソッド
    }

    /// <summary>
    /// Shapeを持つCanvas
    /// アンカー点のThumbは無し
    /// </summary>
    public class ShapeSizeCanvasWithoutAnchorThumb : Canvas
    {
        #region 依存関係プロパティ

        public PointCollection MyAnchorPoints
        {
            get { return (PointCollection)GetValue(MyAnchorPointsProperty); }
            set { SetValue(MyAnchorPointsProperty, value); }
        }
        public static readonly DependencyProperty MyAnchorPointsProperty =
            DependencyProperty.Register(nameof(MyAnchorPoints), typeof(PointCollection), typeof(ShapeSizeCanvasWithoutAnchorThumb),
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
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(ShapeSizeCanvasWithoutAnchorThumb),
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
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(ShapeSizeCanvasWithoutAnchorThumb),
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
            DependencyProperty.Register(nameof(MyGeoShape), typeof(GeoShapeSize), typeof(ShapeSizeCanvasWithoutAnchorThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ

        public ShapeSizeCanvasWithoutAnchorThumb()
        {
            this.Background = Brushes.AliceBlue;
            MyGeoShape = new();
            Children.Add(MyGeoShape);
            SetMyBindings();
        }

        private void SetMyBindings()
        {
            MyGeoShape.SetBinding(GeoShapeSize.AnchorPointsProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MyAnchorPointsProperty)
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
    /// Shapeを持つCanvas
    /// アンカー点のThumbも表示
    /// </summary>
    public class ShapeSizeCanvas : Canvas
    {
        #region 依存関係プロパティ
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(ShapeSizeCanvas),
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
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(ShapeSizeCanvas),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public PointCollection MyAnchorPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyAnchorPoints), typeof(PointCollection), typeof(ShapeSizeCanvas),
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
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(ShapeSizeCanvas),
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
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(ShapeSizeCanvas),
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
            DependencyProperty.Register(nameof(MyGeoShape), typeof(GeoShapeSize), typeof(ShapeSizeCanvas),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ
        public ObservableCollection<Thumb> MyAnchorThumbs { get; private set; } = new();

        public ShapeSizeCanvas()
        {
            this.Background = Brushes.AliceBlue;
            MyGeoShape = new();
            Children.Add(MyGeoShape);
            SetMyBindings();
            Loaded += ShapeSizeCanvas_Loaded;
        }

        private void ShapeSizeCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < MyAnchorPoints.Count; i++)
            {
                Thumb tt = new() { Width = 20, Height = 20 };
                MyAnchorThumbs.Add(tt);
                Children.Add(tt);
                Canvas.SetLeft(tt, MyAnchorPoints[i].X);
                Canvas.SetTop(tt, MyAnchorPoints[i].Y);
                tt.DragDelta += Tt_DragDelta;
            }
        }

        private void Tt_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                int ii = MyAnchorThumbs.IndexOf(t);
                double x = Canvas.GetLeft(t) + e.HorizontalChange;
                double y = Canvas.GetTop(t) + e.VerticalChange;
                MyAnchorPoints[ii] = new Point(x, y);
                Rect pointsRect = GetPointsRect(MyAnchorPoints);
                //全体(自身)が移動になる状況は
                //PointsRect座標が0,0以外になる場合、このときは
                //Pointsをオフセット＋自身を移動
                if (pointsRect.Top != 0 || pointsRect.Left != 0)
                {
                    for (int i = 0; i < MyAnchorPoints.Count; i++)
                    {
                        Point pp = MyAnchorPoints[i];
                        SetLocatePointAndAnchorThumb(i, pp.X - pointsRect.X, pp.Y - pointsRect.Y);
                    }
                    this.X += pointsRect.X;
                    this.Y += pointsRect.Y;
                }
                else
                {
                    Canvas.SetLeft(t, x);
                    Canvas.SetTop(t, y);
                }
            }
        }

        /// <summary>
        /// PointCollectionのPointがすべて収まるRect取得
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>        
        private static Rect GetPointsRect(IEnumerable<Point> points)
        {
            double minx = double.MaxValue;
            double miny = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            foreach (Point pp in points)
            {
                if (pp.X < minx) minx = pp.X;
                if (pp.Y < miny) miny = pp.Y;
                if (pp.X > maxX) maxX = pp.X;
                if (pp.Y > maxY) maxY = pp.Y;
            }
            return new Rect(minx, miny, maxX, maxY);
        }

        private void SetLocatePointAndAnchorThumb(int idx, double x, double y)
        {
            MyAnchorPoints[idx] = new Point(x, y);
            Canvas.SetLeft(MyAnchorThumbs[idx], x);
            Canvas.SetTop(MyAnchorThumbs[idx], y);
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

            //自身の座標
            SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Mode = BindingMode.TwoWay, Path = new PropertyPath(XProperty) });
            SetBinding(Canvas.TopProperty, new Binding() { Source = this, Mode = BindingMode.TwoWay, Path = new PropertyPath(YProperty) });

            //自身のサイズをShapeのサイズにバインド
            SetBinding(WidthProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShapeSize.MyBoundsProperty), Converter = new MyConverterBounds2Width() });
            SetBinding(HeightProperty, new Binding() { Source = MyGeoShape, Path = new PropertyPath(GeoShapeSize.MyBoundsProperty), Converter = new MyConverterBounds2Height() });

        }

    }

    /// <summary>
    /// CanvasベースのMoveThumbにShapeSize表示
    /// Thumb自身のサイズをShapeのMyBoundsにバインド
    /// Shapeの表示座標をMyBoundsを元にオフセット
    /// AnchorにThumbを表示、マウスでAnchor移動で図形変化、自身のサイズも変化
    /// Thumb
    ///     ┗Canvas(Template)
    ///         ┗Shape
    /// </summary>
    public class ShapeSizeCanvasThumb2 : ShapeSizeCanvasThumb
    {

        public ObservableCollection<Thumb> AnchorThumbs { get; private set; } = new();
        public ShapeSizeCanvasThumb2()
        {
            Loaded += ShapeSizeCanvasThumb2_Loaded;
        }

        private void ShapeSizeCanvasThumb2_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < MyAnchorPoints.Count; i++)
            {
                Thumb t = new() { Width = 20, Height = 20 };
                AnchorThumbs.Add(t);
                MyTemplate.Children.Add(t);
                Canvas.SetLeft(t, MyAnchorPoints[i].X);
                Canvas.SetTop(t, MyAnchorPoints[i].Y);
                t.DragDelta += TT_DragDelta;
            }
        }

        private void TT_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                int ii = AnchorThumbs.IndexOf(t);
                double x = Canvas.GetLeft(t) + e.HorizontalChange;
                double y = Canvas.GetTop(t) + e.VerticalChange;
                MyAnchorPoints[ii] = new Point(x, y);
                Rect pointsRect = GetPointsRect();
                //全体(Thumb自身)が移動になる状況は
                //PointsRect座標が0,0以外になる場合、このときは
                //Pointsをオフセット＋自身を移動
                if (pointsRect.Top != 0 || pointsRect.Left != 0)
                {
                    for (int i = 0; i < MyAnchorPoints.Count; i++)
                    {
                        Point pp = MyAnchorPoints[i];
                        SetLocatePointAndAnchorThumb(i, pp.X - pointsRect.X, pp.Y - pointsRect.Y);
                    }
                    this.X += pointsRect.X;
                    this.Y += pointsRect.Y;
                }
                else
                {
                    Canvas.SetLeft(t, x);
                    Canvas.SetTop(t, y);
                }

            }
        }

        private Rect GetPointsRect()
        {
            double minx = double.MaxValue;
            double miny = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            for (int i = 0; i < MyAnchorPoints.Count; i++)
            {
                Point pp = MyAnchorPoints[i];
                if (pp.X < minx) minx = pp.X;
                if (pp.Y < miny) miny = pp.Y;
                if (pp.X > maxX) maxX = pp.X;
                if (pp.Y > maxY) maxY = pp.Y;
            }
            return new Rect(minx, miny, maxX, maxY);
        }

        private void SetLocatePointAndAnchorThumb(int idx, double x, double y)
        {
            MyAnchorPoints[idx] = new Point(x, y);
            Canvas.SetLeft(AnchorThumbs[idx], x);
            Canvas.SetTop(AnchorThumbs[idx], y);
        }
    }

    /// <summary>
    /// CanvasベースのMoveThumbにShapeSize表示
    /// Thumb自身のサイズをShapeのMyBoundsにバインド
    /// Shapeの表示座標をMyBoundsを元にオフセット
    /// </summary>
    public class ShapeSizeCanvasThumb : CanvasThumb
    {
        #region 依存関係プロパティ

        public PointCollection MyAnchorPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyAnchorPoints), typeof(PointCollection), typeof(ShapeSizeCanvasThumb),
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

        #endregion 依存関係プロパティ

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
        public Canvas MyTemplate { get; set; }
        public CanvasThumb()
        {
            MyTemplate = SetMyTemplate<Canvas>();
        }

        private T SetMyTemplate<T>()
        {
            FrameworkElementFactory factory = new(typeof(T), "nemo");
            this.Template = new ControlTemplate() { VisualTree = factory };
            ApplyTemplate();
            return (T)this.Template.FindName("nemo", this);
        }
    }

    public class GridThumb : MoveThumb
    {
        public Grid MyTemplate { get; set; }
        public GridThumb()
        {
            MyTemplate = SetMyTemplate<Grid>();
        }

        private T SetMyTemplate<T>()
        {
            FrameworkElementFactory factory = new(typeof(T), "nemo");
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
