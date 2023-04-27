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
using System.Xml;

namespace _20230427_AppDataTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public AppDatas MyAppDatas { get; set; }
        public AppData MyAppData { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            MyAppDatas = new();
            MyAppData = new() { AppLeft = 200 };
            SetMyBindings();
            SetMyComboBox();
        }

        private void SetMyComboBox()
        {
            MyComboBoxAppDatas.ItemsSource = MyAppDatas.Datas;
            
        }
        private void SetMyBindings()
        {
            DataContext = MyAppDatas.Datas[0];
            SetBinding(LeftProperty, new Binding() { Path = new PropertyPath(AppData.AppLeftProperty) });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = MyAppData;
            SetBinding(LeftProperty, new Binding() { Path = new PropertyPath(AppData.AppLeftProperty) });
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {

        }
        private void DataSave(string fileName,AppDatas data)
        {
            XmlWriterSettings settings = new()
            {
                Encoding = new UTF8Encoding(false),
                Indent = true,
                NewLineOnAttributes = false,
                ConformanceLevel = ConformanceLevel.Fragment
            };

        }
    }
}
