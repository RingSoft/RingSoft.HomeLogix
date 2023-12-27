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
            //CreateAndTestBankAccounts();

            //CreateAndTestBudgetItems();

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

            //CreateAndTestBankAccounts();

            //CreateAndTestBudgetItems();

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

            //CreateAndTestBankAccounts();

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

            //CreateAndTestBankAccounts();

            //CreateAndTestBudgetItems();

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

            //CreateAndTestBankAccounts();

            //CreateAndTestBudgetItems();

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

    }
}
