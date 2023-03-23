using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Documents;

namespace _20230323_BezierSize
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

        /// <summary>
        /// PointCollectionのRectを返す
        /// </summary>
        /// <param name="pt">PointCollection</param>
        /// <returns></returns>
        public static Rect GetPointsRect(PointCollection pt)
        {
            if (pt.Count == 0) return new Rect();
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            foreach (var item in pt)
            {
                if (minX > item.X) minX = item.X;
                if (minY > item.Y) minY = item.Y;
                if (maxX < item.X) maxX = item.X;
                if (maxY < item.Y) maxY = item.Y;
            }
            return new Rect(minX, minY, maxX - minX, maxY - minY);
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

        //できた！！！！！！！！！！！！！！
        //マウス移動後に実行で、CanvasとBezierの座標修正する
        //使う場面は
        //頂点ThumbのDragDeltaでは座標修正なしで動かして
        //頂点ThumbのDragCompletedイベントで使う想定
        public void Fix0Point2()
        {
            Rect pts = GetPointsRect(MyPoints);

            Fix0Point();

            var canRect = VisualTreeHelper.GetDescendantBounds(this);
            var cLocate = VisualTreeHelper.GetOffset(this);
            var bezRect = VisualTreeHelper.GetDescendantBounds(MyBezier);
            var bLocate = VisualTreeHelper.GetOffset(MyBezier);

            SetLeft(this, cLocate.X + bLocate.X + bezRect.X);
            SetTop(this, cLocate.Y + bLocate.Y + bezRect.Y);

            var xxx = -bezRect.X + pts.X;
            var yyy = -bezRect.Y + pts.Y;
            SetLeft(MyBezier, xxx);
            SetTop(MyBezier, yyy);
        }



        //できた！！！！！！！！！！！！！！！！！！！！！！
        //マウスで移動中の計算でもできた
        //使う場面は
        //頂点ThumbのDragDeltaイベントなどで図形が変化しているとき
        public void FixCanvasLocate00()
        {
            var bezExRect = MyBezier.MyExternalBounds;
            if (bezExRect.IsEmpty) { return; }
            //var canRect = VisualTreeHelper.GetDescendantBounds(this);

            //自身Canvasの座標修正、
            //新しい座標 = 自身の座標＋(図形の座標＋Rect座標)
            var bezLocate = VisualTreeHelper.GetOffset(MyBezier);
            var xDiff = bezLocate.X + bezExRect.Left;
            var yDiff = bezLocate.Y + bezExRect.Top;
            var myLocate = VisualTreeHelper.GetOffset(this);
            SetLeft(this, myLocate.X + xDiff);
            SetTop(this, myLocate.Y + yDiff);

            //図形の座標修正、
            //新しい座標 = 図形の座標 - (図形の座標＋Rect座標) + PointsのRect座標
            var ptsRect = GetPointsRect(MyPoints);
            SetLeft(MyBezier, bezLocate.X - xDiff + ptsRect.X);
            SetTop(MyBezier, bezLocate.Y - yDiff + ptsRect.Y);

            //PointsのRect座標を0,0に修正
            Fix0Point();
            //頂点Thumbの座標修正、Pointsに合わせる
            MyBezier.MyAdorner?.FixThumbsLocate();
        }


        protected override Size MeasureOverride(Size constraint)
        {
            //編集状態のときだけ座標修正する(常に修正にするとデザイナー画面での位置がずれる)
            //実際に使うときは頂点Thumbの表示と連動することになる
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


            ////Bezierのオフセット、これはBezier自体で実行もできるけど、ここでした方がいい？
            ////さらに、Bindingじゃなくて任意のタイミングで行った方がいい時もありそう
            //MyBezier.SetBinding(Canvas.LeftProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectLeft() });
            //MyBezier.SetBinding(Canvas.TopProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectTop() });

            //SetBinding(Canvas.LeftProperty,new Binding() { Source = MyBezier,Path=new PropertyPath(Bezier.MyExternalBoundsProperty),Converter = new MyConverterRectLeft2() });
            //SetBinding(Canvas.TopProperty,new Binding() { Source = MyBezier,Path=new PropertyPath(Bezier.MyExternalBoundsProperty),Converter = new MyConverterRectTop2() });

        }
    }

}
