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


//派生クラスの種類がいっぱいあるときのシリアライズ、デシリアライズ

//派生クラスをシリアライズ、デシリアライズするとき
//A：派生クラス型でシリアライズして、派生クラス型でデシリアライズ
//B：派生クラス型でシリアライズして、基底クラス型でデシリアライズ
//C：基底クラス型でシリアライズして、基底クラス型でデシリアライズ
//D：基底クラス型でシリアライズして、派生クラス型でデシリアライズ
//結果
//Cがラク、デシリアライズしたあとにキャストする必要があるけど、どの派生クラスかを知らなくてもいい
//AとDはめんどくさい、デシリアライズするときに、どの派生クラスなのかを知っている必要がある
//Bはデシリアライズでエラーになる

namespace _20230111_XmlSerializeMatome
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Test1();
            TestB();
            TestC();
            TestD();
            Test4Group();
            //Test5();
            Test6Matome();
        }


        private void Test1()
        {
            Data data = new() { X = 123.4 };
            string filePath = $"E:\\20230111.xml";
            MySerialize(filePath, data);
            var neko = MyDeserializer<Data>(filePath);
        }

        private void TestA()
        {
            DataText data = new() { X = 123.4, FontSize = 20, Text = "ﾎｲ！" };
            string filePath = $"E:\\20230111.xml";
            MySerialize(filePath, data);
            DataText? neko = MyDeserializer<DataText>(filePath);
            string? inu = neko?.Text;
        }
        private void TestB()
        {
            DataText data = new() { X = 123.4, FontSize = 20, Text = "ﾎｲ！" };
            string filePath = $"E:\\20230111.xml";
            MySerialize(filePath, data);
            var neko = MyDeserializer<Data>(filePath);//これはエラーになる            
        }

        private void TestC()
        {
            DataText data = new() { X = 123.4, FontSize = 20, Text = "ﾎｲ！" };
            string filePath = $"E:\\20230111.xml";
            //基底クラス型でシリアライズ、これでも派生クラスのプロパティは保存されてる
            MySerialize2(filePath, data);
            Data? neko = MyDeserializer<Data>(filePath);
            string? inu = ((DataText?)neko)?.Text;

        }
        private void TestD()
        {
            DataText data = new() { X = 123.4, FontSize = 20, Text = "ﾎｲ！" };
            string filePath = $"E:\\20230111.xml";
            //基底クラス型でシリアライズ、これでも派生クラスのプロパティは保存されてる
            MySerialize2(filePath, data);
            DataText? neko = MyDeserializer<DataText>(filePath);
            string? inu = neko?.Text;
        }

        private void Test4Group()
        {
            DataText dataText = new() { X = 123.4, FontSize = 20, Text = "ﾃﾞｨｽﾃｨﾆｰｽﾄｰﾝ" };
            DataShape dataShape = new() { X = 123.4, FillBrush = Brushes.Aquamarine };
            DataGroup data = new();
            data.Datas.Add(dataShape);
            data.Datas.Add(dataText);
            string filePath = $"E:\\20230111.xml";
            MySerialize2(filePath, data);
            var neko = MyDeserializer<Data>(filePath);
        }

        private void Test5()
        {
            DataShape shapeData = new() { X = 123.4, FillBrush = Brushes.Aquamarine };
            MatomeData data = new() { Matome = shapeData };
            string filePath = $"E:\\20230111.xml";
            MySerialize(filePath, data);
            var neko = MyDeserializer<MatomeData>(filePath);
        }
        private void Test6Matome()
        {
            DataShape shapeData = new() { X = 123.4, FillBrush = Brushes.Aquamarine };
            DataText textData = new() { FontSize = 30, Text = "ｺｺﾏﾃﾞｶｰ", X = 654.321 };
            DataGroup dataGroup = new();
            dataGroup.Datas.Add(shapeData);
            dataGroup.Datas.Add(textData);
            MatomeData data = new() { Matome = dataGroup };
            string filePath = $"E:\\20230111.xml";
            MySerialize(filePath, data);
            var neko = MyDeserializer<MatomeData>(filePath);
        }


        #region シリアライズ

        //それぞれの派生クラス型でシリアライズ
        private static void MySerialize<T>(string filePath, T obj)
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
        private static void MySerialize2(string filePath, Data obj)
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

        private static T? MyDeserializer<T>(string filePath)
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
    }
}
