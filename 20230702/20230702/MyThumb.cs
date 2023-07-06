using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Controls.Ribbon.Primitives;

namespace _20230702
{


    public class OObservableCollection<T> : ObservableCollection<TThumb>
    {
        public OObservableCollection()
        {


        }

    }

    public class IC : ItemsControl
    {
        private int myInt;

        event Action<object>? OnItemChanged;
        public int MyInt
        {
            get => myInt;
            set
            {
                myInt = value;
                OnItemChanged?.Invoke(nameof(myInt));
            }
        }
        public IC()
        {
            //ItemsSourceProperty.OverrideMetadata(typeof(IC), new FrameworkPropertyMetadata(null, OnItemSourceChanged));
            SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(ItemsControl.ItemsSourceProperty), Converter = new MyConverterItems() });
        }

        private static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IC iicc = (IC)d;
            foreach (var item in iicc.Items)
            {

            }
        }
    }

    [ContentProperty(nameof(Items))]
    public class GroupThumb : TThumb
    {

        public Rect MyRect
        {
            get { return (Rect)GetValue(MyRectProperty); }
            set { SetValue(MyRectProperty, value); }
        }
        public static readonly DependencyProperty MyRectProperty =
            DependencyProperty.Register(nameof(MyRect), typeof(Rect), typeof(GroupThumb),
                new FrameworkPropertyMetadata(Rect.Empty,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public ItemsControl MyItemsControl { get; set; }
        public ObservableCollection<TThumb> Items { get; private set; } = new();

        public GroupThumb()
        {
            MyItemsControl = SetMyTemplate();
            MyItemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(nameof(Items)) { Source = this });

            //背景色
            //Background = Brushes.Red;
            MyItemsControl.Background = Brushes.MediumAquamarine;

            Items.CollectionChanged += Items_CollectionChanged;
            Loaded += GroupThumb_Loaded;
            SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(MyRectProperty), Converter = new MyConverterRectWidth() });
            SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(MyRectProperty), Converter = new MyConverterRectHeight() });

        }

        public void FixItemsLocate()
        {
            foreach (TThumb item in Items)
            {
                item.X -= MyRect.X;
                item.Y -= MyRect.Y;
            }
        }

        private void GroupThumb_Loaded(object sender, RoutedEventArgs e)
        {
            SetMyRect();
        }

        private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems?[0] is TThumb add) add.MyParentThumb = this;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems?[0] is TThumb rem) rem.MyParentThumb = null;
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

        
        private static Rect GetMyRect(ObservableCollection<TThumb> thumbs)
        {
            if (thumbs.Count == 0) return new Rect();
            TThumb tt = thumbs[0];
            double left = tt.X; double top = tt.Y;
            double right = left + tt.Width; double bottom = top + tt.Height;
            foreach (TThumb thumb in thumbs)
            {
                double x = thumb.X; double y = thumb.Y;
                double r = x + thumb.Width; double b = y + thumb.Height;
                if (left > x) left = x;
                if (top > y) top = y;
                if (right < r) right = r;
                if (bottom < b) bottom = b;
            }
            return new Rect(left, top, right - left, bottom - top);
        }


        private ItemsControl SetMyTemplate()
        {
            FrameworkElementFactory fItemsControl = new(typeof(ItemsControl), "nemo");
            FrameworkElementFactory fItemsPanel = new(typeof(Canvas));
            fItemsControl.SetValue(ItemsControl.ItemsPanelProperty, new ItemsPanelTemplate(fItemsPanel));
            Template = new ControlTemplate() { VisualTree = fItemsControl };
            ApplyTemplate();
            return (ItemsControl)Template.FindName("nemo", this);
        }


        public void SetMyRect()
        {
            MyRect = GetMyRect(Items);
            FixItemsLocate();
            //親グループに伝播
            if (MyParentThumb is GroupThumb group) group.SetMyRect();
        }


    }


    public class RectangleThumb : CanvasBaseThumb
    {
        #region 依存関係プロパティ

        public Brush MyFillBrush
        {
            get { return (Brush)GetValue(MyFillBrushProperty); }
            set { SetValue(MyFillBrushProperty, value); }
        }
        public static readonly DependencyProperty MyFillBrushProperty =
            DependencyProperty.Register(nameof(MyFillBrush), typeof(Brush), typeof(RectangleThumb),
                new FrameworkPropertyMetadata(Brushes.Blue,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyObjWidth
        {
            get { return (double)GetValue(MyObjWidthProperty); }
            set { SetValue(MyObjWidthProperty, value); }
        }
        public static readonly DependencyProperty MyObjWidthProperty =
            DependencyProperty.Register(nameof(MyObjWidth), typeof(double), typeof(RectangleThumb),
                new FrameworkPropertyMetadata(100.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyObjHeight
        {
            get { return (double)GetValue(MyObjHeightProperty); }
            set { SetValue(MyObjHeightProperty, value); }
        }
        public static readonly DependencyProperty MyObjHeightProperty =
            DependencyProperty.Register(nameof(MyObjHeight), typeof(double), typeof(RectangleThumb),
                new FrameworkPropertyMetadata(100.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ
        public Rectangle MyRectangle { get; private set; } = new();

        public RectangleThumb()
        {
            MyCanvas.Children.Add(MyRectangle);
            SetMyBindigns();

        }

        private void SetMyBindigns()
        {
            MyRectangle.SetBinding(Rectangle.FillProperty, new Binding() { Source = this, Path = new PropertyPath(MyFillBrushProperty) });
            MyRectangle.SetBinding(Rectangle.WidthProperty, new Binding() { Source = this, Path = new PropertyPath(MyObjWidthProperty) });
            MyRectangle.SetBinding(Rectangle.HeightProperty, new Binding() { Source = this, Path = new PropertyPath(MyObjHeightProperty) });

            SetBinding(WidthProperty, new Binding() { Source = this, Path = new PropertyPath(MyObjWidthProperty) });
            SetBinding(HeightProperty, new Binding() { Source = this, Path = new PropertyPath(MyObjHeightProperty) });

        }
    }


    public class CanvasBaseThumb : TThumb
    {
        public Canvas MyCanvas { get; private set; }
        public CanvasBaseThumb()
        {
            MyCanvas = SetMyTemplate();


        }

        private Canvas SetMyTemplate()
        {
            FrameworkElementFactory fCanvas = new(typeof(Canvas), "nemo");
            Template = new ControlTemplate() { VisualTree = fCanvas };
            ApplyTemplate();
            return (Canvas)Template.FindName("nemo", this);
        }


    }
    public class TThumb : Thumb
    {
        #region 依存関係プロパティ


        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public GroupThumb? MyParentThumb
        {
            get { return (GroupThumb)GetValue(MyParentThumbProperty); }
            set { SetValue(MyParentThumbProperty, value); }
        }
        public static readonly DependencyProperty MyParentThumbProperty =
            DependencyProperty.Register(nameof(MyParentThumb), typeof(GroupThumb), typeof(TThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存関係プロパティ

        public bool MyIsMove { get; set; } = true;

        public TThumb()
        {
            SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(XProperty), Mode = BindingMode.TwoWay });
            SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(YProperty), Mode = BindingMode.TwoWay });
            DragDelta += TThumb_DragDelta;
            DragCompleted += TThumb_DragCompleted;
        }

        private void TThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (MyParentThumb is GroupThumb tt) tt.SetMyRect();
        }

        private void TThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (MyIsMove && e.OriginalSource == sender)
            {
                X += e.HorizontalChange;
                Y += e.VerticalChange;
            }
        }
    }
}