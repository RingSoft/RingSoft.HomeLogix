using System;
using System.Collections.Generic;
using System.Text;

namespace RingSoft.HomeLogix.Library.ViewModels.Main
{
    public class MainBankLookup
    {
        public string Description { get; set; }

        public decimal CurrentBalance { get; set; }

        public  decimal ProjectedLowestBalance { get; set; }

        public DateTime ProjectedLowestBalanceDate { get; set; }
    }
}
