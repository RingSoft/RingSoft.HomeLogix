using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BankAccountRegisterGridBudgetItemRow : BankAccountRegisterGridRow
    {
        public override BankAccountRegisterItemTypes LineType => BankAccountRegisterItemTypes.BudgetItem;
        
        public BankAccountRegisterGridBudgetItemRow(BankAccountRegisterGridManager manager) : base(manager)
        {
            
        }
    }
}
