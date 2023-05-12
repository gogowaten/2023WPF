using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace _20230510
{
    public class Data : DependencyObject
    {
        public Data()
        {
            
        }


        //public ObservableCollection<Point> AnchorPoints
        //{
        //    get { return (ObservableCollection<Point>)GetValue(AnchorPointsProperty); }
        //    set { SetValue(AnchorPointsProperty, value); }
        //}
        //public static readonly DependencyProperty AnchorPointsProperty =
        //    DependencyProperty.Register(nameof(AnchorPoints), typeof(ObservableCollection<Point>), typeof(Data),
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
            DependencyProperty.Register(nameof(AnchorPoints), typeof(PointCollection), typeof(Data),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }
        public static readonly DependencyProperty StrokeWidthProperty =
            DependencyProperty.Register(nameof(StrokeWidth), typeof(double), typeof(Data),
                new FrameworkPropertyMetadata(2.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int TTX
        {
            get { return (int)GetValue(TTXProperty); }
            set { SetValue(TTXProperty, value); }
        }
        public static readonly DependencyProperty TTXProperty =
            DependencyProperty.Register(nameof(TTX), typeof(int), typeof(Data),
                new FrameworkPropertyMetadata(0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int TTY
        {
            get { return (int)GetValue(TTYProperty); }
            set { SetValue(TTYProperty, value); }
        }
        public static readonly DependencyProperty TTYProperty =
            DependencyProperty.Register(nameof(TTY), typeof(int), typeof(Data),
                new FrameworkPropertyMetadata(0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    }
}
