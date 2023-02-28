using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _20230228_PolylineAnchorCanvas
{
    class AnchorThumb:Thumb
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
        
        public AnchorThumb()
        {
            DataContext = this;
            MyTemplateElement = SetTemplate();
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
            SetBinding(Canvas.LeftProperty, nameof(X));
            SetBinding(Canvas.TopProperty, nameof(Y));
            Width = 20;
            Height = 20;
        }
        public AnchorThumb(Point point) : this()
        {
            X = point.X;
            Y = point.Y;

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
