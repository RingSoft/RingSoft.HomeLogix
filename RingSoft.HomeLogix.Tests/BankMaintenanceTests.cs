using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DevLogix.Tests;
using RingSoft.DevLogix.Tests.QualityAssurance;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Tests
{
    [TestClass]
    public class BankMaintenanceTests
    {
        public static TestGlobals<BudgetItemViewModel, BudgetView> Globals { get; } =
            new TestGlobals<BudgetItemViewModel, BudgetView>();

        static BankMaintenanceTests()
        {
            Globals = BudgetItemViewModelTests.Globals;
        }

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            Globals.Initialize();
        }

        [TestMethod]
        public void TestBankAccount_GenerateDetails()
        {
            BudgetItemViewModelTests.Globals.ClearData();

            var dataRepository = AppGlobals.DataRepository;
            BudgetItemViewModelTests.CreateAndTestBankAccounts();

            BudgetItemViewModelTests.CreateAndTestBudgetItems();

            var bankAccount = dataRepository.GetBankAccount(
                BudgetItemViewModelTests.JaneCheckingBankAccountId);

            var bankAccountViewModel = new BankAccountViewModel();
            bankAccountViewModel.Processor = Globals;
            bankAccountViewModel.OnViewLoaded(new TestBankAccountView(nameof(TestBankAccount_GenerateDetails)));

            bankAccountViewModel.OnRecordSelected(bankAccount);
            
            bankAccountViewModel.GenerateTransactions(DateTime.Today.AddMonths(3));
            bankAccountViewModel.SaveCommand.Execute(null);

            bankAccountViewModel.NewCommand.Execute(null);
            bankAccountViewModel.OnRecordSelected(bankAccount);
            
            var registerRow = bankAccountViewModel
                .RegisterGridManager
                .Rows[0];
            registerRow.SetCellValue(new DataEntryGridCheckBoxCellProps(registerRow
            , BankAccountRegisterGridManager.CompletedColumnId
            , true));

            bankAccountViewModel.SaveCommand.Execute(null);
            bankAccount.UtFillOutEntity();

            Assert.AreEqual(24, bankAccount.RegisterItems.Count);
        }
    }
}
