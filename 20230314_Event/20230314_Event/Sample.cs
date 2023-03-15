using System;
using System.Threading;

namespace _20230314_Event
{
    public class Sample
    {
        public event EventHandler? Time;
        public void Start()
        {
            Thread.Sleep(1000);
            Time?.Invoke(this, EventArgs.Empty);
        }

    }


    //[C#]イベントの使い方 -デリゲートと何が違う？-
    //https://yaspage.com/cs-event/
    /// <summary>
    /// プロパティの値が変更されたらイベント発生する
    /// </summary>
    public class Sample2
    {
        //イベントハンドラー
        public event Action<object, int>? TestEvent;


        private int x;
        public int X
        {
            get => x;
            set
            {
                x = value;
                TestEvent?.Invoke(this, value);
            }
        }
    }




}
