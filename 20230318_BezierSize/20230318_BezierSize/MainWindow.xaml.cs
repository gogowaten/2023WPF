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

namespace _20230318_BezierSize
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
            var size = MyBezier0.RenderSize;//160 130
            var drawRect = VisualTreeHelper.GetDrawing(MyBezier0)?.Bounds;//-81 -10 241 140
            var beziRect = VisualTreeHelper.GetDescendantBounds(MyBezier0);//-81 -10 241 140
            
        }
    }
}
