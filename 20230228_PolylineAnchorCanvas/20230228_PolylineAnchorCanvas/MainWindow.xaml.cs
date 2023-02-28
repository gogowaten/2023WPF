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

//Polyline表示、Polylineのアンカー点表示非表示、アンカー点のマウス移動
//アンカー点の追加と削除は右クリックメニュー

//アンカー点がマイナス座標になった場合の処理がまだ
namespace _20230228_PolylineAnchorCanvas
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MyPolylineCanvas.MyAnchorVisible != Visibility.Visible)
            {
                MyPolylineCanvas.MyAnchorVisible = Visibility.Visible;
            }
            else
            {
                MyPolylineCanvas.Visibility = Visibility.Collapsed;
            }
        }
    }
}
