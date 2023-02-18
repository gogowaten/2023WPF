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
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace _20230213_BezierTest
{

    public class TTLine3 : Thumb
    {
        public Arrow MyLine { get; set; }
        public Data MyData { get; set; } = new();

        //public PointCollection MyPoints
        //{
        //    get { return (PointCollection)GetValue(MyPointsProperty); }
        //    set { SetValue(MyPointsProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointsProperty =
        //    DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TTLine3),
        //        new FrameworkPropertyMetadata(null,
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
            DependencyProperty.Register(nameof(MyPoints), typeof(ObservableCollection<Point>), typeof(TTLine3),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ArrowHeadType HeadType
        {
            get { return (ArrowHeadType)GetValue(HeadTypeProperty); }
            set { SetValue(HeadTypeProperty, value); }
        }
        public static readonly DependencyProperty HeadTypeProperty =
            DependencyProperty.Register(nameof(HeadType), typeof(ArrowHeadType), typeof(TTLine3),
                new FrameworkPropertyMetadata(ArrowHeadType.None,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure|
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public TTLine3()
        {
            MyLine = SetTemplate();
            MyLine.Stroke = Brushes.Red;
            MyLine.Fill = Brushes.Red;
            MyLine.StrokeThickness = 10.0;

            MyLine.SetBinding(Arrow.MyPointsProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyPointsProperty),
            });
            //依存プロパティとDataのPointCollectionをBindingしたいけど、なぜか
            //値は更新されるのにPolyLineは変化しないので"="、これだと変化する
            Loaded += (a, b) => { MyData.ObPoints = MyPoints; };
            //SetBinding(MyPointsProperty, new Binding(nameof(MyData.PointCollection))
            //{
            //    Source = MyData,
            //    Mode = BindingMode.TwoWay
            //});
            MyLine.SetBinding(Arrow.HeadTypeProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(HeadTypeProperty),
            });

            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
            DragDelta += TTLine_DragDelta;
        }

        private void TTLine_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is TTLine3 line)
            {
                Canvas.SetLeft(line, Canvas.GetLeft(line) + e.HorizontalChange);
                Canvas.SetTop(line, Canvas.GetTop(line) + e.VerticalChange);
            }
        }

        private Arrow SetTemplate()
        {
            FrameworkElementFactory fLine = new(typeof(Arrow), "line");
            this.Template = new() { VisualTree = fLine };
            this.ApplyTemplate();
            if (Template.FindName("line", this) is Arrow line)
            {
                return line;
            }
            else { throw new Exception(); }
        }
    }


    //TemplateがPolyLineのThumb
    //Points関連のプロパティは3つ
    //依存プロパティのMyPoints
    //DataクラスのPointCollection
    //PolyLineのPoints
    //この内どれかを変更すると全てが変更される
    public class TTLine2 : Thumb
    {
        public Polyline MyLine { get; set; }
        public Data MyData { get; set; } = new();

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TTLine2),
                new FrameworkPropertyMetadata(null,
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
        //    DependencyProperty.Register(nameof(MyPoints), typeof(ObservableCollection<Point>), typeof(TTLine2),
        //        new FrameworkPropertyMetadata(null,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public TTLine2()
        {
            MyLine = SetTemplate();
            MyLine.Stroke = Brushes.Red;
            MyLine.StrokeThickness = 10.0;

            MyLine.SetBinding(Polyline.PointsProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyPointsProperty)
            });
            //依存プロパティとDataのPointCollectionをBindingしたいけど、なぜか
            //値は更新されるのにPolyLineは変化しないので"="、これだと変化する
            Loaded += (a, b) => { MyData.PointCollection = MyPoints; };
            //SetBinding(MyPointsProperty, new Binding(nameof(MyData.PointCollection))
            //{
            //    Source = MyData,
            //    Mode = BindingMode.TwoWay
            //});

            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
            DragDelta += TTLine2_DragDelta;
        }

        private void TTLine2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is TTLine2 line)
            {
                Canvas.SetLeft(line, Canvas.GetLeft(line) + e.HorizontalChange);
                Canvas.SetTop(line, Canvas.GetTop(line) + e.VerticalChange);
            }
        }

        private Polyline SetTemplate()
        {
            FrameworkElementFactory fLine = new(typeof(Polyline), "line");
            this.Template = new() { VisualTree = fLine };
            this.ApplyTemplate();
            if (Template.FindName("line", this) is Polyline line)
            {
                return line;
            }
            else { throw new Exception(); }
        }
    }

    public class TTLine : Thumb
    {
        public Polyline MyLine { get; set; }

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TTLine),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public TTLine()
        {
            MyLine = SetTemplate();
            MyLine.Stroke = Brushes.Red;
            MyLine.StrokeThickness = 10.0;
            MyLine.SetBinding(Polyline.PointsProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyPointsProperty),
            });
        }
        private Polyline SetTemplate()
        {
            FrameworkElementFactory fLine = new(typeof(Polyline), "line");
            this.Template = new() { VisualTree = fLine };
            this.ApplyTemplate();
            if (Template.FindName("line", this) is Polyline line)
            {
                return line;
            }
            else
            {
                throw new Exception();
            }

        }
    }
    public class PolyLineWithVThumb : Thumb
    {

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(PolyLineWithVThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnMyPoints)));

        private static void OnMyPoints(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PolyLineWithVThumb poly)
            {
                var neko = e.NewValue;
            };
        }


        public ObservableCollection<Point> ObPoints
        {
            get { return (ObservableCollection<Point>)GetValue(ObPointsProperty); }
            set { SetValue(ObPointsProperty, value); }
        }
        public static readonly DependencyProperty ObPointsProperty =
            DependencyProperty.Register(nameof(ObPoints), typeof(ObservableCollection<Point>), typeof(PolyLineWithVThumb), new PropertyMetadata(null));

        public Data MyData { get; set; } = new();
        public Polyline MyPolyLine;
        public Canvas? MyCanvas;

        public ObservableCollection<TThumb> VThumbs = new();
        public PolyLineWithVThumb()
        {
            MyPolyLine = SetTemplate();


            MyPolyLine.SetBinding(Polyline.PointsProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyPointsProperty)
            });

            SetBinding(MyPointsProperty, new Binding(nameof(MyData.PointCollection))
            {
                Source = this.MyData,
                Mode = BindingMode.TwoWay,
            });
            Loaded += PolyLineWithVThumb_Loaded;
            if (MyCanvas != null)
            {
                MyCanvas.SetBinding(WidthProperty, new Binding() { Source = MyPolyLine, Path = new PropertyPath(ActualWidthProperty) });
                MyCanvas.SetBinding(HeightProperty, new Binding() { Source = MyPolyLine, Path = new PropertyPath(ActualHeightProperty) });
            }
        }


        private void PolyLineWithVThumb_Loaded(object sender, RoutedEventArgs e)
        {
            if (MyCanvas is null) return;
            foreach (var item in MyPoints)
            {
                TThumb tt = new();
                tt.Width = 20;
                tt.Height = 20;
                Canvas.SetLeft(tt, item.X);
                Canvas.SetTop(tt, item.Y);
                VThumbs.Add(tt);
                MyCanvas.Children.Add(tt);
            }
        }

        private Polyline SetTemplate()
        {
            FrameworkElementFactory fPanel = new(typeof(Canvas), "canvas");
            FrameworkElementFactory fRect = new(typeof(Rectangle));
            FrameworkElementFactory line = new(typeof(Polyline), "line");
            fPanel.AppendChild(fRect);
            fPanel.AppendChild(line);
            fRect.SetValue(Rectangle.StrokeProperty, Brushes.Gray);
            line.SetValue(Polyline.StrokeProperty, Brushes.Red);
            line.SetValue(Polyline.StrokeThicknessProperty, 1.0);

            this.Template = new() { VisualTree = fPanel };
            this.ApplyTemplate();
            MyCanvas = (Canvas?)Template.FindName("canvas", this);
            if (this.Template.FindName("line", this) is Polyline tempLine)
            {
                return tempLine;
            }
            else throw new Exception();
        }
        public void AddPoint(double x, double y)
        {
            MyPoints.Add(new Point(x, y));
        }
    }




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
            //polyline.PointCollection = PointCollection;
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
            SetBinding(MyPointsProperty, new Binding(nameof(MyData.PointCollection)) { Source = MyData, Mode = BindingMode.TwoWay });

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

            SetBinding(MyPointsProperty, new Binding(nameof(MyData.PointCollection)) { Source = MyData, Mode = BindingMode.TwoWay });

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

    public class MyConverterPointCollection : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<Point> points)
            {
                return new PointCollection(points);
            }
            else { return DependencyProperty.UnsetValue; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PointCollection points)
            {
                return new ObservableCollection<Point>(points);
            }
            else { return DependencyProperty.UnsetValue; }
        }
    }

    //c# - コレクションDPのWPF TypeConversionAttribute
    //    https://stackoverflow.com/questions/5154230/wpf-typeconversionattribute-for-collection-dp

    public class MyTypeConverterPoints : TypeConverter
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
                    if (double.TryParse(xy[0], out double x) && double.TryParse(xy[1], out double y))
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
