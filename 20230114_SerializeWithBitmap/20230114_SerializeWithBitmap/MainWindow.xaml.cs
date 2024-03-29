﻿using System;
using System.IO.Compression;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;


//WPF、BitmapSourceを含んだクラスをシリアライズ、と言うかファイルに保存と読み込みできた - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2023/01/15/144935

namespace _20230114_SerializeWithBitmap
{
    public partial class MainWindow : Window
    {
        private readonly string ZIP_FILE_PATH = "E:\\20230113.zip";
        private readonly string XML_FILE_NAME = "Data.xml";

        public MainWindow()
        {
            InitializeComponent();

            //DataImage、Bitmapを含むDataクラス
            DataImage dataImage1 = new()
            {
                X = 150,
                Y = 10,
                ImageSource = GetBitmapImage("D:\\ブログ用\\テスト用画像\\NEC_0541_2017_07_21_午後わてん__Matrix4x4_1.png")
            };

            //DataImage単体でセーブロード確認
            SaveToZip(ZIP_FILE_PATH, dataImage1);
            Data? DataImage単体確認用 = LoadFromZip(ZIP_FILE_PATH);


            //階層構造DataGroup、普通にシリアライズできる値とBitmapを混ぜたData
            //DataGroup_Root
            //  ┣DataImage
            //  ┣DataTextBlock
            //  ┣DataImage
            //  ┗DataGroup
            //      ┗DataTextBlock
            DataGroup dataGroupRoot = new() { X = 10, Y = 10 };
            dataGroupRoot.Datas.Add(dataImage1);
            dataGroupRoot.Datas.Add(new DataTextBlock()
            {
                X = 120,
                Y = 120,
                Text = "マヨネー樹の花の色"
            });
            dataGroupRoot.Datas.Add(new DataImage()
            {
                X = 30,
                Y = 140,
                ImageSource = GetBitmapImage($"D:\\ブログ用\\テスト用画像\\collection1.png")
            });
            DataGroup dataGroup1 = new() { X = 20, Y = 30 };
            dataGroup1.Datas.Add(new DataTextBlock() { X = 20, Y = 30, Text = "東方不敗の理を顕す" });
            dataGroupRoot.Datas.Add(dataGroup1);

            //階層構造Dataのセーブロード確認
            SaveToZip(ZIP_FILE_PATH, dataGroupRoot);
            Data? 階層構造Data確認用 = LoadFromZip(ZIP_FILE_PATH);

        }


        private BitmapImage GetBitmapImage(string path)
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

        private void SaveToZip(string filePath, Data data)
        {
            try
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
                        //シリアライズする型は基底クラス型のDataで大丈夫
                        DataContractSerializer serializer = new(typeof(Data));
                        using var writer = XmlWriter.Create(entryStream, settings);
                        try { serializer.WriteObject(writer, data); }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                    //png形式にした画像をzipに詰め込む
                    AAA(archive);
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            void AAA(ZipArchive archive)
            {
                //if (data.Type == TType.Group)//こっちの方が速いかも
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


        private Data? LoadFromZip(string filePath)
        {
            try
            {
                using FileStream zipStream = File.OpenRead(filePath);
                using ZipArchive archive = new(zipStream, ZipArchiveMode.Read);
                ZipArchiveEntry? entry = archive.GetEntry(XML_FILE_NAME);
                if (entry != null)
                {
                    //デシリアライズ
                    using Stream entryStream = entry.Open();
                    DataContractSerializer serializer = new(typeof(Data));
                    using var reader = XmlReader.Create(entryStream);
                    Data? data = (Data?)serializer.ReadObject(reader);
                    if (data is null) return null;
                    //DataがDataImage型ならzipから画像を取り出して設定
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
                //Guidに一致する画像ファイルをデコードしてプロパティに設定
                ZipArchiveEntry? imageEntry = archive.GetEntry(data.Guid + ".png");
                if (imageEntry != null)
                {
                    using Stream imageStream = imageEntry.Open();
                    PngBitmapDecoder decoder =
                        new(imageStream,
                        BitmapCreateOptions.None,
                        BitmapCacheOption.Default);
                    data.ImageSource = decoder.Frames[0];//設定
                }
            }
        }

        //未使用
        #region シリアライズ、未使用

        ////それぞれのクラス型でシリアライズ
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
        #endregion シリアライズ、未使用



    }
}
