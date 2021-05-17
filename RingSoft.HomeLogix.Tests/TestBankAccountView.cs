using System;
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

        public TestBankAccountView(string ownerName) : base(ownerName)
        {
        }
    }
}
