using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RingSoft.HomeLogix.DataAccess.LookupModel
{
    public class SourceHistoryLookup
    {
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}
