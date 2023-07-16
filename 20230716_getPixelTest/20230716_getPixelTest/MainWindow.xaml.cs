using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;//Imagingで使っている
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


using System.Windows.Interop;//Imaging.CreateBitmapSourceFromHBitmap()で使っている
using System.Windows.Threading;//DispatcherTimerで使っている

//using System.ComponentModel;
//using System.Globalization;
//using System.Runtime.Serialization;
//using System.Xml;
//using System.Collections.ObjectModel;
//using Microsoft.Win32;
//using System.Diagnostics;//クリップボード更新間隔のストップウォッチとか
//using System.Drawing;
//using System.Windows.Threading;
//using Color = System.Windows.Media.Color;
//using System.Reflection.Metadata;
//using System.Windows.Automation.Provider;



//WPFとVB.NET、表示した画像をクリックした場所の色の取得はややこしい - 午後わてんのブログ
//https://gogowaten.hatenablog.com/entry/13952774

//デスクトップ画面でカーソル位置の色を取得、クリック位置の色を取得
//APIを使っているところは
//  デスクトップ画面をBitmapSourceとして取得
//  左クリックの検知
//仕様：色の取得中はアプリのウィンドウは常に最前面になる

namespace _20230716_getPixelTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region API
        #region API構造体

        //Rect取得用
        public struct RECT
        {
            //型はlongじゃなくてintが正解！！！！！！！！！！！！！！
            //longだとおかしな値になる
            public int left;
            public int top;
            public int right;
            public int bottom;
            public override readonly string ToString()
            {
                return $"横:{right - left:0000}, 縦:{bottom - top:0000}  ({left}, {top}, {right}, {bottom})";
            }
        }
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
        #endregion API構造体


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

        //[DllImport("user32.dll")]
        //private static extern bool GetCursorInfo(out CURSORINFO pci);
        //[StructLayout(LayoutKind.Sequential)]
        //struct CURSORINFO
        //{
        //    public int cbSize;
        //    public int flags;
        //    public IntPtr hCursor;
        //    public POINT ptScreenPos;
        //}

        //[DllImport("user32.dll")]
        //private static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO pIconInfo);
        //struct ICONINFO
        //{
        //    public bool fIcon;
        //    public int xHotspot;
        //    public int yHotspot;
        //    public IntPtr hbmMask;
        //    public IntPtr hbmColor;
        //}

        //クリックされているか判定用
        [DllImport("user32.dll")] private static extern short GetKeyState(int nVirtkey);

        #endregion API

        //マウスカーソル情報
        private POINT MyCursorPoint;//座標


        private readonly DispatcherTimer MyTimer;
        private BitmapSource? MyBitmap;
        private SolidColorBrush MySolidColorBrush = Brushes.Transparent;


        public MainWindow()
        {
            InitializeComponent();

            MyTimer = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = new(0, 0, 0, 0, 10)//10ミリ秒ごと
            };

            MyTimer.Tick += Timer_Tick;
        }

        //タイマー起動中動作
        private void Timer_Tick(object? sender, EventArgs e)
        {
            //マウスカーソルの位置の色を取得、表示
            GetCursorPos(out MyCursorPoint);
            Color myColor = GetPixelColor(MyBitmap, MyCursorPoint.X, MyCursorPoint.Y);
            MyTextBlock.Background = new SolidColorBrush(myColor);

            //左クリックされたらデスクトップ画面をキャプチャして色を取得
            if (GetKeyState(0x01) < 0)
            {
                MyBitmap = ScreenCapture();
                MyTextBlockClicedColor.Background = new SolidColorBrush(myColor);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource bmp = ScreenCapture();
            bool cpos = GetCursorPos(out POINT pp);
        }

        //キャプチャ開始
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
                MyTextBlock.Text = "色取得中、終了するにはendボタン";
                this.Topmost = true;//アプリのウィンドウを常に最前面
            }
        }

        //キャプチャ終了
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
                MyTextBlockClicedColor.Background = MySolidColorBrush;

                MyTextBlock.Background = Brushes.Transparent;
                MyTextBlock.Text = "";
                this.Topmost = false;
            }
        }

        //終了ボタンオス直前の処理
        //今取得している色を変数に保存
        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MySolidColorBrush = (SolidColorBrush)MyTextBlockClicedColor.Background;
        }
    }
}
