using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace _20230324
{
    class MyShape
    {
    }
    public class TShape : Shape
    {


        public Point P1
        {
            get { return (Point)GetValue(P1Property); }
            set { SetValue(P1Property, value); }
        }
        public static readonly DependencyProperty P1Property =
            DependencyProperty.Register(nameof(P1), typeof(Point), typeof(TShape),
                new FrameworkPropertyMetadata(new Point(0, 0),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Point P2
        {
            get { return (Point)GetValue(P2Property); }
            set { SetValue(P2Property, value); }
        }
        public static readonly DependencyProperty P2Property =
            DependencyProperty.Register(nameof(P2), typeof(Point), typeof(TShape),
                new FrameworkPropertyMetadata(new Point(100, 100),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //読み取り専用の依存関係プロパティ
        //WPF4.5入門 その43 「読み取り専用の依存関係プロパティ」 - かずきのBlog@hatena
        //        https://blog.okazuki.jp/entry/2014/08/18/083455
        /// <summary>
        /// Descendant、見た目上のRect
        /// </summary>
        private static readonly DependencyPropertyKey MyExternalBoundsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyExternalBounds), typeof(Rect), typeof(TShape),
                new FrameworkPropertyMetadata(Rect.Empty,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty MyExternalBoundsProperty =
            MyExternalBoundsPropertyKey.DependencyProperty;
        public Rect MyExternalBounds
        {
            get { return (Rect)GetValue(MyExternalBoundsProperty); }
            private set { SetValue(MyExternalBoundsPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey MyTFBoundsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyTFBounds), typeof(Rect), typeof(TShape),
                new FrameworkPropertyMetadata(Rect.Empty,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty MyTFBoundsProperty =
            MyTFBoundsPropertyKey.DependencyProperty;
        public Rect MyTFBounds
        {
            get { return (Rect)GetValue(MyTFBoundsProperty); }
            private set { SetValue(MyTFBoundsPropertyKey, value); }
        }

        public Geometry MyGeometry { get; private set; }
        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    context.BeginFigure(P1, false, false);
                    context.LineTo(P2, true, false);
                }
                geometry.Freeze();
                MyGeometry = geometry.Clone();
                return geometry;
            }
        }

        public TShape()
        {
            Stroke = Brushes.Crimson;
            StrokeThickness = 50;
            MyGeometry = DefiningGeometry.Clone();
            Loaded += TShape_Loaded;
        }

        private void TShape_Loaded(object sender, RoutedEventArgs e)
        {
            SetBounds();
        }
        public void SetBounds()
        {
            Pen pen = new(Stroke, StrokeThickness);
            var geo = MyGeometry.Bounds;
            var render = MyGeometry.GetRenderBounds(pen);
            var widen = MyGeometry.GetWidenedPathGeometry(pen).Bounds;
        }
        protected override Size MeasureOverride(Size constraint)
        {

            return base.MeasureOverride(constraint);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }
    }

    public class TTT : Grid
    {

        public Point P1
        {
            get { return (Point)GetValue(P1Property); }
            set { SetValue(P1Property, value); }
        }
        public static readonly DependencyProperty P1Property =
            DependencyProperty.Register(nameof(P1), typeof(Point), typeof(TTT),
                new FrameworkPropertyMetadata(new Point(0, 0),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Point P2
        {
            get { return (Point)GetValue(P2Property); }
            set { SetValue(P2Property, value); }
        }
        public static readonly DependencyProperty P2Property =
            DependencyProperty.Register(nameof(P2), typeof(Point), typeof(TTT),
                new FrameworkPropertyMetadata(new Point(100, 100),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public TShape MyTShape { get; private set; }
        public TTT()
        {
            MyTShape = new();
            Children.Add(MyTShape);
        }
    }
}
