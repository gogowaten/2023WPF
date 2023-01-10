using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
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

//【C#】標準機能でJSON をシリアライズ、デシリアライズする - PG日誌
//https://takap-tech.com/entry/2017/01/18/120000
//[C#] C#でJSONを扱う方法まとめ | DevelopersIO
//https://dev.classmethod.jp/articles/c-sharp-json/
//C#でJsonをSerialize/Deserializeする方法 - Qiita
//https://qiita.com/Jinten/items/3d4745c2663d8b4fa3cc

namespace _20230110_JSON
{


    public partial class MainWindow : Window
    {
        Data MyData { get; set; }
        DataShape MyDataShape { get; set; }
        DataGroup MyGroupData { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MyData = new();
            MyDataShape = new();
            MyGroupData = new() { Name = "Listtttttttttt" };
            MyGroupData.DataList.Add(MyData);
            MyGroupData.DataList.Add(MyDataShape);
        }

        private void Save<T>(string filePath, T data)
        {
            using var fs = new FileStream(filePath, FileMode.Create);
            List<Type> known = new()
                {
                    typeof(SolidColorBrush),
                    typeof(MatrixTransform),
                    typeof(DataShape),
                    //typeof(DataGroup),
                };

            DataContractJsonSerializer serializer = new(typeof(T), known);
            using var writer = JsonReaderWriterFactory.CreateJsonWriter(fs, Encoding.UTF8, true, true);
            serializer.WriteObject(writer, data);
        }
        private T? Load<T>(string filePath)
        {
            List<Type> known = new()
                {
                    typeof(SolidColorBrush),
                    typeof(MatrixTransform),
                    typeof(DataShape),
                    //typeof(DataGroup),
                };

            DataContractJsonSerializer serializer = new(typeof(T), known);
            using var fs = new FileStream(filePath, FileMode.Open);
            using var reader = JsonReaderWriterFactory.CreateJsonReader(fs, System.Xml.XmlDictionaryReaderQuotas.Max);

            var result = serializer.ReadObject(reader);
            if (result is T tt)
            {
                return tt;
            }
            else return default;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Save("E:\\20230110Test.json", MyGroupData);
            //Save("E:\\20230110Test.json", MyDataShape);
            //Save("E:\\20230110Test.json", MyData);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var data0 = Load<DataGroup>("E:\\20230110Test.json");//これだとDataGroup型になるけど
            var data1 = Load<Data>("E:\\20230110Test.json");//こっちだとData型になるので、中のListは取得できない
        }
    }

    #region Data

    [DataContract]
    //[KnownType(nameof(SolidColorBrush))]//ここで指定しても無視される、なんで？
    //[KnownType(nameof(MatrixTransform))]
    public class Data : INotifyPropertyChanged
    {   
        //通知プロパティもシリアル化できた
        private string _name = "";
        [DataMember] public string Name { get => _name; set => SetProperty(ref _name, value); }

        [DataMember] public double X { get; set; }
        [DataMember] public double Y { get; set; }
        [DataMember] public Brush FillBrush { get; set; } = Brushes.Red;
        public Data()
        {
            _name = "syokiti";
            X = 100;
            Y = 200;
            FillBrush = Brushes.Blue;
        }


        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    [DataContract]
    public class DataShape : Data
    {
        [DataMember] public double W { get; set; }

    }
    [DataContract]
    public class DataGroup : Data
    {
        [DataMember] public ObservableCollection<Data> DataList { get; set; } = new();

    }
    #endregion Data
}
