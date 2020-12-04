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
        MonthlySpendingMonthly = 1,
        MonthlySpendingWeekly = 2
    }

    public interface IBudgetItemView
    {
        void SetViewType(RecurringViewTypes viewType);
        void OnValidationFail(FieldDefinition failedFieldDefinition);
        void CloseWindow(BudgetItem budgetItem);
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

        private IBudgetItemView _view;
        private BudgetItem _budgetItem;
        private bool _loading;

        public void OnViewLoaded(IBudgetItemView view, BudgetItem budgetItem)
        {
            _loading = true;

            _view = view;
            _budgetItem = budgetItem;

        }

        private void SetViewMode()
        {
            if (_loading)
                return;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
