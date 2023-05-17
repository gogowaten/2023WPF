using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace _20230517_PolyGeoShapeThumb
{
    /// <summary>
    /// CanvasベースのMoveThumbにShapeSize表示
    /// Thumb自身のサイズをShapeのMyBoundsにバインド
    /// Shapeの表示座標をMyBoundsを元にオフセット
    /// AnchorにThumbを表示、マウスでAnchor移動で図形変化、自身のサイズも変化
    /// </summary>
    class GeoShapeThumb : Thumb
    {
        #region 依存関係プロパティ

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(GeoShapeThumb),
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
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(GeoShapeThumb),
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
            DependencyProperty.Register(nameof(MyAnchorPoints), typeof(PointCollection), typeof(GeoShapeThumb),
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
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(GeoShapeThumb),
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
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(GeoShapeThumb),
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
            DependencyProperty.Register(nameof(MyGeoShape), typeof(GeoShapeSize), typeof(GeoShapeThumb),
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
            DependencyProperty.Register(nameof(MyAnchorThumbSize), typeof(double), typeof(GeoShapeThumb),
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
            DependencyProperty.Register(nameof(MyVisibleAnchorThumb), typeof(Visibility), typeof(GeoShapeThumb),
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

        public GeoShapeThumb()
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

    #region コンバーター
    
    public class MyConverterBounds2LeftOffset : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect r = (Rect)value;
            return -r.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterBounds2TopOffset : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect r = (Rect)value;
            return -r.Top;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterBounds2Width : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect r = (Rect)value;
            return r.Width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterBounds2Height : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect r = (Rect)value;
            return r.Height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion コンバーター
}
