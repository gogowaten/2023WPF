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

namespace _20230320_BezierSize
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
            Top = 100;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rendersize = MyBezier0.RenderSize;//160 130
            var actWidth = MyBezier0.MyBezier.ActualWidth;
            var offset = VisualTreeHelper.GetOffset(MyBezier0);

            var drawRect = VisualTreeHelper.GetDrawing(MyBezier0)?.Bounds;//-81 -10 241 140
            var beziRect = VisualTreeHelper.GetDescendantBounds(MyBezier0);//-81 -10 241 140

            //DrawingVisual dv = new();
            //using(var context = dv.RenderOpen())
            //{
            //    VisualBrush vb = new(MyBezier0);
            //    context.DrawRectangle(vb, null, new Rect(rendersize));
            //}
            RenderTargetBitmap bitmap = new((int)rendersize.Width, (int)rendersize.Height, 96, 96, PixelFormats.Pbgra32);
            //bitmap.Render(dv);
            bitmap.Render(MyBezier0);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MyBezier0.MyPoints[4] = new Point(100, 100);
            MyBezier0.MyBezier.MyAdorner?.FixThumbsLocate();

            var offset = VisualTreeHelper.GetOffset(MyBezier0);
            var rendersize = MyBezier0.RenderSize;
            var renderbezier = MyBezier0.MyBezier.RenderSize;
            var candes = VisualTreeHelper.GetDescendantBounds(MyBezier0);
            var bedes = VisualTreeHelper.GetDescendantBounds(MyBezier0.MyBezier);
            MyBezier0.UpdateLayout();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MyBezier0.FixBezierLocate();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MyBezier0.FixCanvasLocate0();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MyBezier0.MyIsEditing = !MyBezier0.MyIsEditing;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            MyBezier0.Fix0Point();
            var pt = MyBezier0.MyPoints;
            MyBezier0.MyBezier.MyAdorner?.FixThumbsLocate();
            //MyBezier0.FixCanvasLocate0();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var canrect = VisualTreeHelper.GetDescendantBounds(MyBezier0);
            var bezrect = VisualTreeHelper.GetDescendantBounds(MyBezier0.MyBezier);
            var adorect = VisualTreeHelper.GetDescendantBounds(MyBezier0.MyBezier.MyAdorner);
            var adocanrect = VisualTreeHelper.GetDescendantBounds(MyBezier0.MyBezier.MyAdorner.MyCanvas);
            var pointsrect = MyAdorner.GetPointsRect(MyBezier0.MyPoints);
            var bezexrect = MyBezier0.MyBezier.MyExternalBounds;
            MyBezier0.FixCanvasLocate2();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            MyBezier0.Fix0Point2();
            MyBezier0.MyBezier.MyAdorner?.FixThumbsLocate();
        }
    }
}
