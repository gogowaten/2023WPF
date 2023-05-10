using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;

namespace _20230510
{
    public class ShapeCanvas : Canvas
    {

        public Data Data
        {
            get { return (Data)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(Data), typeof(ShapeCanvas),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        //public GeoShape CanvasContent
        //{
        //    get { return (GeoShape)GetValue(GeoShapeProperty); }
        //    set { SetValue(GeoShapeProperty, value); }
        //}
        //public static readonly DependencyProperty GeoShapeProperty =
        //    DependencyProperty.Register(nameof(CanvasContent), typeof(GeoShape), typeof(ShapeCanvas),
        //        new FrameworkPropertyMetadata(null,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public GeoShape CanvasContent { get; set; }

        public ShapeCanvas()
        {
            CanvasContent = new GeoShape();
            CanvasContent.SetBinding(GeoShape.AnchorsProperty, new Binding() { Source = this.Data, Path = new PropertyPath(Data.AnchorPointsProperty), Mode = BindingMode.TwoWay });
            CanvasContent.Stroke = Brushes.Gray;
            CanvasContent.StrokeThickness = 1;
            Children.Add(CanvasContent);
        }
    }
}
