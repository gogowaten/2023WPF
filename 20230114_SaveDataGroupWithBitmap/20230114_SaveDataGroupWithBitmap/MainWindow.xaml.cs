using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
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
using System.ComponentModel.DataAnnotations.Schema;

namespace _20230114_SaveDataGroupWithBitmap
{
    public partial class MainWindow : Window
    {
        private string ZIP_FILE_PATH = "E:\\20230113.zip";
        private string XML_FILE_NAME = "Data.xml";

        public MainWindow()
        {
            InitializeComponent();

            TTTextBlock tTTextBlock = new(new DataTextBlock() { Text = "TTT", X = 100, Y = 100 });
            MyCanvas.Children.Add(tTTextBlock);

            DataTextBlock dataTextBlock = new() { Text = "TTT2", X = 120, Y = 120 };
            MyRoot.AddItem(dataTextBlock);

            DataImage dataImage1 = new() { ImageSource = GetBitmapImage2("D:\\ブログ用\\テスト用画像\\NEC_0541_2017_07_21_午後わてん__Matrix4x4_1.png"), X = 150, Y = 10 };
            SaveToZip1(ZIP_FILE_PATH, dataImage1);
            Data? neko = LoadFromZip1(ZIP_FILE_PATH);

            DataImage dataImage2 = new() { ImageSource = GetBitmapImage2($"D:\\ブログ用\\テスト用画像\\collection1.png"), X = 30, Y = 140 };

            DataGroup dataGroup = new() { X = 10, Y = 10 };
            dataGroup.Datas.Add(dataImage1);
            dataGroup.Datas.Add(dataTextBlock);
            dataGroup.Datas.Add(dataImage2);
            SaveToZip1(ZIP_FILE_PATH, dataGroup);
            Data? inu= LoadFromZip1(ZIP_FILE_PATH);

            TTImage MyTTImage = new(dataImage2);
            MyRoot.AddItem(MyTTImage, dataImage2);

            Serialize2($"E:\\20230113.xml", MyRoot.Data);
            var dedata = Deserializer<Data>($"E:\\20230113.xml");
            SaveToZip1($"E:\\20230113.zip", MyRoot.Data);
            //SaveToZip($"E:\\20230113.zip", MyRoot.Data);
            //var neko = LoadFromZip($"E:\\20230113.zip");
            var uma = LoadFromZip1($"E:\\20230113.zip");
        }


        private BitmapImage GetBitmapImage2(string path)
        {
            BitmapImage bitmap = new();
            FileStream stream = File.OpenRead(path);
            {
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                //bitmap.CacheOption = BitmapCacheOption.OnLoad;
                //bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmap.EndInit();
                //bitmap.Freeze();
            }
            return bitmap;
        }

        private void SaveToZip1(string filePath, Data data)
        {
            using FileStream zipStream = File.Create(filePath);
            using (ZipArchive archive = new(zipStream, ZipArchiveMode.Create))
            {
                //xml形式にシリアライズして、それをzipに詰め込む
                ZipArchiveEntry entry = archive.CreateEntry(XML_FILE_NAME);
                using (Stream entryStream = entry.Open())
                {
                    XmlWriterSettings settings = new()
                    {
                        Indent = true,
                        Encoding = Encoding.UTF8,
                        NewLineOnAttributes = true,
                        ConformanceLevel = ConformanceLevel.Fragment,
                    };
                    DataContractSerializer serializer = new(typeof(Data));
                    using var writer = XmlWriter.Create(entryStream, settings);
                    try { serializer.WriteObject(writer, data); }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                //png形式にした画像をzipに詰め込む
                AAA(archive);
            }

            void AAA(ZipArchive archive)
            {
                if (data is DataGroup group)
                {
                    foreach (var item in group.Datas)
                    {
                        if (item is DataImage dImage)
                        {
                            BBB(dImage, archive);
                        }
                    }
                }
                else if (data is DataImage ddd)
                {
                    BBB(ddd, archive);
                }
            }
            void BBB(DataImage dImage, ZipArchive archive)
            {
                ZipArchiveEntry entry = archive.CreateEntry(dImage.Guid + ".png");
                using Stream entryStream = entry.Open();
                PngBitmapEncoder encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(dImage.ImageSource));
                using MemoryStream memStream = new();
                encoder.Save(memStream);
                memStream.Position = 0;
                memStream.CopyTo(entryStream);
            }
        }


        private Data? LoadFromZip1(string filePath)
        {
            try
            {
                using FileStream zipStream = File.OpenRead(filePath);
                using ZipArchive archive = new(zipStream, ZipArchiveMode.Read);
                ZipArchiveEntry? entry = archive.GetEntry(XML_FILE_NAME);
                if (entry != null)
                {
                    using Stream entryStream = entry.Open();
                    DataContractSerializer serializer = new(typeof(Data));
                    using var reader = XmlReader.Create(entryStream);
                    Data? data = (Data?)serializer.ReadObject(reader);
                    if (data is null) return null;
                    Sub(data, archive);
                    return data;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return null;

            void Sub(Data data, ZipArchive archive)
            {
                if (data is DataGroup group)
                {
                    foreach (Data item in group.Datas)
                    {
                        if (item is DataImage dImage)
                        {
                            SubSub(dImage, archive);
                        }
                    }
                }
                else if (data is DataImage dataImage)
                {
                    SubSub(dataImage, archive);
                }
            }
            void SubSub(DataImage data, ZipArchive archive)
            {
                ZipArchiveEntry? imageEntry = archive.GetEntry(data.Guid + ".png");
                if (imageEntry != null)
                {
                    using Stream imageStream = imageEntry.Open();
                    PngBitmapDecoder decoder =
                        new(imageStream,
                        BitmapCreateOptions.None,
                        BitmapCacheOption.Default);
                    data.ImageSource = decoder.Frames[0];
                }
            }
        }


        #region シリアライズ

        ////それぞれの派生クラス型でシリアライズ
        //private static void Serialize<T>(string filePath, T obj)
        //{
        //    XmlWriterSettings settings = new()
        //    {
        //        Indent = true,
        //        Encoding = Encoding.UTF8,
        //        NewLineOnAttributes = true,
        //        ConformanceLevel = ConformanceLevel.Fragment,
        //    };
        //    DataContractSerializer serializer = new(typeof(T));
        //    using var writer = XmlWriter.Create(filePath, settings);
        //    try { serializer.WriteObject(writer, obj); }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}

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
