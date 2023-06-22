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
    /// Thumb(CanvasTemplate)	
    ///     ┗Shape
    /// ThumbのサイズをShapeのRenderSizeにバインド
    /// Shapeの座標をオフセットしていないのでStrokeThicknessのぶんだけ
    /// 表示位置がずれる、これは仕様にしてもいいかも？
    /// 問題点
    ///     Thumbの座標を1回でも0,0以外にしないとバインドが一手遅れるので
    ///     Loadedイベント時に一度0,0以外にしている
    /// </summary>
    public class TTPolylineThumb2 : TThumb
    {
        #region 依存関係プロパティ


        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TTPolylineThumb2),
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
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(TTPolylineThumb2),
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
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(TTPolylineThumb2),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存関係プロパティ

        public Canvas MyTemplate { get; set; }
        public GeoPolyLine MyGeoPolyLine { get; set; } = new();
        public TTPolylineThumb2()
        {
            MyTemplate = SetMyTemplate<Canvas>();
            MyTemplate.Children.Add(MyGeoPolyLine);
            MyTemplate.Background = Brushes.Blue;
            SetMyBindings();
            Loaded += TTPolylineThumb_Loaded;
            
        }


        private void TTPolylineThumb_Loaded(object sender, RoutedEventArgs e)
        {
            //一度だけ座標を0,0以外にしないとActual系がなぜか一手遅れるのでその処理
            X = 1; X = 0;
        }

        private void SetMyBindings()
        {
            MyGeoPolyLine.SetBinding(GeoPolyLine.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.TwoWay });
            MyGeoPolyLine.SetBinding(GeoPolyLine.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeProperty), Mode = BindingMode.TwoWay });
            MyGeoPolyLine.SetBinding(GeoPolyLine.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty), Mode = BindingMode.TwoWay });

            SetBinding(WidthProperty, new Binding() { Source = MyTemplate, Path = new PropertyPath(WidthProperty)});
            SetBinding(HeightProperty, new Binding() { Source = MyTemplate, Path = new PropertyPath(HeightProperty) });

            MyTemplate.SetBinding(WidthProperty, new Binding(nameof(GeoPolyLine.MyRenderWidth)) { Source = MyGeoPolyLine });
            MyTemplate.SetBinding(HeightProperty, new Binding(nameof(GeoPolyLine.MyRenderHeight)) { Source = MyGeoPolyLine });
            
        }
        private T SetMyTemplate<T>()
        {
            FrameworkElementFactory factory = new(typeof(T), "nemo");
            Template = new ControlTemplate() { VisualTree = factory };
            ApplyTemplate();
            return (T)Template.FindName("nemo", this);
        }
    }


    /// <summary>
    /// Thumb(ResizableCanvasTemplate)	
    ///     ┗Shape
    /// 問題点
    ///     サイズ変更ThumbがShapeの下に表示される
    ///     Thumb座標そのままでShapeをドラッグ移動できない、
    ///         座標を変更するにはPoints全体を変更するしかない
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

        public ResizableCanvas MyTemplate { get; set; }
        public GeoPolyLine MyGeoPolyLine { get; set; } = new();
        public TTPolylineThumb()
        {
            MyTemplate = SetMyTemplate<ResizableCanvas>();
            MyTemplate.Children.Add(MyGeoPolyLine);
            MyTemplate.Background = Brushes.Blue;
            SetMyBindings();
            Loaded += TTPolylineThumb_Loaded;
        }

        private void TTPolylineThumb_Loaded(object sender, RoutedEventArgs e)
        {
            Width = MyGeoPolyLine.MyRenderRect.Width;
            Height = MyGeoPolyLine.MyRenderRect.Height;
        }

        private void SetMyBindings()
        {
            MyGeoPolyLine.SetBinding(GeoPolyLine.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.TwoWay });
            MyGeoPolyLine.SetBinding(GeoPolyLine.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeProperty), Mode = BindingMode.TwoWay });
            MyGeoPolyLine.SetBinding(GeoPolyLine.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty), Mode = BindingMode.TwoWay });

            SetBinding(WidthProperty, new Binding() { Source = MyTemplate, Path = new PropertyPath(WidthProperty), Mode = BindingMode.TwoWay });
            SetBinding(HeightProperty, new Binding() { Source = MyTemplate, Path = new PropertyPath(HeightProperty), Mode = BindingMode.TwoWay });
            MyTemplate.SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(XProperty), Mode = BindingMode.TwoWay });
            MyTemplate.SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(YProperty), Mode = BindingMode.TwoWay });

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
            if (MyIsMove && e.OriginalSource == sender)
            {
                X += e.HorizontalChange;
                Y += e.VerticalChange;
            }
        }
    }


}
