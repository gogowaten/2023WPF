﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _20230213_BezierTest
{
    public enum ArrowHeadType { None = 0, Arrow, Square, Round }
    public class Arrow : Shape
    {
        #region 依存プロパティ
        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }
        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register(nameof(X1), typeof(double), typeof(Arrow),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }
        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register(nameof(Y1), typeof(double), typeof(Arrow),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }
        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register(nameof(X2), typeof(double), typeof(Arrow),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }
        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register(nameof(Y2), typeof(double), typeof(Arrow),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        public double HeadSize
        {
            get { return (double)GetValue(HeadSizeProperty); }
            set { SetValue(HeadSizeProperty, value); }
        }
        public static readonly DependencyProperty HeadSizeProperty =
            DependencyProperty.Register(nameof(HeadSize), typeof(double), typeof(Arrow),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        public ArrowHeadType HeadType
        {
            get { return (ArrowHeadType)GetValue(HeadTypeProperty); }
            set { SetValue(HeadTypeProperty, value); }
        }
        public static readonly DependencyProperty HeadTypeProperty =
            DependencyProperty.Register(nameof(HeadType), typeof(ArrowHeadType), typeof(Arrow),
                new FrameworkPropertyMetadata(ArrowHeadType.None,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register(nameof(Angle), typeof(double), typeof(Arrow),
                new FrameworkPropertyMetadata(30.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        //public PointCollection MyPoints
        //{
        //    get { return (PointCollection)GetValue(MyPointsProperty); }
        //    set { SetValue(MyPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointsProperty =
        //    DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(Arrow),
        //        new FrameworkPropertyMetadata(new PointCollection() { new Point(0, 0), new Point(100, 100) },
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [TypeConverter(typeof(MyTypeConverterPoints))]
        public ObservableCollection<Point> MyPoints
        {
            get { return (ObservableCollection<Point>)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(ObservableCollection<Point>), typeof(Arrow),
                new FrameworkPropertyMetadata(new ObservableCollection<Point>() { new Point(0, 0), new Point(100, 100) },
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion 依存プロパティ

        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new();
                geometry.FillRule = FillRule.Nonzero;
                using (var context = geometry.Open())
                {
                    switch (HeadType)
                    {
                        case ArrowHeadType.None:
                            Draw(context);
                            break;
                        case ArrowHeadType.Arrow:
                            DrawArrow(context);
                            break;
                        case ArrowHeadType.Square:
                            break;
                        case ArrowHeadType.Round:
                            break;
                        default:
                            break;
                    }
                }
                geometry.Freeze();
                return geometry;
            }
        }
        private void Draw(StreamGeometryContext context)
        {
            context.BeginFigure(MyPoints[0], false, false);
            for (int i = 1; i < MyPoints.Count; i++)
            {
                context.LineTo(MyPoints[i], true, false);
            }
        }
        private void DrawArrow(StreamGeometryContext context)
        {
            double x1 = MyPoints[^1].X;
            double x2 = MyPoints[^2].X;
            double y1 = MyPoints[^1].Y;
            double y2 = MyPoints[^2].Y;
            double baseRadian = Math.Atan2(y2 - y1, x2 - x1);
            double bCos = Math.Cos(baseRadian);
            double bSin = Math.Sin(baseRadian);
            double radian = DegreeToRadian(Angle);
            double rCos = Math.Cos(radian);
            double headSize = StrokeThickness * 1.0 / Math.Sin(radian);
            double sideLength = headSize * rCos - 1.5;
            Point pContact = new(x1 + (bCos * sideLength), y1 + bSin * sideLength);

            double arrowHeadRadian = baseRadian + radian;
            Point pWing1 = new(
                x1 + headSize * Math.Cos(arrowHeadRadian),
                y1 + headSize * Math.Sin(arrowHeadRadian));

            arrowHeadRadian = baseRadian - radian;
            Point pWing2 = new(
                x1 + headSize * Math.Cos(arrowHeadRadian),
                y1 + headSize * Math.Sin(arrowHeadRadian));

            context.BeginFigure(MyPoints[0], false, false);
            for (int i = 1; i < MyPoints.Count - 1; i++)
            {
                context.LineTo(MyPoints[i], true, false);
            }
            context.LineTo(pContact, true, false);

            context.BeginFigure(MyPoints[^1], true, true);
            context.LineTo(pWing1, false, false);
            context.LineTo(pWing2, false, false);
            context.LineTo(MyPoints[^1], false, false);

        }
        private static double DegreeToRadian(double degree)
        {
            return degree / 360.0 * (Math.PI * 2.0);
        }
    }
}
