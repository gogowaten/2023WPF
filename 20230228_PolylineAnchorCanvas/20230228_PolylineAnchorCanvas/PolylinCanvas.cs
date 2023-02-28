using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

//
namespace _20230228_PolylineAnchorCanvas
{
    class PolylinCanvas : Canvas
    {
        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(PolylinCanvas),
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
            DependencyProperty.Register(nameof(MyAnchorVisible), typeof(Visibility), typeof(PolylinCanvas),
                new FrameworkPropertyMetadata(Visibility.Collapsed,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public ObservableCollection<AnchorThumb> MyAnchorThumbs { get; set; } = new();

        public Polyline MyShape { get; set; }
        public ContextMenu MyAnchorMenu { get; set; } = new();
        public ContextMenu MyMenu { get; set; } = new();
        public AnchorThumb? MyCurrentAnchorThumb { get; set; }
        public int MyDragPointIndex;
        public Point MyClickedPoint;

        public PolylinCanvas()
        {
            this.Background = Brushes.Transparent;
            MyShape = new Polyline();
            Children.Add(MyShape);
            MyShape.Stroke = Brushes.MediumAquamarine;
            MyShape.StrokeThickness = 20;

            Loaded += PolylinCanvas_Loaded;

            SetBinding(WidthProperty, new Binding() { Source = MyShape, Path = new PropertyPath(ActualWidthProperty) });
            SetBinding(HeightProperty, new Binding() { Source = MyShape, Path = new PropertyPath(ActualHeightProperty) });

            MenuItem item = new() { Header = "削除" };
            item.Click += (o, e) => { RemovePoint(MyCurrentAnchorThumb); };
            MyAnchorMenu.Items.Add(item);

            this.ContextMenu = MyMenu;
            item = new() { Header = "ここに追加" };
            item.Click += (o, e) => { AddPoint(MyClickedPoint); };
            MyMenu.Items.Add(item);
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            MyClickedPoint = Mouse.GetPosition(this);
        }


        private void PolylinCanvas_Loaded(object sender, RoutedEventArgs e)
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
            RemovePoint(MyDragPointIndex);
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
            thumb.SetBinding(VisibilityProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyAnchorVisibleProperty)
            });
            thumb.ContextMenu = MyAnchorMenu;
            thumb.PreviewMouseDown += Thumb_PreviewMouseDown;
        }

        private void Thumb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is AnchorThumb thumb)
            {
                MyCurrentAnchorThumb = thumb;
                MyDragPointIndex = MyAnchorThumbs.IndexOf(thumb);
            }
            else { MyDragPointIndex = -1; }
        }


        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is AnchorThumb thumb)
            {
                thumb.X += e.HorizontalChange;
                thumb.Y += e.VerticalChange;
                MyPoints[MyDragPointIndex] = new Point(thumb.X, thumb.Y);
            }
        }
    }
}
