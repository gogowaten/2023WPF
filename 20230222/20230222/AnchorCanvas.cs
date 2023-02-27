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
    //:Canvas
    //  List<AnchorThumb>
    //  PolyBezierArrowLine2
    public class AnchorCanvas : Canvas
    {
        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(AnchorCanvas),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //[TypeConverter(typeof(MyTypeConverterPoints))]
        //public ObservableCollection<Point> MyPoints
        //{
        //    get { return (ObservableCollection<Point>)GetValue(MyPointsProperty); }
        //    set { SetValue(MyPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointsProperty =
        //    DependencyProperty.Register(nameof(MyPoints), typeof(ObservableCollection<Point>), typeof(AnchorCanvas),
        //        new FrameworkPropertyMetadata(new ObservableCollection<Point>(),
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure|
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public ObservableCollection<Point> MyPoints { get; set; } = new();
        public ObservableCollection<Point> MyDataPoints { get; set; } = new();

        //public List<AnchorThumb> MyThumbs { get; set; } = new();
        public ObservableCollection<AnchorThumb> MyThumbs { get; set; } = new();

        public Polyline MyPolyLine { get; set; } = new();
        public AnchorThumb? CurrentThumb;

        public AnchorCanvas()
        {

            //MyPolyLine.Points = MyPoints;
            MyPolyLine.SetBinding(Polyline.PointsProperty, new Binding()
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                //Converter = new ConverterPcOb(),
                Path = new PropertyPath(MyPointsProperty),
            });

            SetBinding(MyPointsProperty, new Binding(nameof(MyDataPoints))
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Converter = new ConverterPcOb()
            });


            MyPolyLine.Stroke = Brushes.Red;
            MyPolyLine.StrokeThickness = 20;
            Children.Add(MyPolyLine);

            SetBinding(WidthProperty, new Binding() { Source = MyPolyLine, Path = new PropertyPath(ActualWidthProperty) });
            SetBinding(HeightProperty, new Binding() { Source = MyPolyLine, Path = new PropertyPath(ActualHeightProperty) });

            //MyPoints.CollectionChanged += (a, b) => { var neko = 0; };
            MyDataPoints.CollectionChanged += (a, b) => { var neko = 0; };
            Loaded += AnchorCanvas_Loaded;

            //MyDataPoints = MyPoints;

        }



        private void AnchorCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in MyPoints)
            {
                AnchorThumb at = new(item);
                at.DragDelta += At_DragDelta;
                at.MouseLeftButtonDown += At_MouseLeftButtonDown;
                MyThumbs.Add(at);
                Children.Add(at);
            }


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
            }

        }


    }
    public class ConverterPcOb : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<Point> points = (ObservableCollection<Point>)value;
            return new PointCollection(points);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PointCollection points= (PointCollection)value;
            return new ObservableCollection<Point>(points);
        }
    }
}
