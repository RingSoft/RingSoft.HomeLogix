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

        public double BudgetIncome { get; set; }

        public double BudgetExpenses { get; set; }

        public double ActualIncome { get; set; }

        public double ActualExpenses { get; set; }

        public double YtdIncome { get; set; }

        public double YtdExpenses { get; set; }
    }
}
