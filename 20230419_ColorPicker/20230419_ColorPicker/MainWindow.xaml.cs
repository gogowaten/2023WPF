using System;
using System.Collections.Generic;
using System.Globalization;
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

//WPF、カラーピッカーの土台できた - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2023/04/20/164232

namespace _20230419_ColorPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Picker Picker;

        public MainWindow()
        {
            InitializeComponent();

            //色指定で作成
            //Picker = new(Colors.MediumAquamarine);

            //色指定無しで作成
            Picker = new();

            DataContext = this;
            Left = 100;
            Top = 100;
            Loaded += (s, e) => { Picker.Owner = this; };

        }


        
        private void MyButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            Picker.Top = this.Top + 150;
            Picker.Left = this.Left;
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
