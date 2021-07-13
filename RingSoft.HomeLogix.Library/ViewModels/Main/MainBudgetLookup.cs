using System;
using System.Collections.Generic;
using System.Text;

namespace RingSoft.HomeLogix.Library.ViewModels.Main
{
    public class MainBudgetLookup
    {
        public string Description { get; set; }

        public string ItemType { get; set; }

        public decimal ProjectedMonthlyAmount { get; set; }

        public decimal ActualMonthlyAmount { get; set; }

        public decimal MonthlyAmountDifference { get; set; }
    }
}
