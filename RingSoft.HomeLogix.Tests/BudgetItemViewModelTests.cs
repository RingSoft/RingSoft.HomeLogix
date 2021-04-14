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
        public const int EscrowToJaneSavingsBudgetItemId = 6;
        public const int JuniorIncomeBudgetItemId = 7;
        public const int JuniorPaymentBudgetItemId = 8;
        public const int JuniorSavingsDepositBudgetItemId = 9;
        public const int SallyAllowanceBudgetItemId = 10;
        public const int SallyGameFeeBudgetItemId = 11;

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

            var savingsBankAccount = dataRepository.GetBankAccount(JaneSavingsBankAccountId);
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

            var savingsBankAccount = dataRepository.GetBankAccount(JaneSavingsBankAccountId);
            var oldSavingsMonthlyDeposits = savingsBankAccount.MonthlyBudgetDeposits;
            var oldSavingsEscrowBalance = savingsBankAccount.EscrowBalance;

            var janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var oldJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.OnViewLoaded(new TestBudgetItemView(nameof(TestChangeBudgetItemEscrow_AmountChange)));

            var escrowToSavingsBudgetItem = dataRepository.GetBudgetItem(EscrowToJaneSavingsBudgetItemId);
            budgetItemViewModel.UnitTestLoadFromEntity(escrowToSavingsBudgetItem);
            var oldMonthlyAmount = budgetItemViewModel.MonthlyAmount;
            var oldEscrowBalance = budgetItemViewModel.EscrowBalance;

            budgetItemViewModel.Amount = 600;

            var monthlyDifference = budgetItemViewModel.MonthlyAmount - oldMonthlyAmount;
            var escrowDifference = budgetItemViewModel.EscrowBalance - oldEscrowBalance;

            budgetItemViewModel.DoSave(true);

            savingsBankAccount = dataRepository.GetBankAccount(JaneSavingsBankAccountId);
            var newSavingsMonthlyDeposits = savingsBankAccount.MonthlyBudgetDeposits;
            var newSavingsEscrowBalance = savingsBankAccount.EscrowBalance;

            escrowToSavingsBudgetItem = dataRepository.GetBudgetItem(EscrowToJaneSavingsBudgetItemId);

            janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var newJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            Assert.AreEqual(oldSavingsMonthlyDeposits + monthlyDifference, newSavingsMonthlyDeposits,
                nameof(TestChangeBudgetItemEscrow_AmountChange));

            Assert.AreEqual(oldSavingsEscrowBalance + escrowDifference, newSavingsEscrowBalance,
                nameof(TestChangeBudgetItemEscrow_AmountChange));

            Assert.AreEqual(oldJaneMonthlyWithdrawals + monthlyDifference, newJaneMonthlyWithdrawals,
                nameof(TestChangeBudgetItemEscrow_AmountChange));

            Assert.AreEqual(oldEscrowBalance + escrowDifference, escrowToSavingsBudgetItem.EscrowBalance, 
                nameof(TestChangeBudgetItemEscrow_AmountChange));
        }

        [TestMethod]
        public void TestChangeBudgetItemEscrow_Bank_SameEscrowToAndAmountChange()
        {
            var dataRepository = new TestDataRepository();
            AppGlobals.DataRepository = dataRepository;

            CreateAndTestBankAccounts();

            CreateAndTestBudgetItems();

            var savingsBankAccount = dataRepository.GetBankAccount(JaneSavingsBankAccountId);
            var oldSavingsMonthlyDeposits = savingsBankAccount.MonthlyBudgetDeposits;
            var oldSavingsEscrowBalance = savingsBankAccount.EscrowBalance;

            var janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var oldJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            var jointBankAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var oldJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.OnViewLoaded(
                new TestBudgetItemView(nameof(TestChangeBudgetItemEscrow_Bank_SameEscrowToAndAmountChange)));

            var escrowToSavingsBudgetItem = dataRepository.GetBudgetItem(EscrowToJaneSavingsBudgetItemId);
            budgetItemViewModel.UnitTestLoadFromEntity(escrowToSavingsBudgetItem);
            var oldMonthlyAmount = budgetItemViewModel.MonthlyAmount;
            var oldEscrowBalance = budgetItemViewModel.EscrowBalance;

            budgetItemViewModel.Amount = 600;

            var monthlyDifference = budgetItemViewModel.MonthlyAmount - oldMonthlyAmount;
            var escrowDifference = budgetItemViewModel.EscrowBalance - oldEscrowBalance;

            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(jointBankAccount),
                jointBankAccount.Description);

            budgetItemViewModel.DoSave(true);

            savingsBankAccount = dataRepository.GetBankAccount(JaneSavingsBankAccountId);
            var newSavingsMonthlyDeposits = savingsBankAccount.MonthlyBudgetDeposits;
            var newSavingsEscrowBalance = savingsBankAccount.EscrowBalance;

            escrowToSavingsBudgetItem = dataRepository.GetBudgetItem(EscrowToJaneSavingsBudgetItemId);

            janeBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var newJaneMonthlyWithdrawals = janeBankAccount.MonthlyBudgetWithdrawals;

            Assert.AreEqual(oldSavingsMonthlyDeposits + monthlyDifference, newSavingsMonthlyDeposits,
                nameof(TestChangeBudgetItemEscrow_Bank_SameEscrowToAndAmountChange));

            Assert.AreEqual(oldSavingsEscrowBalance + escrowDifference, newSavingsEscrowBalance,
                nameof(TestChangeBudgetItemEscrow_Bank_SameEscrowToAndAmountChange));

            Assert.AreEqual(oldJaneMonthlyWithdrawals - oldMonthlyAmount, newJaneMonthlyWithdrawals,
                nameof(TestChangeBudgetItemEscrow_Bank_SameEscrowToAndAmountChange));

            jointBankAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var newJointMonthlyWithdrawals = jointBankAccount.MonthlyBudgetWithdrawals;

            Assert.AreEqual(oldJointMonthlyWithdrawals + budgetItemViewModel.MonthlyAmount, newJointMonthlyWithdrawals,
                nameof(TestChangeBudgetItemEscrow_Bank_SameEscrowToAndAmountChange));

            Assert.AreEqual(oldEscrowBalance + escrowDifference, escrowToSavingsBudgetItem.EscrowBalance,
                nameof(TestChangeBudgetItemEscrow_Bank_SameEscrowToAndAmountChange));
        }

        [TestMethod]
        public void TestChangeBudgetItemEscrow_Bank_DifferentEscrowToAndAmountChange()
        {
            var dataRepository = new TestDataRepository();
            AppGlobals.DataRepository = dataRepository;

            CreateAndTestBankAccounts();

            CreateAndTestBudgetItems();

            var janeSavingsBankAccount = dataRepository.GetBankAccount(JaneSavingsBankAccountId);
            var oldJaneSavingsMonthlyDeposits = janeSavingsBankAccount.MonthlyBudgetDeposits;
            var oldJaneSavingsEscrowBalance = janeSavingsBankAccount.EscrowBalance;

            var janeCheckingBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var oldJaneCheckingMonthlyWithdrawals = janeCheckingBankAccount.MonthlyBudgetWithdrawals;

            var juniorCheckingBankAccount = dataRepository.GetBankAccount(JuniorCheckingBankAccountId);
            var oldJuniorCheckingMonthlyWithdrawals = juniorCheckingBankAccount.MonthlyBudgetWithdrawals;

            var juniorSavingsBankAccount = dataRepository.GetBankAccount(JuniorSavingsBankAccountId);
            var oldJuniorSavingsMonthlyDeposits = juniorSavingsBankAccount.MonthlyBudgetDeposits;
            var oldJuniorSavingsEscrowBalance = juniorSavingsBankAccount.EscrowBalance;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.OnViewLoaded(
                new TestBudgetItemView(nameof(TestChangeBudgetItemEscrow_Bank_DifferentEscrowToAndAmountChange)));
            var escrowToSavingsBudgetItem = dataRepository.GetBudgetItem(EscrowToJaneSavingsBudgetItemId);

            budgetItemViewModel.UnitTestLoadFromEntity(escrowToSavingsBudgetItem);
            var oldMonthlyAmount = budgetItemViewModel.MonthlyAmount;
            var oldEscrowBalance = budgetItemViewModel.EscrowBalance;

            budgetItemViewModel.Amount = 600;

            var escrowDifference = budgetItemViewModel.EscrowBalance - oldEscrowBalance;

            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(juniorCheckingBankAccount),
                juniorCheckingBankAccount.Description);

            budgetItemViewModel.DoSave(true);
            
            escrowToSavingsBudgetItem = dataRepository.GetBudgetItem(EscrowToJaneSavingsBudgetItemId);

            Assert.AreEqual(oldEscrowBalance + escrowDifference,
                escrowToSavingsBudgetItem.MonthlyAmount,
                nameof(TestChangeBudgetItemEscrow_Bank_DifferentEscrowToAndAmountChange));

            janeSavingsBankAccount = dataRepository.GetBankAccount(JaneSavingsBankAccountId);
            var newJaneSavingsMonthlyDeposits = janeSavingsBankAccount.MonthlyBudgetDeposits;
            var newJaneSavingsEscrowBalance = janeSavingsBankAccount.EscrowBalance;

            janeCheckingBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var newJaneCheckingMonthlyWithdrawals = janeCheckingBankAccount.MonthlyBudgetWithdrawals;

            Assert.AreEqual(oldJaneSavingsMonthlyDeposits - oldMonthlyAmount,
                newJaneSavingsMonthlyDeposits,
                nameof(TestChangeBudgetItemEscrow_Bank_DifferentEscrowToAndAmountChange));

            Assert.AreEqual(oldJaneSavingsEscrowBalance - oldEscrowBalance,
                newJaneSavingsEscrowBalance,
                nameof(TestChangeBudgetItemEscrow_Bank_DifferentEscrowToAndAmountChange));

            Assert.AreEqual(oldJaneCheckingMonthlyWithdrawals - oldMonthlyAmount,
                newJaneCheckingMonthlyWithdrawals,
                nameof(TestChangeBudgetItemEscrow_Bank_DifferentEscrowToAndAmountChange));

            juniorCheckingBankAccount = dataRepository.GetBankAccount(JuniorCheckingBankAccountId);
            var newJuniorCheckingMonthlyWithdrawals = juniorCheckingBankAccount.MonthlyBudgetWithdrawals;

            juniorSavingsBankAccount = dataRepository.GetBankAccount(JuniorSavingsBankAccountId);
            var newJuniorSavingsMonthlyDeposits = juniorSavingsBankAccount.MonthlyBudgetDeposits;
            var newJuniorSavingsEscrowBalance = juniorSavingsBankAccount.EscrowBalance;

            Assert.AreEqual(oldJuniorCheckingMonthlyWithdrawals + budgetItemViewModel.MonthlyAmount,
                newJuniorCheckingMonthlyWithdrawals,
                nameof(TestChangeBudgetItemEscrow_Bank_DifferentEscrowToAndAmountChange));

            Assert.AreEqual(oldJuniorSavingsMonthlyDeposits + budgetItemViewModel.MonthlyAmount,
                newJuniorSavingsMonthlyDeposits,
                nameof(TestChangeBudgetItemEscrow_Bank_DifferentEscrowToAndAmountChange));

            Assert.AreEqual(oldJuniorSavingsEscrowBalance + budgetItemViewModel.MonthlyAmount,
                newJuniorSavingsEscrowBalance,
                nameof(TestChangeBudgetItemEscrow_Bank_DifferentEscrowToAndAmountChange));
        }

        [TestMethod]
        public void TestUnEscrow()
        {
            var dataRepository = new TestDataRepository();
            AppGlobals.DataRepository = dataRepository;

            CreateAndTestBankAccounts();

            CreateAndTestBudgetItems();

            var janeSavingsBankAccount = dataRepository.GetBankAccount(JaneSavingsBankAccountId);
            var oldJaneSavingsMonthlyDeposits = janeSavingsBankAccount.MonthlyBudgetDeposits;
            var oldJaneSavingsEscrowBalance = janeSavingsBankAccount.EscrowBalance;

            var janeCheckingBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var oldJaneCheckingMonthlyWithdrawals = janeCheckingBankAccount.MonthlyBudgetWithdrawals;

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.OnViewLoaded(
                new TestBudgetItemView(nameof(TestUnEscrow)));
            var escrowToSavingsBudgetItem = dataRepository.GetBudgetItem(EscrowToJaneSavingsBudgetItemId);

            budgetItemViewModel.UnitTestLoadFromEntity(escrowToSavingsBudgetItem);
            var oldMonthlyAmount = budgetItemViewModel.MonthlyAmount;
            var oldEscrowBalance = budgetItemViewModel.EscrowBalance;

            budgetItemViewModel.DoEscrow = false;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Months;
            budgetItemViewModel.RecurringPeriod = 1;
            budgetItemViewModel.Amount = 40;

            budgetItemViewModel.DoSave(true);

            escrowToSavingsBudgetItem = dataRepository.GetBudgetItem(EscrowToJaneSavingsBudgetItemId);

            Assert.AreEqual(40,
                escrowToSavingsBudgetItem.MonthlyAmount,
                nameof(TestUnEscrow));

            janeSavingsBankAccount = dataRepository.GetBankAccount(JaneSavingsBankAccountId);
            var newJaneSavingsMonthlyDeposits = janeSavingsBankAccount.MonthlyBudgetDeposits;
            var newJaneSavingsEscrowBalance = janeSavingsBankAccount.EscrowBalance;

            janeCheckingBankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var newJaneCheckingMonthlyWithdrawals = janeCheckingBankAccount.MonthlyBudgetWithdrawals;

            Assert.AreEqual(oldJaneSavingsMonthlyDeposits - oldMonthlyAmount,
                newJaneSavingsMonthlyDeposits,
                nameof(TestUnEscrow));

            Assert.AreEqual(oldJaneSavingsEscrowBalance - oldEscrowBalance,
                newJaneSavingsEscrowBalance,
                nameof(TestUnEscrow));

            Assert.AreEqual(
                (oldJaneCheckingMonthlyWithdrawals - oldMonthlyAmount) + escrowToSavingsBudgetItem.MonthlyAmount,
                newJaneCheckingMonthlyWithdrawals,
                nameof(TestUnEscrow));
        }

        [TestMethod]
        public void TestDeleteBudgetItems()
        {
            var dataRepository = new TestDataRepository();
            AppGlobals.DataRepository = dataRepository;

            CreateAndTestBankAccounts();

            CreateAndTestBudgetItems();

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.OnViewLoaded(
                new TestBudgetItemView(nameof(TestDeleteBudgetItems)));

            var janeCheckingAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var oldJaneDeposits = janeCheckingAccount.MonthlyBudgetDeposits;
            var oldJaneWithdrawals = janeCheckingAccount.MonthlyBudgetWithdrawals;

            var budgetItem = dataRepository.GetBudgetItem(JaneIncomeBudgetItemId);
            budgetItemViewModel.UnitTestLoadFromEntity(budgetItem);

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoDelete(), "Delete Income");

            janeCheckingAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var newJaneDeposits = janeCheckingAccount.MonthlyBudgetDeposits;

            Assert.AreEqual(oldJaneDeposits - budgetItem.MonthlyAmount, newJaneDeposits,
                "Delete Income, Update Deposits");

            budgetItem = dataRepository.GetBudgetItem(GroceriesBudgetItemId);
            budgetItemViewModel.UnitTestLoadFromEntity(budgetItem);

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoDelete(), "Delete Expense");

            janeCheckingAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var newJaneWithdrawals = janeCheckingAccount.MonthlyBudgetWithdrawals;

            Assert.AreEqual(oldJaneWithdrawals - budgetItem.MonthlyAmount, newJaneWithdrawals,
                "Delete Expense, Update Withdrawals");

            janeCheckingAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            oldJaneDeposits = janeCheckingAccount.MonthlyBudgetDeposits;

            var jointCheckingAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);
            var oldJointWithdrawals = jointCheckingAccount.MonthlyBudgetWithdrawals;

            budgetItem = dataRepository.GetBudgetItem(TransferBudgetItemId);
            budgetItemViewModel.UnitTestLoadFromEntity(budgetItem);

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoDelete(), "Delete Transfer");
            janeCheckingAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            jointCheckingAccount = dataRepository.GetBankAccount(JointCheckingBankAccountId);

            newJaneDeposits = janeCheckingAccount.MonthlyBudgetDeposits;
            Assert.AreEqual(oldJaneDeposits - budgetItem.MonthlyAmount, newJaneDeposits,
                "Delete Transfer, Update Deposits");

            var newJointWithdrawals = jointCheckingAccount.MonthlyBudgetWithdrawals;
            Assert.AreEqual(oldJointWithdrawals - budgetItem.MonthlyAmount, newJointWithdrawals,
                "Delete Transfer, Update Withdrawals");

            oldJaneWithdrawals = janeCheckingAccount.MonthlyBudgetWithdrawals;
            var janeSavingsBankAccount = dataRepository.GetBankAccount(JaneSavingsBankAccountId, false);
            oldJaneDeposits = janeSavingsBankAccount.MonthlyBudgetDeposits;
            var oldEscrowAmount = janeSavingsBankAccount.EscrowBalance;

            budgetItem = dataRepository.GetBudgetItem(EscrowToJaneSavingsBudgetItemId);
            budgetItemViewModel.UnitTestLoadFromEntity(budgetItem);
            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoDelete(), "Delete Escrow");

            janeCheckingAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            janeSavingsBankAccount = dataRepository.GetBankAccount(JaneSavingsBankAccountId);
            newJaneWithdrawals = janeCheckingAccount.MonthlyBudgetWithdrawals;
            newJaneDeposits = janeSavingsBankAccount.MonthlyBudgetDeposits;
            var newEscrowAmount = janeSavingsBankAccount.EscrowBalance;

            Assert.AreEqual(oldJaneDeposits - budgetItem.MonthlyAmount, newJaneDeposits, 
                "Delete Escrow, Update Deposits.");

            Assert.AreEqual(oldJaneWithdrawals - budgetItem.MonthlyAmount, newJaneWithdrawals,
                "Delete Escrow, Update Withdrawals.");

            Assert.AreEqual(oldEscrowAmount - budgetItem.EscrowBalance, newEscrowAmount,
                "Delete Escrow, Update Escrow");
        }

        private static void CreateAndTestBudgetItems(bool doEscrow = true)
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
            budgetItemViewModel.Id = EscrowToJaneSavingsBudgetItemId;
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
            budgetItemViewModel.DoEscrow = doEscrow;

            decimal expectedEscrowValue = doEscrow ? (decimal)83.33 : 0;

            Assert.AreEqual(expectedEscrowValue, budgetItemViewModel.MonthlyAmount, "Escrow Initial Monthly Amount");

            Assert.AreEqual(expectedEscrowValue, budgetItemViewModel.EscrowBalance,
                "Initial Budget Item Escrow Balance");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            var monthlyBudgetWithdrawals = bankAccount.MonthlyBudgetWithdrawals;

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoSave(true),
                "Saving Escrow To Jane's Savings Budget Item");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneCheckingBankAccountId);
            Assert.AreEqual( monthlyBudgetWithdrawals + expectedEscrowValue, bankAccount.MonthlyBudgetWithdrawals,
                "Jane's Checking Monthly Withdrawals Changed by Escrow");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JaneSavingsBankAccountId);
            Assert.AreEqual(expectedEscrowValue, bankAccount.MonthlyBudgetDeposits,
                "Jane's Savings Monthly Deposits Changed By Escrow");

            Assert.AreEqual(expectedEscrowValue, bankAccount.EscrowBalance,
                "Jane's Savings Initial Escrow Balance");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.OnNewButton();
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

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoSave(true),
                "Saving Income To Junior's Checking Budget Item");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JuniorCheckingBankAccountId);
            Assert.AreEqual(750, bankAccount.MonthlyBudgetDeposits,
                "Junior's Initial Income");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.OnNewButton();
            budgetItemViewModel.Id = JuniorPaymentBudgetItemId;
            budgetItemViewModel.KeyAutoFillValue = new AutoFillValue(null, "Junior's Room/Board");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JuniorCheckingBankAccountId);
            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            budgetItemViewModel.BudgetItemType = BudgetItemTypes.Expense;
            budgetItemViewModel.Amount = 600;
            budgetItemViewModel.RecurringPeriod = 2;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Months;
            budgetItemViewModel.StartingDate = DateTime.Parse("02/03/2021");
            budgetItemViewModel.DoEscrow = doEscrow;

            expectedEscrowValue = doEscrow ? (decimal) 300 : 0;
            Assert.AreEqual(expectedEscrowValue, budgetItemViewModel.MonthlyAmount, "Junior's Initial Monthly Expense");

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoSave(true),
                "Saving Expense To Junior's Checking Budget Item");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JuniorCheckingBankAccountId);
            Assert.AreEqual(expectedEscrowValue, bankAccount.MonthlyBudgetWithdrawals,
                "Junior's Room/Board Expense");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JuniorSavingsBankAccountId);
            Assert.AreEqual(expectedEscrowValue, bankAccount.MonthlyBudgetDeposits,
                "Junior's Escrow");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.OnNewButton();
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

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoSave(true),
                "Saving Income To Junior's Savings Budget Item");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(JuniorSavingsBankAccountId);
            Assert.AreEqual(400, bankAccount.MonthlyBudgetDeposits,
                "Junior's Initial Savings Deposit");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.OnNewButton();
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

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoSave(true),
                "Saving Sally's Allowance Budget Item");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(SallyCheckingBankAccountId);
            Assert.AreEqual(100, bankAccount.MonthlyBudgetDeposits,
                "Sally's Allowance Deposit");

            //-----------------------------------------------------------------------------------------------------------

            budgetItemViewModel.OnNewButton();
            budgetItemViewModel.Id = SallyGameFeeBudgetItemId;
            budgetItemViewModel.KeyAutoFillValue = new AutoFillValue(null, "Sally's Game Fee");

            bankAccount = AppGlobals.DataRepository.GetBankAccount(SallyCheckingBankAccountId);
            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);
            bankAccount.LastGenerationDate = DateTime.Parse("12/31/2020");

            budgetItemViewModel.BudgetItemType = BudgetItemTypes.Expense;
            budgetItemViewModel.Amount = 600;
            budgetItemViewModel.RecurringPeriod = 1;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Years;
            budgetItemViewModel.StartingDate = DateTime.Parse("08/01/2021");
            budgetItemViewModel.DoEscrow = doEscrow;

            decimal monthlyAmount = doEscrow ? 50 : 0;
            Assert.AreEqual(monthlyAmount, budgetItemViewModel.MonthlyAmount, "Sally's Initial Monthly Expense");

            Assert.AreEqual(DbMaintenanceResults.Success, budgetItemViewModel.DoSave(true),
                "Saving Expense To Sally's Checking Budget Item");

            expectedEscrowValue = doEscrow ? 450 : 0;
            var budgetItem = AppGlobals.DataRepository.GetBudgetItem(SallyGameFeeBudgetItemId);
            Assert.AreEqual(expectedEscrowValue, budgetItem.EscrowBalance);

            bankAccount = AppGlobals.DataRepository.GetBankAccount(SallyCheckingBankAccountId);

            Assert.AreEqual(expectedEscrowValue, bankAccount.EscrowBalance);

            Assert.AreEqual(0, bankAccount.MonthlyBudgetWithdrawals,
                "Sally's Monthly Withdrawals Expense");

            Assert.AreEqual(100, bankAccount.MonthlyBudgetDeposits,
                "Sally's Escrow");
        }

        private void CreateAndTestBankAccounts()
        {
            var bankAccountViewModel = new BankAccountViewModel();
            bankAccountViewModel.OnViewLoaded(new TestBankAccountView(nameof(CreateAndTestBankAccounts)));

            bankAccountViewModel.Id = JaneSavingsBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Jane's Savings Account");

            bankAccountViewModel.DoSave(true);
            bankAccountViewModel.OnNewButton();

            bankAccountViewModel.Id = JuniorSavingsBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Junior's Savings Account");

            bankAccountViewModel.DoSave(true);
            bankAccountViewModel.OnNewButton();

            bankAccountViewModel.Id = JointCheckingBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Joint Checking Account");
            var savingsBank = AppGlobals.DataRepository.GetBankAccount(JaneSavingsBankAccountId, false);
            var savingsBankPrimaryKeyValue =
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(savingsBank);
            bankAccountViewModel.EscrowBankAccountAutoFillValue =
                new AutoFillValue(savingsBankPrimaryKeyValue, savingsBank.Description);
            bankAccountViewModel.EscrowDayOfMonth = 3;

            bankAccountViewModel.DoSave(true);
            bankAccountViewModel.OnNewButton();

            bankAccountViewModel.Id = JaneCheckingBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Jane's Checking Account");
            savingsBank = AppGlobals.DataRepository.GetBankAccount(JaneSavingsBankAccountId, false);
            savingsBankPrimaryKeyValue =
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
            savingsBank = AppGlobals.DataRepository.GetBankAccount(JuniorSavingsBankAccountId, false);
            savingsBankPrimaryKeyValue =
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(savingsBank);
            bankAccountViewModel.EscrowBankAccountAutoFillValue =
                new AutoFillValue(savingsBankPrimaryKeyValue, savingsBank.Description);
            bankAccountViewModel.EscrowDayOfMonth = 3;

            bankAccountViewModel.DoSave(true);
            bankAccountViewModel.OnNewButton();

            bankAccountViewModel.Id = SallyCheckingBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Sally's Checking Account");

            bankAccountViewModel.DoSave(true);
        }
    }
}
