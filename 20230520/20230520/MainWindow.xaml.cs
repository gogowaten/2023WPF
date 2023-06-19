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
using System.Windows.Controls.Primitives;

namespace _20230520
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
            var neko = MyContent.MyGeoPolyLineShape.MyRenderRect;
            var width = BindingOperations.GetBinding(MyContent, WidthProperty);
            Geometry clip = LayoutInformation.GetLayoutClip(MyContent);
            UIElement dispa = LayoutInformation.GetLayoutExceptionElement(MyContent.Dispatcher);
            Rect slot = LayoutInformation.GetLayoutSlot(MyContent);
            Size des = MyContent.DesiredSize;
        }

        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is TThumb tt)
            {
                tt.X += e.HorizontalChange;
                tt.Y += e.VerticalChange;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MyContent.MyAnchirPoints[1] =new Point(100, 50);
        }
    }

}
