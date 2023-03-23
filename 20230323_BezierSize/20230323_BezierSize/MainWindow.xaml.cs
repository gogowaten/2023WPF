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


//編集状態trueのときは頂点Thumbを移動中にCanvasとBezier(Shape)の座標修正する
//falseのときは座標修正しない、このときFix0Point2ボタンで座標修正する
//一週間以上かかってようやく期待通りの動作にできたけど、なんでこうなるのかわかっていない
//あと、回転などの変形にも対応できない
namespace _20230323_BezierSize
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
            var rendersize = MyBezier0.RenderSize;//160 130
            var actWidth = MyBezier0.MyBezier.ActualWidth;
            var offset = VisualTreeHelper.GetOffset(MyBezier0);

            var drawRect = VisualTreeHelper.GetDrawing(MyBezier0)?.Bounds;//-81 -10 241 140
            var beziRect = VisualTreeHelper.GetDescendantBounds(MyBezier0);//-81 -10 241 140

            //DrawingVisual dv = new();
            //using(var context = dv.RenderOpen())
            //{
            //    VisualBrush vb = new(MyBezier0);
            //    context.DrawRectangle(vb, null, new Rect(rendersize));
            //}
            RenderTargetBitmap bitmap = new((int)rendersize.Width, (int)rendersize.Height, 96, 96, PixelFormats.Pbgra32);
            //bitmap.Render(dv);
            bitmap.Render(MyBezier0);

        }

        

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MyBezier0.MyIsEditing = !MyBezier0.MyIsEditing;
        }

        
        
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            MyBezier0.Fix0Point2();
            MyBezier0.MyBezier.MyAdorner?.FixThumbsLocate();
        }
    }
}
