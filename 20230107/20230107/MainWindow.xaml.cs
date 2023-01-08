using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Specialized;

namespace _20230107
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Data MyData { get; set; } = new();
        public TextBlock? tb { get; set; }
        public TTTextBlock MyTTT { get; set; }
        public TTRectangle MyTTR { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            //Test1();
        }
        private void Test1()
        {
            MyData = new DataText() { Text = "test", X = 100, Y = 100 };
            tb = new();
            tb.SetBinding(TextBlock.TextProperty, new Binding(nameof(DataText.Text)) { Source = MyData });
            tb.SetBinding(Canvas.LeftProperty, new Binding(nameof(MyData.X)) { Source = MyData });
            tb.SetBinding(Canvas.TopProperty, new Binding(nameof(MyData.Y)) { Source = MyData });
            MyCanvas.Children.Add(tb);

            DataText data = new() { Text = "tttest", X = 100, Y = 10 };
            MyTTT = new(data);
            MyCanvas.Children.Add(MyTTT);


            MyTTR = new(new() { X = 100, Y = 150, FillBrush = Brushes.Magenta });
            MyCanvas.Children.Add(MyTTR);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MyData.Text = "henkou";
            if (MyData is DataText data) { data.Text = "henkou"; }
            //tb.Text = "henkou";
            var neko = MyGroup1.MyData;
            //MyTTT.MyData.Text = "henkouuuu";
            //MyTTT.TTText = "dipendencyProperty";
        }
    }


    public class Data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private double _x; public double X { get => _x; set => SetProperty(ref _x, value); }
        private double _y; public double Y { get => _y; set => SetProperty(ref _y, value); }


        public Data() { }
    }
    public class DataText : Data
    {

        private string? _text;
        public string? Text { get => _text; set => SetProperty(ref _text, value); }

    }
    public class DataRectangle : Data
    {

        private double _w = 100.0;
        public double W { get => _w; set => SetProperty(ref _w, value); }

        private double _h = 100.0;
        public double H { get => _h; set => SetProperty(ref _h, value); }

        private Brush? _fillBrush;
        public Brush? FillBrush { get => _fillBrush; set => SetProperty(ref _fillBrush, value); }

    }
    public class DataGroup : Data
    {
        private ObservableCollection<Data>? _datas = new();
        public ObservableCollection<Data>? Datas { get => _datas; set => SetProperty(ref _datas, value); }
    }

    public abstract class TThumb : Thumb
    {

        public double TTLeft
        {
            get { return (double)GetValue(TTLeftProperty); }
            set { SetValue(TTLeftProperty, value); }
        }
        public static readonly DependencyProperty TTLeftProperty =
            DependencyProperty.Register(nameof(TTLeft), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double TTTop
        {
            get { return (double)GetValue(TTTopProperty); }
            set { SetValue(TTTopProperty, value); }
        }
        public static readonly DependencyProperty TTTopProperty =
            DependencyProperty.Register(nameof(TTTop), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Data MyData { get; set; } = new();
        public TThumb()
        {
            DataContext = MyData;
            SetBinding(Canvas.LeftProperty, new Binding(nameof(MyData.X)));
            SetBinding(Canvas.TopProperty, new Binding(nameof(MyData.Y)));
            SetBinding(TTLeftProperty, new Binding(nameof(MyData.X)) { Mode = BindingMode.TwoWay });
            SetBinding(TTTopProperty, new Binding(nameof(MyData.Y)) { Mode = BindingMode.TwoWay });
        }
        internal abstract void SetTemplateAndBinding();


    }
    public abstract class TThumbItem : TThumb { }
    public class TTTextBlock : TThumbItem
    {
        public new DataText MyData { get; private set; } = new();

        //デザイナー画面でのリアルタイム更新のために
        //FrameworkPropertyMetadataOptionsが必要
        public string TTText
        {
            get { return (string)GetValue(TTTextProperty); }
            set { SetValue(TTTextProperty, value); }
        }
        public static readonly DependencyProperty TTTextProperty =
            DependencyProperty.Register(nameof(TTText), typeof(string), typeof(TTTextBlock),
                new FrameworkPropertyMetadata("",
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));


        public TTTextBlock()
        {
            DataContext = MyData;
            SetTemplateAndBinding();
        }
        public TTTextBlock(DataText data)
        {
            //この順番が重要、DataをセットしてからDatacontextに指定して、最後にBinding設定
            //こうしないと正しくBindingできない、Bindingがリセットされてしまう
            //特にDataセットしてからDataContextの指定は重要
            //Bindingの設定を先にしたいときはSourceを明示すればBindingできる
            MyData = data;
            DataContext = MyData;
            SetTemplateAndBinding();
        }
        internal override void SetTemplateAndBinding()
        {
            FrameworkElementFactory fTextBlock = new(typeof(TextBlock));
            FrameworkElementFactory factory = new(typeof(Border));
            factory.AppendChild(fTextBlock);
            fTextBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(MyData.Text)));
            this.Template = new() { VisualTree = factory };
            //これは双方向Bindingが必要
            SetBinding(TTTextProperty, new Binding(nameof(MyData.Text)) { Mode = BindingMode.TwoWay });

        }
    }
    public class TTRectangle : TThumbItem
    {
        #region プロパティ

        //public double TTWidth
        //{
        //    get { return (double)GetValue(TTWidthProperty); }
        //    set { SetValue(TTWidthProperty, value); }
        //}
        //public static readonly DependencyProperty TTWidthProperty =
        //    DependencyProperty.Register(nameof(TTWidth), typeof(double), typeof(TTRectangle),
        //        new FrameworkPropertyMetadata(100.0,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure));

        //public double TTHeight
        //{
        //    get { return (double)GetValue(TTHeightProperty); }
        //    set { SetValue(TTHeightProperty, value); }
        //}
        //public static readonly DependencyProperty TTHeightProperty =
        //    DependencyProperty.Register(nameof(TTHeight), typeof(double), typeof(TTRectangle),
        //        new FrameworkPropertyMetadata(100.0,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Brush TTFillBrush
        {
            get { return (Brush)GetValue(TTFillBrushProperty); }
            set { SetValue(TTFillBrushProperty, value); }
        }
        public static readonly DependencyProperty TTFillBrushProperty =
            DependencyProperty.Register(nameof(TTFillBrush), typeof(Brush), typeof(TTRectangle),
                new FrameworkPropertyMetadata(Brushes.Red,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        #endregion プロパティ

        public new DataRectangle MyData { get; set; } = new();
        public TTRectangle()
        {
            DataContext = MyData;
            SetTemplateAndBinding();
        }
        public TTRectangle(DataRectangle data)
        {
            MyData = data;
            DataContext = MyData;
            SetTemplateAndBinding();
        }
        internal override void SetTemplateAndBinding()
        {
            FrameworkElementFactory fContent = new(typeof(Rectangle));
            fContent.SetValue(Shape.FillProperty, new Binding(nameof(MyData.FillBrush)));
            fContent.SetValue(WidthProperty, new Binding(nameof(MyData.W)));
            fContent.SetValue(HeightProperty, new Binding(nameof(MyData.H)));
            this.Template = new() { VisualTree = fContent };

            //これも双方向Binding指定が必要
            SetBinding(WidthProperty, new Binding(nameof(MyData.W)) { Mode = BindingMode.TwoWay });
            SetBinding(HeightProperty, new Binding(nameof(MyData.H)) { Mode = BindingMode.TwoWay });
            SetBinding(TTFillBrushProperty, new Binding(nameof(MyData.FillBrush)) { Mode = BindingMode.TwoWay });

        }

    }

    [ContentProperty(nameof(Children))]
    public class TTGroup : TThumb
    {
        public new DataGroup MyData { get; set; } = new();
        public ObservableCollection<TThumb> Children { get; set; } = new();

        public TTGroup()
        {
            DataContext = MyData;
            SetTemplateAndBinding();
            Children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            var nItem = e.NewItems?[0] as TThumb;
            var oItem = e.OldItems?[0] as TThumb;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (nItem != null) { MyData.Datas?.Add(nItem.MyData); }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (oItem != null) { MyData.Datas?.Remove(oItem.MyData); }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        public TTGroup(DataGroup data)
        {
            MyData = data;
            DataContext = MyData;
        }
        internal override void SetTemplateAndBinding()
        {
            FrameworkElementFactory factory = new(typeof(ItemsControl));
            factory.SetValue(ItemsControl.ItemsPanelProperty,
                new ItemsPanelTemplate(new FrameworkElementFactory(typeof(Canvas))));
            factory.SetValue(ItemsControl.ItemsSourceProperty, new Binding(nameof(Children)) { Source = this });
            this.Template = new() { VisualTree = factory };
        }
    }



}
