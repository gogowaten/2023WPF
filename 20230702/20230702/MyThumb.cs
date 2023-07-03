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

namespace _20230702
{

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

        }
    }

    [ContentProperty(nameof(Items))]
    public class GroupThumb : Thumb
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

            SetBinding(MyRectProperty, new Binding() { Source = MyItemsControl, Path = new PropertyPath(ItemsControl.ItemsSourceProperty), Converter = new MyConverterItemsRect() });

            MyItemsControl.LayoutUpdated += MyItemsControl_LayoutUpdated;
        }

        private void MyItemsControl_LayoutUpdated(object? sender, EventArgs e)
        {
            double left = double.MaxValue;
            double top = double.MaxValue;
            double right = double.MinValue;
            double bottom = double.MinValue;

            foreach (TThumb thumb in Items)
            {
                double minX = thumb.X; double minY = thumb.Y;
                if (left < minX) left = minX;
                if (top < minY) top = minY;
                if (right < minX + thumb.Width) right = minX + thumb.Width;
                if (bottom < minY + thumb.Height) bottom = minY + thumb.Height;
            }
            Rect r = new();
            if (left == double.MaxValue) left = 0;
            if(top == double.MaxValue) top = 0;
            if (Items.Count > 0) { r = new Rect(left, top, right - left, bottom - top); }
            Width = r.Width;
            Height = r.Height;
        }

        private void Items_CurrentChanging(object sender, System.ComponentModel.CurrentChangingEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Items_CurrentChanged(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
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
        #endregion 依存関係プロパティ

        public bool MyIsMove { get; set; } = true;

        public TThumb()
        {
            SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(XProperty), Mode = BindingMode.TwoWay });
            SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(YProperty), Mode = BindingMode.TwoWay });
            DragDelta += TThumb_DragDelta;
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