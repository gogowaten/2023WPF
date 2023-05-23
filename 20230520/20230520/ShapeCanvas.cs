﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

namespace _20230520
{
    public class PolyLineCanvas : Canvas
    {
        #region 依存関係プロパティ
        public PointCollection MyAnchirPoints
        {
            get { return (PointCollection)GetValue(MyAnchorPointsProperty); }
            set { SetValue(MyAnchorPointsProperty, value); }
        }
        public static readonly DependencyProperty MyAnchorPointsProperty =
            DependencyProperty.Register(nameof(MyAnchirPoints), typeof(PointCollection), typeof(PolyLineCanvas),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(PolyLineCanvas),
                new FrameworkPropertyMetadata(Brushes.Gold,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(PolyLineCanvas),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ
        public Polyline MyShape { get; private set; } = new();
        public ObservableCollection<Thumb> MyAnchorThumbs { get; private set; } = new();
        public Rect MyBound { get; private set; } = new();
        public PolyLineCanvas()
        {
            Background = Brushes.WhiteSmoke;
            Children.Add(MyShape);
            SetMyBindings();
            Loaded += PolyLineCanvas_Loaded;

        }

        private void PolyLineCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            SetMyAnchorThumbs();
        }

        private void SetMyBindings()
        {
            MyShape.SetBinding(Polyline.PointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyAnchorPointsProperty), Mode = BindingMode.TwoWay });
            MyShape.SetBinding(Polyline.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeProperty), Mode = BindingMode.TwoWay });
            MyShape.SetBinding(Polyline.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty), Mode = BindingMode.TwoWay });

        }

        private void SetMyAnchorThumbs()
        {
            for (int i = 0; i < MyAnchirPoints.Count; i++)
            {
                Thumb tt = new() { Width = 20, Height = 20, };
                Point pp = MyAnchirPoints[i];
                Canvas.SetLeft(tt, pp.X);
                Canvas.SetTop(tt, pp.Y);
                MyAnchorThumbs.Add(tt);
                Children.Add(tt);
                tt.DragDelta += Tt_DragDelta;
            }
            SetBoungs();
        }

        private void SetBoungs()
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(MyShape);
            MyBound = bounds;
            Rect bounds2 = VisualTreeHelper.GetContentBounds(MyShape);
            Rect bounds3 = VisualTreeHelper.GetDescendantBounds(this);
        }

        private void Tt_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb tt)
            {
                int ii = MyAnchorThumbs.IndexOf(tt);
                double xx = Canvas.GetLeft(tt) + e.HorizontalChange;
                double yy = Canvas.GetTop(tt) + e.VerticalChange;
                Canvas.SetLeft(tt, xx);
                Canvas.SetTop(tt, yy);
                MyAnchirPoints[ii] = new Point(xx, yy);
                SetBoungs();
            }
        }
    }
}
