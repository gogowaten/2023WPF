using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

//WPF、マウスドラッグ移動できるTextBox、Templateを改変したThumbで作成 - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2022/06/15/132409
//2022WPF/MainWindow.xaml.cs at 9c0d112e5bb07bb18d046d1a08f5b8c996e995b8 · gogowaten/2022WPF
//https://github.com/gogowaten/2022WPF/blob/9c0d112e5bb07bb18d046d1a08f5b8c996e995b8/20220615_TextBoxThumb0/20220615_TextBoxThumb0/MainWindow.xaml.cs

namespace _20230208_HutaTextBox
{
    /// <summary>
    /// ダブルクリックで編集可能状態を切り替えるTextBox
    /// </summary>
    class HutaTextBox : ContentControl
    {
        private readonly string HUTA = "huta";
        private readonly string TEXTBOX = "mytextbox";
        private readonly Grid HutaGrid;
        private readonly TextBox MyTextBox;
        public string Text
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(HutaTextBox),
                new FrameworkPropertyMetadata("",
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsEdit
        {
            get { return (bool)GetValue(IsEditProperty); }
            set { SetValue(IsEditProperty, value); }
        }
        public static readonly DependencyProperty IsEditProperty =
            DependencyProperty.Register(nameof(IsEdit), typeof(bool), typeof(HutaTextBox),
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsEditChanged)));
        //ダブルクリックでテキスト編集状態の切り替え
        //蓋の背景色が透明色ならnullにしてTextBoxを編集状態にする
        //蓋の背景色がnullだった場合は透明色にして編集状態終了
        private static void OnIsEditChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HutaTextBox hutaText)
            {
                if (hutaText.HutaGrid.Background == Brushes.Transparent)
                {
                    hutaText.HutaGrid.Background = null;
                    Keyboard.Focus(hutaText.MyTextBox);
                }
                else
                {
                    hutaText.HutaGrid.Background = Brushes.Transparent;
                    Keyboard.ClearFocus();
                }
            }
        }

        public HutaTextBox()
        {

            SetTemplata();
            HutaGrid = (Grid)Template.FindName(HUTA, this);
            MyTextBox = (TextBox)Template.FindName(TEXTBOX, this);
        }

        private void SetTemplata()
        {
            FrameworkElementFactory baseGrid = new(typeof(Grid));
            FrameworkElementFactory factory = new(typeof(TextBox), TEXTBOX);
            FrameworkElementFactory huta = new(typeof(Grid), HUTA);
            huta.SetValue(Grid.BackgroundProperty, Brushes.Transparent);
            factory.SetValue(TextBox.TextProperty,
                new Binding() { Source = this, Path = new PropertyPath(MyTextProperty) });
            baseGrid.AppendChild(factory);
            baseGrid.AppendChild(huta);
            Template = new() { VisualTree = baseGrid };
            ApplyTemplate();
        }

    }


    /// <summary>
    /// ダブルクリックで編集可能状態を切り替えるTextBox型のThumb
    /// </summary>
    public class TTTextBox : Thumb
    {

        public string Text
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(TTTextBox),
                new FrameworkPropertyMetadata("",
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool IsEdit
        {
            get { return (bool)GetValue(IsEditProperty); }
            set { SetValue(IsEditProperty, value); }
        }
        public static readonly DependencyProperty IsEditProperty =
                    DependencyProperty.Register("IsEdit", typeof(bool), typeof(TTTextBox), new PropertyMetadata(false));



        public TTTextBox()
        {
            SetTemplate();
            DragDelta += TTTextBox_DragDelta;
            MouseDoubleClick += TTTextBox_MouseDoubleClick;
        }

        private void TTTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IsEdit = !IsEdit;
        }

        private void TTTextBox_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
        }

        private void SetTemplate()
        {
            FrameworkElementFactory factory = new(typeof(HutaTextBox));
            factory.SetValue(HutaTextBox.MyTextProperty, new Binding() { Source = this, Path = new PropertyPath(MyTextProperty) });
            factory.SetValue(HutaTextBox.IsEditProperty, new Binding() { Source = this,Path=new PropertyPath(IsEditProperty) });
            Template = new() { VisualTree = factory };
        }
    }
}
