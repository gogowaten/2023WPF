using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace _20230514_GeoShapeThumbDataBinding
{
    public class GeoThumb : Thumb
    {

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
                new FrameworkPropertyMetadata(3.0,
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
        public GeoShape MyShape { get; private set; } = new() { Stroke = Brushes.MediumOrchid };

        public GeoThumb()
        {
            MyTemplateCanvas = SetTemplate();
            MyTemplateCanvas.Children.Add(MyShape);

            TTData = new();
            SetMyBinding5();
        }

        public GeoThumb(Data data) : this()
        {
            TTData = data;
        }



        //4よりこっちのほうが自然な感じ
        private void SetMyBinding5()
        {
            //Source      Target
            //TTStroke <- MyShape.Stroke
            //TTStroke <- TTData.Stroke
            Binding b = new() { Source = this, Path = new PropertyPath(TTStrokeThicknessProperty), Mode = BindingMode.TwoWay };
            MyShape.SetBinding(GeoShape.StrokeThicknessProperty, b);
            BindingOperations.SetBinding(TTData, Data.StrokeWidthProperty, b);

            b = new() { Source = this, Path = new PropertyPath(TTAnchorsProperty), Mode = BindingMode.TwoWay };
            MyShape.SetBinding(GeoShape.AnchorPointsProperty, b);
            BindingOperations.SetBinding(TTData, Data.AnchorPointsProperty, b);
        }

        private void SetMyBinding4()
        {
            //Source    Target
            //TTData <- MyShape
            //TTData <- TTStroke
            Binding b = new() { Source = TTData, Path = new PropertyPath(Data.StrokeWidthProperty), Mode = BindingMode.TwoWay };
            MyShape.SetBinding(GeoShape.StrokeThicknessProperty, b);
            SetBinding(TTStrokeThicknessProperty, b);

            if (TTAnchors != null) { TTData.AnchorPoints = TTAnchors; }
            b = new() { Source = TTData, Path = new PropertyPath(Data.AnchorPointsProperty), Mode = BindingMode.TwoWay };
            MyShape.SetBinding(GeoShape.AnchorPointsProperty, b);
            SetBinding(TTAnchorsProperty, b);
        }
        private void SetMyBinding3()
        {
            Binding b = new() { Source = this, Path = new PropertyPath(TTStrokeThicknessProperty), Mode = BindingMode.TwoWay };
            MyShape.SetBinding(GeoShape.StrokeThicknessProperty, b);
            BindingOperations.SetBinding(TTData, Data.StrokeWidthProperty, b);

            b = new() { Source = this, Path = new PropertyPath(TTAnchorsProperty), Mode = BindingMode.TwoWay };
            MyShape.SetBinding(GeoShape.AnchorPointsProperty, b);
            BindingOperations.SetBinding(TTData, Data.AnchorPointsProperty, b);
        }

        private void SetMyBinding2()
        {
            TTData.AnchorPoints = TTAnchors;
            TTData.StrokeWidth = TTStrokeThickness;

            SetBinding(TTAnchorsProperty, new Binding() { Source = this.TTData, Path = new PropertyPath(Data.AnchorPointsProperty), Mode = BindingMode.TwoWay });
            SetBinding(TTStrokeThicknessProperty, new Binding() { Source = this.TTData, Path = new PropertyPath(Data.StrokeWidthProperty), Mode = BindingMode.TwoWay });


            MyShape.SetBinding(GeoShape.StrokeThicknessProperty, new Binding() { Source = TTData, Path = new PropertyPath(TTStrokeThicknessProperty), Mode = BindingMode.TwoWay });
            MyShape.SetBinding(GeoShape.AnchorPointsProperty, new Binding() { Source = TTData, Path = new PropertyPath(TTAnchorsProperty), Mode = BindingMode.TwoWay });

            var neko = MyShape.AnchorPoints;
            var st = MyShape.StrokeThickness;

        }

        private void SetMyBinding1()
        {
            BindingOperations.SetBinding(TTData, Data.StrokeWidthProperty, new Binding() { Source = this, Path = new PropertyPath(TTStrokeThicknessProperty), Mode = BindingMode.TwoWay });
            BindingOperations.SetBinding(TTData, Data.AnchorPointsProperty, new Binding() { Source = this, Path = new PropertyPath(TTAnchorsProperty), Mode = BindingMode.TwoWay });

            MyShape.SetBinding(GeoShape.StrokeThicknessProperty, new Binding() { Source = TTData, Path = new PropertyPath(TTStrokeThicknessProperty), Mode = BindingMode.TwoWay });
            MyShape.SetBinding(GeoShape.AnchorPointsProperty, new Binding() { Source = TTData, Path = new PropertyPath(TTAnchorsProperty), Mode = BindingMode.TwoWay });

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
