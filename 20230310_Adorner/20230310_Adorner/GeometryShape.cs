﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;



namespace _20230310_Adorner
{
    public abstract class GeometryShape : Shape, INotifyPropertyChanged
    {
        #region 依存関係プロパティと通知プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeometryShape),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //以下必要？
        private Rect _myBounds;
        public Rect MyBounds { get => _myBounds; set => SetProperty(ref _myBounds, value); }

        private Rect _myTFBounds;
        public Rect MyTFBounds { get => _myTFBounds; set => SetProperty(ref _myTFBounds, value); }

        private double _myTFWidth;
        public double MyTFWidth { get => _myTFWidth; set => SetProperty(ref _myTFWidth, value); }

        private double _myTFHeight;
        public double MyTFHeight { get => _myTFHeight; set => SetProperty(ref _myTFHeight, value); }


        #endregion 依存関係プロパティと通知プロパティ




        public Geometry MyGeometry { get; protected set; } = new PathGeometry();
        public Rect MyExternalBounds { get; protected set; }//外観のRect、変形なし時
        public Rect MyExternalTFBounds { get; protected set; }//外観のRect、変形加味
        public Rect MyInternalBounds { get; protected set; }//Pointsだけ(内部)のRect、変形なし時


        public List<Thumb> MyThumbs { get; protected set; } = new();
        public GeometryAdorner MyAdorner { get; protected set; }
        //いる？
        public AdornerLayer? MyAdornerLayer { get; protected set; }

        public GeometryShape()
        {
            MyAdorner = new GeometryAdorner(this);
            Loaded += GeometryShapeBase_Loaded;
        }

        public void ChangeAdornerVisible()
        {
            if (MyAdorner.Visibility != Visibility.Visible)
            {
                MyAdorner.Visibility = Visibility.Visible;
            }
            else
            {
                MyAdorner.Visibility = Visibility.Collapsed;
            }
        }

        public void UpdateAdorner()
        {
            if (MyAdornerLayer != null)
            {
                var adols = MyAdornerLayer.GetAdorners(this);
                foreach (var adol in adols)
                {
                    MyAdornerLayer.Remove(adol);
                }
                MyAdornerLayer.Add(new GeometryAdorner(this));
            }
        }
        private void GeometryShapeBase_Loaded(object sender, RoutedEventArgs e)
        {
            MyAdornerLayer = AdornerLayer.GetAdornerLayer(this);
            MyAdornerLayer.Add(MyAdorner);
            //MyAdornerLayer.Add(new BoundsAdorner(this));
        }

        protected override Geometry DefiningGeometry => Geometry.Empty;


        //各種Bounds更新
        protected override Size ArrangeOverride(Size finalSize)
        {
            Geometry geo = MyGeometry.Clone();
            geo.Transform = RenderTransform;
            MyInternalBounds = geo.Bounds;
            MyExternalBounds = geo.GetWidenedPathGeometry(new Pen(Stroke, StrokeThickness)).Bounds;
            MyExternalTFBounds = this.RenderTransform.TransformBounds(MyExternalBounds);
            MyBounds = MyExternalBounds;
            MyTFBounds = MyExternalTFBounds;
            MyTFWidth = MyExternalTFBounds.Width;
            MyTFHeight = MyExternalTFBounds.Height;
            return base.ArrangeOverride(finalSize);
        }
    }

    public class GeometryLine : GeometryShape
    {
        public GeometryLine()
        {
            Stroke = Brushes.Orange;
            StrokeThickness = 20;
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints.Count == 0) return Geometry.Empty;
                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    context.BeginFigure(MyPoints[0], false, false);
                    context.PolyLineTo(MyPoints.Skip(1).ToArray(), true, false);

                }
                geometry.Freeze();
                MyGeometry = geometry;

