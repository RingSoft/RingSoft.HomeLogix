using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RingSoft.HomeLogix.DataAccess.LookupModel
{
    public class SourceHistoryLookup
    {
        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public string Source { get; set; }

        public string BankText { get; set; }

        public int HistoryId { get; set; }
    }
}
