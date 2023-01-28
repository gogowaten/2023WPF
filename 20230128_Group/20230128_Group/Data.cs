using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _20230128_Group
{
    public enum DataType
    {
        None = 0, Root, Group, TextBlock, Image,
    }
    public class Data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public ObservableCollection<Data> ChildrenData { get; set; } = new();

        private double _x;
        public double X { get => _x; set => SetProperty(ref _x, value); }

        private double _y;
        public double Y { get => _y; set => SetProperty(ref _y, value); }

        private string? _text;
        public string? Text { get => _text; set => SetProperty(ref _text, value); }

        private Brush _background;
        public Brush Backgound { get => _background; set => SetProperty(ref _background, value); }

        private BitmapSource? _bitmapSource;
        public BitmapSource? BitmapSource { get => _bitmapSource; set => SetProperty(ref _bitmapSource, value); }

        private DataType _type;
        public DataType Type { get => _type; set => SetProperty(ref _type, value); }
        public Data(DataType type)
        {
            _type = type;
        }
    }
}
