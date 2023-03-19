using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;

namespace _20230319_BezierSize
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
            MyBezier.LayoutUpdated += MyBezier_LayoutUpdated;
        }

        private void MyBezier_LayoutUpdated(object? sender, EventArgs e)
        {
            var desbounds = VisualTreeHelper.GetDescendantBounds(MyBezier);
            if (desbounds.IsEmpty) { return; }
            if (desbounds.Width == Width && desbounds.Height == Height) { return; }
            SetLeft(MyBezier, -desbounds.Left);
            SetTop(MyBezier, -desbounds.Top);
            Width = desbounds.Width;
            Height = desbounds.Height;
        }

        private void MyBezier_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var desbounds = VisualTreeHelper.GetDescendantBounds(MyBezier);
            var offset = VisualTreeHelper.GetOffset(MyBezier);
            //Measure(desbounds.Size);

        }

        private void BeCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            MyBezier.SetBinding(Bezier.MyPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });
            //SetBinding(WidthProperty,new Binding() { Source=MyBezier,Path=new PropertyPath(ActualWidthProperty) });
            //SetBinding(HeightProperty,new Binding() { Source=MyBezier,Path=new PropertyPath(ActualHeightProperty) });

            //var desbounds = VisualTreeHelper.GetDescendantBounds(MyBezier);
            //if( desbounds.IsEmpty) { return; }
            //SetLeft(MyBezier, -desbounds.Left);
            //SetTop(MyBezier, -desbounds.Top);
            //Width = desbounds.Width;
            //Height = desbounds.Height;
        }
    }
}
