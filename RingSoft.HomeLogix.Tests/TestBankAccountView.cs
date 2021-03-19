using System;
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

        public TestBankAccountView(string ownerName) : base(ownerName)
        {
        }
    }
}
