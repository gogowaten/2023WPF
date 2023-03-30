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


//20230329を書き直しただけ
//図形を表示するCanvas
//編集状態の切り替えで頂点Thumbを表示、ドラッグ移動できる
//ドラッグ移動終了後にCanvasと図形の座標修正
//サイズは移動中に変化

//クラス
//GeometricCanvas：Canvas継承
//  CanvasーBezier

//Bezier:Shape継承、ベジェ曲線
//  BezierーAdorner

//MyAdorner：Bezier専用、頂点Thumbを表示する、ドラッグ移動もここ、目印用に背景色を設定している
//  AdornerーCanvasーThumbs

namespace _20230330_BezierCanvas
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
            Top = 100;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyGeo.MyIsEditing = !MyGeo.MyIsEditing;
            MyGeo.ChangeBinding();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var left = Canvas.GetLeft(MyGeo);
            var top = Canvas.GetTop(MyGeo);
            var bezRect = MyGeo.MyBezier.MyExternalBounds;
            //var thumbsRect = MyGeo.MyBezier.MyAdorner.MyVThumbsBounds;
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
            var pts = GeometricCanvas.GetPointsRect(MyGeo.MyPoints);
            var bLeft = Canvas.GetLeft(MyGeo.MyBezier);
            var bTop = Canvas.GetTop(MyGeo.MyBezier);
        }
    }
}
