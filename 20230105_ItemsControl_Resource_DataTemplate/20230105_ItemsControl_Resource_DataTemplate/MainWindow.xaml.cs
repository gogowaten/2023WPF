using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
//Canvasコントロールの子要素を動的に増減させたい
//https://teratail.com/questions/359699
//マツオソフトウェアブログ: Canvasにリストの中身をBindingする方法
//http://my-clip-devdiary.blogspot.com/2011/01/canvasbinding.html
//

namespace _20230105_ItemsControl_Resource_DataTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ItemData> MyDataList { get; set; } = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MyDataList;
            MyDataList.Add(new DataText() { Text = "TestTextBlock", FontSize = 20, X = 0, Y = 0 });
            MyDataList.Add(new DataRectangle()
            {
                FillBrush = Brushes.LightGray,
                Height = 40,
                Width = 120,
                X = 20,
                Y = 30
            });
            SetResourceToItemsControl(MyItemsControl);
        }
        private void SetResourceToItemsControl(ItemsControl itemsControl)
        {
            ResourceDictionary resource = new();
            itemsControl.Resources = resource;
            itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding());
            FrameworkElementFactory itemsPanel = new(typeof(Canvas));
            itemsControl.ItemsPanel = new ItemsPanelTemplate(itemsPanel);

            Style itemStyle = new();
            itemStyle.Setters.Add(new Setter(Canvas.LeftProperty, new Binding(nameof(DataText.X))));
            itemStyle.Setters.Add(new Setter(Canvas.TopProperty, new Binding(nameof(DataText.Y))));
            itemsControl.ItemContainerStyle= itemStyle;


            DataTemplate dt = new(typeof(DataText));
            FrameworkElementFactory factory = new(typeof(TextBlock));
            factory.SetBinding(TextBlock.TextProperty, new Binding(nameof(DataText.Text)));
            factory.SetBinding(TextBlock.FontSizeProperty, new Binding(nameof(DataText.FontSize)));
            dt.VisualTree = factory;
            DataTemplateKey key = new(typeof(DataText));
            resource.Add(key, dt);

            dt = new(typeof(DataRectangle));
            factory = new(typeof(Rectangle));
            factory.SetBinding(WidthProperty, new Binding(nameof(DataRectangle.Width)));
            factory.SetBinding(HeightProperty, new Binding(nameof(DataRectangle.Height)));
            factory.SetBinding(Shape.FillProperty, new Binding(nameof(DataRectangle.FillBrush)));
            dt.VisualTree = factory;
            key = new(typeof(DataRectangle));
            resource.Add(key, dt);
        }
    }

    public abstract class ItemData
    {
        public double X { get; set; }
        public double Y { get; set; }

    }
    public class DataText : ItemData
    {
        public string? Text { get; set; }
        public double FontSize { get; set; } = Application.Current.MainWindow.FontSize;
    }
    public class DataRectangle : ItemData
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public Brush? FillBrush { get; set; }
    }



}
