﻿using System;
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
    public class TThumb : Thumb
    {
        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TThumb),
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
            DependencyProperty.Register(nameof(MyShapeAngle), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        public double MyShapeScale
        {
            get { return (double)GetValue(MyShapeScaleProperty); }
            set { SetValue(MyShapeScaleProperty, value); }
        }
        public static readonly DependencyProperty MyShapeScaleProperty =
            DependencyProperty.Register(nameof(MyShapeScale), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(1.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        #endregion 依存関係プロパティ

        public PolyGeoLine MyPolyGeoLine { get; set; }
        public Rect RenderSizeEx { get; private set; }
        public Rect RenderSizeExEx { get; private set; }
        public Rect GeoBounds { get; private set; }
        public Rect GeoWideBounds { get; private set; }
        public Rect GeoWideTFBounds { get; private set; }
        public Rect GeoTF_WideBounds { get; private set; }
        public TTAdorner MyTTAdorner { get; private set; }


        public TThumb()
        {
            MyTTAdorner = new TTAdorner(this);
            MyPolyGeoLine = MySetTemplate();
            MySetBindings();
            Loaded += TThumb_Loaded;
        }

        private void TThumb_Loaded(object sender, RoutedEventArgs e)
        {
            if(AdornerLayer.GetAdornerLayer(this) is AdornerLayer layer)
            {
                layer.Add(MyTTAdorner);
            }
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            MyTTAdorner.MyRectangle.Arrange(MyPolyGeoLine.MyBounds);
            return base.GetLayoutClip(layoutSlotSize);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Transform RTF = MyPolyGeoLine.RenderTransform;
            Pen pen = new(MyPolyGeoLine.Stroke, MyPolyGeoLine.StrokeThickness);

            RenderSizeEx = VisualTreeHelper.GetDescendantBounds(MyPolyGeoLine);
            RenderSizeExEx = RTF.TransformBounds(RenderSizeEx);

            var geo = MyPolyGeoLine.MyGeometry.Clone();
            GeoBounds = geo.Bounds;
            var wide = geo.GetWidenedPathGeometry(pen);
            GeoWideBounds = wide.Bounds;
            var wideTF = RTF.TransformBounds(wide.Bounds);
            GeoWideTFBounds = wideTF;

            geo.Transform = RTF;
            var TFWide = geo.GetWidenedPathGeometry(pen);
            GeoTF_WideBounds = TFWide.Bounds;

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



    public class TTAdorner : Adorner
    {
        public VisualCollection MyVisuals { get; private set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];
        public Rectangle MyRectangle { get; private set; }
        public Canvas MyCanvas { get; private set; }
        public TThumb MyTThumb { get; private set; }


        public TTAdorner(TThumb adornedElement) : base(adornedElement)
        {
            MyTThumb = adornedElement;
            MyVisuals = new(this);
            MyCanvas = new();
            MyVisuals.Add(MyCanvas);
            MyRectangle = new() { Stroke = Brushes.Lime, StrokeThickness = 1.0 };
            MyCanvas.Children.Add(MyRectangle);

        }

     
        protected override Size ArrangeOverride(Size finalSize)
        {
            MyRectangle.Arrange(MyTThumb.MyPolyGeoLine.MyBounds);
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