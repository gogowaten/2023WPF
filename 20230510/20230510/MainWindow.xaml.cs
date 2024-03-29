﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
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
        
        public ObservableCollection<Data> MyDatas { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            MyDatas = new() { new Data() { TTX = 100, TTY = 20, }, new Data() { TTX = 200, TTY = 30, }};
            DataContext = MyDatas;
            GridThumb gt = new();
        }





        #region click

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {

            var ap0 = MyThumb3.MyAnchorPoints;
            var ap1 = MyThumb3.MyShapeCanvas.MyAnchorPoints;
            var ap2 = MyThumb3.MyShapeCanvas.MyGeoShape.MyAnchorPoints;
           var re1= MyShapeCanvas2.MyPointRect;
            var re2 = MyShapeCanvas2.MyGeoShape.MyRnderBounds;
        }

        #region Binding維持できてる
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyShapeCanvas2.MyAnchorPoints.Add(new Point(0, 120));
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //MyTestContent.TTData.StrokeWidth = 10;
            //MyTestContent.TTData.AnchorPoints.Add(new Point(120, 100));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //MyTestContent.MyShape.StrokeThickness = 20;
            //MyTestContent.MyShape.AnchorPoints.Add(new Point(0, 0));
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

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
          
        }
    }
}
