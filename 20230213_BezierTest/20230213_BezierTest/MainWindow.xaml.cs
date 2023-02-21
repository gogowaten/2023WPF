using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Specialized;


namespace _20230213_BezierTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private ThumbsCanvas MyThumbsCanvas;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ////MyTTBezier3.Points[1] = new Point(100, 100);
            ////MyTTPolyLine.MyPP[1]=new Point(110, 210);
            //var points = MyTTLine.MyPoints;
            //var data = MyTTLine.MyData;
            //var linepoint = MyTTLine.MyLine.Points;
            Point p0 = new(200, 200);
            Point p1 = new(300, 200);
            //MyTTLine.MyData.PointCollection[0] = p0;

            var pcolletion = MyTTArrow3.MyData.PointCollection;
            var arrowP = MyTTArrow3.MyArrow3.MyPoints;
            var ttPoints = MyTTArrow3.MyPoints;
            var dataThick = MyTTArrow3.MyData.StrokeThickness;
            var arrowThick = MyTTArrow3.MyArrow3.StrokeThickness;
            var ttThick = MyTTArrow3.StrokeThickness;

            //MyTTArrow3.MyData.PointCollection[0] = p0;
            //MyTTArrow3.MyPoints[0] = p0;
            //MyTTArrow3.MyArrow3.MyPoints[0] = p0;
            //MyTTArrow3.MyData.StrokeThickness = 20;
            //MyTTArrow3.StrokeThickness= 20;
            MyTTArrow3.MyArrow3.StrokeThickness= 20;
        }
    }
    public class ThumbsCanvas : Canvas
    {
        public ObservableCollection<Point> Points { get; set; } = new();
        public List<TThumb> Thumbs { get; set; } = new();
        public ThumbsCanvas()
        {
            Points.CollectionChanged += Points_CollectionChanged;
        }

        private void Points_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems?[0] is Point p)
                    {
                        AddThumb(p);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }

        }
        private void AddThumb(Point point)
        {
            TThumb tt = new();
            tt.Width = 20;
            tt.Height = 20;
            Canvas.SetLeft(tt, point.X);
            Canvas.SetTop(tt, point.Y);
            this.Children.Add(tt);
            tt.DragDelta += Tt_DragDelta;
        }

        private void Tt_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (sender is TThumb tt)
            {
                double x = GetLeft(tt) + e.HorizontalChange;
                double y = GetTop(tt) + e.VerticalChange;
                SetLeft(tt, x);
                SetTop(tt, y);
            }
        }
    }
}
