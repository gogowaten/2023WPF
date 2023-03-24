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

//2023WPF/20230323_BezierSize at main · gogowaten/2023WPF
//https://github.com/gogowaten/2023WPF/tree/main/20230323_BezierSize
//より、図形座標修正
//編集状態切り替えボタンで頂点Thumbの表示切替、頂点Thumb移動できるようになる
//図形自体もマウスでドラッグ移動


//ThumbのTemplateをCanvasとShape継承のBezierを使っている
//このCanvasは背景色に使っているだけなので、他の要素を入れられるものならなんでもいい
namespace _20230323_BezierSize2Thumb
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
            MyBezier0.MyIsEditing = !MyBezier0.MyIsEditing;
        }

        private void MyBezier0_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Canvas.SetLeft(MyBezier0, Canvas.GetLeft(MyBezier0) + e.HorizontalChange);
            Canvas.SetTop(MyBezier0, Canvas.GetTop(MyBezier0) + e.VerticalChange);
        }
    }
}
