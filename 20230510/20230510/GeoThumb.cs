using System;
using System.Collections.Generic;
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

        public PointCollection ThumbAnchors
        {
            get { return (PointCollection)GetValue(ThumbAnchorsProperty); }
            set { SetValue(ThumbAnchorsProperty, value); }
        }
        public static readonly DependencyProperty ThumbAnchorsProperty =
            DependencyProperty.Register(nameof(ThumbAnchors), typeof(PointCollection), typeof(GeoThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Data Data
        {
            get { return (Data)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(Data), typeof(GeoThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ShapeCanvas MyShapeCanvas { get; private set; }

        public GeoThumb()
        {
            MyShapeCanvas = SetTemplate();
            MyShapeCanvas.SetBinding(ShapeCanvas.DataProperty, new Binding() { Source = this, Path = new PropertyPath(DataProperty), Mode = BindingMode.TwoWay });
            Data = new();
            Loaded += GeoThumb_Loaded;

            var mydata = this.Data;
            var shapedata = MyShapeCanvas.Data;

        }

        private void GeoThumb_Loaded(object sender, RoutedEventArgs e)
        {
            var inu = ThumbAnchors;
            if (ThumbAnchors != null)
            {
                Data.AnchorPoints = ThumbAnchors;
            }
            var dd = Data;


            //BindingOperations.SetBinding(this.Data, Data.AnchorPointsProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbAnchorsProperty), Mode = BindingMode.TwoWay });
            var neko = ThumbAnchors;
            //SetBinding(ThumbAnchorsProperty, new Binding() { Source = this.Data, Path = new PropertyPath(Data.AnchorPointsProperty), Mode = BindingMode.TwoWay });
            SetBinding(Data.AnchorPointsProperty, new Binding() { Source = this, Path = new PropertyPath(ThumbAnchorsProperty), Mode = BindingMode.TwoWay });
            var daa = Data;
            var shapedata = MyShapeCanvas.Data.AnchorPoints;
        }

        private ShapeCanvas SetTemplate()
        {
            FrameworkElementFactory factory = new(typeof(ShapeCanvas), "nemo");
            //factory.SetBinding(ShapeCanvas.DataProperty, new Binding() { Source = this, Path = new PropertyPath(DataProperty), Mode = BindingMode.TwoWay });
            this.Template = new() { VisualTree = factory };
            ApplyTemplate();
            return (ShapeCanvas)Template.FindName("nemo", this);
        }
    }
}
