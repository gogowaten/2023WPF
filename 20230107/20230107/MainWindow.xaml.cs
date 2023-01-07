using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace _20230107
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Data MyData { get; set; } = new();
        public TextBlock? tb { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Test1();
        }
        private void Test1()
        {
            MyData = new DataText() { Text = "test", X = 100, Y = 100 };
            tb = new();
            tb.SetBinding(TextBlock.TextProperty, new Binding(nameof(DataText.Text)) { Source = MyData });
            tb.SetBinding(Canvas.LeftProperty, new Binding(nameof(MyData.X)) { Source = MyData });
            tb.SetBinding(Canvas.TopProperty, new Binding(nameof(MyData.Y)) { Source = MyData });
            MyCanvas.Children.Add(tb);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MyData.Text = "henkou";
            if (MyData is DataText data) { data.Text = "henkou"; }
            //tb.Text = "henkou";
        }
    }


    public class Data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private double _x; public double X { get => _x; set => SetProperty(ref _x, value); }
        private double _y; public double Y { get => _y; set => SetProperty(ref _y, value); }


        public Data() { }
    }
    public class DataText : Data
    {

        private string? _text;
        public string? Text { get => _text; set => SetProperty(ref _text, value); }

    }
}
