using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        //public AppData MyAppData { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            MyAppDatas = new();
            //DataContextを変更してからBinding設定
            DataContext = MyAppDatas.Datas[0];
            SetMyBindings();
            SetMyComboBox();
        }

        private void SetMyComboBox()
        {            
            MyComboBoxAppDatas.DataContext= MyAppDatas;
            MyComboBoxAppDatas.SetBinding(ComboBox.ItemsSourceProperty, new Binding(nameof(MyAppDatas.Datas)));
        }
        private void SetMyBindings()
        {
            
            SetBinding(LeftProperty, new Binding() { Path = new PropertyPath(AppData.AppLeftProperty) });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        //保存
        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            string fileName = $"E:\\appDatasTest.xml";
            SaveAppDatas<AppDatas>(fileName, MyAppDatas);
        }

        

    

        private void SaveAppDatas<T>(string fileName, T data)
        {
            XmlWriterSettings settings = new()
            {
                Encoding = new UTF8Encoding(false),
                Indent = true,
                NewLineOnAttributes = false,
                ConformanceLevel = ConformanceLevel.Fragment
            };
            XmlWriter writer;
            DataContractSerializer serializer = new(typeof(T));
            using (writer = XmlWriter.Create(fileName, settings))
            {
                try { serializer.WriteObject(writer, data); }
                catch (Exception) { throw; }
            }
        }

        private void Button_Click_Load(object sender, RoutedEventArgs e)
        {
            string fileName = $"E:\\appDatasTest.xml";
            var data = DataLoad<AppDatas>(fileName);
            MyAppDatas = data;

        }

        private T DataLoad<T>(string fileName)
        {
            DataContractSerializer serializer = new(typeof(T));
            try
            {
                using XmlReader reader=XmlReader.Create(fileName);
                if(serializer.ReadObject(reader) is T t)
                {
                    return t;
                }
                else throw new NullReferenceException();
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }

        }

        private void Button_Click_Data01Change(object sender, RoutedEventArgs e)
        {
            MyAppDatas.Datas[1].AppLeft = 123;
        }

        private void Button_Click_data01tekiyou(object sender, RoutedEventArgs e)
        {
            DataContext = MyAppDatas.Datas[1];
            SetMyBindings();
        }
    }
}
