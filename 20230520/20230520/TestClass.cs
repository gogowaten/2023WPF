using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

//リサイズできるCanvas
//ハンドルの座標はCanvasのサイズと座標にバインド
//ハンドルのドラッグイベントでCanvasのサイズと座標を変更する
//このときCanvasのサイズがマイナスにならないようにしている
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

        //R:Right, L:Left, T:Top, B:Bottom
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

            Children.Add(TTR); TTR.DragDelta += (o, e) => { TTWidth(e); };
            Children.Add(TTB); TTB.DragDelta += (o, e) => { TTHeight(e); };
            Children.Add(TTL); TTL.DragDelta += (o, e) => { TTWidthAndX(e); };
            Children.Add(TTT); TTT.DragDelta += (o, e) => { TTHeightAndY(e); };
            Children.Add(TTTR); TTTR.DragDelta += (o, e) => { TTHeightAndY(e); TTWidth(e); };
            Children.Add(TTBL); TTBL.DragDelta += (o, e) => { TTWidthAndX(e); TTHeight(e); };
            Children.Add(TTBR); TTBR.DragDelta += (o, e) => { TTWidth(e); TTHeight(e); };
            Children.Add(TTTL); TTTL.DragDelta += (o, e) => { TTWidthAndX(e); TTHeightAndY(e); };
            SetMyBindig();
        }

        private void SetMyBindig()
        {
            SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(XProperty), Mode = BindingMode.TwoWay });
            SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(YProperty), Mode = BindingMode.TwoWay });

            Binding bWidth = new()
            {
                Source = this,
                Path = new PropertyPath(WidthProperty)
            };
            Binding bWidthC = new()
            {
                Source = this,
                Path = new PropertyPath(WidthProperty),
                Converter = new MyConverterHalf()
            };
            Binding bHeight = new()
            {
                Source = this,
                Path = new PropertyPath(HeightProperty)
            };
            Binding bHeightC = new()
            {
                Source = this,
                Path = new PropertyPath(HeightProperty),
                Converter = new MyConverterHalf()
            };
            TTR.SetBinding(TThumb.XProperty, bWidth);
            TTR.SetBinding(TThumb.YProperty, bHeightC);
            TTT.SetBinding(TThumb.XProperty, bWidthC);
            TTL.SetBinding(TThumb.YProperty, bHeightC);
            TTB.SetBinding(TThumb.XProperty, bWidthC);
            TTB.SetBinding(TThumb.YProperty, bHeight);
            TTBR.SetBinding(TThumb.XProperty, bWidth);
            TTBR.SetBinding(TThumb.YProperty, bHeight);
            TTTR.SetBinding(TThumb.XProperty, bWidth);
            TTBL.SetBinding(TThumb.YProperty, bHeight);
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

    public class MyConverterHandlThumb : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double handlSize= (double)values[0];
            double v= (double)values[1];
            return v - handlSize;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterHandlThumbHalf : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double handlSize = (double)values[0];
            double v = (double)values[1];
            //return (v / 2.0) - (handlSize / 2.0);
            return (v + handlSize) / 2.0 - handlSize;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
