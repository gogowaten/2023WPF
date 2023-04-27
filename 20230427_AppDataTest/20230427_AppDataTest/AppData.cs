using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _20230427_AppDataTest
{
    [DataContract]
    public class AppDatas : DependencyObject
    {
        [DataMember] public ObservableCollection<AppData> Datas { get; set; }
        public AppDatas() {
            Datas = new ObservableCollection<AppData>();
            for (int i = 0; i < 10; i++)
            {
                Datas.Add(new AppData());
            }
        }
    }


    [DataContract]
    public class AppData : DependencyObject, IExtensibleDataObject
    {
        public AppData()
        {

        }

        public ExtensionDataObject? ExtensionData { get; set; }// => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Name { get; set; } = "nemo";

        [DataMember]
        public double AppLeft
        {
            get { return (double)GetValue(AppLeftProperty); }
            set { SetValue(AppLeftProperty, value); }
        }
        public static readonly DependencyProperty AppLeftProperty =
            DependencyProperty.Register(nameof(AppLeft), typeof(double), typeof(AppData),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



    }

}
