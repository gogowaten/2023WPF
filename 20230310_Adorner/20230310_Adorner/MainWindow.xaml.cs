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

namespace _20230310_Adorner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource bmp = MyGeoThumb.GetBitmap();
            //MyGeoGrid.RectCheck();
            //MyGeoCanvas.MyPoints[1] = new Point(50, 100);
            //MyGeoCanvas.RectCheck();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //MyGeoCanvas.MyGeometryShape.UpdateAdorner();
        }
    }
}
