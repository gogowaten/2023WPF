using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _20230113
{
    public enum TType { None = 0, TextBlock, Group, Image, Rectangle }



    public class Data : INotifyPropertyChanged, IExtensibleDataObject
    {
        public ExtensionDataObject? ExtensionData { get; set; }

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
        public TType Type { get; protected set; } = TType.None;
    }


    public class DataGroup : Data
    {
        public ObservableCollection<Data> Datas { get; set; } = new();
    }


    public class DataTextBlock : Data
    {
        private string? _text;
        public string? Text { get => _text; set => SetProperty(ref _text, value); }
        public DataTextBlock() { Type = TType.TextBlock; }
    }
    public class DataRectangle : Data
    {
        private SolidColorBrush? _fillBrush;
        public SolidColorBrush? FillBrush { get => _fillBrush; set => SetProperty(ref _fillBrush, value); }
    }
    public class DataImage : Data
    {

        private BitmapSource? _imageSource;
        public BitmapSource? ImageSource { get => _imageSource; set => SetProperty(ref _imageSource, value); }
        //public Guid Guid { get; set; }= Guid.NewGuid();
    }
}
