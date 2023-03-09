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

namespace _20230309_Adorner
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
            if (AdornerLayer.GetAdornerLayer(MyPolyBezier) is AdornerLayer layer)
            {
                layer.Add(new GeoAdorner(MyPolyBezier));                
                layer.Add(new GeoAdorner(MyPolyGeoLine));
                layer.Add(new GeoAdorner(MyTThumb));
                
                
            }
            //if (AdornerLayer.GetAdornerLayer(MyPolyGeoLine) is AdornerLayer layer1)
            //{
            //    layer1.Add(new GeoAdorner(MyPolyGeoLine));
            //}

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           var rs= MyTThumb.MyPolyGeoLine.MyBounds;
            MyTThumb.Width= rs.Width;
            MyTThumb.Height= rs.Height;
            var inu = MyTThumb.GeoBounds;
            var uma = MyTThumb.GeoWideBounds;
            var tako = MyTThumb.GeoWideTFBounds;
            var ika = MyTThumb.GeoTF_WideBounds;
            var neko = MyTThumb.MyPolyGeoLine.MyBounds;

            Rect r = VisualTreeHelper.GetDescendantBounds(MyPolyBezier);
            Rect rr = MyPolyBezier.RenderTransform.TransformBounds(r);
            rr = neko;
            MyRectangle.Width = rr.Width;
            MyRectangle.Height = rr.Height;
            Canvas.SetLeft(MyRectangle, rr.Left + Canvas.GetLeft(MyTThumb));
            Canvas.SetTop(MyRectangle, rr.Top + Canvas.GetTop(MyTThumb));


        }
        
    }
}
