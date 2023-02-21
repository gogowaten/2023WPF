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

namespace TTPolylineZ
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
            Point p1 = new(200, 100);
            var ttpoint = MyThumb.MyPoints;
            var datapoint = MyThumb.MyData.PointCollection;
            var polypoint = MyThumb.MyPolylineZ.MyPoints;

            MyThumb.MyPoints[0] = p1;
            //MyThumb.MyData.PointCollection[0] = p1;
            //MyThumb.MyPolylineZ.MyPoints[0] = p1;
            //MyThumb.MyPolylineZ.InvalidateVisual();
            MyThumb.MyData.StrokeThickness = 20;
            //MyThumb.MyPolylineZ.StrokeThickness= 20;
            //MyThumb.StrokeThickness = 20;
        }
    }
}
