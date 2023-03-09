using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _20230309_Adorner
{
    public class GeoAdorner : Adorner
    {
        #region お約束

        public VisualCollection MyVisuals { get; private set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];
        #endregion お約束

        public Rectangle MyRectangleRed { get; private set; }
        public Rectangle MyRectangleBlue { get; private set; }
        public Rectangle MyRectangleGold { get; private set; }


        public GeoAdorner(UIElement adornedElement) : base(adornedElement)
        {
            MyVisuals = new(this);

            MyRectangleRed = new() { Stroke = Brushes.Red, StrokeThickness = 1.0, };
            MyVisuals.Add(MyRectangleRed);

            MyRectangleBlue = new() { Stroke = Brushes.Blue, StrokeThickness = 1.0 };
            MyVisuals.Add(MyRectangleBlue);

            MyRectangleGold = new() { Stroke = Brushes.Gold, StrokeThickness = 1.0 };
            MyVisuals.Add(MyRectangleGold);

        }

        protected override Size ArrangeOverride(Size finalSize)
        {

            Rect targetDescendantBounds = VisualTreeHelper.GetDescendantBounds(AdornedElement);
            MyRectangleRed.Arrange(targetDescendantBounds);

            Rect targetRenderRect = new Rect(AdornedElement.RenderSize);
            MyRectangleBlue.Arrange(targetRenderRect);

            //if(AdornedElement is PolyGeoBase poly)
            //{
            //    MyRectangleGold.Arrange(poly.MyBounds);
            //}

            return base.ArrangeOverride(finalSize);
        }
    }
}
