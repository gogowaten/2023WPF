using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        private AppData MyAppData = new();
        public AppData AppDataDP
        {
            get { return (AppData)GetValue(AppDataDPProperty); }
            set { SetValue(AppDataDPProperty, value); }
        }
        public static readonly DependencyProperty AppDataDPProperty =
            DependencyProperty.Register(nameof(AppDataDP), typeof(AppData), typeof(MainWindow),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private DataNotify MyDataNotify { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();

            //Sourceを変更してからBindingする
            MyAppData = new AppData();
            MyBinding2();

            //MyInitia1();

        }

        private void MyInitia1()
        {
            //Sourceを変更してからBindingする
            AppDataDP = new();

            SetBinding(LeftProperty, new Binding() { Source = AppDataDP, Path = new PropertyPath(AppData.AppLeftProperty), Mode = BindingMode.TwoWay });
            SetBinding(TopProperty, new Binding() { Source = AppDataDP, Path = new PropertyPath(AppData.AppTopProperty), Mode = BindingMode.TwoWay });
            MyTextBlockLeft.SetBinding(TextBlock.TextProperty, new Binding() { Source = AppDataDP, Path = new PropertyPath(AppData.AppLeftProperty), Mode = BindingMode.TwoWay });
            MyTextBlockTop.SetBinding(TextBlock.TextProperty, new Binding() { Source = AppDataDP, Path = new PropertyPath(AppData.AppTopProperty), Mode = BindingMode.TwoWay });
        }
        private void MyBinding2()
        {
            //Twoway必須、なんで？
            SetBinding(LeftProperty, new Binding() { Source = MyAppData, Path = new PropertyPath(AppData.AppLeftProperty), Mode = BindingMode.TwoWay });
            SetBinding(TopProperty, new Binding() { Source = MyAppData, Path = new PropertyPath(AppData.AppTopProperty), Mode = BindingMode.TwoWay });

            //以下は確認用
            MyTextBlockLeft.SetBinding(TextBlock.TextProperty, new Binding() { Source = MyAppData, Path = new PropertyPath(AppData.AppLeftProperty), Mode = BindingMode.TwoWay, StringFormat = $"left = 0.00" });
            MyTextBlockTop.SetBinding(TextBlock.TextProperty, new Binding() { Source = MyAppData, Path = new PropertyPath(AppData.AppTopProperty), Mode = BindingMode.TwoWay, StringFormat = $"top = 0.00" });
        }

        private void MainWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetMyBindings();
        }

        //【WPF】ComboBoxの使い方と実装方法を解説（バインド）｜○NAKA BLOG
        //        https://marunaka-blog.com/wpf-combobox/3223/

        private void SetMyComboBox()
        {
            //MyComboBoxAppDatas.DisplayMemberPath = nameof(AppData.AppLeft);// "AppLeft";
            //MyComboBoxAppDatas.DisplayMemberPath = nameof(AppData.Name);
            //MyComboBoxAppDatas.SelectedValuePath = "AppLeft";
            //MyComboBoxAppDatas.DataContext = MyAppDatas;
            //MyComboBoxAppDatas.SetBinding(ComboBox.ItemsSourceProperty, new Binding(nameof(MyAppDatas.Datas)));
            //MyComboBoxAppDatas.SetBinding(ComboBox.SelectedItemProperty, new Binding(nameof(MyAppDatas.SelectData)));
        }
        private void SetMyBindings()
        {
            //ModeTwoway必須、なんで？
            SetBinding(LeftProperty, new Binding() { Source = MyAppData, Path = new PropertyPath(AppData.AppLeftProperty), Mode = BindingMode.TwoWay });
            SetBinding(TopProperty, new Binding() { Source = MyAppData, Path = new PropertyPath(AppData.AppTopProperty), Mode = BindingMode.TwoWay });

            //TextBox -> AppData    AppData優先
            MyTextBox.SetBinding(TextBox.TextProperty, new Binding() { Source = MyAppData, Path = new PropertyPath(AppData.AppTextProperty) });

            ////AppData -> TextBox    XAML優先
            //BindingOperations.SetBinding(MyAppData, AppData.AppTextProperty, new Binding() { Source = MyTextBox, Path = new PropertyPath(TextBox.TextProperty) });

            MyTextBox2.SetBinding(TextBox.TextProperty, new Binding() { Source = MyAppData, Path = new PropertyPath(AppData.AppText2Property) });

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext;
            var mad = MyAppData;
            var stack = MyStackPanel.DataContext;
            var appdp = AppDataDP;
        }

        //保存
        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            string fileName = $"E:\\appDatasTest.xml";
            SaveAppDatas<AppData>(fileName, MyAppData);
            fileName = $"E:\\20230428_appDatasTest.xml";
            SaveAppDatas<DataNotify>(fileName, MyDataNotify);
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
            var data = DataLoad<AppData>(fileName);
            MyAppData = data;

        }

        private T DataLoad<T>(string fileName)
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
            catch (Exception)
            {
                throw new ArgumentException();
            }

        }

        //Dataの変更
        private void Button_Click_Data01Change(object sender, RoutedEventArgs e)
        {
            MyAppData.AppLeft = 100;
        }

        private void Button_Click_ChangeData(object sender, RoutedEventArgs e)
        {
            AppData data = new();
            data.AppLeft = 500;
            MyAppData = data;
            MyBinding2();
        }
    }
}
