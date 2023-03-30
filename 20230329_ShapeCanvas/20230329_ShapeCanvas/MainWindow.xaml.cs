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


//できた、リアルタイムは諦めてドラッグ移動終了後に座標修正するようにするテスト

namespace _20230329_ShapeCanvas
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
            var pts = GeometricCanvas.GetPointsRect(MyGeo.MyPoints);
            var bLeft = Canvas.GetLeft(MyGeo.MyBezier);
            var bTop = Canvas.GetTop(MyGeo.MyBezier);
        }
    }
}
