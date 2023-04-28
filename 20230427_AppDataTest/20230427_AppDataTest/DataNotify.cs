using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _20230427_AppDataTest
{
    //DependencyObjectと違って[DataContract]必要
    [DataContract]
    class DataNotify : INotifyPropertyChanged, IExtensibleDataObject
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public DataNotify() { }


        //private double _left;
        //[DataMember] public double Left { get => _left; set => SetProperty(ref _left, value); }

        //private double _top;
        //[DataMember] public double Top { get => _top; set => SetProperty(ref _top, value); }

        private string _dataName = "nemo";
        [DataMember] public string DataName { get => _dataName; set => SetProperty(ref _dataName, value); }
        public ExtensionDataObject? ExtensionData { get; set; }
    }
}
