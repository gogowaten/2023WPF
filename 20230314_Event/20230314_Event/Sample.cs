using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _20230314_Event
{
    class Sample
    {
        public event EventHandler Time;
        public void Start()
        {
            Thread.Sleep(1000);
            if(Time != null)
            {
                Time(this, EventArgs.Empty);
            }
        }

    }
  



}
