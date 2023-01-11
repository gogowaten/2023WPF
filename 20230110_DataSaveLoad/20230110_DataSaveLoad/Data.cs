using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _20230110_DataSaveLoad
{

    public enum TTType
    {
        None,
        Item,
        TextBlock,
        Rectangle,
        Group,
        Root,
    }

    [DataContract]
    [KnownType(typeof(TTType)),
        KnownType(typeof(DataGroup)),
        KnownType(typeof(DataRectangle)),
        KnownType(typeof(SolidColorBrush)),
        KnownType(typeof(MatrixTransform)),
        KnownType(typeof(DataText))]
    public class Data : INotifyPropertyChanged, IExtensibleDataObject
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        [DataMember] public TTType Type { get; set; }//DataMemver属性を付けないと保存されない

        private double _x;
        [DataMember] public double X { get => _x; set => SetProperty(ref _x, value); }

         private double _y;
        [DataMember] public double Y { get => _y; set => SetProperty(ref _y, value); }
        public ExtensionDataObject? ExtensionData { get; set; }//互換性維持

    }

    public class DataText : Data
    {

        [DataMember] private string? _text;
        public string? Text { get => _text; set => SetProperty(ref _text, value); }

        public DataText()
        {
            Type = TTType.TextBlock;
        }

        //public override void SetType()
        //{
        //    //Type = TTType.TextBlock;
        //}
    }
    public class DataRectangle : Data
    {

        [DataMember] private double _w = 100.0;
        public double W { get => _w; set => SetProperty(ref _w, value); }

        [DataMember] private double _h = 100.0;
        public double H { get => _h; set => SetProperty(ref _h, value); }

        [DataMember] private Brush? _fillBrush;
        public Brush? FillBrush { get => _fillBrush; set => SetProperty(ref _fillBrush, value); }
        public DataRectangle()
        {
            Type = TTType.Rectangle;
        }
        //public override void SetType()
        //{
        //    //Type = TTType.Rectangle;
        //}
    }
   
    public class DataGroup : Data
    {
        [DataMember] private ObservableCollection<Data> _datas = new();

        public DataGroup()
        {
            Type = TTType.Group;
        }

        public ObservableCollection<Data> Datas { get => _datas; set => SetProperty(ref _datas, value); }

    }
    //public class DataRoot : DataGroup
    //{

    //}

}
