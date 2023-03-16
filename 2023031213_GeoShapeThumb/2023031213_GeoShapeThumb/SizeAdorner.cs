using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace _2023031213_GeoShapeThumb
{
    public class SizeAdorner : Adorner
    {
        public VisualCollection MyVisuals { get; set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index)
        {
            return MyVisuals[index];
        }
        public Rectangle MyRectOrange { get; set; } = new() { Stroke = Brushes.Orange, StrokeThickness = 1.0 };
        public SizeAdorner(UIElement adornedElement) : base(adornedElement)
        {
            MyVisuals = new(this) { MyRectOrange };
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            MyRectOrange.Arrange(new Rect(finalSize));
            return base.ArrangeOverride(finalSize);
        }
    }
}
