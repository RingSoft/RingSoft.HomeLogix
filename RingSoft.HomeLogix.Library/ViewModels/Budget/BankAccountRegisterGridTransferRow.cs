using RingSoft.DbLookup;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BankAccountRegisterGridTransferRow : BankAccountRegisterGridBudgetItemRow
    {
        public override BankAccountRegisterItemTypes LineType => BankAccountRegisterItemTypes.TransferToBankAccount;

        public string TransferDescription { get; private set; }

        public string TransferRegisterGuid { get; private set; }

        public override string Description
        {
            get
            {
                if (BudgetItemValue.IsValid())
                    return BudgetItemValue.Text;

                return TransferDescription;
            }
        }

        public BankAccountRegisterGridTransferRow(BankAccountRegisterGridManager manager) : base(manager)
        {
        }

        public override void LoadFromEntity(BankAccountRegisterItem entity)
        {
            TransferDescription = entity.Description;
            TransferRegisterGuid = entity.TransferRegisterGuid;
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
