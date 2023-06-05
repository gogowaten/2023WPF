using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Shapes;
using System.Globalization;

namespace _20230520
{
    /// <summary>
    /// 一つの任意要素にサイズ変更のハンドルを付けることができるCanvas
    /// 2つのRectangleを表示
    /// 1つのRectangleにサイズ変更用のハンドルThumbを表示
    /// 公開メソッドで別のRectangleにハンドルThumbを表示切替
    /// </summary>
    public class CCanvas : Canvas
    {
        #region 依存関係プロパティ

        public double MyHandlThumbSize
        {
            get { return (double)GetValue(MyHandlThumbSizeProperty); }
            set { SetValue(MyHandlThumbSizeProperty, value); }
        }
        public static readonly DependencyProperty MyHandlThumbSizeProperty =
            DependencyProperty.Register(nameof(MyHandlThumbSize), typeof(double), typeof(ResizableCanvas),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ


        //R:Right, L:Left, T:Top, B:Bottom
        public Thumb TTR { get; set; } = new();
        public Thumb TTB { get; set; } = new();
        public Thumb TTL { get; set; } = new();
        public Thumb TTT { get; set; } = new();
        public Thumb TTRT { get; set; } = new();
        public Thumb TTLB { get; set; } = new();
        public Thumb TTRB { get; set; } = new();
        public Thumb TTLT { get; set; } = new();
        private readonly List<Thumb> MySizeHandlThumbs = new();

        public FrameworkElement MyElement { get; set; } = new();

        public Rectangle MyRectangle0 { get; set; } = new() { Width = 100, Height = 100, Fill = Brushes.MediumAquamarine };
        public Rectangle MyRectangle1 { get; set; } = new() { Width = 100, Height = 100, Fill = Brushes.Blue };

        public CCanvas()
        {
            Background = Brushes.WhiteSmoke;
            Children.Add(TTR); TTR.DragDelta += (o, e) => { SetWidth(e); };
            Children.Add(TTB); TTB.DragDelta += (o, e) => { SetHeight(e); };
            Children.Add(TTL); TTL.DragDelta += (o, e) => { SetWidthAndLeft(e); };
            Children.Add(TTT); TTT.DragDelta += (o, e) => { SetHeightAndTop(e); };

            Children.Add(TTRT); TTRT.DragDelta += (o, e) => { SetHeightAndTop(e); SetWidth(e); };
            Children.Add(TTLB); TTLB.DragDelta += (o, e) => { SetWidthAndLeft(e); SetHeight(e); };
            Children.Add(TTRB); TTRB.DragDelta += (o, e) => { SetWidth(e); SetHeight(e); };
            Children.Add(TTLT); TTLT.DragDelta += (o, e) => { SetWidthAndLeft(e); SetHeightAndTop(e); };
            
            MySizeHandlThumbs.Add(TTR);
            MySizeHandlThumbs.Add(TTB);
            MySizeHandlThumbs.Add(TTL);
            MySizeHandlThumbs.Add(TTT);
            MySizeHandlThumbs.Add(TTRT);
            MySizeHandlThumbs.Add(TTRB);
            MySizeHandlThumbs.Add(TTLT);
            MySizeHandlThumbs.Add(TTLB);
            foreach (var item in MySizeHandlThumbs) { SetZIndex(item, 1); }
            SetMyBinding();
            Children.Add(MyRectangle0); SetLeft(MyRectangle0, 220); SetTop(MyRectangle0, 20);
            Children.Add(MyRectangle1); SetLeft(MyRectangle1, 50); SetTop(MyRectangle1, 50);
            SetMyElement(MyRectangle1);
        }

        private void SetMyElement(FrameworkElement element)
        {
            MyElement = element;
            Binding bLeft = new() { Source = MyElement, Path = new PropertyPath(LeftProperty) };
            Binding bWidth = new() { Source = MyElement, Path = new PropertyPath(WidthProperty) };
            Binding bTop = new() { Source = MyElement, Path = new PropertyPath(TopProperty) };
            Binding bHeight = new() { Source = MyElement, Path = new PropertyPath(HeightProperty) };
            TTT.SetBinding(TopProperty, bTop);
            TTLT.SetBinding(TopProperty, bTop);
            TTLT.SetBinding(LeftProperty, bLeft);
            TTLB.SetBinding(LeftProperty, bLeft);
            TTRT.SetBinding(TopProperty, bTop);
            TTL.SetBinding(LeftProperty, bLeft);

            MultiBinding mb = new() { Converter = new MyConverterHandlLocate() };
            mb.Bindings.Add(bLeft);
            mb.Bindings.Add(bWidth);
            TTR.SetBinding(LeftProperty, mb);
            TTRT.SetBinding(LeftProperty, mb);
            TTRB.SetBinding(LeftProperty, mb);

            mb = new() { Converter = new MyConverterHandlLocate() };
            mb.Bindings.Add(bTop); mb.Bindings.Add(bHeight);
            TTRB.SetBinding(TopProperty, mb);
            TTLB.SetBinding(TopProperty, mb);
            TTB.SetBinding(TopProperty, mb);

            mb = new() { Converter = new MyCovnerterHandlLocateHalf() };
            mb.Bindings.Add(bTop);
            mb.Bindings.Add(bHeight);
            TTR.SetBinding(TopProperty, mb);
            TTL.SetBinding(TopProperty, mb);

            mb = new() { Converter = new MyCovnerterHandlLocateHalf() };
            mb.Bindings.Add(bLeft); mb.Bindings.Add(bWidth);
            TTT.SetBinding(LeftProperty, mb);
            TTB.SetBinding(LeftProperty, mb);

        }

        private void SetWidth(DragDeltaEventArgs e)
        {
            double value = MyElement.Width + e.HorizontalChange;
            MyElement.Width = value >= 0 ? value : 0;
        }
        private void SetWidthAndLeft(DragDeltaEventArgs e)
        {
            double w = MyElement.Width - e.HorizontalChange;
            if (w >= 0)
            {
                SetLeft(MyElement, GetLeft(MyElement) + e.HorizontalChange);
                MyElement.Width -= e.HorizontalChange;
            }
            else
            {
                SetLeft(MyElement, GetLeft(MyElement) + MyElement.Width);
                MyElement.Width = 0;
            }
        }
        
        private void SetHeight(DragDeltaEventArgs e)
        {
            double h = MyElement.Height + e.VerticalChange;
            MyElement.Height = h >= 0 ? h : 0;
        }

        private void SetHeightAndTop(DragDeltaEventArgs e)
        {
            double h = MyElement.Height - e.VerticalChange;
            if (h >= 0)
            {
                SetTop(MyElement, GetTop(MyElement) + e.VerticalChange);
                MyElement.Height -= e.VerticalChange;
            }
            else
            {
                SetTop(MyElement, GetTop(MyElement) + MyElement.Height);
                MyElement.Height = 0;
            }
        }


        private void SetMyBinding()
        {


            //ハンドルサイズ
            Binding handlSize = new Binding() { Source = this, Path = new PropertyPath(MyHandlThumbSizeProperty) };
            TTT.SetBinding(WidthProperty, handlSize); TTT.SetBinding(HeightProperty, handlSize);
            TTB.SetBinding(WidthProperty, handlSize); TTB.SetBinding(HeightProperty, handlSize);
            TTL.SetBinding(WidthProperty, handlSize); TTL.SetBinding(HeightProperty, handlSize);
            TTR.SetBinding(WidthProperty, handlSize); TTR.SetBinding(HeightProperty, handlSize);
            TTLT.SetBinding(WidthProperty, handlSize); TTLT.SetBinding(HeightProperty, handlSize);
            TTRT.SetBinding(WidthProperty, handlSize); TTRT.SetBinding(HeightProperty, handlSize);
            TTRB.SetBinding(WidthProperty, handlSize); TTRB.SetBinding(HeightProperty, handlSize);
            TTLB.SetBinding(WidthProperty, handlSize); TTLB.SetBinding(HeightProperty, handlSize);

        }

        public void TestChangeMyElement()
        {
            SetMyElement(MyRectangle0);
        }


    }


    public class MyConverterHandlLocate : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double left = (double)values[0];
            double width = (double)values[1];
            return left + width;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyCovnerterHandlLocateHalf : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double locate = (double)values[0];
            double size = (double)values[1];
            return locate + (size / 2.0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
