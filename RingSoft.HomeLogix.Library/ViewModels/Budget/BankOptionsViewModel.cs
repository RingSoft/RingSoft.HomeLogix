using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AutoFill;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.Sqlite.Migrations;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public interface IBankOptionsView
    {
        void Close();
    }
    public class BankOptionsViewModel : INotifyPropertyChanged
    {
        #region Properties

        private int _statementDayOfMonth;

        public int StatementDayOfMonth
        {
            get => _statementDayOfMonth;
            set
            {
                if (_statementDayOfMonth == value)
                {
                    return;
                }
                _statementDayOfMonth = value;
                OnPropertyChanged();
            }
        }

        private double _bankAccountIntrestRate;

        public double BankAccountIntrestRate
        {
            get => _bankAccountIntrestRate;
            set
            {
                if (_bankAccountIntrestRate == value)
                {
                    return;
                }
                _bankAccountIntrestRate = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _interestBudgetAutoFillSetup;

        public AutoFillSetup InterestBudgetAutoFillSetup
        {
            get => _interestBudgetAutoFillSetup;
            set
            {
                if (_interestBudgetAutoFillSetup == value)
                {
                    return;
                }
                _interestBudgetAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _interestBudgetAutoFillValue;
        public AutoFillValue InterestBudgetAutoFillValue
        {
            get => _interestBudgetAutoFillValue;
            set
            {
                if (_interestBudgetAutoFillValue == value)
                {
                    return;
                }
                _interestBudgetAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private BankCreditCardOptions _creditCardOption;

        public BankCreditCardOptions CreditCardOption
        {
            get => _creditCardOption;
            set
            {
                if (_creditCardOption == value)
                {
                    return;
                }
                _creditCardOption = value;
                OnPropertyChanged();
                SetPayCCVisibility();
            }
        }

        private AutoFillSetup _cCPaymentBudgetaAutoFillSetup;

        public AutoFillSetup CCPaymentBudgetAutoFillSetup
        {
            get => _cCPaymentBudgetaAutoFillSetup;
            set
            {
                if (_cCPaymentBudgetaAutoFillSetup == value)
                {
                    return;
                }
                _cCPaymentBudgetaAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _cCPaymentBudgetAutoFillValue;

        public AutoFillValue CCPaymentBudgetAutoFillValue
        {
            get => _cCPaymentBudgetAutoFillValue;
            set
            {
                if (_cCPaymentBudgetAutoFillValue == value)
                {
                    return;
                }
                _cCPaymentBudgetAutoFillValue = value;

                OnPropertyChanged();
            }
        }

        private int _payCCBalanceDay;

        public int PayCCBalanceDay
        {
            get => _payCCBalanceDay;
            set
            {
                if (_payCCBalanceDay == value)
                {
                    return;
                }
                _payCCBalanceDay = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public BankOptionsData BankOptionsData { get; private set; }

        public UiCommand InterestUiCommand { get; } = new UiCommand();

        public UiCommand CcOptionsUiCommand { get; } = new UiCommand();

        public UiCommand PayCCBudgetUiCommand { get; } = new UiCommand();

        public UiCommand PayCCDayUiCommand { get; } = new UiCommand();

        public RelayCommand OkCommand { get; }

        public RelayCommand CancelCommand { get; }

        public IBankOptionsView View { get; private set; }

        public BankOptionsViewModel()
        {
            OkCommand = new RelayCommand(OnOK);

            CancelCommand = new RelayCommand(OnCancel);
        }

        public void Initialize(IBankOptionsView view, BankOptionsData bankOptionsData)
        {
            View = view;
            BankOptionsData = bankOptionsData;

            InterestBudgetAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.BankAccounts.GetFieldDefinition(p => p.InterestBudgetId));

            CCPaymentBudgetAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.BankAccounts.GetFieldDefinition(p => p.PayCCBalanceBudgetId));

            var payCCLookupDefinition =
                (LookupDefinition<BudgetItemLookup, BudgetItem>)CCPaymentBudgetAutoFillSetup.LookupDefinition;
            payCCLookupDefinition.FilterDefinition.ClearFixedFilters();

            switch (BankOptionsData.BankAccountViewModel.AccountType)
            {
                case BankAccountTypes.CreditCard:
                    payCCLookupDefinition.FilterDefinition.AddFixedFilter(p => p.TransferToBankAccountId,
                        Conditions.Equals, BankOptionsData.BankAccountViewModel.Id);
                    break;
            }

            payCCLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Type, Conditions.Equals,
                (int)BudgetItemTypes.Transfer);

            SetupInterestAutoFillSetup();

            CreditCardOption = bankOptionsData.CreditCardOption;

            switch (BankOptionsData.BankAccountViewModel.AccountType)
            {
                case BankAccountTypes.Checking:
                case BankAccountTypes.Savings:
                    InterestUiCommand.Caption = "Interest Payment Budget Item";
                    CcOptionsUiCommand.Visibility = UiVisibilityTypes.Collapsed;
                    break;
                case BankAccountTypes.CreditCard:
                    InterestUiCommand.Caption = "Interest Charge Budget Item";
                    CcOptionsUiCommand.Visibility = UiVisibilityTypes.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (bankOptionsData.StatementDayOfMonth == 0)
            {
                bankOptionsData.StatementDayOfMonth = 1;
            }

            if (bankOptionsData.PayCCBalanceDay == 0)
            {
                bankOptionsData.PayCCBalanceDay = 1;
            }
            StatementDayOfMonth = bankOptionsData.StatementDayOfMonth;
            BankAccountIntrestRate = bankOptionsData.BankAccountIntrestRate;
            InterestBudgetAutoFillValue = bankOptionsData.InterestBudgetAutoFillValue;
            CCPaymentBudgetAutoFillValue = bankOptionsData.CcPaymentBudgetaAutoFillValue;
            PayCCBalanceDay = bankOptionsData.PayCCBalanceDay;
        }

        private void SetupInterestAutoFillSetup()
        {
            var intBudgetLookupDefinition =
                (LookupDefinition<BudgetItemLookup, BudgetItem>)InterestBudgetAutoFillSetup.LookupDefinition;
            intBudgetLookupDefinition.FilterDefinition.ClearFixedFilters();

            switch (BankOptionsData.BankAccountViewModel.AccountType)
            {
                case BankAccountTypes.Checking:
                case BankAccountTypes.Savings:
                    intBudgetLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Type, Conditions.Equals,
                        (int)BudgetItemTypes.Income);
                    break;
                case BankAccountTypes.CreditCard:
                    intBudgetLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Type, Conditions.Equals,
                        (int)BudgetItemTypes.Expense);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public void SetPayCCVisibility()
        {
            switch (CreditCardOption)
            {
                case BankCreditCardOptions.CarryBalance:
                    PayCCBudgetUiCommand.Visibility = UiVisibilityTypes.Collapsed;
                    PayCCDayUiCommand.Visibility = UiVisibilityTypes.Collapsed;
                    break;
                case BankCreditCardOptions.PayOffEachMonth:
                    PayCCBudgetUiCommand.Visibility = UiVisibilityTypes.Visible;
                    PayCCDayUiCommand.Visibility = UiVisibilityTypes.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnOK()
        {
            BankOptionsData.DialogResult = true;

            View.Close();
        }

        private void OnCancel()
        {
            BankOptionsData.DialogResult = false;

            View.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
