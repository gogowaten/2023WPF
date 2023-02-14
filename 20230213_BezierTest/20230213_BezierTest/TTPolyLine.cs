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

namespace _20230213_BezierTest
{
    public class TTPolyLine : Thumb
    {
        PolyLineCanvas MyTemplate { get; set; }

        public PointCollection MyPP;
        public TTPolyLine()
        {
            SetTemplate();
            MyTemplate = (PolyLineCanvas)this.Template.FindName("name", this);
            MyPP = MyTemplate.MyPoints;
        }
        private void SetTemplate()
        {
            FrameworkElementFactory bGrid = new(typeof(Grid));
            FrameworkElementFactory factory = new(typeof(PolyLineCanvas), "name");

            bGrid.AppendChild(factory);
            this.Template = new() { VisualTree = factory };
            this.ApplyTemplate();

        }
    }

    //2022WPF/MainWindow.xaml.cs at 7f77724e85c6c57f8bbebcf46376450dd0b99d3b · gogowaten/2022WPF
    //    https://github.com/gogowaten/2022WPF/blob/7f77724e85c6c57f8bbebcf46376450dd0b99d3b/20220610_PolyLine%E3%81%A8Thumb/20220610_PolyLine%E3%81%A8Thumb/MainWindow.xaml.cs
    //WPF、PolyLineの頂点にThumb表示、マウスドラッグで頂点移動、その2 - 午後わてんのブログ
    //https://gogowaten.hatenablog.com/entry/2022/06/13/115158
    public class PolyLineCanvas : Canvas
    {
        public readonly List<TThumb> MyThumbs = new();
        public readonly PointCollection MyPoints = new();
        public readonly Polyline MyPolyline;
        //クリックしたThumb
        public TThumb? MyCurrentThumb { get; private set; }
        public bool IsThumbVisible = true;
        public PolyLineCanvas()
        {
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

}
