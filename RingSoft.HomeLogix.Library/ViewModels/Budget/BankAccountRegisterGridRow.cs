using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public abstract class BankAccountRegisterGridRow : DbMaintenanceDataEntryGridRow<BankAccountRegisterItem>
    {
        public abstract BankAccountRegisterItemTypes LineType { get; }

        public BankAccountRegisterGridManager BankAccountRegisterGridManager { get; }

        protected BankAccountRegisterGridRow(BankAccountRegisterGridManager manager) : base(manager)
        {
            BankAccountRegisterGridManager = manager;
        }
    }
}
