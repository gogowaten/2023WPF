using System.Windows;
using System.Windows.Documents;

//WPF、マウスでTextBoxのサイズ変更するのにAdorner(装飾者)を使ってみた - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2023/03/05/142526

namespace _20230305_Adorner
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Loaded時に装飾する
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //静的メソッドのGetAdornerLayerで対象要素のAdornerLayerを取得して
            //そこにAdornerを追加する
            if (AdornerLayer.GetAdornerLayer(MyTextBox) is AdornerLayer layer)
            {
                layer.Add(new EzAdoner(MyTextBox));
                layer.Add(new EzAdoner(MyEllipse));
            }
            //if(AdornerLayer.GetAdornerLayer(MyEllipse)is AdornerLayer layer1)
            //{
            //    layer1.Add(new EzAdoner(MyEllipse));
            //}
        }
    }
}
