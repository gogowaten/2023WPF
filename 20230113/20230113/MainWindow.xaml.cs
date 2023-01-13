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

namespace _20230113
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var neko = MyTTTextBlock.Data;
            TTTextBlock tTTextBlock = new(new DataTextBlock() { Text = "TTT", X = 100, Y = 100 });
            MyCanvas.Children.Add(tTTextBlock);

            DataTextBlock data = new() { Text = "TTT2", X = 120, Y = 120 };
            tTTextBlock = new(data);
            //MyRoot.AddItem(tTTextBlock, tTTextBlock.Data);
            MyRoot.AddItem(data);
        }

        private void ButtonTest1_Click(object sender, RoutedEventArgs e)
        {
            var neko = MyRoot.Data;
        }
    }
}
