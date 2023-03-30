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

//できた
//移動と頂点移動できるベジェ曲線のThumb

//TThumb、Thumb継承、TemplateをCanvasに変更、このCanvasにBezierを追加表示している

//Bezier、Shape継承、AdornerにMyAdornerを追加している、これで頂点Thumbを表示している

//MyAdorner、Adorner継承、ベジェ曲線の頂点をThumbで表示している、ドラッグ移動で頂点座標も更新

//TThumb(マウスドラッグ移動可能、頂点Thumb移動終了時にイベント通知を受け取り座標修正する)
//  ┗Canvas(Template)
//      ┗Bezier
//          ┗MyAdorner
//              ┗VisualCollection
//                  ┗Canvas
//                      ┗Thumb(頂点表示用、マウスドラッグ移動可能、移動終了時にそれをイベントで通知)

//TThumbのTemplateをBezierじゃなくてCanvasにしているのは、もしBezierにすると
//Bezierの座標を指定してもCanvasがないとそれが反映されないから

//

namespace _20230330_BezierCanvasThumb
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
            MyTThumb.MyIsEditing = !MyTThumb.MyIsEditing;
            MyTThumb.ChangeBinding();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           var geo= MyTThumb.MyShape;
        }
    }
}
