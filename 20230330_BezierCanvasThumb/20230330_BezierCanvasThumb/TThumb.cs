using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace _20230330_BezierCanvasThumb
{
    class TThumb : Thumb
    {
        #region 依存関係プロパティと通知プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TThumb),
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
            DependencyProperty.Register(nameof(MyAnchorVisible), typeof(Visibility), typeof(TThumb),
                new FrameworkPropertyMetadata(Visibility.Visible,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// ラインのつなぎ目をtrueで丸める、falseで鋭角にする
        /// </summary>
        public bool MyLineSmoothJoin
        {
            get { return (bool)GetValue(MyLineSmoothJoinProperty); }
            set { SetValue(MyLineSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyLineSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyLineSmoothJoin), typeof(bool), typeof(TThumb),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        /// <summary>
        /// ラインの始点と終点を繋ぐかどうか
        /// </summary>
        public bool MyLineClose
        {
            get { return (bool)GetValue(MyLineCloseProperty); }
            set { SetValue(MyLineCloseProperty, value); }
        }
        public static readonly DependencyProperty MyLineCloseProperty =
            DependencyProperty.Register(nameof(MyLineClose), typeof(bool), typeof(TThumb),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(TThumb),
                new FrameworkPropertyMetadata(Brushes.Crimson,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public bool MyIsEditing
        {
            get { return (bool)GetValue(MyIsEditingProperty); }
            set { SetValue(MyIsEditingProperty, value); }
        }
        public static readonly DependencyProperty MyIsEditingProperty =
            DependencyProperty.Register(nameof(MyIsEditing), typeof(bool), typeof(TThumb),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //頂点Thumbのサイズ
        public double MyAnchorThumbSize
        {
            get { return (double)GetValue(MyAnchorThumbSizeProperty); }
            set { SetValue(MyAnchorThumbSizeProperty, value); }
        }
        public static readonly DependencyProperty MyAnchorThumbSizeProperty =
            DependencyProperty.Register(nameof(MyAnchorThumbSize), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティと通知プロパティ

        public Bezier MyShape { get; private set; }
        public Canvas MyCanvas { get; private set; }

        public TThumb()
        {
            MyCanvas = SetTemplate();
            MyShape = new();
            MyCanvas.Children.Add(MyShape);
            Loaded += TThumb_Loaded;
            DragDelta += TThumb_DragDelta;
        }

        private void TThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is TThumb tt)
            {
                Canvas.SetLeft(tt, Canvas.GetLeft(tt) + e.HorizontalChange);
                Canvas.SetTop(tt, Canvas.GetTop(tt) + e.VerticalChange);
            }
        }

        private void TThumb_Loaded(object sender, RoutedEventArgs e)
        {
            SetMyBindings();
            UpdateLayout();//要る？
            if (MyShape.MyExternalBounds.IsEmpty == false)
            {
                {
                    Canvas.SetLeft(MyShape, -MyShape.MyExternalBounds.X);
                    Canvas.SetTop(MyShape, -MyShape.MyExternalBounds.Y);
                }
            }
            MyShape.MyAdorner.ThumbDragCompleted += MyAdorner_ThumbDragCompleted;
        }

        private void MyAdorner_ThumbDragCompleted(object arg1, Vector arg2)
        {
            FixCanvasLocate03();
        }

        private void SetMyBindings()
        {
            MyShape.SetBinding(Bezier.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });
            MyShape.SetBinding(Bezier.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeProperty) });
            MyShape.SetBinding(Bezier.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty) });
            MyShape.SetBinding(Bezier.MyIsEditingProperty, new Binding() { Source = this, Path = new PropertyPath(MyIsEditingProperty) });
            MyShape.SetBinding(Bezier.MyAnchorThumbSizeProperty, new Binding() { Source = this, Path = new PropertyPath(MyAnchorThumbSizeProperty) });

            MyCanvas.SetBinding(BackgroundProperty, new Binding() { Source = this, Path = new PropertyPath(BackgroundProperty) });

            SetBinding(WidthProperty, new Binding() { Source = MyShape, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectWidth() });
            SetBinding(HeightProperty, new Binding() { Source = MyShape, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectHeight() });


        }
        public void ChangeBinding()
        {
            if (MyIsEditing)
            {
                SetBinding(WidthProperty, new Binding() { Source = MyShape, Path = new PropertyPath(Bezier.MyAllBoundsProperty), Converter = new MyConverterRectWidth() });
                SetBinding(HeightProperty, new Binding() { Source = MyShape, Path = new PropertyPath(Bezier.MyAllBoundsProperty), Converter = new MyConverterRectHeight() });
                var all = MyShape.MyAllBounds;
                var ex = MyShape.MyExternalBounds;

                Canvas.SetLeft(this, Canvas.GetLeft(this) + all.X - ex.X);
                Canvas.SetTop(this, Canvas.GetTop(this) + all.Y - ex.Y);

                Canvas.SetLeft(MyShape, -all.X);
                Canvas.SetTop(MyShape, -all.Y);
            }
            else
            {
                SetBinding(WidthProperty, new Binding() { Source = MyShape, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectWidth() });
                SetBinding(HeightProperty, new Binding() { Source = MyShape, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectHeight() });
                var ex = MyShape.MyExternalBounds;
                var all = MyShape.MyAllBounds;
                //var offset = VisualTreeHelper.GetOffset(this);

                Canvas.SetLeft(this, Canvas.GetLeft(this) + ex.X - all.X);
                Canvas.SetTop(this, Canvas.GetTop(this) + ex.Y - all.Y);
                Canvas.SetLeft(MyShape, -ex.X);
                Canvas.SetTop(MyShape, -ex.Y);
            }
        }
        /// <summary>
        /// 頂点Thumbのドラッグ移動終了後に実行する
        /// Canvas自身と図形の座標決定する
        /// </summary>
        public void FixCanvasLocate03()
        {
            var ex = MyShape.MyExternalBounds;
            var pts = GetPointsRect(MyPoints);
            var left = Canvas.GetLeft(this); var top = Canvas.GetTop(this);
            var bLeft = Canvas.GetLeft(MyShape); var bTop = Canvas.GetTop(MyShape);

            //自身の座標決定
            double x;
            if (ex.X > pts.X) x = left + pts.X + bLeft;
            else x = left + ex.X + bLeft;

            double y;
            if (ex.Y > pts.Y) y = top + pts.Y + bTop;
            else y = top + ex.Y + bTop;
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);

            //図形の座標決定
            var ex_ptsx = ex.X - pts.X;
            var ex_ptsy = ex.Y - pts.Y;
            double bx = ex_ptsx < 0 ? -ex_ptsx : 0;
            double by = ex_ptsy < 0 ? -ex_ptsy : 0;
            Canvas.SetLeft(MyShape, bx);
            Canvas.SetTop(MyShape, by);

            if (pts.X != 0 || pts.Y != 0)
            {
                Fix0Point();
                MyShape.MyAdorner.FixThumbsLocate();
            }
        }

        /// <summary>
        /// PointCollectionのRectを返す
        /// </summary>
        /// <param name="pt">PointCollection</param>
        /// <returns></returns>
        public static Rect GetPointsRect(PointCollection pt)
        {
            if (pt.Count == 0) return new Rect();
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            foreach (var item in pt)
            {
                if (minX > item.X) minX = item.X;
                if (minY > item.Y) minY = item.Y;
                if (maxX < item.X) maxX = item.X;
                if (maxY < item.Y) maxY = item.Y;
            }
            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }


        //Pointsの左上を0,0にするだけ
        public void Fix0Point()
        {
            Rect r = GetPointsRect(MyPoints);
            for (int i = 0; i < MyPoints.Count; i++)
            {
                Point pp = MyPoints[i];
                MyPoints[i] = new Point(pp.X - r.Left, pp.Y - r.Top);
            }
        }

        private Canvas SetTemplate()
        {
            FrameworkElementFactory factory = new(typeof(Canvas), "panel");
            Template = new() { VisualTree = factory };
            ApplyTemplate();
            if (Template.FindName("panel", this) is Canvas panel)
            {
                return panel;
            }
            else { throw new Exception(); }
        }
    }
}
