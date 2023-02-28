using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.ComponentModel;
using System.Globalization;

namespace _20230222
{
    public class AnchorCanvas2 : Canvas
    {

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(AnchorCanvas2),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Visibility MyAnchorVisible
        {
            get { return (Visibility)GetValue(MyAnchorVisibleProperty); }
            set { SetValue(MyAnchorVisibleProperty, value); }
        }
        public static readonly DependencyProperty MyAnchorVisibleProperty =
            DependencyProperty.Register(nameof(MyAnchorVisible), typeof(Visibility), typeof(AnchorCanvas2),
                new FrameworkPropertyMetadata(Visibility.Collapsed,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public ObservableCollection<AnchorThumb> MyAnchorThumbs { get; set; } = new();

        public Polyline MyShape { get; set; }
        public ContextMenu MyMenu { get; set; } = new();
        public AnchorThumb? MyCurrentAnchorThumb { get; set; }
        public int DragPointIndex;

        public AnchorCanvas2()
        {
            MyShape = new Polyline();
            Children.Add(MyShape);
            MyShape.Stroke = Brushes.MediumAquamarine;
            MyShape.StrokeThickness = 20;

            Loaded += AnchorCanvas2_Loaded;

            SetBinding(WidthProperty, new Binding() { Source = MyShape, Path = new PropertyPath(ActualWidthProperty) });
            SetBinding(HeightProperty, new Binding() { Source = MyShape, Path = new PropertyPath(ActualHeightProperty) });

            MenuItem item = new() { Header = "削除" };
            item.Click += (o, e) => { RemovePoint(MyCurrentAnchorThumb); };
            MyMenu.Items.Add(item);
            MyMenu.Items.Add(new MenuItem() { Header = "test2" });
        }

        private void AnchorCanvas2_Loaded(object sender, RoutedEventArgs e)
        {
            MyShape.Points = MyPoints;
            foreach (var item in MyPoints)
            {
                AddAnchorThumb(item);
            }
        }

        public void AddPoint(Point point)
        {
            InsertPoint(point, MyPoints.Count);
        }
        public void RemovePoint(int pointIndex)
        {
            if (pointIndex < 0) { return; }
            MyPoints.RemoveAt(pointIndex);
            AnchorThumb thumb = MyAnchorThumbs[pointIndex];
            MyAnchorThumbs.Remove(thumb);
            Children.Remove(thumb);
            MyCurrentAnchorThumb = null;
        }
        public void RemovePoint(AnchorThumb? thumb)
        {
            if (thumb == null) { return; }
            RemovePoint(DragPointIndex);
        }

        public void InsertPoint(Point point, int i)
        {
            MyPoints.Insert(i, point);
            InsertAnchorThumb(point, i);
        }
        private void AddAnchorThumb(Point point)
        {
            InsertAnchorThumb(point, MyAnchorThumbs.Count);
        }
        private void InsertAnchorThumb(Point point, int i)
        {
            AnchorThumb thumb = new(point);
            MyAnchorThumbs.Insert(i, thumb);
            Children.Add(thumb);
            thumb.DragDelta += Thumb_DragDelta;
            thumb.SetBinding(VisibilityProperty, new Binding() { Source = this, Path = new PropertyPath(MyAnchorVisibleProperty) });
            thumb.ContextMenu = MyMenu;
            thumb.PreviewMouseDown += Thumb_PreviewMouseDown;
        }

        private void Thumb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is AnchorThumb thumb)
            {
                MyCurrentAnchorThumb = thumb;
                DragPointIndex = MyAnchorThumbs.IndexOf(thumb);
            }
            else { DragPointIndex = -1; }
        }


        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is AnchorThumb thumb)
            {
                thumb.X += e.HorizontalChange;
                thumb.Y += e.VerticalChange;
                MyPoints[DragPointIndex] = new Point(thumb.X, thumb.Y);
            }
        }
    }
}
