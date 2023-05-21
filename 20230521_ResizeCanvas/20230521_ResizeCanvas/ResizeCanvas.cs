using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;
using System.Globalization;

//ResizeCanvasのシンプル版
//リサイズできるCanvas
//ハンドルの座標はCanvasのサイズと座標にバインド
//ハンドルのドラッグイベントでCanvasのサイズと座標を変更する
//このときCanvasのサイズがマイナスにならないようにしている

namespace _20230521_ResizeCanvas
{
    public class ResizeCanvas2 : Canvas
    {
        //R:Right, L:Left, T:Top, B:Bottom
        public Thumb TTR { get; set; } = new() { Width = 20, Height = 20 };
        public Thumb TTB { get; set; } = new() { Width = 20, Height = 20 };
        public Thumb TTL { get; set; } = new() { Width = 20, Height = 20 };
        public Thumb TTT { get; set; } = new() { Width = 20, Height = 20 };
        public Thumb TTTR { get; set; } = new() { Width = 20, Height = 20 };
        public Thumb TTBL { get; set; } = new() { Width = 20, Height = 20 };
        public Thumb TTBR { get; set; } = new() { Width = 20, Height = 20 };
        public Thumb TTTL { get; set; } = new() { Width = 20, Height = 20 };


        public ResizeCanvas2()
        {
            Background = Brushes.WhiteSmoke;
            Children.Add(TTR); TTR.DragDelta += (o, e) => { TTWidth(e); };
            Children.Add(TTB); TTB.DragDelta += (o, e) => { TTHeight(e); };
            Children.Add(TTL); TTL.DragDelta += (o, e) => { TTWidthAndX(e); };
            Children.Add(TTT); TTT.DragDelta += (o, e) => { TTHeightAndY(e); };
            Children.Add(TTTR); TTTR.DragDelta += (o, e) => { TTHeightAndY(e); TTWidth(e); };
            Children.Add(TTBL); TTBL.DragDelta += (o, e) => { TTWidthAndX(e); TTHeight(e); };
            Children.Add(TTBR); TTBR.DragDelta += (o, e) => { TTWidth(e); TTHeight(e); };
            Children.Add(TTTL); TTTL.DragDelta += (o, e) => { TTWidthAndX(e); TTHeightAndY(e); };

            SetMyBinding();
        }

        private void TTWidthAndX(DragDeltaEventArgs e)
        {
            double value = Width - e.HorizontalChange;
            if (value >= 0)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                Width = value;
            }
            else
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + Width);
                Width = 0;
            }
        }
        private void TTHeightAndY(DragDeltaEventArgs e)
        {
            double value = Height - e.VerticalChange;
            if (value >= 0)
            {
                Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
                Height = value;
            }
            else
            {
                Canvas.SetTop(this, Canvas.GetTop(this) + Height);
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


        private void SetMyBinding()
        {
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
            TTR.SetBinding(Canvas.LeftProperty, bWidth);
            TTR.SetBinding(Canvas.TopProperty, bHeightC);
            TTB.SetBinding(Canvas.LeftProperty, bWidthC);
            TTB.SetBinding(Canvas.TopProperty, bHeight);
            TTT.SetBinding(Canvas.LeftProperty, bWidthC);
            TTL.SetBinding(Canvas.TopProperty, bHeightC);
            TTBR.SetBinding(Canvas.LeftProperty, bWidth);
            TTBR.SetBinding(Canvas.TopProperty, bHeight);
            TTTR.SetBinding(Canvas.LeftProperty, bWidth);
            TTBL.SetBinding(Canvas.TopProperty, bHeight);

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
