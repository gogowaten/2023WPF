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
//PointCollection同士の双方向Binding
//問題ない
//Pointの変更で相手側も変更される
//Collection自体を変更した場合も同じく変更される
//注意したいのはBinding時、Sourceの値が優先される

namespace _20230510
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public PointCollection MyProperty
        {
            get { return (PointCollection)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register(nameof(MyProperty), typeof(PointCollection), typeof(MainWindow),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public MainWindow()
        {
            InitializeComponent();
            //MyProperty -> GeoLineにする場合
            //SetBinding(MyPropertyProperty, new Binding() { Source = MyGeoLine1, Path = new PropertyPath(GeoLine.AnchorsProperty), Mode = BindingMode.TwoWay });

            //GeoLine -> MyPropertyにする場合
            if (MyProperty == null || MyProperty.Count < 2)
            {
                MyProperty = MyGeoLine1.Anchors;
            }
            MyGeoLine1.SetBinding(GeoLine.AnchorsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPropertyProperty), Mode = BindingMode.TwoWay });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyGeoLine1.Anchors.Add(new Point(0, 100));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var neko = MyProperty;
            var inu = MyGeoLine1.Anchors;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MyProperty.Add(new Point(50, 70));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MyProperty = new PointCollection() { new Point(21, 200), new Point(100, 100) };
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MyGeoLine1.Anchors = new PointCollection() { new Point(21, 200), new Point(100, 100) };
        }
    }
}
