using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TTPolylineZ
{
    public class TThumb : Thumb
    {
        #region 依存プロパティ

        /// <summary>
        /// 終点のヘッドタイプ
        /// </summary>
        public HeadType HeadEndType
        {
            get { return (HeadType)GetValue(HeadEndTypeProperty); }
            set { SetValue(HeadEndTypeProperty, value); }
        }
        public static readonly DependencyProperty HeadEndTypeProperty =
            DependencyProperty.Register(nameof(HeadEndType), typeof(HeadType), typeof(TThumb),
                new FrameworkPropertyMetadata(HeadType.None,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 始点のヘッドタイプ
        /// </summary>
        public HeadType HeadBeginType
        {
            get { return (HeadType)GetValue(HeadBeginTypeProperty); }
            set { SetValue(HeadBeginTypeProperty, value); }
        }
        public static readonly DependencyProperty HeadBeginTypeProperty =
            DependencyProperty.Register(nameof(HeadBeginType), typeof(HeadType), typeof(TThumb),
                new FrameworkPropertyMetadata(HeadType.None,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double HeadAngle
        {
            get { return (double)GetValue(HeadAngleProperty); }
            set { SetValue(HeadAngleProperty, value); }
        }
        public static readonly DependencyProperty HeadAngleProperty =
            DependencyProperty.Register(nameof(HeadAngle), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(30.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TThumb),
                new FrameworkPropertyMetadata(new PointCollection() { new Point(0, 0), new Point(100, 100) },
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(TThumb),
                new FrameworkPropertyMetadata(Brushes.Red,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Brush TTFill
        {
            get { return (Brush)GetValue(TTFillProperty); }
            set { SetValue(TTFillProperty, value); }
        }
        public static readonly DependencyProperty TTFillProperty =
            DependencyProperty.Register(nameof(TTFill), typeof(Brush), typeof(TThumb),
                new FrameworkPropertyMetadata(Brushes.Red,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(5.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        #endregion 依存プロパティ
        public PolylineZ MyPolylineZ { get; set; }
        public Data MyData { get; set; } = new();
        public TThumb()
        {
            MyPolylineZ = SetTemplate();
            MySetBinding();
        }
        private PolylineZ SetTemplate()
        {
            FrameworkElementFactory fArrow = new(typeof(PolylineZ), "polyz");
            this.Template = new() { VisualTree = fArrow };
            this.ApplyTemplate();
            if (this.Template.FindName("polyz", this) is PolylineZ arrow)
            {
                return arrow;
            }
            else { throw new Exception(); }
        }
      
        private void MySetBinding()
        {
            //Points同士の連携はBindingより=(イコール)でしたほうが楽
            //連携の方向は
            //mydata <- this <- polyz、で右側がSource
            //mydata <- this ここをイコール
            //this < -polyz ここはBinding
            Loaded += (a, b) => { MyData.PointCollection = MyPoints; };
            this.SetBinding(MyPointsProperty, new Binding() { Source = MyPolylineZ, Path = new PropertyPath(PolylineZ.MyPointsProperty) });

            ////以下だと値の更新はされるけど、見た目の更新がされない。
            ////→値更新後にPolylineZに対してInvalidateVisual();を実行したら見た目も更新された
            ////でもめんどくさいから上の方法が良い
            //MyPolylineZ.SetBinding(PolylineZ.MyPointsProperty,new Binding() { Source=this,Path= new PropertyPath(MyPointsProperty) });
            //this.SetBinding(MyPointsProperty,new Binding(nameof(MyData.PointCollection)) { Source=this.MyData });

            //Bindingの方向、Dataの各プロパティは依存プロパティではないのでTargetにはできないので
            //mydata <- this や mydata <- polyz とかにはできない
            //なのでできるのは以下の3種類
            //polyz <- this & this <- mydata 今回はこれ
            //this <- polyz & polyz <- mydata
            //this <- mydata & polyz <- mydata


            //polyz <- this
            MyPolylineZ.DataContext = this;
            MyPolylineZ.SetBinding(Shape.StrokeThicknessProperty, nameof(StrokeThickness));
            MyPolylineZ.SetBinding(Shape.StrokeProperty, nameof(Stroke));
            MyPolylineZ.SetBinding(Shape.FillProperty, nameof(TTFill));
            MyPolylineZ.SetBinding(PolylineZ.HeadBeginTypeProperty, nameof(HeadBeginType));
            MyPolylineZ.SetBinding(PolylineZ.HeadEndTypeProperty, nameof(HeadEndType));
            MyPolylineZ.SetBinding(PolylineZ.AngleProperty, nameof(HeadAngle));

            //this <- mydata
            this.DataContext = this.MyData;
            this.SetBinding(StrokeThicknessProperty, nameof(MyData.StrokeThickness));
            this.SetBinding(StrokeProperty, nameof(MyData.Stroke));
            this.SetBinding(TTFillProperty, nameof(MyData.Fill));
            this.SetBinding(HeadEndTypeProperty, nameof(MyData.HeadEndType));
            this.SetBinding(HeadBeginTypeProperty, nameof(MyData.HeadBeginType));
            this.SetBinding(HeadAngleProperty, nameof(MyData.HeadAngle));
        }

    }
}
