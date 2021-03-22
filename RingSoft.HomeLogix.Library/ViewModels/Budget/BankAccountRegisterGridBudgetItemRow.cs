using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BankAccountRegisterGridBudgetItemRow : BankAccountRegisterGridRow
    {
        public override BankAccountRegisterItemTypes LineType => BankAccountRegisterItemTypes.BudgetItem;
        public override string Description => BudgetItemValue?.Text;

        public int BudgetItemId { get; private set; }
        public AutoFillValue BudgetItemValue { get; private set; }

        public BankAccountRegisterGridBudgetItemRow(BankAccountRegisterGridManager manager) : base(manager)
        {
            
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (BankAccountRegisterGridColumns)columnId;
            switch (column)
            {
                case BankAccountRegisterGridColumns.Description:
                    return new DataEntryGridTextCellProps(this, columnId, Description);
            }

            return base.GetCellProps(columnId);
        }

        public override void LoadFromEntity(BankAccountRegisterItem entity)
        {
            BudgetItemValue =
                new AutoFillValue(AppGlobals.LookupContext.BudgetItems.GetPrimaryKeyValueFromEntity(entity.BudgetItem),
                    entity.BudgetItem.Description);

            base.LoadFromEntity(entity);
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(BankAccountRegisterItem entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
