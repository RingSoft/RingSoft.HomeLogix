using System;
using System.Collections.Generic;
using System.Text;

namespace RingSoft.HomeLogix.Library.ViewModels.Main
{
    public class MainBudgetLookup
    {
        public int BudgetId { get; set; }

        public string Description { get; set; }

        public string ItemType { get; set; }

        public decimal ProjectedMonthlyAmount { get; set; }

        public decimal ActualMonthlyAmount { get; set; }

        public decimal MonthlyAmountDifference { get; set; }

        public int BudgetItemType { get; set; }
    }
}
