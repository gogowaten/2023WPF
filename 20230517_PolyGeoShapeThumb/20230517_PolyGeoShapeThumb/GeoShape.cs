using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace _20230517_PolyGeoShapeThumb
{
    public class GeoShapeSize : Shape
    {
        public GeoShapeSize()
        {

        }

        public PointCollection AnchorPoints
        {
            get { return (PointCollection)GetValue(AnchorPointsProperty); }
            set { SetValue(AnchorPointsProperty, value); }
        }
        public static readonly DependencyProperty AnchorPointsProperty =
            DependencyProperty.Register(nameof(AnchorPoints), typeof(PointCollection), typeof(GeoShapeSize),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Rect MyBounds
        {
            get { return (Rect)GetValue(MyBoundsProperty); }
            set { SetValue(MyBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyBoundsProperty =
            DependencyProperty.Register(nameof(MyBounds), typeof(Rect), typeof(GeoShapeSize),
                new FrameworkPropertyMetadata(new Rect()));

        protected override Size MeasureOverride(Size constraint)
        {
            var geo = this.DefiningGeometry.Clone();
            geo.Transform = RenderTransform;
            var path = geo.GetWidenedPathGeometry(new Pen(Stroke, StrokeThickness));
            Rect bounds = path.Bounds;

            //MyBounds = bounds;
            //Canvas.SetLeft(this, -bounds.Left);
            //Canvas.SetTop(this, -bounds.Top);
            //Width = bounds.Width;
            //Height = bounds.Height;

            //DrawingVisual dv = new() { Offset = new Vector(-r.X, -r.Y) };
            //using (var context = dv.RenderOpen())
            //{
            //    context.DrawGeometry(Stroke, null, path);
            //}

            //return base.MeasureOverride(bounds.Size);//変化なし
            return base.MeasureOverride(constraint);

        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var geo = this.DefiningGeometry.Clone();
            geo.Transform = RenderTransform;
            var path = geo.GetWidenedPathGeometry(new Pen(Stroke, StrokeThickness));
            Rect bounds = path.Bounds;
            MyBounds = bounds;


            //return base.ArrangeOverride(bounds.Size);//サイズはいいけど、位置指定ができていないのでずれる
            return base.ArrangeOverride(finalSize);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                if (AnchorPoints == null || AnchorPoints.Count < 2) return Geometry.Empty;
                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    context.BeginFigure(AnchorPoints[0], false, false);
                    context.PolyLineTo(AnchorPoints.Skip(1).ToList(), true, false);
                }
                geometry.Freeze();
                return geometry;
            }
        }
    }
    public class GeoShape : Shape
    {
        public GeoShape() { }
        public PointCollection AnchorPoints
        {
            get { return (PointCollection)GetValue(AnchorPointsProperty); }
            set { SetValue(AnchorPointsProperty, value); }
        }
        public static readonly DependencyProperty AnchorPointsProperty =
            DependencyProperty.Register(nameof(AnchorPoints), typeof(PointCollection), typeof(GeoShape),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //[TypeConverter(typeof(MyTypeConverterPoints))]
        //public ObservableCollection<Point> AnchorPoints
        //{
        //    get { return (ObservableCollection<Point>)GetValue(AnchorPointsProperty); }
        //    set { SetValue(AnchorPointsProperty, value); }
        //}
        //public static readonly DependencyProperty AnchorPointsProperty =
        //    DependencyProperty.Register(nameof(AnchorPoints), typeof(ObservableCollection<Point>), typeof(GeoShape),
        //        new FrameworkPropertyMetadata(null,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        protected override Geometry DefiningGeometry
        {
            get
            {
                if (AnchorPoints == null || AnchorPoints.Count < 2) return Geometry.Empty;
                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    context.BeginFigure(AnchorPoints[0], false, false);
                    context.PolyLineTo(AnchorPoints.Skip(1).ToList(), true, false);
                }
                geometry.Freeze();
                return geometry;
            }
        }
    }


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
}
