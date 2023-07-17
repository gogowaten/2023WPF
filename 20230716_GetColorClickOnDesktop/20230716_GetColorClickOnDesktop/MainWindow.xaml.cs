using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

//WinAPIとWPF(.NET 6.0)だけで画面上のカーソル位置の色取得してみた - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/2023/07/17/132509

//参照の追加を使わずにWPFだけでデスクトップ画面でのカーソル位置の色を取得

namespace _20230716_GetColorClickOnDesktop
{
    public partial class MainWindow : Window
    {
        #region API

        //座標取得用
        private struct POINT
        {
            public int X;
            public int Y;
            public override readonly string ToString()
            {
                return $"({X}, {Y})";
            }
        }

        //DC(デバイスコンテキスト)取得
        //nullを渡すと画面全体のDCを取得、ウィンドウハンドルを渡すとそのウィンドウのクライアント領域DC
        //失敗した場合の戻り値はnull
        //使い終わったらReleaseDC
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        //渡したDCに互換性のあるDC作成
        //失敗した場合の戻り値はnull
        //使い終わったらDeleteDC
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        //指定されたDCに関連付けられているデバイスと互換性のあるビットマップを作成
        //使い終わったらDeleteObject
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int cx, int cy);

        //DCにオブジェクトを指定する、オブジェクトの種類はbitmap、brush、font、pen、Regionなど
        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr h);

        //画像転送
        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdc, int x, int y, int cx, int cy, IntPtr hdcSrc, int x1, int y1, uint rop);
        private const int SRCCOPY = 0x00cc0020;
        private const int SRCINVERT = 0x00660046;


        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr ho);

        //マウスカーソル系API
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);


        //クリックされているか判定用
        [DllImport("user32.dll")] private static extern short GetKeyState(int nVirtkey);

        #endregion API

        private BitmapSource? MyBitmap;//デスクトップ画面画像
        private POINT MyCursorPoint;//カーソル位置
        private readonly Data MyData;
        private readonly DispatcherTimer MyTimer;
        private SolidColorBrush MyBackupBrush = Brushes.Transparent;

        public MainWindow()
        {
            InitializeComponent();
            MyData = new();
            this.DataContext = MyData;
            //10ミリ秒ごとに動くタイマー
            MyTimer = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = new(0, 0, 0, 0, 10)
            };
            MyTimer.Tick += MyTimer_Tick;
        }

        //タイマー起動中動作
        private void MyTimer_Tick(object? sender, EventArgs e)
        {
            //マウスカーソルの位置の色を取得、表示
            GetCursorPos(out MyCursorPoint);
            //MyData.Bitmap = ScreenCapture();//リアルタイム更新はあかん、メモリがいくつあっても足りん

            Color myColor = GetPixelColor(MyBitmap, MyCursorPoint.X, MyCursorPoint.Y);
            MyData.Brush = new SolidColorBrush(myColor);

            //左クリックされたらデスクトップ画面をキャプチャして色を取得
            if (GetKeyState(0x01) < 0)
            {
                MyBitmap = ScreenCapture();
                MyData.BrushOfClicked = new SolidColorBrush(myColor);
            }
        }


        /// <summary>
        /// 画像の指定した1ピクセルの色を返す、ピクセルフォーマットがBgra32の画像専用
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>画像がnullだった場合は黒を返す</returns>
        private static Color GetPixelColor(BitmapSource? bmp, int x, int y)
        {
            if (bmp == null) return Colors.Black;

            CroppedBitmap cropbmp = new(bmp, new Int32Rect(x, y, 1, 1));
            byte[] pixels = new byte[4];
            cropbmp.CopyPixels(pixels, 4, 0);
            return Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]);
        }


        //仮想画面全体の画像取得
        private static BitmapSource ScreenCapture()
        {
            var screenDC = GetDC(IntPtr.Zero);//仮想画面全体のDC、コピー元
            var memDC = CreateCompatibleDC(screenDC);//コピー先DC作成
            int width = (int)SystemParameters.VirtualScreenWidth;
            int height = (int)SystemParameters.VirtualScreenHeight;
            var hBmp = CreateCompatibleBitmap(screenDC, width, height);//コピー先のbitmapオブジェクト作成
            SelectObject(memDC, hBmp);//コピー先DCにbitmapオブジェクトを指定

            //コピー元からコピー先へビットブロック転送
            //通常のコピーなのでSRCCOPYを指定
            BitBlt(memDC, 0, 0, width, height, screenDC, 0, 0, SRCCOPY);
            //bitmapオブジェクトからbitmapSource作成
            BitmapSource source =
                Imaging.CreateBitmapSourceFromHBitmap(
                    hBmp,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            //後片付け
            DeleteObject(hBmp);
            _ = ReleaseDC(IntPtr.Zero, screenDC);
            _ = ReleaseDC(IntPtr.Zero, memDC);

            //画像
            return source;
        }

        //開始
        private void ButtonBegin_Click(object sender, RoutedEventArgs e)
        {
            ColorCaptureBegin();
        }

        private void ColorCaptureBegin()
        {
            MyBitmap = ScreenCapture();

            if (MyTimer.IsEnabled == false)
            {
                MyTimer.Start();
                MyTextBlock.Text = "＊ 色取得中 ＊";
                MyTextBlock.Background = Brushes.Red;
                this.Topmost = true;//アプリのウィンドウを常に最前面
            }
        }

        //終了
        private void ButtonEnd_Click(object sender, RoutedEventArgs e)
        {
            ColorCaptureEnd();
        }

        private void ColorCaptureEnd()
        {
            if (MyTimer.IsEnabled)
            {
                MyTimer.Stop();
                //取得した色がボタンの色になるので、バックアップから元の色に戻す
                MyData.BrushOfClicked = MyBackupBrush;
                MyData.Brush = Brushes.Transparent;
                MyTextBlock.Text = "";
                MyTextBlock.Background = SystemColors.WindowBrush;
                this.Topmost = false;
            }

        }

        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MyBackupBrush = MyData.BrushOfClicked;
        }
    }

    public class Data : INotifyPropertyChanged
    {
        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private SolidColorBrush brush = Brushes.Transparent;
        public SolidColorBrush Brush { get => brush; set => SetProperty(ref brush, value); }

        private SolidColorBrush brushOfClicked = Brushes.Transparent;
        public SolidColorBrush BrushOfClicked { get => brushOfClicked; set => SetProperty(ref brushOfClicked, value); }

    }


    public class MyConverterBrushToText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush scb = (SolidColorBrush)value;
            return scb.Color.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
