using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Data;

namespace _20230324
{
    public enum ShapeType { Line = 0, Bezier, }
    public class Bezier : Shape
    {
        #region 依存関係プロパティと通知プロパティ

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(Bezier),
                new FrameworkPropertyMetadata(new PointCollection(),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 頂点座標のThumbsの表示設定
        /// </summary>
        public Visibility MyAnchorVisible
        {
            get { return (Visibility)GetValue(MyAnchorVisibleProperty); }
            set { SetValue(MyAnchorVisibleProperty, value); }
        }
        public static readonly DependencyProperty MyAnchorVisibleProperty =
            DependencyProperty.Register(nameof(MyAnchorVisible), typeof(Visibility), typeof(Bezier),
                new FrameworkPropertyMetadata(Visibility.Visible,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// ラインのつなぎ目をtrueで丸める、falseで鋭角にする
        /// </summary>
        public bool MyLineSmoothJoin
        {
            get { return (bool)GetValue(MyLineSmoothJoinProperty); }
            set { SetValue(MyLineSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyLineSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyLineSmoothJoin), typeof(bool), typeof(Bezier),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public int MyProperty
        {
            get { return (int)GetValue(MyPropertyProperty); }
            private set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register(nameof(MyProperty), typeof(int), typeof(Bezier),
                new FrameworkPropertyMetadata(0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// ラインの始点と終点を繋ぐかどうか
        /// </summary>
        public bool MyLineClose
        {
            get { return (bool)GetValue(MyLineCloseProperty); }
            set { SetValue(MyLineCloseProperty, value); }
        }
        public static readonly DependencyProperty MyLineCloseProperty =
            DependencyProperty.Register(nameof(MyLineClose), typeof(bool), typeof(Bezier),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        //読み取り専用の依存関係プロパティ
        //WPF4.5入門 その43 「読み取り専用の依存関係プロパティ」 - かずきのBlog@hatena
        //        https://blog.okazuki.jp/entry/2014/08/18/083455
        /// <summary>
        /// Descendant、見た目上のRect
        /// </summary>
        private static readonly DependencyPropertyKey MyExternalBoundsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyExternalBounds), typeof(Rect), typeof(Bezier),
                new FrameworkPropertyMetadata(Rect.Empty,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty MyExternalBoundsProperty =
            MyExternalBoundsPropertyKey.DependencyProperty;
        public Rect MyExternalBounds
        {
            get { return (Rect)GetValue(MyExternalBoundsProperty); }
            private set { SetValue(MyExternalBoundsPropertyKey, value); }
        }



        public ShapeType MyType
        {
            get { return (ShapeType)GetValue(MyTypeProperty); }
            set { SetValue(MyTypeProperty, value); }
        }
        public static readonly DependencyProperty MyTypeProperty =
            DependencyProperty.Register(nameof(MyType), typeof(ShapeType), typeof(Bezier),
                new FrameworkPropertyMetadata(ShapeType.Line,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));




        #endregion 依存関係プロパティと通知プロパティ

        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints.Count == 0) return Geometry.Empty;
                StreamGeometry geometry = new();
                using (var context = geometry.Open())
                {
                    switch (MyType)
                    {
                        case ShapeType.Line:
                            context.BeginFigure(MyPoints[0], false, MyLineClose);
                            context.PolyLineTo(MyPoints.Skip(1).ToArray(), true, MyLineSmoothJoin);
                            break;
                        case ShapeType.Bezier:
                            context.BeginFigure(MyPoints[0], false, MyLineClose);
                            context.PolyBezierTo(MyPoints.Skip(1).ToArray(), true, MyLineSmoothJoin);
                            break;
                        default:
                            break;
                    }

                }
                geometry.Freeze();
                return geometry;
            }
        }

        public MyAdorner? MyAdorner { get; private set; }


        //SizeChangedで見た目Rectを更新
        public Bezier()
        {
            SizeChanged += Bezier_SizeChanged;
            Loaded += Bezier_Loaded;
            RenderTransformOrigin = new Point(0.5, 0.5);
        }

        private void Bezier_Loaded(object sender, RoutedEventArgs e)
        {
            SetMyBounds();
            MyAdorner = new(this);
            AdornerLayer.GetAdornerLayer(this).Add(MyAdorner);
        }

        public void SetMyBounds()
        {
            var bounds = VisualTreeHelper.GetDescendantBounds(this);
            if (bounds.IsEmpty) return;
            var rr = RenderTransform.TransformBounds(bounds);
            Pen pen = new(Stroke, StrokeThickness);
            var neko = this.DefiningGeometry.GetWidenedPathGeometry(pen).Bounds;
            var wneko = this.RenderedGeometry.GetWidenedPathGeometry(pen).Bounds;
            var nekot = RenderTransform.TransformBounds(neko);
            var gw = DefiningGeometry.Clone().GetWidenedPathGeometry(pen);
            var rt = RenderTransform.Clone();
            gw.Transform = RenderTransform;
            var gwre = gw.Bounds;
            MyExternalBounds = rr;
        }

        //見た目Rectの更新はSizeChangedで必須と
        //ArrangeOverrideかMeasureOverrideのどちらかで必要
        private void Bezier_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetMyBounds();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            SetMyBounds();
            return base.ArrangeOverride(finalSize);
        }
    }


}
