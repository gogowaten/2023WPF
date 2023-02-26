using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;

namespace _20230224_GeometryAnchorThumbs
{
    public class AnchorThumb
    {
    }
    public class TThumb : Thumb
    {

        //public Point MyPoint
        //{
        //    get { return (Point)GetValue(MyPointProperty); }
        //    set { SetValue(MyPointProperty, value); }
        //}
        //public static readonly DependencyProperty MyPointProperty =
        //    DependencyProperty.Register(nameof(MyPoint), typeof(Point), typeof(TThumb),
        //        new FrameworkPropertyMetadata(new Point(),
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(TThumb),
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
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure|
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public TThumb(Point vartex) : this()
        {
            X = vartex.X;
            Y = vartex.Y;
            //MyPoint = vartex;
        }
        public TThumb()
        {
            this.Template = MakeTemplate();
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
            this.DataContext = this;
            Width = 20;
            Height = 20;
            this.SetBinding(Canvas.LeftProperty, nameof(X));
            this.SetBinding(Canvas.TopProperty, nameof(Y));
            //MultiBinding mb = new();
            //mb.Converter = new ConverterPoint();
            //mb.Bindings.Add(new Binding(nameof(X)));
            //mb.Bindings.Add(new Binding(nameof(Y)));
            //this.SetBinding(MyPointProperty, mb);
        }


        private ControlTemplate MakeTemplate()
        {
            FrameworkElementFactory elementF = new(typeof(Rectangle));
            elementF.SetValue(Rectangle.FillProperty, Brushes.Transparent);
            elementF.SetValue(Rectangle.StrokeProperty, Brushes.Black);
            elementF.SetValue(Rectangle.StrokeDashArrayProperty,
                new DoubleCollection() { 2.0 });
            return new ControlTemplate() { VisualTree = elementF };
        }
    }
    public class ConverterPoint : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Point((double)values[0], (double)values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            Point p = (Point)value;
            object[] os = new object[2];
            os[0] = p.X;
            os[1] = p.Y;
            return os;
        }
    }
}
