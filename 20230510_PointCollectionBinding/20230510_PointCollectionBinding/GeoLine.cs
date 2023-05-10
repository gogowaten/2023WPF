using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace _20230510_PointCollectionBinding
{
    [ContentProperty(nameof(Anchors))]
    class GeoLine : Shape
    {

        public PointCollection Anchors
        {
            get { return (PointCollection)GetValue(AnchorsProperty); }
            set { SetValue(AnchorsProperty, value); }
        }
        public static readonly DependencyProperty AnchorsProperty =
            DependencyProperty.Register(nameof(Anchors), typeof(PointCollection), typeof(GeoLine),
                new FrameworkPropertyMetadata(new PointCollection() { new Point(0, 0), new Point(100, 100) },
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    context.BeginFigure(Anchors[0], false, false);
                    context.PolyLineTo(Anchors.Skip(1).ToList(), true, false);
                }
                geometry.Freeze();
                return geometry;
            }
        }
    }
}
