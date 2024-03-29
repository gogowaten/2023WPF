﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace _20230520
{
    //ResizeCanvas2の改変、ハンドルサイズのプロパティを追加した
    //リサイズできるCanvas
    //ハンドルの座標はCanvasのサイズと座標にバインド
    //ハンドルのドラッグイベントでCanvasのサイズと座標を変更する
    //このときCanvasのサイズがマイナスにならないようにしている
    public class ResizableCanvas : Canvas
    {
        #region 依存関係プロパティ

        public double MyHandlThumbSize
        {
            get { return (double)GetValue(MyHandlThumbSizeProperty); }
            set { SetValue(MyHandlThumbSizeProperty, value); }
        }
        public static readonly DependencyProperty MyHandlThumbSizeProperty =
            DependencyProperty.Register(nameof(MyHandlThumbSize), typeof(double), typeof(ResizableCanvas),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ

        //R:Right, L:Left, T:Top, B:Bottom
        public Thumb TTR { get; set; } = new();
        public Thumb TTB { get; set; } = new();
        public Thumb TTL { get; set; } = new();
        public Thumb TTT { get; set; } = new();
        public Thumb TTTR { get; set; } = new();
        public Thumb TTBL { get; set; } = new();
        public Thumb TTBR { get; set; } = new();
        public Thumb TTTL { get; set; } = new();


        public ResizableCanvas()
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
                SetLeft(this, GetLeft(this) + e.HorizontalChange);
                Width = value;
            }
            else
            {
                SetLeft(this, GetLeft(this) + Width);
                Width = 0;
            }
        }
        private void TTHeightAndY(DragDeltaEventArgs e)
        {
            double value = Height - e.VerticalChange;
            if (value >= 0)
            {
                SetTop(this, GetTop(this) + e.VerticalChange);
                Height = value;
            }
            else
            {
                SetTop(this, GetTop(this) + Height);
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

            //ハンドルサイズ
            Binding handlSize = new Binding() { Source = this, Path = new PropertyPath(MyHandlThumbSizeProperty) };
            TTT.SetBinding(WidthProperty, handlSize); TTT.SetBinding(HeightProperty, handlSize);
            TTB.SetBinding(WidthProperty, handlSize); TTB.SetBinding(HeightProperty, handlSize);
            TTL.SetBinding(WidthProperty, handlSize); TTL.SetBinding(HeightProperty, handlSize);
            TTR.SetBinding(WidthProperty, handlSize); TTR.SetBinding(HeightProperty, handlSize);
            TTTL.SetBinding(WidthProperty, handlSize); TTTL.SetBinding(HeightProperty, handlSize);
            TTTR.SetBinding(WidthProperty, handlSize); TTTR.SetBinding(HeightProperty, handlSize);
            TTBR.SetBinding(WidthProperty, handlSize); TTBR.SetBinding(HeightProperty, handlSize);
            TTBL.SetBinding(WidthProperty, handlSize); TTBL.SetBinding(HeightProperty, handlSize);

        }

    }

}
