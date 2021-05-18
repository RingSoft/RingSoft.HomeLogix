using System;
using System.Collections.Generic;
using System.Text;

namespace RingSoft.HomeLogix.Library.ViewModels.Main
{
    public class MainBudgetLookup
    {
        public string Description { get; set; }

        public decimal MonthlyAmount { get; set; }

        public decimal MonthToDateAmount { get; set; }

        public decimal MonthlyDifference { get; set; }
    }
}
