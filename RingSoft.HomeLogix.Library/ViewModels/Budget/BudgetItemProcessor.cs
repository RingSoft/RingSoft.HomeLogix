using System;
using System.Globalization;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BudgetItemProcessorData
    {
        public BudgetItem BudgetItem { get; set; }

        public decimal YearlyAmount { get; set; }

        public decimal CurrentMonthPercent { get; set; }

        public decimal MonthToDatePercent { get; set; }

        public decimal MonthlyPercentDifference { get; set; }

        public decimal MonthlyAmountRemaining { get; set; }
    }

    public static class BudgetItemProcessor
    {
        public static void CalculateBudgetItem(BudgetItemProcessorData processorData)
        {
            var budgetItem = processorData.BudgetItem;

            var monthlyAmount = CalculateBudgetItemMonthlyAmount(budgetItem);
            if (processorData.BudgetItem.DoEscrow)
            {

            }

            processorData.YearlyAmount = Math.Round(monthlyAmount * 12,
                CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
        }

        public static decimal CalculateBudgetItemMonthlyAmount(BudgetItem budgetItem)
        {
            var dailyAmount = (decimal) 0;
            var monthlyAmount = (decimal) 0;

            switch (budgetItem.RecurringType)
            {
                case BudgetItemRecurringTypes.Days:
                    dailyAmount = budgetItem.Amount / budgetItem.RecurringPeriod;
                    break;
                case BudgetItemRecurringTypes.Weeks:
                    //Convert To amount per week.
                    dailyAmount = budgetItem.Amount / budgetItem.RecurringPeriod;

                    //Convert to amount per day.
                    dailyAmount = dailyAmount / 7;
                    break;
                case BudgetItemRecurringTypes.Months:
                    monthlyAmount = budgetItem.Amount / budgetItem.RecurringPeriod;
                    break;
                case BudgetItemRecurringTypes.Years:
                    //Convert to amount per year.
                    monthlyAmount = budgetItem.Amount / budgetItem.RecurringPeriod;

                    //Convert to amount per month.
                    monthlyAmount = monthlyAmount / 12;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (monthlyAmount.Equals(0))
                monthlyAmount = dailyAmount * 30;

            budgetItem.MonthlyAmount = Math.Round(monthlyAmount, CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
            return monthlyAmount;
        }
    }
}
