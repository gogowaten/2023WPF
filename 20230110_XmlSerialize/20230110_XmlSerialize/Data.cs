using System.Runtime.Serialization;
using System.Windows.Media;

namespace _20230110_XmlSerialize
{

    [DataContract]
    [KnownType(typeof(SolidColorBrush)),
        KnownType(typeof(MatrixTransform))]
    public class Data : IExtensibleDataObject
    {
        [DataMember] public double X { get; set; }
        [DataMember] public SolidColorBrush FillBrush { get; set; } = Brushes.Red;
        public ExtensionDataObject? ExtensionData { get; set; }

        //public Data() { }
    }

    [DataContract]
    [KnownType(typeof(SolidColorBrush))]
    public class DDShape : Data
    {
        [DataMember] public Brush FillBrushShape { get; set; } = Brushes.Red;
        
    }



    #region 上位下位互換性を保ってシリアル化
    //バージョン1の設定クラス
    [DataContract(Name = "SampleSettings")]
    public class SampleSettingsV1 : IExtensibleDataObject
    {
        [DataMember] public string? Message;

        public ExtensionDataObject? ExtensionData { get; set; }
    }

    //バージョン2の設定クラス
    [DataContract(Name = "SampleSettings")]
    public class SampleSettingsV2 : IExtensibleDataObject
    {
        [DataMember] public string? Message;
        //追加された設定
        [DataMember(Order = 2)] public int ID;
        //[DataMember] public SolidColorBrush Foreground { get; set; } = Brushes.Red;
        public ExtensionDataObject? ExtensionData { get; set; }
    }
    #endregion 上位下位互換性を保ってシリアル化


}
