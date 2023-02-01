using Microsoft.VisualBasic;
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
//TabControlテスト
//Header上でのマウスホイールでタブ切り替え
//各TabItemにはScrollViewerを設置するといい、これで
//Header上のみでタブ切り替えになる、しない場合はHeader以外でも切り替えになってしまう

namespace _20230201_Layout
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void MyTabControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is TabControl tab)
            {
                int ii = tab.SelectedIndex;
                if (e.Delta > 0 && ii - 1 >= 0)
                {
                    ii--;
                }
                else if (e.Delta < 0 && ii + 1 < tab.Items.Count)
                {
                    ii++;
                }
                tab.SelectedIndex = ii;

            }
        }
    }
}
