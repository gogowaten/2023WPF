using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

namespace _20230224_GeometryAnchorThumbs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PointCollection MyPoints { get; set; } = new();
        public PointCollection MyPoints2 { get; set; } = new();
        public PointCollection MyPoints3 { get; set; } = new();

        public ObPolyCanvas MyObPolyCanvas { get; set; } = new();

        private List<TThumb> MyThumbList = new();
        private ObThumbCanvas MyObThumbCanvas { get; set; } = new();

        public ObservableCollection<Point> MyObPoints
        {
            get { return (ObservableCollection<Point>)GetValue(MyObPointsProperty); }
            set { SetValue(MyObPointsProperty, value); }
        }
        public static readonly DependencyProperty MyObPointsProperty =
            DependencyProperty.Register(nameof(MyObPoints), typeof(ObservableCollection<Point>), typeof(MainWindow),
                new FrameworkPropertyMetadata(new ObservableCollection<Point>(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public MainWindow()
        {
            InitializeComponent();
            MyObPoints.Add(new Point(0, 0)); MyObPoints.Add(new Point(100, 20));
            MyObThumbCanvas = new ObThumbCanvas(MyObPoints);
            MyCanvas.Children.Add(MyObThumbCanvas);
            //SetBinding(MyObPointsProperty, new Binding(nameof(MyObThumbCanvas.MyPoints))
            //{
            //    Source = MyObThumbCanvas,
            //});
            //MyObPolyCanvas.SetBinding(ObPolyCanvas.MyPointsProperty, new Binding()
            //{
            //    Source = this,
            //    Path = new PropertyPath(MyObPointsProperty),
            //    Mode=BindingMode.TwoWay,
            //});
            //Test1();
        }

        private void Test1()
        {
            MyObPoints.Add(new Point(0, 0));
            MyObPoints.Add(new Point(100, 20));
            MyCanvas.Children.Add(MyObPolyCanvas);
            SetBinding(MyObPointsProperty, new Binding()
            {
                Source = MyObPolyCanvas,
                Path = new PropertyPath(ObPolyCanvas.MyPointsProperty)
            });
        }
        private void MyObPoints_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if (e.NewItems?[0] is Point p) { MyPoints.Add(p); }
                    break;
            }
        }

        private void AnchorTest()
        {
            MyPoints.Add(new(20, 100));
            MyPoints.Add(new(100, 10));
            //foreach (var item in MyPoints)
            //{
            //    MyThumbs.Add(new TThumb(item));
            //}

            //MyItemsControl.DataContext = MyPoints;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyObPoints.Add(new Point(100, 100));
            MyObPolyCanvas.MyPoints.Add(new Point(300, 0));
            var tcpoints = MyObThumbCanvas.MyPoints;
            var points = MyObPoints;
            //Test1Test();
        }
        private void Test1Test()
        {
            MyObPoints.Add(new Point(50, 100));
            MyObPolyCanvas.MyPoints.Add(new Point(220, 180));
            MyObPolyCanvas.MyPolyline.MyObPoints.Add(new Point(330, 50));
        }

        private void TThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (sender is TThumb tt)
            {
                double x = tt.X + e.HorizontalChange;
                double y = tt.Y + e.VerticalChange;
                tt.X = x;
                tt.Y = y;
                MyPoints[MyThumbList.IndexOf(tt)] = new Point(x, y);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var item in MyPoints)
            {
                TThumb tt = new(item);
                tt.MouseLeftButtonDown += Tt_MouseLeftButtonDown;
                tt.DragDelta += TThumb_DragDelta;

                MyCanvas.Children.Add(tt);
                MyThumbList.Add(tt);
            }
        }

        private void Tt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (sender is TThumb tt) { CurrentThumb = tt; }
        }

        private void TThumb_DragDelta_1(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (sender is TThumb tt)
            {
                tt.X += e.HorizontalChange;
                tt.Y += e.VerticalChange;
            }
        }
    }

    public class MyConverterObPoint : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<Point> obpc = (ObservableCollection<Point>)value;
            return new PointCollection(obpc);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PointCollection pc = (PointCollection)value;
            return new ObservableCollection<Point>(pc);

        }
    }
}
