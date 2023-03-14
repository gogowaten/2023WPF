using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;

namespace _2023031213_GeoShapeThumb
{
    public class GeoShapeThumb2 : GeoShapeThumb
    {

        //Pointsの位置修正とThumbのサイズと位置修正
        //回転などの変形には未対応なので、変形されていると位置がずれる
        public void FixLocate()
        {
            double minX = double.MaxValue, minY = double.MaxValue;
            foreach (var item in MyPoints)
            {
                if (item.X < minX) minX = item.X;
                if (item.Y < minY) minY = item.Y;
            }
            for (int i = 0; i < MyPoints.Count; i++)
            {
                Point pp = MyPoints[i];
                MyPoints[i] = new Point(pp.X - minX, pp.Y - minY);
            }
            double left = Canvas.GetLeft(this);
            double top = Canvas.GetTop(this);
            Canvas.SetLeft(this, left + minX);
            Canvas.SetTop(this, top + minY);

            MyGeometryShape.UpdateAdorner();
        }
    }


}
