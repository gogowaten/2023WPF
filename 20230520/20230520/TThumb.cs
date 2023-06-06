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

namespace _20230520
{
    [ContentProperty(nameof(MyThumbs))]
    public class TThumbGroup : TThumb
    {
        public ItemsControl MyTemplateItemsControl { get; set; }
        public ObservableCollection<TThumb> MyThumbs { get; set; } = new();
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
        public TThumb()
        {
            SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(XProperty), Mode = BindingMode.TwoWay });
            SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(YProperty), Mode = BindingMode.TwoWay });

        }
    }

}
