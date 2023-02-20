using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace _20230213_BezierTest
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

        private PointCollection _pointCollection = new();


        public PointCollection PointCollection { get => _pointCollection; set => SetProperty(ref _pointCollection, value); }
        public ObservableCollection<Point> ObPoints { get; set; } = new();

        private double _strokeThickness;
        public double StrokeThickness { get => _strokeThickness; set => SetProperty(ref _strokeThickness, value); }

        private Brush _stroke = Brushes.Red;
        public Brush Stroke { get => _stroke; set => SetProperty(ref _stroke, value); }

        private ArrowHeadType _beginHeadType;
        public ArrowHeadType BeginHeadType { get => _beginHeadType; set => SetProperty(ref _beginHeadType, value); }

        private ArrowHeadType _endHeadType;
        public ArrowHeadType EndHeadType { get => _endHeadType; set => SetProperty(ref _endHeadType, value); }

        public Data()
        {

        }
    }
}
