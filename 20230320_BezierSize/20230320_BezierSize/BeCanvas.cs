using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Security.Cryptography.X509Certificates;

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

        //できた！！！！！！！！！！！！！！
        //けど、移動中に適用するとすっ飛ぶ、使うときは移動後
        public void Fix0Point2()
        {
            Rect pts = MyAdorner.GetPointsRect(MyPoints);
            for (int i = 0; i < MyPoints.Count; i++)
            {
                Point pp = MyPoints[i];
                MyPoints[i] = new Point(pp.X - pts.Left, pp.Y - pts.Top);
            }
            var canRect = VisualTreeHelper.GetDescendantBounds(this);
            var cLocate = VisualTreeHelper.GetOffset(this);
            var bezRect = VisualTreeHelper.GetDescendantBounds(MyBezier);
            var bLocate = VisualTreeHelper.GetOffset(MyBezier);

            //var x = cLocate.X + canRect.X;
            var cx = bLocate.X + bezRect.X;
            //var cxx = x + cx;
            var cxxx = cLocate.X + cx;
            SetLeft(this, cxxx);
            SetTop(this, cLocate.Y + bLocate.Y + bezRect.Y);

            //var bx = bLocate.X + bezRect.X - canRect.X;
            //var by = bLocate.Y + bezRect.Y - canRect.Y;
            //bx = bLocate.X + pts.X-canRect.X;
            //var by = bLocate.Y + pts.Y;
            var xxx = -bezRect.X + pts.X;
            //var xxxx = bLocate.X + pts.X;
            var yyy = -bezRect.Y + pts.Y;
            SetLeft(MyBezier, xxx);
            SetTop(MyBezier, yyy);
        }

        public void FixBezierLocate()
        {
            var bounds = MyBezier.MyExternalBounds;
            Canvas.SetLeft(MyBezier, -bounds.Left);
            Canvas.SetTop(MyBezier, -bounds.Top);
        }

        //頂点移動中にオフセット処理していない場合の修正オフセット
        public void FixCanvasLocate2()
        {
            //CanvasとBezierのオフセット
            var adoRect = VisualTreeHelper.GetDescendantBounds(MyBezier.MyAdorner);
            var bezRect = VisualTreeHelper.GetDescendantBounds(MyBezier);
            var canRect = VisualTreeHelper.GetDescendantBounds(this);
            var adoOffset = VisualTreeHelper.GetOffset(MyBezier.MyAdorner);
            var bezOffset = VisualTreeHelper.GetOffset(MyBezier);
            var canOffset = VisualTreeHelper.GetOffset(this);
            //var bezExRect = MyBezier.MyExternalBounds;
            var ptRect = MyAdorner.GetPointsRect(MyPoints);

            //Bezierのオフセット、PointsのRectの座標が0,0以外ならオフセットが必要
            if (ptRect.X != 0)
            {
                SetLeft(MyBezier, ptRect.X - canRect.X + bezOffset.X);
                Fix0Point();
                MyBezier.MyAdorner?.FixThumbsLocate();
            }
            if (ptRect.Y != 0)
            {
                SetTop(MyBezier, ptRect.Y - canRect.Y + bezOffset.Y);
                Fix0Point();
                MyBezier.MyAdorner?.FixThumbsLocate();
            }

            //Canvasのオフセット
            if (canRect.X < 0)
            {
                SetLeft(this, canOffset.X + canRect.X);
            }
            if (canRect.Y < 0)
            {
                SetTop(this, canOffset.Y + canRect.Y);
            }

        }
        public void FixCanvasLocate3()
        {
            //CanvasとBezierのオフセット
            var adoRect = VisualTreeHelper.GetDescendantBounds(MyBezier.MyAdorner);
            var adoOffset = VisualTreeHelper.GetOffset(MyBezier.MyAdorner);
            var bezRect = VisualTreeHelper.GetDescendantBounds(MyBezier);
            var bezOffset = VisualTreeHelper.GetOffset(MyBezier);
            var canRect = VisualTreeHelper.GetDescendantBounds(this);
            var canOffset = VisualTreeHelper.GetOffset(this);
            //var bezExRect = MyBezier.MyExternalBounds;
            var ptRect = MyAdorner.GetPointsRect(MyPoints);
            var xDiff = bezOffset.X + bezRect.X;
            var yDiff = bezOffset.Y + bezRect.Y;
            if (xDiff != 0)
            {
                var neko = 0;
            }
            //var x = canLocate.X + canRect.X; SetLeft(this, x);
            //var y = canLocate.Y + canRect.Y; SetTop(this, y);

            //Bezierのオフセット、PointsのRectの座標が0,0以外ならオフセットが必要
            //var xx0 = ptRect.X + bezLocate.X - canRect.X;
            //SetLeft(MyBezier, xx0); Fix0Point(); MyBezier.MyAdorner?.FixThumbsLocate();
            //var yy = ptRect.Y + bezLocate.Y - canRect.Y;
            //SetTop(MyBezier, yy);Fix0Point();MyBezier.MyAdorner?.FixThumbsLocate();

            var x = canOffset.X + xDiff; SetLeft(this, x);
            var y = canOffset.Y + yDiff; SetTop(this, y);
            var xx = ptRect.X + bezOffset.X - canRect.X - xDiff;
            var xxx = bezOffset.X - xDiff;
            SetLeft(MyBezier, xx); Fix0Point(); MyBezier.MyAdorner?.FixThumbsLocate();
            var yy = ptRect.Y + bezOffset.Y - canRect.Y - yDiff;
            var yyy = bezOffset.Y - yDiff;
            SetTop(MyBezier, yy); Fix0Point(); MyBezier.MyAdorner?.FixThumbsLocate();


        }

        //リアルタイム、Canvas座標のプラス方向の処理ができていない
        //それ以外はできているので、あと少し？
        public void FixCanvasLocate01()
        {
            //var bezExRect = MyBezier.MyExternalBounds;
            //if (bezExRect.IsEmpty) { return; }
            var canLocate = VisualTreeHelper.GetOffset(this);
            var canRect = VisualTreeHelper.GetDescendantBounds(this);
            var bezLocate = VisualTreeHelper.GetOffset(MyBezier);
            var bezRect = VisualTreeHelper.GetDescendantBounds(MyBezier);
            var ptsRect = MyAdorner.GetPointsRect(MyPoints);

            var xDiff = bezLocate.X + bezRect.Left;
            var yDiff = bezLocate.Y + bezRect.Top;

            //var myLocate = VisualTreeHelper.GetOffset(this);
            SetLeft(this, canLocate.X + xDiff);
            SetTop(this, canLocate.Y + yDiff);

            var xx0 = ptsRect.X - canRect.X;
            var xx1 = ptsRect.X + canRect.X;
            var xx2 = ptsRect.X - bezRect.X;
            var xx3 = ptsRect.X + bezRect.X;
            var xx4 = bezRect.X - ptsRect.X;
            var xx5 = bezRect.X + canRect.X;
            var xx6 = bezRect.X - canRect.X;
            var xx7 = canRect.X - bezRect.X;
            var xx8 = canRect.X - ptsRect.X;
            var yy0 = ptsRect.Y - canRect.Y;
            SetLeft(MyBezier, bezLocate.X + xx0);
            SetTop(MyBezier, bezLocate.Y + yy0);

            Fix0Point();
            MyBezier.MyAdorner?.FixThumbsLocate();
            if (bezLocate.X + bezRect.X != 0)
            {
                SetLeft(MyBezier, -bezRect.X);
            }
            if (bezLocate.Y + bezRect.Y != 0)
            {
                SetTop(MyBezier, -bezRect.Y);
            }
        }

        //CanvasとBezierのオフセットはできているけど、PointsのFix0ができていない
        public void FixCanvasLocate0()
        {
            var bezExRect = MyBezier.MyExternalBounds;
            if (bezExRect.IsEmpty) { return; }
            var canRect = VisualTreeHelper.GetDescendantBounds(this);
            var ptsRect = MyAdorner.GetPointsRect(MyPoints);

            var bezOffset = VisualTreeHelper.GetOffset(MyBezier);
            var xDiff = bezOffset.X + bezExRect.Left;
            var yDiff = bezOffset.Y + bezExRect.Top;

            SetLeft(MyBezier, bezOffset.X - xDiff);
            SetTop(MyBezier, bezOffset.Y - yDiff);

            var myLocate = VisualTreeHelper.GetOffset(this);
            SetLeft(this, myLocate.X + xDiff);
            SetTop(this, myLocate.Y + yDiff);

        }

        //できた！！！！！！！！！！！！！！！！！！！！！！
        //マウスで移動中の計算でもできた
        public void FixCanvasLocate00()
        {
            var bezExRect = MyBezier.MyExternalBounds;
            if (bezExRect.IsEmpty) { return; }
            var canRect = VisualTreeHelper.GetDescendantBounds(this);
            var ptsRect = MyAdorner.GetPointsRect(MyPoints);

            var bezOffset = VisualTreeHelper.GetOffset(MyBezier);
            var xDiff = bezOffset.X + bezExRect.Left;
            var yDiff = bezOffset.Y + bezExRect.Top;

            var myLocate = VisualTreeHelper.GetOffset(this);
            SetLeft(this, myLocate.X + xDiff);
            SetTop(this, myLocate.Y + yDiff);

            SetLeft(MyBezier, bezOffset.X - xDiff + ptsRect.X);
            SetTop(MyBezier, bezOffset.Y - yDiff + ptsRect.Y);
            Fix0Point();
            MyBezier.MyAdorner?.FixThumbsLocate();

        }


        protected override Size MeasureOverride(Size constraint)
        {
            //if (MyIsEditing) { Fix0Point2(); }
            if (MyIsEditing) { FixCanvasLocate00(); }

            return base.MeasureOverride(constraint);
        }
        private void BeCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            MyBezier.SetBinding(Bezier.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });

            SetBinding(WidthProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectWidth() });
            SetBinding(HeightProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectHeight() });

            UpdateLayout();
            FixBezierLocate();
            //FixCanvasLocate0();

            ////Bezierのオフセット、これはBezier自体で実行もできるけど、ここでした方がいい？
            ////さらに、Bindingじゃなくて任意のタイミングで行った方がいい時もありそう
            //MyBezier.SetBinding(Canvas.LeftProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectLeft() });
            //MyBezier.SetBinding(Canvas.TopProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectTop() });

            //SetBinding(Canvas.LeftProperty,new Binding() { Source = MyBezier,Path=new PropertyPath(Bezier.MyExternalBoundsProperty),Converter = new MyConverterRectLeft2() });
            //SetBinding(Canvas.TopProperty,new Binding() { Source = MyBezier,Path=new PropertyPath(Bezier.MyExternalBoundsProperty),Converter = new MyConverterRectTop2() });

        }
    }


}
