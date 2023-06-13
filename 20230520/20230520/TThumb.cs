using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Markup;

using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Globalization;

namespace _20230520
{

    public class TThumbResizableCanvas : TThumb
    {
        #region 依存関係プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TThumbResizableCanvas),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        //public Rect MyRenderRect
        //{
        //    get { return (Rect)GetValue(MyRenderRectProperty); }
        //    set { SetValue(MyRenderRectProperty, value); }
        //}
        //public static readonly DependencyProperty MyRenderRectProperty =
        //    DependencyProperty.Register(nameof(MyRenderRect), typeof(Rect), typeof(TThumbResizableCanvas),
        //        new FrameworkPropertyMetadata(new Rect()));

        //public PathGeometry MyGeometry
        //{
        //    get { return (PathGeometry)GetValue(MyGeometryProperty); }
        //    set { SetValue(MyGeometryProperty, value); }
        //}
        //public static readonly DependencyProperty MyGeometryProperty =
        //    DependencyProperty.Register(nameof(MyGeometry), typeof(PathGeometry), typeof(TThumbResizableCanvas),
        //        new FrameworkPropertyMetadata(null));


        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(TThumbResizableCanvas),
                new FrameworkPropertyMetadata(new Pen(Brushes.Red, 10.0),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(TThumbResizableCanvas),
                new FrameworkPropertyMetadata(Brushes.Blue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyStrokeThickness
        {
            get { return (double)GetValue(MyStrokeThicknessProperty); }
            set { SetValue(MyStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MyStrokeThickness), typeof(double), typeof(TThumbResizableCanvas),
                new FrameworkPropertyMetadata(1.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ

        public ResizableCanvas MyResizableCanvas { get; private set; }
        public GeoShape3 MyGeoShape { get; private set; } = new();

        public TThumbResizableCanvas()
        {
            MyResizableCanvas = SetMyTemplate();
            MyResizableCanvas.Children.Add(MyGeoShape);
            SetMyBindings();
        }

        private ResizableCanvas SetMyTemplate()
        {
            FrameworkElementFactory fCanvas = new(typeof(ResizableCanvas), "nemo");
            Template = new ControlTemplate() { VisualTree = fCanvas };
            ApplyTemplate();
            return (ResizableCanvas)Template.FindName("nemo", this);
        }

        private void SetMyBindings()
        {
            MyGeoShape.SetBinding(GeoShape3.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });
            MyGeoShape.SetBinding(GeoShape3.MyPenProperty, new Binding() { Source = this, Path = new PropertyPath(MyPenProperty) });
            MyGeoShape.SetBinding(Shape.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeProperty) });
            MyGeoShape.SetBinding(Shape.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(MyStrokeThicknessProperty) });

        }
    }

    [ContentProperty(nameof(MyElement))]
    public class TThumbContent : TThumb
    {

        public FrameworkElement MyElement
        {
            get { return (FrameworkElement)GetValue(MyElementProperty); }
            set { SetValue(MyElementProperty, value); }
        }
        public static readonly DependencyProperty MyElementProperty =
            DependencyProperty.Register(nameof(MyElement), typeof(FrameworkElement), typeof(TThumbContent),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ContentControl MyTemplate { get; set; }
        public TThumbContent()
        {
            MyTemplate = SetTemplate();
            MyTemplate.Width = 100;
            MyTemplate.Height = 100;
            MyTemplate.Background = Brushes.Red;
            MyTemplate.SetBinding(ContentControl.ContentProperty, new Binding() { Source = this, Path = new PropertyPath(MyElementProperty) });
        }
        private ContentControl SetTemplate()
        {
            FrameworkElementFactory fItems = new(typeof(ContentControl), "nemo");

            Template = new ControlTemplate() { VisualTree = fItems };
            ApplyTemplate();
            return (ContentControl)Template.FindName("nemo", this);
        }
    }

    [ContentProperty(nameof(MyThumbs))]
    public class TThumbGroup2 : TThumb
    {
        public ItemsControl MyTemplateItemsControl { get; set; }
        public TTItemCollection MyThumbs { get; set; }
        public TThumbGroup2()
        {
            MyThumbs = new(this);
            MyTemplateItemsControl = SetTemplate();
        }
        private ItemsControl SetTemplate()
        {
            FrameworkElementFactory fItems = new(typeof(ItemsControl), "nemo");
            ItemsPanelTemplate ipt = new(new FrameworkElementFactory(typeof(Canvas)));
            fItems.SetValue(ItemsControl.ItemsPanelProperty, ipt);
            fItems.SetValue(ItemsControl.ItemsSourceProperty, new Binding(nameof(MyThumbs)) { Source = this });

            Template = new ControlTemplate() { VisualTree = fItems };
            ApplyTemplate();
            return (ItemsControl)Template.FindName("nemo", this);
        }
    }

