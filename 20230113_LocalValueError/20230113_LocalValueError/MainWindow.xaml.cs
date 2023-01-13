using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


//20221224ではエラーになるのに、コピペしたこっちではエラーにならない
//意味がわからん

namespace _20230113_LocalValueError
{
    public partial class MainWindow : Window
    {
        private int MyAddCounnt = 0;
        public MainWindow()
        {
            InitializeComponent();
            Left = 100; Top = 100;





            //DataMyText dtext = new() { MyText = "test", FontSize = 30, X = 20, Y = 30 };

            TTTextBlock MyTTT = new();//エラーにならない
            //20221224では
            //なぜかTTTextBlockだけローカルウィンドウで内部エラーになる、初めて見た
            //TThumb.csのpublic new DataMyText? MyData { get; set; }これをはずしたときだけ正常だけど、これじゃ使えない
            //動くには動くけど、デバッグでかなり不便
            //同じように作っているTTImageやTTGroupは普通にローカル値が表示される


            //TTTextBlock MyTTTextBlock = new(dtext);
            //if (MyTTTextBlock.MyData != null)
            //{
            //    MyTTTextBlock.MySerializeData($"E:\\20230112.xml", MyTTTextBlock.MyData);
            //}
            //var data = MyTTTextBlock.MyDeserialize<Data>($"E:\\20230112.xml");

            DataImage dimage = new() { MyImage = GetBitmap("D:\\ブログ用\\テスト用画像\\collection_1.png"), X = 100, Y = 30 };
            //DataImage dimage = new() { MyImage = GetBitmap2("D:\\ブログ用\\テスト用画像\\collection_1.png"), X = 100, Y = 30 };

            TTImage MyTTImage = new(dimage);
            TTRectangle MyRect = new();

            MyRoot.Children.Add(MyTTImage);
            //MyRoot.Children.Add(MyTTTextBlock);
            //MyTTImage.MySerializeData($"E:\\20230112.xml", MyTTImage.MyData);

            //data = MyTTImage.MyDeserialize<Data>($"E:\\20230112.xml");


        }

        private BitmapSource GetBitmap(string filePath)
        {
            BitmapImage image = new(new Uri(filePath));
            return image;
        }
        private BitmapSource GetBitmap2(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var frame = BitmapFrame.Create(stream);
                return frame;
            }
        }
   
    }
}
