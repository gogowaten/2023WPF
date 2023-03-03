using System;
using System.Collections.Generic;
using System.Linq;
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

namespace _20230303
{
    class Ador : Adorner
    {
        public Ador(UIElement adornedElement) : base(adornedElement)
        {
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            //base.OnRender(drawingContext);

            SolidColorBrush brush = new(Color.FromArgb(50, 200, 0, 0));
            Pen pen = new Pen(Brushes.Red, 1.0);
            double radius = 10.0;
            Rect rect = new Rect(this.AdornedElement.DesiredSize);
            drawingContext.DrawEllipse(brush, pen, rect.TopLeft, radius, radius);
            drawingContext.DrawEllipse(brush, pen, rect.TopRight, radius, radius);
            drawingContext.DrawEllipse(brush, pen, rect.BottomLeft, radius, radius);
            drawingContext.DrawEllipse(brush, pen, rect.BottomRight, radius, radius);
        }
    }
}
