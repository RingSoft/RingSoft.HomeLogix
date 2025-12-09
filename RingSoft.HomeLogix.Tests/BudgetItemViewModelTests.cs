using System;
using System.Linq;
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
        public void TestBudget_PayCCSame_DifferentCCAccounts()
        {
            Globals.ClearData();

            var dataRepository = AppGlobals.DataRepository;
            var budgetItem = dataRepository.GetBudgetItem(Globals.CCPaymentBudgetItemId);

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            budgetItemViewModel.OnRecordSelected(budgetItem);

            var newToCCBankAccount = dataRepository.GetBankAccount(Globals.MasterCard_PayCCOffEveryMonth_BankAccountId);

            //In this scenario, we purge main and visa card (old).  Recalculate Master card only.
            budgetItemViewModel.TransferToBankAccountAutoFillValue = newToCCBankAccount.GetAutoFillValue();
            budgetItemViewModel.SaveCommand.Execute(null);

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.CreditCardBankAccounts
                .Any(p => p.Id == Globals.MasterCard_PayCCOffEveryMonth_BankAccountId));

            Assert.AreEqual(1, budgetItemViewModel.CCRecalcData.CreditCardBankAccounts.Count);

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister
                .Any(p => p.Id == Globals.MainCheckingAccount_BankAccountId));

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister
                .Any(p => p.Id == Globals.VisaCard_PayCCOffEveryMonth_BankAccountId));

            Assert.AreEqual(2, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister.Count);
        }

        [TestMethod]
        public void TestBudget_From_DbBank_Diff_NewBank()
        {
            Globals.ClearData();

            var dataRepository = AppGlobals.DataRepository;
            var budgetItem = dataRepository.GetBudgetItem(Globals.CCPaymentBudgetItemId);

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            budgetItemViewModel.OnRecordSelected(budgetItem);

            var newCCFromBankAccount = dataRepository.GetBankAccount(Globals.SecondChecking_BankAccountId);

            //In this scenario, we purge main and second.  Recalculate Visa card only.
            budgetItemViewModel.BankAutoFillValue = newCCFromBankAccount.GetAutoFillValue();
            budgetItemViewModel.SaveCommand.Execute(null);

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.CreditCardBankAccounts
                .Any(p => p.Id == Globals.VisaCard_PayCCOffEveryMonth_BankAccountId));

            Assert.AreEqual(1, budgetItemViewModel.CCRecalcData.CreditCardBankAccounts.Count);

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister
                .Any(p => p.Id == Globals.MainCheckingAccount_BankAccountId));

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister
                .Any(p => p.Id == Globals.SecondChecking_BankAccountId));

            Assert.AreEqual(2, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister.Count);
        }

        [TestMethod]
        public void TestBudget_Swap_CCPayOff_CCCarryBalance()
        {
            Globals.ClearData();

            //NewToCC_CarryBalance_OldToCC_PayOff-----------------------------------------------
            var dataRepository = AppGlobals.DataRepository;
            var budgetItem = dataRepository.GetBudgetItem(Globals.CCPaymentBudgetItemId);

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            budgetItemViewModel.OnRecordSelected(budgetItem);

            var newToCCBankAccount = dataRepository.GetBankAccount(Globals.DiscoverCard_CarryBalance_BankAccountId);

            //In this scenario, we purge main and visa card (old).  No recalc needed
            budgetItemViewModel.TransferToBankAccountAutoFillValue = newToCCBankAccount.GetAutoFillValue();
            budgetItemViewModel.SaveCommand.Execute(null);

            Assert.AreEqual(0, budgetItemViewModel.CCRecalcData.CreditCardBankAccounts.Count);

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister
                .Any(p => p.Id == Globals.MainCheckingAccount_BankAccountId));

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister
                .Any(p => p.Id == Globals.VisaCard_PayCCOffEveryMonth_BankAccountId));

            Assert.AreEqual(2, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister.Count);

            //NewToCC_PayOff_OldToCC_CarryBalance-------------------------------------------------

            newToCCBankAccount = dataRepository.GetBankAccount(Globals.VisaCard_PayCCOffEveryMonth_BankAccountId);

            //In this scenario, we recalc visa card (new).  No purge needed
            budgetItemViewModel.TransferToBankAccountAutoFillValue = newToCCBankAccount.GetAutoFillValue();
            budgetItemViewModel.SaveCommand.Execute(null);

            Assert.AreEqual(1, budgetItemViewModel.CCRecalcData.CreditCardBankAccounts.Count);

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.CreditCardBankAccounts
                .Any(p => p.Id == Globals.VisaCard_PayCCOffEveryMonth_BankAccountId));

            Assert.AreEqual(0, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister.Count);
        }

        [TestMethod]
        public void TestBudget_Swap_CCPayOff_NonCC()
        {
            Globals.ClearData();

            //NewTransferTo_NonCC_OldTransferTo_CCPayOff-------------------------------------
            var dataRepository = AppGlobals.DataRepository;
            var budgetItem = dataRepository.GetBudgetItem(Globals.CCPaymentBudgetItemId);

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            budgetItemViewModel.OnRecordSelected(budgetItem);

            var newTransferToBankAccount = dataRepository.GetBankAccount(Globals.SecondChecking_BankAccountId);

            //In this scenario, we purge main and visa card (old).  No recalc needed
            budgetItemViewModel.TransferToBankAccountAutoFillValue = newTransferToBankAccount.GetAutoFillValue();
            budgetItemViewModel.SaveCommand.Execute(null);

            Assert.AreEqual(0, budgetItemViewModel.CCRecalcData.CreditCardBankAccounts.Count);

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister
                .Any(p => p.Id == Globals.MainCheckingAccount_BankAccountId));

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister
                .Any(p => p.Id == Globals.VisaCard_PayCCOffEveryMonth_BankAccountId));

            Assert.AreEqual(2, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister.Count);

            //NewTransferToCC_PayOff_OldTransferTo_NonCC-------------------------------------------------

            var newToCCBankAccount = dataRepository.GetBankAccount(Globals.VisaCard_PayCCOffEveryMonth_BankAccountId);

            //In this scenario, we recalc visa card (new).  No purge needed
            budgetItemViewModel.TransferToBankAccountAutoFillValue = newToCCBankAccount.GetAutoFillValue();
            budgetItemViewModel.SaveCommand.Execute(null);

            Assert.AreEqual(1, budgetItemViewModel.CCRecalcData.CreditCardBankAccounts.Count);

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.CreditCardBankAccounts
                .Any(p => p.Id == Globals.VisaCard_PayCCOffEveryMonth_BankAccountId));

            Assert.AreEqual(0, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister.Count);
        }

        [TestMethod]
        public void TestBudget_ChangeFromBankAccount_ChangeToCCType()
        {
            Globals.ClearData();

            var dataRepository = AppGlobals.DataRepository;
            var budgetItem = dataRepository.GetBudgetItem(Globals.CCPaymentBudgetItemId);

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            budgetItemViewModel.OnRecordSelected(budgetItem);

            var newTransferFromBankAccount = dataRepository.GetBankAccount(Globals.SecondChecking_BankAccountId);
            var newTransferToBankAccount = dataRepository.GetBankAccount(Globals.MasterCard_PayCCOffEveryMonth_BankAccountId);

            //In this scenario, we recakc new ToPayOffCC (master card) only.  We purge Old ToPayOffCC (visa) and both old and new To BankAccounts (Main and Second)
            budgetItemViewModel.BankAutoFillValue = newTransferFromBankAccount.GetAutoFillValue();
            budgetItemViewModel.TransferToBankAccountAutoFillValue = newTransferToBankAccount.GetAutoFillValue();
            budgetItemViewModel.SaveCommand.Execute(null);

            Assert.AreEqual(1, budgetItemViewModel.CCRecalcData.CreditCardBankAccounts.Count);

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.CreditCardBankAccounts
                .Any(p => p.Id == Globals.MasterCard_PayCCOffEveryMonth_BankAccountId));

            Assert.AreEqual(3, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister.Count);

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister
                .Any(p => p.Id == Globals.MainCheckingAccount_BankAccountId));

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister
                .Any(p => p.Id == Globals.SecondChecking_BankAccountId));

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister
                .Any(p => p.Id == Globals.VisaCard_PayCCOffEveryMonth_BankAccountId));
        }

        [TestMethod]
        public void TestBudget_FromBankCCType_ChangeToCCType()
        {
            Globals.ClearData();

            var dataRepository = AppGlobals.DataRepository;
            var budgetItem = dataRepository.GetBudgetItem(Globals.CCPaymentBudgetItemId);

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            budgetItemViewModel.OnRecordSelected(budgetItem);

            var newTransferFromBankAccount = dataRepository.GetBankAccount(Globals.VisaCard_PayCCOffEveryMonth_BankAccountId);
            var newTransferToBankAccount =
                dataRepository.GetBankAccount(Globals.MasterCard_PayCCOffEveryMonth_BankAccountId);

            //In this scenario, we do no recalc.  We purge Old ToPayOffCC (visa) and both old and new To BankAccounts (Main, Visa, and Master Card)
            budgetItemViewModel.BankAutoFillValue = newTransferFromBankAccount.GetAutoFillValue();
            budgetItemViewModel.TransferToBankAccountAutoFillValue = newTransferToBankAccount.GetAutoFillValue();
            budgetItemViewModel.SaveCommand.Execute(null);

            Assert.AreEqual(0, budgetItemViewModel.CCRecalcData.CreditCardBankAccounts.Count);

            Assert.AreEqual(3, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister.Count);

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister
                .Any(p => p.Id == Globals.MainCheckingAccount_BankAccountId));

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister
                .Any(p => p.Id == Globals.MasterCard_PayCCOffEveryMonth_BankAccountId));

            Assert.AreEqual(true, budgetItemViewModel.CCRecalcData.BanksToPurgeRegister
                .Any(p => p.Id == Globals.VisaCard_PayCCOffEveryMonth_BankAccountId));

        }

        [TestMethod]
        public void TestBudget_FromBankType_NoChangeToBankType()
        {
            Globals.ClearData();

            var dataRepository = AppGlobals.DataRepository;
            var budgetItem = dataRepository.GetBudgetItem(Globals.CCPaymentBudgetItemId);

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            budgetItemViewModel.OnRecordSelected(budgetItem);

            //In this scenario, we do nothing.
            budgetItemViewModel.SaveCommand.Execute(null);

            Assert.IsFalse(budgetItemViewModel.CCRecalcData.HasData);
        }

        [TestMethod]
        public void TestBudget_FromBankType_NoChangeToBankType_Transfer()
        {
            Globals.ClearData();

            var dataRepository = AppGlobals.DataRepository;
            var budgetItem = dataRepository.GetBudgetItem(Globals.TransferBudgetItemId);

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            budgetItemViewModel.OnRecordSelected(budgetItem);

            //In this scenario, we do nothing.
            budgetItemViewModel.SaveCommand.Execute(null);

            Assert.IsFalse(budgetItemViewModel.CCRecalcData.HasData);
        }

        [TestMethod]
        public void TestBudget_FromBankType_NoChangeToBankType_TransferCCToCC()
        {
            Globals.ClearData();

            var dataRepository = AppGlobals.DataRepository;
            var budgetItem = dataRepository.GetBudgetItem(Globals.TransferFromToCCBankAccountBudgetItemId);

            var budgetItemViewModel = new BudgetItemViewModel();
            budgetItemViewModel.Processor = Globals;
            budgetItemViewModel.OnViewLoaded(
                new BudgetView());

            budgetItemViewModel.OnRecordSelected(budgetItem);

            //In this scenario, we do nothing.
            budgetItemViewModel.SaveCommand.Execute(null);

            Assert.IsNull(budgetItemViewModel.CCRecalcData);
        }
    }
}
    
