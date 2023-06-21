using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace _20230621_PolylineSizeThumb
{

    /// <summary>
    /// 描画座標を常に0,0にするPolyline
    /// 
    /// </summary>
    public class GeoPolyLine : Shape
    {

        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeoPolyLine),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 図形自体のRect、読み取り専用にしたほうがいい？
        /// </summary>
        public Rect MyRenderRect
        {
            get { return (Rect)GetValue(MyRenderRectProperty); }
            set { SetValue(MyRenderRectProperty, value); }
        }
        public static readonly DependencyProperty MyRenderRectProperty =
            DependencyProperty.Register(nameof(MyRenderRect), typeof(Rect), typeof(GeoPolyLine),
                new FrameworkPropertyMetadata(Rect.Empty,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(GeoPolyLine),
                new FrameworkPropertyMetadata(new Pen(Brushes.Red, 10.0),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存関係プロパティ


        //PathGeometryを使っているけど、問題がなければ
        //より負荷の少ないStreamGeometryを使った方がいい、多分問題ない
        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints.Count <= 1) return Geometry.Empty;

                PolyLineSegment plseg = new(MyPoints.Skip(1).ToList(), true);
                PathSegmentCollection segments = new() { plseg };
                PathFigure figure = new()
                {
                    StartPoint = MyPoints[0],
                    IsClosed = false,
                    Segments = segments
                };
                PathGeometry geometry = new();
                geometry.Figures.Add(figure);

                geometry.Freeze();
                MyRenderRect = geometry.GetRenderBounds(MyPen);

                return geometry;
            }
        }


        public GeoPolyLine()
        {
            SetMyBindings();
        }

        private void SetMyBindings()
        {
            MultiBinding mb = new() { Converter = new MyConverterPen() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeThicknessProperty) });
            SetBinding(MyPenProperty, mb);

            //描画座標を0,0にするバインド
            SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(MyRenderRectProperty), Converter = new MyConverterRectLeft() });
            SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(MyRenderRectProperty), Converter = new MyConverterRectTop() });

        }

    }
}
