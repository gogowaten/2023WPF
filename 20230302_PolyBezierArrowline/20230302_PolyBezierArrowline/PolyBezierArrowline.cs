using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

//WPFで矢印ベジェ曲線できた - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2023/03/03/131036

namespace _20230302_PolyBezierArrowline
{
    //端点の形状
    public enum EdgeType { None = 0, Arrow, }


    public class PolyBezierArrowline : Shape
    {
        #region 依存プロパティ

        public bool IsBezier
        {
            get { return (bool)GetValue(IsBezierProperty); }
            set { SetValue(IsBezierProperty, value); }
        }
        public static readonly DependencyProperty IsBezierProperty =
            DependencyProperty.Register(nameof(IsBezier), typeof(bool), typeof(PolyBezierArrowline),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 終点の図形形状(エッジタイプ)
        /// </summary>
        public EdgeType EdgeEndType
        {
            get { return (EdgeType)GetValue(EdgeEndTypeProperty); }
            set { SetValue(EdgeEndTypeProperty, value); }
        }
        public static readonly DependencyProperty EdgeEndTypeProperty =
            DependencyProperty.Register(nameof(EdgeEndType), typeof(EdgeType), typeof(PolyBezierArrowline),
                new FrameworkPropertyMetadata(EdgeType.None,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 始点の図形形状(エッジタイプ)
        /// </summary>
        public EdgeType EdgeBeginType
        {
            get { return (EdgeType)GetValue(EdgeBeginTypeProperty); }
            set { SetValue(EdgeBeginTypeProperty, value); }
        }
        public static readonly DependencyProperty EdgeBeginTypeProperty =
            DependencyProperty.Register(nameof(EdgeBeginType), typeof(EdgeType), typeof(PolyBezierArrowline),
                new FrameworkPropertyMetadata(EdgeType.None,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 矢印角度、初期値は30.0にしている。30～40くらいが適当
        /// </summary>
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register(nameof(Angle), typeof(double), typeof(PolyBezierArrowline),
                new FrameworkPropertyMetadata(30.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //[TypeConverter(typeof(MyTypeConverterPoints))]
        //public ObservableCollection<Point> Points
        //{
        //    get { return (ObservableCollection<Point>)GetValue(PointsProperty); }
        //    set { SetValue(PointsProperty, value); }
        //}
        //public static readonly DependencyProperty PointsProperty =
        //    DependencyProperty.Register(nameof(Points), typeof(ObservableCollection<Point>), typeof(PolyBezierArrowLine2),
        //        new FrameworkPropertyMetadata(new ObservableCollection<Point>() { new Point(0, 0), new Point(100, 100) },
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure));

        public PointCollection Points
        {
            get { return (PointCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(PolyBezierArrowline),
                new FrameworkPropertyMetadata(new PointCollection() { new Point(0, 0), new Point(100, 100) },
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion 依存プロパティ
        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new() { FillRule = FillRule.Nonzero };
                if (Points.Count < 2) { return geometry; }
                if (Points.Count < 4 && IsBezier) { return geometry; }
                //図形はFillで描画して、中間線部分はStrokeで描画している
                //両方同じ色にするのでStrokeで統一
                Fill = Stroke;
                using (var context = geometry.Open())
                {
                    Point begin = Points[0];//始点
                    Point end = Points[^1];//終点
                    //始点図形の描画
                    switch (EdgeBeginType)
                    {
                        case EdgeType.None:
                            break;
                        case EdgeType.Arrow:
                            begin = DrawArrow(context, Points[0], Points[1]);
                            break;
                    }
                    //終点図形の描画
                    switch (EdgeEndType)
                    {
                        case EdgeType.None:
                            break;
                        case EdgeType.Arrow:
                            end = DrawArrow(context, Points[^1], Points[^2]);
                            break;
                    }
                    //中間線の描画
                    if (IsBezier) { DrawBezier(context, begin, end); }
                    else { DrawLine(context, begin, end); }
                }
                geometry.Freeze();
                return geometry;
            }
        }



        /// <summary>
        /// ベジェ曲線部分の描画
        /// </summary>
        /// <param name="context"></param>
        /// <param name="begin">始点図形との接点</param>
        /// <param name="end">終点図形との接点</param>
        private void DrawBezier(StreamGeometryContext context, Point begin, Point end)
        {
            context.BeginFigure(begin, false, false);
            List<Point> bezier = Points.Skip(1).Take(Points.Count - 2).ToList();
            bezier.Add(end);
            context.PolyBezierTo(bezier, true, false);
        }

        /// <summary>
        /// 直線部分の描画
        /// </summary>
        /// <param name="context"></param>
        /// <param name="begin">始点図形との接点</param>
        /// <param name="end">終点図形との接点</param>
        private void DrawLine(StreamGeometryContext context, Point begin, Point end)
        {
            context.BeginFigure(begin, false, false);
            context.PolyLineTo(Points.Skip(1).Take(Points.Count - 2).ToList(), true, false);
            context.LineTo(end, true, false);
        }

        /// <summary>
        /// アローヘッド(三角形)描画
        /// </summary>
        /// <param name="context"></param>
        /// <param name="edge">端のPoint、始点ならPoints[0]、終点ならPoints[^1]</param>
        /// <param name="next">端から2番めのPoint、始点ならPoints[1]、終点ならPoints[^2]</param>
        /// <returns></returns>
        private Point DrawArrow(StreamGeometryContext context, Point edge, Point next)
        {
            //斜辺 hypotenuse ここでは二等辺三角形の底辺じゃない方の2辺
            //頂角 apex angle 先端の角
            //アローヘッドの斜辺(hypotenuse)の角度(ラジアン)を計算
            double lineRadian = Math.Atan2(next.Y - edge.Y, next.X - edge.X);
            double apexRadian = DegreeToRadian(Angle);
            double edgeSize = StrokeThickness * 2.0;
            double hypotenuseLength = edgeSize / Math.Cos(apexRadian);
            double hypotenuseRadian1 = lineRadian + apexRadian;

            //底角座標
            Point p1 = new(
                hypotenuseLength * Math.Cos(hypotenuseRadian1) + edge.X,
                hypotenuseLength * Math.Sin(hypotenuseRadian1) + edge.Y);

            double hypotenuseRadian2 = lineRadian - DegreeToRadian(Angle);
            Point p2 = new(
                hypotenuseLength * Math.Cos(hypotenuseRadian2) + edge.X,
                hypotenuseLength * Math.Sin(hypotenuseRadian2) + edge.Y);

            //アローヘッド描画、Fill(塗りつぶし)で描画
            context.BeginFigure(edge, true, false);//isFilled, isClose
            context.LineTo(p1, false, false);//isStroke, isSmoothJoin
            context.LineTo(p2, false, false);

            //アローヘッドと中間線の接点座標計算、
            //HeadSizeぴったりで計算すると僅かな隙間ができるので-1.0している、
            //-0.5でも隙間になる、-0.7で隙間なくなる
            return new Point(
                (edgeSize - 1.0) * Math.Cos(lineRadian) + edge.X,
                (edgeSize - 1.0) * Math.Sin(lineRadian) + edge.Y);
        }

        //DrawArrowLineに統合したので未使用
        /// <summary>
        /// 始点にアローヘッド描画
        /// </summary>
        /// <param name="context"></param>
        /// <returns>アローヘッドと直線との接点座標</returns>
        private Point DrawBeginArrow(StreamGeometryContext context)
        {
            double x0 = Points[0].X;
            double y0 = Points[0].Y;
            double x1 = Points[1].X;
            double y1 = Points[1].Y;

            double lineRadian = Math.Atan2(y1 - y0, x1 - x0);
            double arrowRadian = DegreeToRadian(Angle);
            double headSize = StrokeThickness * 2.0;
            double wingLength = headSize / Math.Cos(arrowRadian);

            double wingRadian1 = lineRadian + arrowRadian;
            Point arrowP1 = new(
                wingLength * Math.Cos(wingRadian1) + x0,
                wingLength * Math.Sin(wingRadian1) + y0);
            double wingRadian2 = lineRadian - DegreeToRadian(Angle);
            Point arrowP2 = new(
                wingLength * Math.Cos(wingRadian2) + x0,
                wingLength * Math.Sin(wingRadian2) + y0);
            //アローヘッド描画、Fill(塗りつぶし)で描画
            context.BeginFigure(Points[0], true, false);//fill, close
            context.LineTo(arrowP1, false, false);//isStroke, isSmoothJoin
            context.LineTo(arrowP2, false, false);

            //アローヘッドと直線との接点座標、
            //HeadSizeぴったりで計算すると僅かな隙間ができるので-1.0している、
            //-0.5でも隙間になる、-0.7で隙間なくなる
            return new Point(
                (headSize - 1.0) * Math.Cos(lineRadian) + x0,
                (headSize - 1.0) * Math.Sin(lineRadian) + y0);
        }

        //DrawArrowLineに統合したので未使用
        /// <summary>
        /// 終点にアローヘッド描画
        /// </summary>
        /// <param name="context"></param>
        /// <returns>アローヘッドと直線との接点座標</returns>
        private Point DrawEndArrow(StreamGeometryContext context)
        {
            double x0 = Points[^1].X;
            double x1 = Points[^2].X;
            double y0 = Points[^1].Y;
            double y1 = Points[^2].Y;

            double lineRadian = Math.Atan2(y1 - y0, x1 - x0);
            double arrowRadian = DegreeToRadian(Angle);
            double headSize = StrokeThickness * 2.0;
            double wingLength = headSize / Math.Cos(arrowRadian);

            double wingRadian1 = lineRadian + arrowRadian;
            Point arrowP1 = new(
                wingLength * Math.Cos(wingRadian1) + x0,
                wingLength * Math.Sin(wingRadian1) + y0);
            double wingRadian2 = lineRadian - DegreeToRadian(Angle);
            Point arrowP2 = new(
                wingLength * Math.Cos(wingRadian2) + x0,
                wingLength * Math.Sin(wingRadian2) + y0);
            //アローヘッド描画、Fill(塗りつぶし)で描画
            context.BeginFigure(Points[^1], true, false);//fill, close
            context.LineTo(arrowP1, false, false);//isStroke, isSmoothJoin
            context.LineTo(arrowP2, false, false);

            return new Point(
                (headSize - 1.0) * Math.Cos(lineRadian) + x0,
                (headSize - 1.0) * Math.Sin(lineRadian) + y0);
        }
        public static double DegreeToRadian(double degree)
        {
            return degree / 360.0 * (Math.PI * 2.0);
        }
    }

    //未使用
    /// <summary>
    /// XAMLの文字列をObservableCollectionに変換
    /// </summary>
    public class MyTypeConverterPoints : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value == null) return null;
            if (value is string str)
            {
                string[] ss = str.Split(' ');
                ObservableCollection<Point> points = new();
                foreach (var item in ss)
                {
                    string[] xy = item.Split(',');
                    if (double.TryParse(xy[0], out double x) && double.TryParse(xy[1], out double y))
                    {
                        Point point = new(x, y);
                        points.Add(point);
                    }
                }
                return points;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
