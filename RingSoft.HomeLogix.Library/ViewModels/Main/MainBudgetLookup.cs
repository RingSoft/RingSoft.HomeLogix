using System;
using System.Collections.Generic;
using System.Text;

namespace RingSoft.HomeLogix.Library.ViewModels.Main
{
    public class MainBudgetLookup1
    {
        public int BudgetId { get; set; }

        public string Description { get; set; }

        public string ItemType { get; set; }

        public double ProjectedMonthlyAmount { get; set; }

        public double ActualMonthlyAmount { get; set; }

        public double MonthlyAmountDifference { get; set; }

        public int BudgetItemType { get; set; }
    }
}
