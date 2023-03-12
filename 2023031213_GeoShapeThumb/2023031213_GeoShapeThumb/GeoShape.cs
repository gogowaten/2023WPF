using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Media.Imaging;

//2023WPF/GeometryShape.cs at main · gogowaten/2023WPF
//https://github.com/gogowaten/2023WPF/blob/main/20230312_GeoShape/20230312_GeoShape/GeometryShape.cs
//上のコピペ

namespace _2023031213_GeoShapeThumb
{
    public enum ShapeType { Line = 0, Bezier, Fill }

    public class GeometryShape : Shape, INotifyPropertyChanged
    {
        #region 依存関係プロパティと通知プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeometryShape),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 頂点座標のThumbsの表示設定
        /// </summary>
        public Visibility MyAnchorVisible
        {
            get { return (Visibility)GetValue(MyAnchorVisibleProperty); }
            set { SetValue(MyAnchorVisibleProperty, value); }
        }
        public static readonly DependencyProperty MyAnchorVisibleProperty =
            DependencyProperty.Register(nameof(MyAnchorVisible), typeof(Visibility), typeof(GeometryShape),
                new FrameworkPropertyMetadata(Visibility.Visible,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// ラインのつなぎ目をtrueで丸める、falseで鋭角にする
        /// </summary>
        public bool MyLineSmoothJoin
        {
            get { return (bool)GetValue(MyLineSmoothJoinProperty); }
            set { SetValue(MyLineSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyLineSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyLineSmoothJoin), typeof(bool), typeof(GeometryShape),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        /// <summary>
        /// ラインの始点と終点を繋ぐかどうか
        /// </summary>
        public bool MyLineClose
        {
            get { return (bool)GetValue(MyLineCloseProperty); }
            set { SetValue(MyLineCloseProperty, value); }
        }
        public static readonly DependencyProperty MyLineCloseProperty =
            DependencyProperty.Register(nameof(MyLineClose), typeof(bool), typeof(GeometryShape),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        public ShapeType MyShapeType
        {
            get { return (ShapeType)GetValue(MyShapeTypeProperty); }
            set { SetValue(MyShapeTypeProperty, value); }
        }
        public static readonly DependencyProperty MyShapeTypeProperty =
            DependencyProperty.Register(nameof(MyShapeType), typeof(ShapeType), typeof(GeometryShape),
                new FrameworkPropertyMetadata(ShapeType.Line,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));




        public event PropertyChangedEventHandler? PropertyChanged;
        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //以下必要？
        //private Rect _myBounds;
        //public Rect MyBounds { get => _myBounds; set => SetProperty(ref _myBounds, value); }

        private Rect _myTFBounds;
        public Rect MyTFBounds { get => _myTFBounds; set => SetProperty(ref _myTFBounds, value); }

        private double _myTFWidth;
        public double MyTFWidth { get => _myTFWidth; set => SetProperty(ref _myTFWidth, value); }

        private double _myTFHeight;
        public double MyTFHeight { get => _myTFHeight; set => SetProperty(ref _myTFHeight, value); }


        #endregion 依存関係プロパティと通知プロパティ


        public Geometry MyGeometry { get; protected set; }
        public Rect MyExternalBoundsNotTF { get; protected set; }//外観のRect、変形なし時
        public Rect MyExternalBounds { get; protected set; }//外観のRect、変形加味
        public Rect MyInternalBoundsNotTF { get; protected set; }//PointsだけのRect、変形なし時
        public Rect MyInternalBounds { get; protected set; }//PointsだけのRect、変形なし時


        public List<Thumb> MyThumbs { get; protected set; } = new();
        public GeometryAdorner MyAdorner { get; protected set; }

        public AdornerLayer? MyAdornerLayer { get; protected set; }

        public GeometryShape()
        {
            Stroke = Brushes.Orange;
            StrokeThickness = 20;
            MyAdorner = new GeometryAdorner(this);
            MyGeometry = this.DefiningGeometry.Clone();
            Loaded += this_Loaded;

            MyAdorner.SetBinding(VisibilityProperty, new Binding() { Source = this, Path = new PropertyPath(MyAnchorVisibleProperty) });
        }

        //未使用
        //一旦Thumbを削除して、表示し直す
        public void UpdateAdorner()
        {
            if (MyAdornerLayer != null)
            {
                var adols = MyAdornerLayer.GetAdorners(this);
                foreach (var adol in adols)
                {
                    MyAdornerLayer.Remove(adol);
                }
                MyAdornerLayer.Add(new GeometryAdorner(this));
            }
        }
        private void this_Loaded(object sender, RoutedEventArgs e)
        {
            MyAdornerLayer = AdornerLayer.GetAdornerLayer(this);
            MyAdornerLayer.Add(MyAdorner);
            //MyAdornerLayer.Add(new BoundsAdorner(this));
        }

        //protected override Geometry DefiningGeometry => Geometry.Empty;
        protected override Geometry DefiningGeometry
        {            
            get
            {
                if (MyPoints.Count == 0) return Geometry.Empty;
                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    switch (MyShapeType)
                    {
                        case ShapeType.Line:
                            context.BeginFigure(MyPoints[0], false, MyLineClose);
                            context.PolyLineTo(MyPoints.Skip(1).ToArray(), true, MyLineSmoothJoin);
                            break;
                        case ShapeType.Bezier:
                            context.BeginFigure(MyPoints[0], false, MyLineClose);
                            context.PolyBezierTo(MyPoints.Skip(1).ToArray(), true, MyLineSmoothJoin);
                            break;
                        case ShapeType.Fill:
                            context.BeginFigure(MyPoints[0], true, MyLineClose);
                            context.PolyLineTo(MyPoints.Skip(1).ToArray(), false, MyLineSmoothJoin);
                            break;
                    }
                }
                geometry.Freeze();
                MyGeometry = geometry.Clone();
                return geometry;
            }
        }

        ////変形時にBoundsを更新、これは変形してもArrangeは無反応だから→
        ////Arrangeでも反応していた
        //protected override Geometry GetLayoutClip(Size layoutSlotSize)
        //{
        //    return base.GetLayoutClip(layoutSlotSize);
        //}

        //各種Bounds更新
        protected override Size ArrangeOverride(Size finalSize)
        {
            Pen pen = new(Stroke, StrokeThickness);
            Geometry geo = this.DefiningGeometry.Clone();
            MyInternalBoundsNotTF = geo.Bounds;//内部Rect
            MyExternalBoundsNotTF = geo.GetRenderBounds(pen);//外部Rect

            Geometry exGeo = geo.Clone();
            exGeo.Transform = RenderTransform;//変形
            MyInternalBounds = exGeo.Bounds;//変形内部Rect
            //MyInternalBounds = this.RenderTransform.TransformBounds(MyInternalBoundsNotTF);//変形内部Rect
            MyExternalBounds = exGeo.GetRenderBounds(pen);//変形外部Rect


            MyTFBounds = MyExternalBounds;
            MyTFWidth = MyExternalBounds.Width;
            MyTFHeight = MyExternalBounds.Height;



            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        /// ピッタリのサイズのBitmap取得
        /// </summary>
        /// <returns></returns>
        public BitmapSource GetBitmap()
        {
            Geometry geo = MyGeometry.Clone();
            geo.Transform = RenderTransform;
            PathGeometry pg = geo.GetWidenedPathGeometry(new Pen(Stroke, StrokeThickness));
            Rect rect = pg.Bounds;
            DrawingVisual dv = new() { Offset = new Vector(-rect.X, -rect.Y) };
            using (var context = dv.RenderOpen())
            {
                context.DrawGeometry(Stroke, null, pg);
            }
            RenderTargetBitmap bitmap = new((int)(rect.Width + 1), (int)(rect.Height + 1), 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
            return bitmap;
        }
    }


    ///// <summary>
    ///// 直線塗りつぶし図形
    ///// </summary>
    //public class GeometryShapeFill : GeometryShape
    //{
    //    public GeometryShapeFill()
    //    {

    //    }
    //    protected override Geometry DefiningGeometry
    //    {
    //        get
    //        {
    //            if (MyPoints.Count == 0) return Geometry.Empty;
    //            StreamGeometry geometry = new();
    //            using (var context = geometry.Open())
    //            {
    //                context.BeginFigure(MyPoints[0], true, MyLineClose);
    //                context.PolyLineTo(MyPoints.Skip(1).ToArray(), false, MyLineSmoothJoin);
    //            }
    //            geometry.Freeze();
    //            MyGeometry = geometry.Clone();
    //            return geometry;
    //            //return base.DefiningGeometry;
    //        }
    //    }
    //}


    ///// <summary>
    ///// ベジェ曲線図形
    ///// </summary>
    //public class GeometryShapeBezier : GeometryShape
    //{
    //    public GeometryShapeBezier()
    //    {

    //    }
    //    protected override Geometry DefiningGeometry
    //    {
    //        get
    //        {
    //            if (MyPoints.Count == 0) return Geometry.Empty;
    //            StreamGeometry geometry = new();
    //            using (var context = geometry.Open())
    //            {
    //                context.BeginFigure(MyPoints[0], false, MyLineClose);
    //                context.PolyBezierTo(MyPoints.Skip(1).ToArray(), true, MyLineSmoothJoin);
    //            }
    //            geometry.Freeze();
    //            MyGeometry = geometry.Clone();
    //            return geometry;
    //            //return base.DefiningGeometry;
    //        }
    //    }
    //}


    ///// <summary>
    ///// 直線図形
    ///// </summary>
    //public class GeometryShapeLine : GeometryShape
    //{
    //    public GeometryShapeLine()
    //    {

    //    }
    //    protected override Geometry DefiningGeometry
    //    {
    //        get
    //        {
    //            if (MyPoints.Count == 0) return Geometry.Empty;
    //            StreamGeometry geometry = new();
    //            using (var context = geometry.Open())
    //            {
    //                context.BeginFigure(MyPoints[0], false, MyLineClose);
    //                context.PolyLineTo(MyPoints.Skip(1).ToArray(), true, MyLineSmoothJoin);
    //            }
    //            geometry.Freeze();
    //            MyGeometry = geometry.Clone();
    //            return geometry;
    //            //return base.DefiningGeometry;
    //        }
    //    }
    //}


    /// <summary>
    /// 頂点座標にThumb表示するアドーナー。GeometryShape専用
    /// VisualCollectionにはCanvasだけを追加
    /// ThumbはCanvasに追加
    /// </summary>
    public class GeometryAdorner : Adorner
    {
        public VisualCollection MyVisuals { get; private set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];

        public Canvas MyCanvas { get; private set; } = new();
        public List<Thumb> MyThumbs { get; private set; } = new();
        public GeometryShape MyTargetGeoShape { get; private set; }
        public GeometryAdorner(GeometryShape adornedElement) : base(adornedElement)
        {
            MyVisuals = new VisualCollection(this)
            {
                MyCanvas
            };
            MyTargetGeoShape = adornedElement;
            Loaded += GeometryAdorner_Loaded;
        }

        private void GeometryAdorner_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in MyTargetGeoShape.MyPoints)
            {
                Thumb thumb = new() { Width = 20, Height = 20, Background = Brushes.Red, Opacity = 0.5 };
                MyThumbs.Add(thumb);
                MyCanvas.Children.Add(thumb);
                SetLocate(thumb, item);
                thumb.DragDelta += Thumb_DragDelta;
            }
        }


        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                int i = MyThumbs.IndexOf(t);
                PointCollection points = MyTargetGeoShape.MyPoints;
                double x = points[i].X + e.HorizontalChange;
                double y = points[i].Y + e.VerticalChange;
                points[i] = new Point(x, y);
                SetLocate(t, points[i]);
            }
        }

        private static void SetLocate(Thumb thumb, Point point)
        {
            Canvas.SetLeft(thumb, point.X);
            Canvas.SetTop(thumb, point.Y);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //Thumbが収まるRectをCanvasのArrangeに指定する
            Rect canvasRect = VisualTreeHelper.GetDescendantBounds(MyCanvas);
            if (canvasRect.IsEmpty)
            {
                MyCanvas.Arrange(new Rect(finalSize));
            }
            else
            {
                //座標を0,0したRectにする、こうしないとマイナス座表示に不具合
                canvasRect = new(canvasRect.Size);
                MyCanvas.Arrange(canvasRect);
            }
            return base.ArrangeOverride(finalSize);
        }

    }


}
