using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Data;

namespace _20230319_BezierSize3
{
    class MyAdorner : Adorner
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
        //public BeCanvas MyBeCanvas { get; private set; }
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
                MyCanvas.Background = new SolidColorBrush(Color.FromArgb(50, 100, 100, 100));
                Canvas.SetLeft(thumb, MyPoints[i].X);
                Canvas.SetTop(thumb, MyPoints[i].Y);
                thumb.DragDelta += Thumb_DragDelta;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {

            var desbounds = VisualTreeHelper.GetDescendantBounds(MyCanvas);
            var myrect = GetPointsRect(MyPoints);

            var rr = new Rect(myrect.X, myrect.Y, myrect.Size.Width + ThumbSize, myrect.Height + ThumbSize);
            if (desbounds.IsEmpty)
            {
                MyCanvas.Arrange(new Rect(finalSize));
            }
            else
            {
                MyCanvas.Arrange(rr);
                //MyCanvas.Arrange(desbounds);
            }

            return base.ArrangeOverride(finalSize);
        }
        private Rect GetPointsRect(PointCollection pt)
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
        //protected override Size MeasureOverride(Size constraint)
        //{
        //    var desbounds = VisualTreeHelper.GetDescendantBounds(MyCanvas);
        //    if (desbounds.IsEmpty)
        //    {
        //        //MyCanvas.Arrange(new Rect(constraint));
        //    }
        //    else
        //    {
        //        MyCanvas.Arrange(desbounds);
        //    }

        //    return base.MeasureOverride(constraint);
        //}
        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb thumb)
            {
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
