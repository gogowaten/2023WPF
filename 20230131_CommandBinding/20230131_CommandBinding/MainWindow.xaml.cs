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
//【WPF】Viewにコマンドを作る？RoutedCommandを作ってみよう | さんさめのC＃ブログ
//https://threeshark3.com/routed-command/
//Windows Presentation Foundation プログラミング入門/5.4 コマンド - WisdomSoft
//http://www.wisdomsoft.jp/448.html
//WPF4.5入門 その57「コマンド」 - かずきのBlog@hatena
//https://blog.okazuki.jp/entry/2014/10/29/221029

namespace _20230131_CommandBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand MyCommand = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("test");
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (MyCheckBox?.IsChecked is true);
        }
    }
}
