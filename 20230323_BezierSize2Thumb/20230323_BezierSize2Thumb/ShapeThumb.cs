using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media.Effects;

namespace _20230323_BezierSize2Thumb
{
    class ShapeThumb : Thumb
    {
        #region 依存関係プロパティと通知プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(ShapeThumb),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 頂点座標のThumbsの表示設定
        /// </summary>
        public Visibility MyAnchorVisible
        {
            get { return (Visibility)GetValue(MyAnchorVisibleProperty); }
            set { SetValue(MyAnchorVisibleProperty, value); }
        }
        public static readonly DependencyProperty MyAnchorVisibleProperty =
            DependencyProperty.Register(nameof(MyAnchorVisible), typeof(Visibility), typeof(ShapeThumb),
                new FrameworkPropertyMetadata(Visibility.Visible,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// ラインのつなぎ目をtrueで丸める、falseで鋭角にする
        /// </summary>
        public bool MyLineSmoothJoin
        {
            get { return (bool)GetValue(MyLineSmoothJoinProperty); }
            set { SetValue(MyLineSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyLineSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyLineSmoothJoin), typeof(bool), typeof(ShapeThumb),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        /// <summary>
        /// ラインの始点と終点を繋ぐかどうか
        /// </summary>
        public bool MyLineClose
        {
            get { return (bool)GetValue(MyLineCloseProperty); }
            set { SetValue(MyLineCloseProperty, value); }
        }
        public static readonly DependencyProperty MyLineCloseProperty =
            DependencyProperty.Register(nameof(MyLineClose), typeof(bool), typeof(ShapeThumb),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(ShapeThumb),
                new FrameworkPropertyMetadata(Brushes.Crimson,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(ShapeThumb),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public bool MyIsEditing
        {
            get { return (bool)GetValue(MyIsEditingProperty); }
            set { SetValue(MyIsEditingProperty, value); }
        }
        public static readonly DependencyProperty MyIsEditingProperty =
            DependencyProperty.Register(nameof(MyIsEditing), typeof(bool), typeof(ShapeThumb),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));







        #endregion 依存関係プロパティと通知プロパティ

        public Canvas MyCanvas { get; private set; }
        public Bezier MyBezier { get; private set; }

        public ShapeThumb()
        {
            (MyCanvas, MyBezier) = SetTemplate();
            Loaded += ShapeThumb_Loaded;
            this.Background = Brushes.Red;
            Background = Brushes.Green;
        }

        private void ShapeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            MySetBindings();
            UpdateLayout();//必須、これがないとGetDescendantBoundsがEmpty
            //Fix0Point();
            
            if (MyBezier.MyExternalBounds.IsEmpty == false)
            {                
                FixBezierLocate();
                //FixCanvasLocate00();
            }
        }
        public void FixBezierLocate()
        {
            if (MyBezier.MyExternalBounds.IsEmpty) return;
            var bounds = MyBezier.MyExternalBounds;
            Canvas.SetLeft(MyBezier, -bounds.Left);
            Canvas.SetTop(MyBezier, -bounds.Top);
        }

        /// <summary>
        /// PointCollectionのRectを返す
        /// </summary>
        /// <param name="pt">PointCollection</param>
        /// <returns></returns>
        public static Rect GetPointsRect(PointCollection pt)
        {
            if (pt.Count == 0) return new Rect();
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            foreach (var item in pt)
            {
                if (minX > item.X) minX = item.X;
                if (minY > item.Y) minY = item.Y;
                if (maxX < item.X) maxX = item.X;
                if (maxY < item.Y) maxY = item.Y;
            }
            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }


        //Pointsの左上を0,0にするだけ
        public void Fix0Point()
        {
            Rect r = MyAdorner.GetPointsRect(MyPoints);
            for (int i = 0; i < MyPoints.Count; i++)
            {
                Point pp = MyPoints[i];
                MyPoints[i] = new Point(pp.X - r.Left, pp.Y - r.Top);
            }
        }

        //使う場面は
        //頂点ThumbのDragDeltaイベントなどで図形が変化しているとき
        public void FixCanvasLocate00()
        {
            var bezExRect = MyBezier.MyExternalBounds;
            if (bezExRect.IsEmpty) { return; }
            //var canRect = VisualTreeHelper.GetDescendantBounds(this);

            //自身Canvasの座標修正、
            //新しい座標 = 自身の座標＋(図形の座標＋Rect座標)
            var bezLocate = VisualTreeHelper.GetOffset(MyBezier);
            var xDiff = bezLocate.X + bezExRect.Left;
            var yDiff = bezLocate.Y + bezExRect.Top;
            var myLocate = VisualTreeHelper.GetOffset(this);
            Canvas.SetLeft(this, myLocate.X + xDiff);
            Canvas.SetTop(this, myLocate.Y + yDiff);

            //図形の座標修正、
            //新しい座標 = 図形の座標 - (図形の座標＋Rect座標) + PointsのRect座標
            var ptsRect = GetPointsRect(MyPoints);
            Canvas.SetLeft(MyBezier, bezLocate.X - xDiff + ptsRect.X);
            Canvas.SetTop(MyBezier, bezLocate.Y - yDiff + ptsRect.Y);

            //PointsのRect座標を0,0に修正
            Fix0Point();
            //頂点Thumbの座標修正、Pointsに合わせる
            MyBezier.MyAdorner?.FixThumbsLocate();
        }
        protected override Size MeasureOverride(Size constraint)
        {
            if (MyBezier.MyIsEditing)
            {
                FixCanvasLocate00();
            }
            return base.MeasureOverride(constraint);
        }
        private void MySetBindings()
        {
            MyBezier.SetBinding(Bezier.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });
            MyBezier.SetBinding(Bezier.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeProperty) });
            MyBezier.SetBinding(Bezier.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty) });
            MyBezier.SetBinding(Bezier.MyIsEditingProperty, new Binding() { Source = this, Path = new PropertyPath(MyIsEditingProperty) });


            SetBinding(WidthProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectWidth() });
            SetBinding(HeightProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectHeight() });

            MyCanvas.SetBinding(BackgroundProperty, new Binding() { Source = this, Path = new PropertyPath(BackgroundProperty) });
        }

        private (Canvas, Bezier) SetTemplate()
        {
            FrameworkElementFactory fPanel = new(typeof(Canvas), "nemo");
            FrameworkElementFactory fShape = new(typeof(Bezier), "shape");
            fPanel.AppendChild(fShape);
            this.Template = new() { VisualTree = fPanel };
            this.ApplyTemplate();

            if (Template.FindName("shape", this) is Bezier shape) { }
            else { throw new ArgumentNullException(nameof(this.Template)); }

            if (this.Template.FindName("nemo", this) is Canvas panel) { }
            else { throw new ArgumentNullException(nameof(this.Template)); }
            return (panel, shape);
        }
    }
}
