using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _20230428_AppDataDp
{
    class AppData:DependencyObject
    {
        public AppData() { }


        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }
        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register(nameof(Left), typeof(double), typeof(AppData),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Memo
        {
            get { return (string)GetValue(MemoProperty); }
            set { SetValue(MemoProperty, value); }
        }
        public static readonly DependencyProperty MemoProperty =
            DependencyProperty.Register(nameof(Memo), typeof(string), typeof(AppData),
                new FrameworkPropertyMetadata("None",
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    }
}
