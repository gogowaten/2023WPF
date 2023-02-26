using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace _20230224_GeometryAnchorThumbs
{
    public class NotifyPoint : INotifyPropertyChanged

    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private double _x;
        public double X { get => _x; set => SetProperty(ref _x, value); }

        private double _y;
        public double Y { get => _y; set => SetProperty(ref _y, value); }
        public NotifyPoint() { }
        public NotifyPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
        public Point ToPoint()
        {
            return new Point(_x, _y);
        }
    }
}
