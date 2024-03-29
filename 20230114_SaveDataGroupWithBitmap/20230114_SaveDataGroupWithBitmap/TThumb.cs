﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;

namespace _20230114_SaveDataGroupWithBitmap
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
        //public Data Data { get; set; } = new();
        public TThumb()
        {

            //Data = new Data();
            //DataContext = this;
            SetBinding(Canvas.LeftProperty, new Binding() { Path = new PropertyPath(TTLeftProperty), Source = this });
            SetBinding(Canvas.TopProperty, new Binding() { Path = new PropertyPath(TTTopProperty), Source = this });
        }
    }


    [ContentProperty(nameof(Items))]
    public class TTGroup : TThumb
    {
        #region 依存プロパティ


        #endregion 依存プロパティ

        public DataGroup Data { get; set; }
        private ItemsControl MyTemplateElement;
        public ObservableCollection<TThumb> Items { get; set; } = new();

        public TTGroup()
        {
            Data = new DataGroup();
            MyTemplateElement = SetMyBinding();
        }
        public TTGroup(DataGroup data)
        {
            Data = data;
            MyTemplateElement = SetMyBinding();
        }
        #region 初期設定

        private ItemsControl SetMyBinding()
        {
            this.DataContext = Data;
            SetBinding(TTLeftProperty, nameof(Data.X));
            SetBinding(TTTopProperty, nameof(Data.Y));

            ItemsControl resultElement = SetTemplate();
            //SetBinding(TTSourceProperty, nameof(Data.Text));
            resultElement.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(nameof(Items)) { Source = this });

            return resultElement;
        }
        private ItemsControl SetTemplate()
        {
            FrameworkElementFactory factory = new(typeof(ItemsControl), "nemo");
            factory.SetValue(ItemsControl.ItemsPanelProperty, new ItemsPanelTemplate(new FrameworkElementFactory(typeof(Canvas))));
            this.Template = new() { VisualTree = factory };
            this.ApplyTemplate();
            if (Template.FindName("nemo", this) is ItemsControl element)
            { return element; }
            else { throw new ArgumentException("テンプレートの要素取得できんかった"); }
        }
        #endregion 初期設定

        public void AddItem(TThumb thumb, Data data)
        {
            Items.Add(thumb);
            Data.Datas.Add(data);
        }
        public void RemoveItem(TThumb thumb, Data data)
        {
            Items.Remove(thumb);
            Data.Datas.Remove(data);
        }
        public void AddItem(Data data)
        {
            switch (data.Type)
            {
                case TType.None:
                    break;
                case TType.TextBlock:
                    AddItem(new TTTextBlock((DataTextBlock)data), data);
                    break;
                case TType.Group:
                    break;
                case TType.Image:
                    AddItem(new TTImage((DataImage)data), data);
                    break;
                case TType.Rectangle:
                    break;
                default:
                    break;
            }
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
            SetBinding(TTTextProperty, nameof(Data.Text));
            TextBlock resultElement = SetTemplate();
            resultElement.SetBinding(TextBlock.TextProperty, nameof(Data.Text));

            return resultElement;
        }
        private TextBlock SetTemplate()
        {
            FrameworkElementFactory factory = new(typeof(TextBlock), "nemo");
            this.Template = new() { VisualTree = factory };
            this.ApplyTemplate();
            if (Template.FindName("nemo", this) is TextBlock element)
            { return element; }
            else { throw new ArgumentException("テンプレートの要素取得できんかった"); }
        }
    }
    public class TTImage : TThumb
    {
        #region 依存プロパティ

        public string TTSource
        {
            get { return (string)GetValue(TTSourceProperty); }
            set { SetValue(TTSourceProperty, value); }
        }
        public static readonly DependencyProperty TTSourceProperty =
            DependencyProperty.Register(nameof(TTSource), typeof(string), typeof(TTTextBlock),
                new FrameworkPropertyMetadata("",
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 依存プロパティ

        public DataImage Data { get; set; }
        private Image MyTemplateElement;

        public TTImage()
        {
            Data = new DataImage();
            MyTemplateElement = SetMyBinding();
        }
        public TTImage(DataImage data)
        {
            Data = data;
            MyTemplateElement = SetMyBinding();
        }
        private Image SetMyBinding()
        {
            this.DataContext = Data;
            SetBinding(TTLeftProperty, nameof(Data.X));
            SetBinding(TTTopProperty, nameof(Data.Y));
            SetBinding(TTSourceProperty, nameof(Data.ImageSource));
            Image resultElement = SetTemplate();
            resultElement.SetBinding(Image.SourceProperty, nameof(Data.ImageSource));

            return resultElement;
        }
        private Image SetTemplate()
        {
            FrameworkElementFactory factory = new(typeof(Image), "nemo");
            this.Template = new() { VisualTree = factory };
            this.ApplyTemplate();
            if (Template.FindName("nemo", this) is Image element)
            { return element; }
            else { throw new ArgumentException("テンプレートの要素取得できんかった"); }
        }
    }
}
