namespace RingSoft.HomeLogix.Library
{
    public class BudgetTotals
    {
        public decimal TotalBudgetMonthlyIncome { get; set; }

        public decimal TotalBudgetMonthlyExpenses { get; set; }

        public decimal TotalActualMonthlyIncome { get; set; }

        public decimal TotalActualMonthlyExpenses { get; set; }

        public decimal YearToDateIncome { get; set; }

        public decimal YearToDateExpenses { get; set; }

        public bool PreviousMonthHasValues { get; set; }

        public bool NextMonthHasValues { get; set; }
    }
}
