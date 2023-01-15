using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

namespace _20230114_SerializeWithBitmap
{
    
    // 型の識別用に用意したけど今回は未使用    
    public enum TType { None = 0, TextBlock, Group, Image, Rectangle }


    [KnownType(typeof(DataImage)), KnownType(typeof(DataGroup)), KnownType(typeof(DataTextBlock))]
    public abstract class Data : IExtensibleDataObject
    {
        public ExtensionDataObject? ExtensionData { get; set; }

        public double X;
        public double Y;
        public TType Type { get; protected set; } = TType.None;
    }


    public class DataGroup : Data
    {
        public ObservableCollection<Data> Datas { get; set; } = new();
        public DataGroup() { Type = TType.Group; }
    }


    public class DataTextBlock : Data
    {
        public string? Text;
        public DataTextBlock() { Type = TType.TextBlock; }
    }

    public class DataImage : Data
    {
        //画像、それ自体は直接シリアライズしないので[IgnoreDataMember]
        [IgnoreDataMember] public BitmapSource? ImageSource;
        //シリアライズ時の画像ファイル名に使用、Guidで一意の名前作成している
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        public DataImage() { Type = TType.Image; }
    }
}
