using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Ink;
using System.Windows.Markup;
using System.Windows.Controls;

namespace _20230510
{
    //    [ContentProperty(nameof(AnchorPoints))]
    public class GeoShape : Shape
    {
        //public ObservableCollection<Point> AnchorPoints
        //{
        //    get { return (ObservableCollection<Point>)GetValue(AnchorPointsProperty); }
        //    set { SetValue(AnchorPointsProperty, value); }
        //}
        //public static readonly DependencyProperty AnchorPointsProperty =
        //    DependencyProperty.Register(nameof(AnchorPoints), typeof(ObservableCollection<Point>), typeof(GeoShape),
        //        new FrameworkPropertyMetadata(new ObservableCollection<Point>(),
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure |
        //            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public PointCollection AnchorPoints
        {
            get { return (PointCollection)GetValue(AnchorPointsProperty); }
            set { SetValue(AnchorPointsProperty, value); }
        }
        public static readonly DependencyProperty AnchorPointsProperty =
            DependencyProperty.Register(nameof(AnchorPoints), typeof(PointCollection), typeof(GeoShape),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        protected override Geometry DefiningGeometry
        {
            get
            {
                //Stroke = Brushes.Black;
                if (AnchorPoints == null || AnchorPoints.Count < 2) return Geometry.Empty;
                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    context.BeginFigure(AnchorPoints[0], false, false);
                    context.PolyLineTo(AnchorPoints.Skip(1).ToList(), true, false);
                }
                geometry.Freeze();
                return geometry;
            }
        }
    }
}
