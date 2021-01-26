using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.AutoFill;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Tests
{
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

            CreateBankAccounts(dataRepository);
        }

        private void CreateBankAccounts(TestDataRepository dataRepository)
        {
            var bankAccountViewModel = new BankAccountViewModel
            {
                Id = JointCheckingBankAccountId,
                KeyAutoFillValue = new AutoFillValue(null, "Joint Checking Account")
            };
            bankAccountViewModel.DoSave();
        }
    }
}
