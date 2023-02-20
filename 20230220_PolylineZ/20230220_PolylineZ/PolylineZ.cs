using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

//WPF Arrow and Custom Shapes - CodeProject
//https://www.codeproject.com/Articles/23116/WPF-Arrow-and-Custom-Shapes

//2022WPF/Arrow.cs at master · gogowaten/2022WPF
//https://github.com/gogowaten/2022WPF/blob/master/20221203_%E7%9F%A2%E5%8D%B0%E5%9B%B3%E5%BD%A2/20221203_%E7%9F%A2%E5%8D%B0%E5%9B%B3%E5%BD%A2/Arrow.cs
//E:\オレ\エクセル\WPFでPixtack紫陽花.xlsm_三角関数_$B$95

namespace _20230220_PolylineZ
{
    public enum HeadType { None = 0, Arrow, }
    public class PolylineZ : Shape
    {
        #region 依存プロパティ

        /// <summary>
        /// 終点のヘッドタイプ
        /// </summary>
        public HeadType HeadEndType
        {
            get { return (HeadType)GetValue(HeadEndTypeProperty); }
            set { SetValue(HeadEndTypeProperty, value); }
        }
        public static readonly DependencyProperty HeadEndTypeProperty =
            DependencyProperty.Register(nameof(HeadEndType), typeof(HeadType), typeof(PolylineZ),
                new FrameworkPropertyMetadata(HeadType.None,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        /// <summary>
        /// 始点のヘッドタイプ
        /// </summary>
        public HeadType HeadBeginType
        {
            get { return (HeadType)GetValue(HeadBeginTypeProperty); }
            set { SetValue(HeadBeginTypeProperty, value); }
        }
        public static readonly DependencyProperty HeadBeginTypeProperty =
            DependencyProperty.Register(nameof(HeadBeginType), typeof(HeadType), typeof(PolylineZ),
                new FrameworkPropertyMetadata(HeadType.None,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 矢印角度、初期値は30.0にしている。30～40くらいが適当
        /// </summary>
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register(nameof(Angle), typeof(double), typeof(PolylineZ),
                new FrameworkPropertyMetadata(30.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        //[TypeConverter(typeof(MyTypeConverterPoints))]
        //public ObservableCollection<Point> MyPoints
        //{
        //    get { return (ObservableCollection<Point>)GetValue(MyPointsProperty); }
        //    set { SetValue(MyPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointsProperty =
        //    DependencyProperty.Register(nameof(MyPoints), typeof(ObservableCollection<Point>), typeof(PolylineZ),
        //        new FrameworkPropertyMetadata(new ObservableCollection<Point>() { new Point(0, 0), new Point(100, 100) },
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure));
        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }

        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(PolylineZ),
                new FrameworkPropertyMetadata(new PointCollection() { new Point(0, 0), new Point(100, 100) },
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion 依存プロパティ


        private static double DegreeToRadian(double degree)
        {
            return degree / 360.0 * (Math.PI * 2.0);
        }

        /// <summary>
        /// 直線の描画
        /// </summary>
        /// <param name="context"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        private void DrawLine(StreamGeometryContext context, Point begin, Point end)
        {
            context.BeginFigure(begin, false, false);
            for (int i = 1; i < MyPoints.Count - 1; i++)
            {
                context.LineTo(MyPoints[i], true, false);
            }
            context.LineTo(end, true, false);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new() { FillRule = FillRule.Nonzero };
                using (var context = geometry.Open())
                {
                    Point begin = MyPoints[0];
                    Point end = MyPoints[^1];
                    switch (HeadBeginType)
                    {
                        case HeadType.None:
                            break;
                        case HeadType.Arrow:
                            begin = DrawBeginArrow(context);
                            break;
                    }
                    switch (HeadEndType)
                    {
                        case HeadType.None:
                            break;
                        case HeadType.Arrow:
                            end = DrawEndArrow(context);
                            break;
                    }
                    DrawLine(context, begin, end);
                }
                geometry.Freeze();
                return geometry;
            }
        }

        /// <summary>
        /// 始点にアローヘッド描画
        /// </summary>
        /// <param name="context"></param>
        /// <returns>アローヘッドと直線との接点座標</returns>
        private Point DrawBeginArrow(StreamGeometryContext context)
        {
            double x0 = MyPoints[0].X;
            double y0 = MyPoints[0].Y;
            double x1 = MyPoints[1].X;
            double y1 = MyPoints[1].Y;

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
            context.BeginFigure(MyPoints[0], true, false);//fill, close
            context.LineTo(arrowP1, false, false);//isStroke, isSmoothJoin
            context.LineTo(arrowP2, false, false);

            //アローヘッドと直線との接点座標、
            //HeadSizeぴったりで計算すると僅かな隙間ができるので-1.0している、
            //-0.5でも隙間になる、-0.7で隙間なくなる
            return new Point(
                (headSize - 1.0) * Math.Cos(lineRadian) + x0,
                (headSize - 1.0) * Math.Sin(lineRadian) + y0);

        }

        /// <summary>
        /// 終点にアローヘッド描画
        /// </summary>
        /// <param name="context"></param>
        /// <returns>アローヘッドと直線との接点座標</returns>
        private Point DrawEndArrow(StreamGeometryContext context)
        {
            double x0 = MyPoints[^1].X;
            double x1 = MyPoints[^2].X;
            double y0 = MyPoints[^1].Y;
            double y1 = MyPoints[^2].Y;

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
            context.BeginFigure(MyPoints[^1], true, false);//fill, close
            context.LineTo(arrowP1, false, false);//isStroke, isSmoothJoin
            context.LineTo(arrowP2, false, false);

            return new Point(
                (headSize - 1.0) * Math.Cos(lineRadian) + x0,
                (headSize - 1.0) * Math.Sin(lineRadian) + y0);
        }

    }
}
