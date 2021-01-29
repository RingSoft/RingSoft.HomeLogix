using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Tests
{
    public class TestBudgetItemView : TestDbMaintenanceView, IBudgetItemView
    {
        public void SetViewType()
        {
            
        }

        public TestBudgetItemView(string ownerName) : base(ownerName)
        {
        }
    }

    [TestClass]
    public class BudgetItemViewModelTests
    {
        public const int SavingsBankAccountId = 1;
        public const int JointCheckingBankAccountId = 2;
        public const int JaneCheckingBankAccountId = 3;
        public const int JuniorCheckingBankAccountId = 4;

        public const int JohnIncomeBudgetItemId = 1;
        public const int JaneIncomeBudgetItemId = 2;
        public const int HousePaymentBudgetItemId = 3;
        public const int GroceriesBudgetItemId = 4;
        public const int TransferBudgetItemId = 5;
        public const int EscrowToSavingsBudgetItemId = 6;
        public const int EscrowToJointBudgetItemId = 7;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            AppGlobals.UnitTesting = true;

            AppGlobals.Initialize();
        }

        [TestMethod]
        public void TestBudgetItemTransfer_Swap_TransferFrom_TransferTo()
        {
            var dataRepository = new TestDataRepository();
            AppGlobals.DataRepository = dataRepository;

            CreateAndTestBankAccounts();

            CreateAndTestBudgetItems();

            var jointBankAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var oldJointMonthlyDeposits = jointBankAccount.MonthlyBudgetDeposits;
            var oldJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            var janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var oldJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;
            var oldJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.OnViewLoaded(
                new TestBudgetItemView(nameof(TestBudgetItemTransfer_Swap_TransferFrom_TransferTo)));

            var transferBudgetItem = dataRepository.GetBudgetItem(TransferBudgetItemId);
            budgetItemViewModel.UnitTestLoadFromEntity(transferBudgetItem);
            var oldMonthlyAmount = budgetItemViewModel.MonthlyAmount;

            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(janeBankAccount),
                janeBankAccount.Description);

            budgetItemViewModel.TransferToBankAccountAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(jointBankAccount),
                jointBankAccount.Description);

            budgetItemViewModel.Amount = 175;

            budgetItemViewModel.UnitTestGetEntityData();
            Assert.IsNull(budgetItemViewModel.DbBankAccount,
                $"{nameof(TestBudgetItemTransfer_Swap_TransferFrom_TransferTo)} {nameof(budgetItemViewModel.DbBankAccount)} is null");

            Assert.IsNull(budgetItemViewModel.DbTransferToBankAccount,
                $"{nameof(TestBudgetItemTransfer_Swap_TransferFrom_TransferTo)} {nameof(budgetItemViewModel.DbTransferToBankAccount)} is null");

            budgetItemViewModel.DoSave(true);
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
            var dataRepository = new TestDataRepository();
            AppGlobals.DataRepository = dataRepository;

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

            var savingsBankAccount = dataRepository.GetBankAccount(SavingsBankAccountId);
            var oldSavingsMonthlyDeposits = savingsBankAccount.MonthlyBudgetDeposits;
            var oldSavingsMonthlyWithdrawals = savingsBankAccount.MonthlyBudgetWithdrawals;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.OnViewLoaded(
                new TestBudgetItemView(nameof(TestBudgetItemTransfer_ChangeTransferFrom_AndTransferTo)));

            var transferBudgetItem = dataRepository.GetBudgetItem(TransferBudgetItemId);
            budgetItemViewModel.UnitTestLoadFromEntity(transferBudgetItem);
            var oldMonthlyAmount = budgetItemViewModel.MonthlyAmount;

            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(juniorBankAccount),
                janeBankAccount.Description);

            budgetItemViewModel.TransferToBankAccountAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(savingsBankAccount),
                jointBankAccount.Description);

            budgetItemViewModel.Amount = 175;

            budgetItemViewModel.DoSave(true);
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

            savingsBankAccount = dataRepository.GetBankAccount(SavingsBankAccountId);
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
            var dataRepository = new TestDataRepository();
            AppGlobals.DataRepository = dataRepository;

            CreateAndTestBankAccounts();

            var jointBankAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var oldJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            var janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var oldJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.OnViewLoaded(
                new TestBudgetItemView(nameof(TestBudgetItemTransfer_ChangeTransferFrom_KeepTransferTo)));
            
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


            budgetItemViewModel.DoSave(true);
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
            var dataRepository = new TestDataRepository();
            AppGlobals.DataRepository = dataRepository;

            CreateAndTestBankAccounts();

            CreateAndTestBudgetItems();

            var jointBankAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var oldJointMonthlyDeposits = jointBankAccount.MonthlyBudgetDeposits;
            var oldJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            var janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var oldJaneMonthlyDeposits = janeBankAccount.MonthlyBudgetDeposits;
            var oldJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.OnViewLoaded(new TestBudgetItemView(nameof(TestBudgetItemIncome_Change)));

            var janeIncomeBudgetItem = dataRepository.GetBudgetItem(JaneIncomeBudgetItemId);
            budgetItemViewModel.UnitTestLoadFromEntity(janeIncomeBudgetItem);
            var oldMonthlyAmount = budgetItemViewModel.MonthlyAmount;

            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(jointBankAccount),
                janeBankAccount.Description);

            budgetItemViewModel.Amount = 600;
            budgetItemViewModel.DoSave(true);
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
        public void TestChangeBudgetItemEscrow_AmountChange()
        {
            var dataRepository = new TestDataRepository();
            AppGlobals.DataRepository = dataRepository;

            CreateAndTestBankAccounts();

            CreateAndTestBudgetItems();

            var savingsBankAccount = dataRepository.GetBankAccount(SavingsBankAccountId);
            var oldSavingsMonthlyDeposits = savingsBankAccount.MonthlyBudgetDeposits;
            var oldSavingsEscrowBalance = savingsBankAccount.EscrowBalance;

            var janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var oldJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.OnViewLoaded(new TestBudgetItemView(nameof(TestChangeBudgetItemEscrow_AmountChange)));

            var escrowToSavingsBudgetItem = dataRepository.GetBudgetItem(EscrowToSavingsBudgetItemId);
            budgetItemViewModel.UnitTestLoadFromEntity(escrowToSavingsBudgetItem);
            var oldMonthlyAmount = budgetItemViewModel.MonthlyAmount;
            var oldEscrowBalance = budgetItemViewModel.EscrowBalance;

            //budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
            //    AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(jointBankAccount),
            //    janeBankAccount.Description);

            budgetItemViewModel.Amount = 600;

            var escrowDi = budgetItemViewModel.MonthlyAmount;
            var newEscrowBalance = budgetItemViewModel.EscrowBalance;

            budgetItemViewModel.DoSave(true);

            savingsBankAccount = dataRepository.GetBankAccount(SavingsBankAccountId);
            var newSavingsMonthlyDeposits = savingsBankAccount.MonthlyBudgetDeposits;
            var newSavingsEscrowBalance = savingsBankAccount.EscrowBalance;

            janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var newJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            Assert.AreEqual(oldSavingsMonthlyDeposits + newMonthlyAmount, newSavingsMonthlyDeposits,
                nameof(TestChangeBudgetItemEscrow_AmountChange));

            Assert.AreEqual(oldSavingsEscrowBalance, newSavingsEscrowBalance,
                nameof(TestBudgetItemIncome_Change));

            Assert.AreEqual(oldJaneMonthlyDeposits - oldMonthlyAmount, newJaneMonthlyDeposits,
                nameof(TestBudgetItemIncome_Change));

            Assert.AreEqual(oldJaneMonthlyWithdrawals, newJaneMonthlyWithdrawals,
                nameof(TestBudgetItemIncome_Change));
        }

        private static void CreateAndTestBudgetItems()
        {
            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.OnViewLoaded(new TestBudgetItemView(nameof(CreateAndTestBudgetItems)));

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
            budgetItemViewModel.StartingDate = DateTime.Parse("02/05/2021");

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoSave(true),
                "Saving Jane's Income Budget Item");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            Assert.AreEqual(expected: (decimal)1178.57, bankAccount.MonthlyBudgetDeposits,
                "Jane's Checking Initial Monthly Deposits");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.OnNewButton();
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
            budgetItemViewModel.StartingDate = DateTime.Parse("02/03/2021");

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoSave(true),
                "Saving John's Income Budget Item");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JointCheckingBankAccountId);
            Assert.AreEqual(1530, bankAccount.MonthlyBudgetDeposits,
                "Joint Checking Initial Monthly Deposits");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.OnNewButton();
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
            budgetItemViewModel.StartingDate = DateTime.Parse("02/03/2021");

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoSave(true),
                "Saving House Payment Budget Item");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JointCheckingBankAccountId);
            Assert.AreEqual(790, bankAccount.MonthlyBudgetWithdrawals,
                "Joint Checking Initial Monthly Withdrawals");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.OnNewButton();
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
            budgetItemViewModel.StartingDate = DateTime.Parse("01/29/2021");

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoSave(true),
                "Saving Groceries Budget Item");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            Assert.AreEqual((decimal)1285.71, bankAccount.MonthlyBudgetWithdrawals,
                "Jane's Checking Initial Monthly Withdrawals");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.OnNewButton();
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
            budgetItemViewModel.StartingDate = DateTime.Parse("02/03/2021");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var monthlyDeposits = bankAccount.MonthlyBudgetDeposits;

            budgetItemViewModel.TransferToBankAccountAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoSave(true),
                "Saving Transfer Error Item");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JointCheckingBankAccountId);
            Assert.AreEqual(monthlyWithdrawals + 100, bankAccount.MonthlyBudgetWithdrawals,
                "Joint Checking Monthly Withdrawals After Initial Transfer");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            Assert.AreEqual(monthlyDeposits + 100, bankAccount.MonthlyBudgetDeposits,
                "Jane's Checking Monthly Deposits After Initial Transfer");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.OnNewButton();
            budgetItemViewModel.Id = EscrowToSavingsBudgetItemId;
            budgetItemViewModel.KeyAutoFillValue = new AutoFillValue(null, "Escrow To Savings");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            budgetItemViewModel.BudgetItemType = BudgetItemTypes.Expense;
            budgetItemViewModel.Amount = 500;
            budgetItemViewModel.RecurringPeriod = 6;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Months;
            budgetItemViewModel.StartingDate = bankAccount.LastGenerationDate.AddMonths(5).AddDays(1);

            Assert.AreEqual((decimal)83.33, budgetItemViewModel.MonthlyAmount, "Escrow Initial Monthly Amount");

            Assert.AreEqual((decimal)83.33, budgetItemViewModel.EscrowBalance, "Initial Savings Escrow Balance");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var monthlyBudgetWithdrawals = bankAccount.MonthlyBudgetWithdrawals;

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoSave(true),
                "Saving Escrow To Savings Budget Item");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            Assert.AreEqual( monthlyBudgetWithdrawals + (decimal)83.33, bankAccount.MonthlyBudgetWithdrawals,
                "Jane's Checking Monthly Withdrawals Changed by Escrow");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(SavingsBankAccountId);
            Assert.AreEqual((decimal)83.33, bankAccount.MonthlyBudgetDeposits,
                "Savings Monthly Deposits Changed By Escrow");
        }

        private void CreateAndTestBankAccounts()
        {
            var bankAccountViewModel = new BankAccountViewModel();
            bankAccountViewModel.OnViewLoaded(new TestBankAccountView(nameof(CreateAndTestBankAccounts)));

            bankAccountViewModel.Id = SavingsBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Savings Account");

            bankAccountViewModel.DoSave(true);
            bankAccountViewModel.OnNewButton();

            bankAccountViewModel.Id = JointCheckingBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Joint Checking Account");

            bankAccountViewModel.DoSave(true);
            bankAccountViewModel.OnNewButton();

            bankAccountViewModel.Id = JaneCheckingBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Jane's Checking Account");
            var savingsBank = AppGlobals.DataRepository.GetBankAccount(SavingsBankAccountId, false);
            var savingsBankPrimaryKeyValue =
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(savingsBank);
            bankAccountViewModel.EscrowBankAccountAutoFillValue =
                new AutoFillValue(savingsBankPrimaryKeyValue, savingsBank.Description);
            bankAccountViewModel.EscrowDayOfMonth = 3;

            var newDate = DateTime.Today;
            newDate =
                newDate.AddDays(DateTime.DaysInMonth(newDate.Year, newDate.Month) - newDate.Day);

            Assert.AreEqual(newDate, bankAccountViewModel.LastGenerationDate,
                $"Jane's Checking Account {nameof(bankAccountViewModel.LastGenerationDate)} set right.");

            bankAccountViewModel.DoSave(true);
            bankAccountViewModel.OnNewButton();

            bankAccountViewModel.Id = JuniorCheckingBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Junior's Checking Account");

            bankAccountViewModel.DoSave(true);
        }
    }
}
