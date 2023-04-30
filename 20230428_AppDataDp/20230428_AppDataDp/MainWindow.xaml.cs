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

namespace _20230428_AppDataDp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppData MyAppData { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            //MyAppData = new AppData();
            MyAppData = LoadData<AppData>($"E://20230428_AppDataDpTest.xml");
            MyBindings();
        }
        private void MyBindings()
        {
            SetBinding(LeftProperty, new Binding() { Source = MyAppData, Path = new PropertyPath(AppData.LeftProperty), Mode = BindingMode.TwoWay });

            //Target用
            MyTextBoxTarget.SetBinding(TextBox.TextProperty, new Binding() { Source = MyAppData, Path = new PropertyPath(AppData.MemoProperty), Mode = BindingMode.TwoWay });

            //以下は確認用
            MyTextBlock0.SetBinding(TextBlock.TextProperty, new Binding() { Source = MyAppData, Path = new PropertyPath(AppData.LeftProperty) });
            MyTextBlock1.SetBinding(TextBlock.TextProperty, new Binding() { Source = MyAppData, Path = new PropertyPath(AppData.MemoProperty) });

        }

        #region シリアライズ

        private void SaveData<T>(string fileName, T data)
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

        private T LoadData<T>(string fileName)
        {
            DataContractSerializer serializer = new(typeof(T));
            try
            {
                using XmlReader reader = XmlReader.Create(fileName);
                if (serializer.ReadObject(reader) is T t)
                {
                    return t;
                }
                else throw new NullReferenceException();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        #endregion シリアライズ

        private void MyButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string fileName = $"E://20230428_AppDataDpTest.xml";
            SaveData<AppData>(fileName, MyAppData);
        }

        private void MyButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            string fileName = $"E://20230428_AppDataDpTest.xml";
            AppData data = LoadData<AppData>(fileName);
            ChangeData(data);
        }
        private void ChangeData(AppData data)
        {
            MyAppData = data;
            MyBindings();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var neko = MyAppData;
        }
    }
}
