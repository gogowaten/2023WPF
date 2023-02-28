using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Specialized;
using System.Windows.Data;

namespace _20230222
{
    public enum HeadType { None = 0, Arrow, }




    public class PolyBezierArrowLine2 : Shape
    {
        #region 依存プロパティ

        public bool IsBezier
        {
            get { return (bool)GetValue(IsBezierProperty); }
            set { SetValue(IsBezierProperty, value); }
        }
        public static readonly DependencyProperty IsBezierProperty =
            DependencyProperty.Register(nameof(IsBezier), typeof(bool), typeof(PolyBezierArrowLine2),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 終点のヘッドタイプ
        /// </summary>
        public HeadType HeadEndType
        {
            get { return (HeadType)GetValue(HeadEndTypeProperty); }
            set { SetValue(HeadEndTypeProperty, value); }
        }
        public static readonly DependencyProperty HeadEndTypeProperty =
            DependencyProperty.Register(nameof(HeadEndType), typeof(HeadType), typeof(PolyBezierArrowLine2),
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
            DependencyProperty.Register(nameof(HeadBeginType), typeof(HeadType), typeof(PolyBezierArrowLine2),
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
            DependencyProperty.Register(nameof(Angle), typeof(double), typeof(PolyBezierArrowLine2),
                new FrameworkPropertyMetadata(30.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        [TypeConverter(typeof(MyTypeConverterPoints))]
        public ObservableCollection<Point> Points
        {
            get { return (ObservableCollection<Point>)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register(nameof(Points), typeof(ObservableCollection<Point>), typeof(PolyBezierArrowLine2),
                new FrameworkPropertyMetadata(new ObservableCollection<Point>() { new Point(0, 0), new Point(100, 100) },
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        //public PointCollection Points
        //{
        //    get { return (PointCollection)GetValue(PointsProperty); }
        //    set { SetValue(PointsProperty, value); }
        //}

        //public static readonly DependencyProperty PointsProperty =
        //    DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(PolyBezierArrowLine2),
        //        new FrameworkPropertyMetadata(new PointCollection() { new Point(0, 0), new Point(100, 100) },
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion 依存プロパティ
        protected override Geometry DefiningGeometry
        {

            get
            {
                StreamGeometry geometry = new() { FillRule = FillRule.Nonzero };
                if (Points.Count < 2) { return geometry; }

                using (var context = geometry.Open())
                {
                    Point begin = Points[0];//始点
                    Point end = Points[^1];//終点
                    //始点図形の描画
                    switch (HeadBeginType)
                    {
                        case HeadType.None:
                            break;
                        case HeadType.Arrow:
                            begin = DrawBeginArrow(context);
                            break;
                    }
                    //終点図形の描画
                    switch (HeadEndType)
                    {
                        case HeadType.None:
                            break;
                        case HeadType.Arrow:
                            end = DrawEndArrow(context);
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
        private static double DegreeToRadian(double degree)
        {
            return degree / 360.0 * (Math.PI * 2.0);
        }

    }


    //public class PolyBezierArrowLine2 : Shape
    //{
    //    #region 依存プロパティ

    //    public bool IsBezier
    //    {
    //        get { return (bool)GetValue(IsBezierProperty); }
    //        set { SetValue(IsBezierProperty, value); }
    //    }
    //    public static readonly DependencyProperty IsBezierProperty =
    //        DependencyProperty.Register(nameof(IsBezier), typeof(bool), typeof(PolyBezierArrowLine2),
    //            new FrameworkPropertyMetadata(false,
    //                FrameworkPropertyMetadataOptions.AffectsRender |
    //                FrameworkPropertyMetadataOptions.AffectsMeasure));

    //    /// <summary>
    //    /// 終点のヘッドタイプ
    //    /// </summary>
    //    public HeadType HeadEndType
    //    {
    //        get { return (HeadType)GetValue(HeadEndTypeProperty); }
    //        set { SetValue(HeadEndTypeProperty, value); }
    //    }
    //    public static readonly DependencyProperty HeadEndTypeProperty =
    //        DependencyProperty.Register(nameof(HeadEndType), typeof(HeadType), typeof(PolyBezierArrowLine2),
    //            new FrameworkPropertyMetadata(HeadType.None,
    //                FrameworkPropertyMetadataOptions.AffectsRender |
    //                FrameworkPropertyMetadataOptions.AffectsMeasure));
    //    /// <summary>
    //    /// 始点のヘッドタイプ
    //    /// </summary>
    //    public HeadType HeadBeginType
    //    {
    //        get { return (HeadType)GetValue(HeadBeginTypeProperty); }
    //        set { SetValue(HeadBeginTypeProperty, value); }
    //    }
    //    public static readonly DependencyProperty HeadBeginTypeProperty =
    //        DependencyProperty.Register(nameof(HeadBeginType), typeof(HeadType), typeof(PolyBezierArrowLine2),
    //            new FrameworkPropertyMetadata(HeadType.None,
    //                FrameworkPropertyMetadataOptions.AffectsRender |
    //                FrameworkPropertyMetadataOptions.AffectsMeasure));

    //    /// <summary>
    //    /// 矢印角度、初期値は30.0にしている。30～40くらいが適当
    //    /// </summary>
    //    public double Angle
    //    {
    //        get { return (double)GetValue(AngleProperty); }
    //        set { SetValue(AngleProperty, value); }
    //    }
    //    public static readonly DependencyProperty AngleProperty =
    //        DependencyProperty.Register(nameof(Angle), typeof(double), typeof(PolyBezierArrowLine2),
    //            new FrameworkPropertyMetadata(30.0,
    //                FrameworkPropertyMetadataOptions.AffectsRender |
    //                FrameworkPropertyMetadataOptions.AffectsMeasure));

    //    //[TypeConverter(typeof(MyTypeConverterPoints))]
    //    //public ObservableCollection<Point> Points
    //    //{
    //    //    get { return (ObservableCollection<Point>)GetValue(PointsProperty); }
    //    //    set { SetValue(PointsProperty, value); }
    //    //}
    //    //public static readonly DependencyProperty PointsProperty =
    //    //    DependencyProperty.Register(nameof(Points), typeof(ObservableCollection<Point>), typeof(PolyBezierArrowLine2),
    //    //        new FrameworkPropertyMetadata(new ObservableCollection<Point>() { new Point(0, 0), new Point(100, 100) },
    //    //            FrameworkPropertyMetadataOptions.AffectsRender |
    //    //            FrameworkPropertyMetadataOptions.AffectsMeasure));
    //    public PointCollection Points
    //    {
    //        get { return (PointCollection)GetValue(PointsProperty); }
    //        set { SetValue(PointsProperty, value); }
    //    }

    //    public static readonly DependencyProperty PointsProperty =
    //        DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(PolyBezierArrowLine2),
    //            new FrameworkPropertyMetadata(new PointCollection() { new Point(0, 0), new Point(100, 100) },
    //                FrameworkPropertyMetadataOptions.AffectsRender |
    //                FrameworkPropertyMetadataOptions.AffectsMeasure));

    //    #endregion 依存プロパティ
    //    protected override Geometry DefiningGeometry
    //    {

    //        get
    //        {
    //            StreamGeometry geometry = new() { FillRule = FillRule.Nonzero };

    //            return geometry;
    //        }
    //    }

    //}


    //public class BezierZ : PolyBezierArrowLine2
    //{
    //    public BezierZ()
    //    {

    //    }
    //    protected override Geometry DefiningGeometry
    //    {
    //        get
    //        {
    //            //return base.DefiningGeometry;
    //            StreamGeometry geometry = new() { FillRule = FillRule.Nonzero };
    //            if (Points.Count < 2) { return geometry; }

    //            using (var context = geometry.Open())
    //            {
    //                Point begin = Points[0];
    //                Point end = Points[^1];
    //                switch (HeadBeginType)
    //                {
    //                    case HeadType.None:
    //                        break;
    //                    case HeadType.Arrow:
    //                        begin = DrawBeginArrow(context);
    //                        break;
    //                }
    //                switch (HeadEndType)
    //                {
    //                    case HeadType.None:
    //                        break;
    //                    case HeadType.Arrow:
    //                        end = DrawEndArrow(context);
    //                        break;
    //                }
    //                DrawLine(context, begin, end);
    //            }
    //            geometry.Freeze();
    //            return geometry;
    //        }
    //    }


    //    /// <summary>
    //    /// 直線の描画
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="begin"></param>
    //    /// <param name="end"></param>
    //    private void DrawLine(StreamGeometryContext context, Point begin, Point end)
    //    {
    //        context.BeginFigure(begin, false, false);
    //        //context.PolyBezierTo(Points, true, false);
    //        context.PolyBezierTo(Points.Skip(1).ToList(), true, false);

    //    }

    //    /// <summary>
    //    /// 始点にアローヘッド描画
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <returns>アローヘッドと直線との接点座標</returns>
    //    private Point DrawBeginArrow(StreamGeometryContext context)
    //    {
    //        double x0 = Points[0].X;
    //        double y0 = Points[0].Y;
    //        double x1 = Points[1].X;
    //        double y1 = Points[1].Y;

    //        double lineRadian = Math.Atan2(y1 - y0, x1 - x0);
    //        double arrowRadian = DegreeToRadian(Angle);
    //        double headSize = StrokeThickness * 2.0;
    //        double wingLength = headSize / Math.Cos(arrowRadian);

    //        double wingRadian1 = lineRadian + arrowRadian;
    //        Point arrowP1 = new(
    //            wingLength * Math.Cos(wingRadian1) + x0,
    //            wingLength * Math.Sin(wingRadian1) + y0);
    //        double wingRadian2 = lineRadian - DegreeToRadian(Angle);
    //        Point arrowP2 = new(
    //            wingLength * Math.Cos(wingRadian2) + x0,
    //            wingLength * Math.Sin(wingRadian2) + y0);
    //        //アローヘッド描画、Fill(塗りつぶし)で描画
    //        context.BeginFigure(Points[0], true, false);//fill, close
    //        context.LineTo(arrowP1, false, false);//isStroke, isSmoothJoin
    //        context.LineTo(arrowP2, false, false);

    //        //アローヘッドと直線との接点座標、
    //        //HeadSizeぴったりで計算すると僅かな隙間ができるので-1.0している、
    //        //-0.5でも隙間になる、-0.7で隙間なくなる
    //        return new Point(
    //            (headSize - 1.0) * Math.Cos(lineRadian) + x0,
    //            (headSize - 1.0) * Math.Sin(lineRadian) + y0);

    //    }

    //    /// <summary>
    //    /// 終点にアローヘッド描画
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <returns>アローヘッドと直線との接点座標</returns>
    //    private Point DrawEndArrow(StreamGeometryContext context)
    //    {
    //        double x0 = Points[^1].X;
    //        double x1 = Points[^2].X;
    //        double y0 = Points[^1].Y;
    //        double y1 = Points[^2].Y;

    //        double lineRadian = Math.Atan2(y1 - y0, x1 - x0);
    //        double arrowRadian = DegreeToRadian(Angle);
    //        double headSize = StrokeThickness * 2.0;
    //        double wingLength = headSize / Math.Cos(arrowRadian);

    //        double wingRadian1 = lineRadian + arrowRadian;
    //        Point arrowP1 = new(
    //            wingLength * Math.Cos(wingRadian1) + x0,
    //            wingLength * Math.Sin(wingRadian1) + y0);
    //        double wingRadian2 = lineRadian - DegreeToRadian(Angle);
    //        Point arrowP2 = new(
    //            wingLength * Math.Cos(wingRadian2) + x0,
    //            wingLength * Math.Sin(wingRadian2) + y0);
    //        //アローヘッド描画、Fill(塗りつぶし)で描画
    //        context.BeginFigure(Points[^1], true, false);//fill, close
    //        context.LineTo(arrowP1, false, false);//isStroke, isSmoothJoin
    //        context.LineTo(arrowP2, false, false);

    //        return new Point(
    //            (headSize - 1.0) * Math.Cos(lineRadian) + x0,
    //            (headSize - 1.0) * Math.Sin(lineRadian) + y0);
    //    }
    //    private static double DegreeToRadian(double degree)
    //    {
    //        return degree / 360.0 * (Math.PI * 2.0);
    //    }

    //}

    //public class BezierZ : PolyBezierArrowLine2
    //{
    //    public BezierZ()
    //    {

    //    }
    //    protected override Geometry DefiningGeometry
    //    {
    //        get
    //        {
    //            //return base.DefiningGeometry;
    //            StreamGeometry geometry = new() { FillRule = FillRule.Nonzero };
    //            if(Points.Count < 2) { return geometry; }

    //            using (var context = geometry.Open())
    //            {
    //                Point begin = Points[0];
    //                Point end = Points[^1];
    //                switch (HeadBeginType)
    //                {
    //                    case HeadType.None:
    //                        break;
    //                    case HeadType.Arrow:
    //                        begin = DrawBeginArrow(context);
    //                        break;
    //                }
    //                switch (HeadEndType)
    //                {
    //                    case HeadType.None:
    //                        break;
    //                    case HeadType.Arrow:
    //                        end = DrawEndArrow(context);
    //                        break;
    //                }
    //                DrawBezier(context, begin, end);
    //            }
    //            geometry.Freeze();
    //            return geometry;
    //        }
    //    }


    //    /// <summary>
    //    /// 直線の描画
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="begin"></param>
    //    /// <param name="end"></param>
    //    private void DrawBezier(StreamGeometryContext context, Point begin, Point end)
    //    {
    //        context.BeginFigure(begin, false, false);
    //        //context.PolyBezierTo(Points, true, false);
    //        context.PolyBezierTo(Points.Skip(1).ToList(), true, false);

    //    }

    //    /// <summary>
    //    /// 始点にアローヘッド描画
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <returns>アローヘッドと直線との接点座標</returns>
    //    private Point DrawBeginArrow(StreamGeometryContext context)
    //    {
    //        double x0 = Points[0].X;
    //        double y0 = Points[0].Y;
    //        double x1 = Points[1].X;
    //        double y1 = Points[1].Y;

    //        double lineRadian = Math.Atan2(y1 - y0, x1 - x0);
    //        double arrowRadian = DegreeToRadian(Angle);
    //        double headSize = StrokeThickness * 2.0;
    //        double wingLength = headSize / Math.Cos(arrowRadian);

    //        double wingRadian1 = lineRadian + arrowRadian;
    //        Point arrowP1 = new(
    //            wingLength * Math.Cos(wingRadian1) + x0,
    //            wingLength * Math.Sin(wingRadian1) + y0);
    //        double wingRadian2 = lineRadian - DegreeToRadian(Angle);
    //        Point arrowP2 = new(
    //            wingLength * Math.Cos(wingRadian2) + x0,
    //            wingLength * Math.Sin(wingRadian2) + y0);
    //        //アローヘッド描画、Fill(塗りつぶし)で描画
    //        context.BeginFigure(Points[0], true, false);//fill, close
    //        context.LineTo(arrowP1, false, false);//isStroke, isSmoothJoin
    //        context.LineTo(arrowP2, false, false);

    //        //アローヘッドと直線との接点座標、
    //        //HeadSizeぴったりで計算すると僅かな隙間ができるので-1.0している、
    //        //-0.5でも隙間になる、-0.7で隙間なくなる
    //        return new Point(
    //            (headSize - 1.0) * Math.Cos(lineRadian) + x0,
    //            (headSize - 1.0) * Math.Sin(lineRadian) + y0);

    //    }

    //    /// <summary>
    //    /// 終点にアローヘッド描画
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <returns>アローヘッドと直線との接点座標</returns>
    //    private Point DrawEndArrow(StreamGeometryContext context)
    //    {
    //        double x0 = Points[^1].X;
    //        double x1 = Points[^2].X;
    //        double y0 = Points[^1].Y;
    //        double y1 = Points[^2].Y;

    //        double lineRadian = Math.Atan2(y1 - y0, x1 - x0);
    //        double arrowRadian = DegreeToRadian(Angle);
    //        double headSize = StrokeThickness * 2.0;
    //        double wingLength = headSize / Math.Cos(arrowRadian);

    //        double wingRadian1 = lineRadian + arrowRadian;
    //        Point arrowP1 = new(
    //            wingLength * Math.Cos(wingRadian1) + x0,
    //            wingLength * Math.Sin(wingRadian1) + y0);
    //        double wingRadian2 = lineRadian - DegreeToRadian(Angle);
    //        Point arrowP2 = new(
    //            wingLength * Math.Cos(wingRadian2) + x0,
    //            wingLength * Math.Sin(wingRadian2) + y0);
    //        //アローヘッド描画、Fill(塗りつぶし)で描画
    //        context.BeginFigure(Points[^1], true, false);//fill, close
    //        context.LineTo(arrowP1, false, false);//isStroke, isSmoothJoin
    //        context.LineTo(arrowP2, false, false);

    //        return new Point(
    //            (headSize - 1.0) * Math.Cos(lineRadian) + x0,
    //            (headSize - 1.0) * Math.Sin(lineRadian) + y0);
    //    }
    //    private static double DegreeToRadian(double degree)
    //    {
    //        return degree / 360.0 * (Math.PI * 2.0);
    //    }

    //}




    //public class BezierZ : PolyBezierArrowLine2
    //{
    //    public BezierZ()
    //    {

    //    }
    //    protected override Geometry DefiningGeometry
    //    {
    //        get
    //        {
    //            //return base.DefiningGeometry;
    //            StreamGeometry geometry = new() { FillRule = FillRule.Nonzero };
    //            if(Points.Count < 2) { return geometry; }

    //            using (var context = geometry.Open())
    //            {
    //                Point begin = Points[0];
    //                Point end = Points[^1];
    //                switch (HeadBeginType)
    //                {
    //                    case HeadType.None:
    //                        break;
    //                    case HeadType.Arrow:
    //                        begin = DrawBeginArrow(context);
    //                        break;
    //                }
    //                switch (HeadEndType)
    //                {
    //                    case HeadType.None:
    //                        break;
    //                    case HeadType.Arrow:
    //                        end = DrawEndArrow(context);
    //                        break;
    //                }
    //                DrawBezier(context, begin, end);
    //            }
    //            geometry.Freeze();
    //            return geometry;
    //        }
    //    }


    //    /// <summary>
    //    /// 直線の描画
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="begin"></param>
    //    /// <param name="end"></param>
    //    private void DrawBezier(StreamGeometryContext context, Point begin, Point end)
    //    {
    //        context.BeginFigure(begin, false, false);
    //        for (int i = 1; i < Points.Count - 1; i++)
    //        {
    //            context.LineTo(Points[i], true, false);
    //        }
    //        context.LineTo(end, true, false);
    //    }

    //    /// <summary>
    //    /// 始点にアローヘッド描画
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <returns>アローヘッドと直線との接点座標</returns>
    //    private Point DrawBeginArrow(StreamGeometryContext context)
    //    {
    //        double x0 = Points[0].X;
    //        double y0 = Points[0].Y;
    //        double x1 = Points[1].X;
    //        double y1 = Points[1].Y;

    //        double lineRadian = Math.Atan2(y1 - y0, x1 - x0);
    //        double arrowRadian = DegreeToRadian(Angle);
    //        double headSize = StrokeThickness * 2.0;
    //        double wingLength = headSize / Math.Cos(arrowRadian);

    //        double wingRadian1 = lineRadian + arrowRadian;
    //        Point arrowP1 = new(
    //            wingLength * Math.Cos(wingRadian1) + x0,
    //            wingLength * Math.Sin(wingRadian1) + y0);
    //        double wingRadian2 = lineRadian - DegreeToRadian(Angle);
    //        Point arrowP2 = new(
    //            wingLength * Math.Cos(wingRadian2) + x0,
    //            wingLength * Math.Sin(wingRadian2) + y0);
    //        //アローヘッド描画、Fill(塗りつぶし)で描画
    //        context.BeginFigure(Points[0], true, false);//fill, close
    //        context.LineTo(arrowP1, false, false);//isStroke, isSmoothJoin
    //        context.LineTo(arrowP2, false, false);

    //        //アローヘッドと直線との接点座標、
    //        //HeadSizeぴったりで計算すると僅かな隙間ができるので-1.0している、
    //        //-0.5でも隙間になる、-0.7で隙間なくなる
    //        return new Point(
    //            (headSize - 1.0) * Math.Cos(lineRadian) + x0,
    //            (headSize - 1.0) * Math.Sin(lineRadian) + y0);

    //    }

    //    /// <summary>
    //    /// 終点にアローヘッド描画
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <returns>アローヘッドと直線との接点座標</returns>
    //    private Point DrawEndArrow(StreamGeometryContext context)
    //    {
    //        double x0 = Points[^1].X;
    //        double x1 = Points[^2].X;
    //        double y0 = Points[^1].Y;
    //        double y1 = Points[^2].Y;

    //        double lineRadian = Math.Atan2(y1 - y0, x1 - x0);
    //        double arrowRadian = DegreeToRadian(Angle);
    //        double headSize = StrokeThickness * 2.0;
    //        double wingLength = headSize / Math.Cos(arrowRadian);

    //        double wingRadian1 = lineRadian + arrowRadian;
    //        Point arrowP1 = new(
    //            wingLength * Math.Cos(wingRadian1) + x0,
    //            wingLength * Math.Sin(wingRadian1) + y0);
    //        double wingRadian2 = lineRadian - DegreeToRadian(Angle);
    //        Point arrowP2 = new(
    //            wingLength * Math.Cos(wingRadian2) + x0,
    //            wingLength * Math.Sin(wingRadian2) + y0);
    //        //アローヘッド描画、Fill(塗りつぶし)で描画
    //        context.BeginFigure(Points[^1], true, false);//fill, close
    //        context.LineTo(arrowP1, false, false);//isStroke, isSmoothJoin
    //        context.LineTo(arrowP2, false, false);

    //        return new Point(
    //            (headSize - 1.0) * Math.Cos(lineRadian) + x0,
    //            (headSize - 1.0) * Math.Sin(lineRadian) + y0);
    //    }
    //    private static double DegreeToRadian(double degree)
    //    {
    //        return degree / 360.0 * (Math.PI * 2.0);
    //    }

    //}

    //c# - コレクションDPのWPF TypeConversionAttribute
    //    https://stackoverflow.com/questions/5154230/wpf-typeconversionattribute-for-collection-dp


    //public class MyConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {

    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

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
