using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace _20230303
{
    public class PolyBezierCanvas : Canvas
    {

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(PolyBezierCanvas),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public PolyBezier MyPolyBezier { get; private set; }
        public Rect ExBound { get; private set; }
        public Rectangle ExRectangle { get; private set; }

        public PolyBezierCanvas()
        {
            MyPolyBezier = new()
            {
                Stroke = Brushes.MediumAquamarine,
                StrokeThickness = 20,

            };
            Children.Add(MyPolyBezier);
            ExRectangle = new Rectangle()
            {
                Stroke = Brushes.ForestGreen,
                StrokeThickness = 0,
            };
            Children.Add(ExRectangle);
            MyPolyBezier.SetBinding(PolyBezier.PointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty) });


            Loaded += PolyBezierCanvas_Loaded;
            MyPolyBezier.SizeChanged += MyPolyBezier_SizeChanged;

        }

        private void MyPolyBezier_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var neko = VisualTreeHelper.GetDescendantBounds(this);
            ExBound = VisualTreeHelper.GetDescendantBounds(this);
            ExRectangle.Width = ExBound.Width;
            ExRectangle.Height = ExBound.Height;
            Canvas.SetLeft(ExRectangle, ExBound.Left);
            Canvas.SetTop(ExRectangle, ExBound.Top);
        }

        private void PolyBezierCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            if (AdornerLayer.GetAdornerLayer(MyPolyBezier) is AdornerLayer layer)
            {
                layer.Add(new CCAdor(MyPolyBezier));
            }
        }
    }
}
