using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

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
