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


//失敗
//Pointsを全部依存プロパティにしてみたけど、要素変化でのイベント発生しないので
//Point追加と同時にアンカーThumbの追加ができない
namespace _20230222
{
    //:Canvas
    //  List<MyCurrentAnchorThumb>
    //  PolyBezierArrowLine2
    public class AnchorCanvas : Canvas
    {
        //public PointCollection Points
        //{
        //    get { return (PointCollection)GetValue(PointsProperty); }
        //    set { SetValue(PointsProperty, value); }
        //}
        //public static readonly DependencyProperty PointsProperty =
        //    DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(AnchorCanvas),
        //        new FrameworkPropertyMetadata(new PointCollection(),
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [TypeConverter(typeof(MyTypeConverterPoints))]
        public ObservableCollection<Point> MyPoints
        {
            get { return (ObservableCollection<Point>)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(ObservableCollection<Point>), typeof(AnchorCanvas),
                new FrameworkPropertyMetadata(new ObservableCollection<Point>(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ObservableCollection<Point> MyDataObPoints { get; set; } = new();
        public ObservableCollection<AnchorThumb> MyThumbs { get; set; } = new();

        public PolyBezierArrowLine2 MyShapePoly { get; set; } = new();
        public AnchorThumb? CurrentThumb;

        public AnchorCanvas()
        {
            MyShapePoly.Stroke = Brushes.Red;
            MyShapePoly.StrokeThickness = 20;
            Children.Add(MyShapePoly);

            SetBinding(WidthProperty, new Binding() { Source = MyShapePoly, Path = new PropertyPath(ActualWidthProperty) });
            SetBinding(HeightProperty, new Binding() { Source = MyShapePoly, Path = new PropertyPath(ActualHeightProperty) });

            //MainWindowでPoint追加しても、下記のイベント発生しない
            MyPoints.CollectionChanged += (a, b) => { var neko = 0; };
            MyDataObPoints.CollectionChanged += (a, b) => { var neko = 0; };
            MyShapePoly.Points.CollectionChanged += (a, b) => { var neko = 0; };
            Loaded += AnchorCanvas_Loaded;

            //関連付けは、ここでBindingでも、Loadedイベントで=イコールで関連付けても同じ
            //相変わらず要素変化でのイベント発生しないのでアンカーThumbの追加ができない
            //MyShapePoly.SetBinding(PolyBezierArrowLine2.PointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });
            //SetBinding(MyPointsProperty, new Binding(nameof(MyDataObPoints)) { Source = this, });

        }



        private void AnchorCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            MyShapePoly.Points = MyPoints;
            MyDataObPoints = MyPoints;

            foreach (var item in MyPoints)
            {
                AnchorThumb at = new(item);
                at.DragDelta += At_DragDelta;
                at.MouseLeftButtonDown += At_MouseLeftButtonDown;
                MyThumbs.Add(at);
                Children.Add(at);
                //MyDataObPoints.Add(item);
            }

            //MyShapePoly.SetBinding(Polyline.PointsProperty, new Binding(nameof(MyDataObPoints))
            //{
            //    Source = this,
            //    Mode = BindingMode.TwoWay,
            //    Converter = new ConverterObToPc(),

            //});

        }

        private void At_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is AnchorThumb thumb) { CurrentThumb = thumb; }
        }

        private void At_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is AnchorThumb at)
            {
                int ii = MyThumbs.IndexOf(at);
                at.X += e.HorizontalChange;
                at.Y += e.VerticalChange;
                MyPoints[ii] = new Point(at.X, at.Y);
                MyShapePoly.InvalidateVisual();
            }

        }


    }
    public class ConverterObToPc : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<Point> points = (ObservableCollection<Point>)value;
            return new PointCollection(points);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PointCollection points = (PointCollection)value;
            return new ObservableCollection<Point>(points);
        }
    }
}
