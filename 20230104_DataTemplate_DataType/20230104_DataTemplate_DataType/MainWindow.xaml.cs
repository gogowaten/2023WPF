using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
//【WPF】XAMLでテンプレートを定義とコードで定義は違う。 | 創造的プログラミングと粘土細工
//http://pro.art55.jp/?eid=1150138


namespace _20230104_DataTemplate_DataType
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Item> Items { get; set; } = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Items = new ObservableCollection<Item>
            {
                new PathItem { X = 50, Y = 50, Width = 50, Height = 50, Stroke = Brushes.Red, Data = "M0,25L25,50L50,25L25,0Z", },
                new EllipseItem { X = 200, Y = 50, Width = 50, Height = 100, Fill = Brushes.Green, },
                new RectangleItem { X = 200, Y = 200, Width = 100, Height = 50, Fill = Brushes.Blue, },
            };
            var neko = MyItemsControl.Template;
            ResourceDictionary myresource = this.Resources;
            foreach (var item in myresource.Values)
            {
                var val = item;//DataTemplate
                DataTemplate data = (DataTemplate)item;
                var tree = data.VisualTree;
            }
            foreach (var item in myresource.Keys)
            {
                var val = item;//DataTemplateKey
            }
            ResourceDictionary rd = new();
            DataTemplate dt = new(typeof(RectangleItem));
            FrameworkElementFactory factory = new(typeof(Rectangle));
            factory.SetValue(WidthProperty, new Binding(nameof(RectangleItem.Width)));
            factory.SetValue(HeightProperty, new Binding(nameof(RectangleItem.Height)));
            factory.SetValue(Shape.FillProperty, new Binding(nameof(RectangleItem.Fill)));
            //factory.SetValue(Canvas.LeftProperty, new Binding(nameof(RectangleItem.X)));
            dt.VisualTree = factory;
            DataTemplateKey dkey = new(typeof(RectangleItem));
            myresource.Add(dkey, dt);


            //
            ObservableCollection<Data> myData= new();
            myData.Add(new DataText("text", 20, 110, 110, 0));

            MyTTGroup.MyData = myData;
        }
    }

    public class Item : INotifyPropertyChanged
    {
        //public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        #region 通知プロパティ
        public event PropertyChangedEventHandler PropertyChanged;
        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private double _x;
        public double X { get => _x; set => SetProperty(ref _x, value); }
        #endregion 通知プロパティ

    }
    public class PathItem : Item
    {
        public Brush Stroke { get; set; } = Brushes.Red;
        public string Data { get; set; } = "";
    }

    public class EllipseItem : Item
    {
        public Brush Fill { get; set; } = Brushes.MediumAquamarine;
    }

    //public class ImageItem : Item
    //{
    //    public ImageSource Source { get; set; }
    //}

    public class RectangleItem : Item
    {
        public Brush Fill { get; set; } = Brushes.Lime;
    }

    //public class RichTextBoxItem : Item
    //{
    //    public string Text { get; set; }
    //}
}
