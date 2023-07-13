using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace _20230713_hue
{
    public class MyConverterNotBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? bb = (bool?)value;
            if (bb == true) return false;
            else return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? bb = (bool?)value;
            if (bb == true) return false;
            else return true;
        }
    }

    public class MyConverterHueMin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? bb = (bool?)value;
            if (bb == true) { return -360; }
            else { return 0; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    public class MyConverterSatLumMin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? bb = (bool?)value;
            if (bb == true) { return -1.0; }
            else { return 0.0; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    



}
