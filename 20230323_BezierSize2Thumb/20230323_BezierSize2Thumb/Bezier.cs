using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _20230323_BezierSize2Thumb
{
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
                new FrameworkPropertyMetadata(Visibility.Collapsed,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int MyProperty
        {
            get { return (int)GetValue(MyPropertyProperty); }
            private set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register(nameof(MyProperty), typeof(int), typeof(Bezier),
                new FrameworkPropertyMetadata(0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


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


        public bool MyIsEditing
        {
            get { return (bool)GetValue(MyIsEditingProperty); }
            set { SetValue(MyIsEditingProperty, value); }
        }
        public static readonly DependencyProperty MyIsEditingProperty =
            DependencyProperty.Register(nameof(MyIsEditing), typeof(bool), typeof(Bezier),
                new FrameworkPropertyMetadata(false,
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
                    context.BeginFigure(MyPoints[0], false, MyLineClose);
                    context.PolyBezierTo(MyPoints.Skip(1).ToArray(), true, MyLineSmoothJoin);
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

        }

        private void Bezier_Loaded(object sender, RoutedEventArgs e)
        {
            SetMyBounds();
            MyAdorner = new(this);
            AdornerLayer.GetAdornerLayer(this).Add(MyAdorner);
            SetBinding(MyAnchorVisibleProperty, new Binding() { Source = this, Path = new PropertyPath(MyIsEditingProperty), Converter = new MyConverterBoolVisible() });
            MyAdorner.SetBinding(VisibilityProperty, new Binding() { Source = this, Path = new PropertyPath(MyAnchorVisibleProperty) });
        }

        public void SetMyBounds()
        {
            var bounds = VisualTreeHelper.GetDescendantBounds(this);
            if (bounds.IsEmpty) return;
            MyExternalBounds = bounds;
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
