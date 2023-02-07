using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
//Pixtack2nd/MainWindow.xaml at 2591c4dcfcbd28cf340ae5d21283098bf383eefa · gogowaten/Pixtack2nd
//https://github.com/gogowaten/Pixtack2nd/blob/2591c4dcfcbd28cf340ae5d21283098bf383eefa/Wpf_Pixtack_test2_ExThumb/Wpf_Pixtack_test2_ExThumb/MainWindow.xaml
//第6回　「コマンド」と「MVVMパターン」を理解する：連載：WPF入門（2/3 ページ） - ＠IT
//https://atmarkit.itmedia.co.jp/ait/articles/1011/09/news102_2.html

namespace _20230131_CommandBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand MyCommand { get; } = new();
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
