using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace _20230113
{
    public abstract class TThumb : Thumb
    {
        #region 依存プロパティ
        public double TTLeft
        {
            get { return (double)GetValue(TTLeftProperty); }
            set { SetValue(TTLeftProperty, value); }
        }
        public static readonly DependencyProperty TTLeftProperty =
            DependencyProperty.Register(nameof(TTLeft), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double TTTop
        {
            get { return (double)GetValue(TTTopProperty); }
            set { SetValue(TTTopProperty, value); }
        }
        public static readonly DependencyProperty TTTopProperty =
            DependencyProperty.Register(nameof(TTTop), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存プロパティ
        //public Data Data { get; set; }
        public TThumb()
        {
            //Data = new Data();
            //DataContext = this;
            SetBinding(Canvas.LeftProperty, new Binding() { Path = new PropertyPath(TTLeftProperty), Source = this });
            SetBinding(Canvas.TopProperty, new Binding() { Path = new PropertyPath(TTTopProperty), Source = this });
        }
    }

    public class TTTextBlock : TThumb
    {
        #region 依存プロパティ

        public string TTText
        {
            get { return (string)GetValue(TTTextProperty); }
            set { SetValue(TTTextProperty, value); }
        }
        public static readonly DependencyProperty TTTextProperty =
            DependencyProperty.Register(nameof(TTText), typeof(string), typeof(TTTextBlock),
                new FrameworkPropertyMetadata("",
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存プロパティ

        public DataTextBlock Data { get; set; }
        private TextBlock MyTemplateElement;

        public TTTextBlock()
        {
            Data = new DataTextBlock();
            MyTemplateElement = SetMyBinding();
        }
        public TTTextBlock(DataTextBlock data)
        {
            Data = data;
            MyTemplateElement = SetMyBinding();
        }
        private TextBlock SetMyBinding()
        {
            this.DataContext = Data;
            SetBinding(TTLeftProperty, nameof(Data.X));
            SetBinding(TTTopProperty, nameof(Data.Y));

            TextBlock resultElement = SetTemplate();
            SetBinding(TTTextProperty, nameof(Data.Text));
            resultElement.SetBinding(TextBlock.TextProperty, nameof(Data.Text));

            return resultElement;
        }
        private TextBlock SetTemplate()
        {
            FrameworkElementFactory factory = new(typeof(TextBlock), "n1emo");
            this.Template = new() { VisualTree = factory };
            this.ApplyTemplate();
            if (Template.FindName("nemo", this) is TextBlock element)
            { return element; }
            else { throw new ArgumentException("テンプレートの要素取得できんかった"); }
        }
    }


}
