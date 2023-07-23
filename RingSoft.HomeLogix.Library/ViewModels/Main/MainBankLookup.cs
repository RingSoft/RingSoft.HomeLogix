using System;
using System.Collections.Generic;
using System.Text;

namespace RingSoft.HomeLogix.Library.ViewModels.Main
{
    public class MainBankLookup
    {
        public int BankId { get; set; }

        public string Description { get; set; }

        public double CurrentBalance { get; set; }

        public  double ProjectedLowestBalance { get; set; }

        public DateTime ProjectedLowestBalanceDate { get; set; }

        public byte AccountType { get; set; }
    }
}
