using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Data;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace _20230213_BezierTest
{
    [TypeConverter(typeof(MyConverter))]
    public class ObservarablePointsLine : ContentControl
    {

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(ObservarablePointsLine),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        private readonly Polyline polyline = new();
        public ObservarablePointsLine()
        {
            
            polyline.SetBinding(Polyline.PointsProperty, new Binding(nameof(MyPoints))
            {
                Source = this,
                Mode = BindingMode.TwoWay
            });
            this.Content = polyline;
            //polyline.Points = Points;
            polyline.StrokeThickness = 1;
            polyline.Stroke = Brushes.Gray;
        }

    }
    public class TTPolyLine2 : Thumb
    {

        #region DependencyProperty


        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TTPolyLine2),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private static void CallBackPoints(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TTPolyLine2 ttpl)
            {
                var neko = e.NewValue;
            }
        }

        public PointCollection MyPCollection
        {
            get { return (PointCollection)GetValue(MyPCollectionProperty); }
            set { SetValue(MyPCollectionProperty, value); }
        }
        public static readonly DependencyProperty MyPCollectionProperty =
            DependencyProperty.Register(nameof(MyPCollection),
                typeof(PointCollection),
                typeof(TTPolyLine2),
                new PropertyMetadata(null));


        public double MyThickness
        {
            get { return (double)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register(nameof(MyThickness), typeof(double), typeof(TTPolyLine2),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush MyBrush
        {
            get { return (Brush)GetValue(MyBrushProperty); }
            set { SetValue(MyBrushProperty, value); }
        }
        public static readonly DependencyProperty MyBrushProperty =
            DependencyProperty.Register(nameof(MyBrush), typeof(Brush), typeof(TTPolyLine2), new PropertyMetadata(Brushes.DodgerBlue));
        #endregion DependencyProperty
        public Data MyData { get; set; }
        private readonly List<TThumb> Thumbs = new();
        private readonly Polyline MyLine = new();
        public TTPolyLine2()
        {
            MyData = new();

            SetTemplate();
            SetBinding(MyPointsProperty, new Binding(nameof(MyData.Points)) { Source = MyData, Mode = BindingMode.TwoWay });

        }



        private void SetTemplate()
        {
            FrameworkElementFactory bGrid = new(typeof(Grid));
            FrameworkElementFactory factory = new(typeof(PolyLineCanvas2), "name");

            bGrid.AppendChild(factory);
            this.Template = new() { VisualTree = factory };
            this.ApplyTemplate();

        }
    }





    public class TTPolyLine : Thumb
    {
        #region Property

        PolyLineCanvas2 MyTemplate { get; set; }
        public Data MyData { get; set; }

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TTPolyLine),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyThickness
        {
            get { return (double)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register(nameof(MyThickness), typeof(double), typeof(TTPolyLine),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush MyBrush
        {
            get { return (Brush)GetValue(MyBrushProperty); }
            set { SetValue(MyBrushProperty, value); }
        }
        public static readonly DependencyProperty MyBrushProperty =
            DependencyProperty.Register(nameof(MyBrush), typeof(Brush), typeof(TTPolyLine), new PropertyMetadata(Brushes.MediumAquamarine));
        #endregion Property
        public TTPolyLine()
        {
            MyData = new();
            SetTemplate();
            MyTemplate = (PolyLineCanvas2)this.Template.FindName("name", this);
            MyTemplate.SetBinding(PolyLineCanvas2.MyPointsProperty, new Binding() { Source = this, Mode = BindingMode.TwoWay, Path = new PropertyPath(MyPointsProperty) });

            SetBinding(MyPointsProperty, new Binding(nameof(MyData.Points)) { Source = MyData, Mode = BindingMode.TwoWay });

        }
        private void SetTemplate()
        {
            FrameworkElementFactory bGrid = new(typeof(Grid));
            FrameworkElementFactory factory = new(typeof(PolyLineCanvas2), "name");

            bGrid.AppendChild(factory);
            this.Template = new() { VisualTree = factory };
            this.ApplyTemplate();

        }

    }


    public class PolyLineCanvas2 : Canvas
    {
        #region Property


        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(PolyLineCanvas2),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Brush MyBrush
        {
            get { return (Brush)GetValue(MyBrushProperty); }
            set { SetValue(MyBrushProperty, value); }
        }
        public static readonly DependencyProperty MyBrushProperty =
            DependencyProperty.Register(nameof(MyBrush), typeof(Brush), typeof(PolyLineCanvas2), new PropertyMetadata(Brushes.MediumAquamarine));

        public double MyThickness
        {
            get { return (double)GetValue(MyThicknessProperty); }
            set { SetValue(MyThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyThicknessProperty =
            DependencyProperty.Register(nameof(MyThickness), typeof(double), typeof(PolyLineCanvas2),
                new FrameworkPropertyMetadata(1.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public readonly List<TThumb> MyThumbs = new();
        //public PointCollection MyPoints = new();

        public Polyline MyPolyline;
        //クリックしたThumb
        public TThumb? MyCurrentThumb { get; private set; }
        public bool IsThumbVisible = true;
        #endregion Property
        public PolyLineCanvas2()
        {
            MyPoints = new();
            MyPolyline = new()
            {
                Stroke = MyBrush,
                StrokeThickness = MyThickness,
                Points = MyPoints,
            };
            MyPolyline.SetBinding(Polyline.PointsProperty, new Binding(nameof(MyPoints)) { Source = this });
            SetBinding(WidthProperty, new Binding()
            {
                Source = MyPolyline,
                Path = new PropertyPath(Polyline.ActualWidthProperty),
            });
            SetBinding(HeightProperty, new Binding()
            {
                Source = MyPolyline,
                Path = new PropertyPath(Polyline.ActualHeightProperty),
            });

            //MyPoints.Add(new(0, 0));
            //MyPoints.Add(new(100, 100));
            //MyPoints.Add(new(200, 20));
            this.Children.Add(MyPolyline);
        }
        public PolyLineCanvas2(Brush stroke, double thickness)
        {
            MyPolyline = new()
            {
                Stroke = stroke,
                StrokeThickness = thickness,
                Points = MyPoints
            };
            this.Children.Add(MyPolyline);
        }

        public void AddPoint(Point p)
        {
            TThumb t = new() { Width = 20, Height = 20 };
            t.DragDelta += Thumb_DragDelta;
            t.PreviewMouseDown += Thumb_PreviewMouseDown;
            SetLeft(t, p.X); SetTop(t, p.Y);
            MyPoints.Add(p);
            MyThumbs.Add(t);
            this.Children.Add(t);
        }

        public void RemovePoint()
        {
            if (MyCurrentThumb is null) { return; }
            int i = MyThumbs.IndexOf(MyCurrentThumb);
            MyPoints.RemoveAt(i);
            MyThumbs.Remove(MyCurrentThumb);
            this.Children.Remove(MyCurrentThumb);
            MyCurrentThumb = null;
        }
        public void ChangeVisibleThumb()
        {
            if (IsThumbVisible)
            {
                for (int i = 1; i < MyThumbs.Count; i++)
                {
                    MyThumbs[i].Visibility = Visibility.Collapsed;
                }
                IsThumbVisible = false;
            }
            else
            {
                for (int i = 1; i < MyThumbs.Count; i++)
                {
                    MyThumbs[i].Visibility = Visibility.Visible;
                }
                IsThumbVisible = true;
            }
        }

        private void Thumb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MyCurrentThumb = sender as TThumb;
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is not TThumb t) { return; }
            double x = GetLeft(t) + e.HorizontalChange;
            double y = GetTop(t) + e.VerticalChange;
            MyPoints[MyThumbs.IndexOf(t)] = new Point(x, y);
            SetLeft(t, x); SetTop(t, y);
        }

    }

    //2022WPF/MainWindow.xaml.cs at 7f77724e85c6c57f8bbebcf46376450dd0b99d3b · gogowaten/2022WPF
    //    https://github.com/gogowaten/2022WPF/blob/7f77724e85c6c57f8bbebcf46376450dd0b99d3b/20220610_PolyLine%E3%81%A8Thumb/20220610_PolyLine%E3%81%A8Thumb/MainWindow.xaml.cs
    //WPF、PolyLineの頂点にThumb表示、マウスドラッグで頂点移動、その2 - 午後わてんのブログ
    //https://gogowaten.hatenablog.com/entry/2022/06/13/115158
    public class PolyLineCanvas : Canvas
    {
        public readonly List<TThumb> MyThumbs = new();
        public PointCollection MyPoints = new();

        //public PointCollection MyPoints
        //{
        //    get { return (PointCollection)GetValue(MyPointsProperty); }
        //    set { SetValue(MyPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointsProperty =
        //    DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(PolyLineCanvas),
        //        new FrameworkPropertyMetadata(null,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Polyline MyPolyline;
        //クリックしたThumb
        public TThumb? MyCurrentThumb { get; private set; }
        public bool IsThumbVisible = true;
        public PolyLineCanvas()
        {
            MyPoints = new();
            MyPolyline = new()
            {
                Stroke = Brushes.Red,
                StrokeThickness = 1.0,
                Points = MyPoints,
            };
            MyPoints.Add(new(0, 0));
            MyPoints.Add(new(100, 100));
            MyPoints.Add(new(200, 20));
            this.Children.Add(MyPolyline);
        }
        public PolyLineCanvas(Brush stroke, double thickness)
        {
            MyPolyline = new()
            {
                Stroke = stroke,
                StrokeThickness = thickness,
                Points = MyPoints
            };
            this.Children.Add(MyPolyline);
        }

        public void AddPoint(Point p)
        {
            TThumb t = new() { Width = 20, Height = 20 };
            t.DragDelta += Thumb_DragDelta;
            t.PreviewMouseDown += Thumb_PreviewMouseDown;
            SetLeft(t, p.X); SetTop(t, p.Y);
            MyPoints.Add(p);
            MyThumbs.Add(t);
            this.Children.Add(t);
        }

        public void RemovePoint()
        {
            if (MyCurrentThumb is null) { return; }
            int i = MyThumbs.IndexOf(MyCurrentThumb);
            MyPoints.RemoveAt(i);
            MyThumbs.Remove(MyCurrentThumb);
            this.Children.Remove(MyCurrentThumb);
            MyCurrentThumb = null;
        }
        public void ChangeVisibleThumb()
        {
            if (IsThumbVisible)
            {
                for (int i = 1; i < MyThumbs.Count; i++)
                {
                    MyThumbs[i].Visibility = Visibility.Collapsed;
                }
                IsThumbVisible = false;
            }
            else
            {
                for (int i = 1; i < MyThumbs.Count; i++)
                {
                    MyThumbs[i].Visibility = Visibility.Visible;
                }
                IsThumbVisible = true;
            }
        }

        private void Thumb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MyCurrentThumb = sender as TThumb;
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is not TThumb t) { return; }
            double x = GetLeft(t) + e.HorizontalChange;
            double y = GetTop(t) + e.VerticalChange;
            MyPoints[MyThumbs.IndexOf(t)] = new Point(x, y);
            SetLeft(t, x); SetTop(t, y);
        }

    }

    public class TThumb : Thumb
    {
        public TThumb()
        {
            this.Template = MakeTemplate();
        }
        private ControlTemplate MakeTemplate()
        {
            FrameworkElementFactory elementF = new(typeof(Rectangle));
            elementF.SetValue(Rectangle.FillProperty, Brushes.Transparent);
            elementF.SetValue(Rectangle.StrokeProperty, Brushes.Black);
            elementF.SetValue(Rectangle.StrokeDashArrayProperty,
                new DoubleCollection() { 2.0 });
            ControlTemplate template = new(typeof(Thumb));
            template.VisualTree = elementF;
            return template;
        }
    }
    public class MyConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value == null) return null;
            if (value is string str)
            {
                string[] ss = str.Split(' ');
                ObservableCollection<Point> points = new();
                foreach (var item in ss)
                {
                    string[] xy = item.Split(',');
                    double x;
                    double y;
                    if (double.TryParse(xy[0], out x) && double.TryParse(xy[1], out y))
                    {
                        Point point = new(x, y);
                        points.Add(point);
                    }
                }
                return points;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
