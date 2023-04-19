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

namespace _20230419_ColorPicker
{
    public class Marker : Adorner
    {
        #region 依存関係プロパティ

        public double Saturation
        {
            get { return (double)GetValue(SaturationProperty); }
            set { SetValue(SaturationProperty, value); }
        }
        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register(nameof(Saturation), typeof(double), typeof(Marker),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(Marker),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public double MarkLeft
        //{
        //    get { return (double)GetValue(MarkLeftProperty); }
        //    set { SetValue(MarkLeftProperty, value); }
        //}
        //public static readonly DependencyProperty MarkLeftProperty =
        //    DependencyProperty.Register(nameof(MarkLeft), typeof(double), typeof(Marker),
        //        new FrameworkPropertyMetadata(0.0,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public double MarkTop
        //{
        //    get { return (double)GetValue(MarkTopProperty); }
        //    set { SetValue(MarkTopProperty, value); }
        //}
        //public static readonly DependencyProperty MarkTopProperty =
        //    DependencyProperty.Register(nameof(MarkTop), typeof(double), typeof(Marker),
        //        new FrameworkPropertyMetadata(0.0,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MarkerSize
        {
            get { return (double)GetValue(MarkerSizeProperty); }
            set { SetValue(MarkerSizeProperty, value); }
        }
        public static readonly DependencyProperty MarkerSizeProperty =
            DependencyProperty.Register(nameof(MarkerSize), typeof(double), typeof(Marker),
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
            DependencyProperty.Register(nameof(Color1), typeof(SolidColorBrush), typeof(Marker),
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
            DependencyProperty.Register(nameof(Color2), typeof(SolidColorBrush), typeof(Marker),
                new FrameworkPropertyMetadata(Brushes.White,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存関係プロパティ



        private VisualCollection MyVisuals { get; set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];

        public Thumb MarkerThumb;
        public Canvas MyCanvas;
        public FrameworkElement TargetElement;

        private Point DiffPoint;

        #region コンストラクタ

        //通常
        public Marker(FrameworkElement adornedElement) : base(adornedElement)
        {
            TargetElement = adornedElement;

            MyVisuals = new(this);
            MyCanvas = new();
            MyVisuals.Add(MyCanvas);
            MarkerThumb = new Thumb();
            SetMyCanvas();
            SetMarker();
            SetMarkerTemplate();
            MarkerThumb.DragDelta += Marker_DragDelta;
            MarkerThumb.DragCompleted += (s, e) => { DiffPoint = new(); };
        }

        //色指定で開くとき、彩度と輝度の指定が必要
        public Marker(FrameworkElement adornedElement, double saturation, double value) : this(adornedElement)
        {
            Saturation = saturation;
            Value = value;
        }

        #endregion コンストラクタ


        //
        private void SetMyCanvas()
        {
            MyCanvas.Background = Brushes.Transparent;
            MyCanvas.MouseLeftButtonDown += MyCanvas_MouseLeftButtonDown;
            MyCanvas.Children.Add(MarkerThumb);
            MyCanvas.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ActualWidthProperty) });
            MyCanvas.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ActualHeightProperty) });
        }

        //SaturationとValueをクリック座標に相当する値に更新＋
        //更新前後の座標差を記録、これは
        //続けてドラッグ移動した場合に使う
        private void MyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pp = Mouse.GetPosition(MyCanvas);
            double dx = pp.X - Canvas.GetLeft(MarkerThumb) - (MarkerSize / 2.0);
            double dy = pp.Y - Canvas.GetTop(MarkerThumb) - (MarkerSize / 2.0);
            DiffPoint = new Point(dx, dy);

            double xx = pp.X / TargetElement.Width;
            if (xx < 0) xx = 0; if (xx > 1.0) xx = 1.0;
            Saturation = xx;

            double yy = pp.Y / TargetElement.Height;
            if (yy < 0) yy = 0; if (yy > 1.0) yy = 1.0;
            Value = yy;

            MarkerThumb.RaiseEvent(e);
        }

        private void SetMarker()
        {
            //SV変化でtopleft変化
            MultiBinding mb = new();
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MarkerSizeProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(SaturationProperty) });
            mb.Bindings.Add(new Binding() { Source = TargetElement, Path = new PropertyPath(WidthProperty) });
            mb.Converter = new ConverterTopLeft2XY();
            MarkerThumb.SetBinding(Canvas.LeftProperty, mb);

            mb = new();
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MarkerSizeProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(ValueProperty) });
            mb.Bindings.Add(new Binding() { Source = TargetElement, Path = new PropertyPath(HeightProperty) });
            mb.Converter = new ConverterTopLeft2XY();
            MarkerThumb.SetBinding(Canvas.TopProperty, mb);

        }
        private void SetMarkerTemplate()
        {
            FrameworkElementFactory factory = new(typeof(Grid));
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
            MarkerThumb.Template = new() { VisualTree = factory };

        }

        private void Marker_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var left = Canvas.GetLeft(MarkerThumb);
            var top = Canvas.GetTop(MarkerThumb);
            var h = e.HorizontalChange;
            var v = e.VerticalChange;
            // ドラッグ移動ではtopleftを指定ではなく、saturation,Valueを計算して指定
            var dx = DiffPoint.X + (MarkerSize / 2.0);
            var dy = DiffPoint.Y + (MarkerSize / 2.0);
            double ll = left + h + dx;
            double xx = ll / TargetElement.Width;
            if (xx < 0) xx = 0; if (xx > 1.0) xx = 1.0;
            Saturation = xx;
            double tt = top + v + dy;
            double yy = tt / TargetElement.Height;
            if (yy < 0) yy = 0; if (yy > 1.0) yy = 1.0;
            Value = yy;
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            MyCanvas.Arrange(new Rect(finalSize));
            return base.ArrangeOverride(finalSize);
        }
    }



    public class ConverterTopLeft2XY : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double markerSize = (double)values[0];
            double sv = (double)values[1];
            double targetSize = (double)values[2];
            double result = (sv * targetSize) - (markerSize / 2.0);
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ConverterDownSize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double size = ((double)value) - 2.0;
            if (size < 0) { size = 0; }
            return size;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
