using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
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

            //No longer relevant with new bank month deposit/withdrawal calculations will differ from moth to month.
            return;

            var dataRepository = AppGlobals.DataRepository;
            //CreateAndTestBankAccounts();

            //CreateAndTestBudgetItems();

            var jointBankAccount = dataRepository.GetBankAccount(Globals.JointCheckingBankAccountId);
            var oldJointMonthlyDeposits = jointBankAccount.MonthlyBudgetDeposits;
            var oldJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            var janeBankAccount = dataRepository.GetBankAccount(Globals.JaneCheckingBankAccountId);
            var oldJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;
            var oldJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            //var budgetItemViewModel = new BudgetItemViewModel();
            //budgetItemViewModel.Processor = Globals;
            //budgetItemViewModel.OnViewLoaded(
            //    new BudgetView());

            var transferBudgetItem = dataRepository.GetBudgetItem(Globals.TransferBudgetItemId);
            var transferBudgetPk = AppGlobals
                .LookupContext
                .BudgetItems
                .GetPrimaryKeyValueFromEntity(transferBudgetItem);
            Globals.ViewModel.OnRecordSelected(transferBudgetPk);
            var oldMonthlyAmount = Globals.ViewModel.MonthlyAmount;

            Globals.ViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(janeBankAccount),
                janeBankAccount.Description);

            Globals.ViewModel.TransferToBankAccountAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(jointBankAccount),
                jointBankAccount.Description);

            Globals.ViewModel.Amount = 175;

            //budgetItemViewModel.UnitTestGetEntityData();
            //Assert.IsNull(budgetItemViewModel.DbBankAccount,
            //    $"{nameof(TestBudgetItemTransfer_Swap_TransferFrom_TransferTo)} {nameof(budgetItemViewModel.DbBankAccount)} is null");

            //Assert.IsNull(budgetItemViewModel.DbTransferToBankAccount,
            //    $"{nameof(TestBudgetItemTransfer_Swap_TransferFrom_TransferTo)} {nameof(budgetItemViewModel.DbTransferToBankAccount)} is null");

            Globals.ViewModel.SaveCommand.Execute(null);
            var newMonthlyAmount = Globals.ViewModel.MonthlyAmount;

            jointBankAccount = dataRepository.GetBankAccount(Globals.JointCheckingBankAccountId);
            var newJointMonthlyDeposits = jointBankAccount.MonthlyBudgetDeposits;
            var newJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            janeBankAccount = dataRepository.GetBankAccount(Globals.JaneCheckingBankAccountId);
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

            //No longer relevant with new bank month deposit/withdrawal calculations will differ from moth to month.
            return;

            var dataRepository = AppGlobals.DataRepository;
            
            //CreateAndTestBankAccounts();

            //CreateAndTestBudgetItems();

            var jointBankAccount = dataRepository.GetBankAccount(Globals.JointCheckingBankAccountId);
            var oldJointMonthlyDeposits = jointBankAccount.MonthlyBudgetDeposits;
            var oldJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            var janeBankAccount = dataRepository.GetBankAccount(Globals.JaneCheckingBankAccountId);
            var oldJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;
            var oldJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            var juniorBankAccount = dataRepository.GetBankAccount(Globals.JuniorCheckingBankAccountId);
            var oldJuniorMonthlyDeposits = juniorBankAccount.MonthlyBudgetDeposits;
            var oldJuniorMonthlyWithdrawals = juniorBankAccount.MonthlyBudgetWithdrawals;

            var savingsBankAccount = dataRepository.GetBankAccount(Globals.JaneSavingsBankAccountId);
            var oldSavingsMonthlyDeposits = savingsBankAccount.MonthlyBudgetDeposits;
            var oldSavingsMonthlyWithdrawals = savingsBankAccount.MonthlyBudgetWithdrawals;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            var transferBudgetItem = dataRepository.GetBudgetItem(Globals.TransferBudgetItemId);

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

            jointBankAccount = dataRepository.GetBankAccount(Globals.JointCheckingBankAccountId);
            var newJointMonthlyDeposits = jointBankAccount.MonthlyBudgetDeposits;
            var newJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            janeBankAccount = dataRepository.GetBankAccount(Globals.JaneCheckingBankAccountId);
            var newJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;
            var newJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            juniorBankAccount = dataRepository.GetBankAccount(Globals.JuniorCheckingBankAccountId);
            var newJuniorMonthlyDeposits = juniorBankAccount.MonthlyBudgetDeposits;
            var newJuniorMonthlyWithdrawals = juniorBankAccount.MonthlyBudgetWithdrawals;

            savingsBankAccount = dataRepository.GetBankAccount(Globals.JaneSavingsBankAccountId);
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

            //No longer relevant with new bank month deposit/withdrawal calculations will differ from moth to month.
            return;

            var dataRepository = AppGlobals.DataRepository;

            //CreateAndTestBankAccounts();

            var jointBankAccount = dataRepository.GetBankAccount(Globals.JointCheckingBankAccountId);
            var oldJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            var janeBankAccount = dataRepository.GetBankAccount(Globals.JaneCheckingBankAccountId);
            var oldJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            budgetItemViewModel.Id = Globals.TransferBudgetItemId;
            budgetItemViewModel.KeyAutoFillValue = new AutoFillValue(null, "Transfer Error");

            var bankAccount = dataRepository.GetBankAccount(Globals.JointCheckingBankAccountId);

            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            budgetItemViewModel.BudgetItemType = BudgetItemTypes.Transfer;
            budgetItemViewModel.Amount = 100;
            budgetItemViewModel.RecurringPeriod = 1;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Months;
            budgetItemViewModel.StartingDate = DateTime.Parse("02/03/2021");

            bankAccount = dataRepository.GetBankAccount(Globals.JaneCheckingBankAccountId);

            budgetItemViewModel.TransferToBankAccountAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);


            budgetItemViewModel.SaveCommand.Execute(null);
            var newMonthlyAmount = budgetItemViewModel.MonthlyAmount;

            jointBankAccount = dataRepository.GetBankAccount(Globals.JointCheckingBankAccountId);
            var newJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            janeBankAccount = dataRepository.GetBankAccount(Globals.JaneCheckingBankAccountId);
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

            //No longer relevant with new bank month deposit/withdrawal calculations will differ from moth to month.
            return;
            var dataRepository = AppGlobals.DataRepository;

            //CreateAndTestBankAccounts();

            //CreateAndTestBudgetItems();

            var jointBankAccount = dataRepository.GetBankAccount(Globals.JointCheckingBankAccountId);
            var oldJointMonthlyDeposits = jointBankAccount.MonthlyBudgetDeposits;
            var oldJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            var janeBankAccount = dataRepository.GetBankAccount(Globals.JaneCheckingBankAccountId);
            var oldJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;
            var oldJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(new BudgetView());

            var janeIncomeBudgetItem = dataRepository.GetBudgetItem(Globals.JaneIncomeBudgetItemId);

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

            jointBankAccount = dataRepository.GetBankAccount(Globals.JointCheckingBankAccountId);
            var newJointMonthlyDeposits = jointBankAccount.MonthlyBudgetDeposits;
            var newJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            janeBankAccount = dataRepository.GetBankAccount(Globals.JaneCheckingBankAccountId);
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

            //No longer relevant with new bank month deposit/withdrawal calculations will differ from moth to month.
            return;

            var dataRepository = AppGlobals.DataRepository;

            //CreateAndTestBankAccounts();

            //CreateAndTestBudgetItems();

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            var janeCheckingAccount = dataRepository.GetBankAccount(Globals.JaneCheckingBankAccountId);
            var oldJaneDeposits = janeCheckingAccount.MonthlyBudgetDeposits;
            var oldJaneWithdrawals = janeCheckingAccount.MonthlyBudgetWithdrawals;

            var budgetItem = dataRepository.GetBudgetItem(Globals.JaneIncomeBudgetItemId);
            budgetItemViewModel.OnRecordSelected(budgetItem);

            Globals.MessageBoxResult = MessageBoxButtonsResult.Yes;
            budgetItemViewModel.DeleteCommand.Execute(null);

            janeCheckingAccount = dataRepository.GetBankAccount(Globals.JaneCheckingBankAccountId);
            var newJaneDeposits = janeCheckingAccount.MonthlyBudgetDeposits;

            Assert.AreEqual(oldJaneDeposits - budgetItem.MonthlyAmount, newJaneDeposits,
                "Delete Income, Update Deposits");

            budgetItem = dataRepository.GetBudgetItem(Globals.GroceriesBudgetItemId);

            budgetItemViewModel.OnRecordSelected(budgetItem);

            budgetItemViewModel.DeleteCommand.Execute(null);

            janeCheckingAccount = dataRepository.GetBankAccount(Globals.JaneCheckingBankAccountId);
            var newJaneWithdrawals = janeCheckingAccount.MonthlyBudgetWithdrawals;

            Assert.AreEqual(oldJaneWithdrawals - budgetItem.MonthlyAmount, newJaneWithdrawals,
                "Delete Expense, Update Withdrawals");

            janeCheckingAccount = dataRepository.GetBankAccount(Globals.JaneCheckingBankAccountId);
            oldJaneDeposits = janeCheckingAccount.MonthlyBudgetDeposits;

            var jointCheckingAccount = dataRepository.GetBankAccount(Globals.JointCheckingBankAccountId);
            var oldJointWithdrawals = jointCheckingAccount.MonthlyBudgetWithdrawals;

            budgetItem = dataRepository.GetBudgetItem(Globals.TransferBudgetItemId);

            budgetItemViewModel.OnRecordSelected(budgetItem);

            budgetItemViewModel.DeleteCommand.Execute(null);

            janeCheckingAccount = dataRepository.GetBankAccount(Globals.JaneCheckingBankAccountId);
            jointCheckingAccount = dataRepository.GetBankAccount(Globals.JointCheckingBankAccountId);

            newJaneDeposits = janeCheckingAccount.MonthlyBudgetDeposits;
            Assert.AreEqual(oldJaneDeposits - budgetItem.MonthlyAmount, newJaneDeposits,
                "Delete Transfer, Update Deposits");

            var newJointWithdrawals = jointCheckingAccount.MonthlyBudgetWithdrawals;
            Assert.AreEqual(oldJointWithdrawals - budgetItem.MonthlyAmount, newJointWithdrawals,
                "Delete Transfer, Update Withdrawals");
        }

        [TestMethod]
        public void TestNewBudget_NoPayCCGenerated()
        {
            Globals.ClearData();

            var dataRepository = AppGlobals.DataRepository;
            var budgetItem = dataRepository.GetBudgetItem(Globals.CCPaymentBudgetItemId);

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            budgetItemViewModel.OnRecordSelected(budgetItem);

            var newCCBankAccount = dataRepository.GetBankAccount(Globals.MasterCard_PayCCOffEveryMonth_BankAccountId);
            budgetItemViewModel.TransferToBankAccountAutoFillValue = newCCBankAccount.GetAutoFillValue();
            budgetItemViewModel.SaveCommand.Execute(null);
        }
    }
    }
