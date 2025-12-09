using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AutoFill;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RingSoft.DbLookup;
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

        private async void CheckRecalculate()
        {
            BankOptionsData.Recalculate = false;
            {
                var message =
                    "Do you wish to recalculate the Register Grid?  All existing calculations will be lost.";
                if (await ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, "Recalculate") ==
                    MessageBoxButtonsResult.Yes)
                    BankOptionsData.Recalculate = true;
            }
        }

        private void OnOK()
        {
            if (!ValidateData())
            {
                return;
            }

            BankOptionsData.DialogResult = true;

            var recalc = false;
            if (BankOptionsData.StatementDayOfMonth != StatementDayOfMonth)
            {
                BankOptionsData.StatementDayOfMonth = StatementDayOfMonth;
                recalc = true;
            }

            if (Math.Abs(BankOptionsData.BankAccountIntrestRate - BankAccountIntrestRate) > 0.0001)
            {
                BankOptionsData.BankAccountIntrestRate = BankAccountIntrestRate;
                recalc = true;
            }

            if (BankOptionsData.InterestBudgetAutoFillValue.GetEntity<BudgetItem>().Id
                != InterestBudgetAutoFillValue.GetEntity<BudgetItem>().Id)
            {
                BankOptionsData.InterestBudgetAutoFillValue = InterestBudgetAutoFillValue;
                recalc = true;
            }

            if (BankOptionsData.CreditCardOption != CreditCardOption)
            {
                BankOptionsData.CreditCardOption = CreditCardOption;
                recalc = true;
            }

            if (BankOptionsData.CcPaymentBudgetaAutoFillValue.GetEntity<BudgetItem>().Id
                != CCPaymentBudgetAutoFillValue.GetEntity<BudgetItem>().Id)
            {
                BankOptionsData.CcPaymentBudgetaAutoFillValue = CCPaymentBudgetAutoFillValue;
                recalc = true;
            }

            if (BankOptionsData.PayCCBalanceDay != PayCCBalanceDay)
            {
                BankOptionsData.PayCCBalanceDay = PayCCBalanceDay;
                recalc = true;
            }

            if (BankOptionsData.BankAccountViewModel.RegisterGridManager.Rows.Any())
            {
                BankOptionsData.Recalculate = recalc;
                if (!recalc)
                {
                    CheckRecalculate();
                }
            }
            else
            {
                BankOptionsData.Recalculate = false;
            }

            BankOptionsData.StatementDayOfMonth = StatementDayOfMonth;
            BankOptionsData.BankAccountIntrestRate = BankAccountIntrestRate;
            BankOptionsData.InterestBudgetAutoFillValue = InterestBudgetAutoFillValue;
            BankOptionsData.CreditCardOption = CreditCardOption;
            BankOptionsData.CcPaymentBudgetaAutoFillValue = CCPaymentBudgetAutoFillValue;
            BankOptionsData.PayCCBalanceDay = PayCCBalanceDay;

            View.Close();
        }

        private bool ValidateData()
        {
            switch (BankOptionsData.BankAccountViewModel.AccountType)
            {
                case BankAccountTypes.Checking:
                case BankAccountTypes.Savings:
                    if (InterestBudgetAutoFillValue.IsValid() && BankAccountIntrestRate > 0)
                    {
                        return true;
                    }
                    else
                    {
                        ControlsGlobals.UserInterface.ShowMessageBox(
                            "You must specify an interest rate and an interest budget item."
                            , "Validation Fail", RsMessageBoxIcons.Exclamation);
                        return false;
                    }

                    break;
                case BankAccountTypes.CreditCard:
                    switch (CreditCardOption)
                    {
                        //case BankCreditCardOptions.CarryBalance:
                        //    if (InterestBudgetAutoFillValue.IsValid() && BankAccountIntrestRate > 0)
                        //    {
                        //        return true;
                        //    }
                        //    else
                        //    {
                        //        ControlsGlobals.UserInterface.ShowMessageBox(
                        //            "You must specify an interest rate and an interest budget item."
                        //            , "Validation Fail", RsMessageBoxIcons.Exclamation);
                        //        return false;
                        //    }
                        //    break;
                        case BankCreditCardOptions.PayOffEachMonth:
                            if (CCPaymentBudgetAutoFillValue.IsValid()
                                && PayCCBalanceDay > 0)
                            {
                                var budgetItem = CCPaymentBudgetAutoFillValue.GetEntity<BudgetItem>().FillOutProperties(false);
                                if (budgetItem.TransferToBankAccountId != BankOptionsData.BankAccountViewModel.Id)
                                {
                                    ControlsGlobals.UserInterface.ShowMessageBox(
                                        "The credit card payment budget item must be a transfer to this credit card account."
                                        , "Validation Fail", RsMessageBoxIcons.Exclamation);
                                    PayCCBudgetUiCommand.SetFocus();
                                    CCPaymentBudgetAutoFillSetup.Control.ShowLookupWindow();
                                    return false;
                                }
                                if (PayCCBalanceDay > StatementDayOfMonth)
                                {
                                    ControlsGlobals.UserInterface.ShowMessageBox(
                                        "The credit card payment day of the month must be less than or equal to the bank account statement day of the month."
                                        , "Validation Fail", RsMessageBoxIcons.Exclamation);
                                    PayCCDayUiCommand.SetFocus();
                                    return false;
                                }

                                //var ccPaymentBudget = CCPaymentBudgetAutoFillValue.GetEntity<BudgetItem>();
                                //ccPaymentBudget = ccPaymentBudget.FillOutProperties(true);
                                //if (ccPaymentBudget.BankAccount.AccountType == ((byte)BankAccountTypes.CreditCard))
                                //{
                                    
                                //}
                                return true;
                            }
                            else
                            {
                                ControlsGlobals.UserInterface.ShowMessageBox(
                                    "You must specify a transfer credit card payment budget item."
                                    , "Validation Fail", RsMessageBoxIcons.Exclamation);
                                PayCCBudgetUiCommand.SetFocus();
                                CCPaymentBudgetAutoFillSetup.Control.ShowLookupWindow();
                                return false;
                            }
                            break;
                        case BankCreditCardOptions.CarryBalance:
                            return true;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return false;
        }


        private void OnCancel()
        {
            BankOptionsData.Recalculate = false;
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
