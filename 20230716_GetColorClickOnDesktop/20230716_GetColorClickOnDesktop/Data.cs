using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Windows.Media;
using System.ComponentModel;

namespace _20230716_GetColorClickOnDesktop
{
    public class Data : INotifyPropertyChanged
    {
        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private BitmapSource? bitmap;
        public BitmapSource Bitmap { get => bitmap; set => SetProperty(ref bitmap, value); }


        private SolidColorBrush brush = Brushes.Transparent;
        public SolidColorBrush Brush { get => brush; set => SetProperty(ref brush, value); }

        private SolidColorBrush brushOfClicked = Brushes.Transparent;
        public SolidColorBrush BrushOfClicked { get => brushOfClicked; set => SetProperty(ref brushOfClicked, value); }


        private bool isCapture;
        public bool IsCapture { get => isCapture; set => SetProperty(ref isCapture, value); }

    }
}
