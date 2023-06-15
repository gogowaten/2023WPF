using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using System.Runtime.CompilerServices;


namespace _20230520
{

    /// <summary>
    /// 直線図形
    /// </summary>
    public class GeoPolyLineShape : Shape
    {
        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeoPolyLineShape),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 図形自体のRect、読み取り専用
        /// </summary>
        public Rect MyRenderRect
        {
            get { return (Rect)GetValue(MyRenderRectProperty); }
            set { SetValue(MyRenderRectProperty, value); }
        }
        public static readonly DependencyProperty MyRenderRectProperty =
            DependencyProperty.Register(nameof(MyRenderRect), typeof(Rect), typeof(GeoPolyLineShape),
                new FrameworkPropertyMetadata(new Rect()));


        //public Rect MyRenderRect
        //{
        //    get { return (Rect)GetValue(MyRenderRectProperty); }
        //    set { SetValue(MyRenderRectProperty, value); }
        //}
        //public static readonly DependencyProperty MyRenderRectProperty =
        //    DependencyProperty.Register(nameof(MyRenderRect), typeof(Rect), typeof(GeoPolyLineShape),
        //        new FrameworkPropertyMetadata(Rect.Empty,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        /// <summary>
        /// 線の太さを考慮した図形のPathGeometry。読み取り専用
        /// </summary>
        public PathGeometry MyGeometry
        {
            get { return (PathGeometry)GetValue(MyGeometryProperty); }
            set { SetValue(MyGeometryProperty, value); }
        }
        public static readonly DependencyProperty MyGeometryProperty =
            DependencyProperty.Register(nameof(MyGeometry), typeof(PathGeometry), typeof(GeoPolyLineShape),
                new FrameworkPropertyMetadata(null));


        //public PathGeometry MyGeometry
        //{
        //    get { return (PathGeometry)GetValue(MyGeometryProperty); }
        //    set { SetValue(MyGeometryProperty, value); }
        //}
        //public static readonly DependencyProperty MyGeometryProperty =
        //    DependencyProperty.Register(nameof(MyGeometry), typeof(PathGeometry), typeof(GeoPolyLineShape),
        //        new FrameworkPropertyMetadata(null,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(GeoPolyLineShape),
                new FrameworkPropertyMetadata(new Pen(Brushes.Red, 10.0),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存関係プロパティ

        public GeoPolyLineShape()
        {
            //MyPenにStrokeとStrokeThicknessをバインド
            MultiBinding mb = new() { Converter = new MyConverterPen() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeThicknessProperty) });
            SetBinding(MyPenProperty, mb);

