using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            BitmapImage image = GetBitmapImage($"D:\\ブログ用\\テスト用画像\\collection1.png");
            DataImage dImage = new() { ImageSource = bitmap, X = 30, Y = 40 };
            TTImage MyTTImage = new(dImage);
            MyRoot.AddItem(MyTTImage, dImage);

            Serialize2($"E:\\20230113.xml", MyRoot.Data);
            var dedata = Deserializer<Data>($"E:\\20230113.xml");
            SaveToZip($"E:\\20230113.zip", MyRoot.Data);
            var neko = LoadFromZip($"E:\\20230113.zip");
            var inu = LoadFromZip1($"E:\\20230113.zip");
        }

        private BitmapImage GetBitmapImage(string path)
        {
            BitmapImage bitmap = new();
            using (var stream = File.OpenRead(path))
            {
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                //bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmap.EndInit();
                //bitmap.Freeze();
            }
            return bitmap;
        }
        private void SaveToZip(string filePath, Data data)
        {
            using (var zipStream = File.Create(filePath))
            {
                using (ZipArchive archive = new(zipStream, ZipArchiveMode.Create))
                {
                    //xmlをzipに詰め込む
                    ZipArchiveEntry entry = archive.CreateEntry("Data.xml");
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
                                entry = archive.CreateEntry(dImage.Guid);
                                //entry = archive.CreateEntry("20230113_" + imageCount + ".png");
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

        private object? LoadFromZip(string filePath)
        {
            using (var zipStream = File.OpenRead(filePath))
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    var entry = archive.GetEntry("Data.xml");
                    if (entry != null)
                    {
                        using (var entryStream = entry.Open())
                        {
                            DataContractSerializer serializer = new(typeof(Data));
                            try
                            {
                                using (var reader = XmlReader.Create(entryStream))
                                {
                                    Data? data = (Data?)serializer.ReadObject(reader);
                                    if (data is DataGroup group)
                                    {
                                        foreach (var item in group.Datas)
                                        {
                                            if (item is DataImage dImage)
                                            {
                                                var neko = dImage.Guid;
                                                var imageEntry = archive.GetEntry(neko);
                                                if (imageEntry != null)
                                                {
                                                    using (var imageStream = imageEntry.Open())
                                                    {
                                                        PngBitmapDecoder decoder =
                                                            new(imageStream,
                                                            BitmapCreateOptions.None,
                                                            BitmapCacheOption.Default);
                                                        dImage.ImageSource = decoder.Frames[0];
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    return data;
                                }
                            }
                            catch (Exception ex) { MessageBox.Show(ex.Message); }
                        }
                    }
                }
            }
            return null;
        }

        private Data? LoadFromZip1(string filePath)
        {
            try
            {
                using FileStream zipStream = File.OpenRead(filePath);
                using ZipArchive archive = new(zipStream, ZipArchiveMode.Read);
                ZipArchiveEntry? entry = archive.GetEntry("Data.xml");
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
                ZipArchiveEntry? imageEntry = archive.GetEntry(data.Guid);
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

        //private void SetImageSource(Data data, ZipArchive archive)
        //{
        //    if (data is DataGroup group)
        //    {
        //        foreach (var item in group.Datas)
        //        {
        //            if (item is DataImage dImage)
        //            {
        //                DecodeAndSetID(dImage, archive);
        //            }
        //        }
        //    }
        //    else if (data is DataImage dataImage)
        //    {
        //        DecodeAndSetID(dataImage, archive);
        //    }
        //}
        //private void DecodeAndSetID(DataImage data, ZipArchive archive)
        //{
        //    string imageID = data.Guid;
        //    var imageEntry = archive.GetEntry(imageID);
        //    if (imageEntry != null)
        //    {
        //        using Stream imageStream = imageEntry.Open();
        //        PngBitmapDecoder decoder =
        //            new(imageStream,
        //            BitmapCreateOptions.None,
        //            BitmapCacheOption.Default);
        //        data.ImageSource = decoder.Frames[0];
        //    }
        //}
        private object? LoadFromZip2(string filePath)
        {
            using (var zipStream = File.OpenRead(filePath))
            {
                using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
                var entry = archive.GetEntry("20230113.xml");
                if (entry != null)
                {
                    using var entryStream = entry.Open();
                    DataContractSerializer serializer = new(typeof(Data));
                    try
                    {
                        using var reader = XmlReader.Create(entryStream);
                        Data? data = (Data?)serializer.ReadObject(reader);
                        if (data is DataGroup group)
                        {
                            foreach (var item in group.Datas)
                            {
                                if (item is DataImage dImage)
                                {
                                    var neko = dImage.Guid;
                                    var imageEntry = archive.GetEntry(neko);
                                    if (imageEntry != null)
                                    {
                                        using var imageStream = imageEntry.Open();
                                        PngBitmapDecoder decoder =
                                            new(imageStream, BitmapCreateOptions.None, BitmapCacheOption.Default);
                                        dImage.ImageSource = decoder.Frames[0];
                                    }
                                }
                            }
                        }
                        return data;
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            return null;
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
