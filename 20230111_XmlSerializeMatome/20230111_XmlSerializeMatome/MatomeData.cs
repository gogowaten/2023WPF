using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace _20230111_XmlSerializeMatome
{
    public enum DType { None, Text, Shape, Group }
    //すべてのDataの基本にするクラス
    [KnownType(typeof(DataText)), KnownType(typeof(DataGroup))]
    public class Data : IExtensibleDataObject
    {
        public DType DType { get; set; }
        public double X { get; set; }
        //上位下位互換性維持
        public ExtensionDataObject? ExtensionData { get; set; }

        public Data() { DType = DType.None; }
    }


    #region 派生クラス
    public class DataText : Data
    {
        public string? Text { get; set; }
        public double FontSize { get; set; }
        public DataText() { DType = DType.Text; }
    }

    //2つのKnownTypeはDataShapeは、こことMatomeDataの両方で指定しているけど、
    //シリアル化する予定のある方だけでいい
    [KnownType(typeof(SolidColorBrush)), KnownType(typeof(MatrixTransform))]
    public class DataShape : Data
    {
        public Brush? FillBrush { get; set; }
        public DataShape() { DType = DType.Shape; }
    }

    [KnownType(typeof(DataShape)), KnownType(typeof(DataText))]
    public class DataGroup : Data
    {
        public ObservableCollection<Data> Datas { get; set; } = new();
        public DataGroup() { DType = DType.Group; }
    }
    #endregion 派生クラス


    //あんまり意味なかった
    [DataContract]
    [KnownType(typeof(MatomeData)),
        KnownType(typeof(Data)),
        KnownType(typeof(DataText)),
        KnownType(typeof(DataShape)),
        KnownType(typeof(DataGroup)),
        KnownType(typeof(SolidColorBrush)),
        KnownType(typeof(MatrixTransform))]
    public class MatomeData { [DataMember] public Data? Matome { get; set; } }

}
