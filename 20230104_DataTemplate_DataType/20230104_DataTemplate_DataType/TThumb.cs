using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace _20230104_DataTemplate_DataType
{
    public class TThumb : Thumb
    {
        public TThumb()
        {

        }

    }
    public class TTRectangle : TThumb
    {
        //public DataRect MyData { get; set; } = new();


        public DataRect MyData
        {
            get { return (DataRect)GetValue(MyDataProperty); }
            set { SetValue(MyDataProperty, value); }
        }
        public static readonly DependencyProperty MyDataProperty =
            DependencyProperty.Register(nameof(MyData), typeof(DataRect), typeof(TTRectangle), new PropertyMetadata(null));

        public double MyWidth
        {
            get { return (double)GetValue(MyWidthProperty); }
            set { SetValue(MyWidthProperty, value); }
        }
        public static readonly DependencyProperty MyWidthProperty =
            DependencyProperty.Register(nameof(MyWidth), typeof(double), typeof(TTRectangle),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double MyHeight
        {
            get { return (double)GetValue(MyHeightProperty); }
            set { SetValue(MyHeightProperty, value); }
        }
        public static readonly DependencyProperty MyHeightProperty =
            DependencyProperty.Register(nameof(MyHeight), typeof(double), typeof(TTRectangle),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Brush MyFillBrush
        {
            get { return (Brush)GetValue(MyFillBrushProperty); }
            set { SetValue(MyFillBrushProperty, value); }
        }
        public static readonly DependencyProperty MyFillBrushProperty =
            DependencyProperty.Register(nameof(MyFillBrush), typeof(Brush), typeof(TTRectangle),
                new FrameworkPropertyMetadata(Brushes.Blue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public TTRectangle()
        {
            DataContext = this;

            FrameworkElementFactory factory = new(typeof(Rectangle));
            factory.SetValue(WidthProperty, new Binding(nameof(MyWidth)));
            factory.SetValue(HeightProperty, new Binding(nameof(MyHeight)));
            factory.SetValue(Shape.FillProperty, Brushes.Tomato);
            //factory.SetValue(Shape.FillProperty, new Binding(nameof(MyFillBrush)));
            this.Template = new() { VisualTree = factory };

        }
    }
    public class TTTextBlock : TThumb
    {

    }

    [DebuggerDisplay("mydata = {" + nameof(MyData) + "}")]
    public class TTGroup : Thumb
    {

        public ObservableCollection<Data> MyData { get; set; } = new();
        public TTGroup()
        {
            DataContext = this;
            SetTemplate();
            SetResource();


        }
        private void SetTemplate()
        {
            FrameworkElementFactory fCanvas = new(typeof(Canvas));
            ItemsPanelTemplate panelTemp = new(fCanvas);
            FrameworkElementFactory fItems = new(typeof(ItemsControl));
            fItems.SetValue(ItemsControl.ItemsPanelProperty, panelTemp);
            fItems.SetValue(ItemsControl.ItemsSourceProperty, new Binding(nameof(MyData)));

            Style ss = new();
            Setter sst = new(Canvas.LeftProperty, new Binding(nameof(Data.X)));
            ss.Setters.Add(sst);
            sst = new(Canvas.TopProperty, new Binding(nameof(Data.Y)));
            ss.Setters.Add(sst);
            fItems.SetValue(ItemsControl.ItemContainerStyleProperty, ss);

            this.Template = new() { VisualTree = fItems };

        }
        private void SetResource()
        {
            ResourceDictionary resource = new();

            FrameworkElementFactory factory = new(typeof(Rectangle));
            factory.SetValue(WidthProperty, new Binding(nameof(DataRect.Width)));
            factory.SetValue(HeightProperty, new Binding(nameof(DataRect.Height)));
            factory.SetValue(Shape.FillProperty, new Binding(nameof(DataRect.Brush)));

            DataTemplate dt = new(typeof(DataRect));
            DataTemplateKey dkey = new(typeof(Rectangle));
            dt.VisualTree = factory;
            resource.Add(dkey, dt);

            factory = new(typeof(TextBlock));
            factory.SetValue(TextBlock.FontSizeProperty, new Binding(nameof(DataText.FontSize)));
            factory.SetValue(TextBlock.TextProperty, new Binding(nameof(DataText.Text)));
            factory.SetValue(Canvas.LeftProperty, new Binding(nameof(DataText.X)));
            dt = new(typeof(DataText));
            dt.VisualTree = factory; ;
            resource.Add(new DataTemplateKey(typeof(DataText)), dt);


            this.Resources = resource;
        }
    }

    public class DTThumb : ContentControl
    {
        public Data? MyData { get; set; }
        public DTThumb(Data data)
        {
            MyData = data;
            DataContext = MyData;
            SetTemplate();
            SetBinding(ContentControl.ContentProperty, new Binding());
        }
        private void SetTemplate()
        {
            ResourceDictionary resource = new();
            FrameworkElementFactory factory = new(typeof(TTRectangle));
            factory.SetBinding(TTRectangle.MyWidthProperty, new Binding(nameof(DataRect.Width)));
            factory.SetBinding(TTRectangle.MyHeightProperty, new Binding(nameof(DataRect.Height)));
            factory.SetBinding(TTRectangle.MyFillBrushProperty, new Binding(nameof(DataRect.Brush)));
            factory.SetValue(TTRectangle.MyDataProperty, new Binding(nameof(MyData)));
            //factory.SetValue(WidthProperty, 100.0);
            //factory.SetValue(HeightProperty, 20.0);
            //factory.SetValue(Shape.FillProperty, Brushes.Black);
            DataTemplate dt = new(typeof(DataRect));
            dt.VisualTree = factory;
            resource.Add(new DataTemplateKey(typeof(DataRect)), dt);

            factory = new(typeof(TTTextBlock));
            factory.SetBinding(TextBlock.TextProperty, new Binding(nameof(DataText.Text)));
            factory.SetBinding(TextBlock.FontSizeProperty, new Binding(nameof(DataText.FontSize)));
            dt = new(typeof(DataText));
            dt.VisualTree = factory;
            resource.Add(new DataTemplateKey(typeof(DataText)), dt);

            this.Resources = resource;

        }

    }
    public class Data
    {
        public Data() { }

        public double X { get; set; }
        public double Y { get; set; }

    }
    public class DataText : Data
    {
        public string Text { get; set; } = "";
        public double FontSize { get; set; }
    }
    public class DataRect : Data
    {
        public Brush Brush { get; set; } = Brushes.MediumAquamarine;
        public double Width { get; set; }
        public double Height { get; set; }
    }

}
