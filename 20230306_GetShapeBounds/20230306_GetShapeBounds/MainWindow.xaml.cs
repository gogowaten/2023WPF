using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

//ブログ記事
//WPF、Polylineとかの要素をBitmapで取得、ピッタリ収まるサイズで取得、でもまだ不完全 - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2023/03/06/233732


//PolylineのBitmapを作成
//VisualTreeHelper.GetDescendantBoundsを使うと見た目通りのサイズが取得できる
//サイズ関連のプロパティはいくつかあるけど見た目通りではないものが多い
//Width、Height、RenderSize、ActualWidth、ActualHeight、DesiredSize

//RenderTransformを使った回転拡大縮小されているPolylineには未対応
namespace _20230306_GetShapeBounds
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource lineBitmap = MakeBitmap(MyPolyline);
            BitmapSource lineVBitmap = MakeBitmap(MyPolylineV);
        }


        /// <summary>
        /// 指定要素がピッタリ収まるサイズで、見た目通りのBitmapを作成する
        /// </summary>
        /// <param name="element">Polyline以外でもできるはず</param>
        /// <returns></returns>
        private BitmapSource MakeBitmap(FrameworkElement element)
        {
            //ピッタリ収まるサイズ取得
            Size desSize = VisualTreeHelper.GetDescendantBounds(element).Size;

            VisualBrush brush = new(element) { Stretch = Stretch.None };
            DrawingVisual visual = new();
            using (var context = visual.RenderOpen())
            {
                context.DrawRectangle(brush, null, new Rect(desSize));
            }
            //Bitmapのサイズはint型なので+0.5してからintにキャストすることで
            //四捨五入してるけど、+1にして切り上げにするのもいい
            RenderTargetBitmap bitmap = new((int)(desSize.Width + 0.5),
                                            (int)(desSize.Height + 0.5),
                                            96,
                                            96,
                                            PixelFormats.Pbgra32);
            bitmap.Render(visual);
            return bitmap;
        }
    }

}
