using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Runtime.CompilerServices;

namespace _20230620
{
    /// <summary>
    /// LayoutUpdatedで強制的に再測定するようにした
    /// これでActual系も更新される
    /// </summary>
    public class TTPolylineThumb : TThumb
    {
        #region 依存関係プロパティ


        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TTPolylineThumb),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(TTPolylineThumb),
                new FrameworkPropertyMetadata(Brushes.DodgerBlue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(TTPolylineThumb),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存関係プロパティ

        public Canvas MyTemplate { get; set; }
        public GeoPolyLine MyGeoPolyLine { get; set; } = new();
        public TTPolylineThumb()
        {
            MyTemplate = SetMyTemplate<Canvas>();
            MyTemplate.Children.Add(MyGeoPolyLine);
            //Canvas.SetLeft(this, 0);
            //Canvas.SetTop(this, 0);
            SetMyBindings();
            
            LayoutUpdated += TTPolylineThumb_LayoutUpdated;
        }

        /// <summary>
        /// 強制的にMeasure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TTPolylineThumb_LayoutUpdated(object? sender, EventArgs e)
        {
            var w = Width; var h = Height;
            var aw = ActualWidth; var ah = ActualHeight;
            if (w != aw || h != ah) { InvalidateMeasure(); }

        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }
        //private void TTPolylineThumb_SizeChanged(object sender, SizeChangedEventArgs e)
        //{

        //}

        //protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        //{
        //    base.OnRenderSizeChanged(sizeInfo);
        //    var w = Width; var h = Height;
        //    if (w != sizeInfo.NewSize.Width) { InvalidateMeasure(); }
        //    var des = DesiredSize;
        //}

        private void SetMyBindings()
        {
            MyGeoPolyLine.SetBinding(GeoPolyLine.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.TwoWay });
            MyGeoPolyLine.SetBinding(GeoPolyLine.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeProperty), Mode = BindingMode.TwoWay });
            MyGeoPolyLine.SetBinding(GeoPolyLine.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty), Mode = BindingMode.TwoWay });

            SetBinding(WidthProperty, new Binding() { Source = MyGeoPolyLine, Path = new PropertyPath(GeoPolyLine.MyRenderRect2Property), Converter = new MyConverterRectWidth() });
            SetBinding(HeightProperty, new Binding() { Source = MyGeoPolyLine, Path = new PropertyPath(GeoPolyLine.MyRenderRect2Property), Converter = new MyConverterRectHeight() });

            //SetBinding(Canvas.LeftProperty, new Binding() { Source = MyGeoPolyLine, Path = new PropertyPath(GeoPolyLine.MyRenderRect2Property), Converter = new MyConverterRectLeft() });
            //SetBinding(Canvas.TopProperty, new Binding() { Source = MyGeoPolyLine, Path = new PropertyPath(GeoPolyLine.MyRenderRect2Property), Converter = new MyConverterRectTop() });


        }
        private T SetMyTemplate<T>()
        {
            FrameworkElementFactory factory = new(typeof(T), "nemo");
            Template = new ControlTemplate() { VisualTree = factory };
            ApplyTemplate();
            return (T)Template.FindName("nemo", this);
        }
    }

    public class TThumb : Thumb
    {

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
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool MyIsMove { get; set; } = true;
        public TThumb()
        {
            SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(XProperty), Mode = BindingMode.TwoWay });
            SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(YProperty), Mode = BindingMode.TwoWay });
            DragDelta += TThumb_DragDelta;
        }

        private void TThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (MyIsMove)
            {
                X += e.HorizontalChange;
                Y += e.VerticalChange;
            }
        }
    }


}
