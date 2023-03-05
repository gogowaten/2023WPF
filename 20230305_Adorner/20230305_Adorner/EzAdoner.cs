using System;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;


//embellishとadornとdecorateの語感の違い... - Yahoo!知恵袋
//https://detail.chiebukuro.yahoo.co.jp/qa/question_detail/q1023045950
//adorn：飾る、装飾する、アドーン
//adorner：装飾者、アドーナー、アドナー
//装飾：decorate、装飾者：decorator

//装飾の概要 - WPF .NET Framework | Microsoft Learn
//https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/controls/adorners-overview?view=netframeworkdesktop-4.8
//WPF Adorners Part 1 – What are adorners
//https://www.nbdtech.com/Blog/archive/2010/06/21/wpf-adorners-part-1-ndash-what-are-adorners.aspx

//Mitesh Sureja's Blog: Adorners in WPF
//    http://miteshsureja.blogspot.com/2016/08/adorners-in-wpf.html
//簡略化した、例外とか考えてない、Thumbは右下に一個だけ
//装飾できるのはUIElementから狭めてFrameworkElementに変更
namespace _20230305_Adorner
{
    public class EzAdoner : Adorner
    {
        readonly Thumb MyThumb;//サイズ変更用つまみ
        readonly VisualCollection MyVisualChildren;//表示したい要素を管理する用？
        readonly FrameworkElement MyTarget;//装飾する対象要素
        public EzAdoner(FrameworkElement adornedElement) : base(adornedElement)
        {
            MyTarget = adornedElement;
            MyVisualChildren = new VisualCollection(this);
            MyThumb = new Thumb()
            {
                Cursor = Cursors.SizeNWSE,
                Height = 20,
                Width = 20,
                Opacity = 0.5,
                Background = Brushes.Red,
            };
            MyThumb.DragDelta += MyThumb_DragDelta;
            MyVisualChildren.Add(MyThumb);

            //TextBoxなどWidthの既定値がNaNなのを解除する
            MyTarget.Width = MyTarget.DesiredSize.Width;
            MyTarget.Height = MyTarget.DesiredSize.Height;
        }

        private void MyThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            //対象要素のサイズ変更、Thumbより小さくならないようにする
            MyTarget.Width = Math.Max(MyTarget.Width + e.HorizontalChange, MyThumb.Width);
            MyTarget.Height = Math.Max(MyTarget.Height + e.VerticalChange, MyThumb.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //Thumbの表示位置修正、常に対象要素の右下に表示
            MyThumb.Arrange(new Rect(
                MyTarget.Width / 2, MyTarget.Height / 2,
                MyTarget.Width, MyTarget.Height));
            return base.ArrangeOverride(finalSize);
        }

        #region VisualCollectionで必要        
        protected override int VisualChildrenCount => MyVisualChildren.Count;

        protected override Visual GetVisualChild(int index) => MyVisualChildren[index];
        #endregion VisualCollectionで必要
    }
}

