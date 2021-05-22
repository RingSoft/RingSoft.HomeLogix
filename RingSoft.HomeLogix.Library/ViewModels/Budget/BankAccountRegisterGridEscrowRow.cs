using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BankAccountRegisterGridEscrowRow : BankAccountRegisterGridTransferRow
    {
        public override BankAccountRegisterItemTypes LineType => BankAccountRegisterItemTypes.MonthlyEscrow;
        public override string Description => EscrowDescription;
        public bool IsEscrowFrom { get; private set; }

        public string EscrowDescription { get; set; }

        private bool _differentEscrowBank;

        public override bool AffectsEscrow
        {
            get
            {
                if (!Manager.ViewModel.EscrowBankAccountAutoFillValue.IsValid())
                {
                    return !_differentEscrowBank;
                }

                return base.AffectsEscrow;
            }
        }

        public BankAccountRegisterGridEscrowRow(BankAccountRegisterGridManager manager) : base(manager)
        {
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (BankAccountRegisterGridColumns) columnId;
            switch (column)
            {
                case BankAccountRegisterGridColumns.Completed:
                case BankAccountRegisterGridColumns.ItemType:
                case BankAccountRegisterGridColumns.TransactionType:
                case BankAccountRegisterGridColumns.Balance:
                    break;
                default:
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
                    if (value is DataEntryGridCheckBoxCellProps checkBoxCellProps)
                    {
                        //Mark Complete
                        if (checkBoxCellProps.Value)
                        {
                            if (IsEscrowFrom)
                            {
                                switch (TransactionType)
                                {
                                    case TransactionTypes.Deposit:
                                        //Escrow To bank account.  No change in bank Escrow balance.  Change budget escrow balance.
                                        break;
                                    case TransactionTypes.Withdrawal:
                                        //Escrow From bank account.  Reduce bank escrow balance.
                                        Manager.ViewModel.EscrowBalance -= ProjectedAmount;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                            else
                            {
                                switch (TransactionType)
                                {
                                    case TransactionTypes.Deposit:
                                        //Escrow from bank account.  Increase bank escrow balance.
                                        Manager.ViewModel.EscrowBalance += ProjectedAmount;
                                        break;
                                    case TransactionTypes.Withdrawal:
                                        //Escrow To bank account.  No change in bank Escrow balance.  Change budget escrow balance.
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                        }
                        else
                        {
                            //Mark Uncomplete
                            if (IsEscrowFrom)
                            {
                                switch (TransactionType)
                                {
                                    case TransactionTypes.Deposit:
                                        //Escrow To bank account.  No change in bank Escrow balance.
                                        break;
                                    case TransactionTypes.Withdrawal:
                                        //Escrow from bank account.  Increase bank escrow balance.
                                        Manager.ViewModel.EscrowBalance += ProjectedAmount;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                            else
                            {
                                switch (TransactionType)
                                {
                                    case TransactionTypes.Deposit:
                                        //Escrow From bank account.  Reduce bank escrow balance.
                                        Manager.ViewModel.EscrowBalance -= ProjectedAmount;
                                        break;
                                    case TransactionTypes.Withdrawal:
                                        //Escrow To bank account.  No change in bank Escrow balance.
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }

                            ActualAmount = null;
                        }
                    }
                    break;
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(BankAccountRegisterItem entity)
        {
            EscrowDescription = entity.Description;
            _differentEscrowBank = !entity.TransferRegisterGuid.IsNullOrEmpty();
            IsEscrowFrom = entity.IsEscrowFrom;
            base.LoadFromEntity(entity);
        }

        public override void SaveToEntity(BankAccountRegisterItem entity, int rowIndex)
        {
            entity.Description = EscrowDescription;
            entity.IsEscrowFrom = IsEscrowFrom;
            base.SaveToEntity(entity, rowIndex);
        }
    }
}
