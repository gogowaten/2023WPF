using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _20230213_BezierTest
{
    class TTBezier2 : Shape
    {
        #region 依存プロパティ

        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }
        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register(nameof(X1), typeof(double), typeof(TTBezier2),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }
        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register(nameof(Y1), typeof(double), typeof(TTBezier2),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }
        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register(nameof(X2), typeof(double), typeof(TTBezier2),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }
        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register(nameof(Y2), typeof(double), typeof(TTBezier2),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        public double X3
        {
            get { return (double)GetValue(X3Property); }
            set { SetValue(X3Property, value); }
        }
        public static readonly DependencyProperty X3Property =
            DependencyProperty.Register(nameof(X3), typeof(double), typeof(TTBezier2),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double Y3
        {
            get { return (double)GetValue(Y3Property); }
            set { SetValue(Y3Property, value); }
        }
        public static readonly DependencyProperty Y3Property =
            DependencyProperty.Register(nameof(Y3), typeof(double), typeof(TTBezier2),
                new FrameworkPropertyMetadata(0.0,
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
                    Draw(context);
                }
                geometry.Freeze();
                return geometry;

            }
        }
        private void Draw(StreamGeometryContext context)
        {
            context.BeginFigure(new Point(X1 - 0.5, Y1 - 0.5),
                false, false);//isFill,isClose
            context.PolyBezierTo(new List<Point>()
            {
                FixPoint(X1,Y1),
                FixPoint(X2,Y2),FixPoint(X2,Y2),FixPoint(X2,Y2),
                FixPoint(X3,Y3),FixPoint(X3,Y3),
            }
            , true, false);//isStroke, isSmoothJoin
        }
        private Point FixPoint(double x, double y)
        {
            return new Point(x - 0.5, y - 0.5);
        }
    }
}
