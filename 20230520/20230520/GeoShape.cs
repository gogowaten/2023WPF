﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace _20230520
{
    public class GeoShape : Shape
    {

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(GeoShape),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        //読み取り専用の依存関係プロパティ
        //WPF4.5入門 その43 「読み取り専用の依存関係プロパティ」 - かずきのBlog@hatena
        //        https://blog.okazuki.jp/entry/2014/08/18/083455

        public Rect MyRenderBounds
        {
            get { return (Rect)GetValue(MyRenderBoundsProperty); }
            set { SetValue(MyRenderBoundsPropertyKey, value); }
        }
        public static readonly DependencyPropertyKey MyRenderBoundsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyRenderBounds), typeof(Rect), typeof(GeoShape),
                new FrameworkPropertyMetadata(new Rect(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty MyRenderBoundsProperty =
            MyRenderBoundsPropertyKey.DependencyProperty;

        //回転角度。RenderTransformとバインドすると回転角度変更時に
        //ArrangeOverrideが発生するようになる
        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(GeoShape),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints.Count <= 1) return Geometry.Empty;

                StreamGeometry geometry = new();
                using (var content = geometry.Open())
                {
                    content.BeginFigure(MyPoints[0], false, false);
                    content.PolyLineTo(MyPoints.Skip(1).ToList(), true, false);

                }
                geometry.Freeze();
                return geometry;
            }
        }

        public GeoShape()
        {
            Loaded += GeoShape_Loaded;

            //回転角度のバインド。角度変更でArrangeOverrideを発生させるため
            SetBinding(MyAngleProperty, new Binding() { Source = this, Path = new PropertyPath(RenderTransformProperty), Converter = new MyConverterRenderTransform2Angle() });
        
        }

        private void GeoShape_Loaded(object sender, RoutedEventArgs e)
        {
            UpdataRenderBounds();
        }

        //Transform変更(開店)時にも実行したいけど、ArrangeOverrideでは反応しない
        //→なぜかできた、MyAngleとRenderTransformをバインドしたらなぜかArrangeOverrideが発動
        protected override Size ArrangeOverride(Size finalSize)
        {
            UpdataRenderBounds();
            return base.ArrangeOverride(finalSize);
        }

        //PropertyChangedならTransformに反応するけど、大掛かりすぎる
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            //if (e.Property.PropertyType == typeof(Transform))
            //{
            //    UpdataRenderBounds();
            //}
            base.OnPropertyChanged(e);
        }


        /// <summary>
        /// 図形の描画のRectを更新、StrokeThicknessを考慮したもの
        /// </summary>
        private void UpdataRenderBounds()
        {
            //GetDescendantBoundsだとMyPoints変更時に正しい値が取れない
            //var bb = VisualTreeHelper.GetDescendantBounds(this);
            //if (bb.IsEmpty) return;


            Pen pen = new(Stroke, StrokeThickness);
            Geometry geo = DefiningGeometry.Clone();
            geo.Transform = RenderTransform;//変形時には必要？
            //PathGeometry widenGeo = geo.GetWidenedPathGeometry(pen);
            //Rect widenRenderRect = widenGeo.GetRenderBounds(null);
            //MyRenderBounds = widenRenderRect;

            //これだけでいい？
            var renderB = geo.GetRenderBounds(pen);
            MyRenderBounds = renderB;
        }
    }


    /// <summary>
    /// RenderTransformの回転角度(RotateTransform)を取り出す
    /// </summary>
    public class MyConverterRenderTransform2Angle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Transform tf = (Transform)value;
            if (tf is TransformGroup tfg)
            {
                foreach (var item in tfg.Children)
                {
                    if (item is RotateTransform rotate)
                    {
                        return rotate.Angle;
                    }
                }

            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double angel=(double)value;
            TransformGroup tfg = new();
            tfg.Children.Add(new RotateTransform(angel));
            return tfg;
        }
    }
}
