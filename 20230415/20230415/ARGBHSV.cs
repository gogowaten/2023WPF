using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace _20230415
{
    public class ARGBHSV2 : INotifyPropertyChanged
    {
        #region 通知プロパティ

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private byte _a = 0;
        private byte _r = 0;
        private byte _g = 0;
        private byte _b = 0;
        private double _h = 0;
        private double _s = 0;
        private double _v = 0;
        private void RGB2HSV()
        {
            if (isHSVChangeNow) return;
            isRGBChangeNow = true;
            (H, S, V) = MathHSV.Rgb2hsv(_r, _g, _b);
            isRGBChangeNow = false;
        }
        public byte A { get => _a; set => SetProperty(ref _a, value); }
        public byte R { get => _r; set { SetProperty(ref _r, value); RGB2HSV(); } }
        public byte G { get => _g; set { SetProperty(ref _g, value); RGB2HSV(); } }
        public byte B { get => _b; set { SetProperty(ref _b, value); RGB2HSV(); } }

        private void HSV2RGB()
        {
            if (isRGBChangeNow) return;
            isHSVChangeNow = true;
            (R, G, B) = MathHSV.Hsv2rgb(_h, _s, _v);
            isHSVChangeNow = false;
        }
        public double H { get => _h; set { SetProperty(ref _h, value); HSV2RGB(); } }
        public double S { get => _s; set { SetProperty(ref _s, value); HSV2RGB(); } }
        public double V { get => _v; set { SetProperty(ref _v, value); HSV2RGB(); } }

        #endregion 依存関係プロパティ

        private bool isRGBChangeNow;
        private bool isHSVChangeNow;

        public ARGBHSV2()
        {
            //_a = 0; _r = 0; _g = 0; _b = 0; _h = 0; _s = 0; _v = 0;
            A = 0; R = 0; G = 0; B = 0; H = 0; S = 0; V = 0;
        }
        public ARGBHSV2(byte a, byte r, byte g, byte b, double h, double s, double v)
        {
            A = a; R = r; G = g; B = b; H = h; S = s; V = v;
        }
        public override string ToString()
        {
            return $"argbhsv {A}, {R}, {G}, {B}, {H}, {S}, {V}";
            //return base.ToString();
        }
    }
}
