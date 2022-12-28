using System;

namespace RingSoft.HomeLogix.Library.PhoneModel
{
    public enum StatisticsType
    {
        Current = 0,
        Previous = 1,
    }
    public class BudgetStatistics
    {
        public StatisticsType Type { get; set; }

        public DateTime MonthEnding { get; set; }

        public decimal BudgetIncome { get; set; }

        public decimal BudgetExpenses { get; set; }

        public decimal ActualIncome { get; set; }

        public decimal ActualExpenses { get; set; }

        public decimal YtdIncome { get; set; }

        public decimal YtdExpenses { get; set; }
    }
}
