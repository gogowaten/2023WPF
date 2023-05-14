﻿using System;
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
//PointCollection同士の双方向Binding
//問題ない
//Pointの変更で相手側も変更される
//Collection自体を変更した場合も同じく変更される
//注意したいのはBinding時、Sourceの値が優先される

namespace _20230510
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ControlTemplate MyControlTemplate;

        public MainWindow()
        {
            InitializeComponent();

            MyControlTemplate = new ControlTemplate();
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
            MyControlTemplate.VisualTree = eBorder;
            Button myb = new()
            {
                Template = MyControlTemplate,
                Content = "button1",
                Background = Brushes.LightBlue,
                Margin = new Thickness(30)
            };
            MyStackPanel.Children.Add(myb);

            ControlTemplate template = new();
            eBorder = new(typeof(Border));
            eBorder.SetValue(Border.BackgroundProperty, new Binding() { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent), Path = new PropertyPath(BackgroundProperty) });
        }





        #region click

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            //var data = MyTestContent.TTData.StrokeWidth;
            var ttstroke = MyTestContent.StrokeThickness;
            var ttshapestroke = MyTestContent.MyShape.StrokeThickness;

            var shapeanchor = MyTestContent.MyShape.AnchorPoints;
            //var dataanchor = MyTestContent.TTData.AnchorPoints;
            var thumbanchor = MyTestContent.Anchors;
        }

        #region Binding維持できてる
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyTestContent.StrokeThickness = 1;
            MyTestContent.Anchors.Add(new Point(20, 100));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //MyTestContent.TTData.StrokeWidth = 10;
            //MyTestContent.TTData.AnchorPoints.Add(new Point(120, 100));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MyTestContent.MyShape.StrokeThickness = 20;
            MyTestContent.MyShape.AnchorPoints.Add(new Point(0, 0));
        }

        private void Button_change_pointcollection1_Click(object sender, RoutedEventArgs e)
        {
            //MyTestContent.Anchors = new PointCollection() { new Point(0, 0), new Point(100, 100) };
        }


        private void Button_change_pointcollection2_Click(object sender, RoutedEventArgs e)
        {
            //MyTestContent.TTData.AnchorPoints = new PointCollection() { new Point(0, 0), new Point(10, 100) };
        }

        private void Button_change_pointcollection3_Click(object sender, RoutedEventArgs e)
        {
            //MyTestContent.MyShape.AnchorPoints = new PointCollection() { new Point(0, 0), new Point(100, 10) };
        }

        #endregion Binding維持できてる

        #region Bindingなくなる        
        //Dataを入れ替えはBindingがなくなる
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }
        #endregion Bindingなくなる
        #endregion click
    }
}
