using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace _20230318_BezierSize
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



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //以下必要？
        //private Rect _myBounds;
        //public Rect MyBounds { get => _myBounds; set => SetProperty(ref _myBounds, value); }

        private Rect _myTFBounds;
        public Rect MyTFBounds { get => _myTFBounds; set => SetProperty(ref _myTFBounds, value); }

        private double _myTFWidth;
        public double MyTFWidth { get => _myTFWidth; set => SetProperty(ref _myTFWidth, value); }

        private double _myTFHeight;
        public double MyTFHeight { get => _myTFHeight; set => SetProperty(ref _myTFHeight, value); }


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

        public Bezier()
        {

        }
    }
}
