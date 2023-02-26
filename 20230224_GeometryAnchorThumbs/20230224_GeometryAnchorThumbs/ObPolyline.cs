using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _20230224_GeometryAnchorThumbs
{
    public class ObPolyline : Shape
    {

        public ObservableCollection<Point> MyObPoints
        {
            get { return (ObservableCollection<Point>)GetValue(MyObPointsProperty); }
            set { SetValue(MyObPointsProperty, value); }
        }
        public static readonly DependencyProperty MyObPointsProperty =
            DependencyProperty.Register(nameof(MyObPoints), typeof(ObservableCollection<Point>), typeof(ObPolyline),
                new FrameworkPropertyMetadata(new ObservableCollection<Point>(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        //public ObservableCollection<Point> MyObPoints { get; set; } = new();

        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geo = new() { FillRule = FillRule.Nonzero };
                using (var context = geo.Open())
                {
                    MyDraw(context);
                }
                geo.Freeze();
                return geo;
            }
        }
        private void MyDraw(StreamGeometryContext context)
        {
            if (MyObPoints.Count < 2) { return; }
            context.BeginFigure(MyObPoints[0], false, false);
            for (int i = 1; i < MyObPoints.Count; i++)
            {
                context.LineTo(MyObPoints[i], true, false);
            }
        }
        public ObPolyline()
        {
            MyObPoints.Add(new Point(0, 0));
            MyObPoints.Add(new Point(100, 0));
            Stroke = Brushes.Red;
            StrokeThickness = 10;
            MyObPoints.CollectionChanged += MyObPoints_CollectionChanged;
        }

        //Point変化時に描画更新、これをしないとPointsだけ変化して見た目が変化しない
        private void MyObPoints_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var neko = 0;
            //InvalidateVisual();//ok
            //InvalidateArrange();//none
            InvalidateMeasure();//ok
            //InvalidateProperty(MyObPointsProperty);//none
            neko = 1;
        }
    }
}
