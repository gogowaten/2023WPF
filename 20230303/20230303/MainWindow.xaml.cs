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

namespace _20230303
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(MyTextBox).Add(new BBAdor(MyTextBox));
            AdornerLayer.GetAdornerLayer(MyPolyline).Add(new CCAdor(MyPolyline));
            //AdornerLayer.GetAdornerLayer(MyBorder).Add(new Ador(MyBorder));
            //AdornerLayer.GetAdornerLayer(MyButton).Add(new Ador(MyButton));
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            var csize = MyCanvas.RenderSize;
            var size = MyTextBox.RenderSize;
            var conrect = VisualTreeHelper.GetContentBounds(MyTextBox);//empty
            var desenbound = VisualTreeHelper.GetDescendantBounds(MyTextBox);
            var tbound = MyTextBox.RenderTransform.TransformBounds(new Rect(MyTextBox.RenderSize));
            VisualBrush vb = new(MyTextBox);
            DrawingVisual dv = new();
            using (var context = dv.RenderOpen())
            {
                context.DrawRectangle(vb, null, new Rect(tbound.Size));
            }
            RenderTargetBitmap bitmap = new((int)tbound.Width, (int)tbound.Height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
        }
    }
}
