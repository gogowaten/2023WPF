using System;
using System.Collections.Generic;
using System.IO;
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
            DataImage dImage = new() { ImageSource = bitmap, X = 30, Y = 40 };
            TTImage MyTTImage = new(dImage);
            MyRoot.AddItem(MyTTImage, dImage);

            Serialize2($"E:\\20230113.xml", MyRoot.Data);
            var dedata = Deserializer<Data>($"E:\\20230113.xml");
            MakePng($"E:\\20230113.zip", MyRoot.Data);
        }

        private void MakePng(string filePath, Data data)
        {
            using (var zipStream = File.Create(filePath))
            {
                using (ZipArchive archive = new(zipStream, ZipArchiveMode.Create))
                {
                    //xmlをzipに詰め込む
                    ZipArchiveEntry entry = archive.CreateEntry("20230113.xml");
                    using (var entryStream = entry.Open())
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

                    //画像をzipに詰め込む
                    if (data is DataGroup group)
                    {
                        int imageCount = 0;
                        foreach (var item in group.Datas)
                        {
                            if (item is DataImage dImage)
                            {
                                entry = archive.CreateEntry("20230113_" + imageCount + ".png");
                                using (var entryStream = entry.Open())
                                {
                                    PngBitmapEncoder encoder = new();
                                    encoder.Frames.Add(BitmapFrame.Create(dImage.ImageSource));
                                    using (var memStream = new MemoryStream())
                                    {
                                        encoder.Save(memStream);
                                        memStream.Position = 0;
                                        memStream.CopyTo(entryStream);
                                        imageCount++;
                                    }

                                }

                            }
                        }
                    }

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
