using System;
using System.Collections.Generic;
using System.Globalization;
using RingSoft.App.Library;
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
            if (processorData.BudgetItem.DoEscrow && budgetItem.StartingDate != null)
            {
                int months;
                switch (budgetItem.RecurringType)
                {
                    case BudgetItemRecurringTypes.Months:
                        months = budgetItem.RecurringPeriod;
                        break;
                    case BudgetItemRecurringTypes.Years:
                        months = budgetItem.RecurringPeriod * 12;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var startDate = budgetItem.StartingDate;
                //var currentDate = budgetItem.LastCompletedDate;
                //var startDate = budgetItem.StartingDate.AddMonths(-months);
                //var currentDate = budgetItem.LastCompletedDate;
                //if (currentDate == null)
                //{
                var bankAccount = AppGlobals.DataRepository.GetBankAccount(budgetItem.BankAccountId, false);
                var currentDate = bankAccount.LastGenerationDate;
                //}

                var monthsToGo =
                    RingSoftAppGlobals.CalculateMonthsInTimeSpan((DateTime)startDate, (DateTime)currentDate);

                var monthsAccrued = months - Math.Floor(monthsToGo);
                budgetItem.EscrowBalance = Math.Round(budgetItem.MonthlyAmount * (decimal)monthsAccrued,
                    CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                if (budgetItem.EscrowBalance > budgetItem.Amount)
                    budgetItem.EscrowBalance = budgetItem.Amount;
                if (budgetItem.EscrowBalance < 0)
                    budgetItem.EscrowBalance = 0;

                if (budgetItem.EndingDate != null && startDate > (DateTime)budgetItem.EndingDate)
                    budgetItem.EscrowBalance = 0;
                budgetItem.MonthlyAmount = Math.Round(budgetItem.MonthlyAmount,
                    CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
            }
            else if (!budgetItem.DoEscrow)
            {
                budgetItem.EscrowBalance = 0;
            }

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
                    if (!(budgetItem.RecurringPeriod > 1 && !budgetItem.DoEscrow))
                        monthlyAmount = budgetItem.Amount / budgetItem.RecurringPeriod;
                    break;
                case BudgetItemRecurringTypes.Years:
                    if (budgetItem.DoEscrow)
                    {
                        //Convert to amount per year.
                        monthlyAmount = budgetItem.Amount / budgetItem.RecurringPeriod;

                        //Convert to amount per month.
                        monthlyAmount = monthlyAmount / 12;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (monthlyAmount.Equals(0))
                monthlyAmount = dailyAmount * 30;

            budgetItem.MonthlyAmount = budgetItem.DoEscrow ? monthlyAmount : Math.Round(monthlyAmount, CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

            return monthlyAmount;
        }

        public static List<BankAccountRegisterItem> GenerateBankAccountRegisterItems(BudgetItem budgetItem,
            DateTime generateToDate)
        {
            var result = new List<BankAccountRegisterItem>();

            if (budgetItem.StartingDate != null)
            {
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

                while (budgetItem.StartingDate < generateToDate)
                {
                    var registerItem = new BankAccountRegisterItem
                    {
                        RegisterId = Guid.NewGuid().ToString(),
                        BankAccountId = budgetItem.BankAccountId,
                        ItemType = BankAccountRegisterItemTypes.BudgetItem,
                        ItemDate = budgetItem.StartingDate.Value,
                        BudgetItemId = budgetItem.Id,
                        BudgetItem = budgetItem,
                        TransferToBankAccountId = budgetItem.TransferToBankAccountId,
                        Description = budgetItem.Description,
                        ProjectedAmount = amount
                    };

                    switch (budgetItem.RecurringType)
                    {
                        case BudgetItemRecurringTypes.Days:
                            budgetItem.StartingDate = budgetItem.StartingDate.Value.AddDays(budgetItem.RecurringPeriod);
                            break;
                        case BudgetItemRecurringTypes.Weeks:
                            budgetItem.StartingDate =
                                budgetItem.StartingDate.Value.AddDays(budgetItem.RecurringPeriod * 7);
                            break;
                        case BudgetItemRecurringTypes.Months:
                            budgetItem.StartingDate =
                                budgetItem.StartingDate.Value.AddMonths(budgetItem.RecurringPeriod);
                            break;
                        case BudgetItemRecurringTypes.Years:
                            budgetItem.StartingDate =
                                budgetItem.StartingDate.Value.AddYears(budgetItem.RecurringPeriod);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    result.Add(registerItem);
                }
            }
            return result;
        }
    }
}
