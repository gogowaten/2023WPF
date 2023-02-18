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

namespace _20230213_BezierTest
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
            //MyTTBezier3.Points[1] = new Point(100, 100);
            //MyTTPolyLine.MyPP[1]=new Point(110, 210);
            var points = MyTTLine.MyPoints;
            var data = MyTTLine.MyData;
            var linepoint = MyTTLine.MyLine.Points;
            Point p0 = new(200, 200);
            Point p1 = new(300, 200);
            //MyTTLine.MyData.PointCollection.Add(p0);
            //MyTTLine.MyData.PointCollection.Add(p1);
            MyTTLine.MyPoints.Add(p0);
            MyTTLine.MyPoints.Add(p1);
            //MyTTLine.MyLine.Points.Add(p0);
            //MyTTLine.MyLine.Points.Add(p1);
        }
    }
}
