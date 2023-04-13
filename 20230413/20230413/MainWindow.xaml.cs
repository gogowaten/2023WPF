using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlLibraryCore20200620;

namespace _20230413
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 依存関係プロパティ

        public Color PickupColor
        {
            get { return (Color)GetValue(PickupColorProperty); }
            set { SetValue(PickupColorProperty, value); }
        }
        public static readonly DependencyProperty PickupColorProperty =
            DependencyProperty.Register(nameof(PickupColor), typeof(Color), typeof(MainWindow));

        public byte R
        {
            get { return (byte)GetValue(RProperty); }
            set { SetValue(RProperty, value); }
        }
        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register(nameof(R), typeof(byte), typeof(MainWindow));

        public byte G
        {
            get { return (byte)GetValue(GProperty); }
            set { SetValue(GProperty, value); }
        }
        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register(nameof(G), typeof(byte), typeof(MainWindow));

        public byte B
        {
            get { return (byte)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register(nameof(B), typeof(byte), typeof(MainWindow));

        public byte A
        {
            get { return (byte)GetValue(AProperty); }
            set { SetValue(AProperty, value); }
        }
        public static readonly DependencyProperty AProperty =
            DependencyProperty.Register(nameof(A), typeof(byte), typeof(MainWindow));


        #endregion
        private int ImageSize = 200;//100以外にすると表示が崩れる
        private bool IsHsvChanging = false;
        private bool IsRgbChanging = false;
        private double ThumbSize = 20;
        private Point PointDiff;//SV画像のクリック位置とThumbの位置の差を記録

        public MainWindow()
        {
            InitializeComponent();

            MySetBinting();
            MySetEvents();
            InitializeThumb();
            MyInitialize();

            R = 255;
            G = 0;
            B = 0;
            A = 255;
        }

        private void MySetEvents()
        {
            UpDownR.MyValueChanged += UpDownR_MyValueChanged;
            UpDownG.MyValueChanged += UpDownR_MyValueChanged;
            UpDownB.MyValueChanged += UpDownR_MyValueChanged;

            UpDownH.MyValueChanged += UpDownH_MyValueChanged;
            UpDownS.MyValueChanged += UpDownH_MyValueChanged;
            UpDownV.MyValueChanged += UpDownH_MyValueChanged;

            ImageSV.MouseLeftButtonDown += ImageSV_MouseLeftButtonDown;
            SliderHue.MouseWheel += SliderHue_MouseWheel;
        }

        //HSV変更時
        private void UpDownH_MyValueChanged(object sender, MyValuechangedEventArgs e)
        {
            IsHsvChanging = true;
            if (IsRgbChanging != true)
            {
                NumericUpDown ud = (NumericUpDown)sender;
                if (ud == UpDownH) { SetImageSV((double)UpDownH.MyValue); }

                var s = (double)UpDownS.MyValue / 100.0;
                var v = (double)UpDownV.MyValue / 100.0;

                Canvas.SetLeft(ThumbPicker, (s * ImageSize) - (ThumbSize / 2f));
                Canvas.SetTop(ThumbPicker, (v * ImageSize) - (ThumbSize / 2f));

                (R, G, B) = MathHSV.Hsv2rgb((double)UpDownH.MyValue, s, v);

            }
            IsHsvChanging = false;
        }
        //private void UpDownH_MyValueChanged(object sender, MyValuechangedEventArgs e)
        //{
        //    IsHsvChanging = true;
        //    if (IsRgbChanging != true)
        //    {
        //        NumericUpDown ud = (NumericUpDown)sender;
        //        if (ud == UpDownH) { SetImageSV((double)UpDownH.MyValue); }

        //        var s = (double)UpDownS.MyValue / ImageSize;
        //        var v = (double)UpDownV.MyValue / ImageSize;

        //        Canvas.SetLeft(ThumbPicker, (s * ImageSize) - (ThumbSize / 2f));
        //        Canvas.SetTop(ThumbPicker, (v * ImageSize) - (ThumbSize / 2f));

        //        (R, G, B) = MathHSV.Hsv2rgb((double)UpDownH.MyValue, s, v);

        //    }
        //    IsHsvChanging = false;
        //}

        //RGB変更時
        private void UpDownR_MyValueChanged(object sender, MyValuechangedEventArgs e)
        {
            IsRgbChanging = true;
            if (IsHsvChanging != true)
            {
                (double h, double s, double v) = MathHSV.RGB2hsv(R, G, B);
                if (UpDownH.MyValue != (int)h)
                {
                    UpDownH.MyValue = (int)h;
                    SetImageSV(h);
                }
                UpDownS.MyValue = (int)Math.Round(s * 100.0, MidpointRounding.AwayFromZero);
                UpDownV.MyValue = (int)Math.Round(v * 100.0, MidpointRounding.AwayFromZero);

                int x = (int)(s * ImageSize);
                int y = (int)(v * ImageSize);
                Canvas.SetLeft(ThumbPicker, x - (ThumbSize / 2f));
                Canvas.SetTop(ThumbPicker, y - (ThumbSize / 2f));
            }
            IsRgbChanging = false;
        }
        //private void UpDownR_MyValueChanged(object sender, MyValuechangedEventArgs e)
        //{
        //    IsRgbChanging = true;
        //    if (IsHsvChanging != true)
        //    {
        //        (double h, double s, double v) = MathHSV.RGB2hsv(R, G, B);
        //        if (UpDownH.MyValue != (int)h)
        //        {
        //            UpDownH.MyValue = (int)h;
        //            SetImageSV(h);
        //        }
        //        UpDownS.MyValue = (int)(s * ImageSize);
        //        UpDownV.MyValue = (int)(v * ImageSize);


        //        Canvas.SetLeft(ThumbPicker, (int)UpDownS.MyValue - (ThumbSize / 2f));
        //        Canvas.SetTop(ThumbPicker, (int)UpDownV.MyValue - (ThumbSize / 2f));
        //    }
        //    IsRgbChanging = false;
        //}

        private void SliderHue_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0) { UpDownH.MyValue--; }
            else { UpDownH.MyValue++; }
        }


        private void MySetBinting()
        {
            Binding br = new Binding();
            br.Source = this;
            br.Path = new PropertyPath(RProperty);
            br.Mode = BindingMode.TwoWay;//必須
            UpDownR.SetBinding(NumericUpDown.MyValueProperty, br);

            Binding bg = new();
            bg.Source = this;
            bg.Path = new PropertyPath(GProperty);
            bg.Mode = BindingMode.TwoWay;
            UpDownG.SetBinding(NumericUpDown.MyValueProperty, bg);

            Binding bb = new();
            bb.Source = this;
            bb.Path = new PropertyPath(BProperty);
            bb.Mode = BindingMode.TwoWay;
            UpDownB.SetBinding(NumericUpDown.MyValueProperty, bb);

            Binding ba = new();
            ba.Source = this;
            ba.Path = new PropertyPath(AProperty);
            ba.Mode = BindingMode.TwoWay;
            UpDownA.SetBinding(NumericUpDown.MyValueProperty, ba);

            MultiBinding mb = new();
            mb.Bindings.Add(br);
            mb.Bindings.Add(bg);
            mb.Bindings.Add(bb);
            mb.Bindings.Add(ba);
            mb.Mode = BindingMode.TwoWay;
            mb.Converter = new ConverterRGB2Color();
            BindingOperations.SetBinding(this, PickupColorProperty, mb);

            Binding bColor = new();
            bColor.Source = this;
            bColor.Path = new PropertyPath(PickupColorProperty);
            bColor.Mode = BindingMode.TwoWay;
            bColor.Converter = new ConverterColor2SolidBrush();
            BorderPickupColorSample.SetBinding(Border.BackgroundProperty, bColor);


            Binding bSlider = new();
            bSlider.Source = UpDownH;
            bSlider.Path = new PropertyPath(NumericUpDown.MyValueProperty);
            bSlider.Mode = BindingMode.TwoWay;
            SliderHue.SetBinding(Slider.ValueProperty, bSlider);

        }


        private void MyInitialize()
        {
            MyGrid.Margin = new Thickness(ThumbSize / 2, ThumbSize / 2, 0, 0);
            ImageHue.Source = GetImageHue(ImageSize / 2, ImageSize);
            ImageHue.Width = ImageHue.Source.Width;

            SliderHue.RenderTransform = new RotateTransform(180);
            SliderHue.RenderTransformOrigin = new Point(0.5, 0.5);

            SetImageSV(0);
            SetImageAlphaSource();

        }

        private void InitializeThumb()
        {
            ThumbPicker.Width = ThumbSize;//20
            ThumbPicker.Height = ThumbSize;
            ControlTemplate template = new ControlTemplate(typeof(Thumb));
            template.VisualTree = new FrameworkElementFactory(typeof(Grid), "tempGrid");
            ThumbPicker.Template = template;
            ThumbPicker.ApplyTemplate();

            Grid myGrid = (Grid)ThumbPicker.Template.FindName("tempGrid", ThumbPicker);
            Ellipse eBlack = new()
            {
                Width = ThumbSize,//20
                Height = ThumbSize,
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.Transparent)
            };
            Ellipse eWhite = new()
            {
                Width = ThumbSize - 2,//18
                Height = ThumbSize - 2,
                Stroke = new SolidColorBrush(Colors.White)
            };

            myGrid.Children.Add(eBlack);
            myGrid.Children.Add(eWhite);
            //myGrid.Background = new SolidColorBrush(Colors.Transparent);

            ThumbPicker.DragDelta += ThumbPicker_DragDelta;
            ThumbPicker.PreviewMouseLeftButtonDown += ThumbPicker_PreviewMouseLeftButtonDown;
            ThumbPicker.DragStarted += ThumbPicker_DragStarted;
        }

        private void ThumbPicker_DragStarted(object sender, DragStartedEventArgs e)
        {
            var h = e.HorizontalOffset;
            var v = e.VerticalOffset;
            PointDiff = new Point(h, v);
        }

        private void ImageSV_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image svImage = (Image)sender;
            Point p = e.GetPosition(svImage);
            //SV画像のクリック位置とThumbの位置の差を記録
            var xx = (double)UpDownS.MyValue * (ImageSize / 100.0);
            xx += p.X;
            var xxx = p.X - (double)UpDownS.MyValue;
            var xxxx = p.X - Canvas.GetLeft(ThumbPicker);

            PointDiff = new Point(xxxx, p.Y - Canvas.GetTop(ThumbPicker));
            var x = (int)(Math.Round(p.X / ImageSize * 100, MidpointRounding.AwayFromZero));
            var y = (int)(Math.Round(p.Y / ImageSize * 100, MidpointRounding.AwayFromZero));

            UpDownS.MyValue = x;
            UpDownV.MyValue = y;
            //Canvas.SetLeft(ThumbPicker, p.X - ThumbSize / 2.0);
            //Canvas.SetTop(ThumbPicker, p.Y - ThumbSize / 2.0);
            //            単体テストコードでコントロールのイベントを発生させる - ABCの海岸で
            //http://d.hatena.ne.jp/abcneet/20110620/1308551640
            //Thumbにクリックイベント発生させてそのままThumbのドラッグ移動開始させる
            ThumbPicker.RaiseEvent(e);
        }

        //private void ImageSV_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    Image i = (Image)sender;
        //    Point p = e.GetPosition(i);
        //    //SV画像のクリック位置とThumbの位置の差を記録
        //    PointDiff = new Point(p.X - (double)UpDownS.MyValue, p.Y - (double)UpDownV.MyValue);
        //    UpDownS.MyValue = (int)p.X;
        //    UpDownV.MyValue = (int)p.Y;

        //    //            単体テストコードでコントロールのイベントを発生させる - ABCの海岸で
        //    //http://d.hatena.ne.jp/abcneet/20110620/1308551640
        //    //Thumbにクリックイベント発生させてそのままThumbのドラッグ移動開始させる
        //    ThumbPicker.RaiseEvent(e);
        //}


        private void ThumbPicker_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //SV画像のクリック位置とThumbの位置の差を初期化
            //PointDiff = new Point();
        }

        //Thumbドラッグ移動
        private void ThumbPicker_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb marker = (Thumb)sender;
            double left = Canvas.GetLeft(marker);
            double top = Canvas.GetTop(marker);
            double nx = Canvas.GetLeft(marker) + e.HorizontalChange;
            double ny = Canvas.GetTop(marker) + e.VerticalChange;
            //SV画像のクリック位置とThumbの位置の差を加味する
            //nx += PointDiff.X;
            //ny += PointDiff.Y;

            ////移動制限、SV画像の範囲内にする
            //double lowerLimit = -ThumbSize / 2.0;
            //double upperLimit = ImageSize + lowerLimit;
            //if (nx < lowerLimit) { nx = lowerLimit; }
            //else if (nx > upperLimit) { nx = upperLimit; }
            //if (ny < lowerLimit) { ny = lowerLimit; }
            //else if (ny > upperLimit) { ny = upperLimit; }

            //セット
            //Canvas.SetLeft(marker, nx);
            //Canvas.SetTop(marker, ny);
            //彩度、明度
            var s = (int)(nx * (100.0 / ImageSize));
            var v = (int)(ny * (100.0 / ImageSize));

            UpDownS.MyValue = s;
            UpDownV.MyValue = v;
        }
        //private void ThumbPicker_DragDelta(object sender, DragDeltaEventArgs e)
        //{
        //    Thumb marker = (Thumb)sender;
        //    double nx = Canvas.GetLeft(marker) + e.HorizontalChange;
        //    double ny = Canvas.GetTop(marker) + e.VerticalChange;
        //    ////SV画像のクリック位置とThumbの位置の差を加味する
        //    //nx += PointDiff.X;
        //    //ny += PointDiff.Y;

        //    //移動制限、SV画像の範囲内にする
        //    double lowerLimit = -ThumbSize / 2.0;
        //    double upperLimit = ImageSize + lowerLimit;
        //    if (nx < lowerLimit) { nx = lowerLimit; }
        //    else if (nx > upperLimit) { nx = upperLimit; }
        //    if (ny < lowerLimit) { ny = lowerLimit; }
        //    else if (ny > upperLimit) { ny = upperLimit; }
        //    //セット
        //    Canvas.SetLeft(marker, nx);
        //    Canvas.SetTop(marker, ny);
        //    //彩度、明度
        //    UpDownS.MyValue = (int)Math.Round(nx - lowerLimit, MidpointRounding.AwayFromZero);
        //    UpDownV.MyValue = (int)Math.Round(ny - lowerLimit, MidpointRounding.AwayFromZero);
        //}

        //Hue画像作成、起動時のみ使用。使用側でStretchすればサイズは幅1、高さ6でも実用できる
        private BitmapSource GetImageHue(int w, int h)
        {
            var wb = new WriteableBitmap(w, h, 96, 96, PixelFormats.Rgb24, null);
            int stride = wb.BackBufferStride;
            var pixels = new byte[h * stride];
            wb.CopyPixels(pixels, stride, 0);
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    int p = y * stride + (x * 3);
                    (pixels[p], pixels[p + 1], pixels[p + 2])
                        = MathHSV.Hsv2rgb(y / (double)h * 360.0, 1.0, 1.0);
                }
            }
            wb.WritePixels(new Int32Rect(0, 0, w, h), pixels, stride, 0);
            return wb;
        }

        //SV画像作成
        private void SetImageSV(double hue)
        {
            var wb = new WriteableBitmap(this.ImageSize, ImageSize, 96, 96, PixelFormats.Rgb24, null);
            int stride = wb.BackBufferStride;
            var pixels = new byte[ImageSize * stride];
            wb.CopyPixels(pixels, stride, 0);
            int p = 0;
            Parallel.For(0, ImageSize, y =>
            {
                ParallelImageSV(p, y, stride, pixels, hue, this.ImageSize, ImageSize);
            });

            wb.WritePixels(new Int32Rect(0, 0, this.ImageSize, ImageSize), pixels, stride, 0);
            ImageSV.Source = wb;
        }
        private void ParallelImageSV(int p, int y, int stride, byte[] pixels, double hue, int w, int h)
        {
            for (int x = 0; x < w; ++x)
            {
                p = y * stride + (x * 3);
                (pixels[p], pixels[p + 1], pixels[p + 2])
                    = MathHSV.Hsv2rgb(hue, x / (double)w, y / (double)h);
            }
        }

        //市松模様画像
        private void SetImageAlphaSource()
        {
            int w = (int)ImageAlpha.Width;
            int h = (int)ImageAlpha.Height;
            var wb = new WriteableBitmap(w, h, 96, 96, PixelFormats.Bgr24, null);
            int stride = wb.BackBufferStride;
            var pixels = new byte[ImageSize * stride];
            wb.CopyPixels(pixels, stride, 0);
            int x, y;
            for (int i = 0; i < pixels.Length; i += 3)
            {
                x = (i / (10 * 3)) % 2;
                y = (i / 10 / stride) % 2;

                byte color;
                if ((x == 0 && y == 0) || (x == 1 && y == 1))
                {
                    color = 255;
                }
                else
                {
                    color = 200;
                }
                pixels[i] = color;
                pixels[i + 1] = color;
                pixels[i + 2] = color;
            }
            wb.WritePixels(new Int32Rect(0, 0, w, h), pixels, stride, 0);
            ImageAlpha.Source = wb;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(ThumbPicker, 50);
        }
    }




    internal class ConverterRGB2Color : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            byte r = (byte)values[0];
            byte g = (byte)values[1];
            byte b = (byte)values[2];
            byte a = (byte)values[3];
            return Color.FromArgb(a, r, g, b);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            Color c = (Color)value;
            return new object[] { c.R, c.G, c.B, c.A };
        }
    }

    internal class ConverterColor2SolidBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color c = (Color)value;
            return new SolidColorBrush(c);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = (SolidColorBrush)value;
            return brush.Color;
        }
    }
}
