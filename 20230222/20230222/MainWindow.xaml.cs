using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

namespace _20230222
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region

        public ObservableCollection<Point> DpOb1
        {
            get { return (ObservableCollection<Point>)GetValue(DpOb1Property); }
            set { SetValue(DpOb1Property, value); }
        }
        public static readonly DependencyProperty DpOb1Property =
            DependencyProperty.Register(nameof(DpOb1), typeof(ObservableCollection<Point>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<Point>()));
        public ObservableCollection<Point> DpOb2
        {
            get { return (ObservableCollection<Point>)GetValue(DpOb2Property); }
            set { SetValue(DpOb2Property, value); }
        }
        public static readonly DependencyProperty DpOb2Property =
            DependencyProperty.Register(nameof(DpOb2), typeof(ObservableCollection<Point>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<Point>()));

        public ObservableCollection<Point> Ob1 { get; set; } = new();



        public PointCollection DpPc1
        {
            get { return (PointCollection)GetValue(DpPc1Property); }
            set { SetValue(DpPc1Property, value); }
        }
        public static readonly DependencyProperty DpPc1Property =
            DependencyProperty.Register(nameof(DpPc1), typeof(PointCollection), typeof(MainWindow), new PropertyMetadata(new PointCollection()));

        public PointCollection Pc1 { get; set; } = new();
        #endregion




        public MainWindow()
        {
            InitializeComponent();
            #region

            //DpOb1.CollectionChanged += (a, b) => { var neko = 0; };
            //DpOb2.CollectionChanged += (a, b) => { var neko = 0; };
            //Ob1.CollectionChanged += (a, b) => { var neko = 0; };


            //SetBinding(DpOb1Property, new Binding()
            //{
            //    Source = this,
            //    Path = new PropertyPath(DpOb2Property),
            //    Mode = BindingMode.TwoWay,
            //});
            //SetBinding(DpOb2Property, new Binding()
            //{
            //    Source = this,
            //    Path = new PropertyPath(DpOb1Property),
            //    Mode = BindingMode.OneWay
            //});

            //SetBinding(DpOb1Property, new Binding(nameof(Ob1))
            //{
            //    Source = Ob1,
            //    Mode = BindingMode.TwoWay
            //});

            //DpOb1 = DpOb2;
            //DpOb2 = DpOb1;
            //DpOb1 = Ob1;

            //SetBinding(DpOb1Property, new Binding()
            //{
            //    Source = this.DpPc1,
            //    Path = new PropertyPath(DpPc1Property),
            //    Converter = new MyConverterPointCollection(),
            //    Mode = BindingMode.TwoWay
            //});

            //SetBinding(DpPc1Property, new Binding(nameof(Pc1))
            //{
            //    Source = this,
            //    Converter = new MyConverterPointCollection(),
            //    Mode = BindingMode.TwoWay,
            //});

            //DpOb1.Add(new Point());
            //DpPc1.Add(new());
            //Ob1.Add(new());
            //DpOb2.Add(new Point());
            #endregion

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MyAnchorCanvas.MyPoints.Add(new Point(200, 20));//アンカーThumbは追加されない
            MyAnchorCanvas.AddPoint(new Point(300, 50));

            //MyAnchorCanvas.MyDataObPoints.Add(new Point(300, 333));
            //MyAnchorCanvas.MyShapePoly.Points.Add(new Point(400, 100));

            var inu = MyAnchorCanvas.MyPoints;
            var neko = MyAnchorCanvas.MyShape.Points;
            //MyAnchorCanvas.MyShapePoly.InvalidateVisual();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(MyAnchorCanvas.MyAnchorVisible == Visibility.Visible)
            {
                MyAnchorCanvas.MyAnchorVisible = Visibility.Collapsed;
            }
            else
            {
                MyAnchorCanvas.MyAnchorVisible = Visibility.Visible;
            }
        }
    }

    public class MyConverterPointCollection : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PointCollection pc = (PointCollection)value;
            return new ObservableCollection<Point>(pc);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<Point> ob = (ObservableCollection<Point>)value;
            return new PointCollection(ob);
        }
    }
}
