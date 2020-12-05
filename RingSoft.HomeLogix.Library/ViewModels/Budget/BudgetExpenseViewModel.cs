using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AutoFill;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public enum RecurringViewTypes
    {
        Escrow = 0,
        DayOrWeek = 11,
        MonthlySpendingMonthly = 2,
        MonthlySpendingWeekly = 3
    }

    public interface IBudgetItemView
    {
        void SetViewType(RecurringViewTypes viewType);
        void OnValidationFail(FieldDefinition failedFieldDefinition);
        void CloseWindow();
    }

    public class BudgetExpenseViewModel : INotifyPropertyChanged
    {
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

        private List<ComboBoxItem> _recurringTypeItemSource;

        public List<ComboBoxItem> RecurringTypeItemSource
        {
            get => _recurringTypeItemSource;
            set
            {
                if (_recurringTypeItemSource == value)
                    return;

                _recurringTypeItemSource = value;
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

        private List<ComboBoxItem> _spendingTypeItemSource;

        public List<ComboBoxItem> SpendingTypeItemSource
        {
            get => _spendingTypeItemSource;
            set
            {
                if (_spendingTypeItemSource == value)
                    return;

                _spendingTypeItemSource = value;
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

        private List<ComboBoxItem> _spendingDayOfWeekItemSource;

        public List<ComboBoxItem> SpendingDayOfWeekItemSource
        {
            get => _spendingDayOfWeekItemSource;
            set
            {
                if (_spendingDayOfWeekItemSource == value)
                    return;

                _spendingDayOfWeekItemSource = value;
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

        private DateTime? _lastTransactionDate;

        public DateTime? LastTransactionDate
        {
            get => _lastTransactionDate;
            set
            {
                if (_lastTransactionDate == value)
                    return;

                _lastTransactionDate = value;
                OnPropertyChanged();
            }
        }

        public BudgetItem BudgetItem { get; private set; }

        public bool DialogResult { get; private set; }

        public RelayCommand OkCommand { get; }
        public RelayCommand CancelCommand { get; }

        private IBudgetItemView _view;
        private bool _loading;
        private DataEntryComboBoxSetup _recurringTypeComboBoxSetup = new DataEntryComboBoxSetup();
        private DataEntryComboBoxSetup _spendingTypeComboBoxSetup = new DataEntryComboBoxSetup();
        private DataEntryComboBoxSetup _spendingDayOfWeekComboBoxSetup = new DataEntryComboBoxSetup();

        public BudgetExpenseViewModel()
        {
            _loading = true;

            RecurringPeriod = 1;

            _recurringTypeComboBoxSetup.LoadFromEnum<BudgetItemRecurringTypes>();
            RecurringTypeItemSource = _recurringTypeComboBoxSetup.Items;
            RecurringTypeComboBoxItem = _recurringTypeComboBoxSetup.GetItem((int) BudgetItemRecurringTypes.Months);

            _spendingTypeComboBoxSetup.LoadFromEnum<BudgetItemSpendingTypes>();
            SpendingTypeItemSource = _spendingTypeComboBoxSetup.Items;
            SpendingTypeComboBoxItem = _spendingTypeComboBoxSetup.GetItem((int) BudgetItemSpendingTypes.Month);

            _spendingDayOfWeekComboBoxSetup.LoadFromEnum<DayOfWeek>();
            SpendingDayOfWeekItemSource = _spendingDayOfWeekComboBoxSetup.Items;
            SpendingDayOfWeekComboBoxItem = _spendingDayOfWeekComboBoxSetup.GetItem((int) DayOfWeek.Sunday);

            _loading = false;
        }

        public void OnViewLoaded(IBudgetItemView view, BudgetItem budgetItem)
        {
            _loading = true;

            _view = view;
            BudgetItem = budgetItem;

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
