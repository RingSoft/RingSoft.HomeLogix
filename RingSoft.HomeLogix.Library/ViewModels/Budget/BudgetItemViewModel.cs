using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using System.ComponentModel;
using System.Linq;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
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
                SetViewMode(true);
                OnPropertyChanged();
            }
        }

        private decimal? _escrowBalance;

        public decimal? EscrowBalance
        {
            get => _escrowBalance;
            set
            {
                if (_escrowBalance == value)
                    return;

                _escrowBalance = value;
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

        private decimal _yearlyAmount;

        public decimal YearlyAmount
        {
            get => _yearlyAmount;
            set
            {
                if (_yearlyAmount == value)
                    return;

                _yearlyAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal? _currentMonthAmount;

        public decimal? CurrentMonthAmount
        {
            get => _currentMonthAmount;
            set
            {
                if (_currentMonthAmount == value)
                    return;

                _currentMonthAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal? _currentYearAmount;

        public decimal? CurrentYearAmount
        {
            get => _currentYearAmount;
            set
            {
                if (_currentYearAmount == value)
                    return;

                _currentYearAmount = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _lastCompletedDate;

        public DateTime? LastCompletedDate
        {
            get => _lastCompletedDate;
            set
            {
                if (_lastCompletedDate == value)
                    return;

                _lastCompletedDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _nextTransactionDate;

        public DateTime? NextTransactionDate
        {
            get => _nextTransactionDate;
            set
            {
                if (_nextTransactionDate == value)
                    return;

                _nextTransactionDate = value;
                OnPropertyChanged();
            }
        }

        private string _notes;

        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes == value)
                    return;

                _notes = value;
                OnPropertyChanged();
            }
        }

        public bool MonthlyAmountVisible { get; set; }
        public bool EscrowVisible { get; set; }
        public bool TransferToBankVisible { get; set; }

        public ViewModelInput ViewModelInput { get; set; }


        private IBudgetItemView _view;
        private bool _loading;

        protected override void Initialize()
        {
            if (View is IBudgetItemView budgetExpenseView)
                _view = budgetExpenseView;

            _loading = true;

            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is ViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
            }
            else
            {
                ViewModelInput = new ViewModelInput();
            }
            ViewModelInput.BudgetItemViewModels.Add(this);

            BudgetItemTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            BudgetItemTypeComboBoxControlSetup.LoadFromEnum<BudgetItemTypes>();

            RecurringTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            RecurringTypeComboBoxControlSetup.LoadFromEnum<BudgetItemRecurringTypes>();

            BankAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BankAccountsLookup);

            _loading = false;

            base.Initialize();
        }

        private void SetViewMode(bool fromSetEscrow = false)
        {
            if (_loading)
                return;

            MonthlyAmountVisible = EscrowVisible = TransferToBankVisible = false;

            switch (BudgetItemType)
            {
                case BudgetItemTypes.Income:
                    EscrowVisible = false;
                    break;
                case BudgetItemTypes.Expense:
                    switch (RecurringType)
                    {
                        case BudgetItemRecurringTypes.Days:
                        case BudgetItemRecurringTypes.Weeks:
                            EscrowVisible = false;
                            DoEscrow = false;
                            break;
                        case BudgetItemRecurringTypes.Months:
                            if (RecurringPeriod > 1)
                            {
                                EscrowVisible = true;
                                if (!fromSetEscrow)
                                    DoEscrow = true;
                            }
                            else
                            {
                                EscrowVisible = false;
                            }
                            break;
                        case BudgetItemRecurringTypes.Years:
                            EscrowVisible = true;
                            if (!fromSetEscrow)
                                DoEscrow = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case BudgetItemTypes.Transfer:
                    EscrowVisible = false;
                    DoEscrow = false;
                    TransferToBankVisible = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (BudgetItemType != BudgetItemTypes.Transfer)
                SetMonthlyAmountVisibility();

            _view.SetViewType();
        }

        private void SetMonthlyAmountVisibility()
        {
            var monthlyAmountVisible = false;
            switch (RecurringType)
            {
                case BudgetItemRecurringTypes.Days:
                case BudgetItemRecurringTypes.Weeks:
                    monthlyAmountVisible = true;
                    break;
                case BudgetItemRecurringTypes.Months:
                    if (RecurringPeriod > 1 && DoEscrow)
                        monthlyAmountVisible = true;
                    break;
                case BudgetItemRecurringTypes.Years:
                    if (DoEscrow)
                        monthlyAmountVisible = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            MonthlyAmountVisible = monthlyAmountVisible;
        }

        protected override BudgetItem PopulatePrimaryKeyControls(BudgetItem newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
            var budgetItem = AppGlobals.DataRepository.GetBudgetItem(Id);
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, budgetItem.Description);

            ReadOnlyMode = ViewModelInput.BudgetItemViewModels.Any(a => a != this && a.Id == Id);
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

            if (ReadOnlyMode)
            {
                ControlsGlobals.UserInterface.ShowMessageBox("This Budget Item is being modified in another window.  Editing not allowed.", "Editing Not Allowed", RsMessageBoxIcons.Exclamation);
            }
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

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                ViewModelInput.BudgetItemViewModels.Remove(this);
        }
    }
}
