using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using System.Security.Policy;

namespace _20230322_BezierSize
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

        #endregion 依存関係プロパティ


        public Canvas MyCanvas { get; set; } = new();
        public List<Thumb> MyThumbList { get; set; } = new();
        public Thumb? MyCurrentThumb { get; private set; }
        public Bezier MyTargetBezier { get; private set; }
        public double ThumbSize { get; set; } = 20.0;
        public MyAdorner(Bezier adornedElement) : base(adornedElement)
        {
            MyTargetBezier = adornedElement;
            MyVisuals = new VisualCollection(this) { MyCanvas };
            Loaded += MyAdorner_Loaded;
        }

        private void MyAdorner_Loaded(object sender, RoutedEventArgs e)
        {
            SetBinding(MyPointsProperty, new Binding() { Source = MyTargetBezier, Path = new PropertyPath(Bezier.MyPointsProperty) });

            for (int i = 0; i < MyPoints.Count; i++)
            {
                Thumb thumb = new()
                {
                    Width = ThumbSize,
                    Height = ThumbSize,
                    Opacity = 0.5,
                    Background = Brushes.Blue,
                };
                MyThumbList.Add(thumb);
                MyCanvas.Children.Add(thumb);
                MyCanvas.Background = new SolidColorBrush(Color.FromArgb(50, 100, 200, 0));
                Canvas.SetLeft(thumb, MyPoints[i].X);
                Canvas.SetTop(thumb, MyPoints[i].Y);
                thumb.DragDelta += Thumb_DragDelta;
            }
            var myrect = GetPointsRect(MyPoints);
            var rr = new Rect(myrect.X, myrect.Y, myrect.Size.Width + ThumbSize, myrect.Height + ThumbSize);
            var rrr = new Rect(rr.Size);

            MyCanvas.Arrange(rr);
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
            //var r = GetPointsRect(MyPoints);
            //MyCanvas.Arrange(r);
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            //頂点Thumbが収まるRectを取得、これをCanvasのRectに指定する
            //このRect取得はGetDescendantBounds(MyCanvas)できそうだけど
            //これだと今のCanvasのサイズも含めてしまうので、一度広がったCanvasがそのままになってしまうので
            //手動のGetPointsRectにて取得している
            var myrect = GetPointsRect(MyPoints);
            var rr = new Rect(myrect.X, myrect.Y, myrect.Size.Width + ThumbSize, myrect.Height + ThumbSize);
            var rrr = new Rect(rr.Size);
            MyCanvas.Arrange(rrr);

            return base.ArrangeOverride(finalSize);
        }
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
