using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace _20230222
{
    //:Canvas
    //  List<AnchorThumb>
    //  PolyBezierArrowLine2
    public class AnchorCanvas : Canvas
    {
        public List<AnchorThumb> MyThumbs;
        public PolyBezierArrowLine2 MyPolyBezierArrowLine2 { get; set; }
        public AnchorCanvas()
        {
            MyThumbs = new List<AnchorThumb>();

        }
    }

}
