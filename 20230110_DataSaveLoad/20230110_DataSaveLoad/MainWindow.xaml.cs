﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Text.Json;
using System.IO;
using System.Runtime.Serialization.Json;

namespace _20230110_DataSaveLoad
{
    public partial class MainWindow : Window
    {
        public Data? MyData { get; set; }
        public TextBlock? MyTextBlock { get; set; }
        public TTTextBlock? MyTTTextBlock { get; set; }
        public TTRectangle? MyTTR { get; set; }
        public TTGroup? MyTTG1 { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            //Test1();
            //AddGroupFromItem();
            //AddGroupFromData();
            //SetDataForMyRoot();
        }

        //通常のCanvasにItemThumb追加テスト
        private void Test1()
        {
            //MyData = new DataText() { Text = "test", X = 100, Y = 100 };
            //MyTextBlock = new();
            //MyTextBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(DataText.Text)) { Source = MyData });
            //MyTextBlock.SetBinding(Canvas.LeftProperty, new Binding(nameof(MyData.X)) { Source = MyData });
            //MyTextBlock.SetBinding(Canvas.TopProperty, new Binding(nameof(MyData.Y)) { Source = MyData });
            //MyCanvas.Children.Add(MyTextBlock);

            var (text, rect) = MakeData(nameof(Test1) + "_Red", 0, 0, Brushes.Red);
            MyTTTextBlock = new(text);
            MyCanvas.Children.Add(MyTTTextBlock);
            MyTTR = new(rect);
            MyCanvas.Children.Add(MyTTR);
        }

        private (DataText text, DataRectangle rect) MakeData(string text, double x, double y, Brush brush)
        {
            return (new DataText() { Text = text, X = x, Y = y },
                new DataRectangle() { FillBrush = brush, X = 200, Y = 0, W = 100, H = 30 });
        }
        //通常のCanvasにGroupThumb追加テスト
        //GroupThumbコンストラクタにDataを渡して作成
        private void AddGroupFromData()
        {
            DataGroup dataGroup = new() { X = 30, Y = 30 };
            var (text, rect) = MakeData(nameof(AddGroupFromData) + "_BlueViolet", 0, 0, Brushes.BlueViolet);
            dataGroup.Datas.Add(text);
            dataGroup.Datas.Add(rect);

            MyTTG1 = new TTGroup(dataGroup);
            MyCanvas.Children.Add(MyTTG1);
        }
        //通常のCanvasにGroupThumb追加テスト
        //GroupThumbコンストラクタにDataを渡して作成
        private void AddGroupFromItem()
        {
            var (text, rect) = MakeData(nameof(AddGroupFromItem) + "_Olive", 0, 0, Brushes.Olive);
            TTTextBlock tb = new(text);
            TTRectangle trec = new(rect);

            DataGroup dataGroup = new() { X = 30, Y = 60 };
            MyTTG1 = new TTGroup(dataGroup);
            MyTTG1.AddItem(tb);
            MyTTG1.AddItem(trec);
            MyCanvas.Children.Add(MyTTG1);
        }

        //XAMLに用意してあるRootThumbにDataを追加
        private void SetDataForMyRoot()
        {
            DataGroup dataGroup = new() { X = 10, Y = 110 };
            var (text, rect) = MakeData(nameof(SetDataForMyRoot) + "_Gold", 0, 0, Brushes.Gold);
            dataGroup.Datas.Add(text);
            dataGroup.Datas.Add(rect);

            DataGroup dataRoot = new();
            dataRoot.Datas.Add(dataGroup);
            dataRoot.Datas.Add(new DataText() { Text = "roottyokka", X = 10, Y = 130 });
            MyRoot.SetData(dataRoot);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MyData.Text = "henkou";
            if (MyData is DataText data) { data.Text = "henkou"; }
            //MyTextBlock.Text = "henkou";
            //var neko = MyTTG1.MyData;
            //var inu = MyGroup1.MyData;
            var uma = MyRoot.MyData;
            MyRoot.AddItem(new TTTextBlock(new DataText() { Text = "addtext", X = 20, Y = 50 }));
            //MyTTTextBlock.TTText = "dipendencyProperty";
            MyRoot.MyData.Datas[0].X = 200;
        }

