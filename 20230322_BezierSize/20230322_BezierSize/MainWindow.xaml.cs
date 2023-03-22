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


//編集状態はfalseにしておいて、頂点移動後にFix0Point2ボタンでRect修正
//編集状態trueで頂点移動では不具合で、図形がCanvasRectの外に出た瞬間に頂点Thumbがすっ飛ぶ

//要素構造
//BeCanvasはCanvas継承、これのChildrenにBezierを入れているだけ
//BezierはShape継承、Pointsを設定でベジェ曲線表示、
//頂点ThumbはBezierのAdornerに表示している

namespace _20230322_BezierSize
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MyBezier0.MyPoints[4] = new Point(100, 100);
            MyBezier0.MyBezier.MyAdorner?.FixThumbsLocate();

            var offset = VisualTreeHelper.GetOffset(MyBezier0);
            var rendersize = MyBezier0.RenderSize;
            var renderbezier = MyBezier0.MyBezier.RenderSize;
            var candes = VisualTreeHelper.GetDescendantBounds(MyBezier0);
            var bedes = VisualTreeHelper.GetDescendantBounds(MyBezier0.MyBezier);
            MyBezier0.UpdateLayout();
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

