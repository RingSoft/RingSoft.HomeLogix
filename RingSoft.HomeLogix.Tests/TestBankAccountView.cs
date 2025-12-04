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
            
        }

        public void GenerateTransactions(DateTime generateToDate)
        {
            throw new NotImplementedException();
        }

        public void PostRegister(CompletedRegisterData completedRegisterData, List<BankAccountRegisterGridRow> completedRows)
        {
            
        }

        public void UpdateStatus(string status)
        {
            
        }

        public void ShowMessageBox(string message, string caption, RsMessageBoxIcons icon)
        {
            
        }

        public void SetInitGridFocus(BankAccountRegisterGridRow row, int columnId)
        {
            
        }

        public void RestartApp()
        {
            
        }

        public void RefreshGrid(BankAccount bankAccount)
        {
            
        }

        public void ToggleCompleteAll(bool completeAll)
        {
            
        }

        public void ShowBankOptionsWindow(BankOptionsData bankOptionsData)
        {
            
        }

        public void SetBankOptionsButtonCaption(string caption)
        {
            
        }
    }
}
