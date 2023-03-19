using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;

//頂点Thumbの表示と移動できるようにした
//BezierではSizeChangedで見た目Rectを更新
//このRectにCanvasのサイズをBinding

//頂点移動時の不具合
//Canvasの座標は固定なので、頂点がマイナス座標になると不具合
//見た目Rectの座標が変化したときも不具合
namespace _20230319_BezierSize3
{
    class BeCanvas : Canvas
    {

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(BeCanvas),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public Bezier MyBezier { get; set; }
        public BeCanvas()
        {
            MyBezier = new()
            {
                Stroke = Brushes.DarkMagenta,
                StrokeThickness = 20,

            };
            Children.Add(MyBezier);

            Loaded += BeCanvas_Loaded;
        }



        private void BeCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            MyBezier.SetBinding(Bezier.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });
            SetBinding(WidthProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectWidth() });
            SetBinding(HeightProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectHeight() });

            //Bezierのオフセット、これはBezier自体で実行もできるけど、ここでした方がいい？
            //さらに、Bindingじゃなくて任意のタイミングで行った方がいい時もありそう
            MyBezier.SetBinding(Canvas.LeftProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectLeft() });
            MyBezier.SetBinding(Canvas.TopProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectTop() });

        }
    }

}
