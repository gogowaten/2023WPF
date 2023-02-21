using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows;


namespace TTPolylineZ
{
    public class Data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //private PointCollection _pointCollection = new();
        //public PointCollection PointCollection { get => _pointCollection; set => SetProperty(ref _pointCollection, value); }
        public PointCollection PointCollection { get; set; }
        //public ObservableCollection<Point> ObPoints { get; set; } = new();

        private double _strokeThickness;
        public double StrokeThickness { get => _strokeThickness; set => SetProperty(ref _strokeThickness, value); }

        private Brush _stroke = Brushes.Red;
        public Brush Stroke { get => _stroke; set => SetProperty(ref _stroke, value); }

        private Brush _fill = Brushes.DarkOrange;
        public Brush Fill { get => _fill; set => SetProperty(ref _fill, value); }

        private HeadType _beginHeadType;
        public HeadType HeadBeginType { get => _beginHeadType; set => SetProperty(ref _beginHeadType, value); }

        private HeadType _endHeadType;
        public HeadType HeadEndType { get => _endHeadType; set => SetProperty(ref _endHeadType, value); }

        private double _headAngle;
        public double HeadAngle { get => _headAngle; set => SetProperty(ref _headAngle, value); }

        public Data()
        {
            PointCollection = new();

        }
    }
}
