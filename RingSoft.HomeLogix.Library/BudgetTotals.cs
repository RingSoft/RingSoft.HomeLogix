namespace RingSoft.HomeLogix.Library
{
    public class BudgetTotals
    {
        public decimal TotalProjectedMonthlyIncome { get; set; }

        public decimal TotalProjectedMonthlyExpenses { get; set; }

        public decimal TotalActualMonthlyIncome { get; set; }

        public decimal TotalActualMonthlyExpenses { get; set; }

        public decimal YearToDateIncome { get; set; }

        public decimal YearToDateExpenses { get; set; }

        public bool PreviousMonthHasValues { get; set; }

        public bool NextMonthHasValues { get; set; }
    }
}
