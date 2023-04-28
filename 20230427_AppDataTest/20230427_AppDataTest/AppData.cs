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

    //[DataContract]//これはいらない、というかあるとDependencyObjectはシリアル化できないって言われる
    public class AppData : DependencyObject, IExtensibleDataObject
    {
        public AppData()
        {

        }

        //public override string ToString()
        //{
        //    return Name;
        //    //return base.ToString();
        //}

        public ExtensionDataObject? ExtensionData { get; set; }

        [DataMember] public string Name { get; set; } = "nemo";

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

        public double AppTop
        {
            get { return (double)GetValue(AppTopProperty); }
            set { SetValue(AppTopProperty, value); }
        }
        public static readonly DependencyProperty AppTopProperty =
            DependencyProperty.Register(nameof(AppTop), typeof(double), typeof(AppData),
                new FrameworkPropertyMetadata(100.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public string AppText
        {
            get { return (string)GetValue(AppTextProperty); }
            set { SetValue(AppTextProperty, value); }
        }
        public static readonly DependencyProperty AppTextProperty =
            DependencyProperty.Register(nameof(AppText), typeof(string), typeof(AppData),
                new FrameworkPropertyMetadata("none",
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public string AppText2
        {
            get { return (string)GetValue(AppText2Property); }
            set { SetValue(AppText2Property, value); }
        }
        public static readonly DependencyProperty AppText2Property =
            DependencyProperty.Register(nameof(AppText2), typeof(string), typeof(AppData),
                new FrameworkPropertyMetadata("Text2 NewData",
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


    }

}
