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

        public static BudgetItem GetBudgetItem(BudgetItemBankAccountData budgetItemBankAccountData)
        {
            BankAccount newBankAccount = null;
            BankAccount newTransferToBankAccount = null;
            if (budgetItemBankAccountData.BudgetItem.BankAccountId != 0)
            {
                newBankAccount = AppGlobals.DataRepository.GetBankAccount(budgetItemBankAccountData.BudgetItem.BankAccountId, false);
                if (newTransferToBankAccountId != null)
                {
                    newTransferToBankAccount = AppGlobals.DataRepository.GetBankAccount((int)newTransferToBankAccountId, false);
                }

                if (budgetItemBankAccountData.BudgetItem.BankAccountId == DbBankAccountId || DbBankAccountId == 0)
                {
                    switch (BudgetItemType)
                    {
                        case BudgetItemTypes.Income:
                            newBankAccount.MonthlyBudgetDeposits += MonthlyAmount - DbMonthlyAmount;
                            break;
                        case BudgetItemTypes.Expense:
                            newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount - DbMonthlyAmount;
                            break;
                        case BudgetItemTypes.Transfer:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    _dbBankAccount = AppGlobals.DataRepository.GetBankAccount(DbBankAccountId, false);
                    switch (BudgetItemType)
                    {
                        case BudgetItemTypes.Income:
                            _dbBankAccount.MonthlyBudgetDeposits -= DbMonthlyAmount;
                            newBankAccount.MonthlyBudgetDeposits += MonthlyAmount;
                            break;
                        case BudgetItemTypes.Expense:
                            _dbBankAccount.MonthlyBudgetWithdrawals -= DbMonthlyAmount;
                            newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount;
                            break;
                        case BudgetItemTypes.Transfer:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (BudgetItemType == BudgetItemTypes.Transfer)
                {
                    if (newTransferToBankAccount != null && newBankAccount != null)
                    {
                        //_dbBankAccount is Old Transfer From Bank Account.
                        _dbBankAccount = AppGlobals.DataRepository.GetBankAccount(DbBankAccountId, false);

                        if (DbTransferToBankId != null)
                            _dbTransferToBankAccount = AppGlobals.DataRepository.GetBankAccount((int)DbTransferToBankId, false);

                        if (budgetItemBankAccountData.BudgetItem.BankAccountId == DbBankAccountId || DbBankAccountId == 0)
                        {
                            //Same transfer from (new) bank account
                            newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount - DbMonthlyAmount;
                            if (_dbTransferToBankAccount == null)
                            {
                                //Same new transfer to bank account.
                                newTransferToBankAccount.MonthlyBudgetDeposits += MonthlyAmount - DbMonthlyAmount;
                            }
                            else
                            {
                                //Different transfer to bank account.
                                _dbTransferToBankAccount.MonthlyBudgetDeposits -= DbMonthlyAmount;
                                newTransferToBankAccount.MonthlyBudgetDeposits += MonthlyAmount;
                            }
                        }
                        else
                        {
                            //Different transfer from (new) bank account
                            newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount;
                            if (_dbTransferToBankAccount == null)
                            {
                                //Same new transfer to bank account.
                                newTransferToBankAccount.MonthlyBudgetDeposits += MonthlyAmount - DbMonthlyAmount;
                            }
                            else
                            {
                                var swap = false;
                                if (newTransferToBankAccount.Id != _dbTransferToBankAccount.Id)
                                {
                                    //Different transfer to bank account.
                                    newTransferToBankAccount.MonthlyBudgetDeposits += MonthlyAmount;
                                    if (newBankAccount.Id == _dbTransferToBankAccount.Id)
                                    {
                                        //Swap.
                                        swap = true;
                                        newTransferToBankAccount.MonthlyBudgetWithdrawals -= DbMonthlyAmount;
                                        newBankAccount.MonthlyBudgetDeposits -= DbMonthlyAmount;
                                    }
                                    else if (_dbTransferToBankAccount.Id != newTransferToBankAccount.Id)
                                    {
                                        _dbTransferToBankAccount.MonthlyBudgetDeposits -= DbMonthlyAmount;
                                    }
                                }
                                if (_dbBankAccount.Id != budgetItemBankAccountData.BudgetItem.BankAccountId && !swap)
                                {
                                    _dbBankAccount.MonthlyBudgetWithdrawals -= DbMonthlyAmount;
                                    newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount - DbMonthlyAmount;
                                }
                            }
                        }
                    }
                }
            }

            budgetItem.BankAccount = newBankAccount;

            budgetItem.TransferToBankAccount = newTransferToBankAccount;

            if (DbBankAccountId == newTransferToBankAccountId || DbBankAccountId == budgetItemBankAccountData.BudgetItem.BankAccountId)
            {
                _dbBankAccount = null;
            }

            if (DbTransferToBankId == budgetItemBankAccountData.BudgetItem.BankAccountId || DbTransferToBankId == newTransferToBankAccountId)
            {
                _dbTransferToBankAccount = null;
            }

            return budgetItemBankAccountData.BudgetItem;
        }
    }
}
