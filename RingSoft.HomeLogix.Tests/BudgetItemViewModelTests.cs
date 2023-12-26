using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.DevLogix.Tests;
using RingSoft.DevLogix.Tests.QualityAssurance;
using RingSoft.HomeLogix.DataAccess;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Tests
{
    [TestClass]
    public class BudgetItemViewModelTests
    {
        public const int JaneSavingsBankAccountId = 1;
        public const int JuniorSavingsBankAccountId = 2;
        public const int JointCheckingBankAccountId = 3;
        public const int JaneCheckingBankAccountId = 4;
        public const int JuniorCheckingBankAccountId = 5;
        public const int SallyCheckingBankAccountId = 6;

        public const int JohnIncomeBudgetItemId = 1;
        public const int JaneIncomeBudgetItemId = 2;
        public const int HousePaymentBudgetItemId = 3;
        public const int GroceriesBudgetItemId = 4;
        public const int TransferBudgetItemId = 5;
        public const int JuniorIncomeBudgetItemId = 7;
        public const int JuniorSavingsDepositBudgetItemId = 9;
        public const int SallyAllowanceBudgetItemId = 10;

        public static TestGlobals<BudgetItemViewModel, BudgetView> Globals { get; } =
            new TestGlobals<BudgetItemViewModel, BudgetView>();

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            Globals.Initialize();
        }

        [TestMethod]
        public void TestBudgetItemTransfer_Swap_TransferFrom_TransferTo()
        {
            Globals.ClearData();
            var dataRepository = AppGlobals.DataRepository;
            CreateAndTestBankAccounts();

            CreateAndTestBudgetItems();

            var jointBankAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var oldJointMonthlyDeposits = jointBankAccount.MonthlyBudgetDeposits;
            var oldJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            var janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var oldJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;
            var oldJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            var transferBudgetItem = dataRepository.GetBudgetItem(TransferBudgetItemId);
            var transferBudgetPk = AppGlobals
                .LookupContext
                .BudgetItems
                .GetPrimaryKeyValueFromEntity(transferBudgetItem);
            budgetItemViewModel.OnRecordSelected(transferBudgetPk);
            var oldMonthlyAmount = budgetItemViewModel.MonthlyAmount;

            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(janeBankAccount),
                janeBankAccount.Description);

            budgetItemViewModel.TransferToBankAccountAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(jointBankAccount),
                jointBankAccount.Description);

            budgetItemViewModel.Amount = 175;

            //budgetItemViewModel.UnitTestGetEntityData();
            //Assert.IsNull(budgetItemViewModel.DbBankAccount,
            //    $"{nameof(TestBudgetItemTransfer_Swap_TransferFrom_TransferTo)} {nameof(budgetItemViewModel.DbBankAccount)} is null");

            //Assert.IsNull(budgetItemViewModel.DbTransferToBankAccount,
            //    $"{nameof(TestBudgetItemTransfer_Swap_TransferFrom_TransferTo)} {nameof(budgetItemViewModel.DbTransferToBankAccount)} is null");

            budgetItemViewModel.SaveCommand.Execute(null);
            var newMonthlyAmount = budgetItemViewModel.MonthlyAmount;

            jointBankAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var newJointMonthlyDeposits = jointBankAccount.MonthlyBudgetDeposits;
            var newJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var newJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;
            var newJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            Assert.AreEqual(oldJaneMonthlyWithdrawals + newMonthlyAmount, newJaneMonthlyWithdrawals,
                nameof(TestBudgetItemTransfer_Swap_TransferFrom_TransferTo));

            Assert.AreEqual(oldJaneMonthlyDeposits - oldMonthlyAmount, newJaneMonthlyDeposits,
                nameof(TestBudgetItemTransfer_Swap_TransferFrom_TransferTo));


            Assert.AreEqual(oldJointMonthlyDeposits + newMonthlyAmount, newJointMonthlyDeposits,
                nameof(TestBudgetItemTransfer_Swap_TransferFrom_TransferTo));

            Assert.AreEqual(oldJointMonthlyWithdrawals - oldMonthlyAmount, newJointMonthlyWithdrawals,
                nameof(TestBudgetItemTransfer_Swap_TransferFrom_TransferTo));
        }

        [TestMethod]
        public void TestBudgetItemTransfer_ChangeTransferFrom_AndTransferTo()
        {
            Globals.ClearData();
            var dataRepository = AppGlobals.DataRepository;

            CreateAndTestBankAccounts();

            CreateAndTestBudgetItems();

            var jointBankAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var oldJointMonthlyDeposits = jointBankAccount.MonthlyBudgetDeposits;
            var oldJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            var janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var oldJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;
            var oldJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            var juniorBankAccount = dataRepository.GetBankAccount(JuniorCheckingBankAccountId);
            var oldJuniorMonthlyDeposits = juniorBankAccount.MonthlyBudgetDeposits;
            var oldJuniorMonthlyWithdrawals = juniorBankAccount.MonthlyBudgetWithdrawals;

            var savingsBankAccount = dataRepository.GetBankAccount(JaneSavingsBankAccountId);
            var oldSavingsMonthlyDeposits = savingsBankAccount.MonthlyBudgetDeposits;
            var oldSavingsMonthlyWithdrawals = savingsBankAccount.MonthlyBudgetWithdrawals;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            var transferBudgetItem = dataRepository.GetBudgetItem(TransferBudgetItemId);

            var transferBudgetPk = AppGlobals
                .LookupContext
                .BudgetItems
                .GetPrimaryKeyValueFromEntity(transferBudgetItem);
            budgetItemViewModel.OnRecordSelected(transferBudgetPk);
            //budgetItemViewModel.UnitTestLoadFromEntity(transferBudgetItem);
            var oldMonthlyAmount = budgetItemViewModel.MonthlyAmount;

            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(juniorBankAccount),
                janeBankAccount.Description);

            budgetItemViewModel.TransferToBankAccountAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(savingsBankAccount),
                jointBankAccount.Description);

            budgetItemViewModel.Amount = 175;

            budgetItemViewModel.SaveCommand.Execute(null);
            var newMonthlyAmount = budgetItemViewModel.MonthlyAmount;

            jointBankAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var newJointMonthlyDeposits = jointBankAccount.MonthlyBudgetDeposits;
            var newJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var newJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;
            var newJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            juniorBankAccount = dataRepository.GetBankAccount(JuniorCheckingBankAccountId);
            var newJuniorMonthlyDeposits = juniorBankAccount.MonthlyBudgetDeposits;
            var newJuniorMonthlyWithdrawals = juniorBankAccount.MonthlyBudgetWithdrawals;

            savingsBankAccount = dataRepository.GetBankAccount(JaneSavingsBankAccountId);
            var newSavingsMonthlyDeposits = savingsBankAccount.MonthlyBudgetDeposits;
            var newSavingsMonthlyWithdrawals = savingsBankAccount.MonthlyBudgetWithdrawals;

            Assert.AreEqual(oldJointMonthlyDeposits, newJointMonthlyDeposits,
                nameof(TestBudgetItemTransfer_ChangeTransferFrom_AndTransferTo));

            Assert.AreEqual(oldJointMonthlyWithdrawals - oldMonthlyAmount, newJointMonthlyWithdrawals,
                nameof(TestBudgetItemTransfer_ChangeTransferFrom_AndTransferTo));

            Assert.AreEqual(oldJaneMonthlyWithdrawals, newJaneMonthlyWithdrawals,
                nameof(TestBudgetItemTransfer_ChangeTransferFrom_AndTransferTo));

            Assert.AreEqual(oldJaneMonthlyDeposits - oldMonthlyAmount, newJaneMonthlyDeposits,
                nameof(TestBudgetItemTransfer_ChangeTransferFrom_AndTransferTo));


            Assert.AreEqual(oldJuniorMonthlyDeposits, newJuniorMonthlyDeposits,
                nameof(TestBudgetItemTransfer_ChangeTransferFrom_AndTransferTo));

            Assert.AreEqual(oldJuniorMonthlyWithdrawals + newMonthlyAmount, newJuniorMonthlyWithdrawals,
                nameof(TestBudgetItemTransfer_ChangeTransferFrom_AndTransferTo));

            Assert.AreEqual(oldSavingsMonthlyWithdrawals, newSavingsMonthlyWithdrawals,
                nameof(TestBudgetItemTransfer_ChangeTransferFrom_AndTransferTo));

            Assert.AreEqual(oldSavingsMonthlyDeposits + newMonthlyAmount, newSavingsMonthlyDeposits,
                nameof(TestBudgetItemTransfer_ChangeTransferFrom_AndTransferTo));
        }

        [TestMethod]
        public void TestBudgetItemTransfer_ChangeTransferFrom_KeepTransferTo()
        {
            Globals.ClearData();
            var dataRepository = AppGlobals.DataRepository;

            CreateAndTestBankAccounts();

            var jointBankAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var oldJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            var janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var oldJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            budgetItemViewModel.Id = TransferBudgetItemId;
            budgetItemViewModel.KeyAutoFillValue = new AutoFillValue(null, "Transfer Error");

            var bankAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);

            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            budgetItemViewModel.BudgetItemType = BudgetItemTypes.Transfer;
            budgetItemViewModel.Amount = 100;
            budgetItemViewModel.RecurringPeriod = 1;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Months;
            budgetItemViewModel.StartingDate = DateTime.Parse("02/03/2021");

            bankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);

            budgetItemViewModel.TransferToBankAccountAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);


            budgetItemViewModel.SaveCommand.Execute(null);
            var newMonthlyAmount = budgetItemViewModel.MonthlyAmount;

            jointBankAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var newJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var newJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;

            Assert.AreEqual(oldJointMonthlyWithdrawals + newMonthlyAmount, newJointMonthlyWithdrawals,
                nameof(TestBudgetItemTransfer_ChangeTransferFrom_KeepTransferTo));

            Assert.AreEqual(oldJaneMonthlyDeposits + newMonthlyAmount, newJaneMonthlyDeposits,
                nameof(TestBudgetItemTransfer_ChangeTransferFrom_KeepTransferTo));
        }

        [TestMethod]
        public void TestBudgetItemIncome_Change()
        {
            Globals.ClearData();
            var dataRepository = AppGlobals.DataRepository;

            CreateAndTestBankAccounts();

            CreateAndTestBudgetItems();

            var jointBankAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var oldJointMonthlyDeposits = jointBankAccount.MonthlyBudgetDeposits;
            var oldJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            var janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var oldJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;
            var oldJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(new BudgetView());

            var janeIncomeBudgetItem = dataRepository.GetBudgetItem(JaneIncomeBudgetItemId);

            //budgetItemViewModel.UnitTestLoadFromEntity(janeIncomeBudgetItem);
            var janeBudgetPk = AppGlobals
                .LookupContext
                .BudgetItems
                .GetPrimaryKeyValueFromEntity(janeIncomeBudgetItem);
            budgetItemViewModel.OnRecordSelected(janeBudgetPk);

            var oldMonthlyAmount = budgetItemViewModel.MonthlyAmount;

            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(jointBankAccount),
                janeBankAccount.Description);

            budgetItemViewModel.Amount = 600;
            budgetItemViewModel.SaveCommand.Execute(null);
            var newMonthlyAmount = budgetItemViewModel.MonthlyAmount;

            jointBankAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var newJointMonthlyDeposits = jointBankAccount.MonthlyBudgetDeposits;
            var newJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var newJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;
            var newJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            Assert.AreEqual(oldJointMonthlyDeposits + newMonthlyAmount, newJointMonthlyDeposits,
                nameof(TestBudgetItemIncome_Change));

            Assert.AreEqual(oldJointMonthlyWithdrawals, newJointMonthlyWithdrawals,
                nameof(TestBudgetItemIncome_Change));

            Assert.AreEqual(oldJaneMonthlyDeposits - oldMonthlyAmount, newJaneMonthlyDeposits,
                nameof(TestBudgetItemIncome_Change));

            Assert.AreEqual(oldJaneMonthlyWithdrawals, newJaneMonthlyWithdrawals,
                nameof(TestBudgetItemIncome_Change));
        }

        [TestMethod]
        public void TestDeleteBudgetItems()
        {
            Globals.ClearData();
            var dataRepository = AppGlobals.DataRepository;

            CreateAndTestBankAccounts();

            CreateAndTestBudgetItems();

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            var janeCheckingAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var oldJaneDeposits = janeCheckingAccount.MonthlyBudgetDeposits;
            var oldJaneWithdrawals = janeCheckingAccount.MonthlyBudgetWithdrawals;

            var budgetItem = dataRepository.GetBudgetItem(JaneIncomeBudgetItemId);

            var budgetPk = AppGlobals
                .LookupContext
                .BudgetItems
                .GetPrimaryKeyValueFromEntity(budgetItem);
            budgetItemViewModel.OnRecordSelected(budgetPk);

            //budgetItemViewModel.UnitTestLoadFromEntity(budgetItem);

            Globals.MessageBoxResult = MessageBoxButtonsResult.Yes;
            budgetItemViewModel.DeleteCommand.Execute(null);

            janeCheckingAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var newJaneDeposits = janeCheckingAccount.MonthlyBudgetDeposits;

            Assert.AreEqual(oldJaneDeposits - budgetItem.MonthlyAmount, newJaneDeposits,
                "Delete Income, Update Deposits");

            budgetItem = dataRepository.GetBudgetItem(GroceriesBudgetItemId);

            budgetPk = AppGlobals
                .LookupContext
                .BudgetItems
                .GetPrimaryKeyValueFromEntity(budgetItem);
            budgetItemViewModel.OnRecordSelected(budgetPk);

            //budgetItemViewModel.UnitTestLoadFromEntity(budgetItem);

            //Assert.AreEqual(DbMaintenanceResults.Success
            //    , budgetItemViewModel.DoDelete(true), "Delete Expense");
            budgetItemViewModel.DeleteCommand.Execute(null);

            janeCheckingAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var newJaneWithdrawals = janeCheckingAccount.MonthlyBudgetWithdrawals;

            Assert.AreEqual(oldJaneWithdrawals - budgetItem.MonthlyAmount, newJaneWithdrawals,
                "Delete Expense, Update Withdrawals");

            janeCheckingAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            oldJaneDeposits = janeCheckingAccount.MonthlyBudgetDeposits;

            var jointCheckingAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var oldJointWithdrawals = jointCheckingAccount.MonthlyBudgetWithdrawals;

            budgetItem = dataRepository.GetBudgetItem(TransferBudgetItemId);

            budgetPk = AppGlobals
                .LookupContext
                .BudgetItems
                .GetPrimaryKeyValueFromEntity(budgetItem);
            budgetItemViewModel.OnRecordSelected(budgetPk);

            //budgetItemViewModel.UnitTestLoadFromEntity(budgetItem);

            //Assert.AreEqual(DbMaintenanceResults.Success
            //    , budgetItemViewModel.DoDelete(true)
            //    , "Delete Transfer");
            budgetItemViewModel.DeleteCommand.Execute(null);

            janeCheckingAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            jointCheckingAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);

            newJaneDeposits = janeCheckingAccount.MonthlyBudgetDeposits;
            Assert.AreEqual(oldJaneDeposits - budgetItem.MonthlyAmount, newJaneDeposits,
                "Delete Transfer, Update Deposits");

            var newJointWithdrawals = jointCheckingAccount.MonthlyBudgetWithdrawals;
            Assert.AreEqual(oldJointWithdrawals - budgetItem.MonthlyAmount, newJointWithdrawals,
                "Delete Transfer, Update Withdrawals");
        }

        public static void CreateAndTestBudgetItems()
        {
            var budgetItemViewModel = new BudgetItemViewModel();
            //budgetItemViewModel.Processor = new TestDbMaintenanceProcessor();
            budgetItemViewModel.Processor = Globals;
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

        public static void CreateAndTestBankAccounts()
        {
            var bankAccountViewModel = new BankAccountViewModel();
            //bankAccountViewModel.Processor = new TestDbMaintenanceProcessor();
            bankAccountViewModel.Processor = Globals;
            bankAccountViewModel.OnViewLoaded(new TestBankAccountView(nameof(CreateAndTestBankAccounts)));

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
    }
}
