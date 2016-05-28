using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ping_Tracker
{
    public class CustomQueue
    {
        public ObservableCollection<string> Queue { get; set; }

        int Limit = 0;

        public CustomQueue(int limit)
        {
            Queue = new ObservableCollection<string>();
            Limit = limit;
        }

        public void AddToQueue(string input)
        {
            if(Queue.Count >= Limit)
                Queue.RemoveAt(Limit-1);

            Queue.Insert(0,input);
        }
    }
}
