using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Globalization;
using System.Data;

namespace _20230526_PolyLineEx
{
    /// <summary>
    /// 実際の描画位置とサイズを表すRect型プロパティの、MyRenderBoundsを持つPolyLineShape
    /// StrokeThicknessやRenderTransformも反映、ただし
    /// RenderTransformOriginは反映されない、これは親要素が絡むからできない
    /// </summary>
    public class PolyLineExShape : Shape
    {

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(PolyLineExShape),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //本当は外部からは読み取り専用にしたい
        public Rect MyRenderBounds
        {
            get { return (Rect)GetValue(MyRenderBoundsProperty); }
            set { SetValue(MyRenderBoundsProperty, value); }
        }
        public static readonly DependencyProperty MyRenderBoundsProperty =
            DependencyProperty.Register(nameof(MyRenderBounds), typeof(Rect), typeof(PolyLineExShape),
                new FrameworkPropertyMetadata(new Rect()));

        //本当は外部からは読み取り専用にしたい
        public PathGeometry MyGeometry
        {
            get { return (PathGeometry)GetValue(MyGeometryProperty); }
            set { SetValue(MyGeometryProperty, value); }
        }
        public static readonly DependencyProperty MyGeometryProperty =
            DependencyProperty.Register(nameof(MyGeometry), typeof(PathGeometry), typeof(PolyLineExShape),
                new FrameworkPropertyMetadata(null));
        //これ↓だと無限ループになるのでAffectsRenderとAffectsMeasureはいらない
        //  public PathGeometry MyGeometry
        //{
        //    get { return (PathGeometry)GetValue(MyGeometryProperty); }
        //    set { SetValue(MyGeometryProperty, value); }
        //}
        //public static readonly DependencyProperty MyGeometryProperty =
        //    DependencyProperty.Register(nameof(MyGeometry), typeof(PathGeometry), typeof(PolyLineExShape),
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
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(PolyLineExShape),
                new FrameworkPropertyMetadata(new Pen(Brushes.Red, 10.0),
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
                //Bounds計算用のGeometryを更新
                MyGeometry = geometry.GetWidenedPathGeometry(MyPen);

                return geometry;
            }
        }

        public PolyLineExShape()
        {

            //MyPenにStrokeとStrokeThicknessをバインド
            MultiBinding mb = new() { Converter = new MyConverterPen() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeThicknessProperty) });
            SetBinding(MyPenProperty, mb);

            //MyRenderBoundsにMyGeometryとRenderTransformをバインド
            mb = new() { Converter = new MyConverterGeometry2Bounds() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyGeometryProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(RenderTransformProperty) });
            SetBinding(MyRenderBoundsProperty, mb);
            
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

    public class MyConverterGeometry2Bounds : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            PathGeometry geo = (PathGeometry)values[0];
            if (geo == null) { return new Rect(); }
            Transform tf = (Transform)values[1];
            geo.Transform = tf;
            return geo.GetRenderBounds(null);



            ////RenderTransformOriginも含めたBoundsの計算がわからん
            //PathGeometry geoc = geo.Clone();
            //if (tf is TransformGroup group)
            //{
            //    foreach (var item in group.Children)
            //    {
            //        if (item is RotateTransform rotate)
            //        {
            //            //rotate.CenterX = geo.Bounds.Width / 2.0;
            //            //rotate.CenterY = geo.Bounds.Height / 2.0;
            //            geoc.Transform = rotate;
            //            return geoc.GetRenderBounds(null);
            //        }
            //    }
            //}
            //return new Rect();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