        private void ButtonSave_Click_1(object sender, RoutedEventArgs e)
        {
            //MyRoot.SaveDataToJson("E:\\MyData20230109.json");
            MyRoot.MySerializeData("E:\\MyData20230109.xml", MyRoot.MyData);
        }

        private void ButtonLoad_Click_2(object sender, RoutedEventArgs e)
        {
            //var ddata = MyRoot.MyDeserializeDData("E:\\MyData20230109.json");
            var data = MyRoot.MyDeserialize<Data>("E:\\MyData20230109.xml");
            if(data is DataGroup group) { MyRoot.SetData(group); }
        }
    }



    public abstract class TThumb : Thumb
    {
        #region プロパティ

        public double TTLeft
        {
            get { return (double)GetValue(TTLeftProperty); }
            set { SetValue(TTLeftProperty, value); }
        }
        public static readonly DependencyProperty TTLeftProperty =
            DependencyProperty.Register(nameof(TTLeft), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double TTTop
        {
            get { return (double)GetValue(TTTopProperty); }
            set { SetValue(TTTopProperty, value); }
        }
        public static readonly DependencyProperty TTTopProperty =
            DependencyProperty.Register(nameof(TTTop), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        #endregion プロパティ

        public Data? MyData { get; set; }
        public TThumb()
        {
            //MyData = new Data();
            DataContext = MyData;
            SetBinding(Canvas.LeftProperty, new Binding(nameof(MyData.X)));
            SetBinding(Canvas.TopProperty, new Binding(nameof(MyData.Y)));
            SetBinding(TTLeftProperty, new Binding(nameof(MyData.X)) { Mode = BindingMode.TwoWay });
            SetBinding(TTTopProperty, new Binding(nameof(MyData.Y)) { Mode = BindingMode.TwoWay });
        }

        internal abstract void SetTemplateAndBinding();


    }
    public abstract class TThumbItem : TThumb { }
    public class TTTextBlock : TThumbItem
    {
        public new DataText MyData { get; private set; } = new();

        #region プロパティ


        //図形コントロール
        //        http://www.kanazawa-net.ne.jp/~pmansato/wpf/wpf_graph_drawtool.htm#arrow
        //C#のWPFでコントロールを自作する その２ - Ararami Studio
        //        https://araramistudio.jimdo.com/2016/09/30/wpf%E3%81%A7%E3%82%B3%E3%83%B3%E3%83%88%E3%83%AD%E3%83%BC%E3%83%AB%E3%82%92%E8%87%AA%E4%BD%9C%E3%81%99%E3%82%8B-%E3%81%9D%E3%81%AE%EF%BC%92/

        //デザイナー画面でのリアルタイム更新と規定で双方向Bindingのために
        //FrameworkPropertyMetadataOptionsが必要
        public string TTText
        {
            get { return (string)GetValue(TTTextProperty); }
            set { SetValue(TTTextProperty, value); }
        }


        public static readonly DependencyProperty TTTextProperty =
            DependencyProperty.Register(nameof(TTText), typeof(string), typeof(TTTextBlock),
                new FrameworkPropertyMetadata("",
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion プロパティ

        public TTTextBlock()
        {
            DataContext = MyData;
            //DData = new(TTType.TextBlock) { Data = MyData };
            SetTemplateAndBinding();
        }
        public TTTextBlock(DataText data)
        {
            //この順番が重要、DataをセットしてからDatacontextに指定して、最後にBinding設定
            //こうしないと正しくBindingできない、Bindingがリセットされてしまう
            //特にDataセットしてからDataContextの指定は重要
            //Bindingの設定を先にしたいときはSourceを明示すればBindingできる
            MyData = data;
            DataContext = MyData;
            //DData = new(TTType.TextBlock) { Data = MyData };
            SetTemplateAndBinding();
        }
        internal override void SetTemplateAndBinding()
        {
            FrameworkElementFactory fTextBlock = new(typeof(TextBlock));
            FrameworkElementFactory factory = new(typeof(Border));
            factory.AppendChild(fTextBlock);
            fTextBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(MyData.Text)));
            this.Template = new() { VisualTree = factory };
            //これは双方向Bindingが必要と思ってたけど、依存プロパティのメタデータの設定でBindsTwoWayByDefaultを指定しておくと不必要だった
            SetBinding(TTTextProperty, new Binding(nameof(MyData.Text)));
            //SetBinding(TTTextProperty, new Binding(nameof(MyData.Text)) { Mode = BindingMode.TwoWay });

        }
    }
    public class TTRectangle : TThumbItem
    {
        #region プロパティ

        
        public Brush TTFillBrush
        {
            get { return (Brush)GetValue(TTFillBrushProperty); }
            set { SetValue(TTFillBrushProperty, value); }
        }
        public static readonly DependencyProperty TTFillBrushProperty =
            DependencyProperty.Register(nameof(TTFillBrush), typeof(Brush), typeof(TTRectangle),
                new FrameworkPropertyMetadata(Brushes.Red,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        #endregion プロパティ

        public new DataRectangle MyData { get; set; } = new();
        public TTRectangle()
        {
            DataContext = MyData;
            SetTemplateAndBinding();
        }
        public TTRectangle(DataRectangle data)
        {
            MyData = data;
            DataContext = MyData;
            SetTemplateAndBinding();
        }
        internal override void SetTemplateAndBinding()
        {
            FrameworkElementFactory fContent = new(typeof(Rectangle));
            fContent.SetValue(Shape.FillProperty, new Binding(nameof(MyData.FillBrush)));
            fContent.SetValue(WidthProperty, new Binding(nameof(MyData.W)));
            fContent.SetValue(HeightProperty, new Binding(nameof(MyData.H)));
            this.Template = new() { VisualTree = fContent };

            //これも双方向Binding指定が必要
            SetBinding(WidthProperty, new Binding(nameof(MyData.W)) { Mode = BindingMode.TwoWay });
            SetBinding(HeightProperty, new Binding(nameof(MyData.H)) { Mode = BindingMode.TwoWay });
            SetBinding(TTFillBrushProperty, new Binding(nameof(MyData.FillBrush)) { Mode = BindingMode.TwoWay });

        }

    }

    [ContentProperty(nameof(Children))]
    public class TTGroup : TThumb
    {
        public new DataGroup MyData { get; set; } = new();
        public ObservableCollection<TThumb> Children { get; set; } = new();
        #region 初期設定

        public TTGroup()
        {
            DataContext = MyData;
            SetTemplateAndBinding();
            Children.CollectionChanged += Children_CollectionChanged;
        }
        public TTGroup(DataGroup data)
        {
            MyData = data;
            DataContext = MyData;
            SetTemplateAndBinding();
            SetData(data);

        }
        internal override void SetTemplateAndBinding()
        {
            FrameworkElementFactory factory = new(typeof(ItemsControl));
            factory.SetValue(ItemsControl.ItemsPanelProperty,
                new ItemsPanelTemplate(new FrameworkElementFactory(typeof(Canvas))));
            factory.SetValue(ItemsControl.ItemsSourceProperty, new Binding(nameof(Children)) { Source = this });
            this.Template = new() { VisualTree = factory };
        }

        public void SetData(DataGroup data)
        {
            foreach (var item in data.Datas)
            {
                TThumb? tt = null;
                switch (item)
                {
                    case DataText dataText:
                        tt = new TTTextBlock(dataText);
                        break;
                    case DataRectangle rectangle:
                        tt = new TTRectangle(rectangle);
                        break;
                    case DataGroup dataGroup:
                        tt = new TTGroup(dataGroup);
                        break;
                }
                if (tt != null)
                {
                    Children.Add(tt);
                    //tt.DragDelta += TThumb_DragDelta;
                }
            }
        }

        #endregion 初期設定

        public void AddItem(TThumb thumb)
        {
            Children.Add(thumb);
            AddItemData(thumb);
        }
        private void Children_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems?[0] is TThumb nItem) { AddItemData(nItem); }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems?[0] is TThumb oItem) { MyData.Datas.Remove(oItem.MyData); }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }
        void AddItemData(TThumb thumb)
        {
            var data = GetMyDataFromItem(thumb);
            if (data != null) { MyData.Datas.Add(data); }

            static Data? GetMyDataFromItem(TThumb tt)
            {
                return tt switch
                {
                    TTTextBlock text => text.MyData,
                    TTRectangle rect => rect.MyData,
                    TTGroup group => group.MyData,
                    _ => null,
                };
                ;
            }
        }
        private void TThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            TTTop += e.VerticalChange;
            TTLeft += e.HorizontalChange;
        }

    }


