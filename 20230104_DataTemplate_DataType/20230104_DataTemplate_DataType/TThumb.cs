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

namespace _20230104_DataTemplate_DataType
{
    public class TThumb : Thumb
    {
        public TThumb()
        {
            DataTemplate dTemp = new(typeof(Rectangle));

        }

    }
    public class TTRectangle : TThumb
    {
        public RectangleItem MyData { get; set; } = new();
        public TTRectangle()
        {
            DataContext = MyData;
        }
    }

    public class TTGroup : Thumb
    {
        public ObservableCollection<Data> MyData { get; set; } = new();
        public TTGroup()
        {
            DataContext = this;
            SetTemplate();
            SetResource();

            var neko = this.Resources;
            foreach (var data in neko.Values)
            {

            }

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
            sst=new(Canvas.TopProperty, new Binding(nameof(Data.Y)));
            ss.Setters.Add(sst);
            fItems.SetValue(ItemsControl.ItemContainerStyleProperty, ss);

            this.Template = new() { VisualTree = fItems };

        }
        private void SetResource()
        {
            ResourceDictionary resource = new();

            FrameworkElementFactory factory = new(typeof(Rectangle));
            factory.SetValue(WidthProperty, new Binding(nameof(Data.Width)));
            factory.SetValue(HeightProperty, new Binding(nameof(Data.Height)));
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

    public class Data
    {
        public Data() { }
        public Data(double x, double y, double z, double width, double height)
        {
            X = x;
            Y = y;
            Z = z;
            Width = width;
            Height = height;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
    public class DataText : Data
    {
        public DataText(string text, double fontSize, double x, double y, double z) : base(x, y, z, double.NaN, double.NaN)
        {
            Text = text;
            FontSize = fontSize;
        }

        public string Text { get; set; }
        public double FontSize { get; set; }
    }
    public class DataRect : Data
    {
        public DataRect(Brush brush, double x, double y, double z, double width, double height) : base(x, y, z, width, height)
        {
            Brush = brush;
        }

        public Brush Brush { get; set; }
    }

}
