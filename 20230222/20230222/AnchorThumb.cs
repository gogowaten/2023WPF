using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;


namespace _20230222
{
    public class AnchorThumb:Thumb
    {

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(AnchorThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(AnchorThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Rectangle MyTemplateElement { get; private set; }
        //public Point MyPoint;
        public AnchorThumb(Point point)
        {
            DataContext = this;
            MyTemplateElement = SetTemplate();
            SetBinding(Canvas.LeftProperty, nameof(X));
            SetBinding(Canvas.TopProperty, nameof(Y));
            X = point.X;
            Y = point.Y;
            Width = 20;
            Height = 20;
            //DragDelta += AnchorThumb_DragDelta;
        }

        private void AnchorThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is AnchorThumb at)
            {
                X += e.HorizontalChange;
                Y += e.VerticalChange;
            }
        }

        private Rectangle SetTemplate()
        {
            FrameworkElementFactory fRect = new(typeof(Rectangle), "rect");
            fRect.SetValue(Rectangle.FillProperty, Brushes.Transparent);
            fRect.SetValue(Rectangle.StrokeProperty, Brushes.Black);
            fRect.SetValue(Rectangle.StrokeDashArrayProperty, new DoubleCollection() { 2.0 });
            this.Template = new() { VisualTree = fRect };
            this.ApplyTemplate();
            if (this.Template.FindName("rect", this) is Rectangle rect)
            {
                return rect;
            }
            else { throw new Exception(); }
        }
    }
}
