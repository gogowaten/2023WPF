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

namespace _20230318_BezierSize2
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
            var rendersize = MyBezier0.RenderSize;//160 130
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
            
        }
    }
}
