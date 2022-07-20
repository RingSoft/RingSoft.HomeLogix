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

        public decimal YearlyAmount { get; set; }

        public decimal CurrentMonthPercent { get; set; }

        public decimal MonthToDatePercent { get; set; }

        public decimal MonthlyPercentDifference { get; set; }

        public decimal MonthlyAmountRemaining { get; set; }
    }

    public class BudgetItemBankAccountData
    {
        public BudgetItem BudgetItem { get; }

        public decimal DbMonthlyAmount { get; }

        public int DbBankAccountId { get; }

        public BankAccount DbBankAccount { get; set; }

        public int? DbTransferToBankId { get; }

        public BankAccount DbTransferToBank { get; set; }

        public BudgetItemBankAccountData(BudgetItem budgetItem, decimal dbMonthlyAmount, int dbBankAccountId, int? dbTransferToBankId)
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

            switch (processorData.BudgetItem.RecurringType)
            {
                case BudgetItemRecurringTypes.Days:
                case BudgetItemRecurringTypes.Weeks:
                    if (processorData.BudgetItem.LastCompletedDate != null)
                    {
                        var currentDays = DateTime.DaysInMonth(
                            processorData.BudgetItem.LastCompletedDate.Value.Year,
                            processorData.BudgetItem.LastCompletedDate.Value.Month);

                        var currentMonthPercent =
                            (decimal) processorData.BudgetItem.LastCompletedDate.Value.Day / currentDays;

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

        public static decimal CalculateBudgetItemMonthlyAmount(BudgetItem budgetItem)
        {
            var dailyAmount = (decimal)0;
            var monthlyAmount = (decimal)0;

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

            budgetItem.MonthlyAmount = monthlyAmount.RoundCurrency();

            return monthlyAmount;
        }

        public static IEnumerable<BankAccountRegisterItem> GenerateBankAccountRegisterItems(int bankAccountId,
            BudgetItem budgetItem, DateTime generateToDate)
        {
            var result = new List<BankAccountRegisterItem>();
            if (budgetItem.StartingDate == null) 
                return result;

            var amount = budgetItem.Amount;
            switch (budgetItem.Type)
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

                var registerItem = new BankAccountRegisterItem
                {
                    BankAccountId = budgetItem.BankAccountId,
                    ItemType = (int)BankAccountRegisterItemTypes.BudgetItem,
                    ItemDate = budgetItem.StartingDate.Value,
                    BudgetItemId = budgetItem.Id,
                    Description = budgetItem.Description,
                    ProjectedAmount = amount
                };

                result.Add(registerItem);

                if (budgetItem.Type == BudgetItemTypes.Transfer && budgetItem.TransferToBankAccountId != null)
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
                        ItemDate = budgetItem.StartingDate.Value,
                        BudgetItemId = budgetItem.Id,
                        Description = budgetItem.Description,
                        ProjectedAmount = -amount,
                        TransferRegisterGuid = transferFromRegisterId
                    };
                    result.Add(registerItem);
                }

                AdvanceBudgetItemStartingDate(budgetItem);
            }
            return result;
        }

        private static void AdvanceBudgetItemStartingDate(BudgetItem budgetItem)
        {
            if (budgetItem.StartingDate == null)
                return;

            budgetItem.StartingDate = budgetItem.RecurringType switch
            {
                BudgetItemRecurringTypes.Days => budgetItem.StartingDate.Value.AddDays(budgetItem.RecurringPeriod),
                BudgetItemRecurringTypes.Weeks => budgetItem.StartingDate.Value.AddDays(budgetItem.RecurringPeriod * 7),
                BudgetItemRecurringTypes.Months => budgetItem.StartingDate.Value.AddMonths(budgetItem.RecurringPeriod),
                BudgetItemRecurringTypes.Years => budgetItem.StartingDate.Value.AddYears(budgetItem.RecurringPeriod),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
