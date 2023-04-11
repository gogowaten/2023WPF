using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace _20230411_ColorPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public Color MyColor
        {
            get { return (Color)GetValue(MyColorProperty); }
            set { SetValue(MyColorProperty, value); }
        }
        public static readonly DependencyProperty MyColorProperty =
            DependencyProperty.Register("MyColor", typeof(Color), typeof(MainWindow),
                new FrameworkPropertyMetadata(Color.FromArgb(0, 0, 0, 0),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public MyHsv MyHsv
        {
            get { return (MyHsv)GetValue(MyHsvProperty); }
            set { SetValue(MyHsvProperty, value); }
        }
        public static readonly DependencyProperty MyHsvProperty =
            DependencyProperty.Register(nameof(MyHsv), typeof(MyHsv), typeof(MainWindow),
                new FrameworkPropertyMetadata(new MyHsv(0, 0, 0),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));





        public MainWindow()
        {
            InitializeComponent();

            SetBinding(MyColorProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyHsvProperty),
                Converter = new ConverterHsv()
            });
            MyBorder.SetBinding(BackgroundProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyColorProperty),
                Converter = new ConverterSolidBruhs()
            });

            MyScrollBarH.SetBinding(ScrollBar.ValueProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyHsvProperty),
                Converter = new ConverterHsv2H(),
                Mode = BindingMode.TwoWay,
                ConverterParameter = new object[] { MyScrollBarS.Value, MyScrollBarV.Value }
            });
            MyScrollBarS.SetBinding(ScrollBar.ValueProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyHsvProperty),
                Converter = new ConverterHsv2S(),
                Mode = BindingMode.TwoWay,
                ConverterParameter = new object[] { MyScrollBarH.Value, MyScrollBarV.Value }
            });
            MyScrollBarV.SetBinding(ScrollBar.ValueProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(MyHsvProperty),
                Converter = new ConverterHsv2V(),
                Mode = BindingMode.TwoWay,
                ConverterParameter = new object[] { MyScrollBarH.Value, MyScrollBarS.Value }
            });

            //MyBorder.SetBinding(BackgroundProperty, new Binding()
            //{
            //    Source = this,
            //    Path = new PropertyPath(MyHsvProperty),
            //    Converter = new ConverterHsv(),
            //});
        }
        private void ScrollRGBBinding()
        {
            MyScrollBarR.SetBinding(ScrollBar.ValueProperty, new Binding()
            {
                Source = MyBorder,
                Path = new PropertyPath(BackgroundProperty),
                Converter = new ConverterR(),
                ConverterParameter = MyBorder.Background,
            });
            MyScrollBarG.SetBinding(ScrollBar.ValueProperty, new Binding()
            {
                Source = MyBorder,
                Path = new PropertyPath(BackgroundProperty),
                Converter = new ConverterG(),
                ConverterParameter = MyBorder.Background,
            });
            MyScrollBarB.SetBinding(ScrollBar.ValueProperty, new Binding()
            {
                Source = MyBorder,
                Path = new PropertyPath(BackgroundProperty),
                Converter = new ConverterB(),
                ConverterParameter = MyBorder.Background,
            });

        }

        private void MyScrollBarHsv_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //(byte r, byte g, byte b) = HSV.HSV2RGB(MyScrollBarH.Value, MyScrollBarS.Value, MyScrollBarV.Value);
            //MyScrollBarR.Value = r;
            //MyScrollBarG.Value = g;
            //MyScrollBarB.Value = b;
        }

        private void MyScrollBarRGB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            //(double h, double s, double v) = HSV.RGB2HSV(MyScrollBarR.Value, MyScrollBarG.Value, MyScrollBarB.Value);
            //MyScrollBarH.Value = h;
            //MyScrollBarS.Value = s;
            //MyScrollBarV.Value = v;
        }


    }
}