    public class TTRoot : TTGroup
    {
        public TTGroup? ActiveGroup { get; set; }
        public TTRoot()
        {
            DataContext = MyData;
        }

        //JSON形式で保存、XMLをあんまり変わらないかな
        //public void SaveDataToJson(string filePath)
        //{
        //    //using MemoryStream stream = new();
        //    //using FileStream fs = new(filePath, FileMode.Create);
        //    //using (var sw = new StreamWriter(fs))
        //    //{
        //    //    DataContractJsonSerializerSettings settings = new();
        //    //    var ser = new DataContractJsonSerializer(typeof(DData));
        //    //    ser.WriteObject(stream, DData);
        //    //    var str2write = Encoding.UTF8.GetString(stream.ToArray());
        //    //    sw.Write(str2write);
        //    //}

        //    //using var stream = new MemoryStream();
        //    //using var reader = new StreamReader(stream);
        //    //using (var writer = new StreamWriter(filePath, false))
        //    //{
        //    //    var ser = new DataContractJsonSerializer(typeof(DData));
        //    //    ser.WriteObject(stream, DData);
        //    //    stream.Position = 0;
        //    //    writer.WriteLine(reader.ReadToEnd());
        //    //}

        //    ////オブジェクトの配列形式のJSONを書き出す : C# | iPentec
        //    ////        https://www.ipentec.com/document/csharp-class-serialize-write-json-array-file