                return geometry;
                //return base.DefiningGeometry;
            }
        }
    }


    public class GeometryPolygon : GeometryShape { }
    public class GeometryBezier : GeometryShape { }



    public class BoundsAdorner : Adorner
    {
        public VisualCollection MyVisuals { get; private set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];


        public Rectangle MyBoundsRed { get; private set; } = new() { Stroke = Brushes.Red, StrokeThickness = 1.0 };
        public Rectangle MyBoundsBlue { get; private set; } = new() { Stroke = Brushes.Blue, StrokeThickness = 1.0 };
        public Rectangle MyBoundsGreen { get; private set; } = new() { Stroke = Brushes.Green, StrokeThickness = 1.0 };
        public GeometryShape MyTargetGeoShape { get; private set; }


        public BoundsAdorner(GeometryShape adornedElement) : base(adornedElement)
        {
            MyVisuals = new VisualCollection(this)
            {
                MyBoundsRed,
                MyBoundsBlue,
                MyBoundsGreen,
            };
            MyTargetGeoShape = adornedElement;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (MyTargetGeoShape.ActualHeight != 0)
            {
                MyBoundsRed.Arrange(MyTargetGeoShape.MyInternalBounds);
                MyBoundsBlue.Arrange(MyTargetGeoShape.MyExternalBounds);
            }
            return base.ArrangeOverride(finalSize);
        }
        //protected override Geometry GetLayoutClip(Size layoutSlotSize)
        //{
        //    MyBoundsRed.Arrange(MyTargetGeoShape.MyInternalBounds);
        //    MyRectangleBlue.Arrange(MyTargetGeoShape.MyExternalBounds);
        //    return base.GetLayoutClip(layoutSlotSize);
        //}
    }


    /// <summary>
    /// 頂点座標にThumb表示するアドーナー。GeometryShape専用
    /// </summary>
    public class GeometryAdorner : Adorner
    {
        public VisualCollection MyVisuals { get; private set; }
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];

        public Canvas MyCanvas { get; private set; } = new();
        public List<Thumb> MyThumbs { get; private set; } = new();
        public GeometryShape MyTargetGeoShape { get; private set; }
        public GeometryAdorner(GeometryShape adornedElement) : base(adornedElement)
        {
            MyVisuals = new VisualCollection(this)
            {
                MyCanvas
            };
            MyTargetGeoShape = adornedElement;
            Loaded += GeometryAdorner_Loaded;
        }

        private void GeometryAdorner_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in MyTargetGeoShape.MyPoints)
            {
                Thumb thumb = new() { Width = 20, Height = 20, Background = Brushes.Red, Opacity = 0.5 };
                MyThumbs.Add(thumb);
                MyCanvas.Children.Add(thumb);
                SetLocate(thumb, item);
                thumb.DragDelta += Thumb_DragDelta;
            }
        }


        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                int i = MyThumbs.IndexOf(t);
                PointCollection points = MyTargetGeoShape.MyPoints;
                double x = points[i].X + e.HorizontalChange;
                double y = points[i].Y + e.VerticalChange;
                points[i] = new Point(x, y);
                SetLocate(t, points[i]);
            }
        }

        private static void SetLocate(Thumb thumb, Point point)
        {
            Canvas.SetLeft(thumb, point.X);
            Canvas.SetTop(thumb, point.Y);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect canvasRect = VisualTreeHelper.GetDescendantBounds(MyCanvas);
            if (canvasRect.IsEmpty)
            {
                MyCanvas.Arrange(new Rect(finalSize));
            }
            else
            {
                //座標を0,0したRectにする、こうしないとマイナス座表示に不具合
                canvasRect = new(canvasRect.Size);
                MyCanvas.Arrange(canvasRect);
            }
            return base.ArrangeOverride(finalSize);
        }

    }

    public class MyConverterTransform : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double angle = (double)values[0];
            double scale = (double)values[1];
            TransformGroup group = new();
            group.Children.Add(new RotateTransform(angle));
            group.Children.Add(new ScaleTransform(scale, scale));
            return group;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
