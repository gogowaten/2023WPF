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

namespace _2023031213_GeoShapeThumb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Left = 100;
            Top= 100;
                
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MyGeoShapeThumb2.FixLocate();
           var pc= MyGeoShapeCanvas.MyPoints;
           
            //MyGeoThumb.MyShapeType = ShapeType.Fill;
            //BitmapSource bmp = MyGeoThumb.GetBitmap();

        }
    }
}
