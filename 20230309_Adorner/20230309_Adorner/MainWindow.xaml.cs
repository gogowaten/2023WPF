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

namespace _20230309_Adorner
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(MyTThumb.MyTTAdorner.Visibility == Visibility.Visible)
            {
                MyTThumb.MyTTAdorner.Visibility=Visibility.Collapsed;
            }
            else
            {
                MyTThumb.MyTTAdorner.Visibility = Visibility.Visible;
            }
            var vvv = VisualTreeHelper.GetDescendantBounds(MyCanvas);
            MyCanvas.Width=vvv.Width;
            MyCanvas.Height = vvv.Height;

        }
        
    }
}
