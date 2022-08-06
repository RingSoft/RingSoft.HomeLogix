using System;
using System.Collections.Generic;
using System.Text;

namespace RingSoft.App.Library
{
    public class ChartDataItem
    {
        public double Value { get; set; }
        public string Name { get; set; }
    }

    public class ChartData
    {
        public List<ChartDataItem> Items { get; set; }

        public string Title { get; set; }

        public ChartData()
        {
            Items = new List<ChartDataItem>();
        }
    }
}
