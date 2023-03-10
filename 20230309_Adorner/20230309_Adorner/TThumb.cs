using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;

namespace _20230309_Adorner
{
    public class GeometryThumb : Thumb
    {
        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeometryThumb),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyShapeAngle
        {
            get { return (double)GetValue(MyShapeAngleProperty); }
            set { SetValue(MyShapeAngleProperty, value); }
        }
        public static readonly DependencyProperty MyShapeAngleProperty =
            DependencyProperty.Register(nameof(MyShapeAngle), typeof(double), typeof(GeometryThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        public double MyShapeScale
        {
            get { return (double)GetValue(MyShapeScaleProperty); }
            set { SetValue(MyShapeScaleProperty, value); }
        }
        public static readonly DependencyProperty MyShapeScaleProperty =
            DependencyProperty.Register(nameof(MyShapeScale), typeof(double), typeof(GeometryThumb),
                new FrameworkPropertyMetadata(1.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        #endregion 依存関係プロパティ

        public PolyGeoLine MyPolyGeoLine { get; set; }
        public GeometryThumbAdorner MyTTAdorner { get; private set; }


        public GeometryThumb()
        {
            MyTTAdorner = new GeometryThumbAdorner(this);
            MyPolyGeoLine = MySetTemplate();
            MySetBindings();
            Loaded += TThumb_Loaded;
        }


        private void TThumb_Loaded(object sender, RoutedEventArgs e)
        {
            if (AdornerLayer.GetAdornerLayer(this) is AdornerLayer layer)
            {
                layer.Add(MyTTAdorner);
            }
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            MyTTAdorner.MyExternalBounds.Arrange(MyPolyGeoLine.MyTransformedExternalBounds);
            MyTTAdorner.MyInternalBounds.Arrange(MyPolyGeoLine.MyTransformedInternalBounds);
            
            return base.GetLayoutClip(layoutSlotSize);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
        }

        private PolyGeoLine MySetTemplate()
        {
            FrameworkElementFactory shape = new(typeof(PolyGeoLine), "Nemo");
            this.Template = new() { VisualTree = shape };
            this.ApplyTemplate();
            if (this.Template.FindName("Nemo", this) is PolyGeoLine line)
            {
                return line;
            }
            else { throw new Exception(); }
        }

        private void MySetBindings()
        {
            MyPolyGeoLine.SetBinding(PolyGeoLine.PointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });
            MultiBinding mb = new() { Converter = new MyConverterRenderTransform() };
            Binding b0 = new() { Source = this, Path = new PropertyPath(MyShapeAngleProperty) };
            Binding b1 = new() { Source = this, Path = new PropertyPath(MyShapeScaleProperty) };
            mb.Bindings.Add(b0);
            mb.Bindings.Add(b1);
            MyPolyGeoLine.SetBinding(PolyGeoLine.RenderTransformProperty, mb);
        }
    }



    public class GeometryThumbAdorner : Adorner
    {
        public VisualCollection MyVisuals { get; private set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];

        public Rectangle MyExternalBounds { get; private set; } = new() { Stroke = Brushes.Lime, StrokeThickness = 1.0 };
        public Rectangle MyInternalBounds { get; private set; } = new() { Stroke = Brushes.Magenta, StrokeThickness = 1.0 };
        //public Canvas MyCanvas { get; private set; } = new();
        public GeometryThumb MyTargetGeoThumb { get; private set; }


        public GeometryThumbAdorner(GeometryThumb adornedElement) : base(adornedElement)
        {
            MyTargetGeoThumb = adornedElement;
            MyVisuals = new(this)
            {
                //MyCanvas,
                MyExternalBounds,
                MyInternalBounds,
            };
            //MyCanvas.Children.Add(MyExternalBounds);
            //MyCanvas.Children.Add(MyInternalBounds);

        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            MyExternalBounds.Arrange(MyTargetGeoThumb.MyPolyGeoLine.MyTransformedExternalBounds);
            MyInternalBounds.Arrange(MyTargetGeoThumb.MyPolyGeoLine.MyTransformedInternalBounds);
            return base.ArrangeOverride(finalSize);
        }
    }


    public class MyConverterRenderTransform : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double angle = (double)values[0];
            double scale = (double)values[1];
            RotateTransform rotate = new(angle);
            ScaleTransform scaleT = new(scale, scale);
            TransformGroup TG = new();
            TG.Children.Add(rotate);
            TG.Children.Add(scaleT);
            return TG;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
