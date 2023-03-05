using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

//embellishとadornとdecorateの語感の違い... - Yahoo!知恵袋
//https://detail.chiebukuro.yahoo.co.jp/qa/question_detail/q1023045950

//adorn：飾る、装飾する、アドーン
//adorner：装飾者、アドーナー、アドナー
//装飾：decorate、装飾者：decorator
//Adol Christhin

//装飾の概要 - WPF .NET Framework | Microsoft Learn
//https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/controls/adorners-overview?view=netframeworkdesktop-4.8
//WPF Adorners Part 1 – What are adorners
//https://www.nbdtech.com/Blog/archive/2010/06/21/wpf-adorners-part-1-ndash-what-are-adorners.aspx

namespace _20230303
{
    class Ador : Adorner
    {
        public Ador(UIElement adornedElement) : base(adornedElement)
        {
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            //base.OnRender(drawingContext);

            SolidColorBrush brush = new(Color.FromArgb(50, 200, 0, 0));
            Pen pen = new Pen(Brushes.Red, 1.0);
            double radius = 10.0;
            Rect rect = new Rect(this.AdornedElement.DesiredSize);
            drawingContext.DrawEllipse(brush, pen, rect.TopLeft, radius, radius);
            drawingContext.DrawEllipse(brush, pen, rect.TopRight, radius, radius);
            drawingContext.DrawEllipse(brush, pen, rect.BottomLeft, radius, radius);
            drawingContext.DrawEllipse(brush, pen, rect.BottomRight, radius, radius);
        }
    }



    //C#のWPFでサイズ変更できるTextBoxを作る - Ararami Studio
    //    https://araramistudio.jimdo.com/2016/12/08/wpf%E3%81%A7%E3%82%B5%E3%82%A4%E3%82%BA%E5%A4%89%E6%9B%B4%E3%81%A7%E3%81%8D%E3%82%8Btextbox%E3%82%92%E4%BD%9C%E3%82%8B/

    //そのままでは動かないので、少し変更したけどなんか違う
    //Thumbが対象の中央に表示されるし、サイズ変更できるけどサイズが大きくなるに連れて
    //Thumbの表示位置と本体とのズレが大きくなる
    public class AAdor : Adorner
    {
        private Thumb MyGrip = new();
        private VisualCollection MyVisuals;
        public AAdor(UIElement adornedElement) : base(adornedElement)
        {
            MyGrip.Width = 20;
            MyGrip.Height = 20;
            MyGrip.Background = Brushes.Red;
            MyGrip.DragDelta += MyGrip_DragDelta;

            MyVisuals = new VisualCollection(this);
            MyVisuals.Add(MyGrip);
        }


        private void MyGrip_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (AdornedElement is FrameworkElement elem)
            {
                var x = Canvas.GetLeft(MyGrip);
                double w = elem.ActualWidth + e.HorizontalChange;
                double h = elem.ActualHeight + e.VerticalChange;
                if (w < 20) { w = 20; }
                if (h < 20) { h = 20; }
                elem.Width = w;
                elem.Height = h;
            }
        }
        //private void MyGrip_DragDelta(object sender, DragDeltaEventArgs e)
        //{
        //    if (AdornedElement is FrameworkElement adorElem)
        //    {
        //        double targetW = adorElem.Width;
        //        double targetH = adorElem.Height;
        //        if (targetW.Equals(double.NaN)) { targetW = adorElem.DesiredSize.Width; }
        //        if (targetH.Equals(double.NaN)) { targetH = adorElem.DesiredSize.Height; }
        //        targetW += e.HorizontalChange;
        //        targetH += e.VerticalChange;
        //        targetW = Math.Max(MyGrip.Width, targetW);
        //        targetH = Math.Max(MyGrip.Height, targetH);
        //        targetW = Math.Max(adorElem.MinWidth, targetW);
        //        targetH = Math.Max(adorElem.MinHeight, targetH);
        //        targetW = Math.Min(adorElem.MaxWidth, targetW);
        //        targetH = Math.Min(adorElem.MaxHeight, targetH);
        //        adorElem.Width = targetW;
        //        adorElem.Height = targetH;
        //    }
        //}

        protected override Size ArrangeOverride(Size finalSize)
        {
            //return base.ArrangeOverride(finalSize);

            if (AdornedElement is FrameworkElement elem)
            {
                var aw = elem.ActualWidth;
                var dw = elem.DesiredSize.Width;
                double w = elem.ActualWidth;
                double h = elem.ActualHeight;
                double x = elem.ActualWidth - w;
                double y = elem.ActualHeight - h;
                MyGrip.Arrange(new Rect(x, y, w, h));
                return finalSize;
            }
            else { return base.ArrangeOverride(finalSize); }
        }
        protected override int VisualChildrenCount
        {
            get
            {
                //return base.VisualChildrenCount;
                return MyVisuals.Count;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            //return base.GetVisualChild(index);
            return MyVisuals[index];
        }
    }










