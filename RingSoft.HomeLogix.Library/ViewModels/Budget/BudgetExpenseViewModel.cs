using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public enum RecurringViewTypes
    {
        Escrow = 0,
        DayOrWeek = 1,
        MonthlySpendingMonthly = 2,
        MonthlySpendingWeekly = 3
    }

    public interface IBudgetExpenseView
    {
        void SetViewType(RecurringViewTypes viewType);
        void OnValidationFail(FieldDefinition failedFieldDefinition);
        void CloseWindow();
    }

    public class BudgetExpenseViewModel : INotifyPropertyChanged
    {
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

        private AutoFillValue _escrowBankAccountAutoFillValue;

        public AutoFillValue EscrowBankAccountAutoFillValue
        {
            get => _escrowBankAccountAutoFillValue;
            set
            {
                if (_escrowBankAccountAutoFillValue == value)
                    return;

                _escrowBankAccountAutoFillValue = value;
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

        public BudgetItem BudgetItem { get; private set; }

        public bool DialogResult { get; private set; }

        public RelayCommand OkCommand { get; }
        public RelayCommand CancelCommand { get; }

        private IBudgetExpenseView _view;
        private bool _loading;

        public BudgetExpenseViewModel()
        {
            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand(OnCancel);
        }

        public void OnViewLoaded(IBudgetExpenseView view, BudgetItem budgetItem)
        {
            _loading = true;

            _view = view;
            BudgetItem = budgetItem;
            RecurringPeriod = 1;

            RecurringTypeComboBoxControlSetup = new ComboBoxControlSetup();
            RecurringTypeComboBoxControlSetup.LoadFromEnum<BudgetItemRecurringTypes>();
            RecurringTypeComboBoxItem = RecurringTypeComboBoxControlSetup.GetItem((int)BudgetItemRecurringTypes.Months);

            SpendingTypeComboBoxControlSetup = new ComboBoxControlSetup();
            SpendingTypeComboBoxControlSetup.LoadFromEnum<BudgetItemSpendingTypes>();
            SpendingTypeComboBoxItem = SpendingTypeComboBoxControlSetup.GetItem((int)BudgetItemSpendingTypes.Month);

            SpendingDayOfWeekComboBoxControlSetup = new ComboBoxControlSetup();
            SpendingDayOfWeekComboBoxControlSetup.LoadFromEnum<DayOfWeek>();
            SpendingDayOfWeekComboBoxItem = SpendingDayOfWeekComboBoxControlSetup.GetItem((int)DayOfWeek.Sunday);

            BankAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BankAccountsLookup);

            _loading = false;
        }

        private void SetViewMode()
        {
            if (_loading)
                return;

            var recurringType = (BudgetItemRecurringTypes) RecurringTypeComboBoxItem.NumericValue;
            var spendingType = (BudgetItemSpendingTypes) SpendingTypeComboBoxItem.NumericValue;
            
            RecurringViewTypes recurringViewType;
            switch (recurringType)
            {
                case BudgetItemRecurringTypes.Days:
                case BudgetItemRecurringTypes.Weeks:
                    recurringViewType = RecurringViewTypes.DayOrWeek;
                    break;
                case BudgetItemRecurringTypes.Months:
                    if (RecurringPeriod > 1)
                    {
                        recurringViewType = RecurringViewTypes.Escrow;
                    }
                    else
                    {
                        switch (spendingType)
                        {
                            case BudgetItemSpendingTypes.Week:
                                recurringViewType = RecurringViewTypes.MonthlySpendingWeekly;
                                break;
                            default:
                                recurringViewType = RecurringViewTypes.MonthlySpendingMonthly;
                                break;
                        }
                    }
                    break;
                case BudgetItemRecurringTypes.Years:
                    recurringViewType = RecurringViewTypes.Escrow;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _view.SetViewType(recurringViewType);
        }

        private void OnOk()
        {
            DialogResult = true;
            _view.CloseWindow();
        }

        private void OnCancel()
        {
            _view.CloseWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
