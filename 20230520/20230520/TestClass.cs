using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows;


namespace _20230520
{
    public class ResizeCanvas : Canvas
    {
        public TThumb TTBR { get; set; } = new() { Width = 20, Height = 20 };
        public TThumb TTR { get; set; } = new() { Width = 20, Height = 20 };

        public ResizeCanvas()
        {
            Children.Add(TTBR);
            Children.Add(TTR);
            Background = Brushes.AliceBlue;
            TTBR.DragDelta += TTBR_DragDelta;
            TTR.DragDelta += TTR_DragDelta;
            Loaded += ResizeCanvas_Loaded;

        }

        private void ResizeCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            TTBR.SetBinding(TThumb.XProperty, new Binding() { Source = this, Path = new PropertyPath(WidthProperty), Mode = BindingMode.TwoWay });
            TTBR.SetBinding(TThumb.YProperty, new Binding() { Source = this, Path = new PropertyPath(HeightProperty), Mode = BindingMode.TwoWay });
            TTR.SetBinding(TThumb.XProperty, new Binding() { Source = this, Path = new PropertyPath(WidthProperty), Mode = BindingMode.TwoWay });
            TTR.SetBinding(TThumb.XProperty, new Binding() { Source = this, Path = new PropertyPath(WidthProperty), Mode = BindingMode.TwoWay });
            常に中間
        }

        private void TTR_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (TTR.X + e.HorizontalChange >= 0) TTR.X += e.HorizontalChange;
        }

        private void TTBR_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (TTBR.X + e.HorizontalChange >= 0) { TTBR.X += e.HorizontalChange; }
            if (TTBR.Y + e.VerticalChange >= 0) TTBR.Y += e.VerticalChange;
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
        public TThumb()
        {
            SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(XProperty), Mode = BindingMode.TwoWay });
            SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(YProperty), Mode = BindingMode.TwoWay });

        }
    }
}
