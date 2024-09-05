using System;
using System.Collections.Generic;
using System.Text;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BankAccountRegisterGridMiscRow : BankAccountRegisterGridRow
    {
        public override BankAccountRegisterItemTypes LineType => BankAccountRegisterItemTypes.Miscellaneous;

        public BudgetItemTypes ItemType { get; set; }

        public string TransferRegisterGuid { get; set; }

        public override string Description => _description;

        private string _description;

        public BankAccountRegisterGridMiscRow(BankAccountRegisterGridManager manager) : base(manager)
        {
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (BankAccountRegisterGridColumns) columnId;
            switch (column)
            {
                case BankAccountRegisterGridColumns.ItemType:
                    if (!TransferRegisterGuid.IsNullOrEmpty())
                        return new DataEntryGridCustomControlCellProps(this, columnId,
                            (int) BankAccountRegisterItemTypes.TransferToBankAccount);
                    break;
                case BankAccountRegisterGridColumns.Description:
                    return new MiscCellProps(this, columnId, _description);
            }
            return base.GetCellProps(columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (BankAccountRegisterGridColumns)columnId;
            switch (column)
            {
                case BankAccountRegisterGridColumns.Amount:
                case BankAccountRegisterGridColumns.ActualAmount:
                {
                    if (!TransferRegisterGuid.IsNullOrEmpty())
                    {
                        return new DataEntryGridCellStyle { State = DataEntryGridCellStates.Disabled };
                    }
                    break;
                }
            }
            return base.GetCellStyle(columnId);
        }

        public override void LoadFromEntity(BankAccountRegisterItem entity)
        {
            _description = entity.Description;
            TransferRegisterGuid = entity.TransferRegisterGuid;
            base.LoadFromEntity(entity);
        }

        public override void SaveToEntity(BankAccountRegisterItem entity, int rowIndex)
        {
            entity.TransferRegisterGuid = TransferRegisterGuid;
            entity.BudgetItem = AppGlobals.DataRepository.GetBudgetItem(BudgetItemId);
            base.SaveToEntity(entity, rowIndex);
        }
    }
}
