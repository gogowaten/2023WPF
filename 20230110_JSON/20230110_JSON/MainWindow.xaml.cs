using System;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Save("E:\\20230110Test.json", MyGroupData);
            //Save("E:\\20230110Test.json", MyDataShape);
            //Save("E:\\20230110Test.json", MyData);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }

    [DataContract]
    //[KnownType(nameof(SolidColorBrush))]//ここで指定しても無視される、なんで？
    //[KnownType(nameof(MatrixTransform))]
    public class Data : INotifyPropertyChanged
    {
        //public string Name { get; set; }

        private string _name = "";
        [DataMember] public string Name { get => _name; set => SetProperty(ref _name, value); }

        [DataMember] public double X { get; set; }
        [DataMember] public double Y { get; set; }
        [DataMember] public Brush FillBrush { get; set; } = Brushes.Red;
        public Data()
        {
            _name = "syokiti";
            X = 100; Y = 200;
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
        public DataGroup()
        {

        }
    }
}
