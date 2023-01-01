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

namespace _20230101_ElementToPngFile
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
        private void Save1(FrameworkElement el, string filePath)
        {
            RenderTargetBitmap bitmap = new((int)el.ActualWidth, (int)el.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(el);
            PngBitmapEncoder encoder = new();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using (var stream = System.IO.File.Open(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(stream);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path = "E:elementImage.png";
            Save1(MyRectangle1, path);
            //Save1(MyCanvas,path);
        }
    }
}
