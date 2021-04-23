using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BankAccountRegisterGridEscrowRow : BankAccountRegisterGridTransferRow
    {
        public override BankAccountRegisterItemTypes LineType => BankAccountRegisterItemTypes.MonthlyEscrow;
        public override string Description => EscrowDescription;

        public string EscrowDescription { get; set; }

        public BankAccountRegisterGridEscrowRow(BankAccountRegisterGridManager manager) : base(manager)
        {
        }

        public override void LoadFromEntity(BankAccountRegisterItem entity)
        {
            EscrowDescription = entity.Description;
            base.LoadFromEntity(entity);
        }

        public override void SaveToEntity(BankAccountRegisterItem entity, int rowIndex)
        {
            entity.Description = EscrowDescription;
            base.SaveToEntity(entity, rowIndex);
        }
    }
}
