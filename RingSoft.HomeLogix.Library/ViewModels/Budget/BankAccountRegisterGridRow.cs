using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.MobileInterop.PhoneModel;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public abstract class BankAccountRegisterGridRow : DbMaintenanceDataEntryGridRow<BankAccountRegisterItem>
    {
        public abstract DataAccess.Model.BankAccountRegisterItemTypes LineType { get; }

        public new BankAccountRegisterGridManager Manager { get; }
            
        public int RegisterId { get; set; }

        public string RegisterGuid { get; private set; }

        public DateTime ItemDate { get; internal set; }

        public int? BudgetItemId { get; private set; }
        
        public AutoFillValue BudgetItemValue { get; private set; }

        public BudgetItem BudgetItem { get; private set; }

        public virtual string Description => BudgetItemValue?.Text;

        public TransactionTypes TransactionType { get; private set; }

        public double ProjectedAmount { get; set; }

        public bool Completed { get; set; }

        public double? Balance { get; set; }

        public double? ActualAmount { get; set; }

        public double? Difference
        {
            get
            {
                switch (TransactionType)
                {
                    case TransactionTypes.Deposit:
                        return ActualAmount - ProjectedAmount;
                    case TransactionTypes.Withdrawal:
                        return ProjectedAmount - ActualAmount;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool IsNegative { get; set; }

        public bool PayCCAllowEdit { get; set; }

        public List<BankAccountRegisterItemAmountDetail> ActualAmountDetails { get; private set; }

        public string BankText { get; set; }

        public RegisterPayCCTypes RegisterPayCCType { get; set; }

        private DecimalEditControlSetup _decimalValueSetup;
        private DateEditControlSetup _dateEditControlSetup;

        protected BankAccountRegisterGridRow(BankAccountRegisterGridManager manager) : base(manager)
        {
            Manager = manager;
            _decimalValueSetup = new DecimalEditControlSetup
            {
                AllowNullValue = true,
                DataEntryMode = DataEntryModes.FormatOnEntry,
                MaximumValue = null,
                MinimumValue = 0,
                NumberFormatString = null,
                FormatType = DecimalEditFormatTypes.Currency,
                Precision = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits
            };

            _dateEditControlSetup = new DateEditControlSetup
            {
                AllowNullValue = false,
                DateFormatType = DateFormatTypes.DateOnly,
            };

            ActualAmountDetails = new List<BankAccountRegisterItemAmountDetail>();
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (BankAccountRegisterGridColumns) columnId;
            switch (column)
            {
                case BankAccountRegisterGridColumns.ItemType:
                    return new DataEntryGridCustomControlCellProps(this, columnId, (int) LineType);
                case BankAccountRegisterGridColumns.Date:
                    return new DataEntryGridDateCellProps(this, columnId, _dateEditControlSetup, ItemDate);
                case BankAccountRegisterGridColumns.Description:
                    return new BudgetItemCellProps(this, columnId, Description);
                case BankAccountRegisterGridColumns.TransactionType:
                    return new DataEntryGridCustomControlCellProps(this, columnId, (int) TransactionType);
                case BankAccountRegisterGridColumns.Amount:
                    return new DataEntryGridDecimalCellProps(this, columnId, _decimalValueSetup, ProjectedAmount);
                case BankAccountRegisterGridColumns.Completed:
                    return new DataEntryGridCheckBoxCellProps(this, columnId, Completed);
                case BankAccountRegisterGridColumns.Balance:
                    return new DataEntryGridDecimalCellProps(this, columnId, _decimalValueSetup, Balance);
                case BankAccountRegisterGridColumns.ActualAmount:
                    return new ActualAmountCellProps(this, columnId, _decimalValueSetup, ActualAmount);
                case BankAccountRegisterGridColumns.Difference:
                    return new DataEntryGridDecimalCellProps(this, columnId, _decimalValueSetup, Difference);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (BankAccountRegisterGridColumns) columnId;

            switch (column)
            {
                case BankAccountRegisterGridColumns.ItemType:
                case BankAccountRegisterGridColumns.TransactionType:
                    return new DataEntryGridControlCellStyle {State = DataEntryGridCellStates.ReadOnly};
                case BankAccountRegisterGridColumns.Date:
                    return new DataEntryGridCellStyle {State = DataEntryGridCellStates.ReadOnly};
                case BankAccountRegisterGridColumns.Description:
                case BankAccountRegisterGridColumns.Amount:
                case BankAccountRegisterGridColumns.ActualAmount:
                    break;
                case BankAccountRegisterGridColumns.Balance:
                    var style = new DataEntryGridCellStyle {State = DataEntryGridCellStates.ReadOnly };
                    if (Balance < 0)
                        style.DisplayStyleId = BankAccountRegisterGridManager.NegativeDisplayId;
                    return style;
                case BankAccountRegisterGridColumns.Difference:
                    style = new DataEntryGridCellStyle { State = DataEntryGridCellStates.ReadOnly };
                    if (Difference < 0)
                        style.DisplayStyleId = BankAccountRegisterGridManager.NegativeDisplayId;
                    if (Difference > 0)
                        style.DisplayStyleId = BankAccountRegisterGridManager.PositiveDisplayId;

                    return style;
                case BankAccountRegisterGridColumns.Completed:
                    return new DataEntryGridControlCellStyle();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (BankAccountRegisterGridColumns) value.ColumnId;
            switch (column)
            {
                case BankAccountRegisterGridColumns.ItemType:
                    break;
                case BankAccountRegisterGridColumns.Date:
                    break;
                case BankAccountRegisterGridColumns.Description:
                    break;
                case BankAccountRegisterGridColumns.TransactionType:
                    break;
                case BankAccountRegisterGridColumns.Amount:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps)
                    {
                        ProjectedAmount = (double)decimalCellProps.Value.GetValueOrDefault(0);
                        Manager.ViewModel.CalculateTotals();
                        SaveToDbOnTheFly();
                    }
                    break;
                case BankAccountRegisterGridColumns.Completed:
                    if (value is DataEntryGridCheckBoxCellProps checkBoxCellProps)
                    {
                        SetComplete(checkBoxCellProps.Value);

                        Manager.ViewModel.CalculateTotals();

                        SaveToDbOnTheFly();
                    }
                    break;
                case BankAccountRegisterGridColumns.Balance:
                    break;
                case BankAccountRegisterGridColumns.ActualAmount:
                    var actualAmountCellProps = (DataEntryGridDecimalCellProps) value;
                    ActualAmount = actualAmountCellProps.Value;
                    Manager.ViewModel.CalculateTotals();
                    SaveToDbOnTheFly();
                    break;
                case BankAccountRegisterGridColumns.Difference:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.SetCellValue(value);
        }

        public void SetComplete(bool value)
        {
            Completed = value;
            if (Completed && ActualAmount == null)
            {
                ActualAmount = ProjectedAmount;
            }
        }

        public void SaveToDbOnTheFly(bool updateOnly = true)
        {
            if (updateOnly)
            {
                var context = SystemGlobals.DataRepository.GetDataContext();
                if (context != null)
                {
                    var table = context.GetTable<BankAccountRegisterItem>();
                    if (table != null)
                    {
                        if (!table.Any(p => p.RegisterGuid == RegisterGuid))
                        {
                            return;
                        }
                    }
                }
            }
            var registerItem = new BankAccountRegisterItem();
            SaveToEntity(registerItem, 0);
            AppGlobals.DataRepository.SaveRegisterItem(registerItem);
        }

        public override void LoadFromEntity(BankAccountRegisterItem entity)
        {
            //var count = entity.BankAccount.RegisterItems.Count;
            //var rowIndex = entity.BankAccount.RegisterItems.ToList().IndexOf(entity) + 1;
            //Manager.ViewModel.BankAccountView.UpdateStatus($"Loading Grid Row {rowIndex} of {count}");
            RegisterId = entity.Id;
            RegisterGuid = entity.RegisterGuid;
            ItemDate = entity.ItemDate;
            RegisterPayCCType = (RegisterPayCCTypes)entity.PayCCType;

            BudgetItemId = entity.BudgetItemId;  //Must default to null or completed Escrow rows won't save.
            BudgetItemValue = entity.BudgetItem.GetAutoFillValue();
            BudgetItem = entity.BudgetItem;

            BankText = entity.BankText;

            if (entity.TransferRegisterGuid.IsNullOrEmpty())
            {
                if (entity.BudgetItem != null)
                {
                    if ((DataAccess.Model.BankAccountRegisterItemTypes) entity.ItemType ==
                        DataAccess.Model.BankAccountRegisterItemTypes.Miscellaneous)
                    {
                        IsNegative = entity.IsNegative;
                        switch ((BudgetItemTypes)entity.BudgetItem.Type)
                        {
                            case BudgetItemTypes.Income:
                                switch (Manager.ViewModel.AccountType)
                                {
                                    case BankAccountTypes.Checking:
                                    case BankAccountTypes.Savings:
                                        TransactionType = entity.IsNegative
                                            ? TransactionTypes.Withdrawal
                                            : TransactionTypes.Deposit;
                                        break;
                                    case BankAccountTypes.CreditCard:
                                        TransactionType = entity.IsNegative
                                            ? TransactionTypes.Deposit
                                            : TransactionTypes.Withdrawal;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                break;
                            case BudgetItemTypes.Expense:
                                TransactionType = entity.IsNegative
                                    ? TransactionTypes.Deposit
                                    : TransactionTypes.Withdrawal;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        switch ((BudgetItemTypes)entity.BudgetItem.Type)
                        {
                            case BudgetItemTypes.Income:
                                TransactionType = TransactionTypes.Deposit;
                                break;
                            case BudgetItemTypes.Expense:
                                TransactionType = TransactionTypes.Withdrawal;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    ProjectedAmount = Math.Abs(entity.ProjectedAmount);
                }
            }
            else
            {
                TransactionType = entity.ProjectedAmount < 0
                    ? TransactionTypes.Withdrawal
                    : TransactionTypes.Deposit;
                ProjectedAmount = Math.Abs(entity.ProjectedAmount);

                var payCCType = (RegisterPayCCTypes)entity.PayCCType;
                switch (payCCType)
                {
                    case RegisterPayCCTypes.None:
                        break;
                    case RegisterPayCCTypes.FromBank:
                        TransactionType = TransactionTypes.Withdrawal;
                        break;
                    case RegisterPayCCTypes.ToCC:
                        TransactionType = TransactionTypes.Deposit;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (entity.ActualAmount != null) 
            {
                ActualAmount = Math.Abs(entity.ActualAmount.GetValueOrDefault());
            }
            else
            {
                ActualAmount = entity.ActualAmount;
            }
            ActualAmountDetails = entity.AmountDetails.ToList();
            Completed = entity.Completed;
        }

        public override bool ValidateRow()
        {
            return true;
        }

        public override void SaveToEntity(BankAccountRegisterItem entity, int rowIndex)
        {
            entity.Id = RegisterId;
            entity.BankAccountId = Manager.ViewModel.Id;
            entity.RegisterGuid = RegisterGuid;
            entity.ItemType = (int)LineType;
            entity.BudgetItemId = BudgetItemId.GetValueOrDefault();
            entity.ItemDate = ItemDate;
            entity.Description = Description;
            entity.ActualAmount = ActualAmount;
            if (!ActualAmountDetails.Any())
            {
                entity.BankText = BankText;
            }
            
            switch (TransactionType)
            {
                case TransactionTypes.Deposit:
                    entity.ProjectedAmount = ProjectedAmount;
                    break;
                case TransactionTypes.Withdrawal:
                    entity.ProjectedAmount = -ProjectedAmount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            entity.Completed = Completed;
            entity.IsNegative = IsNegative;
        }

        public void SaveToEntity(BankAccountRegisterItem entity, int rowIndex,
            List<BankAccountRegisterItemAmountDetail> actualDetails)
        {
            SaveToEntity(entity, rowIndex);
            if (ActualAmount != null)
            {
                var detailId = 1;
                foreach (var actualAmountDetail in ActualAmountDetails)
                {
                    actualDetails.Add(
                        new BankAccountRegisterItemAmountDetail
                        {
                            RegisterId = RegisterId,
                            DetailId = detailId,
                            Date = actualAmountDetail.Date,
                            SourceId = actualAmountDetail.SourceId,
                            Amount = actualAmountDetail.Amount,
                            BankText = actualAmountDetail.BankText
                        });
                    detailId++;
                }
            }
        }

        internal RegisterData GetRegisterData()
        {
            var registerData = new RegisterData();
            registerData.BankAccountId = Manager.ViewModel.Id;
            registerData.Description = Description;
            registerData.Completed = Completed;
            registerData.ProjectedAmount = ProjectedAmount;
            registerData.ActualAmount = ActualAmount.GetValueOrDefault();
            registerData.ItemDate = ItemDate;
            registerData.IsNegative = IsNegative;
            //registerData.RegisterItemType =
            //    (MobileInterop.PhoneModel.BankAccountRegisterItemTypes)register.ItemType;
            registerData.RegisterItemType = MobileInterop.PhoneModel.BankAccountRegisterItemTypes.BudgetItem;

            registerData.TransactionType = TransactionType.ToRegisterDataTranType();

            switch (RegisterPayCCType)
            {
                case RegisterPayCCTypes.None:
                    registerData.RegisterPayCCType = MobileRegisterPayCCTypes.None;
                    break;
                case RegisterPayCCTypes.FromBank:
                    registerData.RegisterPayCCType = MobileRegisterPayCCTypes.FromBank;
                    break;
                case RegisterPayCCTypes.ToCC:
                    registerData.RegisterPayCCType = MobileRegisterPayCCTypes.ToCC;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            registerData.PayCCDay = BudgetItem.PayCCDay;
            return registerData;
        }

    }
}
