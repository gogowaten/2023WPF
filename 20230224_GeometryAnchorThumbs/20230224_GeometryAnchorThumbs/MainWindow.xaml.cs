using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace _20230224_GeometryAnchorThumbs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PointCollection MyPoints { get; set; } = new();
        //public ObservableCollection<Point> MyPoints { get; set; } = new();

        
        public MainWindow()
        {
            InitializeComponent();
            AnchorTest();
        }
        private void AnchorTest()
        {
            MyPoints.Add(new(20, 100));
            MyPoints.Add(new(100, 10));
            //foreach (var item in MyPoints)
            //{
            //    MyThumbs.Add(new TThumb(item));
            //}

            MyItemsControl.DataContext = MyPoints;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var neko = MyPoints;
            //var inu = MyThumbs[0].X;
        }
    }
}
