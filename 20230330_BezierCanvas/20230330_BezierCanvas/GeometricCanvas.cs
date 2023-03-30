using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Documents;


namespace _20230330_BezierCanvas
{
    public class GeometricCanvas : Canvas
    {
        #region 依存関係プロパティと通知プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeometricCanvas),
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
            DependencyProperty.Register(nameof(MyAnchorVisible), typeof(Visibility), typeof(GeometricCanvas),
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
            DependencyProperty.Register(nameof(MyLineSmoothJoin), typeof(bool), typeof(GeometricCanvas),
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
            DependencyProperty.Register(nameof(MyLineClose), typeof(bool), typeof(GeometricCanvas),
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
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(GeometricCanvas),
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
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(GeometricCanvas),
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
            DependencyProperty.Register(nameof(MyIsEditing), typeof(bool), typeof(GeometricCanvas),
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
            DependencyProperty.Register(nameof(MyAnchorThumbSize), typeof(double), typeof(GeometricCanvas),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティと通知プロパティ

        public Bezier MyBezier { get; private set; }
        public GeometricCanvas()
        {
            MyBezier = new() { Stroke = Brushes.Crimson, StrokeThickness = 20.0, };

            Children.Add(MyBezier);
            Loaded += GeometricCanvas_Loaded;
        }

        private void GeometricCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            SetMyBindings();
            UpdateLayout();//必須、ここで実行しないと次のがEmptyでFixが実行されないので表示位置がずれる
            if (MyBezier.MyExternalBounds.IsEmpty == false) { FixGeometricLocate(); }
            MyBezier.MyAdorner.ThumbDragCompleted += MyAdorner_ThumbDragCompleted;
        }

        //頂点Thumbのドラッグ移動終了後にCanvas自身と図形の座標修正
        private void MyAdorner_ThumbDragCompleted(object arg1, Vector arg2)
        {

            FixCanvasLocate03();

        }

        private void FixGeometricLocate()
        {
            if (MyBezier.MyExternalBounds.IsEmpty) return;
            SetLeft(MyBezier, -MyBezier.MyExternalBounds.X);
            SetTop(MyBezier, -MyBezier.MyExternalBounds.Y);
        }
        private void SetMyBindings()
        {
            MyBezier.SetBinding(Bezier.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });
            MyBezier.SetBinding(Bezier.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeProperty) });
            MyBezier.SetBinding(Bezier.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty) });
            MyBezier.SetBinding(Bezier.MyIsEditingProperty, new Binding() { Source = this, Path = new PropertyPath(MyIsEditingProperty) });
            MyBezier.SetBinding(Bezier.MyAnchorThumbSizeProperty, new Binding() { Source = this, Path = new PropertyPath(MyAnchorThumbSizeProperty) });

            SetBinding(WidthProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectWidth() });
            SetBinding(HeightProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectHeight() });

        }
        //編集状態の切り替え、CanvasサイズのBinding変更と、Canvasと図形の座標修正
        public void ChangeBinding()
        {
            if (MyIsEditing)
            {
                SetBinding(WidthProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyAllBoundsProperty), Converter = new MyConverterRectWidth() });
                SetBinding(HeightProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyAllBoundsProperty), Converter = new MyConverterRectHeight() });
                var all = MyBezier.MyAllBounds;
                var ex = MyBezier.MyExternalBounds;

                Canvas.SetLeft(this, Canvas.GetLeft(this) + all.X - ex.X);
                Canvas.SetTop(this, Canvas.GetTop(this) + all.Y - ex.Y);

                Canvas.SetLeft(MyBezier, -all.X);
                Canvas.SetTop(MyBezier, -all.Y);
            }
            else
            {
                SetBinding(WidthProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectWidth() });
                SetBinding(HeightProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectHeight() });
                var ex = MyBezier.MyExternalBounds;
                var all = MyBezier.MyAllBounds;
                //var offset = VisualTreeHelper.GetOffset(this);

                Canvas.SetLeft(this, Canvas.GetLeft(this) + ex.X - all.X);
                Canvas.SetTop(this, Canvas.GetTop(this) + ex.Y - all.Y);
                Canvas.SetLeft(MyBezier, -ex.X);
                Canvas.SetTop(MyBezier, -ex.Y);
            }
        }

        
        /// <summary>
        /// 頂点Thumbのドラッグ移動終了後に実行する
        /// Canvas自身と図形の座標決定する
        /// </summary>
        public void FixCanvasLocate03()
        {
            var ex = MyBezier.MyExternalBounds;
            var pts = GetPointsRect(MyPoints);
            var left = Canvas.GetLeft(this); var top = Canvas.GetTop(this);
            var bLeft = Canvas.GetLeft(MyBezier); var bTop = Canvas.GetTop(MyBezier);

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
            Canvas.SetLeft(MyBezier, bx);
            Canvas.SetTop(MyBezier, by);

            if (pts.X != 0 || pts.Y != 0)
            {
                Fix0Point();
                MyBezier.MyAdorner.FixThumbsLocate();
            }
        }

        //protected override Size MeasureOverride(Size constraint)
        //{
        //    if (MyBezier.MyIsEditing)
        //    {
        //        //FixCanvasLocate01();
        //        //                FixCanvasLocate00();
        //    }
        //    return base.MeasureOverride(constraint);
        //}


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


    }

}
