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

namespace _20230303
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(MyTextBox).Add(new Ador(MyTextBox));
            AdornerLayer.GetAdornerLayer(MyBorder).Add(new Ador(MyBorder));
            AdornerLayer.GetAdornerLayer(MyButton).Add(new Ador(MyButton));
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            var rect = MyBorder.RenderSize;
            var conrect = VisualTreeHelper.GetContentBounds(MyBorder);
        }
    }
}
