using System;
using System.Collections.Generic;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Tests
{
    public class TestBankAccountView : TestDbMaintenanceView, IBankAccountView
    {
        public void EnableRegisterGrid(bool value)
        {
            
        }

        public DateTime? GetGenerateToDate(DateTime nextGenerateToDate)
        {
            return nextGenerateToDate;
        }

        public void ShowActualAmountDetailsWindow(ActualAmountCellProps actualAmountCellProps)
        {
            
        }

        public bool ShowBankAccountMiscWindow(BankAccountRegisterItem registerItem, ViewModelInput viewModelInput)
        {
            return true;
        }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public object OwnerWindow { get; }
        public bool ImportFromBank(BankAccountViewModel bankAccountViewModel)
        {
            throw new NotImplementedException();
        }

        public void LoadBank(BankAccount entity)
        {
            throw new NotImplementedException();
        }

        public void GenerateTransactions(DateTime generateToDate)
        {
            throw new NotImplementedException();
        }

        public void PostRegister(CompletedRegisterData completedRegisterData, List<BankAccountRegisterGridRow> completedRows)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatus(string status)
        {
            throw new NotImplementedException();throw new NotImplementedException();
        }

        public void ShowMessageBox(string message, string caption, RsMessageBoxIcons icon)
        {
            throw new NotImplementedException();
        }

        public void SetInitGridFocus(BankAccountRegisterGridRow row, int columnId)
        {
            throw new NotImplementedException();
        }

        public TestBankAccountView(string ownerName) : base(ownerName)
        {
        }
    }
}
