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
    [ContentProperty(nameof(Anchors))]
    public class GeoShape : Shape
    {

        public PointCollection Anchors
        {
            get { return (PointCollection)GetValue(AnchorsProperty); }
            set { SetValue(AnchorsProperty, value); }
        }
        public static readonly DependencyProperty AnchorsProperty =
            DependencyProperty.Register(nameof(Anchors), typeof(PointCollection), typeof(GeoShape),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        protected override Geometry DefiningGeometry
        {
            get
            {
                StrokeThickness = 10;Stroke = Brushes.Black;
                if (Anchors==null|| Anchors.Count < 2) return Geometry.Empty;
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