            //MyRenderBoundsにMyGeometryとRenderTransformをバインド
            SetBinding(MyRenderRectProperty, new Binding() { Source = this, Path = new PropertyPath(MyGeometryProperty), Converter = new MyConverterGeometry2Bounds2() });

        }


        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints.Count <= 1) return Geometry.Empty;

                StreamGeometry geometry = new();
                using (var content = geometry.Open())
                {
                    content.BeginFigure(MyPoints[0], false, false);
                    content.PolyLineTo(MyPoints.Skip(1).ToList(), true, false);

                }
                geometry.Freeze();
                //Bounds計算用のGeometryを更新、StrokeThicknessを考慮したGeometry
                MyGeometry = geometry.GetWidenedPathGeometry(MyPen);

                return geometry;
            }
        }



    }



    /// <summary>
    /// GeoShapeの改変 
    /// MyRenderBoundsの更新を自動(Binding)にした
    /// ArrangeOverride時の更新が不要になった
    /// Loaded時の更新が不要になった
    /// かなりスッキリしたけど、MyRenderBoundsが読み取り専用じゃなくなってしまった
    /// </summary>
    public class GeoShape2 : Shape
    {
        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeoShape2),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Rect MyRenderBounds
        {
            get { return (Rect)GetValue(MyRenderBoundsProperty); }
            set { SetValue(MyRenderBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyRenderBoundsProperty =
            DependencyProperty.Register(nameof(MyRenderBounds), typeof(Rect), typeof(GeoShape2),
                new FrameworkPropertyMetadata(new Rect()));

        public PathGeometry MyGeometry
        {
            get { return (PathGeometry)GetValue(MyGeometryProperty); }
            set { SetValue(MyGeometryProperty, value); }
        }
        public static readonly DependencyProperty MyGeometryProperty =
            DependencyProperty.Register(nameof(MyGeometry), typeof(PathGeometry), typeof(GeoShape2),
                new FrameworkPropertyMetadata(null));


        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(GeoShape2),
                new FrameworkPropertyMetadata(new Pen(Brushes.Red, 10.0),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存関係プロパティ

        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints.Count <= 1) return Geometry.Empty;

                StreamGeometry geometry = new();
                using (var content = geometry.Open())
                {
                    content.BeginFigure(MyPoints[0], false, false);
                    content.PolyLineTo(MyPoints.Skip(1).ToList(), true, false);

                }
                geometry.Freeze();
                //Bounds計算用のGeometryを更新、StrokeThicknessを考慮したGeometry
                MyGeometry = geometry.GetWidenedPathGeometry(MyPen);

                return geometry;
            }
        }

        public GeoShape2()
        {

            //MyPenにStrokeとStrokeThicknessをバインド
            MultiBinding mb = new() { Converter = new MyConverterPen() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeThicknessProperty) });
            SetBinding(MyPenProperty, mb);

            //MyRenderBoundsにMyGeometryとRenderTransformをバインド
            mb = new() { Converter = new MyConverterGeometry2Bounds(), Mode = BindingMode.OneWay };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyGeometryProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(RenderTransformProperty) });
            SetBinding(MyRenderBoundsProperty, mb);

        }

    }




    public class GeoShape : Shape
    {

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeoShape),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        //読み取り専用の依存関係プロパティ
        //WPF4.5入門 その43 「読み取り専用の依存関係プロパティ」 - かずきのBlog@hatena
        //        https://blog.okazuki.jp/entry/2014/08/18/083455

        public Rect MyRenderBounds
        {
            get { return (Rect)GetValue(MyRenderBoundsProperty); }
            set { SetValue(MyRenderBoundsPropertyKey, value); }
        }
        public static readonly DependencyPropertyKey MyRenderBoundsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyRenderBounds), typeof(Rect), typeof(GeoShape),
                new FrameworkPropertyMetadata(new Rect(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty MyRenderBoundsProperty =
            MyRenderBoundsPropertyKey.DependencyProperty;

        //回転角度。RenderTransformとバインドすると回転角度変更時に
        //ArrangeOverrideが発生するようになる
        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(GeoShape),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints.Count <= 1) return Geometry.Empty;

                StreamGeometry geometry = new();
                using (var content = geometry.Open())
                {
                    content.BeginFigure(MyPoints[0], false, false);
                    content.PolyLineTo(MyPoints.Skip(1).ToList(), true, false);

                }
                geometry.Freeze();
                return geometry;
            }
        }

        public GeoShape()
        {
            Loaded += GeoShape_Loaded;

            //回転角度のバインド。角度変更でArrangeOverrideを発生させるため
            SetBinding(MyAngleProperty, new Binding() { Source = this, Path = new PropertyPath(RenderTransformProperty), Converter = new MyConverterRenderTransform2Angle() });


        }

        private void GeoShape_Loaded(object sender, RoutedEventArgs e)
        {
            UpdataRenderBounds();
        }

        //Transform変更(回転)時にも実行したいけど、ArrangeOverrideでは反応しない
        //→なぜかできた、MyAngleとRenderTransformをバインドしたらなぜかArrangeOverrideが発動
        protected override Size ArrangeOverride(Size finalSize)
        {
            UpdataRenderBounds();
            return base.ArrangeOverride(finalSize);
        }

        //PropertyChangedならTransformに反応するけど、大掛かりすぎる
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            //if (e.Property.PropertyType == typeof(Transform))
            //{
            //    UpdataRenderBounds();
            //}
            base.OnPropertyChanged(e);
        }


        /// <summary>
        /// 図形の描画のRectを更新、StrokeThicknessを考慮したもの
        /// </summary>
        private void UpdataRenderBounds()
        {
            //GetDescendantBoundsだとMyPoints変更時に正しい値が取れない
            //var bb = VisualTreeHelper.GetDescendantBounds(this);
            //if (bb.IsEmpty) return;


            Pen pen = new(Stroke, StrokeThickness);
            Geometry geo = DefiningGeometry.Clone();
            geo.Transform = RenderTransform;//変形時には必要？
            //PathGeometry widenGeo = geo.GetWidenedPathGeometry(pen);
            //Rect widenRenderRect = widenGeo.GetRenderBounds(null);
            //MyRenderBounds = widenRenderRect;

            //これだけでいい？
            var renderB = geo.GetRenderBounds(pen);
            MyRenderBounds = renderB;
        }
    }


    public class MyConverterPen : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Brush b = (Brush)values[0];
            double thickness = (double)values[1];
            return new Pen(b, thickness);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            Pen p = (Pen)value;
            object[] objects = new object[2];
            objects[0] = p.Brush;
            objects[1] = p.Thickness;
            return objects;
        }
    }

    /// <summary>
    /// PathGeometryをRectに変換、Transform無考慮
    /// </summary>
    public class MyConverterGeometry2Bounds2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PathGeometry geo = (PathGeometry)value;
            if (geo == null) return new Rect();
            return geo.GetRenderBounds(null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterGeometry2Bounds : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            PathGeometry geo = (PathGeometry)values[0];
            if (geo == null) { return new Rect(); }
            Transform tf = (Transform)values[1];

            geo.Transform = tf;
            Rect r = geo.GetRenderBounds(null);
            return r;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// RenderTransformの回転角度(RotateTransform)を取り出す
    /// </summary>
    public class MyConverterRenderTransform2Angle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Transform tf = (Transform)value;
            if (tf is TransformGroup tfg)
            {
                foreach (var item in tfg.Children)
                {
                    if (item is RotateTransform rotate)
                    {
                        return rotate.Angle;
                    }
                }

            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double angel = (double)value;
            TransformGroup tfg = new();
            tfg.Children.Add(new RotateTransform(angel));
            return tfg;
        }
    }
}
