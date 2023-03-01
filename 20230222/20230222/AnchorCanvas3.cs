using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace _20230222
{
    public class AnchorCanvas3 : Canvas
    {
        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(AnchorCanvas3),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Visibility MyAnchorVisible
        {
            get { return (Visibility)GetValue(MyAnchorVisibleProperty); }
            set { SetValue(MyAnchorVisibleProperty, value); }
        }
        public static readonly DependencyProperty MyAnchorVisibleProperty =
            DependencyProperty.Register(nameof(MyAnchorVisible), typeof(Visibility), typeof(AnchorCanvas3),
                new FrameworkPropertyMetadata(Visibility.Collapsed,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(AnchorCanvas3),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(AnchorCanvas3),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public ObservableCollection<AnchorThumb> MyAnchorThumbs { get; set; } = new();

        public Polyline MyShape { get; set; }
        public ContextMenu MyAnchorMenu { get; set; } = new();
        public ContextMenu MyMenu { get; set; } = new();
        public AnchorThumb? MyCurrentAnchorThumb { get; set; }
        public int DragPointIndex;
        public Point MyClickedPoint;

        public AnchorCanvas3()
        {
            this.Background = Brushes.Transparent;
            MyShape = new Polyline();
            Children.Add(MyShape);
            MyShape.Stroke = Brushes.OliveDrab;
            MyShape.StrokeThickness = 20;

            Loaded += AnchorCanvas3_Loaded;

            SetBinding(WidthProperty, new Binding() { Source = MyShape, Path = new PropertyPath(ActualWidthProperty) });
            SetBinding(HeightProperty, new Binding() { Source = MyShape, Path = new PropertyPath(ActualHeightProperty) });
            SetBinding(MyPointsProperty, new Binding() { Source = MyShape, Path = new PropertyPath(Polyline.PointsProperty) });

            MenuItem item = new() { Header = "削除" };
            item.Click += (o, e) => { RemovePoint(MyCurrentAnchorThumb); };
            MyAnchorMenu.Items.Add(item);

            this.ContextMenu = MyMenu;
            item = new() { Header = "ここに追加" };
            item.Click += (o, e) => { AddPoint(MyClickedPoint); };
            MyMenu.Items.Add(item);
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            MyClickedPoint = Mouse.GetPosition(this);
        }


        private void AnchorCanvas3_Loaded(object sender, RoutedEventArgs e)
        {
            //MyShape.Points = MyPoints;
            foreach (var item in MyPoints)
            {
                AddAnchorThumb(item);
            }
        }

        public void AddPoint(Point point)
        {
            InsertPoint(point, MyPoints.Count);
        }
        public void RemovePoint(int pointIndex)
        {
            if (pointIndex < 0) { return; }
            MyPoints.RemoveAt(pointIndex);
            AnchorThumb thumb = MyAnchorThumbs[pointIndex];
            MyAnchorThumbs.Remove(thumb);
            Children.Remove(thumb);
            MyCurrentAnchorThumb = null;
        }
        public void RemovePoint(AnchorThumb? thumb)
        {
            if (thumb == null) { return; }
            RemovePoint(DragPointIndex);
        }

        public void InsertPoint(Point point, int i)
        {
            MyPoints.Insert(i, point);
            InsertAnchorThumb(point, i);
        }
        private void AddAnchorThumb(Point point)
        {
            InsertAnchorThumb(point, MyAnchorThumbs.Count);
        }
        private void InsertAnchorThumb(Point point, int i)
        {
            AnchorThumb thumb = new(point);
            MyAnchorThumbs.Insert(i, thumb);
            Children.Add(thumb);
            thumb.DragDelta += Thumb_DragDelta3;
            thumb.SetBinding(VisibilityProperty, new Binding() { Source = this, Path = new PropertyPath(MyAnchorVisibleProperty) });
            thumb.ContextMenu = MyAnchorMenu;
            thumb.PreviewMouseDown += Thumb_PreviewMouseDown;
        }

        private void Thumb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is AnchorThumb thumb)
            {
                MyCurrentAnchorThumb = thumb;
                DragPointIndex = MyAnchorThumbs.IndexOf(thumb);
            }
            else { DragPointIndex = -1; }
        }

        /// <summary>
        /// 移動に本体のサイズ追従＋本体の座標修正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Thumb_DragDelta3(object sender, DragDeltaEventArgs e)
        {
            if (sender is AnchorThumb anchorT)
            {
                //該当のアンカーThumbの座標修正
                anchorT.X += e.HorizontalChange;
                anchorT.Y += e.VerticalChange;

                //全体のアンカー点から左上座標取得
                double minX = anchorT.X;
                double minY = anchorT.Y;
                foreach (var item in MyAnchorThumbs)
                {
                    if (minX > item.X) { minX = item.X; }
                    if (minY > item.Y) { minY = item.Y; }
                }

                //左上座標が0なら該当Pointだけ変更、
                //違う場合は本体と全アンカー点を修正
                if (minX == 0 && minY == 0)
                {
                    MyPoints[DragPointIndex] = new Point(anchorT.X, anchorT.Y);
                }
                else
                {
                    SetLeft(this, GetLeft(this) + minX);
                    SetTop(this, GetTop(this) + minY);

                    for (int i = 0; i < MyPoints.Count; i++)
                    {
                        MyAnchorThumbs[i].X -= minX;
                        MyAnchorThumbs[i].Y -= minY;
                        MyPoints[i] = new Point(MyAnchorThumbs[i].X, MyAnchorThumbs[i].Y);
                    }
                }
            }
        }
        private void Offset(double xOffset, double yOffset)
        {
            
        }
        //private void Thumb_DragDelta2(object sender, DragDeltaEventArgs e)
        //{
        //    if (sender is AnchorThumb anchorT)
        //    {
        //        double x = anchorT.X + e.HorizontalChange;
        //        double y = anchorT.Y + e.VerticalChange;
        //        if (x < 0 || y < 0)
        //        {
        //            if (x < 0)
        //            {
        //                for (int i = 0; i < MyPoints.Count; i++)
        //                {
        //                    //MyPoints[i].Offset(x, 0);
        //                    MyPoints[i] = new Point(MyPoints[i].X - x, MyPoints[i].Y);
        //                    MyAnchorThumbs[i].X -= x;
        //                }
        //                anchorT.X = 0;
        //                MyPoints[DragPointIndex] = new Point(0, MyPoints[DragPointIndex].Y);
        //                SetLeft(this, GetLeft(this) + x);
        //            }
        //            if (y < 0)
        //            {
        //                for (int i = 0; i < MyPoints.Count; i++)
        //                {
        //                    //MyPoints[i].Offset(0, y);
        //                    MyPoints[i] = new Point(MyPoints[i].X, MyPoints[i].Y - y);
        //                    MyAnchorThumbs[i].Y -= y;
        //                }
        //                anchorT.Y = 0;
        //                MyPoints[DragPointIndex] = new Point(MyPoints[DragPointIndex].X, 0);
        //                SetTop(this, GetTop(this) + y);
        //            }
        //        }
        //        else
        //        {

        //            anchorT.X = x;
        //            anchorT.Y = y;
        //            MyPoints[DragPointIndex] = new Point(x, y);
        //        }
        //    }
        //}


        //private void Thumb_DragDelta2(object sender, DragDeltaEventArgs e)
        //{
        //    if (sender is AnchorThumb anchorT)
        //    {
        //        anchorT.X += e.HorizontalChange;
        //        anchorT.Y += e.VerticalChange;
        //        MyPoints[DragPointIndex] = new Point(anchorT.X, anchorT.Y);
        //    }
        //}

    }
}
