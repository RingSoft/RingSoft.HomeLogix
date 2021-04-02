using System;
using System.Collections.Generic;
using System.Text;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BankAccountRegisterGridTransferRow : BankAccountRegisterGridRow
    {
        public override BankAccountRegisterItemTypes LineType => BankAccountRegisterItemTypes.TransferToBankAccount;

        public string TransferDescription { get; private set; }

        public override string Description => TransferDescription;

        public BankAccountRegisterGridTransferRow(BankAccountRegisterGridManager manager) : base(manager)
        {
        }

        public override void LoadFromEntity(BankAccountRegisterItem entity)
        {
            TransferDescription = entity.Description;

            base.LoadFromEntity(entity);
        }

        public override bool ValidateRow()
        {
            throw new NotImplementedException();
        }

        public override void SaveToEntity(BankAccountRegisterItem entity, int rowIndex)
        {
            throw new NotImplementedException();
        }
    }
}
