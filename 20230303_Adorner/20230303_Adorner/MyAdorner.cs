using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

//embellishとadornとdecorateの語感の違い... - Yahoo!知恵袋
//https://detail.chiebukuro.yahoo.co.jp/qa/question_detail/q1023045950

//adorn：飾る、装飾する、アドーン
//adorner：装飾者、アドーナー、アドナー
//装飾：decorate、装飾者：decorator
//Adol Christhin

//装飾の概要 - WPF .NET Framework | Microsoft Learn
//https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/controls/adorners-overview?view=netframeworkdesktop-4.8
//WPF Adorners Part 1 – What are adorners
//https://www.nbdtech.com/Blog/archive/2010/06/21/wpf-adorners-part-1-ndash-what-are-adorners.aspx


//Adornerを対象に装着するタイミングはLoadedイベントのとき
//もしInitializeで装着しようとしても、必要なAdornerLayerが取得できない
namespace _20230303_Adorner
{
    /// <summary>
    /// 対象の四隅に円を表示するだけのAdorner
    /// </summary>
    internal class MyAdorner : Adorner
    {
        public MyAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            //base.OnRender(drawingContext);
            SolidColorBrush b = new(Color.FromArgb(50, 0, 0, 255));
            Pen pen=new(Brushes.Blue, 1);
            double radius = 10.0;
            Rect r = new(this.AdornedElement.DesiredSize);
            drawingContext.DrawEllipse(b, pen, r.TopLeft, radius, radius);
            drawingContext.DrawEllipse(b, pen, r.TopRight, radius, radius);
            drawingContext.DrawEllipse(b, pen, r.BottomLeft, radius, radius);
            drawingContext.DrawEllipse(b, pen, r.BottomRight, radius, radius);
        }
    }
}
