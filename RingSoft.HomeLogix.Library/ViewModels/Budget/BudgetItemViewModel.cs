using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using RingSoft.DbLookup.ModelDefinition;

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
        void SetViewType(RecurringViewTypes viewType);
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

        private ComboBoxControlSetup _budgetItemTypeComboBoxControlSetup;

        public ComboBoxControlSetup BudgetItemTypeComboBoxControlSetup
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


        private ComboBoxItem _budgetItemTypeComboBoxItem;

        public ComboBoxItem BudgetItemTypeComboBoxItem
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


        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                if (_description == value)
                    return;

                _description = value;
                OnPropertyChanged();
            }
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

        private ComboBoxControlSetup _recurringTypeComboBoxControlSetup;

        public ComboBoxControlSetup RecurringTypeComboBoxControlSetup
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


        private ComboBoxItem _recurringTypeComboBoxItem;

        public ComboBoxItem RecurringTypeComboBoxItem
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

        private bool? _doEscrow;

        public bool? DoEscrow
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

        private ComboBoxControlSetup _spendingTypeComboBoxControlSetup;

        public ComboBoxControlSetup SpendingTypeComboBoxControlSetup
        {
            get => _spendingTypeComboBoxControlSetup;
            set
            {
                if (_spendingTypeComboBoxControlSetup == value)
                    return;

                _spendingTypeComboBoxControlSetup = value;
                OnPropertyChanged();
            }
        }


        private ComboBoxItem _spendingTypeComboBoxItem;

        public ComboBoxItem SpendingTypeComboBoxItem
        {
            get => _spendingTypeComboBoxItem;
            set
            {
                if (_spendingTypeComboBoxItem == value)
                    return;

                _spendingTypeComboBoxItem = value;
                SetViewMode();
                OnPropertyChanged();
            }
        }

        public BudgetItemSpendingTypes SpendingType
        {
            get => (BudgetItemSpendingTypes)SpendingTypeComboBoxItem.NumericValue;
            set => SpendingTypeComboBoxItem = SpendingTypeComboBoxControlSetup.GetItem((int)value);
        }

        private ComboBoxControlSetup _spendingDayOfWeekComboBoxControlSetup;

        public ComboBoxControlSetup SpendingDayOfWeekComboBoxControlSetup
        {
            get => _spendingDayOfWeekComboBoxControlSetup;
            set
            {
                if (_spendingDayOfWeekComboBoxControlSetup == value)
                    return;

                _spendingDayOfWeekComboBoxControlSetup = value;
                OnPropertyChanged();
            }
        }


        private ComboBoxItem _spendingDayOfWeekComboBoxItem;

        public ComboBoxItem SpendingDayOfWeekComboBoxItem
        {
            get => _spendingDayOfWeekComboBoxItem;
            set
            {
                if (_spendingDayOfWeekComboBoxItem == value)
                    return;

                _spendingDayOfWeekComboBoxItem = value;
                OnPropertyChanged();
            }
        }

        public DayOfWeek SpendingDayOfWeekType
        {
            get => (DayOfWeek)SpendingDayOfWeekComboBoxItem.NumericValue;
            set => SpendingDayOfWeekComboBoxItem = SpendingDayOfWeekComboBoxControlSetup.GetItem((int)value);
        }

        private string _itemTypeAmountLabel;

        public string ItemTypeAmountLabel
        {
            get => _itemTypeAmountLabel;
            set
            {
                if (_itemTypeAmountLabel == value)
                    return;

                _itemTypeAmountLabel = value;
                OnPropertyChanged();
            }
        }


        private decimal? _itemTypeAmount;

        public decimal? ItemTypeAmount
        {
            get => _itemTypeAmount;
            set
            {
                if (_itemTypeAmount == value)
                    return;

                _itemTypeAmount = value;
                OnPropertyChanged();
            }
        }

        private IBudgetItemView _view;
        private bool _loading;

        protected override void Initialize()
        {
            if (View is IBudgetItemView budgetExpenseView)
                _view = budgetExpenseView;

            _loading = true;

            BudgetItemTypeComboBoxControlSetup = new ComboBoxControlSetup();
            BudgetItemTypeComboBoxControlSetup.LoadFromEnum<BudgetItemTypes>();

            RecurringTypeComboBoxControlSetup = new ComboBoxControlSetup();
            RecurringTypeComboBoxControlSetup.LoadFromEnum<BudgetItemRecurringTypes>();

            SpendingTypeComboBoxControlSetup = new ComboBoxControlSetup();
            SpendingTypeComboBoxControlSetup.LoadFromEnum<BudgetItemSpendingTypes>();

            SpendingDayOfWeekComboBoxControlSetup = new ComboBoxControlSetup();
            SpendingDayOfWeekComboBoxControlSetup.LoadFromEnum<DayOfWeek>();

            BankAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BankAccountsLookup);

            _loading = false;

            SetViewMode();
            base.Initialize();
        }

        private void ResetSpendingPattern()
        {
            SpendingType = BudgetItemSpendingTypes.Month;
            SpendingDayOfWeekType = DayOfWeek.Sunday;
        }

        private void SetViewMode()
        {
            if (_loading)
                return;

            RecurringViewTypes recurringViewType;
            switch (BudgetItemType)
            {
                case BudgetItemTypes.Income:
                    recurringViewType = RecurringViewTypes.Income;
                    SetEscrow(false);
                    ResetSpendingPattern();
                    break;
                case BudgetItemTypes.Expense:
                    switch (RecurringType)
                    {
                        case BudgetItemRecurringTypes.Days:
                        case BudgetItemRecurringTypes.Weeks:
                            recurringViewType = RecurringViewTypes.DayOrWeek;
                            SetEscrow(false);
                            ResetSpendingPattern();
                            break;
                        case BudgetItemRecurringTypes.Months:
                            if (RecurringPeriod > 1)
                            {
                                recurringViewType = RecurringViewTypes.Escrow;
                                SetEscrow(true);
                                ResetSpendingPattern();
                            }
                            else
                            {
                                switch (SpendingType)
                                {
                                    case BudgetItemSpendingTypes.Week:
                                        recurringViewType = RecurringViewTypes.MonthlySpendingWeekly;
                                        break;
                                    default:
                                        recurringViewType = RecurringViewTypes.MonthlySpendingMonthly;
                                        break;
                                }
                                SetEscrow(false);
                            }
                            break;
                        case BudgetItemRecurringTypes.Years:
                            recurringViewType = RecurringViewTypes.Escrow;
                            SetEscrow(true);
                            ResetSpendingPattern();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case BudgetItemTypes.Transfer:
                    recurringViewType = RecurringViewTypes.Transfer;
                    SetEscrow(false);
                    ResetSpendingPattern();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            CalculateItemTypeAmount();
            _view.SetViewType(recurringViewType);
        }

        private void CalculateItemTypeAmount()
        {
            switch (RecurringType)
            {
                case BudgetItemRecurringTypes.Days:
                    ItemTypeAmountLabel = "Monthly Amount";
                    break;
                case BudgetItemRecurringTypes.Weeks:
                    ItemTypeAmountLabel = "Monthly Amount";
                    break;
                case BudgetItemRecurringTypes.Months:
                    if (RecurringPeriod > 1)
                        CalculateEscrow();
                    else
                    {
                        switch (SpendingType)
                        {
                            case BudgetItemSpendingTypes.Day:
                                ItemTypeAmountLabel = "Daily Amount";
                                break;
                            case BudgetItemSpendingTypes.Week:
                                ItemTypeAmountLabel = "Weekly Amount";
                                break;
                            default:
                                ItemTypeAmountLabel = string.Empty;
                                break;
                        }
                    }

                    break;
                case BudgetItemRecurringTypes.Years:
                    CalculateEscrow();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CalculateEscrow()
        {
            var escrow = DoEscrow != null && DoEscrow != false;

            if (escrow || BudgetItemType != BudgetItemTypes.Expense)
            {
                ItemTypeAmountLabel = "Monthly Amount";
            }
        }

        private void SetEscrow(bool value)
        {
            if (value)
            {
                DoEscrow ??= false;
            }
            else
            {
                DoEscrow = null;
            }
        }

        protected override BudgetItem PopulatePrimaryKeyControls(BudgetItem newEntity, PrimaryKeyValue primaryKeyValue)
        {
            throw new NotImplementedException();
        }

        protected override void LoadFromEntity(BudgetItem entity)
        {
            throw new NotImplementedException();
        }

        protected override BudgetItem GetEntityData()
        {
            throw new NotImplementedException();
        }

        protected override void ClearData()
        {
            _loading = true;

            BudgetItemType = BudgetItemTypes.Expense;
            RecurringPeriod = 1;
            RecurringType = BudgetItemRecurringTypes.Months;
            StartingDate = DateTime.Today;

            _loading = false;

            SetViewMode();
        }

        protected override bool SaveEntity(BudgetItem entity)
        {
            throw new NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new NotImplementedException();
        }
    }
}
