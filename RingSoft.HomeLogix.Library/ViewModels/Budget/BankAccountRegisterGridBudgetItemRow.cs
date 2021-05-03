using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BankAccountRegisterGridBudgetItemRow : BankAccountRegisterGridRow
    {
        public override BankAccountRegisterItemTypes LineType => BankAccountRegisterItemTypes.BudgetItem;
        
        public BankAccountRegisterGridBudgetItemRow(BankAccountRegisterGridManager manager) : base(manager)
        {
            
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (BankAccountRegisterGridColumns)columnId;
            switch (column)
            {
                case BankAccountRegisterGridColumns.Description:
                    return new BudgetItemCellProps(this, columnId, Description);
            }

            return base.GetCellProps(columnId);
        }
    }
}
