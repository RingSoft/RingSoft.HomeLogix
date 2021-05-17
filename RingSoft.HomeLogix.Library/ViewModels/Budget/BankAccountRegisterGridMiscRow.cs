using System;
using System.Collections.Generic;
using System.Text;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BankAccountRegisterGridMiscRow : BankAccountRegisterGridRow
    {
        public override BankAccountRegisterItemTypes LineType => BankAccountRegisterItemTypes.Miscellaneous;

        public BudgetItemTypes ItemType { get; set; }

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
                case BankAccountRegisterGridColumns.Description:
                    return new MiscCellProps(this, columnId, _description);
            }
            return base.GetCellProps(columnId);
        }

        public override void LoadFromEntity(BankAccountRegisterItem entity)
        {
            _description = entity.Description;
            base.LoadFromEntity(entity);
        }
    }
}