    public class TTItemCollection : ObservableCollection<TThumb>
    {
        public TThumbGroup2 MyOwner { get; set; }
        public TTItemCollection(TThumbGroup2 group)
        {
            MyOwner = group;
        }
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            double x = double.MaxValue;
            foreach (var item in Items)
            {
                var xx = item.X;
                if (x > item.X) x = item.X;
            }
            MyOwner.X = x;
        }
        //protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        //{
        //    base.OnPropertyChanged(e);
        //}
    }

    /// <summary>
    /// グループ用TThumb
    /// </summary>
    [ContentProperty(nameof(MyThumbs))]
    public class TThumbGroup : TThumb
    {

        public ObservableCollection<TThumb> MyThumbs
        {
            get { return (ObservableCollection<TThumb>)GetValue(MyThumbsProperty); }
            set { SetValue(MyThumbsProperty, value); }
        }
        public static readonly DependencyProperty MyThumbsProperty =
            DependencyProperty.Register(nameof(MyThumbs), typeof(ObservableCollection<TThumb>), typeof(TThumbGroup),
                new FrameworkPropertyMetadata(new ObservableCollection<TThumb>(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ItemsControl MyTemplateItemsControl { get; set; }
        //public ObservableCollection<TThumb> MyThumbs { get; set; } = new();
        public TThumbGroup()
        {
            MyTemplateItemsControl = SetTemplate();

        }
        private ItemsControl SetTemplate()
        {
            FrameworkElementFactory fItems = new(typeof(ItemsControl), "nemo");
            ItemsPanelTemplate ipt = new(new FrameworkElementFactory(typeof(Canvas)));
            fItems.SetValue(ItemsControl.ItemsPanelProperty, ipt);
            fItems.SetValue(ItemsControl.ItemsSourceProperty, new Binding(nameof(MyThumbs)) { Source = this });

            Template = new ControlTemplate() { VisualTree = fItems };
            ApplyTemplate();
            return (ItemsControl)Template.FindName("nemo", this);
        }

    }

    public class MyConverterLocate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<TThumb> thumbs = (ObservableCollection<TThumb>)value;
            if (thumbs == null || thumbs.Count == 0) return 0;

            double x = double.MaxValue;
            foreach (var item in thumbs)
            {
                if (x > item.X) x = item.X;
            }
            return x;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// TextBlockのThumb
    /// </summary>
    public class TThumbText : TThumbC
    {

        public string MyText
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(nameof(MyText), typeof(string), typeof(TThumbText),
                new FrameworkPropertyMetadata("",
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TextBlock MyTextBlock { get; set; } = new TextBlock();
        public TThumbText()
        {
            MyTemplateCanvas.Children.Add(MyTextBlock);
            MyTextBlock.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath(MyTextProperty), Mode = BindingMode.TwoWay });
            SetBinding(WidthProperty, new Binding() { Source = MyTextBlock, Path = new PropertyPath(ActualWidthProperty) });
            SetBinding(HeightProperty, new Binding() { Source = MyTextBlock, Path = new PropertyPath(ActualHeightProperty) });
            SetBinding(BackgroundProperty, new Binding() { Source = MyTextBlock, Path = new PropertyPath(TextBlock.BackgroundProperty), Mode = BindingMode.TwoWay });

        }
    }

    /// <summary>
    /// TemplateがCanvasのThumb
    /// </summary>
    public class TThumbC : TThumb
    {
        public Canvas MyTemplateCanvas { get; set; }
        public TThumbC()
        {
            MyTemplateCanvas = SetTemplateCanvas();
        }
        private Canvas SetTemplateCanvas()
        {
            FrameworkElementFactory fCanvas = new(typeof(Canvas), "nemo");
            //自身の背景色をCanvasの背景色とバインド、TemplateBindingを使ったけど、普通のバインドとどう違うのかわからん
            //fCanvas.SetValue(BackgroundProperty, new TemplateBindingExtension(BackgroundProperty));
            Template = new ControlTemplate() { VisualTree = fCanvas };
            ApplyTemplate();
            return (Canvas)Template.FindName("nemo", this);
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

        public bool MyIsMove { get; set; } = true;
        public TThumb()
        {
            SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(XProperty), Mode = BindingMode.TwoWay });
            SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(YProperty), Mode = BindingMode.TwoWay });
            DragDelta += TThumb_DragDelta;
        }

        private void TThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (MyIsMove)
            {
                X += e.HorizontalChange;
                Y += e.VerticalChange;
            }
        }
    }

}
