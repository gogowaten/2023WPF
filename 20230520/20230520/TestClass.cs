using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace _20230520
{
    public class ResizeCanvas : Canvas
    {

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(ResizeCanvas),
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
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(ResizeCanvas),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public TThumb TTR { get; set; } = new() { Width = 20, Height = 20 };
        public TThumb TTB { get; set; } = new() { Width = 20, Height = 20 };
        public TThumb TTL { get; set; } = new() { Width = 20, Height = 20 };
        public TThumb TTT { get; set; } = new() { Width = 20, Height = 20 };
        public TThumb TTTR { get; set; } = new() { Width = 20, Height = 20 };
        public TThumb TTBL { get; set; } = new() { Width = 20, Height = 20 };
        public TThumb TTBR { get; set; } = new() { Width = 20, Height = 20 };
        public TThumb TTTL { get; set; } = new() { Width = 20, Height = 20 };


        public ResizeCanvas()
        {
            Background = Brushes.AliceBlue;
            Loaded += ResizeCanvas_Loaded;
            Children.Add(TTR); TTR.DragDelta += (o, e) => { TTWidth(e); };
            Children.Add(TTB); TTB.DragDelta += (o, e) => { TTHeight(e); };
            Children.Add(TTL); TTL.DragDelta += (o, e) => { TTWidthAndX(e); };
            Children.Add(TTT); TTT.DragDelta += (o, e) => { TTHeightAndY(e); };
            Children.Add(TTTR); TTTR.DragDelta += (o, e) => { TTHeightAndY(e); TTWidth(e); };
            Children.Add(TTBL); TTBL.DragDelta += (o, e) => { TTWidthAndX(e); TTHeight(e); };
            Children.Add(TTBR); TTBR.DragDelta += (o, e) => { TTWidth(e); TTHeight(e); };
            Children.Add(TTTL); TTTL.DragDelta += (o, e) => { TTWidthAndX(e); TTHeightAndY(e); };
        }


        private void TTWidthAndX(DragDeltaEventArgs e)
        {
            double value = Width - e.HorizontalChange;
            if (value >= 0)
            {
                X += e.HorizontalChange;
                Width = value;
            }
            else
            {
                X += Width;
                Width = 0;
            }
        }
        private void TTHeightAndY(DragDeltaEventArgs e)
        {
            double value = Height - e.VerticalChange;
            if (value >= 0)
            {
                Y += e.VerticalChange;
                Height = value;
            }
            else
            {
                Y += Height;
                Height = 0;
            }
        }

        private void TTHeight(DragDeltaEventArgs e)
        {
            double value = Height + e.VerticalChange;
            Height = value >= 0 ? value : 0;
        }

        private void TTWidth(DragDeltaEventArgs e)
        {
            double value = Width + e.HorizontalChange;
            Width = value >= 0 ? value : 0;
        }


        private void ResizeCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(XProperty), Mode = BindingMode.TwoWay });
            SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(YProperty), Mode = BindingMode.TwoWay });

            TTBR.SetBinding(TThumb.XProperty, new Binding() { Source = this, Path = new PropertyPath(WidthProperty) });
            TTBR.SetBinding(TThumb.YProperty, new Binding() { Source = this, Path = new PropertyPath(HeightProperty) });
            TTR.SetBinding(TThumb.XProperty, new Binding() { Source = this, Path = new PropertyPath(WidthProperty) });
            TTR.SetBinding(TThumb.YProperty, new Binding() { Source = this, Path = new PropertyPath(HeightProperty), Converter = new MyConverterHalf() });
            TTTR.SetBinding(TThumb.XProperty, new Binding() { Source = this, Path = new PropertyPath(WidthProperty) });
            TTT.SetBinding(TThumb.XProperty, new Binding() { Source = this, Path = new PropertyPath(WidthProperty), Converter = new MyConverterHalf() });
            TTL.SetBinding(TThumb.YProperty, new Binding() { Source = this, Path = new PropertyPath(HeightProperty), Converter = new MyConverterHalf() });
            TTBL.SetBinding(TThumb.YProperty, new Binding() { Source = this, Path = new PropertyPath(HeightProperty) });
            TTB.SetBinding(TThumb.XProperty, new Binding() { Source = this, Path = new PropertyPath(WidthProperty), Converter = new MyConverterHalf() });
            TTB.SetBinding(TThumb.YProperty, new Binding() { Source = this, Path = new PropertyPath(HeightProperty) });

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

    public class MyConverterHalf : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double v = (double)value;
            return v / 2.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
