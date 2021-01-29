using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Tests
{
    public class TestBankAccountView : TestDbMaintenanceView, IBankAccountView
    {
        public void EnableRegisterGrid(bool value)
        {
            
        }

        public TestBankAccountView(string ownerName) : base(ownerName)
        {
        }
    }
}
