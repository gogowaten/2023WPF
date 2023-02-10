using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace _20230210_hutidoriTextBox
{
    public class TTTextBox : Thumb
    {
        public TTTextBox()
        {
            SetTemplate();
        }
        private void SetTemplate()
        {
            FrameworkElementFactory bGrid = new(typeof(Grid));
            FrameworkElementFactory factory = new(typeof(TextBox));
            FrameworkElementFactory hutaGrid = new(typeof(Grid));
            bGrid.AppendChild(factory);
            bGrid.AppendChild(hutaGrid);
            this.Template = new() { VisualTree = bGrid };
        }

    }

    internal class HutiTextBox : TextBox
    {
        private Color brushColor = Colors.Black;
        private Color penColor = Colors.White;
        private double penSize = 1.0;
        public Color BrushColor { get => brushColor; set { brushColor = value; InvalidateVisual(); } }
        public Color PenColor { get => penColor; set { penColor = value; InvalidateVisual(); } }
        public double PenSize { get => penSize; set { penSize = value; InvalidateVisual(); } }

        public HutiTextBox()
        {
            Foreground = Brushes.Transparent;
            Background = Brushes.Transparent;
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            InvalidateVisual();
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (string.IsNullOrEmpty(Text)) { return; }

            //c# - FormattedText.FormttedText は廃止されました。 PixelsPerDipオーバーライドを使用する
            //        https://stackoverflow.com/questions/45765980/formattedtext-formttedtext-is-obsolete-use-the-pixelsperdip-override

            //旧式
            //FormattedText fText = new(
            //    Text,
            //    System.Globalization.CultureInfo.CurrentCulture,
            //    FlowDirection.LeftToRight,
            //    new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
            //    FontSize,
            //    new SolidColorBrush());

            FormattedText fText = new(
                Text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                new SolidColorBrush(),
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            //var neko = VisualTreeHelper.GetDpi(this).PixelsPerDip;//1
            var leading = fText.OverhangLeading;
            var after = fText.OverhangAfter;
            var trailing = fText.OverhangTrailing;
            var w = fText.Width;
            var ww = fText.MaxTextWidth;
            var www = fText.WidthIncludingTrailingWhitespace;
            fText.TextAlignment= TextAlignment.Center;
            
            drawingContext.DrawGeometry(
                new SolidColorBrush(BrushColor),
                new Pen(new SolidColorBrush(PenColor), PenSize),
                fText.BuildGeometry(new Point(Padding.Left, Padding.Top)));
            //drawingContext.DrawGeometry(
            //    new SolidColorBrush(BrushColor),
            //    new Pen(new SolidColorBrush(PenColor), PenSize),
            //    fText.BuildGeometry(new Point(Padding.Left, Padding.Top)));

        }

    }
}
