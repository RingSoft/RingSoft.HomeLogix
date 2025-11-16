using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Tests.QualityAssurance;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using RingSoft.HomeLogix.Library.ViewModels.Main;
using RingSoft.HomeLogix.Sqlite;
using RingSoft.HomeLogix.Tests;

namespace RingSoft.DevLogix.Tests
{
    public class TestGlobals<TViewModel, TView> : DbMaintenanceTestGlobals<TViewModel, TView>
        where TViewModel : DbMaintenanceViewModelBase
        where TView : IDbMaintenanceView, new()
    {
        public int JaneSavingsBankAccountId { get; } = 1;
        public int JuniorSavingsBankAccountId { get; } = 2;
        public int JointCheckingBankAccountId { get; } = 3;
        public int JaneCheckingBankAccountId { get; } = 4;
        public int JuniorCheckingBankAccountId { get; } = 5;
        public int SallyCheckingBankAccountId { get; } = 6;

        public int JohnIncomeBudgetItemId { get; } = 1;
        public int JaneIncomeBudgetItemId { get; } = 2;
        public int HousePaymentBudgetItemId { get; } = 3;
        public int GroceriesBudgetItemId { get; } = 4;
        public int TransferBudgetItemId { get; } = 5;
        public int JuniorIncomeBudgetItemId { get; } = 7;
        public int JuniorSavingsDepositBudgetItemId { get; } = 9;
        public int SallyAllowanceBudgetItemId { get; } = 10;

        public new HomeLogixTestDataRepository DataRepository { get; } 
            
        public TestGlobals() : base(new HomeLogixTestDataRepository(new TestDataRegistry()))
        {
            DataRepository = base.DataRepository as HomeLogixTestDataRepository;
        }

        public override void Initialize()
        {
            AppGlobals.UnitTesting = true;
            SystemGlobals.UnitTestMode = true;
            AppGlobals.Initialize();
            AppGlobals.LookupContext.Initialize(new SqliteHomeLogixDbContext(), DbPlatforms.Sqlite);
            AppGlobals.MainViewModel = new MainViewModel();

            base.Initialize();
        }

        public override void ClearData()
        {

            base.ClearData();
            LoadDatabase();
        }


        private void LoadDatabase()
        {
            CreateAndTestBankAccounts();

            CreateAndTestBudgetItems();
        }

        private void CreateAndTestBankAccounts()
        {
            BankAccountViewModel bankAccountViewModel = null;
            if (ViewModel is BankAccountViewModel bankAccountViewModel1)
            {
                bankAccountViewModel = bankAccountViewModel1;
            }
            else
            {
                bankAccountViewModel = new BankAccountViewModel();
            }
            //bankAccountViewModel.Processor = new TestDbMaintenanceProcessor();
            bankAccountViewModel.Processor = this;
            bankAccountViewModel.OnViewLoaded(new TestBankAccountView());

            bankAccountViewModel.Id = JaneSavingsBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Jane's Savings Account");

            bankAccountViewModel.SaveCommand.Execute(null);
            bankAccountViewModel.NewCommand.Execute(null);

            bankAccountViewModel.Id = JuniorSavingsBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Junior's Savings Account");

            bankAccountViewModel.SaveCommand.Execute(null);
            bankAccountViewModel.NewCommand.Execute(null);

            bankAccountViewModel.Id = JointCheckingBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Joint Checking Account");
            var savingsBank = AppGlobals.DataRepository.GetBankAccount(JaneSavingsBankAccountId, false);
            var savingsBankPrimaryKeyValue =
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(savingsBank);

            bankAccountViewModel.SaveCommand.Execute(null);
            bankAccountViewModel.NewCommand.Execute(null);

            bankAccountViewModel.Id = JaneCheckingBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Jane's Checking Account");
            savingsBank = AppGlobals.DataRepository.GetBankAccount(JaneSavingsBankAccountId, false);
            savingsBankPrimaryKeyValue =
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(savingsBank);

            var newDate = DateTime.Today;
            newDate =
                newDate.AddDays(DateTime.DaysInMonth(newDate.Year, newDate.Month) - newDate.Day);

            Assert.AreEqual(newDate, bankAccountViewModel.LastGenerationDate,
                $"Jane's Checking Account {nameof(bankAccountViewModel.LastGenerationDate)} set right.");

            bankAccountViewModel.SaveCommand.Execute(null);
            bankAccountViewModel.NewCommand.Execute(null);

            bankAccountViewModel.Id = JuniorCheckingBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Junior's Checking Account");
            savingsBank = AppGlobals.DataRepository.GetBankAccount(JuniorSavingsBankAccountId, false);
            savingsBankPrimaryKeyValue =
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(savingsBank);

            bankAccountViewModel.SaveCommand.Execute(null);
            bankAccountViewModel.NewCommand.Execute(null);

            bankAccountViewModel.Id = SallyCheckingBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Sally's Checking Account");

            bankAccountViewModel.SaveCommand.Execute(null);
        }

        private void CreateAndTestBudgetItems()
        {
            var budgetItemViewModel = new BudgetItemViewModel();
            //budgetItemViewModel.Processor = new TestDbMaintenanceProcessor();
            budgetItemViewModel.Processor = this;
            budgetItemViewModel.OnViewLoaded(new BudgetView());

            budgetItemViewModel.Id = JaneIncomeBudgetItemId;
            budgetItemViewModel.KeyAutoFillValue = new AutoFillValue(null, "Jane's Income");

            var bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            budgetItemViewModel.BudgetItemType = BudgetItemTypes.Income;
            budgetItemViewModel.Amount = 550;
            budgetItemViewModel.RecurringPeriod = 2;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Weeks;
            //budgetItemViewModel.StartingDate = DateTime.Parse("02/05/2021");

            //Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoSave(true),
            //    "Saving Jane's Income Budget Item");
            budgetItemViewModel.SaveCommand.Execute(null);

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            Assert.AreEqual(expected: (double)1178.57, bankAccount.MonthlyBudgetDeposits,
                "Jane's Checking Initial Monthly Deposits");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.NewCommand.Execute(null);
            budgetItemViewModel.Id = JohnIncomeBudgetItemId;
            budgetItemViewModel.KeyAutoFillValue = new AutoFillValue(null, "John's Social Security Disability");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JointCheckingBankAccountId);
            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            budgetItemViewModel.BudgetItemType = BudgetItemTypes.Income;
            budgetItemViewModel.Amount = 1530;
            budgetItemViewModel.RecurringPeriod = 1;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Months;
            //budgetItemViewModel.StartingDate = DateTime.Parse("02/03/2021");

            budgetItemViewModel.SaveCommand.Execute(null);

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JointCheckingBankAccountId);
            Assert.AreEqual(1530, bankAccount.MonthlyBudgetDeposits,
                "Joint Checking Initial Monthly Deposits");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.NewCommand.Execute(null);
            budgetItemViewModel.Id = HousePaymentBudgetItemId;
            budgetItemViewModel.KeyAutoFillValue = new AutoFillValue(null, "House Payment");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JointCheckingBankAccountId);
            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            budgetItemViewModel.BudgetItemType = BudgetItemTypes.Expense;
            budgetItemViewModel.Amount = 790;
            budgetItemViewModel.RecurringPeriod = 1;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Months;
            //budgetItemViewModel.StartingDate = DateTime.Parse("02/03/2021");

            budgetItemViewModel.SaveCommand.Execute(null);

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JointCheckingBankAccountId);
            Assert.AreEqual(790, bankAccount.MonthlyBudgetWithdrawals,
                "Joint Checking Initial Monthly Withdrawals");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.NewCommand.Execute(null);
            budgetItemViewModel.Id = GroceriesBudgetItemId;
            budgetItemViewModel.KeyAutoFillValue = new AutoFillValue(null, "Groceries");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            budgetItemViewModel.BudgetItemType = BudgetItemTypes.Expense;
            budgetItemViewModel.Amount = 300;
            budgetItemViewModel.RecurringPeriod = 1;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Weeks;
            //budgetItemViewModel.StartingDate = DateTime.Parse("01/29/2021");

            budgetItemViewModel.SaveCommand.Execute(null);

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            Assert.AreEqual((double)1285.71, bankAccount.MonthlyBudgetWithdrawals,
                "Jane's Checking Initial Monthly Withdrawals");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.NewCommand.Execute(null);
            budgetItemViewModel.Id = TransferBudgetItemId;
            budgetItemViewModel.KeyAutoFillValue = new AutoFillValue(null, "Transfer Error");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JointCheckingBankAccountId);
            var monthlyWithdrawals = bankAccount.MonthlyBudgetWithdrawals;

            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            budgetItemViewModel.BudgetItemType = BudgetItemTypes.Transfer;
            budgetItemViewModel.Amount = 100;
            budgetItemViewModel.RecurringPeriod = 1;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Months;
            //budgetItemViewModel.StartingDate = DateTime.Parse("02/03/2021");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var monthlyDeposits = bankAccount.MonthlyBudgetDeposits;

            budgetItemViewModel.TransferToBankAccountAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            budgetItemViewModel.SaveCommand.Execute(null);

            var budgetTest = AppGlobals.DataRepository.GetBudgetItem(TransferBudgetItemId);

            var type = (BudgetItemTypes)budgetTest.Type;

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JointCheckingBankAccountId);
            Assert.AreEqual(monthlyWithdrawals + 100, bankAccount.MonthlyBudgetWithdrawals,
                "Joint Checking Monthly Withdrawals After Initial Transfer");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            Assert.AreEqual(monthlyDeposits + 100, bankAccount.MonthlyBudgetDeposits,
                "Jane's Checking Monthly Deposits After Initial Transfer");


            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.NewCommand.Execute(null);
            budgetItemViewModel.Id = JuniorIncomeBudgetItemId;
            budgetItemViewModel.KeyAutoFillValue = new AutoFillValue(null, "Junior's Social Security");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JuniorCheckingBankAccountId);
            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            budgetItemViewModel.BudgetItemType = BudgetItemTypes.Income;
            budgetItemViewModel.Amount = 750;
            budgetItemViewModel.RecurringPeriod = 1;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Months;
            budgetItemViewModel.StartingDate = DateTime.Parse("02/03/2021");

            Assert.AreEqual(750, budgetItemViewModel.MonthlyAmount, "Junior's Initial Monthly Amount");

            budgetItemViewModel.SaveCommand.Execute(null);

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JuniorCheckingBankAccountId);
            Assert.AreEqual(750, bankAccount.MonthlyBudgetDeposits,
                "Junior's Initial Income");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.NewCommand.Execute(null);
            budgetItemViewModel.Id = JuniorSavingsDepositBudgetItemId;
            budgetItemViewModel.KeyAutoFillValue = new AutoFillValue(null, "Junior's Savings");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JuniorSavingsBankAccountId);
            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            budgetItemViewModel.BudgetItemType = BudgetItemTypes.Income;
            budgetItemViewModel.Amount = 100;
            budgetItemViewModel.RecurringPeriod = 1;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Months;
            budgetItemViewModel.StartingDate = DateTime.Parse("02/03/2021");

            Assert.AreEqual(100, budgetItemViewModel.MonthlyAmount, "Junior's Initial Savings");

            budgetItemViewModel.SaveCommand.Execute(null);

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JuniorSavingsBankAccountId);
            Assert.AreEqual(100, bankAccount.MonthlyBudgetDeposits,
                "Junior's Initial Savings Deposit");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.NewCommand.Execute(null);
            budgetItemViewModel.Id = SallyAllowanceBudgetItemId;
            budgetItemViewModel.KeyAutoFillValue = new AutoFillValue(null, "Sally's Monthly Allowance");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(SallyCheckingBankAccountId);
            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            budgetItemViewModel.BudgetItemType = BudgetItemTypes.Income;
            budgetItemViewModel.Amount = 100;
            budgetItemViewModel.RecurringPeriod = 1;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Months;
            budgetItemViewModel.StartingDate = DateTime.Parse("02/03/2021");

            Assert.AreEqual(100, budgetItemViewModel.MonthlyAmount, "Sally's Monthly Allowance");

            budgetItemViewModel.SaveCommand.Execute(null);

            bankAccount = AppGlobals.DataRepository.GetBankAccount(SallyCheckingBankAccountId);
            Assert.AreEqual(100, bankAccount.MonthlyBudgetDeposits,
                "Sally's Allowance Deposit");

        }
    }
}
