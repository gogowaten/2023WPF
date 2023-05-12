using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace _20230510
{
    public class GeoThumb : Thumb
    {

        //public ObservableCollection<Point> TTAnchors
        //{
        //    get { return (ObservableCollection<Point>)GetValue(TTAnchorsProperty); }
        //    set { SetValue(TTAnchorsProperty, value); }
        //}
        //public static readonly DependencyProperty TTAnchorsProperty =
        //    DependencyProperty.Register(nameof(TTAnchors), typeof(ObservableCollection<Point>), typeof(GeoThumb),
        //        new FrameworkPropertyMetadata(null,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public PointCollection TTAnchors
        {
            get { return (PointCollection)GetValue(TTAnchorsProperty); }
            set { SetValue(TTAnchorsProperty, value); }
        }
        public static readonly DependencyProperty TTAnchorsProperty =
            DependencyProperty.Register(nameof(TTAnchors), typeof(PointCollection), typeof(GeoThumb),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double TTStrokeThickness
        {
            get { return (double)GetValue(TTStrokeThicknessProperty); }
            set { SetValue(TTStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty TTStrokeThicknessProperty =
            DependencyProperty.Register(nameof(TTStrokeThickness), typeof(double), typeof(GeoThumb),
                new FrameworkPropertyMetadata(1.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Data TTData
        {
            get { return (Data)GetValue(TTDataProperty); }
            set { SetValue(TTDataProperty, value); }
        }
        public static readonly DependencyProperty TTDataProperty =
            DependencyProperty.Register(nameof(TTData), typeof(Data), typeof(GeoThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Canvas MyTemplateCanvas { get; private set; }
        public GeoShape MyContentGeoShape { get; private set; } = new();

        public GeoThumb()
        {
            MyTemplateCanvas = SetTemplate();
            MyTemplateCanvas.Children.Add(MyContentGeoShape);

            TTData = new();
            Loaded += GeoThumb_Loaded;

            var mydata = this.TTData;

        }

        private void GeoThumb_Loaded(object sender, RoutedEventArgs e)
        {
            SetMyBinding3();
        }

        private void SetMyBinding3()
        {
           MyContentGeoShape.SetBinding(GeoShape.StrokeThicknessProperty, new Binding() { Source = this, Path = new PropertyPath(TTStrokeThicknessProperty), Mode = BindingMode.TwoWay });
            MyContentGeoShape.SetBinding(GeoShape.AnchorPointsProperty, new Binding() { Source = this, Path = new PropertyPath(TTAnchorsProperty), Mode = BindingMode.TwoWay });
            
        }

        private void SetMyBinding2()
        {
            TTData.AnchorPoints = TTAnchors;
            TTData.StrokeWidth = TTStrokeThickness;

            SetBinding(TTAnchorsProperty, new Binding() { Source = this.TTData, Path = new PropertyPath(Data.AnchorPointsProperty), Mode = BindingMode.TwoWay });
            SetBinding(TTStrokeThicknessProperty, new Binding() { Source = this.TTData, Path = new PropertyPath(Data.StrokeWidthProperty), Mode = BindingMode.TwoWay });


            MyContentGeoShape.SetBinding(GeoShape.StrokeThicknessProperty, new Binding() { Source = TTData, Path = new PropertyPath(TTStrokeThicknessProperty), Mode = BindingMode.TwoWay });
            MyContentGeoShape.SetBinding(GeoShape.AnchorPointsProperty, new Binding() { Source = TTData, Path = new PropertyPath(TTAnchorsProperty), Mode = BindingMode.TwoWay });

            var neko = MyContentGeoShape.AnchorPoints;
            var st = MyContentGeoShape.StrokeThickness;

        }

        private void SetMyBinding1()
        {
            BindingOperations.SetBinding(TTData, Data.StrokeWidthProperty, new Binding() { Source = this, Path = new PropertyPath(TTStrokeThicknessProperty), Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(TTData, Data.AnchorPointsProperty, new Binding() { Source = this, Path = new PropertyPath(TTAnchorsProperty), Mode = BindingMode.TwoWay });

            MyContentGeoShape.SetBinding(GeoShape.StrokeThicknessProperty, new Binding() { Source = TTData, Path = new PropertyPath(TTStrokeThicknessProperty), Mode = BindingMode.TwoWay });
            MyContentGeoShape.SetBinding(GeoShape.AnchorPointsProperty, new Binding() { Source = TTData, Path = new PropertyPath(TTAnchorsProperty), Mode = BindingMode.TwoWay });

        }

        private Canvas SetTemplate()
        {
            FrameworkElementFactory factory = new(typeof(Canvas), "nemo");
            this.Template = new() { VisualTree = factory };
            ApplyTemplate();
            return (Canvas)Template.FindName("nemo", this);
        }






    }
}
