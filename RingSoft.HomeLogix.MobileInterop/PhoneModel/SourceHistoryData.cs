using System;

namespace RingSoft.HomeLogix.Library.PhoneModel
{
    public class SourceHistoryData
    {
        public int HistoryId { get; set; }

        public string Source { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string BankText { get; set; }
    }
}
