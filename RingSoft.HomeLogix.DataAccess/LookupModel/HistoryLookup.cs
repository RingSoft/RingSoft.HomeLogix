using System;
using System.Collections.Generic;
using System.Text;

namespace RingSoft.HomeLogix.DataAccess.LookupModel
{
    public class HistoryLookup
    {
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public byte ItemType { get; set; }

        public decimal ProjectedAmount { get; set; }

        public decimal ActualAmount { get; set; }

        public decimal Difference { get; set; }

        public string BankText { get; set; }
    }
}
