using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//第4回　“見た目”を決めるコントロール・テンプレート：連載　WPF／Silverlight UIフレームワーク入門（2/3 ページ） - ＠IT
//https://atmarkit.itmedia.co.jp/ait/articles/0907/13/news093_2.html
//↑のXAMLをC#コードビハインドで書いた

namespace _20230511_TemlateBindingExtension
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyTest();
        }

        private void MyTest()
        {
            Button myb = new()
            {
                Template = GetTemplate(),
                Content = "button1",
                Background = Brushes.LightBlue,
                Margin = new Thickness(30)
            };
            MyStackPanel.Children.Add(myb);
            
            myb = new()
            {
                Template = GetTemplate(),
                Content = "button2",
                Background = Brushes.MediumAquamarine,
                Margin = new Thickness(30)
            };
            MyStackPanel.Children.Add(myb);
        }

        private ControlTemplate GetTemplate()
        {
            ControlTemplate template = new();
            FrameworkElementFactory eBorder = new(typeof(Border));
            FrameworkElementFactory eTextb = new(typeof(TextBlock));

            eBorder.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(BackgroundProperty));
            //↑と↓は同じ結果
            //eBorder.SetValue(Border.BackgroundProperty, new Binding() { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent), Path = new PropertyPath(BackgroundProperty) });

            eBorder.SetValue(Border.CornerRadiusProperty, new CornerRadius(30.0));
            eBorder.SetValue(Border.PaddingProperty, new Thickness(10.0));
            eTextb.SetValue(TextBlock.TextProperty, new TemplateBindingExtension(ContentProperty));
            eTextb.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            eTextb.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            eBorder.AppendChild(eTextb);
            template.VisualTree = eBorder;
            return template;
        }
    }
}
