using RingSoft.HomeLogix.Library.PhoneModel;
using RingSoft.HomeLogix.MobileInterop.PhoneModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.HomeLogix.Mobile.ViewModels
{
    public class BankDetailsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<RegisterData> _registerData;

        public ObservableCollection<RegisterData> RegisterData
        {
            get => _registerData;
            set
            {
                if (_registerData == value)
                {
                    return;
                }
                _registerData = value;
                OnPropertyChanged();
            }
        }

        private decimal _endingBalance;

        public decimal EndingBalance
        {
            get => _endingBalance;
            set
            {
                if (_endingBalance == value)
                {
                    return;
                }
                _endingBalance = value;
                OnPropertyChanged();
            }
        }

        private decimal _projectedLowestBalance;

        public decimal ProjectedLowestBalance
        {
            get => _projectedLowestBalance;
            set
            {
                if (_projectedLowestBalance == value)
                {
                    return;
                }
                _projectedLowestBalance = value;
                OnPropertyChanged();
            }
        }

        private DateTime _projectedLowestBalanceDate;

        public DateTime ProjectedLowestBalanceDate
        {
            get => _projectedLowestBalanceDate;
            set
            {
                if (_projectedLowestBalanceDate == value)
                {
                    return;
                }
                _projectedLowestBalanceDate = value;
                OnPropertyChanged();
            }
        }

        private decimal _currentBalance;

        public decimal CurrentBalance
        {
            get => _currentBalance;
            set
            {
                if (_currentBalance == value)
                {
                    return;
                }
                _currentBalance = value;
                OnPropertyChanged();
            }
        }

        private string _accountType;

        public string AccountType
        {
            get => _accountType;
            set
            {
                if (_accountType == value)
                {
                    return;
                }
                _accountType = value;
                OnPropertyChanged();
            }
        }

        private string _header;

        public string Header
        {
            get => _header;
            set
            {
                if (_header == value)
                    return;
                
                _header = value;
                OnPropertyChanged();
            }
        }


        public void Initialize(BankData bankAccount)
        {
            Header = bankAccount.Description;
            var registerItems = MobileGlobals.MainViewModel.RegisterData
                .Where(p => p.BankAccountId == bankAccount.BankId)
                .OrderBy(p => p.ItemDate)
                .ThenByDescending(p => p.ProjectedAmount)
                .ToList();

            var balance = bankAccount.CurrentBalance;
            foreach (var registerItem in registerItems)
            {
                var amount = Math.Abs(registerItem.ProjectedAmount);
                if (!registerItem.Completed)
                {
                    switch (registerItem.RegisterItemType)
                    {
                        case BankAccountRegisterItemTypes.BudgetItem:
                            balance = SetBalance(bankAccount, registerItem, balance, amount);
                            break;
                        case BankAccountRegisterItemTypes.Miscellaneous:
                            if (registerItem.IsNegative)
                            {
                                balance -= amount;
                            }
                            else
                            {
                                balance += amount;
                            }

                            break;
                        case BankAccountRegisterItemTypes.TransferToBankAccount:
                            balance = SetBalance(bankAccount, registerItem, balance, amount);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                switch (registerItem.TransactionType)
                {
                    case TransactionTypes.Deposit:
                        registerItem.TransactionTypeText = "Deposit";
                        break;
                    case TransactionTypes.Withdrawal:
                        registerItem.TransactionTypeText = "Withdrawal";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                registerItem.EndingBalance = balance;
            }

            switch (bankAccount.AccountType)
            {
                case BankAccountTypes.Checking:
                    AccountType = "Checking";
                    break;
                case BankAccountTypes.Savings:
                    AccountType = "Savings";
                    break;
                case BankAccountTypes.CreditCard:
                    AccountType = "Credit Card";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            RegisterData = new ObservableCollection<RegisterData>(registerItems);
            CurrentBalance = bankAccount.CurrentBalance;
            EndingBalance = balance;
            ProjectedLowestBalance = bankAccount.ProjectedLowestBalance;
            ProjectedLowestBalanceDate = bankAccount.ProjectedLowestBalanceDate;
        }

        private decimal SetBalance(BankData bankAccount, RegisterData registerItem, decimal balance, decimal amount)
        {
            switch (bankAccount.AccountType)
            {
                case BankAccountTypes.Checking:
                case BankAccountTypes.Savings:
                    switch (registerItem.TransactionType)
                    {
                        case TransactionTypes.Deposit:
                            balance += amount;
                            break;
                        case TransactionTypes.Withdrawal:
                            balance -= amount;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                case BankAccountTypes.CreditCard:
                    switch (registerItem.TransactionType)
                    {
                        case TransactionTypes.Deposit:
                            balance -= amount;
                            break;
                        case TransactionTypes.Withdrawal:
                            balance += amount;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return balance;
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
