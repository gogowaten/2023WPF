using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Data;

namespace _20230411_ColorPicker
{
    public class ColorPicker : Grid
    {
        public Color PickupColor
        {
            get { return (Color)GetValue(PickupColorProperty); }
            set { SetValue(PickupColorProperty, value); }
        }
        public static readonly DependencyProperty PickupColorProperty =
            DependencyProperty.Register(nameof(PickupColor), typeof(Color), typeof(ColorPicker),
                new FrameworkPropertyMetadata(Color.FromArgb(0, 0, 0, 0),
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ColorPicker()
        {
            
            Loaded += ColorPicker_Loaded;
        }

        private void ColorPicker_Loaded(object sender, RoutedEventArgs e)
        {
            Border border = new() { Background = new SolidColorBrush(PickupColor) };
            Children.Add(border);
        }
    }
}
