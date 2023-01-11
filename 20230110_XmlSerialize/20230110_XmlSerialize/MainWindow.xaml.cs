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
using System.Windows.Markup;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace _20230110_XmlSerialize
{
    public partial class MainWindow : Window
    {
        private Data MyData;
        public MainWindow()
        {
            InitializeComponent();

            //DataContractSerializerで、上位下位互換性を保ってシリアル化、逆シリアル化できるようにする - .NET Tips (VB.NET,C#...)
            //        https://dobon.net/vb/dotnet/file/iextensibledataobject.html

            #region 上位下位互換性を保ってシリアル化
            //string fileNameV1 = @"E:\\V1.xml";
            //string fileNameV2 = @"E:\\V2.xml";

            ////保存する設定を作成する
            //SampleSettingsV2 settingsV2 = new SampleSettingsV2();
            //settingsV2.ID = 2;
            //settingsV2.Message = "こんにちは。";

            ////バージョン2の設定を書き込む
            //SaveSettings(fileNameV2, settingsV2);

            ////バージョン2の設定をバージョン1で読み込む
            //SampleSettingsV1? settingsV1 =
            //    LoadSettings<SampleSettingsV1>(fileNameV2);

            ////バージョン1のまま書き込む
            //SaveSettings(fileNameV1, settingsV1);

            ////バージョン2でバージョン1の設定を読み込む
            //SampleSettingsV2? settingsV2_2 =
            //    LoadSettings<SampleSettingsV2>(fileNameV1);

            ////バージョン2にしかないIDプロパティが復元されていることを確認する
            //Console.WriteLine(settingsV2_2.ID);
            #endregion 上位下位互換性を保ってシリアル化



            MyData = new()
            {
                X = 123,
                FillBrush = Brushes.MediumAquamarine
            };
            Serialize($"E:\\202301101756.xml", MyData);
            Data? data = Deserialize<Data>($"E:\\202301101756.xml");
            //継承して作成した子クラスのDataをデシリアライズしたいときは
            //その型を指定する必要がある。けど、事前にそれがわからないときは
            //総当たりでCastするしかない？
            //それ以外の方法だと、列挙型を作成してそれをDataに追加してData型で
            //でシリアライズしてからキャスト
        }
        private static void Serialize<T>(string filePath, T data)
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
            using (writer = XmlWriter.Create(filePath, settings))
            {
                try
                {
                    serializer.WriteObject(writer, data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private static T? Deserialize<T>(string filePath)
        {
            DataContractSerializer serializer = new(typeof(T));
            try
            {
                using var reader = XmlReader.Create(filePath);
                return (T?)serializer.ReadObject(reader);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //private void Serialize(string filePath, Data data)
        //{
        //    XmlWriterSettings settings = new()
        //    {
        //        Encoding = new UTF8Encoding(false),
        //        Indent = true,
        //        NewLineOnAttributes = false,
        //        ConformanceLevel = ConformanceLevel.Fragment
        //    };
        //    XmlWriter writer;            
        //    DataContractSerializer serializer = new(typeof(Data));            
        //    using (writer = XmlWriter.Create(filePath, settings))
        //    {
        //        try
        //        {
        //            serializer.WriteObject(writer, data);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //}

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //Serialize("E:\\202301101756.xml", MyDDShape);
            Serialize("E:\\202301101756.xml", MyData);
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            var data = Deserialize<Data>($"E:\\202301101756.xml");
        }










        #region 上位下位互換性を保ってシリアル化
        /// <summary>
        /// 設定を保存する
        /// </summary>
        /// <typeparam name="T">設定クラスの型</typeparam>
        /// <param name="fileName">保存先のファイル名</param>
        /// <param name="settings">設定が格納されたオブジェクト</param>
        public static void SaveSettings<T>(string fileName, T settings)
        {
            DataContractSerializer serializer = new(typeof(T));
            XmlWriterSettings setting = new()
            {
                Encoding = new UTF8Encoding()
            };
            using XmlWriter xw = XmlWriter.Create(fileName, setting);
            serializer.WriteObject(xw, settings);
        }

        /// <summary>
        /// 設定を読み込む
        /// </summary>
        /// <typeparam name="T">設定クラスの型</typeparam>
        /// <param name="fileName">読み込むファイル名</param>
        /// <returns>設定を格納したオブジェクト</returns>
        public static T? LoadSettings<T>(string fileName)
        {
            DataContractSerializer serializer = new(typeof(T));
            using XmlReader xr = XmlReader.Create(fileName);
            return (T?)serializer.ReadObject(xr);
        }
        #endregion 上位下位互換性を保ってシリアル化
    }
}
