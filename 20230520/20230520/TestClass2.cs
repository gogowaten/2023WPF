using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace _20230520
{
    public class CanvasThumb5 : Thumb
    {
        #region 依存関係プロパティ

        public double MyHandlThumbSize
        {
            get { return (double)GetValue(MyHandlThumbSizeProperty); }
            set { SetValue(MyHandlThumbSizeProperty, value); }
        }
        public static readonly DependencyProperty MyHandlThumbSizeProperty =
            DependencyProperty.Register(nameof(MyHandlThumbSize), typeof(double), typeof(CanvasThumb5),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ

        #region SizeHandlThumbs

        //R:Right, L:Left, T:Top, B:Bottom
        public Thumb TTR { get; set; } = new();
        public Thumb TTB { get; set; } = new();
        public Thumb TTL { get; set; } = new();
        public Thumb TTT { get; set; } = new();
        public Thumb TTTR { get; set; } = new();
        public Thumb TTBL { get; set; } = new();
        public Thumb TTBR { get; set; } = new();
        public Thumb TTTL { get; set; } = new();
        #endregion SizeHandlThumbs
        //要素を表示するため、TemplateをCanvas
        public Canvas MyTemplateCanvas { get; private set; }
        //中の要素
        public Polyline MyContent;

        public CanvasThumb5()
        {
            MyTemplateCanvas = SetMyTemplate<Canvas>();
            MyContent = new()
            {
                Points = new PointCollection() { new Point(30,20), new Point(150, 100) },
                StrokeThickness = 20,
                Stroke = Brushes.Gold,
            };
            Canvas.SetLeft(MyContent, 0);
            Canvas.SetTop(MyContent, 0);
            MyTemplateCanvas.Children.Add(MyContent);

            DragDelta += CanvasThumb2_DragDelta;
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
            SetMyBindings();

            MyTemplateCanvas.Children.Add(TTTL); TTTL.DragDelta += (o, e) => { TTWidthAndX(e); TTHeightAndY(e); };
            MyTemplateCanvas.Children.Add(TTT); TTT.DragDelta += (o, e) => { TTHeightAndY(e); };
            MyTemplateCanvas.Children.Add(TTTR); TTTR.DragDelta += (o, e) => { TTHeightAndY(e); TTWidth(e); };
            MyTemplateCanvas.Children.Add(TTL); TTL.DragDelta += (o, e) => { TTWidthAndX(e); };
            MyTemplateCanvas.Children.Add(TTR); TTR.DragDelta += (o, e) => { TTWidth(e); };
            MyTemplateCanvas.Children.Add(TTBL); TTBL.DragDelta += (o, e) => { TTWidthAndX(e); TTHeight(e); };
            MyTemplateCanvas.Children.Add(TTB); TTB.DragDelta += (o, e) => { TTHeight(e); };
            MyTemplateCanvas.Children.Add(TTBR); TTBR.DragDelta += (o, e) => { TTWidth(e); TTHeight(e); };

        }

        private void CanvasThumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is CanvasThumb5 canvas && canvas == e.OriginalSource)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            }
        }

        private T SetMyTemplate<T>()
        {
            FrameworkElementFactory factory = new(typeof(T), "nemo");
            Template = new ControlTemplate() { VisualTree = factory };
            ApplyTemplate();
            return (T)Template.FindName("nemo", this);
        }

        private void SetMyBindings()
        {
            //サイズをTemplateCanvasのサイズに合わせる
            SetBinding(WidthProperty, new Binding() { Source = MyTemplateCanvas, Path = new PropertyPath(WidthProperty), Mode = BindingMode.TwoWay });
            SetBinding(HeightProperty, new Binding() { Source = MyTemplateCanvas, Path = new PropertyPath(HeightProperty), Mode = BindingMode.TwoWay });
            SetBinding(BackgroundProperty, new Binding() { Source = MyTemplateCanvas, Path = new PropertyPath(BackgroundProperty), Mode = BindingMode.TwoWay });

            //ハンドルサイズ
            Binding handlSize = new Binding() { Source = this, Path = new PropertyPath(MyHandlThumbSizeProperty) };
            TTB.SetBinding(WidthProperty, handlSize);
            TTB.SetBinding(HeightProperty, handlSize);
            TTBL.SetBinding(WidthProperty, handlSize);
            TTBL.SetBinding(HeightProperty, handlSize);
            TTBR.SetBinding(WidthProperty, handlSize);
            TTBR.SetBinding(HeightProperty, handlSize);
            TTL.SetBinding(WidthProperty, handlSize);
            TTL.SetBinding(HeightProperty, handlSize);
            TTR.SetBinding(WidthProperty, handlSize);
            TTR.SetBinding(HeightProperty, handlSize);
            TTTL.SetBinding(WidthProperty, handlSize);
            TTTL.SetBinding(HeightProperty, handlSize);
            TTTR.SetBinding(WidthProperty, handlSize);
            TTTR.SetBinding(HeightProperty, handlSize);
            TTT.SetBinding(WidthProperty, handlSize);
            TTT.SetBinding(HeightProperty, handlSize);

            //ハンドルの座標、ハンドルのサイズを考慮してサイズに合わせる
            Binding bWidth = new() { Source = this, Path = new PropertyPath(WidthProperty) };
            Binding bHeight = new() { Source = this, Path = new PropertyPath(HeightProperty) };
            MultiBinding mbw = new() { Converter = new MyConverterHandlThumb() };
            mbw.Bindings.Add(handlSize);
            mbw.Bindings.Add(bWidth);
            MultiBinding mbh = new() { Converter = new MyConverterHandlThumb() };
            mbh.Bindings.Add(handlSize);
            mbh.Bindings.Add(bHeight);
            MultiBinding mbwh = new() { Converter = new MyConverterHandlThumbHalf() };
            mbwh.Bindings.Add(handlSize);
            mbwh.Bindings.Add(bWidth);
            MultiBinding mbhh = new() { Converter = new MyConverterHandlThumbHalf() };
            mbhh.Bindings.Add(handlSize);
            mbhh.Bindings.Add(bHeight);

            TTR.SetBinding(Canvas.LeftProperty, mbw);
            TTR.SetBinding(Canvas.TopProperty, mbhh);
            TTB.SetBinding(Canvas.LeftProperty, mbwh);
            TTB.SetBinding(Canvas.TopProperty, mbh);
            TTT.SetBinding(Canvas.LeftProperty, mbwh);
            TTL.SetBinding(Canvas.TopProperty, mbhh);
            TTBR.SetBinding(Canvas.LeftProperty, mbw);
            TTBR.SetBinding(Canvas.TopProperty, mbh);
            TTTR.SetBinding(Canvas.LeftProperty, mbw);
            TTBL.SetBinding(Canvas.TopProperty, mbh);

        }

        #region ハンドル移動

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

        private void TTWidthAndX(DragDeltaEventArgs e)
        {
            double value = Width - e.HorizontalChange;
            if (value >= 0)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                Width = value;
                Canvas.SetLeft(MyContent, Canvas.GetLeft(MyContent) - e.HorizontalChange);
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
                Canvas.SetTop(MyContent, Canvas.GetTop(MyContent) - e.VerticalChange);
            }
            else
            {
                Canvas.SetTop(this, Canvas.GetTop(this) + Height);
                Height = 0;
            }
        }
        #endregion ハンドル移動

        //サイズを中の要素に合わせる＋座標も変更
        public void FitSizeContent()
        {
            Width = MyContent.ActualWidth;
            Height = MyContent.ActualHeight;
            Canvas.SetLeft(this, Canvas.GetLeft(this) + Canvas.GetLeft(MyContent));
            Canvas.SetTop(this, Canvas.GetTop(this) + Canvas.GetTop(MyContent));
            Canvas.SetLeft(MyContent, 0);
            Canvas.SetTop(MyContent, 0);
        }

    }


    /// <summary>
    /// CanvasThumb3の改変
    /// サイズと座標を中の要素に合わせるメソッド追加しただけ
    /// </summary>
    public class CanvasThumb4 : Thumb
    {
        #region 依存関係プロパティ

        public double MyHandlThumbSize
        {
            get { return (double)GetValue(MyHandlThumbSizeProperty); }
            set { SetValue(MyHandlThumbSizeProperty, value); }
        }
        public static readonly DependencyProperty MyHandlThumbSizeProperty =
            DependencyProperty.Register(nameof(MyHandlThumbSize), typeof(double), typeof(CanvasThumb4),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ

        #region SizeHandlThumbs

        //R:Right, L:Left, T:Top, B:Bottom
        public Thumb TTR { get; set; } = new();
        public Thumb TTB { get; set; } = new();
        public Thumb TTL { get; set; } = new();
        public Thumb TTT { get; set; } = new();
        public Thumb TTTR { get; set; } = new();
        public Thumb TTBL { get; set; } = new();
        public Thumb TTBR { get; set; } = new();
        public Thumb TTTL { get; set; } = new();
        #endregion SizeHandlThumbs
        //要素を表示するため、TemplateをCanvas
        public Canvas MyTemplateCanvas { get; private set; }
        //中の要素
        public TextBlock MyContent;

        public CanvasThumb4()
        {
            MyTemplateCanvas = SetMyTemplate<Canvas>();
            MyContent = new() { Text = "aaaaaaaaaaaaaaaaaaaaaaaaa" };
            Canvas.SetLeft(MyContent, 0);
            Canvas.SetTop(MyContent, 0);
            MyTemplateCanvas.Children.Add(MyContent);

            DragDelta += CanvasThumb2_DragDelta;
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
            SetMyBindings();

            MyTemplateCanvas.Children.Add(TTTL); TTTL.DragDelta += (o, e) => { TTWidthAndX(e); TTHeightAndY(e); };
            MyTemplateCanvas.Children.Add(TTT); TTT.DragDelta += (o, e) => { TTHeightAndY(e); };
            MyTemplateCanvas.Children.Add(TTTR); TTTR.DragDelta += (o, e) => { TTHeightAndY(e); TTWidth(e); };
            MyTemplateCanvas.Children.Add(TTL); TTL.DragDelta += (o, e) => { TTWidthAndX(e); };
            MyTemplateCanvas.Children.Add(TTR); TTR.DragDelta += (o, e) => { TTWidth(e); };
            MyTemplateCanvas.Children.Add(TTBL); TTBL.DragDelta += (o, e) => { TTWidthAndX(e); TTHeight(e); };
            MyTemplateCanvas.Children.Add(TTB); TTB.DragDelta += (o, e) => { TTHeight(e); };
            MyTemplateCanvas.Children.Add(TTBR); TTBR.DragDelta += (o, e) => { TTWidth(e); TTHeight(e); };

        }

        private void CanvasThumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is CanvasThumb4 canvas && canvas == e.OriginalSource)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            }
        }

        private T SetMyTemplate<T>()
        {
            FrameworkElementFactory factory = new(typeof(T), "nemo");
            Template = new ControlTemplate() { VisualTree = factory };
            ApplyTemplate();
            return (T)Template.FindName("nemo", this);
        }

        private void SetMyBindings()
        {
            //サイズをTemplateCanvasのサイズに合わせる
            SetBinding(WidthProperty, new Binding() { Source = MyTemplateCanvas, Path = new PropertyPath(WidthProperty), Mode = BindingMode.TwoWay });
            SetBinding(HeightProperty, new Binding() { Source = MyTemplateCanvas, Path = new PropertyPath(HeightProperty), Mode = BindingMode.TwoWay });
            SetBinding(BackgroundProperty, new Binding() { Source = MyTemplateCanvas, Path = new PropertyPath(BackgroundProperty), Mode = BindingMode.TwoWay });

            //ハンドルサイズ
            Binding handlSize = new Binding() { Source = this, Path = new PropertyPath(MyHandlThumbSizeProperty) };
            TTB.SetBinding(WidthProperty, handlSize);
            TTB.SetBinding(HeightProperty, handlSize);
            TTBL.SetBinding(WidthProperty, handlSize);
            TTBL.SetBinding(HeightProperty, handlSize);
            TTBR.SetBinding(WidthProperty, handlSize);
            TTBR.SetBinding(HeightProperty, handlSize);
            TTL.SetBinding(WidthProperty, handlSize);
            TTL.SetBinding(HeightProperty, handlSize);
            TTR.SetBinding(WidthProperty, handlSize);
            TTR.SetBinding(HeightProperty, handlSize);
            TTTL.SetBinding(WidthProperty, handlSize);
            TTTL.SetBinding(HeightProperty, handlSize);
            TTTR.SetBinding(WidthProperty, handlSize);
            TTTR.SetBinding(HeightProperty, handlSize);
            TTT.SetBinding(WidthProperty, handlSize);
            TTT.SetBinding(HeightProperty, handlSize);

            //ハンドルの座標、ハンドルのサイズを考慮してサイズに合わせる
            Binding bWidth = new() { Source = this, Path = new PropertyPath(WidthProperty) };
            Binding bHeight = new() { Source = this, Path = new PropertyPath(HeightProperty) };
            MultiBinding mbw = new() { Converter = new MyConverterHandlThumb() };
            mbw.Bindings.Add(handlSize);
            mbw.Bindings.Add(bWidth);
            MultiBinding mbh = new() { Converter = new MyConverterHandlThumb() };
            mbh.Bindings.Add(handlSize);
            mbh.Bindings.Add(bHeight);
            MultiBinding mbwh = new() { Converter = new MyConverterHandlThumbHalf() };
            mbwh.Bindings.Add(handlSize);
            mbwh.Bindings.Add(bWidth);
            MultiBinding mbhh = new() { Converter = new MyConverterHandlThumbHalf() };
            mbhh.Bindings.Add(handlSize);
            mbhh.Bindings.Add(bHeight);

            TTR.SetBinding(Canvas.LeftProperty, mbw);
            TTR.SetBinding(Canvas.TopProperty, mbhh);
            TTB.SetBinding(Canvas.LeftProperty, mbwh);
            TTB.SetBinding(Canvas.TopProperty, mbh);
            TTT.SetBinding(Canvas.LeftProperty, mbwh);
            TTL.SetBinding(Canvas.TopProperty, mbhh);
            TTBR.SetBinding(Canvas.LeftProperty, mbw);
            TTBR.SetBinding(Canvas.TopProperty, mbh);
            TTTR.SetBinding(Canvas.LeftProperty, mbw);
            TTBL.SetBinding(Canvas.TopProperty, mbh);

        }

        #region ハンドル移動

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

        private void TTWidthAndX(DragDeltaEventArgs e)
        {
            double value = Width - e.HorizontalChange;
            if (value >= 0)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                Width = value;
                Canvas.SetLeft(MyContent, Canvas.GetLeft(MyContent) - e.HorizontalChange);
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
                Canvas.SetTop(MyContent, Canvas.GetTop(MyContent) - e.VerticalChange);
            }
            else
            {
                Canvas.SetTop(this, Canvas.GetTop(this) + Height);
                Height = 0;
            }
        }
        #endregion ハンドル移動

        //サイズを中の要素に合わせる＋座標も変更
        public void FitSizeContent()
        {
            Width = MyContent.ActualWidth;
            Height = MyContent.ActualHeight;
            Canvas.SetLeft(this, Canvas.GetLeft(this) + Canvas.GetLeft(MyContent));
            Canvas.SetTop(this, Canvas.GetTop(this) + Canvas.GetTop(MyContent));
            Canvas.SetLeft(MyContent, 0);
            Canvas.SetTop(MyContent, 0);
        }

    }



    /// <summary>
    /// CanvasThumb2の改変
    /// ハンドルの表示位置のテスト
    /// すべてを自身の境界線の内側に表示
    /// </summary>
    public class CanvasThumb3 : Thumb
    {
        #region 依存関係プロパティ

        public double MyHandlThumbSize
        {
            get { return (double)GetValue(MyHandlThumbSizeProperty); }
            set { SetValue(MyHandlThumbSizeProperty, value); }
        }
        public static readonly DependencyProperty MyHandlThumbSizeProperty =
            DependencyProperty.Register(nameof(MyHandlThumbSize), typeof(double), typeof(CanvasThumb3),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ

        #region SizeHandlThumbs

        //R:Right, L:Left, T:Top, B:Bottom
        public Thumb TTR { get; set; } = new();
        public Thumb TTB { get; set; } = new();
        public Thumb TTL { get; set; } = new();
        public Thumb TTT { get; set; } = new();
        public Thumb TTTR { get; set; } = new();
        public Thumb TTBL { get; set; } = new();
        public Thumb TTBR { get; set; } = new();
        public Thumb TTTL { get; set; } = new();
        #endregion SizeHandlThumbs
        //要素を表示するため、TemplateをCanvas
        public Canvas MyTemplateCanvas { get; private set; }
        //中の要素
        public TextBlock MyTextBlock;

        public CanvasThumb3()
        {
            MyTemplateCanvas = SetMyTemplate<Canvas>();
            MyTextBlock = new() { Text = "aaaaaaaaaaaaaaaaaaaaaaaaa" };
            Canvas.SetLeft(MyTextBlock, 0);
            Canvas.SetTop(MyTextBlock, 0);
            MyTemplateCanvas.Children.Add(MyTextBlock);

            DragDelta += CanvasThumb2_DragDelta;
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
            SetMyBindings();

            MyTemplateCanvas.Children.Add(TTR); TTR.DragDelta += (o, e) => { TTWidth(e); };
            MyTemplateCanvas.Children.Add(TTB); TTB.DragDelta += (o, e) => { TTHeight(e); };
            MyTemplateCanvas.Children.Add(TTL); TTL.DragDelta += (o, e) => { TTWidthAndX(e); };
            MyTemplateCanvas.Children.Add(TTT); TTT.DragDelta += (o, e) => { TTHeightAndY(e); };
            MyTemplateCanvas.Children.Add(TTTR); TTTR.DragDelta += (o, e) => { TTHeightAndY(e); TTWidth(e); };
            MyTemplateCanvas.Children.Add(TTBL); TTBL.DragDelta += (o, e) => { TTWidthAndX(e); TTHeight(e); };
            MyTemplateCanvas.Children.Add(TTBR); TTBR.DragDelta += (o, e) => { TTWidth(e); TTHeight(e); };
            MyTemplateCanvas.Children.Add(TTTL); TTTL.DragDelta += (o, e) => { TTWidthAndX(e); TTHeightAndY(e); };

        }

        private void CanvasThumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is CanvasThumb3 canvas && canvas == e.OriginalSource)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            }
        }

        private T SetMyTemplate<T>()
        {
            FrameworkElementFactory factory = new(typeof(T), "nemo");
            Template = new ControlTemplate() { VisualTree = factory };
            ApplyTemplate();
            return (T)Template.FindName("nemo", this);
        }

        private void SetMyBindings()
        {
            //サイズをTemplateCanvasのサイズに合わせる
            SetBinding(WidthProperty, new Binding() { Source = MyTemplateCanvas, Path = new PropertyPath(WidthProperty), Mode = BindingMode.TwoWay });
            SetBinding(HeightProperty, new Binding() { Source = MyTemplateCanvas, Path = new PropertyPath(HeightProperty), Mode = BindingMode.TwoWay });
            SetBinding(BackgroundProperty, new Binding() { Source = MyTemplateCanvas, Path = new PropertyPath(BackgroundProperty), Mode = BindingMode.TwoWay });

            //ハンドルサイズ
            Binding handlSize = new Binding() { Source = this, Path = new PropertyPath(MyHandlThumbSizeProperty) };
            TTB.SetBinding(WidthProperty, handlSize);
            TTB.SetBinding(HeightProperty, handlSize);
            TTBL.SetBinding(WidthProperty, handlSize);
            TTBL.SetBinding(HeightProperty, handlSize);
            TTBR.SetBinding(WidthProperty, handlSize);
            TTBR.SetBinding(HeightProperty, handlSize);
            TTL.SetBinding(WidthProperty, handlSize);
            TTL.SetBinding(HeightProperty, handlSize);
            TTR.SetBinding(WidthProperty, handlSize);
            TTR.SetBinding(HeightProperty, handlSize);
            TTTL.SetBinding(WidthProperty, handlSize);
            TTTL.SetBinding(HeightProperty, handlSize);
            TTTR.SetBinding(WidthProperty, handlSize);
            TTTR.SetBinding(HeightProperty, handlSize);
            TTT.SetBinding(WidthProperty, handlSize);
            TTT.SetBinding(HeightProperty, handlSize);

            //ハンドルの座標、ハンドルのサイズを考慮してサイズに合わせる
            Binding bWidth = new() { Source = this, Path = new PropertyPath(WidthProperty) };
            Binding bHeight = new() { Source = this, Path = new PropertyPath(HeightProperty) };
            MultiBinding mbw = new() { Converter = new MyConverterHandlThumb() };
            mbw.Bindings.Add(handlSize);
            mbw.Bindings.Add(bWidth);
            MultiBinding mbh = new() { Converter = new MyConverterHandlThumb() };
            mbh.Bindings.Add(handlSize);
            mbh.Bindings.Add(bHeight);
            MultiBinding mbwh = new() { Converter = new MyConverterHandlThumbHalf() };
            mbwh.Bindings.Add(handlSize);
            mbwh.Bindings.Add(bWidth);
            MultiBinding mbhh = new() { Converter = new MyConverterHandlThumbHalf() };
            mbhh.Bindings.Add(handlSize);
            mbhh.Bindings.Add(bHeight);

            TTR.SetBinding(Canvas.LeftProperty, mbw);
            TTR.SetBinding(Canvas.TopProperty, mbhh);
            TTB.SetBinding(Canvas.LeftProperty, mbwh);
            TTB.SetBinding(Canvas.TopProperty, mbh);
            TTT.SetBinding(Canvas.LeftProperty, mbwh);
            TTL.SetBinding(Canvas.TopProperty, mbhh);
            TTBR.SetBinding(Canvas.LeftProperty, mbw);
            TTBR.SetBinding(Canvas.TopProperty, mbh);
            TTTR.SetBinding(Canvas.LeftProperty, mbw);
            TTBL.SetBinding(Canvas.TopProperty, mbh);

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

        private void TTWidthAndX(DragDeltaEventArgs e)
        {
            double value = Width - e.HorizontalChange;
            if (value >= 0)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                Width = value;
                Canvas.SetLeft(MyTextBlock, Canvas.GetLeft(MyTextBlock) - e.HorizontalChange);
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
                Canvas.SetTop(MyTextBlock, Canvas.GetTop(MyTextBlock) - e.VerticalChange);
            }
            else
            {
                Canvas.SetTop(this, Canvas.GetTop(this) + Height);
                Height = 0;
            }
        }


    }


    /// <summary>
    /// 要素を表示できるThumb
    /// 中の要素サイズに関係なくサイズ変更できるThumb
    /// 座標変更が伴うサイズ変更時でも、中の要素の見た目上の座標は変化させない
    /// 中の要素はTextBlock限定、これを汎用にしたいけど無理そう
    /// </summary>
    public class CanvasThumb2 : Thumb
    {
        #region SizeHandlThumbs

        //R:Right, L:Left, T:Top, B:Bottom
        public Thumb TTR { get; set; } = new() { Width = 20, Height = 20 };
        public Thumb TTB { get; set; } = new() { Width = 20, Height = 20 };
        public Thumb TTL { get; set; } = new() { Width = 20, Height = 20 };
        public Thumb TTT { get; set; } = new() { Width = 20, Height = 20 };
        public Thumb TTTR { get; set; } = new() { Width = 20, Height = 20 };
        public Thumb TTBL { get; set; } = new() { Width = 20, Height = 20 };
        public Thumb TTBR { get; set; } = new() { Width = 20, Height = 20 };
        public Thumb TTTL { get; set; } = new() { Width = 20, Height = 20 };
        #endregion SizeHandlThumbs
        //要素を表示するため、TemplateをCanvas
        public Canvas MyTemplateCanvas { get; private set; }
        //中の要素
        public TextBlock MyTextBlock;

        public CanvasThumb2()
        {
            MyTemplateCanvas = SetMyTemplate<Canvas>();
            MyTextBlock = new() { Text = "aaaaaaaaaaaaaaaaaaaaaaaaa" };
            Canvas.SetLeft(MyTextBlock, 0);
            Canvas.SetTop(MyTextBlock, 0);
            MyTemplateCanvas.Children.Add(MyTextBlock);

            DragDelta += CanvasThumb2_DragDelta;
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
            SetMyBindings();

            MyTemplateCanvas.Children.Add(TTR); TTR.DragDelta += (o, e) => { TTWidth(e); };
            MyTemplateCanvas.Children.Add(TTB); TTB.DragDelta += (o, e) => { TTHeight(e); };
            MyTemplateCanvas.Children.Add(TTL); TTL.DragDelta += (o, e) => { TTWidthAndX(e); };
            MyTemplateCanvas.Children.Add(TTT); TTT.DragDelta += (o, e) => { TTHeightAndY(e); };
            MyTemplateCanvas.Children.Add(TTTR); TTTR.DragDelta += (o, e) => { TTHeightAndY(e); TTWidth(e); };
            MyTemplateCanvas.Children.Add(TTBL); TTBL.DragDelta += (o, e) => { TTWidthAndX(e); TTHeight(e); };
            MyTemplateCanvas.Children.Add(TTBR); TTBR.DragDelta += (o, e) => { TTWidth(e); TTHeight(e); };
            MyTemplateCanvas.Children.Add(TTTL); TTTL.DragDelta += (o, e) => { TTWidthAndX(e); TTHeightAndY(e); };

        }

        private void CanvasThumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is CanvasThumb2 canvas && canvas == e.OriginalSource)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            }
        }

        private T SetMyTemplate<T>()
        {
            FrameworkElementFactory factory = new(typeof(T), "nemo");
            Template = new ControlTemplate() { VisualTree = factory };
            ApplyTemplate();
            return (T)Template.FindName("nemo", this);
        }

        private void SetMyBindings()
        {
            SetBinding(WidthProperty, new Binding() { Source = MyTemplateCanvas, Path = new PropertyPath(WidthProperty), Mode = BindingMode.TwoWay });
            SetBinding(HeightProperty, new Binding() { Source = MyTemplateCanvas, Path = new PropertyPath(HeightProperty), Mode = BindingMode.TwoWay });
            SetBinding(BackgroundProperty, new Binding() { Source = MyTemplateCanvas, Path = new PropertyPath(BackgroundProperty), Mode = BindingMode.TwoWay });

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

        private void TTWidthAndX(DragDeltaEventArgs e)
        {
            double value = Width - e.HorizontalChange;
            if (value >= 0)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                Width = value;
                Canvas.SetLeft(MyTextBlock, Canvas.GetLeft(MyTextBlock) - e.HorizontalChange);
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
                Canvas.SetTop(MyTextBlock, Canvas.GetTop(MyTextBlock) - e.VerticalChange);
            }
            else
            {
                Canvas.SetTop(this, Canvas.GetTop(this) + Height);
                Height = 0;
            }
        }


    }


    /// <summary>
    /// 失敗
    /// ResizeCanvas2をTemplateにしたThumb
    /// 上と左方向のサイズ変更で不具合、Canvas単体時はCanvasの座標の変更でいいけど
    /// ThumbのTemplateにした場合はThumbの座標を変更する必要がある、これができない
    /// </summary>
    public class CanvasThumb : Thumb
    {
        public ResizeCanvas2 MyTemplateCanvas { get; private set; }
        public CanvasThumb()
        {
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
            MyTemplateCanvas = SetMyTemplate<ResizeCanvas2>();
            DragDelta += CanvasThumb_DragDelta;
            SetMyBindings();
        }

        private void CanvasThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (e.Source == e.OriginalSource)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            }
        }

        private T SetMyTemplate<T>()
        {
            FrameworkElementFactory factory = new(typeof(T), "nemo");
            Template = new ControlTemplate() { VisualTree = factory };
            ApplyTemplate();
            return (T)Template.FindName("nemo", this);
        }
        private void SetMyBindings()
        {
            MyTemplateCanvas.SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(WidthProperty), Mode = BindingMode.TwoWay });
            MyTemplateCanvas.SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(HeightProperty), Mode = BindingMode.TwoWay });

        }
    }


    //ResizeCanvasのシンプル版
    //リサイズできるCanvas
    //ハンドルの座標はCanvasのサイズと座標にバインド
    //ハンドルのドラッグイベントでCanvasのサイズと座標を変更する
    //このときCanvasのサイズがマイナスにならないようにしている
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

        //private void TTWidthAndX(DragDeltaEventArgs e)
        //{
        //    if (e.Source == e.OriginalSource)
        //    {
        //        double value = Width - e.HorizontalChange;
        //        if (value >= 0)
        //        {
        //            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
        //            Width = value;
        //        }
        //        else
        //        {
        //            Canvas.SetLeft(this, Canvas.GetLeft(this) + Width);
        //            Width = 0;
        //        }
        //    }
        //}
        //private void TTHeightAndY(DragDeltaEventArgs e)
        //{
        //    if (e.Source == e.OriginalSource)
        //    {
        //        double value = Height - e.VerticalChange;
        //        if (value >= 0)
        //        {
        //            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
        //            Height = value;
        //        }
        //        else
        //        {
        //            Canvas.SetTop(this, Canvas.GetTop(this) + Height);
        //            Height = 0;
        //        }
        //    }
        //}

        //private void TTHeight(DragDeltaEventArgs e)
        //{
        //    if (e.Source == e.OriginalSource)
        //    {
        //        double value = Height + e.VerticalChange;
        //        Height = value >= 0 ? value : 0;
        //    }
        //}

        //private void TTWidth(DragDeltaEventArgs e)
        //{
        //    if (e.Source == e.OriginalSource)
        //    {
        //        double value = Width + e.HorizontalChange;
        //        Width = value >= 0 ? value : 0;
        //    }
        //}


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

}
