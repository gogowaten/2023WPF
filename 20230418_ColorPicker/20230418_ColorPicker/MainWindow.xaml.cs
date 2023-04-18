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

namespace _20230418_ColorPicker
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
            this.Picker = new();
            Left = 100;
            Top = 100;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mx = Picker.Marker.Saturation;
        }

        private void MyButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            Picker.Show();
            Picker.Owner = this;

        }
    }




}