        //    //DataContractJsonSerializer serializer = new(typeof(DData));
        //    //using var stream = new FileStream(filePath, FileMode.Create);
        //    //try
        //    //{
        //    //    serializer.WriteObject(stream, DData);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show(ex.Message);
        //    //}

        //    //C#でJsonをSerialize/Deserializeする方法 - Qiita
        //    //        https://qiita.com/Jinten/items/3d4745c2663d8b4fa3cc
        //    using var fs = new FileStream(filePath, FileMode.Create);
        //    //DataContractJsonSerializer serializer = new(typeof(DData));
        //    //改行できてる？
        //    using var writer = JsonReaderWriterFactory.CreateJsonWriter(fs, Encoding.UTF8, true, true);
        //    try
        //    {
        //        serializer.WriteObject(fs, DData);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }


        //}

        //public void MySerialize<T>(string filePath, T data)
        //{
        //    XmlWriterSettings settings = new()
        //    {
        //        Encoding = new UTF8Encoding(false),
        //        Indent = true,
        //        NewLineOnAttributes = false,
        //        ConformanceLevel = ConformanceLevel.Fragment
        //    };
        //    DataContractSerializer serializer = new(typeof(T));
        //    using var writer = XmlWriter.Create(filePath, settings);
        //    try { serializer.WriteObject(writer, data); }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}
        public void MySerializeData(string filePath, Data data)
        {
            XmlWriterSettings settings = new()
            {
                Encoding = new UTF8Encoding(false),
                Indent = true,
                NewLineOnAttributes = false,
                ConformanceLevel = ConformanceLevel.Fragment
            };
            DataContractSerializer serializer = new(typeof(Data));
            using var writer = XmlWriter.Create(filePath, settings);
            try { serializer.WriteObject(writer, data); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }


      
        public T? MyDeserialize<T>(string filePath)
        {
            DataContractSerializer serializer = new(typeof(T));
            try
            {
                using XmlReader reader = XmlReader.Create(filePath);
                return (T?)serializer.ReadObject(reader);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return default;
            }
        }
    }

}
