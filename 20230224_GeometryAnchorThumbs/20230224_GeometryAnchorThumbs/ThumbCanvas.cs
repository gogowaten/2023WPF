using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _20230224_GeometryAnchorThumbs
{

    public class ThumbCanvas : Canvas
    {
        public PointCollection Points { get; set; } = new();
        public ObservableCollection<Point> ObPoints { get; set; } = new();
        public List<TThumb> Thumbs { get; set; } = new();
        public ThumbCanvas()
        {

        }

        public ThumbCanvas(PointCollection points)
        {
            Points = points;
            ObPoints = new(points);
            foreach (Point item in points)
            {
                TThumb thumb = new(item);
                thumb.DragDelta += Thumb_DragDelta;
                Thumbs.Add(thumb);
                Children.Add(thumb);
            }
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (sender is TThumb thumb)
            {
                thumb.X += e.HorizontalChange;
                thumb.Y += e.VerticalChange;
                Points[Thumbs.IndexOf(thumb)] = new Point(thumb.X, thumb.Y);
            }
        }
    }

    public class ObThumbCanvas : Canvas
    {

        public ObservableCollection<Point> MyPoints
        {
            get { return (ObservableCollection<Point>)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(ObservableCollection<Point>), typeof(ObThumbCanvas),
                new FrameworkPropertyMetadata(new ObservableCollection<Point>(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public List<TThumb> MyThumbs { get; set; } = new();

        public ObThumbCanvas()
        {
            MyPoints.CollectionChanged += MyPoints_CollectionChanged;
        }
        public ObThumbCanvas(ObservableCollection<Point> points):this()
        {
            foreach (var item in points)
            {
                MyPoints = points;
                TThumb thumb = new(item);
                thumb.DragDelta += Thumb_DragDelta;
                SetLeft(thumb, item.X);
                SetTop(thumb, item.Y);
                MyThumbs.Add(thumb);
                Children.Add(thumb);
            }
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if(sender is TThumb thumb)
            {
                thumb.X += e.HorizontalChange;
                thumb.Y += e.VerticalChange;
                int ii = MyThumbs.IndexOf(thumb);
                MyPoints[ii]=new Point(thumb.X, thumb.Y);   
            }
        }


        private void MyPoints_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            var neko = 0;
        }
    }

}
