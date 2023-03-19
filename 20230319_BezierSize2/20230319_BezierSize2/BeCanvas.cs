using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;

namespace _20230319_BezierSize2
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
            MyBezier.SizeChanged += MyBezier_SizeChanged;
        }


        private void MyBezier_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //UpdateMyRect();
        }
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            //UpdateMyRect();
            return base.ArrangeOverride(arrangeSize);
        }
        protected override Size MeasureOverride(Size constraint)
        {
            //UpdateMyRect();
            return base.MeasureOverride(constraint);
        }

        /// <summary>
        /// 表示しているShapeBezierのRectを取得して、自身Canvasに反映させる
        /// </summary>
        //private void UpdateMyRect()
        //{
        //    SetMyBoundsRect(GetOffsetBounds());
        //}

        private Rect GetOffsetBounds()
        {
            var bounds = VisualTreeHelper.GetDescendantBounds(MyBezier);

            if (bounds.Width == Width
                && bounds.Height == Height
                && bounds.Left == GetLeft(this)
                && bounds.Top == GetTop(this))
            { return Rect.Empty; }
            else return bounds;
        }
        private void SetMyBoundsRect(Rect desbounds)
        {
            if (desbounds.IsEmpty) { return; }

            SetLeft(MyBezier, -desbounds.Left);
            SetTop(MyBezier, -desbounds.Top);
            Width = desbounds.Width;
            Height = desbounds.Height;
        }
        private void BeCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            MyBezier.SetBinding(Bezier.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });
            SetBinding(WidthProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectWidth() });
            SetBinding(HeightProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectHeight() });

            //Bezierのオフセット、これはBezier自体で実行もできるけど、ここでした方がいい？
            MyBezier.SetBinding(Canvas.LeftProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectLeft() });
            MyBezier.SetBinding(Canvas.TopProperty, new Binding() { Source = MyBezier, Path = new PropertyPath(Bezier.MyExternalBoundsProperty), Converter = new MyConverterRectTop() });

        }
    }

}
