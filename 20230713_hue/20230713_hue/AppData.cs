using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _20230713_hue
{
    public class AppData : INotifyPropertyChanged
    {

        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private bool? isHueAdd = true;
        public bool? IsHueAdd { get => isHueAdd; set => SetProperty(ref isHueAdd, value); }

        private bool? isSatAdd = true;
        public bool? IsSatAdd { get => isSatAdd; set => SetProperty(ref isSatAdd, value); }

        private bool? isLumAdd = true;
        public bool? IsLumAdd { get => isLumAdd; set => SetProperty(ref isLumAdd, value); }


        private BitmapSource? myBitmap;
        public BitmapSource? MyBitmap { get => myBitmap; set => SetProperty(ref myBitmap, value); }

        #region 範囲指定

        private double hueMin;
        public double HueMin { get => hueMin; set => SetProperty(ref hueMin, value); }

        private double hueMax = 360.0;
        public double HueMax { get => hueMax; set => SetProperty(ref hueMax, value); }

        private double satMin;
        public double SatMin { get => satMin; set => SetProperty(ref satMin, value); }

        private double satMax = 1.0;
        public double SatMax { get => satMax; set => SetProperty(ref satMax, value); }

        private double lumMin;
        public double LumMin { get => lumMin; set => SetProperty(ref lumMin, value); }

        private double lumMax = 1.0;
        public double LumMax { get => lumMax; set => SetProperty(ref lumMax, value); }
        #endregion 範囲指定

        #region 変換指定

        private double hueChange;
        public double HueChange { get => hueChange; set => SetProperty(ref hueChange, value); }

        //private double hueChangeMax;
        //public double HueChangeMax { get => hueChangeMax; set => SetProperty(ref hueChangeMax, value); }

        private double satChange;
        public double SatChange { get => satChange; set => SetProperty(ref satChange, value); }

        //private double satCahngeMax;
        //public double SatChangeMax { get => satCahngeMax; set => SetProperty(ref satCahngeMax, value); }

        private double lumCahnge;
        public double LumChange { get => lumCahnge; set => SetProperty(ref lumCahnge, value); }

        //private double lumCahngeMax;
        //public double LumChangeMax { get => lumCahngeMax; set => SetProperty(ref lumCahngeMax, value); }

        #endregion 変換指定

        public AppData()
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
