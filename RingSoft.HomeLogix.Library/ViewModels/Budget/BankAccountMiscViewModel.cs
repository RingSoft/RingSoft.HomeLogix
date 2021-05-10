using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AutoFill;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public interface IBankAccountMiscView
    {
        void SetViewType();

        void OnOkButtonCloseWindow();
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
                SetViewMode();
                OnPropertyChanged();
            }
        }

        public BudgetItemTypes BudgetItemType
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

        public BankAccountMiscViewModel()
        {
            OkButtonCommand = new RelayCommand(OnOkButton);
        }

        public void OnViewLoaded(IBankAccountMiscView view)
        {
            View = view;
            BudgetItemAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BudgetItemsLookup);
            TransferToBankAccountAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BankAccountsLookup);

            ItemTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            ItemTypeComboBoxControlSetup.LoadFromEnum<BudgetItemTypes>();
        }

        private void SetViewMode()
        {
            switch (BudgetItemType)
            {
                case BudgetItemTypes.Income:
                case BudgetItemTypes.Expense:
                    BudgetItemVisible = true;
                    TransferToVisible = false;
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

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
