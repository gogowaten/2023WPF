using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _20230620
{
    //public class GeoPolyLine2 : Shape
    //{

    //    #region 依存関係プロパティ

    //    public PointCollection MyPoints
    //    {
    //        get { return (PointCollection)GetValue(MyPointsProperty); }
    //        set { SetValue(MyPointsProperty, value); }
    //    }
    //    public static readonly DependencyProperty MyPointsProperty =
    //        DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeoPolyLine2),
    //            new FrameworkPropertyMetadata(new PointCollection(),
    //                FrameworkPropertyMetadataOptions.AffectsRender |
    //                FrameworkPropertyMetadataOptions.AffectsMeasure |
    //                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    //    /// <summary>
    //    /// 図形自体のRect、読み取り専用にしたほうがいい？
    //    /// </summary>

    //    public Rect MyRenderRect
    //    {
    //        get { return (Rect)GetValue(MyRenderRectProperty); }
    //        set { SetValue(MyRenderRectProperty, value); }
    //    }
    //    public static readonly DependencyProperty MyRenderRectProperty =
    //        DependencyProperty.Register(nameof(MyRenderRect), typeof(Rect), typeof(GeoPolyLine2),
    //            new FrameworkPropertyMetadata(Rect.Empty,
    //                FrameworkPropertyMetadataOptions.AffectsRender |
    //                FrameworkPropertyMetadataOptions.AffectsMeasure |
    //                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    //    public Rect MyRenderRect2
    //    {
    //        get { return (Rect)GetValue(MyRenderRect2Property); }
    //        set { SetValue(MyRenderRect2Property, value); }
    //    }
    //    public static readonly DependencyProperty MyRenderRect2Property =
    //        DependencyProperty.Register(nameof(MyRenderRect2), typeof(Rect), typeof(GeoPolyLine2),
    //            new FrameworkPropertyMetadata(Rect.Empty,
    //                FrameworkPropertyMetadataOptions.AffectsRender |
    //                FrameworkPropertyMetadataOptions.AffectsMeasure |
    //                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



    //    public Rect MyGeoRect
    //    {
    //        get { return (Rect)GetValue(MyGeoRectProperty); }
    //        set { SetValue(MyGeoRectProperty, value); }
    //    }
    //    public static readonly DependencyProperty MyGeoRectProperty =
    //        DependencyProperty.Register(nameof(MyGeoRect), typeof(Rect), typeof(GeoPolyLine2),
    //            new FrameworkPropertyMetadata(Rect.Empty,
    //                FrameworkPropertyMetadataOptions.AffectsRender |
    //                FrameworkPropertyMetadataOptions.AffectsMeasure |
    //                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));




    //    public Pen MyPen
    //    {
    //        get { return (Pen)GetValue(MyPenProperty); }
    //        set { SetValue(MyPenProperty, value); }
    //    }
    //    public static readonly DependencyProperty MyPenProperty =
    //        DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(GeoPolyLine2),
    //            new FrameworkPropertyMetadata(new Pen(Brushes.Red, 10.0),
    //                FrameworkPropertyMetadataOptions.AffectsRender |
    //                FrameworkPropertyMetadataOptions.AffectsMeasure |
    //                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    //    #endregion 依存関係プロパティ


    //    protected override Geometry DefiningGeometry
    //    {

    //        get
    //        {
    //            if (MyPoints.Count <= 1) return Geometry.Empty;

    //            PolyLineSegment plseg = new(MyPoints.Skip(1).ToList(), true);
    //            PathSegmentCollection segments = new() { plseg };
    //            PathFigure figure = new()
    //            {
    //                StartPoint = MyPoints[0],
    //                IsClosed = false,
    //                Segments = segments
    //            };
    //            PathGeometry geometry = new();
    //            geometry.Figures.Add(figure);


    //            geometry.Freeze();
    //            MyGeoRect = geometry.Bounds;
    //            //以下2つは同じ値
    //            MyRenderRect = geometry.GetWidenedPathGeometry(MyPen).GetRenderBounds(null);
    //            MyRenderRect2 = geometry.GetRenderBounds(MyPen);

    //            return geometry;
    //        }
    //    }


    //    public GeoPolyLine2()
    //    {
    //        SetMyBindings();
    //    }

    //    private void SetMyBindings()
    //    {
    //        MultiBinding mb = new() { Converter = new MyConverterPen() };
    //        mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeProperty) });
    //        mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeThicknessProperty) });
    //        SetBinding(MyPenProperty, mb);


    //    }

    //}


    /// <summary>
    /// DefiningGeometryのgeometryをStreamGeometryからPathGeometryに変えてみたけど
    /// ThumbのActualが一手遅れる、StreamGeometryのときと同じ
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

        public double MyRenderWidth
        {
            get { return (double)GetValue(MyRenderWidthProperty); }
            set { SetValue(MyRenderWidthProperty, value); }
        }
        public static readonly DependencyProperty MyRenderWidthProperty =
            DependencyProperty.Register(nameof(MyRenderWidth), typeof(double), typeof(GeoPolyLine),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyRenderHeight
        {
            get { return (double)GetValue(MyRenderHeightProperty); }
            set { SetValue(MyRenderHeightProperty, value); }
        }
        public static readonly DependencyProperty MyRenderHeightProperty =
            DependencyProperty.Register(nameof(MyRenderHeight), typeof(double), typeof(GeoPolyLine),
                new FrameworkPropertyMetadata(0.0,
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

        //public Rect MyRenderRect { get; private set; }
        //public double MyRenderWidth { get; private set; }
        //public double MyRenderHeight { get; private set; }
        //public double MyRenderLeft { get; private set; }
        //public double MyRenderTop { get; private set; }

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
                MyRenderWidth = MyRenderRect.Width;
                MyRenderHeight = MyRenderRect.Height;
                //MyRenderLeft = MyRenderRect.Left;
                //MyRenderTop = MyRenderRect.Top;
                return geometry;
            }
        }

        //protected override Geometry DefiningGeometry
        //{

        //    get
        //    {
        //        if (MyPoints.Count <= 1) return Geometry.Empty;

        //        StreamGeometry geometry = new();                
        //        using (var content = geometry.Open())
        //        {
        //            content.BeginFigure(MyPoints[0], false, false);
        //            content.PolyLineTo(MyPoints.Skip(1).ToList(), true, false);
        //        }

        //        geometry.Freeze();
        //        ////Bounds計算用のGeometryを更新、StrokeThicknessを考慮したGeometry
        //        //MyGeometry = geometry.GetWidenedPathGeometry(MyPen);

        //        MyGeoRect = geometry.Bounds;
        //        //MyRenderRect = MyGeometry.GetRenderBounds(null);


        //        return geometry;
        //    }
        //}


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

            //SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(MyRenderRect2Property), Converter = new MyConverterRectLeft() });
            //SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(MyRenderRect2Property), Converter = new MyConverterRectTop() });

        }

    }

}
