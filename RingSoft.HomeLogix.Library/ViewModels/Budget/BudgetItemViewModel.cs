using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public enum RecurringViewTypes
    {
        Escrow = 0,
        DayOrWeek = 1,
        MonthlySpendingMonthly = 2,
        MonthlySpendingWeekly = 3,
        Income = 4,
        Transfer = 5
    }

    public interface IBudgetItemView : IDbMaintenanceView
    {
        void SetViewType();
    }

    public class BudgetItemViewModel : AppDbMaintenanceViewModel<BudgetItem>
    {
        public override TableDefinition<BudgetItem> TableDefinition => AppGlobals.LookupContext.BudgetItems;

        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                if (_id == value)
                    return;

                _id = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxControlSetup _budgetItemTypeComboBoxControlSetup;

        public TextComboBoxControlSetup BudgetItemTypeComboBoxControlSetup
        {
            get => _budgetItemTypeComboBoxControlSetup;
            set
            {
                if (_budgetItemTypeComboBoxControlSetup == value)
                    return;

                _budgetItemTypeComboBoxControlSetup = value;
                OnPropertyChanged();
            }
        }


        private TextComboBoxItem _budgetItemTypeComboBoxItem;

        public TextComboBoxItem BudgetItemTypeComboBoxItem
        {
            get => _budgetItemTypeComboBoxItem;
            set
            {
                if (_budgetItemTypeComboBoxItem == value)
                    return;

                _budgetItemTypeComboBoxItem = value;
                SetViewMode();
                OnPropertyChanged();
            }
        }

        public BudgetItemTypes BudgetItemType
        {
            get => (BudgetItemTypes)BudgetItemTypeComboBoxItem.NumericValue;
            set => BudgetItemTypeComboBoxItem = BudgetItemTypeComboBoxControlSetup.GetItem((int) value);
        }


        private AutoFillSetup _bankAutoFillSetup;

        public AutoFillSetup BankAutoFillSetup
        {
            get => _bankAutoFillSetup;
            set
            {
                if (_bankAutoFillSetup == value)
                    return;

                _bankAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _bankAutoFillValue;

        public AutoFillValue BankAutoFillValue
        {
            get => _bankAutoFillValue;
            set
            {
                if (_bankAutoFillValue == value)
                    return;

                _bankAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private decimal _amount;

        public decimal Amount
        {
            get => _amount;
            set
            {
                if (_amount == value)
                    return;

                _amount = value;
                OnPropertyChanged();
            }
        }

        private int _recurringPeriod;

        public int RecurringPeriod
        {
            get => _recurringPeriod;
            set
            {
                if (_recurringPeriod == value)
                    return;

                _recurringPeriod = value;
                SetViewMode();
                OnPropertyChanged();
            }
        }

        private TextComboBoxControlSetup _recurringTypeComboBoxControlSetup;

        public TextComboBoxControlSetup RecurringTypeComboBoxControlSetup
        {
            get => _recurringTypeComboBoxControlSetup;
            set
            {
                if (_recurringTypeComboBoxControlSetup == value)
                    return;

                _recurringTypeComboBoxControlSetup = value;
                OnPropertyChanged();
            }
        }


        private TextComboBoxItem _recurringTypeComboBoxItem;

        public TextComboBoxItem RecurringTypeComboBoxItem
        {
            get => _recurringTypeComboBoxItem;
            set
            {
                if (_recurringTypeComboBoxItem == value)
                    return;

                _recurringTypeComboBoxItem = value;
                SetViewMode();
                OnPropertyChanged();
            }
        }

        public BudgetItemRecurringTypes RecurringType
        {
            get => (BudgetItemRecurringTypes)RecurringTypeComboBoxItem.NumericValue;
            set => RecurringTypeComboBoxItem = RecurringTypeComboBoxControlSetup.GetItem((int)value);
        }

        private DateTime _startingDate;

        public DateTime StartingDate
        {
            get => _startingDate;
            set
            {
                if (_startingDate == value)
                    return;

                _startingDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endingDate;

        public DateTime? EndingDate
        {
            get => _endingDate;
            set
            {
                if (_endingDate == value)
                    return;

                _endingDate = value;
                OnPropertyChanged();
            }
        }

        private bool _doEscrow;

        public bool DoEscrow
        {
            get => _doEscrow;
            set
            {
                if (_doEscrow == value)
                    return;

                _doEscrow = value;
                SetViewMode();
                OnPropertyChanged();
            }
        }

        private AutoFillValue _transferToBankAccountAutoFillValue;

        public AutoFillValue TransferToBankAccountAutoFillValue
        {
            get => _transferToBankAccountAutoFillValue;
            set
            {
                if (_transferToBankAccountAutoFillValue == value)
                    return;

                _transferToBankAccountAutoFillValue = value;
                OnPropertyChanged();
            }
        }


        private decimal? _monthlyAmount;

        public decimal? MonthlyAmount
        {
            get => _monthlyAmount;
            set
            {
                if (_monthlyAmount == value)
                    return;

                _monthlyAmount = value;
                OnPropertyChanged();
            }
        }

        public bool MonthlyAmountVisible { get; set; }
        public bool TransferToVisible { get; set; }
        public bool EscrowVisible { get; set; }
        public bool TransferToBankVisible { get; set; }

        private IBudgetItemView _view;
        private bool _loading;

        protected override void Initialize()
        {
            if (View is IBudgetItemView budgetExpenseView)
                _view = budgetExpenseView;

            _loading = true;

            BudgetItemTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            BudgetItemTypeComboBoxControlSetup.LoadFromEnum<BudgetItemTypes>();

            RecurringTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            RecurringTypeComboBoxControlSetup.LoadFromEnum<BudgetItemRecurringTypes>();

            BankAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BankAccountsLookup);

            _loading = false;

            base.Initialize();
        }

        private void SetViewMode()
        {
            if (_loading)
                return;

            MonthlyAmountVisible = TransferToVisible = EscrowVisible = TransferToBankVisible = false;

            switch (BudgetItemType)
            {
                case BudgetItemTypes.Income:
                    SetEscrow(false);
                    break;
                case BudgetItemTypes.Expense:
                    switch (RecurringType)
                    {
                        case BudgetItemRecurringTypes.Days:
                        case BudgetItemRecurringTypes.Weeks:
                            SetEscrow(false);
                            break;
                        case BudgetItemRecurringTypes.Months:
                            if (RecurringPeriod > 1)
                            {
                                SetEscrow(true);
                            }
                            else
                            {
                                SetEscrow(false);
                            }
                            break;
                        case BudgetItemRecurringTypes.Years:
                            SetEscrow(true);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case BudgetItemTypes.Transfer:
                    SetEscrow(false);
                    TransferToVisible = TransferToBankVisible = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (BudgetItemType != BudgetItemTypes.Transfer)
                CalculateMonthlyAmount();

            _view.SetViewType();
        }

        private void CalculateMonthlyAmount()
        {
            var monthlyAmountVisible = true;
            switch (RecurringType)
            {
                case BudgetItemRecurringTypes.Days:
                    break;
                case BudgetItemRecurringTypes.Weeks:
                    break;
                case BudgetItemRecurringTypes.Months:
                    if (RecurringPeriod > 1)
                        CalculateEscrow();
                    else
                    {
                        monthlyAmountVisible = false;
                    }
                    break;
                case BudgetItemRecurringTypes.Years:
                    CalculateEscrow();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            MonthlyAmountVisible = monthlyAmountVisible;
        }

        private void CalculateEscrow()
        {
            if (DoEscrow || BudgetItemType != BudgetItemTypes.Expense)
            {
                //Calculate
            }
        }

        private void SetEscrow(bool value)
        {
            DoEscrow = value;
            TransferToVisible = value;
            EscrowVisible = value;
            
            if (DoEscrow)
                TransferToBankVisible = value;
        }

        protected override BudgetItem PopulatePrimaryKeyControls(BudgetItem newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
            var budgetItem = AppGlobals.DataRepository.GetBudgetItem(Id);
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, budgetItem.Description);
            return budgetItem;
        }

        protected override void LoadFromEntity(BudgetItem entity)
        {
            _loading = true;

            BudgetItemType = entity.Type;
            BankAutoFillValue =
                new AutoFillValue(
                    AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(entity.BankAccount),
                    entity.BankAccount.Description);
            Amount = entity.Amount;
            RecurringPeriod = entity.RecurringPeriod;
            RecurringType = entity.RecurringType;
            StartingDate = entity.StartingDate;
            EndingDate = entity.EndingDate;
            DoEscrow = entity.DoEscrow;

            if (entity.TransferToBankAccount != null)
            {
                TransferToBankAccountAutoFillValue = new AutoFillValue(
                    AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(entity.TransferToBankAccount),
                    entity.TransferToBankAccount.Description);
            }

            _loading = false;
            SetViewMode();
        }

        protected override BudgetItem GetEntityData()
        {
            var description = string.Empty;
            if (KeyAutoFillValue != null)
                description = KeyAutoFillValue.Text;

            var bankAccountId = 0;
            if (BankAutoFillValue != null && BankAutoFillValue.PrimaryKeyValue.IsValid)
                bankAccountId = AppGlobals.LookupContext.BankAccounts
                    .GetEntityFromPrimaryKeyValue(BankAutoFillValue.PrimaryKeyValue).Id;

            int? transferToBankAccountId = null;
            if (TransferToBankAccountAutoFillValue != null &&
                TransferToBankAccountAutoFillValue.PrimaryKeyValue.IsValid)
                transferToBankAccountId = AppGlobals.LookupContext.BankAccounts
                    .GetEntityFromPrimaryKeyValue(TransferToBankAccountAutoFillValue.PrimaryKeyValue).Id;

            var budgetItem = new BudgetItem
            {
                Id = Id,
                Description = description,
                Type = BudgetItemType,
                BankAccountId = bankAccountId,
                Amount = Amount,
                RecurringPeriod = RecurringPeriod,
                RecurringType = RecurringType,
                StartingDate = StartingDate,
                EndingDate = EndingDate,
                DoEscrow = DoEscrow,
                TransferToBankAccountId = transferToBankAccountId,
                MonthlyAmount = MonthlyAmount
            };
            return budgetItem;
        }

        protected override void ClearData()
        {
            _loading = true;

            Id = 0;
            BudgetItemType = BudgetItemTypes.Expense;
            BankAutoFillValue = null;
            Amount = 0;
            RecurringPeriod = 1;
            RecurringType = BudgetItemRecurringTypes.Months;
            StartingDate = DateTime.Today;
            EndingDate = null;
            MonthlyAmount = null;
            
            TransferToBankAccountAutoFillValue = null;

            _loading = false;

            SetViewMode();
        }

        protected override bool SaveEntity(BudgetItem entity)
        {
            return AppGlobals.DataRepository.SaveBudgetItem(entity);
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DataRepository.DeleteBudgetItem(Id);
        }

        public override bool ValidateEntityProperty(FieldDefinition fieldDefinition, string valueToValidate)
        {
            return base.ValidateEntityProperty(fieldDefinition, valueToValidate);
        }
    }
}
