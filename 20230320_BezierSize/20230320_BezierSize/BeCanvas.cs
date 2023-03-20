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

        //Pointsの左上を0,0にするだけ
        public void Fix0Point()
        {
            Rect r = MyAdorner.GetPointsRect(MyPoints);
            for (int i = 0; i < MyPoints.Count; i++)
            {
                Point pp = MyPoints[i];
                MyPoints[i] = new Point(pp.X - r.Left, pp.Y - r.Top);
            }
        }

        public void FixBezierLocate()
        {
            var bounds = MyBezier.MyExternalBounds;
            Canvas.SetLeft(MyBezier, -bounds.Left);
            Canvas.SetTop(MyBezier, -bounds.Top);
        }
        public void FixCanvasLocate2()
        {
            //CanvasとBezierのオフセット
            var adRect = VisualTreeHelper.GetDescendantBounds(MyBezier.MyAdorner);
            var beRect = VisualTreeHelper.GetDescendantBounds(MyBezier);
            var myRect = VisualTreeHelper.GetDescendantBounds(this);
            var adOffset = VisualTreeHelper.GetOffset(MyBezier.MyAdorner);
            var beOffset = VisualTreeHelper.GetOffset(MyBezier);
            var myOffset = VisualTreeHelper.GetOffset(this);
            var myp = MyPoints;

            if (adRect.X < 0)
            {
                var x = beOffset.X + adRect.X;
                Canvas.SetLeft(MyBezier, x);
                Fix0Point();
                MyBezier.MyAdorner?.FixThumbsLocate();
            }

            if (myRect.X < 0)
            {
                var xxxx = adRect.X - beRect.X;
                Canvas.SetLeft(MyBezier, xxxx);
                Fix0Point();
                MyBezier.MyAdorner?.FixThumbsLocate();
                var x = myOffset.X + myRect.X;
                Canvas.SetLeft(this, x);
            }
            
            if (adRect.Y < 0)
            {
                var y = beOffset.Y + adRect.Y;
                Canvas.SetTop(MyBezier, y);
                Fix0Point();
                MyBezier.MyAdorner?.FixThumbsLocate();
            }

            if (myRect.Y < 0)
            {
                var yyyy = adRect.Y - beRect.Y;
                Canvas.SetTop(MyBezier, yyyy);
                Fix0Point();
                MyBezier.MyAdorner?.FixThumbsLocate();
                var y = myOffset.Y + myRect.Y;
                Canvas.SetTop(this, y);
            }
            

        }
        public void MyOffset(Vector offset)
        {
            var x = Canvas.GetLeft(this);
            var y = Canvas.GetTop(this);
            var xx = x + offset.X;
            var yy = y + offset.Y;
            Canvas.SetLeft(this, xx);
            Canvas.SetTop(this, yy);
        }
        public void FixCanvasLocate()
        {
            var exBezier = MyBezier.MyExternalBounds;
            if (exBezier.IsEmpty) { return; }
            var canrect = VisualTreeHelper.GetDescendantBounds(this);
            var bezier = VisualTreeHelper.GetOffset(MyBezier);
            var xDiff = bezier.X + exBezier.Left;
            var yDiff = bezier.Y + exBezier.Top;

            var xx = bezier.X - xDiff;
            var yy = bezier.Y - yDiff;
            Canvas.SetLeft(MyBezier, xx);
            Canvas.SetTop(MyBezier, yy);

            var myLocate = VisualTreeHelper.GetOffset(this);
            var x = myLocate.X + xDiff;
            var y = myLocate.Y + yDiff;
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
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
