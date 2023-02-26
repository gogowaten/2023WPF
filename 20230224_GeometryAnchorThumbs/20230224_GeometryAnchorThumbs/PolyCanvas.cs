using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Data;

namespace _20230224_GeometryAnchorThumbs
{
    public class PolyCanvas : Canvas
    {
        public Polyline MyPolyline { get; set; } = new();
        public PointCollection MyPoints { get; set; } = new();
        public PolyCanvas()
        {
            MyPoints.Add(new(0, 0));
            MyPoints.Add(new(100, 50));
            MyPolyline = new()
            {
                Stroke = Brushes.Crimson,
                StrokeThickness = 10,
                Points=MyPoints,
            };
            Children.Add(MyPolyline);
        }
    }
    public class ObPolyCanvas : Canvas
    {
        public ObPolyline MyPolyline { get; set; } = new();

        public ObservableCollection<Point> MyPoints
        {
            get { return (ObservableCollection<Point>)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(ObservableCollection<Point>), typeof(ObPolyCanvas),
                new FrameworkPropertyMetadata(new ObservableCollection<Point>(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure|
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ObPolyCanvas()
        {
            //MyPolyline.SetBinding(ObPolyline.MyObPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });
            SetBinding(MyPointsProperty,new Binding() { Source=this,Path = new PropertyPath(ObPolyline.MyObPointsProperty) });
            Children.Add(MyPolyline);
        }
    }
}
