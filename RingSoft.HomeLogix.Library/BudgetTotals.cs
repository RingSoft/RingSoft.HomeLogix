namespace RingSoft.HomeLogix.Library
{
    public class BudgetTotals
    {
        public double TotalProjectedMonthlyIncome { get; set; }

        public double TotalProjectedMonthlyExpenses { get; set; }

        public double TotalActualMonthlyIncome { get; set; }

        public double TotalActualMonthlyExpenses { get; set; }

        public double YearToDateIncome { get; set; }

        public double YearToDateExpenses { get; set; }

        public bool PreviousMonthHasValues { get; set; }

        public bool NextMonthHasValues { get; set; }
    }
}
