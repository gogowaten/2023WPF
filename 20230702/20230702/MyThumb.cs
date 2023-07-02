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

namespace _20230702
{

    [ContentProperty(nameof(Items))]
    public class GroupThumb : Thumb
    {
        public ItemsControl MyItemsControl { get; set; }
        public ObservableCollection<TThumb> Items { get; private set; } = new();
        public GroupThumb()
        {
            //MyItemsControl = new();
            //MyItemsControl.ItemsPanel = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(Canvas)));
            MyItemsControl = SetMyTemplate();
            MyItemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(nameof(Items)) { Source = this });

        }

        private ItemsControl SetMyTemplate()
        {
            FrameworkElementFactory fItemsControl = new(typeof(ItemsControl), "nemo");
            FrameworkElementFactory fff = new(typeof(Canvas));
            fItemsControl.SetValue(ItemsControl.ItemsPanelProperty, new ItemsPanelTemplate(fff));
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