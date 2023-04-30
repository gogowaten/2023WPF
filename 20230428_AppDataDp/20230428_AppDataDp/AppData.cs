using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _20230428_AppDataDp
{

    public class AppData : DependencyObject, IExtensibleDataObject//, INotifyPropertyChanged
    {
        public AppData() { }

        public int Nume { get; set; } = 1111;

        [DataMember]
        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }

        [DataMember] public ExtensionDataObject? ExtensionData { get; set; }

        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register(nameof(Left), typeof(double), typeof(AppData),
                new FrameworkPropertyMetadata(100.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [DataMember]
        public string Memo
        {
            get { return (string)GetValue(MemoProperty); }
            set { SetValue(MemoProperty, value); }
        }
        public static readonly DependencyProperty MemoProperty =
            DependencyProperty.Register(nameof(Memo), typeof(string), typeof(AppData),
                new FrameworkPropertyMetadata("ないです",
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public event PropertyChangedEventHandler? PropertyChanged;

        //protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        //{
        //    if (EqualityComparer<T>.Default.Equals(field, value)) return;
        //    field = value;
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //}

        //[DataMember] private string _notify = "通知ないです";
        //public string Notify { get => _notify; set => SetProperty(ref _notify, value); }

    }
}
