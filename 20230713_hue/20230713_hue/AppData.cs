using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        private bool? myIsHueAdd = true;
        public bool? MyIsHueAdd { get => myIsHueAdd; set => SetProperty(ref myIsHueAdd, value); }

        private bool? myIsSatAdd = true;
        public bool? MyIsSatAdd { get => myIsSatAdd; set => SetProperty(ref myIsSatAdd, value); }

        private bool? myIsLumAdd = true;
        public bool? MyIsLumAdd { get => myIsLumAdd; set => SetProperty(ref myIsLumAdd, value); }

        public AppData()
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
