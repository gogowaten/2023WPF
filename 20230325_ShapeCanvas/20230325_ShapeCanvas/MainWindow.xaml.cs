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

//テスト完成形
//切り替えボタンで頂点Thumbを表示して、編集状態になると
//頂点ThumbとそのRect表示

//クラス
//Bezier：Shape継承、ベジェ曲線を表示する、AdornerLayberにMyAdornerを持つ
//MyAdorner：Bezier専用Adorner、頂点Thumbを表示する
//GeometricCanvas：Canvas継承、Bezier一つを子に持つ。GridじゃなくてCanvas継承なのは、Bezierを指定位置に表示するため

namespace _20230325_ShapeCanvas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Left = 100;
            Top=100;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyGeo.MyIsEditing = !MyGeo.MyIsEditing;
            MyGeo.ChangeBinding();
            //if (MyGeo.MyIsEditing) { MyGeo.MyBoundsBorder.Visibility=Visibility.Visible; }
            //else { MyGeo.MyBoundsBorder.Visibility= Visibility.Collapsed; }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var left = Canvas.GetLeft(MyGeo);
            var top = Canvas.GetTop(MyGeo);
            var bezRect = MyGeo.MyBezier.MyExternalBounds;
            var thumbsRect = MyGeo.MyBezier.MyAdorner.MyVThumbsBounds;
            var bezAllRect = MyGeo.MyBezier.MyAllBounds;
            var sizeGeo = MyGeo.RenderSize;
            var sizeBez = MyGeo.MyBezier.RenderSize;
            var sizeAd = MyGeo.MyBezier.MyAdorner.RenderSize;
            var sizeAdCan = MyGeo.MyBezier.MyAdorner.MyCanvas.RenderSize;
            var drawGeo = VisualTreeHelper.GetDrawing(MyGeo)?.Bounds;
            var drawBez = VisualTreeHelper.GetDrawing(MyGeo.MyBezier)?.Bounds;
            var drawAd = VisualTreeHelper.GetDrawing(MyGeo.MyBezier.MyAdorner)?.Bounds;
            var drawAdCan = VisualTreeHelper.GetDrawing(MyGeo.MyBezier.MyAdorner.MyCanvas)?.Bounds;
            var desBez = VisualTreeHelper.GetDescendantBounds(MyGeo.MyBezier);
        }
    }
}
