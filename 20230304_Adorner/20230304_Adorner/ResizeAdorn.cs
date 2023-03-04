using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;


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


//Mitesh Sureja's Blog: Adorners in WPF
//    http://miteshsureja.blogspot.com/2016/08/adorners-in-wpf.html


namespace _20230304_Adorner
{
    internal class ResizeAdorn : Adorner
    {
        private Thumb MyThumb;
        private VisualCollection MyVisualChild;
        public ResizeAdorn(UIElement adornedElement) : base(adornedElement)
        {
            MyVisualChild = new VisualCollection(this);
            MyThumb = new()
            {
                Width = 20,
                Height = 20,
                Opacity = 0.2,
                Background = Brushes.Blue,
            };
            MyVisualChild.Add(MyThumb);
            MyThumb.DragDelta += MyThumb_DragDelta;
        }

        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (AdornedElement is FrameworkElement target)
            {
                EnforceSize(target);
                target.Width = Math.Max(target.Width + e.HorizontalChange, MyThumb.DesiredSize.Width);
                target.Height = Math.Max(target.Height + e.VerticalChange, MyThumb.DesiredSize.Height);
            }
        }
        private void EnforceSize(FrameworkElement elem)
        {
            if (elem.Width.Equals(double.NaN)) { elem.Width = elem.DesiredSize.Width; }
            if (elem.Height.Equals(double.NaN)) { elem.Height = elem.DesiredSize.Height; }
            if (elem.Parent is FrameworkElement parent)
            {
                elem.MaxHeight = parent.ActualHeight;
                elem.MaxWidth = parent.ActualWidth;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //return base.ArrangeOverride(finalSize);
            base.ArrangeOverride(finalSize);
            double targetW = AdornedElement.DesiredSize.Width;
            double targetH = AdornedElement.DesiredSize.Height;
            double w = DesiredSize.Width;
            double h = DesiredSize.Height;
            MyThumb.Arrange(new Rect(targetW - w / 2, targetH - h / 2, w, h));
            return finalSize;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                //return base.VisualChildrenCount;
                return MyVisualChild.Count;
            }
        }
        protected override Visual GetVisualChild(int index)
        {
            //return base.GetVisualChild(index);
            return MyVisualChild[index];
        }
    }
}
