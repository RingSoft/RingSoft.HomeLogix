using RingSoft.App.Library;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BudgetItemProcessorData
    {
        public BudgetItem BudgetItem { get; set; }

        public double YearlyAmount { get; set; }

        public double CurrentMonthPercent { get; set; }

        public double MonthToDatePercent { get; set; }

        public double MonthlyPercentDifference { get; set; }

        public double MonthlyAmountRemaining { get; set; }
    }

    public class BudgetItemBankAccountData
    {
        public BudgetItem BudgetItem { get; }

        public double DbMonthlyAmount { get; }

        public int DbBankAccountId { get; }

        public BankAccount DbBankAccount { get; set; }

        public int? DbTransferToBankId { get; }

        public BankAccount DbTransferToBank { get; set; }

        public BudgetItemBankAccountData(BudgetItem budgetItem, double dbMonthlyAmount, int dbBankAccountId, int? dbTransferToBankId)
        {
            BudgetItem = budgetItem;
            DbMonthlyAmount = dbMonthlyAmount;
            DbBankAccountId = dbBankAccountId;
            DbTransferToBankId = dbTransferToBankId;
        }
    }

    public static class BudgetItemProcessor
    {
        public static void CalculateBudgetItem(BudgetItemProcessorData processorData)
        {
            var budgetItem = processorData.BudgetItem;

            var monthlyAmount = CalculateBudgetItemMonthlyAmount(budgetItem);

            switch ((BudgetItemRecurringTypes)processorData.BudgetItem.RecurringType)
            {
                case BudgetItemRecurringTypes.Days:
                case BudgetItemRecurringTypes.Weeks:
                    if (processorData.BudgetItem.LastCompletedDate != null)
                    {
                        var currentDays = DateTime.DaysInMonth(
                            processorData.BudgetItem.LastCompletedDate.Value.Year,
                            processorData.BudgetItem.LastCompletedDate.Value.Month);

                        var currentMonthPercent =
                            (double) processorData.BudgetItem.LastCompletedDate.Value.Day / currentDays;

                        processorData.CurrentMonthPercent = Math.Round(currentMonthPercent, 4);
                        if (processorData.BudgetItem.MonthlyAmount > 0)
                            processorData.MonthToDatePercent =
                                Math.Round(
                                    processorData.BudgetItem.CurrentMonthAmount / processorData.BudgetItem.MonthlyAmount,
                                4);
                        else
                        {
                            processorData.MonthToDatePercent = 0;
                        }

                        processorData.MonthlyPercentDifference =
                            processorData.CurrentMonthPercent - processorData.MonthToDatePercent;
                    }
                    break;
                case BudgetItemRecurringTypes.Months:
                case BudgetItemRecurringTypes.Years:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            processorData.MonthlyAmountRemaining = processorData.BudgetItem.MonthlyAmount -
                                                   processorData.BudgetItem.CurrentMonthAmount;

            processorData.YearlyAmount = Math.Round(monthlyAmount * 12,
                CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
        }

        public static double CalculateBudgetItemMonthlyAmount(BudgetItem budgetItem)
        {
            var dailyAmount = (double)0;
            var monthlyAmount = (double)0;

            switch ((BudgetItemRecurringTypes)budgetItem.RecurringType)
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
                    if (!(budgetItem.RecurringPeriod > 1))
                        monthlyAmount = budgetItem.Amount / budgetItem.RecurringPeriod;
                    break;
                case BudgetItemRecurringTypes.Years:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (monthlyAmount.Equals(0))
                monthlyAmount = dailyAmount * 30;

            var decimalMonthlyAmount = (decimal)monthlyAmount;

            budgetItem.MonthlyAmount = (double)decimalMonthlyAmount.RoundCurrency();

            return monthlyAmount;
        }

        public static IEnumerable<BankAccountRegisterItem> GenerateBankAccountRegisterItems(int bankAccountId,
            BudgetItem budgetItem, DateTime generateToDate)
        {
            var result = new List<BankAccountRegisterItem>();
            if (budgetItem.StartingDate == null) 
                return result;

            var amount = budgetItem.Amount;
            switch ((BudgetItemTypes)budgetItem.Type)
            {
                case BudgetItemTypes.Income:
                    break;
                case BudgetItemTypes.Expense:
                case BudgetItemTypes.Transfer:
                    amount = -amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            while (budgetItem.StartingDate <= generateToDate)
            {
                if (budgetItem.EndingDate != null && budgetItem.EndingDate < budgetItem.StartingDate)
                    break;

                var itemDate = budgetItem.StartingDate.GetValueOrDefault();
                var lastDayOfMonth = new DateTime(itemDate.Year, itemDate.Month, 1);
                lastDayOfMonth = lastDayOfMonth.AddMonths(1)
                    .AddDays(-1);

                if (budgetItem.MonthOnDay.HasValue 
                    && (BudgetItemRecurringTypes)budgetItem.RecurringType == BudgetItemRecurringTypes.Months)
                {
                    if (budgetItem.MonthOnDay.Value > lastDayOfMonth.Day)
                    {
                        itemDate = lastDayOfMonth;
                    }
                    else
                    {
                        itemDate = new DateTime(itemDate.Year, itemDate.Month, budgetItem.MonthOnDay.Value);
                    }
                }

                var registerItem = new BankAccountRegisterItem
                {
                    BankAccountId = budgetItem.BankAccountId,
                    ItemType = (int)BankAccountRegisterItemTypes.BudgetItem,
                    ItemDate = itemDate,
                    BudgetItemId = budgetItem.Id,
                    Description = budgetItem.Description,
                    ProjectedAmount = amount
                };

                result.Add(registerItem);

                if ((BudgetItemTypes)budgetItem.Type == BudgetItemTypes.Transfer && budgetItem.TransferToBankAccountId != null)
                {
                    registerItem.RegisterGuid = Guid.NewGuid().ToString();
                    registerItem.ItemType = (int) BankAccountRegisterItemTypes.TransferToBankAccount;
                    var transferToRegisterId = Guid.NewGuid().ToString();
                    var transferFromRegisterId = registerItem.RegisterGuid;

                    registerItem.TransferRegisterGuid = transferToRegisterId;

                    registerItem = new BankAccountRegisterItem
                    {
                        RegisterGuid = transferToRegisterId,
                        BankAccountId = budgetItem.TransferToBankAccountId.Value,
                        ItemType = (int)BankAccountRegisterItemTypes.TransferToBankAccount,
                        ItemDate = itemDate,
                        BudgetItemId = budgetItem.Id,
                        Description = budgetItem.Description,
                        ProjectedAmount = -amount,
                        TransferRegisterGuid = transferFromRegisterId
                    };

                    result.Add(registerItem);
                }

                AdvanceBudgetItemStartingDate(budgetItem, itemDate);
            }
            return result;
        }

        private static void AdvanceBudgetItemStartingDate(BudgetItem budgetItem, DateTime itemDate)
        {
            if (budgetItem.StartingDate == null)
                return;

            var newDate = (BudgetItemRecurringTypes)budgetItem.RecurringType switch
            {
                BudgetItemRecurringTypes.Days => itemDate.AddDays(budgetItem.RecurringPeriod),
                BudgetItemRecurringTypes.Weeks => itemDate.AddDays(budgetItem.RecurringPeriod * 7),
                BudgetItemRecurringTypes.Months => itemDate.AddMonths(budgetItem.RecurringPeriod),
                BudgetItemRecurringTypes.Years => itemDate.AddYears(budgetItem.RecurringPeriod),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (budgetItem.MonthOnDay.HasValue)
            {
                var lastDayOfMonth = new DateTime(newDate.Year, newDate.Month, 1);
                lastDayOfMonth = lastDayOfMonth.AddMonths(1)
                    .AddDays(-1);

                if (newDate.Day != budgetItem.MonthOnDay.Value
                    && budgetItem.MonthOnDay.Value < lastDayOfMonth.Day)
                {
                    newDate = new DateTime(newDate.Year, newDate.Month, budgetItem.MonthOnDay.Value);
                }
            }

            budgetItem.StartingDate = newDate;
        }
    }
}
