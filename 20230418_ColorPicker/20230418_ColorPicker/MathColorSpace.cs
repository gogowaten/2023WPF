using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _20230418_ColorPicker
{
    public struct ARGBHSV
    {

        public byte A { get; set; } = 0;
        public byte R { get; set; } = 0;
        public byte G { get; set; } = 0;
        public byte B { get; set; } = 0;


        public double H { get; set; } = 0.0;
        public double S { get; set; } = 0.0;
        public double V { get; set; } = 0.0;
        public ARGBHSV(byte a, byte r, byte g, byte b, double h, double s, double v)
        {
            A = a; R = r; G = g; B = b; H = h; S = s; V = v;
        }
        public ARGBHSV() { }
        public override string ToString()
        {
            return $"{A}, {R}, {G}, {B}, {H}, {S}, {V}";
            //return base.ToString();
        }
    }
    public struct HSV
    {
        //public double H, S, V;
        public double H { get; set; }
        public double S { get; set; }
        public double V { get; set; }

        public HSV(double h, double s, double v)
        {
            H = h;
            S = s;
            V = v;
        }
        public override string ToString()
        {
            return $"{H}, {S}, {V}";
        }

    }
    public struct RGB
    {
        public byte R { get; set; } = 0;
        public byte G { get; set; } = 0;
        public byte B { get; set; } = 0;
        public RGB(byte r, byte g, byte b)
        {
            R = r; G = g; B = b;
        }
        public RGB() { }
        public override string ToString()
        {
            return $"{R}, {G}, {B}";
            //return base.ToString();
        }
    }

    public class MathHSV
    {
        #region Color -> HSV

        /// <summary>
        /// Color(RGB)をHSV(円柱モデル)に変換、Hの値は0fから360f、SとVは0fから1f
        /// </summary>
        /// <param name="color"></param>
        /// <returns>HSV</returns>
        public static (double h, double s, double v) Color2HSV(Color color)
        {
            return RGB2hsv(color.R, color.G, color.B);
        }
        public static HSV Color2HSV2(Color color)
        {
            return Rgb2HSV(color.R, color.G, color.B);
        }

        #endregion Color -> HSV

        #region RGB -> HSV

        /// <summary>
        /// RGBをHSV(円柱モデル)に変換、RGBそれぞれの値を指定する
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns>HSV</returns>
        public static (double h, double s, double v) RGB2hsv(byte r, byte g, byte b)
        {
            byte Max = Math.Max(r, Math.Max(g, b));
            byte Min = Math.Min(r, Math.Min(g, b));
            if (Max == 0) { return (360, 0, 0); }

            double chroma = Max - Min;
            double h;
            double s = chroma / Max;
            double v = Max / 255f;

            if (Max == Min) { h = 360f; }
            else if (Max == r)
            {
                h = 60f * (g - b) / chroma;
                if (h < 0) { h += 360f; }
            }
            else if (Max == g)
            {
                h = 60f * (b - r) / chroma + 120f;
            }
            else if (Max == b)
            {
                h = 60f * (r - g) / chroma + 240f;
            }
            else { h = 360f; }

            return (h, s, v);
        }
        public static HSV Rgb2HSV(byte r, byte g, byte b)
        {
            (double h, double s, double v) = RGB2hsv(r, g, b);
            return new HSV(h, s, v);
        }

        public static (double h, double s, double v) Rgb2hsv(double r, double g, double b)
        {
            return RGB2hsv(
                (byte)(Math.Round(r, MidpointRounding.AwayFromZero)),
                (byte)(Math.Round(g, MidpointRounding.AwayFromZero)),
                (byte)(Math.Round(b, MidpointRounding.AwayFromZero)));
        }
        public static HSV Rgb2HSV(double r, double g, double b)
        {
            return Rgb2HSV(
                (byte)(Math.Round(r, MidpointRounding.AwayFromZero)),
                (byte)(Math.Round(g, MidpointRounding.AwayFromZero)),
                (byte)(Math.Round(b, MidpointRounding.AwayFromZero)));
        }
        public static (double h, double s, double v) RGB2hsv(RGB rgb)
        {
            return RGB2hsv(rgb.R, rgb.G, rgb.B);
        }
        public static HSV RGB2HSV(RGB rgb)
        {
            return Rgb2HSV(rgb.R, rgb.G, rgb.B);
        }

        #endregion RGB -> HSV

        #region Color -> HSV(円錐モデル)

        //        プログラミング 第6弾(プログラム ) - Color Model：色をプログラムするブログ - Yahoo!ブログ
        //https://blogs.yahoo.co.jp/pspevolution7/17682985.html

        /// <summary>
        /// Color(RGB)をHSV(円錐モデル)に変換、Hの値は0fから360f、SとVは0fから1f
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static (double h, double s, double v) Color2Hsv_ConicalModel(Color color)
        {
            byte R = color.R;
            byte G = color.G;
            byte B = color.B;
            byte Max = Math.Max(R, Math.Max(G, B));
            byte Min = Math.Min(R, Math.Min(G, B));
            if (Max == 0) { return (360f, 0f, 0f); }

            double chroma = Max - Min;
            double h;
            double s = chroma / 255f;//円錐モデル
            double v = Max / 255f;

            if (Max == Min) { h = 360f; }
            else if (Max == R)
            {
                h = 60f * (G - B) / chroma;
                if (h < 0) { h += 360f; }
            }
            else if (Max == G)
            {
                h = 60f * (B - R) / chroma + 120f;
            }
            else if (Max == B)
            {
                h = 60f * (R - G) / chroma + 240f;
            }
            else { h = 360f; }

            return (h, s, v);
        }
        public static HSV Color2HSV_ConicalModel(Color color)
        {
            (double h, double s, double v) = Color2Hsv_ConicalModel(color);
            return new HSV(h, s, v);
        }
        #endregion Color -> HSV(円錐モデル)

        #region HSV(円柱モデル) -> RGB、Color

        public static (byte r, byte g, byte b) Hsv2rgb(double h, double s, double v)
        {
            Color color = HSV2Color(h, s, v);
            return (color.R, color.G, color.B);
        }
        public static (byte r, byte g, byte b) HSV2rgb(HSV hsv)
        {
            Color color = HSV2Color(hsv.H, hsv.S, hsv.V);
            return (color.R, color.G, color.B);
        }
        public static RGB HSV2RGB(HSV hsv)
        {
            var (r, g, b) = HSV2rgb(hsv);
            return new RGB(r, g, b);
        }
        public static RGB Hsv2RGB(double h, double s, double v)
        {
            var (r, g, b) = Hsv2rgb(h, s, v);
            return new RGB(r, g, b);
        }

        #endregion HSV(円柱モデル) -> RGB

        #region HSV(円柱モデル) -> Color

        /// <summary>
        /// HSV(円柱モデル)をColorに変換
        /// </summary>
        /// <param name="hsv"></param>
        /// <returns>Color</returns>
        public static Color HSV2Color(double h, double s, double v)
        {
            h = h % 360f / 60f;
            double r = v, g = v, b = v;

            if (v == 0) { return Color.FromRgb(0, 0, 0); }

            int i = (int)Math.Floor(h);
            double d = h - i;
            if (h < 1)
            {
                g *= 1f - s * (1f - d);
                b *= 1f - s;
            }
            else if (h < 2)
            {
                r *= 1f - s * d;
                b *= 1f - s;
            }
            else if (h < 3)
            {
                r *= 1f - s;
                b *= 1f - s * (1f - d);
            }
            else if (h < 4)
            {
                r *= 1f - s;
                g *= 1f - s * d;
            }
            else if (h < 5)
            {
                r *= 1f - s * (1f - d);
                g *= 1f - s;
            }
            else// if (h < 6)
            {
                g *= 1f - s;
                b *= 1f - s * d;
            }

            //return Color.FromScRgb(1f,(float)r,(float)g,(float)b);
            return Color.FromRgb(
                (byte)Math.Round(r * 255f, MidpointRounding.AwayFromZero),
                (byte)Math.Round(g * 255f, MidpointRounding.AwayFromZero),
                (byte)Math.Round(b * 255f, MidpointRounding.AwayFromZero));
        }
        public static Color HSV2Color(HSV hsv)
        {
            return HSV2Color(hsv.H, hsv.S, hsv.V);
        }
        #endregion HSV(円柱モデル) -> Color

        #region HSV(円錐モデル) -> RGB

        /// <summary>
        /// RGBをHSV(円錐モデル)に変換、RGBそれぞれの値を指定する
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static (double h, double s, double v) RGB2hsv_ConicalModel(byte r, byte g, byte b)
        {
            return Color2Hsv_ConicalModel(Color.FromRgb(r, g, b));
        }
        public static HSV RGB2HSV_ConicalModel(byte r, byte g, byte b)
        {
            return Color2HSV_ConicalModel(Color.FromRgb(r, g, b));
        }
        #endregion HSV(円錐モデル) -> RGB

        #region HSV(円錐モデル) -> Color


        /// <summary>
        /// 円錐モデルのHSVをColorに変換
        /// </summary>
        /// <param name="hsv">円錐モデルのHSV</param>
        /// <returns></returns>
        public static Color HSV_ConicalModel2Color(double h, double s, double v)
        {
            double Max = v * 255f;
            double Min = (v - s) * 255f;
            double d = Max - Min;
            double r, g, b;
            if (h < 60)
            {
                r = Max;
                g = Min + d * h / 60f;
                b = Min;
            }
            else if (h < 120)
            {
                r = Min + d * (120f - h) / 60f;
                g = Max;
                b = Min;
            }
            else if (h < 180)
            {
                r = Min;
                g = Max;
                b = Min + d * (h - 120f) / 60f;
            }
            else if (h < 240)
            {
                r = Min;
                g = Min + d * (240f - h) / 60f;
                b = Max;
            }
            else if (h < 300)
            {
                r = Min + d * (h - 240f) / 60f;
                g = Min;
                b = Max;
            }
            else
            {
                r = Max;
                g = Min;
                b = Min + d * (360f - h) / 60f;
            }
            return Color.FromRgb(
                (byte)Math.Round(r, MidpointRounding.AwayFromZero),
                (byte)Math.Round(g, MidpointRounding.AwayFromZero),
                (byte)Math.Round(b, MidpointRounding.AwayFromZero));
        }

        public static (byte r, byte g, byte b) HSV_ConicalModel2RGB(double h, double s, double v)
        {
            Color color = HSV_ConicalModel2Color(h, s, v);
            return (color.R, color.G, color.B);
        }

        #endregion HSV(円錐モデル) -> HSV

        //public override string ToString()
        //{
        //    //return base.ToString();
        //    return $"{Hue}, {Saturation}, {Value}";
        //}
        //public string ToString100()
        //{
        //    return $"{Hue:000.00}, {Saturation * 100:000.00}, {Value * 100:000.00}";
        //}
    }
}
