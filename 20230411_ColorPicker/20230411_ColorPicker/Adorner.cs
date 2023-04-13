using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;

namespace _20230411_ColorPicker
{
    internal class MarkerAdorner : Adorner
    {

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(MarkerAdorner),
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
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(MarkerAdorner),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MarkerSize
        {
            get { return (double)GetValue(MarkerSizeProperty); }
            set { SetValue(MarkerSizeProperty, value); }
        }
        public static readonly DependencyProperty MarkerSizeProperty =
            DependencyProperty.Register(nameof(MarkerSize), typeof(double), typeof(MarkerAdorner),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public SolidColorBrush Color1
        {
            get { return (SolidColorBrush)GetValue(Color1Property); }
            set { SetValue(Color1Property, value); }
        }
        public static readonly DependencyProperty Color1Property =
            DependencyProperty.Register(nameof(Color1), typeof(SolidColorBrush), typeof(MarkerAdorner),
                new FrameworkPropertyMetadata(Brushes.Black,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public SolidColorBrush Color2
        {
            get { return (SolidColorBrush)GetValue(Color2Property); }
            set { SetValue(Color2Property, value); }
        }
        public static readonly DependencyProperty Color2Property =
            DependencyProperty.Register(nameof(Color2), typeof(SolidColorBrush), typeof(MarkerAdorner),
                new FrameworkPropertyMetadata(Brushes.White,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));




        private VisualCollection MyVisuals { get; set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];

        public Thumb Marker;
        public Canvas MyCanvas;
        public Border TargetElement;
        public MarkerAdorner(FrameworkElement adornedElement) : base(adornedElement)
        {
            if (adornedElement is Border border)
            {
                TargetElement = border;
            }
            else throw new ArgumentException(nameof(adornedElement));

            MyVisuals = new(this);
            MyCanvas = new();
            MyVisuals.Add(MyCanvas);
            MyCanvas.Width = 200;
            MyCanvas.Height = 200;

            Marker = new Thumb();
            MyCanvas.Children.Add(Marker);


            SetMarker();
            SetMarkerTemplate();
        }
        private void SetMarker()
        {
            Binding b0 = new() { Source = this, Path = new PropertyPath(XProperty) };
            Binding b1 = new() { Source = TargetElement, Path = new PropertyPath(WidthProperty) };
            MultiBinding mb = new();
            mb.Bindings.Add(b0);
            mb.Bindings.Add(b1);
            mb.Converter = new ConverterTopLeft2XY();
            Marker.SetBinding(Canvas.LeftProperty, mb);

            b0 = new() { Source = this, Path = new PropertyPath(YProperty) };
            b1 = new() { Source = TargetElement, Path = new PropertyPath(HeightProperty) };
            mb = new();
            mb.Bindings.Add(b0);
            mb.Bindings.Add(b1);
            mb.Converter = new ConverterTopLeft2XY();
            Marker.SetBinding(Canvas.TopProperty, mb);

        }
        private void SetMarkerTemplate()
        {
            FrameworkElementFactory factory = new(typeof(Grid), "nemo");
            FrameworkElementFactory e1 = new(typeof(Ellipse));
            e1.SetValue(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(MarkerSizeProperty) });
            e1.SetValue(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(MarkerSizeProperty) });
            e1.SetValue(Ellipse.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(Color1Property) });
            e1.SetValue(Ellipse.FillProperty, Brushes.Transparent);
            FrameworkElementFactory e2 = new(typeof(Ellipse));
            e2.SetValue(WidthProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MarkerSizeProperty),
                Converter = new ConverterDownSize()
            });
            e2.SetValue(HeightProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MarkerSizeProperty),
                Converter = new ConverterDownSize()
            });
            e2.SetValue(Ellipse.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(Color2Property), });
            factory.AppendChild(e1);
            factory.AppendChild(e2);
            Marker.Template = new() { VisualTree = factory };
            Marker.DragDelta += Marker_DragDelta;
        }

        private void Marker_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var left = Canvas.GetLeft(Marker);
            var top = Canvas.GetTop(Marker);

            var xx = left + e.HorizontalChange;
            var yy = top + e.VerticalChange;

            X = xx / TargetElement.Width;
            Y = yy / TargetElement.Height;

        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            MyCanvas.Arrange(new Rect(finalSize));
            return base.ArrangeOverride(finalSize);
        }

    }

    public class ConverterDownSize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double size = (double)value;
            return size -= 1.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
