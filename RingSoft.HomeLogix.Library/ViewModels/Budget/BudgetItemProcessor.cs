using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Org.BouncyCastle.Math.EC;
using RingSoft.App.Library;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BankAccountRegisterEscrowGenerateOutput
    {
        public List<BankAccountRegisterItem> RegisterItems { get; set; }

        public List<BankAccountRegisterItemEscrow> RegisterItemEscrows { get; set; }
    }

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
                        processorData.MonthToDatePercent =
                            Math.Round(
                                processorData.BudgetItem.CurrentMonthAmount / processorData.BudgetItem.MonthlyAmount,
                                4);

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

                var monthsAccrued = months - Math.Ceiling(monthsToGo);  //Must be ceiling or else escrow item generation will generate an extra escrow.
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

        public static BankAccountRegisterEscrowGenerateOutput GenerateEscrowRegisterItems(BankAccount bankAccount,
            DateTime generateToDate)
        {
            var result = new BankAccountRegisterEscrowGenerateOutput
            {
                RegisterItems = new List<BankAccountRegisterItem>(),
                RegisterItemEscrows = new List<BankAccountRegisterItemEscrow>()
            };

            var escrowItems = AppGlobals.DataRepository.GetEscrowBudgetItems(bankAccount.Id).ToList();
            if (!escrowItems.Any())
                return result;

            var lastGenerationDate = bankAccount.LastGenerationDate;
            var escrowDayOfMonth = bankAccount.EscrowDayOfMonth ?? 1;
            var lastDayOfFeb = false;
            if (lastGenerationDate.Month == 2 && escrowDayOfMonth > 28)
            {
                lastDayOfFeb = true;
                escrowDayOfMonth = DateTime.DaysInMonth(lastGenerationDate.Year, lastGenerationDate.Month);
            }

            var startDate = new DateTime(lastGenerationDate.Year, lastGenerationDate.Month, escrowDayOfMonth);

            while (startDate < generateToDate)
            {
                decimal escrowToBankAmount = 0;
                decimal escrowFromBankAmount = 0;
                var escrowToItems = new List<BankAccountRegisterItemEscrow>();
                var escrowFromItems = new List<BankAccountRegisterItemEscrow>();
                foreach (var escrowItem in escrowItems)
                {
                    if (escrowItem.EndingDate != null && escrowItem.EndingDate.Value > startDate ||
                        escrowItem.EndingDate == null)
                    {
                        escrowToBankAmount += escrowItem.MonthlyAmount;
                        escrowToItems.Add(new BankAccountRegisterItemEscrow
                        {
                            BudgetItemId = escrowItem.Id,
                            Amount = escrowItem.MonthlyAmount
                        });
                        var incrementEscrow = true;
                        if (escrowItem.StartingDate != null)
                        {
                            if (escrowItem.StartingDate.Value < startDate.AddMonths(1)
                                && escrowItem.EscrowBalance != null)
                            {
                                escrowFromBankAmount += escrowItem.EscrowBalance.Value + escrowItem.MonthlyAmount;
                                escrowFromItems.Add(new BankAccountRegisterItemEscrow
                                {
                                    BudgetItemId = escrowItem.Id,
                                    Amount = escrowItem.EscrowBalance.Value + escrowItem.MonthlyAmount
                                });
                                escrowItem.EscrowBalance = 0;
                                AdvanceBudgetItemStartingDate(escrowItem);
                                incrementEscrow = false;
                            }
                        }
                        if (incrementEscrow)
                            escrowItem.EscrowBalance += escrowItem.MonthlyAmount;//This is only temporary
                    }
                }

                AddEscrowRegisterItems(bankAccount, startDate, escrowToBankAmount, "Monthly Escrow", result,
                    escrowToItems, false);

                if (escrowFromBankAmount > 0)
                {
                    AddEscrowRegisterItems(bankAccount, startDate, -escrowFromBankAmount, "Transfer From Escrow",
                        result, escrowFromItems, true);
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
            string registerItemDescription, BankAccountRegisterEscrowGenerateOutput output,
            List<BankAccountRegisterItemEscrow> registerItemEscrows, bool isEscrowFrom)
        {
            var registerItem = new BankAccountRegisterItem
            {
                RegisterGuid = Guid.NewGuid().ToString(),
                BankAccountId = bankAccount.Id,
                ItemType = (int)BankAccountRegisterItemTypes.MonthlyEscrow,
                ItemDate = date,
                Description = registerItemDescription,
                ProjectedAmount = -escrowAmount,IsEscrowFrom = isEscrowFrom
            };
            AddEscrowsToRegisterItem(output, registerItemEscrows, registerItem);

            if (bankAccount.EscrowToBankAccountId == null) 
                return;

            var escrowToBankRegisterId = Guid.NewGuid().ToString();
            var escrowFromBankRegisterId = registerItem.RegisterGuid;

            registerItem.TransferRegisterGuid = escrowToBankRegisterId;

            registerItem = new BankAccountRegisterItem
            {
                RegisterGuid = escrowToBankRegisterId,
                BankAccountId = bankAccount.EscrowToBankAccountId.Value,
                ItemType = (int)BankAccountRegisterItemTypes.MonthlyEscrow,
                ItemDate = date,
                Description = registerItemDescription,
                ProjectedAmount = escrowAmount,
                TransferRegisterGuid = escrowFromBankRegisterId,
                IsEscrowFrom = isEscrowFrom
            };
            AddEscrowsToRegisterItem(output, registerItemEscrows, registerItem);
        }

        private static void AddEscrowsToRegisterItem(BankAccountRegisterEscrowGenerateOutput output, List<BankAccountRegisterItemEscrow> registerItemEscrows,
            BankAccountRegisterItem registerItem)
        {
            output.RegisterItems.Add(registerItem);
            foreach (var bankAccountRegisterItemEscrow in registerItemEscrows)
            {
                output.RegisterItemEscrows.Add(new BankAccountRegisterItemEscrow
                {
                    BudgetItemId = bankAccountRegisterItemEscrow.BudgetItemId,
                    RegisterItem = registerItem,
                    Amount = bankAccountRegisterItemEscrow.Amount
                });
            }
        }
    }
}
