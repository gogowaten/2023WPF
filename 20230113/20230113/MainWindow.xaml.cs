using System;
using System.Collections.Generic;
using System.IO.Compression;
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

namespace _20230113
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TTTextBlock tTTextBlock = new(new DataTextBlock() { Text = "TTT", X = 100, Y = 100 });
            MyCanvas.Children.Add(tTTextBlock);

            DataTextBlock data = new() { Text = "TTT2", X = 120, Y = 120 };
            tTTextBlock = new(data);
            //MyRoot.AddItem(tTTextBlock, tTTextBlock.Data);
            MyRoot.AddItem(data);

            Uri uriSource = new($"D:\\ブログ用\\テスト用画像\\collection1.png");
            BitmapImage bitmap = new(uriSource);
            DataImage dImage = new() { ImageSource = bitmap };
            TTImage MyTTImage = new(dImage);
            MyRoot.AddItem(MyTTImage, dImage);

        }

        #region シリアライズ

        //それぞれの派生クラス型でシリアライズ
        private static void Serialize<T>(string filePath, T obj)
        {
            XmlWriterSettings settings = new()
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                NewLineOnAttributes = true,
                ConformanceLevel = ConformanceLevel.Fragment,
            };
            DataContractSerializer serializer = new(typeof(T));
            using var writer = XmlWriter.Create(filePath, settings);
            try { serializer.WriteObject(writer, obj); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //Data型で決め打ちシリアライズ
        private static void Serialize2(string filePath, Data obj)
        {
            XmlWriterSettings settings = new()
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                NewLineOnAttributes = true,
                ConformanceLevel = ConformanceLevel.Fragment,
            };
            DataContractSerializer serializer = new(typeof(Data));
            using var writer = XmlWriter.Create(filePath, settings);
            try { serializer.WriteObject(writer, obj); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private static T? Deserializer<T>(string filePath)
        {
            DataContractSerializer serializer = new(typeof(T));
            try
            {
                using var reader = XmlReader.Create(filePath);
                return (T?)serializer.ReadObject(reader);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return default; }
        }
        #endregion シリアライズ



        private void ButtonTest1_Click(object sender, RoutedEventArgs e)
        {
            var neko = MyRoot.Data;
        }
    }
}
