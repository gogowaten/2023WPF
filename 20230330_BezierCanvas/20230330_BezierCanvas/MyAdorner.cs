using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using System;


namespace _20230330_BezierCanvas
{
    public class MyAdorner : Adorner
    {
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index)
        {
            return MyVisuals[index];
        }
        public VisualCollection MyVisuals { get; set; }

        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(MyAdorner),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //読み取り専用の依存関係プロパティ
        //WPF4.5入門 その43 「読み取り専用の依存関係プロパティ」 - かずきのBlog@hatena
        //        https://blog.okazuki.jp/entry/2014/08/18/083455
        /// <summary>
        /// 頂点Thumbすべてが収まるRect
        /// </summary>
        private static readonly DependencyPropertyKey MyVThumbsBoundsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyVThumbsBounds), typeof(Rect), typeof(MyAdorner),
                new FrameworkPropertyMetadata(Rect.Empty,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty MyVThumbsBoundsProperty =
            MyVThumbsBoundsPropertyKey.DependencyProperty;
        public Rect MyVThumbsBounds
        {
            get { return (Rect)GetValue(MyVThumbsBoundsProperty); }
            private set { SetValue(MyVThumbsBoundsPropertyKey, value); }
        }


        public double MyAnchorThumbSize
        {
            get { return (double)GetValue(MyAnchorThumbSizeProperty); }
            set { SetValue(MyAnchorThumbSizeProperty, value); }
        }
        public static readonly DependencyProperty MyAnchorThumbSizeProperty =
            DependencyProperty.Register(nameof(MyAnchorThumbSize), typeof(double), typeof(MyAdorner),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ


        public Canvas MyCanvas { get; set; } = new();
        public List<Thumb> MyThumbList { get; set; } = new();
        public Thumb? MyCurrentThumb { get; private set; }
        public Bezier MyTargetBezier { get; private set; }


        public MyAdorner(Bezier adornedElement) : base(adornedElement)
        {
            MyTargetBezier = adornedElement;
            MyVisuals = new VisualCollection(this) { MyCanvas };
            Loaded += MyAdorner_Loaded;
            SizeChanged += MyAdorner_SizeChanged;
        }


        private void MyAdorner_Loaded(object sender, RoutedEventArgs e)
        {
            SetBinding(MyPointsProperty, new Binding() { Source = MyTargetBezier, Path = new PropertyPath(Bezier.MyPointsProperty) });

            UpdateLayout();//Loadedで実行するとデザイナー画面で頂点ThumbRectが反映されるけど、アプリ起動直後にはそうなっていない、なんで？
            AddAnchorThumbs();

            var myrect = GetPointsRect(MyPoints);
            var rr = new Rect(myrect.X, myrect.Y, myrect.Size.Width + MyAnchorThumbSize, myrect.Height + MyAnchorThumbSize);
            MyCanvas.Arrange(rr);
            MyVThumbsBounds = rr;

        }
        private void AddAnchorThumbs()
        {
            for (int i = 0; i < MyPoints.Count; i++)
            {
                Thumb thumb = new()
                {
                    Width = MyAnchorThumbSize,
                    Height = MyAnchorThumbSize,
                    Opacity = 0.5,
                    Background = Brushes.Blue,
                };
                MyThumbList.Add(thumb);
                MyCanvas.Children.Add(thumb);
                //テスト用目印のため、背景色決め打ち
                MyCanvas.Background = new SolidColorBrush(Color.FromArgb(50, 100, 200, 0));
                Canvas.SetLeft(thumb, MyPoints[i].X);
                Canvas.SetTop(thumb, MyPoints[i].Y);
                thumb.DragDelta += Thumb_DragDelta;
                thumb.DragCompleted += Thumb_DragCompleted;
            }
        }


        //頂点ThumbとPointsのズレを修正
        public void FixThumbsLocate()
        {
            for (int i = 0; i < MyThumbList.Count; i++)
            {
                Point pp = MyPoints[i];
                Canvas.SetLeft(MyThumbList[i], pp.X);
                Canvas.SetTop(MyThumbList[i], pp.Y);
            }

        }
        //起動時1回発生、デザイナー画面で頂点ThumbのRectが更新されない
        //ArrangeOverrideとMeasureOverrideは更新される
        private void MyAdorner_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //UpDateCanvasAndAnchorThumbsBounds();
        }

        //起動時3回発生する
        //protected override Size ArrangeOverride(Size finalSize)
        //{
        //    UpDateCanvasAndAnchorThumbsBounds();
        //    return base.ArrangeOverride(finalSize);
        //}
        //起動時2回発生、ArrangeOverrideと結果に変化なしなので、効率がいい？
        protected override Size MeasureOverride(Size constraint)
        {
            UpDateCanvasAndAnchorThumbsBounds();
            return base.MeasureOverride(constraint);
        }
        private void UpDateCanvasAndAnchorThumbsBounds()
        {
            //CanvasのサイズをArrange()で指定する、サイズは頂点Thumbが収まるサイズ
            Rect ptsRect = GetPointsRect(MyPoints);
            //座標もここで指定できそうなんだけど、なぜか指定値の半分になるのでここではしていない
            var r = new Rect(new Size(ptsRect.Size.Width + MyAnchorThumbSize, ptsRect.Size.Height + MyAnchorThumbSize));
            MyCanvas.Arrange(r);
            MyVThumbsBounds = r;
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

        public void Fix0Point()
        {
            Rect r = GetPointsRect(MyPoints);
            for (int i = 0; i < MyPoints.Count; i++)
            {
                Point pp = MyPoints[i];
                MyPoints[i] = new Point(pp.X - r.Left, pp.Y - r.Top);
            }
        }


        //イベント、頂点Thumbのドラッグ移動終了後に、そのことを通知するため
        public event Action<object, Vector>? ThumbDragCompleted;
        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            Thumb t = (Thumb)sender;
            double x = Canvas.GetLeft(t);
            double y = Canvas.GetTop(t);
            ThumbDragCompleted?.Invoke(this, new Vector(x, y));
            //var r = GetPointsRect(MyPoints);
            //if (r.X != 0 || r.Y != 0) { Fix0Point(); FixThumbsLocate(); }
        }

        //頂点Thumbのドラッグ移動、Pointsも書き換える
        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb thumb)
            {
                MyCurrentThumb = thumb;
                int ii = MyThumbList.IndexOf(thumb);
                Point pp = MyPoints[ii];
                var xx = pp.X + e.HorizontalChange;
                var yy = pp.Y + e.VerticalChange;

                MyPoints[ii] = new Point(xx, yy);
                Canvas.SetLeft(thumb, xx);
                Canvas.SetTop(thumb, yy);

            }
        }
    }

}
