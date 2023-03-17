using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;


//[028722]ベジエ曲線の各部の名称
//https://support.justsystems.com/faq/1032/app/servlet/qadoc?QID=028722

namespace _202303171255_DashLine
{

    /// <summary>
    /// ベジェ曲線の方向線表示用、2色破線
    /// OnRenderで直線描画、その上にDefiningGeometryで破線描画
    /// </summary>
    class DirectionLine2 : Shape
    {
        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(DirectionLine2),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// ベースの色
        /// </summary>
        public Brush StrokeBase
        {
            get { return (Brush)GetValue(StrokeBaseProperty); }
            set { SetValue(StrokeBaseProperty, value); }
        }
        public static readonly DependencyProperty StrokeBaseProperty =
            DependencyProperty.Register(nameof(StrokeBase), typeof(Brush), typeof(DirectionLine2),
                new FrameworkPropertyMetadata(Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        public DirectionLine2()
        {

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            for (int i = 0; i < MyPoints.Count - 1; i++)
            {
                if ((i - 1) % 3 != 0)
                {
                    drawingContext.DrawLine(new Pen(StrokeBase, StrokeThickness), MyPoints[i], MyPoints[i + 1]);
                }
            }
            base.OnRender(drawingContext);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    for (int i = 0; i < MyPoints.Count - 1; i++)
                    {
                        if ((i - 1) % 3 != 0)
                        {
                            //context.BeginFigure(MyPoints[0], false, false);
                            //context.LineTo(MyPoints[1], true, false);
                            //context.BeginFigure(MyPoints[2], false, false);
                            //context.LineTo(MyPoints[3], true, false);
                            //context.BeginFigure(MyPoints[3], false, false);
                            //context.LineTo(MyPoints[4], true, false);
                            //context.BeginFigure(MyPoints[5], false, false);
                            //context.LineTo(MyPoints[6], true, false);
                            //context.BeginFigure(MyPoints[6], false, false);
                            //context.LineTo(MyPoints[7], true, false);
                            //context.BeginFigure(MyPoints[8], false, false);
                            //context.LineTo(MyPoints[9], true, false);
                            context.BeginFigure(MyPoints[i], false, false);
                            context.LineTo(MyPoints[i + 1], true, false);
                        }
                    }
                }
                geometry.Freeze();
                return geometry;
            }
        }
    }


    /// <summary>
    /// Shapeより上の要素を継承して制御線表示してみたけど、いまいち
    /// いいところはDefiningGeometryを使わずにOnRenderだけで済むところ
    /// よくないのは依存関係プロパティが増える、
    /// サイズが0になる、回避するにはMeasureとかで設定する必要があるみたいだけどわからん
    /// </summary>
    class DirectionLine : FrameworkElement
    {
        #region 依存関係プロパティ



        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(DirectionLine),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// ベースの色
        /// </summary>
        public Brush StrokeBase
        {
            get { return (Brush)GetValue(StrokeBaseProperty); }
            set { SetValue(StrokeBaseProperty, value); }
        }
        public static readonly DependencyProperty StrokeBaseProperty =
            DependencyProperty.Register(nameof(StrokeBase), typeof(Brush), typeof(DirectionLine),
                new FrameworkPropertyMetadata(Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(DirectionLine),
                new FrameworkPropertyMetadata(Brushes.Black,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(DirectionLine),
                new FrameworkPropertyMetadata(1.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public DoubleCollection DashArray
        {
            get { return (DoubleCollection)GetValue(DashArrayProperty); }
            set { SetValue(DashArrayProperty, value); }
        }
        public static readonly DependencyProperty DashArrayProperty =
            DependencyProperty.Register(nameof(DashArray), typeof(DoubleCollection), typeof(DirectionLine),
                new FrameworkPropertyMetadata(new DoubleCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        #endregion 依存関係プロパティ

        public DirectionLine()
        {
            Loaded += DirectionLine_Loaded;
        }

        private void DirectionLine_Loaded(object sender, RoutedEventArgs e)
        {
            var rsize = RenderSize;
            var drawrect = VisualTreeHelper.GetDrawing(this)?.Bounds;
            var desbound = VisualTreeHelper.GetDescendantBounds(this);
            var desTF = RenderTransform.TransformBounds(desbound);
            Measure(desTF.Size);
            //Arrange(new Rect(desTF.Size));
            Arrange(desTF);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            for (int i = 0; i < MyPoints.Count; i++)
            {
                if ((i - 1) % 3 != 0)
                {
                    //実線
                    Pen pen = new(StrokeBase, StrokeThickness);
                    drawingContext.DrawLine(pen, MyPoints[i], MyPoints[i + 1]);
                    //破線
                    pen = new(Stroke, StrokeThickness);
                    pen.DashStyle = new DashStyle(DashArray, 0);
                    drawingContext.DrawLine(pen, MyPoints[i], MyPoints[i + 1]);
                }
            }
            base.OnRender(drawingContext);
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            var rsize = RenderSize;
            var drawrect = VisualTreeHelper.GetDrawing(this)?.Bounds;
            var desbound = VisualTreeHelper.GetDescendantBounds(this);

            return base.GetLayoutClip(layoutSlotSize);
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            var rsize = RenderSize;
            var drawrect = VisualTreeHelper.GetDrawing(this)?.Bounds;
            var desbound = VisualTreeHelper.GetDescendantBounds(this);
            return base.ArrangeOverride(finalSize);
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            var rsize = RenderSize;
            var drawrect = VisualTreeHelper.GetDrawing(this)?.Bounds;
            var desbound = VisualTreeHelper.GetDescendantBounds(this);

            return base.MeasureOverride(availableSize);
        }
    }


}
