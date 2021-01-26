using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.AutoFill;
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
    }

    [TestClass]
    public class BudgetItemViewModelTests
    {
        public const int JointCheckingBankAccountId = 1;
        public const int JaneCheckingBankAccountId = 2;
        public const int JuniorCheckingBankAccountId = 3;
        public const int SavingsBankAccountId = 4;

        public const int JohnIncomeBudgetItemId = 1;
        public const int JaneIncomeBudgetItemId = 2;
        public const int HousePaymentBudgetItemId = 3;
        public const int GroceriesBudgetItemId = 4;
        public const int TransferBudgetItemId = 5;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            AppGlobals.UnitTesting = true;

            AppGlobals.Initialize();
        }

        [TestMethod]
        public void TestBudgetTransferChanges()
        {
            var dataRepository = new TestDataRepository();
            AppGlobals.DataRepository = dataRepository;

            CreateBankAccounts();

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.OnViewLoaded(new TestBudgetItemView());

            budgetItemViewModel.Id = JaneIncomeBudgetItemId;
            budgetItemViewModel.KeyAutoFillValue = new AutoFillValue(null, "Jane's Income");
            budgetItemViewModel.BudgetItemType = BudgetItemTypes.Income;
            var bankAccount = dataRepository.GetBankAccount(JaneCheckingBankAccountId);
            budgetItemViewModel.BankAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(bankAccount),
                bankAccount.Description);

            budgetItemViewModel.Amount = 1000;
            budgetItemViewModel.RecurringPeriod = 1;
            budgetItemViewModel.RecurringType = BudgetItemRecurringTypes.Months;
            budgetItemViewModel.StartingDate = new DateTime(2021, 2, 5);
            budgetItemViewModel.DoSave(true);
        }

        private void CreateBankAccounts()
        {
            var bankAccountViewModel = new BankAccountViewModel();
            bankAccountViewModel.OnViewLoaded(new TestBankAccountView());

            bankAccountViewModel.Id = JointCheckingBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Joint Checking Account");

            bankAccountViewModel.DoSave(true);
            bankAccountViewModel.OnNewButton();

            bankAccountViewModel.Id = JaneCheckingBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Jane's Checking Account");

            bankAccountViewModel.DoSave(true);
            bankAccountViewModel.OnNewButton();

            bankAccountViewModel.Id = JuniorCheckingBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Junior's Checking Account");

            bankAccountViewModel.DoSave(true);
            bankAccountViewModel.OnNewButton();

            bankAccountViewModel.Id = SavingsBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Savings Account");

            bankAccountViewModel.DoSave(true);
        }
    }
}
