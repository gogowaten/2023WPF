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
            MyDataList.Add(new DataText() { Text = "test1", FontSize = 20, X = 0, Y = 0 });
            MyDataList.Add(new DataRectangle() { FillBrush = Brushes.Gold, Height = 20, Width = 120, X = 20, Y = 30 });
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
