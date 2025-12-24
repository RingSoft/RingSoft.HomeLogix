using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.LookupModel;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public enum ValidationFocusControls
    {
        BudgetItem = 0,
        TransferToBank = 1,
        Amount = 2
    }

    public interface IBankAccountMiscView
    {
        void SetViewType();

        void OnOkButtonCloseWindow();

        void OnValidationFail(string message, string caption, ValidationFocusControls control);
    }
    public class BankAccountMiscViewModel : INotifyPropertyChanged
    {
        #region Properties

        private int _registerId;

        public int RegisterId
        {
            get => _registerId;
            set
            {
                if (_registerId == value)
                    return;

                _registerId = value;
                OnPropertyChanged();
            }
        }

        private DateTime _date;

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date == value)
                    return;

                _date = value;
                OnPropertyChanged();
            }
        }

        private string _bankText;

        public string BankText
        {
            get => _bankText;
            set
            {
                if (_bankText == value)
                {
                    return;
                }
                _bankText = value;
                OnPropertyChanged();
            }
        }

        public BudgetItemTypes ItemType { get; private set; }

        private bool _itemTypeEnabled;

        public bool ItemTypeEnabled
        {
            get => _itemTypeEnabled;
            set
            {
                if (_itemTypeEnabled == value)
                    return;

                _itemTypeEnabled = value;
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

        private string _transferFromDescription;

        public string TransferFromDescription
        {
            get => _transferFromDescription;
            set
            {
                if (_transferFromDescription == value)
                    return;

                _transferFromDescription = value;
                OnPropertyChanged();
            }
        }


        private double _amount;

        public double Amount
        {
            get => _amount;
            set
            {
                if (_amount == value)
                    return;

                _amount = value;
                AutofillDescription();
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _budgetItemAutoFillSetup;

        public AutoFillSetup BudgetItemAutoFillSetup
        {
            get => _budgetItemAutoFillSetup;
            set
            {
                if (_budgetItemAutoFillSetup == value)
                    return;

                _budgetItemAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _budgetItemAutoFillValue;

        public AutoFillValue BudgetItemAutoFillValue
        {
            get => _budgetItemAutoFillValue;
            set
            {
                if (_budgetItemAutoFillValue == value)
                    return;

                _budgetItemAutoFillValue = value;
                SetViewMode();
                if (BudgetItemAutoFillValue.IsValid())
                {
                    var budgetItem = BudgetItemAutoFillValue.GetEntity<BudgetItem>();
                    if (budgetItem != null)
                    {
                        budgetItem = budgetItem.FillOutProperties(false);
                        ItemType = (BudgetItemTypes)budgetItem.Type;
                        switch (ItemType)
                        {
                            case BudgetItemTypes.Transfer:
                                var transferBankAccount = new BankAccount
                                {
                                    Id = budgetItem.TransferToBankAccountId.GetValueOrDefault()
                                };
                                transferBankAccount = transferBankAccount.FillOutProperties(false);
                                TransferToBankAccountAutoFillValue = transferBankAccount.GetAutoFillValue();
                                break;
                        }
                    }
                }
                if (BudgetItemAutoFillValue != null)
                    AutofillDescription();
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _transferToBankAccountAutoFillSetup;

        public AutoFillSetup TransferToBankAccountAutoFillSetup
        {
            get => _transferToBankAccountAutoFillSetup;
            set
            {
                if (_transferToBankAccountAutoFillSetup == value)
                    return;

                _transferToBankAccountAutoFillSetup = value;
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

                if (TransferToBankAccountAutoFillValue != null)
                    AutofillDescription();

                OnPropertyChanged();
            }
        }

        #endregion

        public IBankAccountMiscView View { get; private set; }

        public BankAccountViewModel BankViewModel { get; private set; }

        public BankAccountRegisterItem RegisterItem { get; private set; }

        public RelayCommand OkButtonCommand { get; }

        public bool TransferToVisible { get; private set; }

        public bool BudgetItemVisible { get; private set; }

        private bool _loading = true;
        private BankAccountRegisterItem _registerItem;
        private BankAccountRegisterItem _transferToRegisterItem;

        private ViewModelInput _viewModelInput;

        public BankAccountMiscViewModel()
        {
            OkButtonCommand = new RelayCommand(OnOkButton);
            ItemTypeEnabled = true;
        }

        public void OnViewLoaded(IBankAccountMiscView view, BankAccountViewModel bankAccountViewModel,
            BankAccountRegisterItem registerItem, ViewModelInput viewModelInput)
        {
            _viewModelInput = viewModelInput;
            BankViewModel = bankAccountViewModel;
            View = view;
            TransferToBankAccountAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BankAccountsLookup.Clone())
                {AddViewParameter = _viewModelInput};

            _registerItem = registerItem;
            if (_registerItem.Id == 0)
            {
                ItemType = BudgetItemTypes.Expense;
                Date = DateTime.Today;
                TransferFromDescription = AppGlobals.DataRepository
                    .GetBankAccount(_registerItem.BankAccountId, false).Description;
            }
            else
            {
                RegisterId = _registerItem.Id;
                Date = _registerItem.ItemDate;
                BankText = _registerItem.BankText;
                if (_registerItem.IsNegative)
                    Amount = -_registerItem.ProjectedAmount;
                else
                    Amount = Math.Abs(_registerItem.ProjectedAmount);

                var budgetItem =
                    AppGlobals.DataRepository.GetBudgetItem(_registerItem.BudgetItemId);
                if (budgetItem != null)
                {
                    BudgetItemAutoFillValue = budgetItem.GetAutoFillValue();

                    if (_registerItem.BudgetItem != null) 
                        ItemType = (BudgetItemTypes)_registerItem.BudgetItem.Type;
                }
                else if (!_registerItem.TransferRegisterGuid.IsNullOrEmpty())
                {
                    ItemType = BudgetItemTypes.Transfer;
                }

                if (_registerItem.TransferRegisterGuid.IsNullOrEmpty())
                {
                    //if (_registerItem.ProjectedAmount < 0)
                    //    ItemType = BudgetItemTypes.Expense;
                    //else
                    //    ItemType = BudgetItemTypes.Income;

                }
                else
                {
                    ItemType = BudgetItemTypes.Transfer;
                    if (_registerItem.ProjectedAmount < 0)
                    {
                        _transferToRegisterItem =
                            AppGlobals.DataRepository.GetTransferRegisterItem(_registerItem.TransferRegisterGuid);
                        var transferToBankAccount =
                            AppGlobals.DataRepository.GetBankAccount(_transferToRegisterItem.BankAccountId, false);
                        TransferToBankAccountAutoFillValue = transferToBankAccount.GetAutoFillValue();

                        TransferFromDescription = AppGlobals.DataRepository
                            .GetBankAccount(_registerItem.BankAccountId, false).Description;
                    }
                    else
                    {
                        var transferToBankAccount =
                            AppGlobals.DataRepository.GetBankAccount(_registerItem.BankAccountId, false);
                        TransferToBankAccountAutoFillValue = transferToBankAccount.GetAutoFillValue();

                        _transferToRegisterItem = _registerItem;
                        _registerItem =
                            AppGlobals.DataRepository.GetTransferRegisterItem(_transferToRegisterItem.TransferRegisterGuid);
                        TransferFromDescription = _registerItem.BankAccount.Description;
                    }
                }

                ItemTypeEnabled = false;
            }
            Description = _registerItem.Description;
            SetBudgetAutoFillSetup();
            _loading = false;

            RegisterItem = _registerItem;
            SetViewMode();
        }

        private void SetBudgetAutoFillSetup()
        {
            var budgetItemLookup = AppGlobals.LookupContext.BudgetItemsLookup.Clone();
            BudgetItemAutoFillSetup = new AutoFillSetup(budgetItemLookup) {AddViewParameter = _viewModelInput};
        }

        private void SetViewMode()
        {
            if (_loading)
                return;

            switch (ItemType)
            {
                case BudgetItemTypes.Income:
                    BudgetItemVisible = true;
                    TransferToVisible = false;
                    break;
                case BudgetItemTypes.Expense:
                    BudgetItemVisible = true;
                    TransferToVisible = false;
                    if (BudgetItemAutoFillValue.IsValid())
                    {
                        var budgetItem =
                            AppGlobals.LookupContext.BudgetItems.GetEntityFromPrimaryKeyValue(BudgetItemAutoFillValue
                                .PrimaryKeyValue);
                        budgetItem = AppGlobals.DataRepository.GetBudgetItem(budgetItem.Id);
                    }
                    break;
                case BudgetItemTypes.Transfer:
                    BudgetItemVisible = true;
                    TransferToVisible = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            View.SetViewType();
        }

        private void OnOkButton()
        {
            if (ItemType == BudgetItemTypes.Transfer)
            {
                var test = _registerItem.BankAccountId;
                var test1 = this;
                if (_registerItem.BankAccountId != BankViewModel.Id)
                {
                    var message = $"This Miscellaneous Transfer can only be saved from the {_registerItem.BankAccount.Description} Bank Account.";
                    var caption = "Invalid Transfer Bank Account";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return;
                }
            }
            if (!BudgetItemAutoFillValue.IsValid())
            {
                BudgetItemAutoFillSetup.HandleValFail("Budget Item");
                return;
            }
            _registerItem.BudgetItemId = BudgetItemAutoFillValue.GetEntity<BudgetItem>().Id;
            _registerItem.BudgetItem =
                AppGlobals.DataRepository.GetBudgetItem(_registerItem.BudgetItemId);

            if (ItemType == BudgetItemTypes.Transfer)
            {
                if (!TransferToBankAccountAutoFillValue.IsValid())
                {
                    var message = "Transfer To Bank Account must be a valid Bank Account.";
                    View.OnValidationFail(message, "Invalid Transfer To Bank Account",
                        ValidationFocusControls.TransferToBank);
                    return;
                }

                var transferToBankAccount = TransferToBankAccountAutoFillValue
                    .GetEntity<BankAccount>();
                if (transferToBankAccount.Id == _registerItem.BankAccountId)
                {
                    var message = "Transfer To Bank Account cannot be the same as the Bank Account.";
                    View.OnValidationFail(message, "Invalid Transfer To Bank Account",
                        ValidationFocusControls.TransferToBank);
                    return;
                }

                if (Amount < 0)
                {
                    var message = "Transfer amount cannot be less than 0.";
                    View.OnValidationFail(message, "Invalid Amount", ValidationFocusControls.Amount);
                    return;
                }
                _registerItem.RegisterGuid = Guid.NewGuid().ToString();
                _registerItem.TransferRegisterGuid = Guid.NewGuid().ToString();

                _transferToRegisterItem ??= new BankAccountRegisterItem();

                _transferToRegisterItem.BankAccountId = transferToBankAccount.Id;
                _transferToRegisterItem.BudgetItemId = BudgetItemAutoFillValue.GetEntity<BudgetItem>().Id;
                _transferToRegisterItem.ProjectedAmount = Amount;
                _transferToRegisterItem.RegisterGuid = _registerItem.TransferRegisterGuid;
                _transferToRegisterItem.TransferRegisterGuid = _registerItem.RegisterGuid;
                _transferToRegisterItem.ItemDate = Date;
                _transferToRegisterItem.Description = Description;
                _transferToRegisterItem.ItemType = (int) BankAccountRegisterItemTypes.TransferToBankAccount;
                _registerItem.ProjectedAmount = -Amount;
                _transferToRegisterItem.IsTransferMisc = true;
            }
            else
            {

                //_registerItem.ProjectedAmount = Math.Abs(Amount);
                switch (ItemType)
                {
                    case BudgetItemTypes.Income:
                        _registerItem.ProjectedAmount = Amount;
                        //switch (BankViewModel.AccountType)
                        //{
                        //    case BankAccountTypes.Checking:
                        //    case BankAccountTypes.Savings:
                        //        _registerItem.ProjectedAmount = Amount;
                        //        break;
                        //    case BankAccountTypes.CreditCard:
                        //        _registerItem.ProjectedAmount = Amount;
                        //        break;
                        //    default:
                        //        throw new ArgumentOutOfRangeException();
                        //}
                        break;
                    case BudgetItemTypes.Expense:
                        _registerItem.ProjectedAmount = -Amount;
                        //switch (BankViewModel.AccountType)
                        //{
                        //    case BankAccountTypes.Checking:
                        //    case BankAccountTypes.Savings:
                        //        _registerItem.ProjectedAmount = -Amount;
                        //        break;
                        //    case BankAccountTypes.CreditCard:
                        //        _registerItem.ProjectedAmount = -Amount;
                        //        break;
                        //    default:
                        //        throw new ArgumentOutOfRangeException();
                        //}
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

            _registerItem.IsTransferMisc = true;
            _registerItem.ItemType = (int)BankAccountRegisterItemTypes.Miscellaneous;
            _registerItem.ItemDate = Date;
            _registerItem.Description = Description;

            _registerItem.IsNegative = Amount < 0;

            if (_registerItem.ActualAmount != null)
                _registerItem.ActualAmount = _registerItem.ProjectedAmount;

            if (AppGlobals.DataRepository.SaveNewRegisterItem(_registerItem, _transferToRegisterItem))
            {
                //Peter Ringering - 11/22/2024 04:55:34 PM - E-74
                if (_transferToRegisterItem != null && _transferToRegisterItem.BankAccountId > 0)
                {
                    var transferToViewModels
                        = AppGlobals
                            .MainViewModel
                            .BankAccountViewModels
                            .Where(p => p.Id == _transferToRegisterItem.BankAccountId);
                    foreach (var transferToViewModel in transferToViewModels)
                    {
                        transferToViewModel.RefreshAfterBudgetItemSave();
                    }
                }

                if (_viewModelInput != null)
                {
                    if (_viewModelInput.RefreshImportRow != null)
                    {
                        _viewModelInput.RefreshImportRow.RefreshMiscBeforeClose(_registerItem);
                    }
                }
                View.OnOkButtonCloseWindow();
            }
        }

        private void AutofillDescription()
        {
            if (_loading)
                return;

            if (_registerItem.Id != 0)
            {
                return;
            }

            var adjust = "Decrease";
            if (Amount >= 0)
                adjust = "Increase";

            var type = "Expense";
            if (ItemType == BudgetItemTypes.Income)
                type = "Income";

            if (ItemType == BudgetItemTypes.Transfer)
            {
                if (TransferToBankAccountAutoFillValue != null &&
                    !TransferToBankAccountAutoFillValue.Text.IsNullOrEmpty())
                    Description = $"Transfer To {TransferToBankAccountAutoFillValue.Text}";
            }
            else
                Description = $"{adjust} {BudgetItemAutoFillValue.Text} {type}";
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
