using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace _20230510
{
    public class ResizeAdorner : Adorner
    {
        #region 必要

        readonly VisualCollection MyVisuals;
        protected override int VisualChildrenCount => MyVisuals.Count;
        protected override Visual GetVisualChild(int index) => MyVisuals[index];
        #endregion 必要

        #region 依存関係プロパティ


        public double ThumbSize
        {
            get { return (double)GetValue(ThumbSizeProperty); }
            set { SetValue(ThumbSizeProperty, value); }
        }
        public static readonly DependencyProperty ThumbSizeProperty =
            DependencyProperty.Register(nameof(ThumbSize), typeof(double), typeof(ResizeAdorner),
                new FrameworkPropertyMetadata(40.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存関係プロパティ

        private readonly TwoColorWakuEllipseThumb thumbTopL, thumbBotR, thumbTopR, thumbBotL, thumbTop, thumbBot, thumbL, thumbR;
        private readonly FrameworkElement MyTarget;


        public ResizeAdorner(UIElement adornedElement) : base(adornedElement)
        {
            MyTarget = (FrameworkElement)adornedElement;
            MyVisuals = new VisualCollection(this);
            thumbTop = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
            thumbTopL = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
            thumbTopR = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
            thumbBot = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
            thumbBotL = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
            thumbBotR = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
            thumbL = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };
            thumbR = new TwoColorWakuEllipseThumb() { Width = ThumbSize, Height = ThumbSize };

            thumbTopL.DragDelta += ThumbTopL_DragDelta;
            thumbBotR.DragDelta += ThumbBotR_DragDelta;
            thumbTop.DragDelta += ThumbTop_DragDelta;
            thumbTopR.DragDelta += ThumbTopR_DragDelta;
            thumbBot.DragDelta += ThumbBot_DragDelta;
            thumbBotL.DragDelta += ThumbBotL_DragDelta;
            thumbL.DragDelta += ThumbL_DragDelta;
            thumbR.DragDelta += ThumbR_DragDelta;

            MyVisuals.Add(thumbTop);
            MyVisuals.Add(thumbTopL);
            MyVisuals.Add(thumbTopR);
            MyVisuals.Add(thumbBot);
            MyVisuals.Add(thumbBotL);
            MyVisuals.Add(thumbL);
            MyVisuals.Add(thumbR);
            //右下を最後に追加
            MyVisuals.Add(thumbBotR);

            thumbTop.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbTop.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbTopL.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbTopL.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbTopR.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbTopR.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbBot.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbBot.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbBotL.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbBotL.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbBotR.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbBotR.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbR.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbR.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbL.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });
            thumbL.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbSizeProperty) });

            ContextMenu context = new();
            this.ContextMenu = context;
            context.Items.Add(new MenuItem() { Header = "test" });
        }

        #region ドラッグ移動時の動作

        private void MySetLeft(double horizontalChange)
        {
            Canvas.SetLeft(MyTarget, Canvas.GetLeft(MyTarget) + horizontalChange);
        }

        private void MySetTop(double verticalChange)
        {
            Canvas.SetTop(MyTarget, Canvas.GetTop(MyTarget) + verticalChange);
        }

        //移動中はマイナス座標でも修正しない
        /// <summary>
        /// ハンドルの横移動時に使う
        /// </summary>
        /// <param name="horizontalChange"></param>
        /// <param name="isLeft">左要素のハンドルならtrue指定、右ならfalse</param>
        private void DeltaHorizontal(double horizontalChange, bool isLeft)
        {
            double w;
            if (isLeft)
            {
                w = MyTarget.Width - horizontalChange;
                MySetLeft(horizontalChange);
            }
            else
            {
                w = MyTarget.Width + horizontalChange;
                if (w < 1)
                {
                    MySetLeft(horizontalChange);
                }
            }
            if (w < 1) w = 1;
            MyTarget.Width = w;
        }

        private void DeltaVertical(double verticalChange, bool toTop)
        {
            double h;
            if (toTop)
            {
                h = MyTarget.Height - verticalChange;
                MySetTop(verticalChange);
            }
            else
            {
                h = MyTarget.Height + verticalChange;
                if (h < 1)
                {
                    MySetTop(verticalChange);
                }
            }
            if (h < 1) h = 1;
            MyTarget.Height = h;
        }
        //右
        private void ThumbR_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DeltaHorizontal(e.HorizontalChange, false);
        }

        //左
        private void ThumbL_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DeltaHorizontal(e.HorizontalChange, true);
        }

        //左下
        private void ThumbBotL_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DeltaHorizontal(e.HorizontalChange, true);
            DeltaVertical(e.VerticalChange, false);
        }

        //下
        private void ThumbBot_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DeltaVertical(e.VerticalChange, false);
        }

        //右上
        private void ThumbTopR_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DeltaVertical(e.VerticalChange, true);
            DeltaHorizontal(e.HorizontalChange, false);
        }

        //上
        private void ThumbTop_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DeltaVertical(e.VerticalChange, true);
        }

        //右下
        private void ThumbBotR_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DeltaHorizontal(e.HorizontalChange, false);
            DeltaVertical(e.VerticalChange, false);
        }

        //左上
        private void ThumbTopL_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DeltaHorizontal(e.HorizontalChange, true);
            DeltaVertical(e.VerticalChange, true);
        }
        #endregion ドラッグ移動時の動作


        ////ドラッグ移動終了後にLayoutUpdateを実行
        //private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        //{
        //    MyTarget.TTParent?.TTGroupUpdateLayout();
        //}

        protected override Size ArrangeOverride(Size finalSize)
        {
            double tsHalf = ThumbSize / 2.0;
            double w = AdornedElement.DesiredSize.Width;
            double h = AdornedElement.DesiredSize.Height;

            double horizontalCenter = (w / 2.0) - tsHalf;
            double verticalCenter = (h / 2.0) - tsHalf;
            double horizontalRight = w - tsHalf;
            double verticalBottom = h - tsHalf;
            thumbTop.Arrange(new Rect(horizontalCenter, -tsHalf, ThumbSize, ThumbSize));
            thumbTopL.Arrange(new Rect(-tsHalf, -tsHalf, ThumbSize, ThumbSize));
            thumbTopR.Arrange(new Rect(horizontalRight, -tsHalf, ThumbSize, ThumbSize));
            thumbBot.Arrange(new Rect(horizontalCenter, verticalBottom, ThumbSize, ThumbSize));
            thumbBotL.Arrange(new Rect(-tsHalf, verticalBottom, ThumbSize, ThumbSize));
            thumbBotR.Arrange(new Rect(horizontalRight, verticalBottom, ThumbSize, ThumbSize));
            thumbL.Arrange(new Rect(-tsHalf, verticalCenter, ThumbSize, ThumbSize));
            thumbR.Arrange(new Rect(horizontalRight, verticalCenter, ThumbSize, ThumbSize));


            return base.ArrangeOverride(finalSize);
        }

    }
    //TemplateをCanvasにして、そこにサイズ違いのEllipseを2つ乗せる
    public class TwoColorWakuEllipseThumb : Thumb
    {
        #region 依存関係プロパティ

        public Brush MyWakuSotoColor
        {
            get { return (Brush)GetValue(MyWakuSotoColorProperty); }
            set { SetValue(MyWakuSotoColorProperty, value); }
        }
        public static readonly DependencyProperty MyWakuSotoColorProperty =
            DependencyProperty.Register(nameof(MyWakuSotoColor), typeof(Brush), typeof(TwoColorWakuEllipseThumb),
                new FrameworkPropertyMetadata(Brushes.White,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush MyWakuNakaColor
        {
            get { return (Brush)GetValue(MyWakuNakaColorProperty); }
            set { SetValue(MyWakuNakaColorProperty, value); }
        }
        public static readonly DependencyProperty MyWakuNakaColorProperty =
            DependencyProperty.Register(nameof(MyWakuNakaColor), typeof(Brush), typeof(TwoColorWakuEllipseThumb),
                new FrameworkPropertyMetadata(Brushes.Red,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        #endregion 依存関係プロパティ


        public Ellipse MyMarker1 { get; private set; } = new();
        public Ellipse MyMarker2 { get; private set; } = new();
        public Canvas MyCanvas { get; private set; }
        public TwoColorWakuEllipseThumb()
        {
            MyCanvas = SetTemplate();
            MyCanvas.Children.Add(MyMarker1);
            MyCanvas.Children.Add(MyMarker2);
            MyMarker1.StrokeThickness = 2;

            MyCanvas.Background = Brushes.Transparent;
            SetMyBindings();
            Loaded += TwoColorWakuThumb_Loaded;
        }

        private void TwoColorWakuThumb_Loaded(object sender, RoutedEventArgs e)
        {
            //MyMarker2.Height = MyMarker1.Height - 2;
            //MyMarker2.Width = MyMarker1.Width - 2;
            //Canvas.SetLeft(MyMarker2, 1);
            //Canvas.SetTop(MyMarker2, 1);
        }

        private Canvas SetTemplate()
        {
            FrameworkElementFactory factory = new(typeof(Canvas), "nemo");
            this.Template = new() { VisualTree = factory };
            this.ApplyTemplate();
            if (Template.FindName("nemo", this) is Canvas panel)
            {
                return panel;
            }
            else throw new ArgumentException();
        }
        private void SetMyBindings()
        {
            //MyCanvas.SetBinding(Canvas.BackgroundProperty, new Binding() { Source = this, Path = new PropertyPath(BackgroundProperty) ,Mode= BindingMode.TwoWay});

            MyMarker1.SetBinding(Rectangle.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyWakuSotoColorProperty) });
            MyMarker1.SetBinding(Rectangle.WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ActualWidthProperty) });
            MyMarker1.SetBinding(Rectangle.HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ActualHeightProperty) });

            MyMarker2.SetBinding(Rectangle.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyWakuNakaColorProperty) });
            MyMarker2.SetBinding(Rectangle.WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ActualWidthProperty) });
            MyMarker2.SetBinding(Rectangle.HeightProperty, new Binding() { Source = this, Path = new PropertyPath(ActualHeightProperty) });

        }
    }

}