    //Mitesh Sureja's Blog: Adorners in WPF
    //    http://miteshsureja.blogspot.com/2016/08/adorners-in-wpf.html

    //期待どおりにサイズ変更できる
    //簡略化した
    public class BBAdor : Adorner
    {
        readonly Thumb MyThumb;
        readonly VisualCollection MyVisuals;
        readonly FrameworkElement MyTarget;
        public BBAdor(FrameworkElement adornedElement) : base(adornedElement)
        {
            MyTarget = adornedElement;
            MyVisuals = new VisualCollection(this);
            MyThumb = new Thumb()
            {
                Cursor = Cursors.SizeNWSE,
                Height = 20,
                Width = 20,
                Opacity = 0.5,
                Background = Brushes.Red,
            };
            MyThumb.DragDelta += MyThumb_DragDelta;
            MyVisuals.Add(MyThumb);

            //TextBoxなどWidthの既定値がNaNなのを解除する
            MyTarget.Width = MyTarget.DesiredSize.Width;
            MyTarget.Height = MyTarget.DesiredSize.Height;

        }

        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            MyTarget.Width = Math.Max(MyTarget.Width + e.HorizontalChange, MyThumb.Width);
            MyTarget.Height = Math.Max(MyTarget.Height + e.VerticalChange, MyThumb.Height);
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            //return base.ArrangeOverride(finalSize);

            base.ArrangeOverride(finalSize);//いらない？なくても動く
            double thisWidth = this.DesiredSize.Width;
            double thisHeight = this.DesiredSize.Height;
            //ThumbのRect変更？ここがわからん、/2しないと対象の中央に表示される、/2で右下に表示される
            MyThumb.Arrange(new Rect(
                MyTarget.Width - thisWidth / 2,
                MyTarget.Height - thisHeight / 2,
                thisWidth, thisHeight));
            return finalSize;
        }

        protected override int VisualChildrenCount => MyVisuals.Count;

        protected override Visual GetVisualChild(int index) => MyVisuals[index];
    }






    public class CCAdor : Adorner
    {
        public Thumb? MyCurrentThumb;
        int MyCurrentIndex;
        public VisualCollection MyVisuals { get; private set; }
        readonly Polyline MyPolyline;
        public Canvas MyCanvas = new();
        public CCAdor(Polyline adornedElement) : base(adornedElement)
        {
            MyPolyline = adornedElement;
            MyVisuals = new VisualCollection(this);
            MyVisuals.Add(MyCanvas);
            foreach (var item in MyPolyline.Points)
            {

                Thumb tt = new Thumb()
                {
                    Cursor = Cursors.Hand,
                    Height = 20,
                    Width = 20,
                    Opacity = 0.5,
                    Background = Brushes.Red,
                };
                MyCanvas.Children.Add(tt);
                                
                Canvas.SetLeft(tt, item.X);
                Canvas.SetTop(tt, item.Y);
                tt.DragDelta += MyThumb_DragDelta;
                tt.PreviewMouseLeftButtonDown += MyCurrentThumb_PreviewMouseLeftButtonDown;
            }

        }

        private void MyCurrentThumb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Thumb thumb)
            {
                MyCurrentThumb = thumb;
                MyCurrentIndex = MyCanvas.Children.IndexOf(MyCurrentThumb);
            }
        }

        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Point pp = MyPolyline.Points[MyCurrentIndex];
            Canvas.SetLeft(MyCurrentThumb, pp.X + e.HorizontalChange);
            Canvas.SetTop(MyCurrentThumb, pp.Y + e.VerticalChange);
            MyPolyline.Points[MyCurrentIndex] = Point.Add(pp, new Vector(e.HorizontalChange, e.VerticalChange));
            //MyPolyline.Width = Math.Max(MyPolyline.Width + e.HorizontalChange, MyCurrentThumb.Width);
            //MyPolyline.Height = Math.Max(MyPolyline.Height + e.VerticalChange, MyCurrentThumb.Height);

        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            //return base.ArrangeOverride(finalSize);
            base.ArrangeOverride(finalSize);//いらない？なくても動く
            MyCanvas.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));

            //double thisWidth = this.DesiredSize.Width;
            //double thisHeight = this.DesiredSize.Height;
            ////ThumbのRect変更？ここがわからん、/2しないと対象の中央に表示される、/2で右下に表示される
            ////MyCurrentThumb.Arrange(new Rect(
            ////    MyPolyline.Width / 2, MyPolyline.Height / 2, thisWidth, thisHeight));
            //MyCurrentThumb.Arrange(new Rect(
            //    MyPolyline.Width / 2, MyPolyline.Height / 2, finalSize.Width, finalSize.Height));

            return finalSize;
        }

        protected override int VisualChildrenCount => MyVisuals.Count;

        protected override Visual GetVisualChild(int index) => MyVisuals[index];
    }


}
