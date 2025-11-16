using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DevLogix.Tests;
using RingSoft.DevLogix.Tests.QualityAssurance;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Tests
{
    [TestClass]
    public class BankMaintenanceTests
    {
        public static TestGlobals<BankAccountViewModel, TestBankAccountView> Globals { get; } =
            new TestGlobals<BankAccountViewModel, TestBankAccountView>();

        static BankMaintenanceTests()
        {
            Globals = new TestGlobals<BankAccountViewModel, TestBankAccountView>();
        }

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            Globals.Initialize();
        }

        [TestMethod]
        public void TestBankAccount_GenerateDetails()
        {
            Globals.ClearData();

            var dataRepository = AppGlobals.DataRepository;

            var bankAccount = dataRepository.GetBankAccount(
                Globals.JaneCheckingBankAccountId);

            var bankAccountViewModel = Globals.ViewModel;

            bankAccountViewModel.OnRecordSelected(bankAccount);
            
            bankAccountViewModel.GenerateTransactions(DateTime.Today.AddMonths(3));
            bankAccountViewModel.SaveCommand.Execute(null);

            bankAccountViewModel.NewCommand.Execute(null);
            bankAccountViewModel.OnRecordSelected(bankAccount);

            var count = bankAccountViewModel.Entity.RegisterItems.Count;
            var amount = bankAccountViewModel.ProjectedLowestBalanceAmount;

            var depRow = bankAccountViewModel
                .RegisterGridManager
                .Rows.OfType<BankAccountRegisterGridRow>()
                .FirstOrDefault(p => p.TransactionType == TransactionTypes.Deposit);
            depRow.SetCellValue(new DataEntryGridCheckBoxCellProps(depRow
                , BankAccountRegisterGridManager.CompletedColumnId, true));

            bankAccountViewModel.SaveCommand.Execute(null);
            bankAccount.UtFillOutEntity();

            Assert.AreEqual(count - 1, bankAccount.RegisterItems.Count);

            Assert.AreEqual(amount - depRow.ProjectedAmount
                , bankAccountViewModel.ProjectedLowestBalanceAmount);

            count = bankAccount.RegisterItems.Count;
            amount = bankAccountViewModel.ProjectedLowestBalanceAmount;

            var withRow = bankAccountViewModel
                .RegisterGridManager
                .Rows.OfType<BankAccountRegisterGridRow>()
                .FirstOrDefault(p => p.TransactionType == TransactionTypes.Withdrawal);
            withRow.SetCellValue(new DataEntryGridCheckBoxCellProps(withRow
                , BankAccountRegisterGridManager.CompletedColumnId
                , true));

            bankAccountViewModel.SaveCommand.Execute(null);

            bankAccount.UtFillOutEntity();

            Assert.AreEqual(count - 1, bankAccount.RegisterItems.Count);

            Assert.AreEqual(amount + withRow.ProjectedAmount
                , bankAccountViewModel.ProjectedLowestBalanceAmount);

        }
    }
}
