using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

            while (budgetItem.StartingDate < generateToDate)
            {
                if (budgetItem.EndingDate != null && budgetItem.EndingDate < budgetItem.StartingDate)
                    break;

                var registerItem = new BankAccountRegisterItem
                {
                    RegisterId = Guid.NewGuid().ToString(),
                    BankAccountId = budgetItem.BankAccountId,
                    ItemType = BankAccountRegisterItemTypes.BudgetItem,
                    ItemDate = budgetItem.StartingDate.Value,
                    BudgetItemId = budgetItem.Id,
                    Description = budgetItem.Description,
                    ProjectedAmount = amount
                };

                result.Add(registerItem);

                if (budgetItem.Type == BudgetItemTypes.Transfer && budgetItem.TransferToBankAccountId != null)
                {
                    var transferToRegisterId = Guid.NewGuid().ToString();
                    var transferFromRegisterId = registerItem.RegisterId;

                    registerItem.TransferRegisterId = transferToRegisterId;

                    registerItem = new BankAccountRegisterItem
                    {
                        RegisterId = transferToRegisterId,
                        BankAccountId = budgetItem.TransferToBankAccountId.Value,
                        ItemType = BankAccountRegisterItemTypes.BudgetItem,
                        ItemDate = budgetItem.StartingDate.Value,
                        BudgetItemId = budgetItem.Id,
                        Description = budgetItem.Description,
                        ProjectedAmount = -amount,
                        TransferRegisterId = transferFromRegisterId
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

        public static IEnumerable<BankAccountRegisterItem> GenerateEscrowRegisterItems(BankAccount bankAccount,
            DateTime generateToDate)
        {
            var result = new List<BankAccountRegisterItem>();
            var escrowItems = AppGlobals.DataRepository.GetEscrowBudgetItems(bankAccount.Id).ToList();
            if (!escrowItems.Any())
                return result;

            var escrowDayOfMonth = bankAccount.EscrowDayOfMonth ?? 1;
            var lastDayOfFeb = false;
            if (bankAccount.LastGenerationDate.Month == 2 && escrowDayOfMonth > 28)
            {
                lastDayOfFeb = true;
                escrowDayOfMonth = DateTime.DaysInMonth(bankAccount.LastGenerationDate.Year,
                    bankAccount.LastGenerationDate.Month);
            }

            var startDate = new DateTime(bankAccount.LastGenerationDate.Year, bankAccount.LastGenerationDate.Month,
                escrowDayOfMonth);

            while (startDate < generateToDate)
            {
                decimal escrowToBankAmount = 0;
                decimal escrowFromBankAmount = 0;
                foreach (var escrowItem in escrowItems)
                {
                    if (escrowItem.EndingDate != null && escrowItem.EndingDate.Value > startDate ||
                        escrowItem.EndingDate == null)
                    {
                        escrowToBankAmount += escrowItem.MonthlyAmount;
                        escrowItem.EscrowBalance += escrowItem.MonthlyAmount;//This is only temporary
                        
                        if (escrowItem.StartingDate != null)
                        {
                            if (escrowItem.StartingDate.Value.AddMonths(1) < startDate
                                && escrowItem.EscrowBalance != null)
                            {
                                escrowFromBankAmount += escrowItem.EscrowBalance.Value;
                                escrowItem.EscrowBalance = 0;
                                AdvanceBudgetItemStartingDate(escrowItem);
                            }
                        }
                    }
                }

                AddEscrowRegisterItems(bankAccount, startDate, escrowToBankAmount, "Monthly Escrow", result);

                if (escrowFromBankAmount > 0)
                {
                    AddEscrowRegisterItems(bankAccount, startDate, -escrowFromBankAmount, "Transfer From Escrow",
                        result);
                }
                startDate = startDate.AddMonths(1);
                if (lastDayOfFeb)
                {
                    if (bankAccount.EscrowDayOfMonth != null)
                        startDate = new DateTime(startDate.Year, 3, bankAccount.EscrowDayOfMonth.Value);
                    lastDayOfFeb = false;
                }
            }

            return result;
        }

        private static void AddEscrowRegisterItems(BankAccount bankAccount, DateTime date, decimal escrowAmount,
            string registerItemDescription, List<BankAccountRegisterItem> registerItems)
        {
            var registerItem = new BankAccountRegisterItem
            {
                RegisterId = Guid.NewGuid().ToString(),
                BankAccountId = bankAccount.Id,
                ItemType = BankAccountRegisterItemTypes.MonthlyEscrow,
                ItemDate = date,
                Description = registerItemDescription,
                ProjectedAmount = -escrowAmount
            };
            registerItems.Add(registerItem);

            if (bankAccount.EscrowToBankAccountId == null) 
                return;

            var escrowToBankRegisterId = Guid.NewGuid().ToString();
            var escrowFromBankRegisterId = registerItem.RegisterId;

            registerItem.TransferRegisterId = escrowToBankRegisterId;

            registerItem = new BankAccountRegisterItem
            {
                RegisterId = escrowToBankRegisterId,
                BankAccountId = bankAccount.EscrowToBankAccountId.Value,
                ItemType = BankAccountRegisterItemTypes.MonthlyEscrow,
                ItemDate = date,
                Description = registerItemDescription,
                ProjectedAmount = escrowAmount,
                TransferRegisterId = escrowFromBankRegisterId
            };
            registerItems.Add(registerItem);
        }
    }
}
