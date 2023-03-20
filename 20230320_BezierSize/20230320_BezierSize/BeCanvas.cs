using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;

namespace _20230320_BezierSize
{
    public class BeCanvas : Canvas
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


        public bool MyIsEditing
        {
            get { return (bool)GetValue(MyIsEditingProperty); }
            set { SetValue(MyIsEditingProperty, value); }
        }
        public static readonly DependencyProperty MyIsEditingProperty =
            DependencyProperty.Register(nameof(MyIsEditing), typeof(bool), typeof(BeCanvas),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));



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

        public void FixBezierLocate()
        {
            var bounds = MyBezier.MyExternalBounds;
            Canvas.SetLeft(MyBezier, -bounds.Left);
            Canvas.SetTop(MyBezier, -bounds.Top);
        }
        public void FixCanvasLocate()
        {
            var bounds = MyBezier.MyExternalBounds;
            if(bounds.IsEmpty) { return; }
            var bezierLocate = VisualTreeHelper.GetOffset(MyBezier);
            var canvasLocate = VisualTreeHelper.GetOffset(this);
            var xDiff = bezierLocate.X + bounds.Left;
            var yDiff = bezierLocate.Y + bounds.Top;
            var x = canvasLocate.X + xDiff;
            var y = canvasLocate.Y + yDiff;
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
            var xx = bezierLocate.X - xDiff;
            var yy = bezierLocate.Y - yDiff;
            Canvas.SetLeft(MyBezier, xx);
            Canvas.SetTop(MyBezier, yy);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (MyIsEditing) { FixCanvasLocate(); }
            
            return base.MeasureOverride(constraint);
        }
        private void BeCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            MyBezier.SetBinding(Bezier.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });
            SetBinding(WidthProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectWidth() });
            SetBinding(HeightProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectHeight() });

            UpdateLayout();
            FixBezierLocate();
            //FixCanvasLocate();

            ////Bezierのオフセット、これはBezier自体で実行もできるけど、ここでした方がいい？
            ////さらに、Bindingじゃなくて任意のタイミングで行った方がいい時もありそう
            //MyBezier.SetBinding(Canvas.LeftProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectLeft() });
            //MyBezier.SetBinding(Canvas.TopProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectTop() });

            //SetBinding(Canvas.LeftProperty,new Binding() { Source = MyBezier,Path=new PropertyPath(Bezier.MyExternalBoundsProperty),Converter = new MyConverterRectLeft2() });
            //SetBinding(Canvas.TopProperty,new Binding() { Source = MyBezier,Path=new PropertyPath(Bezier.MyExternalBoundsProperty),Converter = new MyConverterRectTop2() });

        }
    }


}
