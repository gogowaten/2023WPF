using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls.Primitives;

//PointCollectionにPointが追加されたらマーカーThumbも自動で追加したい
namespace _20230510
{
    public class ShapeCanvas : Canvas
    {
        #region 依存関係プロパティ


        //public PointCollection Anchors
        //{
        //    get { return (PointCollection)GetValue(AnchorsProperty); }
        //    set { SetValue(AnchorsProperty, value); }
        //}
        //public static readonly DependencyProperty AnchorsProperty =
        //    DependencyProperty.Register(nameof(Anchors), typeof(PointCollection), typeof(ShapeCanvas),
        //        new FrameworkPropertyMetadata(null,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [TypeConverter(typeof(MyTypeConverterPoints))]
        public ObservableCollection<Point> Anchors
        {
            get { return (ObservableCollection<Point>)GetValue(AnchorsProperty); }
            set { SetValue(AnchorsProperty, value); }
        }
        public static readonly DependencyProperty AnchorsProperty =
            DependencyProperty.Register(nameof(Anchors), typeof(ObservableCollection<Point>), typeof(ShapeCanvas),
                new FrameworkPropertyMetadata(new ObservableCollection<Point>(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(ShapeCanvas),
                new FrameworkPropertyMetadata(5.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(ShapeCanvas),
                new FrameworkPropertyMetadata(Brushes.MediumAquamarine,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存関係プロパティ

        public GeoShape MyShape { get; private set; } = new GeoShape();
        
        public ShapeCanvas()
        {
            Children.Add(MyShape);
            Anchors.CollectionChanged += Anchors_CollectionChanged;
        
            MyShape.SetBinding(GeoShape.AnchorPointsProperty, new Binding() { Source = this, Path = new PropertyPath(AnchorsProperty), Mode = BindingMode.TwoWay });
            MyShape.SetBinding(GeoShape.StrokeProperty, new Binding() { Source = this, Path = new PropertyPath(StrokeProperty), Mode = BindingMode.TwoWay });
            MyShape.SetBinding(GeoShape.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(StrokeThicknessProperty), Mode = BindingMode.TwoWay });

            
            Clip = new RectangleGeometry(new Rect(0, 0, 100, 100));
        }

        private void Points_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }

        
        private void Anchors_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }
    }

    public class MyTypeConverterPoints : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value == null) return null;
            if (value is string str)
            {
                string[] ss = str.Split(' ');
                ObservableCollection<Point> points = new();
                foreach (var item in ss)
                {
                    string[] xy = item.Split(',');
                    if (double.TryParse(xy[0], out double x) && double.TryParse(xy[1], out double y))
                    {
                        Point point = new(x, y);
                        points.Add(point);
                    }
                }
                return points;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
