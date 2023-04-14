using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace _20230413
{
    internal class MarkerAdorner : Adorner
    {
        #region 依存関係プロパティ

        //0 to 100
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

        public double MarkLeft
        {
            get { return (double)GetValue(MarkLeftProperty); }
            set { SetValue(MarkLeftProperty, value); }
        }
        public static readonly DependencyProperty MarkLeftProperty =
            DependencyProperty.Register(nameof(MarkLeft), typeof(double), typeof(MarkerAdorner),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MarkTop
        {
            get { return (double)GetValue(MarkTopProperty); }
            set { SetValue(MarkTopProperty, value); }
        }
        public static readonly DependencyProperty MarkTopProperty =
            DependencyProperty.Register(nameof(MarkTop), typeof(double), typeof(MarkerAdorner),
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

        #endregion 依存関係プロパティ


        private VisualCollection MyVisuals { get; set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];

        public Thumb Marker;
        public Canvas MyCanvas;
        public FrameworkElement TargetElement;
        private Point DiffPoint;
        public MarkerAdorner(FrameworkElement adornedElement) : base(adornedElement)
        {
            TargetElement = adornedElement;
            MyVisuals = new(this);
            MyCanvas = new();
            MyVisuals.Add(MyCanvas);
            MyCanvas.Width = TargetElement.Width;
            MyCanvas.Height = TargetElement.Height;
            MyCanvas.Background = Brushes.Transparent;

            MyCanvas.MouseLeftButtonDown += MyCanvas_MouseLeftButtonDown;
            Marker = new Thumb();
            MyCanvas.Children.Add(Marker);


            SetMarker();
            SetMarkerTemplate();
            Marker.DragDelta += Marker_DragDelta;

        }

        private void MyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pp = Mouse.GetPosition(MyCanvas);
            DiffPoint = new Point(pp.X - X, pp.Y - Y);
            X = pp.X;
            Y = pp.Y;
            Marker.RaiseEvent(e);
        }

        private void SetMarker()
        {
            Marker.SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(MarkLeftProperty) });
            Marker.SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(MarkTopProperty) });

            ////markerLeft -> X
            //Binding b0 = new() { Source = this, Path = new PropertyPath(XProperty) };
            //Binding b1 = new() { Source = this, Path = new PropertyPath(MarkerSizeProperty) };
            //Binding b2 = new() { Source = TargetElement, Path = new PropertyPath(WidthProperty) };
            //MultiBinding mb = new();
            //mb.Bindings.Add(b0);
            //mb.Bindings.Add(b1);
            //mb.Bindings.Add(b2);
            //mb.Converter = new ConverterTopLeft2XY();
            //Marker.SetBinding(Canvas.LeftProperty, mb);

            //b0 = new() { Source = this, Path = new PropertyPath(YProperty) };
            //b2 = new() { Source = TargetElement, Path = new PropertyPath(HeightProperty) };
            //mb = new();
            //mb.Bindings.Add(b0);
            //mb.Bindings.Add(b1);
            //mb.Bindings.Add(b2);
            //mb.Converter = new ConverterTopLeft2XY();
            //Marker.SetBinding(Canvas.TopProperty, mb);

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

        }

        private void Marker_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var h = e.HorizontalChange;
            var v = e.VerticalChange;

            MarkLeft += h;MarkTop += v;

            //0 to 100 にする
            


        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            MyCanvas.Arrange(new Rect(finalSize));
            return base.ArrangeOverride(finalSize);
        }

    }

    #region 専用コンバーター

    //2ピクセル小さく
    public class ConverterDownSize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double size = ((double)value) - 2.0;
            if (size < 0) size = 0;
            return size;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ConverterTopLeft2XY : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double sv = (double)values[0];
            double size = (double)values[1];
            double target = (double)values[2];
            double result = (sv / 100.0 * target) - (size / 2.0);
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //public class ConverterTopLeft2XY : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        double sv = (double)values[0];
    //        double rank = (double)values[1];
    //        double size = (double)values[2];
    //        double result = (sv * rank) - (size / 2.0);
    //        return result;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    #endregion 専用コンバーター
}
