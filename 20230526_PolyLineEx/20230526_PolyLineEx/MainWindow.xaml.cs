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

namespace _20230526_PolyLineEx
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
            var neko = MyShape.RenderedGeometry.Bounds;
            var inu = MyShape.MyRenderBounds;

            //TransformBounds と RenderTransformOrigin
            //        https://social.msdn.microsoft.com/Forums/vstudio/en-US/b50eda7f-f888-4ce1-a604-385ec7b6ad7f/transformbounds-and-rendertransformorigin?forum=wpf

            //Originや全てのTransformを考慮したRectの取得
            Rect des = VisualTreeHelper.GetDescendantBounds(MyShape);
            GeneralTransform can = MyShape.TransformToAncestor(MyCanvas);
            var result = can.TransformBounds(des);//実際のBounds
            var dd = can.TransformBounds(inu);//これは違った、ってことはMyRenderBoundsはいらなかった？
        }
    }
}
