using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BankAccountRegisterGridTransferRow : BankAccountRegisterGridRow
    {
        public override BankAccountRegisterItemTypes LineType => BankAccountRegisterItemTypes.TransferToBankAccount;

        public string TransferDescription { get; private set; }

        public string TransferRegisterGuid { get; private set; }

        public bool IsTransferMisc { get; private set; }

        public override string Description
        {
            get
            {
                if (BudgetItemValue.IsValid() || IsTransferMisc)
                    return BudgetItemValue.Text;

                return TransferDescription;
            }
        }

        public BankAccountRegisterGridTransferRow(BankAccountRegisterGridManager manager) : base(manager)
        {
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (BankAccountRegisterGridColumns)columnId;
            switch (column)
            {
                case BankAccountRegisterGridColumns.Description:
                    if (BudgetItemId == null || IsTransferMisc)
                    {
                        return new MiscCellProps(this, columnId, Description);
                    }
                    break;
            }
            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (BankAccountRegisterGridColumns) columnId;
            switch (column)
            {
                case BankAccountRegisterGridColumns.Amount:
                case BankAccountRegisterGridColumns.ActualAmount:
                    return new DataEntryGridCellStyle {State = DataEntryGridCellStates.Disabled};
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (BankAccountRegisterGridColumns) value.ColumnId;
            switch (column)
            {
                case BankAccountRegisterGridColumns.Completed:
                    if (value is DataEntryGridCheckBoxCellProps {Value: false})
                        ActualAmount = null;
                    break;
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(BankAccountRegisterItem entity)
        {
            TransferDescription = entity.Description;
            TransferRegisterGuid = entity.TransferRegisterGuid;
            IsTransferMisc = entity.IsTransferMisc;
            base.LoadFromEntity(entity);
        }

        public override void SaveToEntity(BankAccountRegisterItem entity, int rowIndex)
        {
            entity.TransferRegisterGuid = TransferRegisterGuid;
            entity.Description = TransferDescription;
            base.SaveToEntity(entity, rowIndex);
        }
    }
}
