using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
//Pixtack3rdをコピペしてきたけど
//同じくグループ化のグループ化でUIがフリーズする

namespace _20230127_FreezTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppConfig MyAppConfig;
        //アプリ情報
        private const string APP_NAME = "Pixtack3rd";
        private const string APP_CONFIG_FILE_NAME = "config" + APP_EXTENSION_NAME;

        private const string APP_EXTENSION_NAME = ".p3rd";//Rootデータとアプリの設定を含んだ拡張子
        private const string DATA_EXTENSION_NAME = ".p3";//データだけの拡張子

        private const string EXTENSION_FILTER_P3 = "Pixtace3 設定＆全Data|*" + APP_EXTENSION_NAME;
        private const string EXTENSION_FILTER_P3D = "Pixtack3 アイテムData|*" + DATA_EXTENSION_NAME;
        private string AppVersion;
        //datetime.tostringの書式、これを既定値にする
        private const string DATE_TIME_STRING_FORMAT = "yyyyMMdd'_'HHmmss'_'fff";
        //データ保存時のxmlのファイル名
        private readonly string XML_FILE_NAME = "Data.xml";
        private const string APP_ROOT_DATA_FILENAME = "TTRoot" + DATA_EXTENSION_NAME;

        public MainWindow()
        {
            InitializeComponent();
            MyAppConfig = GetAppConfig(APP_CONFIG_FILE_NAME);
            DataContext = MyAppConfig;

            AppVersion = GetAppVersion();
            MyInitialize();


            MyInitializeComboBox();

            Drop += MainWindow_Drop;
            Closed += MainWindow_Closed;

            string imagePath = "D:\\ブログ用\\テスト用画像\\collection5.png";
            string imagePath1 = "D:\\ブログ用\\テスト用画像\\collection4.png";
            //string imagePath2 = "D:\\ブログ用\\テスト用画像\\hueRect000.png";
            //string imagePath3 = "D:\\ブログ用\\テスト用画像\\hueRect030.png";


            Data dataImg1 = new(TType.Image) { BitmapSource = GetBitmap(imagePath) };
            Data dataImg2 = new(TType.Image) { BitmapSource = GetBitmap(imagePath1), X = 100, Y = 100 };

            //TTGroup group = new(new Data(TType.Group) { X = 100, Y = 100 });
            //Data dataGroup = new(TType.Group);
            //dataGroup.Datas.Add(dataImg1);
            //dataGroup.Datas.Add(dataImg2);
            ////dataGroup.Datas.Add(new Data(TType.Image) { BitmapSource=GetBitmap(imagePath2), X = 120, Y = 120 });
            //MyRoot.AddThumbDataToActiveGroup(dataGroup);


            //MyRoot.AddItem(group, group.Data);
            ////    dataImg1.BitmapSource = new BitmapImage(new Uri("D:\\ブログ用\\テスト用画像\\collection4.png"));
            ////dataImg1.BitmapSource = GetBitmap("D:\\ブログ用\\テスト用画像\\collection4.png");

            ////MyImage.Data.BitmapSource = GetBitmap("D:\\ブログ用\\テスト用画像\\collection1.png");
        }

        private static BitmapImage GetBitmap(string filePath)
        {
            BitmapImage bmp = new();
            FileStream stream = new(filePath, FileMode.Open, FileAccess.Read);
            bmp.BeginInit();
            bmp.StreamSource = stream;
            bmp.EndInit();
            return bmp;
        }


        #region 初期設定
        /// <summary>
        /// 実行ファイルと同じフォルダにある設定ファイルをデシリアライズして返す、
        /// 見つからないときは新規作成して返す
        /// </summary>
        /// <fileName>拡張子を含めたファイル名</fileName>
        /// <returns></returns>
        private static AppConfig GetAppConfig(string fileName)
        {
            string configFile = System.IO.Path.Combine(
                Environment.CurrentDirectory, fileName);

            if (File.Exists(configFile)
                && LoadConfig(configFile) is AppConfig config)
            {
                return config;
            }
            else
            {
                return new AppConfig();
            }
        }
        private void MyInitialize()
        {
            MyAppConfig = FixAppWindowLocate(MyAppConfig);
            //タイトルをアプリの名前 + バージョン
            this.Title = APP_NAME + AppVersion;
            //前回終了時のData読み込み
            if (MyAppConfig.IsLoadPreviewData)
            {
                var (data, appConfig) = LoadDataFromFile(System.IO.Path.Combine(Environment.CurrentDirectory, APP_ROOT_DATA_FILENAME));
                if (data != null)
                {
                    MyRoot.SetRootData(data);
                }
            }

        }
        private void MyInitializeComboBox()
        {
            ComboBoxSaveFileType.ItemsSource = Enum.GetValues(typeof(ImageType));

            //List<double> vs = new() { 0, 1.5, 2.5, 3.5, 5 };
            //MyComboBoxFileNameDateOrder.ItemsSource = vs;
            //MyComboBoxFileNameSerialOrder.ItemsSource = vs;

            //MyComboBoxCaputureRect.ItemsSource = new Dictionary<CaptureRectType, string>
            //{
            //    { CaptureRectType.Screen, "画面全体" },
            //    { CaptureRectType.Window, "ウィンドウ" },
            //    { CaptureRectType.WindowClient, "ウィンドウのクライアント領域" },
            //    { CaptureRectType.UnderCursor, "カーソル下のコントロール" },
            //    { CaptureRectType.UnderCursorClient, "カーソル下コントロールのクライアント領域" },
            //    { CaptureRectType.WindowWithMenu, "ウィンドウ + メニューウィンドウ" },
            //    { CaptureRectType.WindowWithRelatedWindow, "ウィンドウ + 関連ウィンドウ" },
            //    { CaptureRectType.WindowWithRelatedWindowPlus, "ウィンドウ + より多くの関連ウィンドウ" },
            //};


            //MyComboBoxHotKey.ItemsSource = Enum.GetValues(typeof(Key));


            //MyComboBoxSoundType.ItemsSource = new Dictionary<MySoundPlay, string> {
            //    { MySoundPlay.None, "無音"},
            //    { MySoundPlay.PlayDefault, "既定の音" },
            //    { MySoundPlay.PlayOrder, "指定した音" }
            //};

            //MyComboBoxSaveBehavior.ItemsSource = new Dictionary<SaveBehaviorType, string> {
            //    { SaveBehaviorType.Save, "画像ファイルとして保存する" },
            //    { SaveBehaviorType.Copy, "クリップボードにコピーする (保存はしない)" },
            //    { SaveBehaviorType.SaveAndCopy, "保存 + コピー" },
            //    { SaveBehaviorType.SaveAtClipboardChange, "クリップボード監視、更新されたら保存" },
            //    { SaveBehaviorType.AddPreviewWindowFromClopboard, "クリップボード監視、更新されたらプレビューウィンドウに追加 (保存はしない)" }
            //};
        }

        /// <summary>
        /// アプリの設定のウィンドウ位置が画面外だった場合は0に変更して返す
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private AppConfig FixAppWindowLocate(AppConfig config)
        {
            if (config.Left < -10 ||
                config.Left > SystemParameters.VirtualScreenWidth - 100)
            {
                config.Left = 0;
            }
            if (config.Top < -10 ||
                config.Top > SystemParameters.VirtualScreenHeight - 100)
            {
                config.Top = 0;
            }

            return config;
        }

        #endregion 初期設定
        #region 設定保存と読み込み
        private bool SaveConfig<T>(string path, T obj)
        {
            try
            {
                XmlWriterSettings settings = new()
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                    NewLineOnAttributes = true,
                    ConformanceLevel = ConformanceLevel.Fragment,
                };
                using (XmlWriter writer = XmlWriter.Create(path, settings))
                {
                    DataContractSerializer serializer = new(typeof(T));
                    serializer.WriteObject(writer, obj);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存できなかった\n{ex.Message}");
                return false;
                //throw new ArgumentException("保存できなかった", ex.Message);                
            }
        }
        #endregion 設定保存と読み込み

        #region その他関数

        /// <summary>
        /// DataTypeの変換、RootDataだった場合GroupDataに変換する
        /// ディープコピーしてTypeだけGroupに書き換える、Datasもディープコピーする
        /// 変換部分が怪しい、項目が増えた場合はここも増やす必要があるのでバグ発生源になる？
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Data? ConvertDataRootToGroup(Data? data)
        {
            if (data == null) return null;
            if (data.Type == TType.Group) return data;
            if (data.Type != TType.Root) return null;

            if (data.DeepCopy() is Data groupData)
            {
                groupData.Type = TType.Group;
                return groupData;
            }
            return null;
        }


        /// <summary>
        /// アプリのバージョン取得、できなかかったときはstring.Emptyを返す
        /// </summary>
        /// <returns></returns>
        private static string GetAppVersion()
        {
            //実行ファイルのバージョン取得
            string[] cl = Environment.GetCommandLineArgs();

            //System.Diagnostics.FileVersionInfo
            if (FileVersionInfo.GetVersionInfo(cl[0]).FileVersion is string ver)
            {
                return ver;
            }
            else { return string.Empty; }

        }


        /// <summary>
        /// 設定ファイルをAppConfigにデシリアライズ
        /// </summary>
        /// <param name="filePath">設定ファイルのフルパス</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static AppConfig? LoadConfig(string filePath)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    DataContractSerializer serializer = new(typeof(AppConfig));
                    AppConfig? result = (AppConfig?)serializer.ReadObject(reader);
                    if (result == null)
                    {
                        MessageBox.Show("読込できんかった");
                        return null;
                    }
                    else { return result; }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "読込できんかった");
                return null;
            }
        }

        /// <summary>
        /// ファイル名に使える文字列ならtrueを返す
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckFileNameValidated(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            char[] invalid = System.IO.Path.GetInvalidFileNameChars();
            return name.IndexOfAny(invalid) < 0;
        }

        /// <summary>
        /// 保存ディレクトリ取得、未指定ならマイドキュメントにする。存在しない場合はstring.Emptyを返す
        /// </summary>
        /// <returns></returns>
        private string GetSaveDirectory(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            if (Directory.Exists(directory) == false)
            {
                MessageBox.Show($"指定されている保存場所は存在しないので保存できない", "注意");
                return string.Empty;
            }
            return directory;
        }

        /// <summary>
        /// クリップボードから画像を取得してActiveGroupに追加
        /// <paramref name="isPreferPNG">取得時に"PNG"形式を優先するときはtrue</paramref>
        /// <paramref name="isBgr32">ピクセルフォーマットをBgr32に変換(アルファ値を255に)するときはtrue</paramref>
        /// </summary>
        private void AddImageFromClipboard(bool isPreferPNG, bool isBgr32)
        {
            BitmapSource? bmp;
            if (isPreferPNG)
            {
                if (isBgr32)
                {
                    bmp = GetPngImageFromClipboard();
                }
                else
                {
                    bmp = GetPngImageFromClipboardWithAlphaFix();
                }
            }
            else
            {
                if (isBgr32)
                {
                    bmp = GetImageFromClipboardConvertBgr32();
                }
                else
                {
                    bmp = GetImageFromClipboardWithAlphaFix();
                }
            }
            if (bmp != null)
            {//追加
                MyRoot.AddThumbDataToActiveGroup(new Data(TType.Image) { BitmapSource = bmp });
            }
            else
            {
                MessageBox.Show("画像は得られなかった");
            }
        }
        /// <summary>
        /// クリップボードから画像を取得してActiveGroupに追加
        /// "PNG"形式優先で取得、できなければGetImageで取得
        /// </summary>
        private void AddImageFromClipboard()
        {
            if (GetImageFromClipboardPreferPNG() is BitmapSource bmp)
            {
                MyRoot.AddThumbDataToActiveGroup(new Data(TType.Image) { BitmapSource = bmp });
            }
            else { MessageBox.Show("画像は得られなかった"); }
        }

        #endregion その他関数

        #region 画像保存


        //private bool SaveBitmap(BitmapSource bitmap, string fullPath)
        //{
        //    bool isSavedDone = false;
        //    bool isSuccess = false;
        //    //ファイルに保存
        //    {
        //        bool result = SaveBitmapSub(bitmap, fullPath);

        //        isSavedDone = result;
        //        isSuccess = result;
        //    }

        //    //クリップボードにコピー、BMPとPNG形式の両方をセットする
        //    //BMPはアルファ値が255になる、PNGはアルファ値保持するけどそれが活かせるかは貼り付けるアプリに依る
        //    if (MyAppConfig.SaveBehaviorType is SaveBehaviorType.Copy or
        //        SaveBehaviorType.SaveAndCopy)
        //    {
        //        try
        //        {
        //            //BMP
        //            DataObject data = new();
        //            data.SetData(typeof(BitmapSource), bitmap);
        //            //PNG
        //            PngBitmapEncoder enc = new();
        //            enc.Frames.Add(BitmapFrame.Create(bitmap));
        //            using var ms = new System.IO.MemoryStream();
        //            enc.Save(ms);
        //            data.SetData("PNG", ms);

        //            Clipboard.SetDataObject(data, true);//true必須
        //            isSuccess = true;

        //            ////コピーだけのときは連番に加算
        //            //if (MyAppConfig.SaveBehaviorType == SaveBehaviorType.Copy &&
        //            //    MyAppConfig.IsFileNameSerial)
        //            //{
        //            //    AddIncrementToSerial();
        //            //}
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"クリップボードにコピーできなかった\n" +
        //                $"理由は不明、まれに起こる\n\n" +
        //                $"エラーメッセージ\n" +
        //                $"{ex.Message}", "エラー発生");
        //        }
        //    }


        //    //プレビューウィンドウに表示
        //    //DisplayPreviewWindow(bitmap, fullPath, isSavedDone);

        //    return isSuccess;
        //}


        ///// <summary>
        ///// プレビューウィンドウに表示
        ///// </summary>
        ///// <param name="bitmap"></param>
        ///// <param name="fullPath"></param>
        ///// <param name="isSavedDone">保存済みならtrue</param>
        //private bool DisplayPreviewWindow(BitmapSource bitmap, string fullPath, bool isSavedDone)
        //{
        //    if (MyPreviweWindow != null && bitmap != null)
        //    {
        //        MyPreviewItems.Add(new PreviewItem(
        //            System.IO.Path.GetFileNameWithoutExtension(fullPath), bitmap, fullPath, isSavedDone));
        //        ListBox lb = MyPreviweWindow.MyListBox;
        //        lb.SelectedIndex = MyPreviewItems.Count - 1;
        //        lb.ScrollIntoView(lb.SelectedItem);
        //        if (isSavedDone == false)
        //        {
        //            AddIncrementToSerial();
        //        }
        //        return true;
        //    }
        //    return false;
        //}

        /// <summary>
        /// 複数Rect範囲を組み合わせた形にbitmapを切り抜く
        /// </summary>
        /// <param name="source">元の画像</param>
        /// <param name="rectList">Rectのコレクション</param>
        /// <returns></returns>
        private BitmapSource CroppedBitmapFromRects(BitmapSource source, List<Rect> rectList)
        {
            List<Int32Rect> re = new();
            foreach (var item in rectList)
            {
                re.Add(new Int32Rect((int)item.X, (int)item.Y, (int)item.Width, (int)item.Height));
            }

            return CroppedBitmapFromRects(source, re);
        }
        private BitmapSource CroppedBitmapFromRects(BitmapSource source, List<Int32Rect> rectList)
        {
            var dv = new DrawingVisual();

            using (DrawingContext dc = dv.RenderOpen())
            {
                //それぞれのRect範囲で切り抜いた画像を描画していく
                foreach (var rect in rectList)
                {
                    dc.DrawImage(new CroppedBitmap(source, rect), new Rect(rect.X, rect.Y, rect.Width, rect.Height));
                }
            }

            //描画位置調整
            dv.Offset = new Vector(-dv.ContentBounds.X, -dv.ContentBounds.Y);

            //bitmap作成、縦横サイズは切り抜き後の画像全体がピッタリ収まるサイズにする
            //PixelFormatsはPbgra32で決め打ち、これ以外だとエラーになるかも、
            //画像を読み込んだbitmapImageのPixelFormats.Bgr32では、なぜかエラーになった
            var bmp = new RenderTargetBitmap(
                (int)Math.Ceiling(dv.ContentBounds.Width),
                (int)Math.Ceiling(dv.ContentBounds.Height),
                96, 96, PixelFormats.Pbgra32);

            bmp.Render(dv);
            //bmp.Freeze();
            return bmp;
        }

        //RectからInt32Rect作成、小数点以下切り捨て編
        //Rectの数値は整数のはずだから、これでいいはず
        private Int32Rect RectToIntRectWith切り捨て(Rect re)
        {
            return new Int32Rect((int)re.X, (int)re.Y, (int)re.Width, (int)re.Height);
        }

        ////画像にマウスカーソルを描画してからCropp
        //private BitmapSource MakeBitmapForSave(BitmapSource source, List<Rect> reList)
        //{
        //    BitmapSource bitmap;
        //    if (MyAppConfig.IsDrawCursor == true)
        //    {
        //        bitmap = DrawCursor(source);
        //    }
        //    else { bitmap = source; }

        //    return CroppedBitmapFromRects(bitmap, reList);
        //}
        //private BitmapSource MakeBitmapForSave(BitmapSource source, List<Int32Rect> reList)
        //{
        //    List<Rect> re = new();
        //    try
        //    {
        //        foreach (var itemData in reList)
        //        {
        //            re.Add(new Rect(itemData.X, itemData.Y, itemData.Width, itemData.Height));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"{ex}");
        //    }
        //    return MakeBitmapForSave(source, re);
        //}

        //private BitmapSource DrawCursor(BitmapSource source)
        //{
        //    BitmapSource bitmap;
        //    if (IsMaskUse)
        //    {
        //        bitmap = DrawCursorOnBitmapWithMask(source);
        //    }
        //    else
        //    {
        //        bitmap = DrawCursorOnBitmap(source);
        //    }
        //    return bitmap;
        //}

        //internal bool SaveBitmapSub(BitmapSource bitmap, string fullPath)
        //{
        //    //CroppedBitmapで切り抜いた画像でBitmapFrame作成して保存
        //    BitmapEncoder encoder = GetEncoder();
        //    //メタデータ作成、アプリ名記入
        //    BitmapMetadata meta = MakeMetadata();
        //    if (MakeMetadata() is BitmapMetadata meta)
        //    {
        //        encoder.Frames.Add(BitmapFrame.Create(bitmap, null, meta, null));
        //        //重複回避ファイルパス取得
        //        fullPath = MakeFilePathAvoidDuplicate(fullPath);
        //        try
        //        {
        //            using FileStream fs = new(fullPath, FileMode.Create, FileAccess.Write);
        //            encoder.Save(fs);
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"保存できなかった\n{ex}", "保存できなかった");
        //            return false;
        //        }
        //    }
        //    else { return false; }
        //}
        internal bool SaveBitmapSub(BitmapSource bitmap, string fullPath, BitmapEncoder encoder)
        {
            if (MakeMetadata() is BitmapMetadata meta)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmap, null, meta, null));
                //重複回避ファイルパス取得
                //fullPath = MakeFilePathAvoidDuplicate(fullPath);
                try
                {
                    using FileStream fs = new(fullPath, FileMode.Create, FileAccess.Write);
                    encoder.Save(fs);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存できなかった\n{ex}", "保存できなかった");
                    return false;
                }
            }
            else { return false; }
        }


        ///// <summary>
        ///// 保存ファイルのフルパスを取得、無効なパスの場合はstring.Emptyを返す
        ///// ファイル名の重複を回避、拡張子の前に"_"を付け足す
        ///// </summary>
        ///// <returns></returns>
        //private string GetSaveFileFullPath()
        //{
        //    //ファイル名取得、無効なファイル名なら中止
        //    string fileName = MakeFileName();
        //    if (CheckFileNameValidated(fileName) == false)
        //    {
        //        return string.Empty;
        //    }

        //    //保存ディレクトリ取得、存在しない場合は中止
        //    string directory = GetSaveDirectory("E:\\Pixtack3rdTest");
        //    if (directory == string.Empty)
        //    {
        //        return string.Empty;
        //    }

        //    string dir = System.IO.Path.Combine(directory, fileName);
        //    string extension = "." + MyAppConfig.ImageType.ToString();
        //    string fullPaht = dir + extension;
        //    //重複回避パス取得
        //    return MakeFilePathAvoidDuplicate(fullPaht);
        //}

        /// <summary>
        /// 重複回避ファイルパス作成、重複しなくなるまでファイル名末尾に_を追加して返す
        /// </summary>
        /// <returns></returns>
        private string MakeFilePathAvoidDuplicate(string fullPath)
        {
            string extension = System.IO.Path.GetExtension(fullPath);
            string name = System.IO.Path.GetFileNameWithoutExtension(fullPath);
            string? directory = System.IO.Path.GetDirectoryName(fullPath);
            if (directory != null)
            {
                while (File.Exists(fullPath))
                {
                    name += "_";
                    fullPath = System.IO.Path.Combine(directory, name) + extension;
                }
                return fullPath;
            }
            else return string.Empty;
        }

        //メタデータ作成
        private BitmapMetadata? MakeMetadata()
        {
            BitmapMetadata? data = null;
            string software = APP_NAME + "_" + AppVersion;
            switch (ComboBoxSaveFileType.SelectedValue)
            {
                case ImageType.png:
                    data = new BitmapMetadata("png");
                    data.SetQuery("/tEXt/Software", software);
                    break;
                case ImageType.jpg:
                    data = new BitmapMetadata("jpg");
                    data.SetQuery("/app1/ifd/{ushort=305}", software);
                    break;
                case ImageType.bmp:

                    break;
                case ImageType.gif:
                    data = new BitmapMetadata("Gif");
                    //data.SetQuery("/xmp/xmp:CreatorTool", "Pixtrim2");
                    //data.SetQuery("/XMP/XMP:CreatorTool", "Pixtrim2");
                    data.SetQuery("/XMP/XMP:CreatorTool", software);
                    break;
                case ImageType.tiff:
                    data = new BitmapMetadata("tiff")
                    {
                        ApplicationName = software
                    };
                    break;
                default:
                    break;
            }

            return data;
        }

        //画像ファイル形式によるEncoder取得
        private BitmapEncoder GetEncoder(int filterIndex)
        {
            //var type = MyAppConfig.ImageType;

            switch (filterIndex)
            {
                case 1:
                    return new PngBitmapEncoder();
                case 2:
                    var jpeg = new JpegBitmapEncoder
                    {
                        QualityLevel = MyAppConfig.JpegQuality
                    };
                    return jpeg;
                case 3:
                    return new BmpBitmapEncoder();
                case 4:
                    return new GifBitmapEncoder();
                case 5:
                    return new TiffBitmapEncoder();
                default:
                    throw new Exception();
            }

        }

        //今の日時をStringで作成
        private string MakeStringNowTime()
        {
            DateTime dt = DateTime.Now;
            //string str = dt.ToString("yyyyMMdd");            
            //string str = dt.ToString("yyyyMMdd" + "_" + "HHmmssfff");
            string str = dt.ToString(DATE_TIME_STRING_FORMAT);
            //string str = dt.ToString("yyyyMMdd" + "_" + "HH" + "_" + "mm" + "_" + "ss" + "_" + "fff");
            return str;
        }


        //private string MakeFileName()
        //{
        //    double count = 0.0;
        //    string fileName = "";
        //    DateTime dateTime = DateTime.Now;
        //    bool isOverDate = false, isOverSerial = false;
        //    if (MyAppConfig.IsFileNameDate == false && MyAppConfig.IsFileNameSerial == false)
        //    {
        //        MyCheckBoxFileNameData.IsChecked = true;
        //    }
        //    if (MyAppConfig.IsFileNameDate == false) isOverDate = true;
        //    if (MyAppConfig.IsFileNameSerial == false) isOverSerial = true;
        //    MyOrder();

        //    if (MyAppConfig.IsFileNameText1) MyAddText(MyComboBoxFileNameText1);
        //    count += 1.5; MyOrder();

        //    if (MyAppConfig.IsFileNameText2) MyAddText(MyComboBoxFileNameText2);
        //    count++; MyOrder();

        //    if (MyAppConfig.IsFileNameText3) MyAddText(MyComboBoxFileNameText3);
        //    count++; MyOrder();

        //    if (MyAppConfig.IsFileNameText4) MyAddText(MyComboBoxFileNameText4);
        //    count += 1.5; MyOrder();

        //    if (string.IsNullOrWhiteSpace(fileName)) fileName = MakeStringNowTime();
        //    fileName = fileName.TrimStart();
        //    fileName = fileName.TrimEnd();
        //    return fileName;


        //    void MyOrder()
        //    {
        //        //日時
        //        if (isOverDate == false && MyAppConfig.FileNameDateOrder == count)
        //        {
        //            var format = MyComboBoxFileNameDateFormat.Text;
        //            if (string.IsNullOrEmpty(format))
        //            {
        //                fileName += MakeStringNowTime();
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    fileName += dateTime.ToString(MyComboBoxFileNameDateFormat.Text);
        //                    isOverDate = true;
        //                }
        //                catch (Exception)
        //                {

        //                }

        //            }
        //        }

        //        //連番
        //        if (isOverSerial == false && MyAppConfig.FileNameSerialOrder == count)
        //        {
        //            //fileName += MyNumericUpDownFileNameSerial.MyValue.ToString(MySerialFormat());
        //            fileName += MyAppConfig.FileNameSerial.ToString(MySerialFormat());

        //            isOverSerial = true;
        //        }
        //    }

        //    string MyAddText(ComboBox comboBox)
        //    {
        //        return fileName += comboBox.Text;
        //    }
        //    string MySerialFormat()
        //    {
        //        string str = "";
        //        for (int i = 0; i < MyAppConfig.FileNameSerialDigit; i++)
        //        {
        //            str += "0";
        //        }
        //        return str;
        //    }

        //}

        //連番に増加値を加算
        //private void AddIncrementToSerial()
        //{
        //    MyNumericUpDownFileNameSerial.MyValue += MyNumericUpDownFileNameSerialIncreace.MyValue;
        //}

        private bool SaveBitmapFromThumb(TThumb? thumb)
        {
            if (thumb == null) return false;
            if (MyRoot.GetBitmap(thumb) is BitmapSource bitmap)
            {

                Microsoft.Win32.SaveFileDialog dialog = new()
                {
                    Filter = "*.png|*.png|*.jpg|*.jpg;*.jpeg|*.bmp|*.bmp|*.gif|*.gif|*.tiff|*.tiff",
                    AddExtension = true,
                };
                if (dialog.ShowDialog() == true)
                {
                    BitmapEncoder encoder = GetEncoder(dialog.FilterIndex);
                    if (SaveBitmapSub(bitmap, dialog.FileName, encoder))
                    {
                        return true;
                    }
                    else return false;
                }
            }
            return false;
        }

        #endregion 画像保存

        #region ファイルドロップで開く

        private void MainWindow_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //ファイル名一覧取得
                var fileList2 = ((string[])e.Data.GetData(DataFormats.FileDrop)).ToArray();
                Array.Sort(fileList2);//昇順ソート
                List<string> errorFiles = new();

                foreach (var item in fileList2)
                {
                    //拡張子で判定、関連ファイルならDataで追加
                    var ext = System.IO.Path.GetExtension(item);
                    if (ext == DATA_EXTENSION_NAME || ext == APP_EXTENSION_NAME)
                    {
                        var (data, appConfig) = LoadDataFromFile(item);
                        if (data == null)
                        {
                            errorFiles.Add(item); continue;
                        }
                        //DataがRootならGroupに変換して追加
                        if (data.Type == TType.Root)
                        {
                            //2
                            //MyRoot.SetRootData(data);

                            //3
                            //MyRoot.AddThumbDataToActiveGroup(data);

                            //1
                            data = ConvertDataRootToGroup(data);
                            if (data != null)
                            {
                                MyRoot.AddThumbDataToActiveGroup(data);
                            }
                            else { errorFiles.Add(item); continue; }
                        }
                        else
                        {
                            MyRoot.AddThumbDataToActiveGroup(data);
                        }


                    }
                    //それ以外の拡張子ファイルは画像として読み込む
                    else
                    {
                        //試みてエラーだったらファイル名を表示
                        try
                        {
                            FileStream stream = new(item, FileMode.Open, FileAccess.Read);
                            BitmapImage img = new();
                            img.BeginInit();
                            img.StreamSource = stream;
                            img.EndInit();
                            Data data = new(TType.Image)
                            {
                                BitmapSource = img
                            };
                            MyRoot.AddThumbDataToActiveGroup(data);
                        }
                        catch (Exception)
                        {
                            errorFiles.Add(item);
                            continue;
                        }
                    }
                }
                if (errorFiles.Count > 0)
                {
                    string ms = "";
                    foreach (var name in errorFiles)
                    {
                        ms += $"{name}\n";
                    }
                    MessageBox.Show(ms, "開くことができなかったファイル一覧", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion ファイルドロップで開く


        #region データ保存、アプリの設定保存

        /// <summary>
        /// データをzipで保存
        /// </summary>
        /// <param name="filePath">拡張子も含めたフルパス</param>
        /// <param name="data"></param>
        /// <param name="isWithAppConfigSave">アプリの設定も保存するときはtrue</param>
        private void SaveDataToAz3(string filePath, Data data, bool isWithAppConfigSave)
        {
            try
            {
                using FileStream zipStream = File.Create(filePath);
                using ZipArchive archive = new(zipStream, ZipArchiveMode.Create);
                XmlWriterSettings settings = new()
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                    NewLineOnAttributes = true,
                    ConformanceLevel = ConformanceLevel.Fragment,
                };
                //シリアライズする型は基底クラス型のDataで大丈夫
                DataContractSerializer serializer = new(typeof(Data));
                //xml形式にシリアライズして、それをzipに詰め込む
                ZipArchiveEntry entry = archive.CreateEntry(XML_FILE_NAME);
                using (Stream entryStream = entry.Open())
                {
                    using XmlWriter writer = XmlWriter.Create(entryStream, settings);
                    try { serializer.WriteObject(writer, data); }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                //アプリの設定保存
                if (isWithAppConfigSave)
                {
                    entry = archive.CreateEntry(APP_CONFIG_FILE_NAME);
                    serializer = new(typeof(AppConfig));
                    using (Stream entryStream = entry.Open())
                    {
                        using XmlWriter writer = XmlWriter.Create(entryStream, settings);
                        try { serializer.WriteObject(writer, MyAppConfig); }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                }

                //BitmapSourceの保存
                SubLoop(archive, data);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            void SubLoop(ZipArchive archive, Data subData)
            {
                if (data.BitmapSource != null)
                {
                    Sub(data, archive);
                }
                //子要素のBitmapSource保存
                foreach (Data item in subData.Datas)
                {
                    Sub(item, archive);
                    if (item.Type == TType.Group) { SubLoop(archive, item); }
                }
            }

            void Sub(Data itemData, ZipArchive archive)
            {
                //画像があった場合はpng形式にしてzipに詰め込む
                if (itemData.BitmapSource is BitmapSource bmp)
                {
                    ZipArchiveEntry entry = archive.CreateEntry(itemData.Guid + ".png");
                    using Stream entryStream = entry.Open();
                    PngBitmapEncoder encoder = new();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    using MemoryStream memStream = new();
                    encoder.Save(memStream);
                    memStream.Position = 0;
                    memStream.CopyTo(entryStream);
                }
            }
        }







        #endregion データ保存、アプリの設定保存

        #region データ読み込み、アプリの設定読み込み

        /// <summary>
        /// ファイルからData読み込み
        /// </summary>
        /// <param name="filePath">フルパス</param>
        /// <returns></returns>
        private (Data? data, AppConfig? appConfig) LoadDataFromFile(string filePath)
        {
            try
            {
                using FileStream zipStream = File.OpenRead(filePath);
                using ZipArchive archive = new(zipStream, ZipArchiveMode.Read);
                ZipArchiveEntry? entry = archive.GetEntry(XML_FILE_NAME);
                if (entry != null)
                {
                    //Dataをデシリアライズ
                    using Stream entryStream = entry.Open();
                    DataContractSerializer serializer = new(typeof(Data));
                    using var reader = XmlReader.Create(entryStream);
                    Data? data = (Data?)serializer.ReadObject(reader);
                    if (data is null) return (null, null);

                    SubSetImageSource(data, archive);

                    //画像をデコードしてDataに設定する
                    SubLoop(data, archive);


                    //アプリの設定をデシリアライズ
                    entry = archive.GetEntry(APP_CONFIG_FILE_NAME);
                    if (entry != null)
                    {
                        using Stream entryAppConfig = entry.Open();
                        serializer = new(typeof(AppConfig));
                        using (var appConfigReader = XmlReader.Create(entryAppConfig))
                        {
                            AppConfig? appConfig = (AppConfig?)serializer.ReadObject(appConfigReader);
                            return (data, appConfig);
                        }
                    }
                    else
                    {
                        return (data, null);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return (null, null);

            void SubLoop(Data data, ZipArchive archive)
            {
                foreach (var item in data.Datas)
                {
                    //DataのTypeがImage型ならzipから画像を取り出して設定
                    if (item.Type == TType.Image) { SubSetImageSource(item, archive); }
                    //DataのTypeがGroupなら子要素も取り出す
                    else if (item.Type == TType.Group) { SubLoop(item, archive); }
                }
            }
            void SubSetImageSource(Data data, ZipArchive archive)
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
                    data.BitmapSource = decoder.Frames[0];//設定
                }
            }
        }

        //dataファイルの読み込み
        //TTRootのDataとアプリの設定を取得して設定
        private void ButtonLoadData_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new();
            dialog.Filter = EXTENSION_FILTER_P3;
            if (dialog.ShowDialog() == true)
            {
                (Data? data, AppConfig? appConfig) = LoadDataFromFile(dialog.FileName);
                if (data != null)
                {
                    MyRoot.SetRootData(data);
                    if (appConfig != null)
                    {
                        MyAppConfig = appConfig;
                        DataContext = MyAppConfig;
                    }
                }
            }
        }

        //RootDataであるaz3ファイルを読み込んで、TTGroupに変換して追加
        //変換部分が怪しい、項目が増えた場合はここも増やす必要があるのでバグ発生源になる？
        private void ButtonLoadDataRootToGroup_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new();
            dialog.Filter = EXTENSION_FILTER_P3;
            if (dialog.ShowDialog() == true)
            {
                (Data? data, AppConfig? appConfig) = LoadDataFromFile(dialog.FileName);
                if (ConvertDataRootToGroup(data) is Data groupData)
                {
                    MyRoot.AddThumbDataToActiveGroup(groupData);
                }
            }
        }

        //個別Data読み込み
        private void ButtonLoadDataThumb_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new();
            dialog.Filter = EXTENSION_FILTER_P3D;
            if (dialog.ShowDialog() == true)
            {
                (Data? data, AppConfig? appConfig) = LoadDataFromFile(dialog.FileName);
                data = ConvertDataRootToGroup(data);
                if (data is not null)
                {
                    MyRoot.AddThumbDataToActiveGroup(data);
                }
            }
        }

        #endregion データ読み込み、アプリの設定読み込み



        #region クリップボード監視、画像取得、画像保存
        //       クリップボードの中にある画像をWPFで取得してみた、Clipboard.GetImage() だけだと透明になる - 午後わてんのブログ
        //https://gogowaten.hatenablog.com/entry/2019/11/12/201852

        //        アルファ値を失わずに画像のコピペできた、.NET WPFのClipboard - 午後わてんのブログ
        //https://gogowaten.hatenablog.com/entry/2021/02/10/134406


        //四角形の場合は"PNG"で取得してBgr32に変換
        //テキストボックスはGetImage()で取得してBgr32に変換

        /// <summary>
        /// クリップボードから画像を取得する、なかった場合はnullを返す
        /// </summary>
        /// <returns>BitmapSource</returns>
        private BitmapSource? GetImageFromClipboard()
        {

            BitmapSource? source = null;
            int count = 1;
            int limit = 5;
            do
            {
                try { source = Clipboard.GetImage(); }
                catch (Exception) { }
                finally { count++; }
            } while (limit >= count && source == null);

            if (source == null) { return null; }

            //エクセル系のデータだった場合はGetImageで取得、このままだとアルファ値が0になっているので
            //Bgr32に変換することでファルファ値を255にする
            if (IsExcelCell())
            {
                source = new FormatConvertedBitmap(source, PixelFormats.Bgr32, null, 0);
            }
            //エクセル系以外はPNG形式で取得を試みて、得られなければGetImageで取得
            else
            {
                BitmapSource? png = GetPngImageFromCripboard();
                if (png != null)
                {
                    source = png;
                }
            }

            if (source == null) { return null; }

            //アルファ値が異常な画像ならピクセルフォーマットをBgr32に変換(アルファ値を255にする)
            if (IsExceptionTransparent(source))
            //if (IsExceptionTransparent(source))
            {
                source = new FormatConvertedBitmap(source, PixelFormats.Bgr32, null, 0);
            }

            return source;
        }

        /// <summary>
        /// クリップボードから画像取得、
        /// アルファ値をチェックして異常だった場合は修正する
        /// </summary>
        /// <returns></returns>
        private BitmapSource? GetImageFromClipboardWithAlphaFix()
        {
            BitmapSource? source = null;
            int count = 1;
            int limit = 5;
            do
            {
                try { source = Clipboard.GetImage(); }
                catch (Exception) { }
                finally { count++; }
            } while (limit >= count && source == null);

            if (source == null) { return null; }

            //アルファ値が異常な画像ならピクセルフォーマットをBgr32に変換(アルファ値を255にする)
            if (IsExceptionTransparent(source))
            //if (IsExceptionTransparent(source))
            {
                source = new FormatConvertedBitmap(source, PixelFormats.Bgr32, null, 0);
            }
            return source;
        }
        /// <summary>
        /// "PNG"形式優先でクリップボードから画像取得
        /// </summary>
        /// <returns></returns>
        private BitmapSource? GetImageFromClipboardPreferPNG()
        {
            //"PNG"形式で取得できたら返す
            if (GetPngImageFromClipboardWithAlphaFix() is BitmapSource png)
            {
                if (IsExceptionTransparent(png) == false)
                {
                    return png;
                }
            }

            //取得できなかった場合はGetImageで取得
            int count = 1;
            int limit = 5;
            BitmapSource? source = null;
            do
            {
                try { source = Clipboard.GetImage(); }
                catch (Exception) { }
                finally { count++; }
            } while (limit >= count && source == null);

            if (source == null) { return null; }

            //アルファ値が異常な画像ならピクセルフォーマットをBgr32に変換(アルファ値を255にする)
            if (IsExceptionTransparent(source))
            //if (IsExceptionTransparent(source))
            {
                source = new FormatConvertedBitmap(source, PixelFormats.Bgr32, null, 0);
            }
            return source;
        }

        private BitmapSource? GetImageFromClipboardConvertBgr32()
        {
            BitmapSource? source = null;
            int count = 1;
            int limit = 5;
            do
            {
                try { source = Clipboard.GetImage(); }
                catch (Exception) { }
                finally { count++; }
            } while (limit >= count && source == null);

            if (source == null) { return null; }

            //ピクセルフォーマットをBgr32に変換(アルファ値を255にする)
            source = new FormatConvertedBitmap(source, PixelFormats.Bgr32, null, 0);

            return source;
        }

        /// <summary>
        /// クリップボードから"PNG"形式で画像取得、
        /// アルファ値をチェックして異常だった場合は修正する
        /// </summary>
        /// <returns></returns>
        private BitmapSource? GetPngImageFromClipboardWithAlphaFix()
        {
            if (GetPngImageFromCripboard() is BitmapSource source)
            {
                if (IsExceptionTransparent(source))
                {
                    //アルファ値が異常な画像ならピクセルフォーマットをBgr32に変換(アルファ値を255にする)
                    return source = new FormatConvertedBitmap(source, PixelFormats.Bgr32, null, 0);
                }
                else { return source; }
            }
            else { return null; }
        }
        private BitmapSource? GetPngImageFromClipboard()
        {
            if (GetPngImageFromCripboard() is BitmapSource source)
            {
                //ピクセルフォーマットをBgr32に変換(アルファ値を255にする)
                return new FormatConvertedBitmap(source, PixelFormats.Bgr32, null, 0);
            }
            else { return null; }
        }


        /// <summary>
        /// BitmapSourceの全ピクセルのアルファ値を検査、一つでも1以上があれば正常なのでfalseを返す
        /// すべて0だった場合はtrueを返す、Bgra32専用
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private bool IsExceptionTransparent(BitmapSource source)
        {
            if (source.Format != PixelFormats.Bgra32) return false;
            int stride = source.PixelWidth * 4;
            byte[] pixels = new byte[stride * source.PixelHeight];
            source.CopyPixels(pixels, stride, 0);
            for (int i = 3; i < pixels.Length; i += 4)
            {
                if (pixels[i] > 0) return false;
            }
            return true;
        }


        /// <summary>
        /// クリップボードのPNG形式の画像を取得する、ない場合はnullを返す
        /// </summary>
        /// <returns></returns>
        private static BitmapFrame? GetPngImageFromCripboard()
        {
            try
            {
                using MemoryStream stream = (MemoryStream)Clipboard.GetData("PNG");
                //source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// クリップボードのデータ判定、エクセル判定、
        /// データの中にEnhancedMetafile形式があればエクセルと判定してtrueを返す
        /// </summary>
        /// <returns></returns>
        private static bool IsExcelCell()
        {

            IDataObject? obj = null;
            int count = 1;
            int limit = 5;
            do
            {
                try { obj = Clipboard.GetDataObject(); }
                catch (Exception) { }
                finally
                {
                    count++;
                    Task.Delay(10);
                }
            } while (obj == null && limit >= count);

            if (obj == null) { return false; }

            string[] formats = obj.GetFormats();
            foreach (var item in formats)
            {
                if (item == "EnhancedMetafile")
                {
                    return true;
                }
            }
            return false;
        }


        //アルファ値を失わずに画像のコピペできた、.NET WPFのClipboard - 午後わてんのブログ
        //        https://gogowaten.hatenablog.com/entry/2021/02/10/134406

        /// <summary>
        /// BitmapSourceをPNG形式に変換したものと、そのままの形式の両方をクリップボードにコピーする
        /// </summary>
        /// <param name="source"></param>
        private static void ClipboardSetBitmapWithPng(BitmapSource source)
        {
            //DataObjectに入れたいデータを入れて、それをクリップボードにセットする
            DataObject data = new();

            //BitmapSource形式そのままでセット
            data.SetData(typeof(BitmapSource), source);

            //PNG形式にエンコードしたものをMemoryStreamして、それをセット
            //画像をPNGにエンコード
            PngBitmapEncoder pngEnc = new();
            pngEnc.Frames.Add(BitmapFrame.Create(source));
            //エンコードした画像をMemoryStreamにSava
            using var ms = new System.IO.MemoryStream();
            pngEnc.Save(ms);
            data.SetData("PNG", ms);

            //クリップボードにセット
            Clipboard.SetDataObject(data, true);
        }


        #endregion クリップボード監視、画像取得



        #region ボタンクリックイベント

        #region 保存系

        //Rootを画像ファイルとして保存
        private void ButtonSaveToImage_Click(object sender, RoutedEventArgs e)
        {
            if (SaveBitmapFromThumb(MyRoot) == false)
            {
                MessageBox.Show("保存できなかった");
            }
        }
        private void ButtonSaveToImageActive_Click(object sender, RoutedEventArgs e)
        {//ActiveThumb            
            if (SaveBitmapFromThumb(MyRoot.ActiveThumb) == false)
            {
                MessageBox.Show("保存できなかった");
            }
        }

        private void ButtonSaveToImageClicked_Click(object sender, RoutedEventArgs e)
        {//ClickedThumb
            if (SaveBitmapFromThumb(MyRoot.ClickedThumb) == false)
            {
                MessageBox.Show("保存できなかった");
            }
        }
        //TTRootのDataとアプリの設定を保存
        private void ButtonSaveData_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dialog = new();
            dialog.Filter = EXTENSION_FILTER_P3;
            if (dialog.ShowDialog() == true)
            {
                SaveDataToAz3(dialog.FileName, MyRoot.Data, true);
            }
        }
        //個別保存、ActiveThumbのDataを保存
        //アプリ終了時
        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            //設定保存
            SaveConfig(System.IO.Path.Combine(
                Environment.CurrentDirectory, APP_CONFIG_FILE_NAME), MyAppConfig);
            //RootData保存
            SaveDataToAz3(System.IO.Path.Combine(
                Environment.CurrentDirectory, APP_ROOT_DATA_FILENAME), MyRoot.Data, true);
        }

        private void ButtonSaveDataThumb_Click(object sender, RoutedEventArgs e)
        {
            //ActiveThumbのDataを保存
            if (MyRoot.ActiveThumb?.Data is Data data)
            {
                Microsoft.Win32.SaveFileDialog dialog = new();
                dialog.Filter = EXTENSION_FILTER_P3D;
                if (dialog.ShowDialog() == true)
                {
                    SaveDataToAz3(dialog.FileName, data, false);
                }
            }
        }

        private string? GetSaveDataFilePath(string extFilter)
        {
            Microsoft.Win32.SaveFileDialog dialog = new();
            dialog.Filter = extFilter;
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            else
            {
                return null;
            }
        }
        private void ButtonSaveAllData_Click(object sender, RoutedEventArgs e)
        {
            if (GetSaveDataFilePath(EXTENSION_FILTER_P3) is string path)
            {
                SaveDataToAz3(path, MyRoot.Data, true);
            }
        }

        private void ButtonSaveRootThumb_Click(object sender, RoutedEventArgs e)
        {
            if (MyRoot.Thumbs.Count == 0) { return; }
            if (GetSaveDataFilePath(EXTENSION_FILTER_P3D) is string path)
            {
                SaveDataToAz3(path, MyRoot.Data, false);
            }
        }

        private void ButtonSaveCickedThumb_Click(object sender, RoutedEventArgs e)
        {
            if (MyRoot.ClickedThumb?.Data == null) { return; }
            if (GetSaveDataFilePath(EXTENSION_FILTER_P3D) is string path)
            {
                SaveDataToAz3(path, MyRoot.ClickedThumb.Data, false);
            }
        }

        private void ButtonSaveActiveThumb_Click(object sender, RoutedEventArgs e)
        {
            if (MyRoot.ActiveThumb?.Data == null) { return; }
            if (GetSaveDataFilePath(EXTENSION_FILTER_P3D) is string path)
            {
                SaveDataToAz3(path, MyRoot.ActiveThumb.Data, false);
            }
        }
        #endregion 保存系

        private void ButtonToGroup_Click(object sender, RoutedEventArgs e)
        {
            //グループ化
            MyRoot.AddGroup();
        }

        private void ButtonUnGroup_Click(object sender, RoutedEventArgs e)
        {
            //グループ解除、ActiveThumbが対象
            MyRoot.UnGroup();
        }

        private void ButtonIn_Click(object sender, RoutedEventArgs e)
        {
            //ActiveGroupの外へ
            MyRoot.ActiveGroupInside();
        }

        private void ButtonOut_Click(object sender, RoutedEventArgs e)
        {
            //ActiveGroupの中へ
            MyRoot.ActiveGroupOutside();
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            //選択Thumbを削除
            MyRoot.RemoveThumb();
        }
        private void ButtonRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            //全削除
            MyRoot.RemoveAll();
        }
        private void ButtonAddFromClipboard_Click(object sender, RoutedEventArgs e)
        {//クリップボードから画像追加、"PNG"形式優先で取得
            AddImageFromClipboard();
        }

        private void ButtonAddFromClipboardPNG_Click(object sender, RoutedEventArgs e)
        {//クリップボードから画像追加、"PNG"形式で取得
            AddImageFromClipboard(true, false);
        }
        private void ButtonAddFromClipboardBgr32_Click(object sender, RoutedEventArgs e)
        {//クリップボードから画像追加、"PNG"形式で取得＋強制Bgr32
            AddImageFromClipboard(false, true); ;
        }

        //private void ButtonAddFromClipboardPNGBgr32_Click(object sender, RoutedEventArgs e)
        //{
        //    AddImageFromClipboard(true, true);
        //}


        private void ButtonCopyImage_Click(object sender, RoutedEventArgs e)
        {//画像として全体をコピー、クリップボードにセット
            if (MyRoot.GetBitmap(MyRoot) is BitmapSource bmp)
            {
                ClipboardSetBitmapWithPng(bmp);
            }
        }

        private void ButtonCopyImageActiveThumb_Click(object sender, RoutedEventArgs e)
        {
            //画像としてActiveThumbをコピー、クリップボードにセット
            if (MyRoot.ActiveThumb is not null)
            {
                if (MyRoot.GetBitmap(MyRoot.ActiveThumb) is BitmapSource bmp)
                {
                    ClipboardSetBitmapWithPng(bmp);
                }
            }
        }

        private void ButtonCopyImageClicedThumb_Click(object sender, RoutedEventArgs e)
        {
            //画像としてClickedThumbをコピー、クリップボードにセット
            if (MyRoot.ClickedThumb is not null)
            {
                if (MyRoot.GetBitmap(MyRoot.ClickedThumb) is BitmapSource bmp)
                {
                    ClipboardSetBitmapWithPng(bmp);
                }
            }
        }
        private void ButtonDuplicateImage_Click(object sender, RoutedEventArgs e)
        {
            //画像として複製、全体
            if (MyRoot.GetBitmap(MyRoot) is BitmapSource bmp)
            {
                MyRoot.AddThumbDataToActiveGroup(new Data(TType.Image) { BitmapSource = bmp });
            }
        }


        private void ButtpmDuplicateImageActiveThumb_Click(object sender, RoutedEventArgs e)
        {
            //画像として複製、ActiveThumb
            if (MyRoot.ActiveThumb is TThumb thumb && MyRoot.GetBitmap(thumb) is BitmapSource bmp)
            {
                MyRoot.AddThumbDataToActiveGroup(new Data(TType.Image) { BitmapSource = bmp });
            }
        }
        private void ButtonDuplicateImageClickedThumb_Click(object sender, RoutedEventArgs e)
        {
            //画像として複製、ClickedThumb
            if (MyRoot.ClickedThumb is TThumb thumb && MyRoot.GetBitmap(thumb) is BitmapSource bmp)
            {
                MyRoot.AddThumbDataToActiveGroup(new Data(TType.Image) { BitmapSource = bmp });
            }
        }
        private void ButtonDuplicateData_Click(object sender, RoutedEventArgs e)
        {
            //Dataとして複製、全体
            if (ConvertDataRootToGroup(MyRoot.Data) is Data data)
            {
                MyRoot.AddThumbDataToActiveGroup(data);
            }
        }

        private void ButtpmDuplicateDataActiveThumb_Click(object sender, RoutedEventArgs e)
        {
            if (MyRoot.ActiveThumb?.Data.DeepCopy() is Data data)
            {
                MyRoot.AddThumbDataToActiveGroup(data);
            }
        }

        private void ButtonDuplicateDataClickedThumb_Click(object sender, RoutedEventArgs e)
        {
            if (MyRoot.ClickedThumb?.Data.DeepCopy() is Data data)
            {
                MyRoot.AddThumbDataToActiveGroup(data);
            }
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            //前面へ移動
            MyRoot.ZUp();
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            //背面へ移動
            MyRoot.ZDown();
        }

        private void ButtonMostUp_Click(object sender, RoutedEventArgs e)
        {
            //最前面へ
            MyRoot.ZUpFrontMost();
        }

        private void ButtonMostDown_Click(object sender, RoutedEventArgs e)
        {
            //最背面へ移動
            MyRoot.ZDownBackMost();
        }




        #endregion ボタンクリックイベント


    }


    /// <summary>
    /// アプリの設定値用クラス
    /// </summary>
    [DataContract]
    public class AppConfig : INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler? PropertyChanged;
        //private void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        //private int _xShift;
        //[DataMember] public int XShift { get => _xShift; set => SetProperty(ref _xShift, value); }
        //private int _yShift;
        //[DataMember] public int YShift { get => _yShift; set => SetProperty(ref _yShift, value); }
        //private int _grid;
        //[DataMember] public int Grid { get => _grid; set => SetProperty(ref _grid, value); }



        [DataMember] public int JpegQuality { get; set; } = 96;//jpeg画質
        [DataMember] public double Top { get; set; }//アプリ
        [DataMember] public double Left { get; set; }//アプリ
        //保存先リスト
        [DataMember] public ObservableCollection<string> DirList { get; set; }
        [DataMember] public string? Dir { get; set; }
        [DataMember] public int DirIndex { get; set; }

        //チェックボックス
        [DataMember] public bool? IsDrawCursor { get; set; }//マウスカーソル描画の有無


        //ホットキー
        [DataMember] public bool HotkeyAlt { get; set; }
        [DataMember] public bool HotkeyCtrl { get; set; }
        [DataMember] public bool HotkeyShift { get; set; }
        [DataMember] public bool HotkeyWin { get; set; }
        [DataMember] public Key HotKey { get; set; }//キャプチャーキー


        //ファイルネーム        
        //[DataMember] public FileNameBaseType FileNameBaseType { get; set; }
        [DataMember] public bool IsFileNameDate { get; set; }
        [DataMember] public double FileNameDateOrder { get; set; }
        [DataMember] public string? FileNameDataFormat { get; set; }
        [DataMember] public ObservableCollection<string> FileNameDateFormatList { get; set; } = new();

        [DataMember] public bool IsFileNameSerial { get; set; }
        [DataMember] public decimal FileNameSerial { get; set; }
        [DataMember] public double FileNameSerialOrder { get; set; }
        [DataMember] public decimal FileNameSerialDigit { get; set; }
        [DataMember] public decimal FileNameSerialIncreace { get; set; }

        [DataMember] public bool IsFileNameText1 { get; set; }
        [DataMember] public string? FileNameText1 { get; set; }
        [DataMember] public ObservableCollection<string> FileNameText1List { get; set; } = new();

        [DataMember] public bool IsFileNameText2 { get; set; }
        [DataMember] public string? FileNameText2 { get; set; }
        [DataMember] public ObservableCollection<string> FileNameText2List { get; set; } = new();

        [DataMember] public bool IsFileNameText3 { get; set; }
        [DataMember] public string? FileNameText3 { get; set; }
        [DataMember] public ObservableCollection<string> FileNameText3List { get; set; } = new();

        [DataMember] public bool IsFileNameText4 { get; set; }
        [DataMember] public string? FileNameText4 { get; set; }
        [DataMember] public ObservableCollection<string> FileNameText4List { get; set; } = new();


        //音
        [DataMember] public bool IsSoundPlay { get; set; }
        //[DataMember] public bool IsSoundDefault { get; set; }
        [DataMember] public ObservableCollection<string> SoundFilePathList { get; set; } = new();
        [DataMember] public string? SoundFilePath { get; set; }
        [DataMember] public MySoundPlay MySoundPlay { get; set; }


        //保存時の動作
        [DataMember] public SaveBehaviorType SaveBehaviorType { get; set; }

        //起動時の動作
        //前回終了時の編集状態を読み込む
        [DataMember] public bool IsLoadPreviewData { get; set; } = false;

        private ImageType _ImageType = ImageType.png;//保存画像形式
        [DataMember]
        public ImageType ImageType
        {
            get => _ImageType;
            set
            {
                _ImageType = value;
                //if (_ImageType == value) return;
                //_ImageType = value;
                //RaisePropertyChanged();
            }
        }

        private CaptureRectType _RectType;//切り出し範囲

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        [DataMember]
        public CaptureRectType RectType
        {
            get => _RectType;
            set
            {
                _RectType = value;
                //if (_RectType == value) return;
                //_RectType = value;
                //RaisePropertyChanged();
            }
        }




        public AppConfig()
        {
            DirList = new ObservableCollection<string>();
            JpegQuality = 94;
            FileNameSerialIncreace = 1m;
            FileNameSerialDigit = 4m;
            HotKey = Key.PrintScreen;
            IsDrawCursor = false;
            IsFileNameDate = true;
        }


        //        c# - DataContract、デフォルトのDataMember値
        //https://stackoverrun.com/ja/q/2220925

        //初期値の設定
        [OnDeserialized]
        void OnDeserialized(System.Runtime.Serialization.StreamingContext c)
        {
            if (DirList == null) DirList = new();
            if (FileNameDateFormatList == null) FileNameDateFormatList = new();
            if (FileNameText1List == null) FileNameText1List = new();
            if (FileNameText2List == null) FileNameText2List = new();
            if (FileNameText3List == null) FileNameText3List = new();
            if (FileNameText4List == null) FileNameText4List = new();
            if (SoundFilePathList == null) SoundFilePathList = new();
        }
    }





    #region 列挙型

    public enum ImageType
    {
        png,
        bmp,
        jpg,
        gif,
        tiff,

    }
    public enum CaptureRectType
    {
        Screen,
        Window,
        WindowClient,
        UnderCursor,
        UnderCursorClient,
        WindowWithMenu,
        WindowWithRelatedWindow,
        WindowWithRelatedWindowPlus,

    }

    public enum MySoundPlay
    {
        None,
        PlayDefault,
        PlayOrder
    }

    public enum SaveBehaviorType
    {
        Save,
        Copy,
        SaveAndCopy,
        SaveAtClipboardChange,
        AddPreviewWindowFromClopboard
    }
    #endregion 列挙型
}
