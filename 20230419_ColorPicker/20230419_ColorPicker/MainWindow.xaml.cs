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

namespace _20230419_ColorPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Picker Picker;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            
            //色指定で作成
            //Picker = new(Colors.MediumAquamarine);

            //色指定無しで作成
            Picker = new();

            Left = 100;
            Top = 100;
            Picker.Closing += Picker_Closing;
            Loaded += (s, e) => { Picker.Owner = this; };
        }

        private void Picker_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            var neko = Picker.PickColor;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mx = Picker.Marker.Saturation;
            var www = Picker.Marker.Width;
            var ww = Picker.MyImageSV.Width;
            var str = Picker.MyImageSV.Stretch;
            Picker.SetColor(Color.FromArgb(200, 200, 2, 0));
        }

        private void MyButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            Picker.Top = this.Top + 150;
            Picker.Show();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SolidColorBrush solid = (SolidColorBrush)MyBorder1.Background;
            Picker.SetColor(solid.Color);
            Picker.Show();

            //最前面表示
            //Picker.Topmost = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SolidColorBrush solid = (SolidColorBrush)MyBorder2.Background;
            Picker.SetColor(solid.Color);
            Picker.Show();
        }
    }
}
