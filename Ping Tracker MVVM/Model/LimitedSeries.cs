using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ping_Tracker.Model
{
    public class LimitedSeries
    {
        public SeriesCollection Series { get; set; }

        int Limit = 0;

        public LimitedSeries(int limit)
        {
            Series = new SeriesCollection();
            Limit = limit;
        }

        public void AddToQueue(double input)
        {
            if (Series[0].Values.Count >= Limit)
                Series[0].Values.RemoveAt(0);

            Series[0].Values.Add(input);
        }
    }
}
