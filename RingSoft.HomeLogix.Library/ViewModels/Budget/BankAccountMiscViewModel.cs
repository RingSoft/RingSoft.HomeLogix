using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.LookupModel;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public enum ValidationFocusControls
    {
        BudgetItem = 0,
        TransferToBank = 1
    }

    public interface IBankAccountMiscView
    {
        void SetViewType();

        void OnOkButtonCloseWindow();

        void OnValidationFail(string message, string caption, ValidationFocusControls control);
    }
    public class BankAccountMiscViewModel : INotifyPropertyChanged
    {
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

        private TextComboBoxControlSetup _itemTypeComboBoxControlSetup;

        public TextComboBoxControlSetup ItemTypeComboBoxControlSetup
        {
            get => _itemTypeComboBoxControlSetup;
            set
            {
                if (_itemTypeComboBoxControlSetup == value)
                    return;

                _itemTypeComboBoxControlSetup = value;
                OnPropertyChanged();
            }
        }


        private TextComboBoxItem _itemTypeComboBoxItem;

        public TextComboBoxItem ItemTypeComboBoxItem
        {
            get => _itemTypeComboBoxItem;
            set
            {
                if (_itemTypeComboBoxItem == value)
                    return;

                _itemTypeComboBoxItem = value;
                if (!_loading)
                {
                    BudgetItemAutoFillValue = null;
                }
                SetBudgetAutoFillSetup();
                SetViewMode();
                OnPropertyChanged();
            }
        }

        public BudgetItemTypes ItemType
        {
            get => (BudgetItemTypes)ItemTypeComboBoxItem.NumericValue;
            set => ItemTypeComboBoxItem = ItemTypeComboBoxControlSetup.GetItem((int)value);
        }

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
                OnPropertyChanged();
            }
        }

        public IBankAccountMiscView View { get; private set; }

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

        public void OnViewLoaded(IBankAccountMiscView view, BankAccountRegisterItem registerItem, ViewModelInput viewModelInput)
        {
            _viewModelInput = viewModelInput;
            View = view;
            TransferToBankAccountAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BankAccountsLookup.Clone())
                {AddViewParameter = _viewModelInput};

            ItemTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            ItemTypeComboBoxControlSetup.LoadFromEnum<BudgetItemTypes>();

            _registerItem = registerItem;
            if (_registerItem.Id == 0)
            {
                ItemType = BudgetItemTypes.Expense;
                Date = DateTime.Today;
            }
            else
            {
                RegisterId = _registerItem.Id;
                Description = _registerItem.Description;
                Date = _registerItem.ItemDate;
                Amount = Math.Abs(_registerItem.ProjectedAmount);
                if (_registerItem.TransferRegisterGuid.IsNullOrEmpty())
                {
                    if (_registerItem.ProjectedAmount < 0)
                        ItemType = BudgetItemTypes.Expense;
                    else
                        ItemType = BudgetItemTypes.Income;

                    var budgetItem =
                        AppGlobals.DataRepository.GetBudgetItem(_registerItem.BudgetItemId.GetValueOrDefault(0));
                    BudgetItemAutoFillValue =
                        new AutoFillValue(
                            AppGlobals.LookupContext.BudgetItems.GetPrimaryKeyValueFromEntity(budgetItem),
                            budgetItem.Description);
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
                        TransferToBankAccountAutoFillValue = new AutoFillValue(
                            AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(transferToBankAccount),
                            transferToBankAccount.Description);
                    }
                    else
                    {
                        var transferToBankAccount =
                            AppGlobals.DataRepository.GetBankAccount(_registerItem.BankAccountId, false);
                        TransferToBankAccountAutoFillValue = new AutoFillValue(
                            AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(transferToBankAccount),
                            transferToBankAccount.Description);

                        _transferToRegisterItem = _registerItem;
                        _registerItem =
                            AppGlobals.DataRepository.GetTransferRegisterItem(_transferToRegisterItem.TransferRegisterGuid);
                    }
                }

                ItemTypeEnabled = false;
            }
            SetBudgetAutoFillSetup();
            _loading = false;

            SetViewMode();
        }

        private void SetBudgetAutoFillSetup()
        {
            var budgetItemLookup = AppGlobals.LookupContext.BudgetItemsLookup.Clone();
            budgetItemLookup.FilterDefinition.AddFixedFilter(f => f.Type, Conditions.Equals,
                ItemType);

            _viewModelInput.LockBudgetItemType = ItemType;
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
                    BudgetItemVisible = false;
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
                if (!TransferToBankAccountAutoFillValue.IsValid())
                {
                    var message = "Transfer To Bank Account must be a valid Bank Account.";
                    View.OnValidationFail(message, "Invalid Transfer To Bank Account",
                        ValidationFocusControls.TransferToBank);
                    return;
                }

                var transferToBankAccount = AppGlobals.LookupContext.BankAccounts
                    .GetEntityFromPrimaryKeyValue(TransferToBankAccountAutoFillValue.PrimaryKeyValue);
                if (transferToBankAccount.Id == _registerItem.BankAccountId)
                {
                    var message = "Transfer To Bank Account cannot be the same as the Bank Account.";
                    View.OnValidationFail(message, "Invalid Transfer To Bank Account",
                        ValidationFocusControls.TransferToBank);
                    return;
                }
                _registerItem.RegisterGuid = Guid.NewGuid().ToString();
                _registerItem.TransferRegisterGuid = Guid.NewGuid().ToString();

                _transferToRegisterItem ??= new BankAccountRegisterItem();

                _transferToRegisterItem.BankAccountId = transferToBankAccount.Id;
                _transferToRegisterItem.ProjectedAmount = Amount;
                _transferToRegisterItem.RegisterGuid = _registerItem.TransferRegisterGuid;
                _transferToRegisterItem.TransferRegisterGuid = _registerItem.RegisterGuid;
                _transferToRegisterItem.ItemDate = Date;
                _transferToRegisterItem.Description = Description;
                _transferToRegisterItem.ItemType = (int) BankAccountRegisterItemTypes.Miscellaneous;
            }
            else
            {
                if (!BudgetItemAutoFillValue.IsValid())
                {
                    var message = "Budget Item must contain a valid value.";
                    View.OnValidationFail(message, "Invalid Budget Item.",
                        ValidationFocusControls.BudgetItem);
                    return;
                }
                _registerItem.BudgetItemId = AppGlobals.LookupContext.BudgetItems
                    .GetEntityFromPrimaryKeyValue(BudgetItemAutoFillValue.PrimaryKeyValue).Id;
            }

            _registerItem.ItemType = (int)BankAccountRegisterItemTypes.Miscellaneous;
            _registerItem.ItemDate = Date;
            _registerItem.Description = Description;
            
            switch (ItemType)
            {
                case BudgetItemTypes.Income:
                    _registerItem.ProjectedAmount = Amount;
                    break;
                case BudgetItemTypes.Expense:
                    _registerItem.ProjectedAmount = -Amount;
                    break;
                case BudgetItemTypes.Transfer:
                    _registerItem.ProjectedAmount = -Amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (AppGlobals.DataRepository.SaveNewRegisterItem(_registerItem, _transferToRegisterItem))
                View.OnOkButtonCloseWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
