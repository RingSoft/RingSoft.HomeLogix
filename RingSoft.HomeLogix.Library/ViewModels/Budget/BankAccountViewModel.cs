using System;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BankAccountViewModel : AppDbMaintenanceViewModel<BankAccount>
    {
        public override TableDefinition<BankAccount> TableDefinition => AppGlobals.LookupContext.BankAccounts;


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

        private AutoFillSetup _escrowBankAccountAutoFillSetup;

        public AutoFillSetup EscrowBankAccountAutoFillSetup
        {
            get => _escrowBankAccountAutoFillSetup;
            set
            {
                if (_escrowBankAccountAutoFillSetup == value)
                    return;

                _escrowBankAccountAutoFillSetup = value;
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

        private int _escrowDayOfMonth;

        public int EscrowDayOfMonth
        {
            get => _escrowDayOfMonth;
            set
            {
                if (_escrowDayOfMonth == value)
                    return;

                _escrowDayOfMonth = value;
                OnPropertyChanged();
            }
        }

        private decimal _oldBalance;

        public decimal OldBalance
        {
            get => _oldBalance;
            set
            {
                if (_oldBalance == value)
                    return;


                _oldBalance = value;
                OnPropertyChanged();
            }
        }

        private decimal _currentBankBalance;

        public decimal CurrentBankBalance
        {
            get => _currentBankBalance;
            set
            {
                if (_currentBankBalance == value)
                    return;

                _currentBankBalance = value;
                OnPropertyChanged();
            }
        }

        private decimal? _escrowBalance;

        public decimal? EscrowBalance
        {
            get => _escrowBalance;
            set
            {
                if (_escrowBalance == value)
                    return;

                _escrowBalance = value;
                OnPropertyChanged();
            }
        }


        private decimal _projectedBankBalance;

        public decimal ProjectedBankBalance
        {
            get => _projectedBankBalance;
            set
            {
                if (_projectedBankBalance == value)
                    return;


                _projectedBankBalance = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _projectedLowestBankBalanceDate;

        public DateTime? ProjectedLowestBankBalanceDate
        {
            get => _projectedLowestBankBalanceDate;
            set
            {
                if (_projectedLowestBankBalanceDate == value)
                    return;

                _projectedLowestBankBalanceDate = value;
                OnPropertyChanged();
            }
        }

        private decimal _projectedLowestBankBalanceAmount;

        public decimal ProjectedLowestBankBalanceAmount
        {
            get => _projectedLowestBankBalanceAmount;
            set
            {
                if (_projectedLowestBankBalanceAmount == value)
                    return;

                _projectedLowestBankBalanceAmount = value;
                OnPropertyChanged();
            }
        }


        private decimal _monthlyBudgetDeposits;

        public decimal MonthlyBudgetDeposits
        {
            get => _monthlyBudgetDeposits;
            set
            {
                if (_monthlyBudgetDeposits == value)
                    return;

                _monthlyBudgetDeposits = value;
                OnPropertyChanged();
            }
        }

        private decimal _monthlyBudgetWithdrawals;

        public decimal MonthlyBudgetWithdrawals
        {
            get => _monthlyBudgetWithdrawals;
            set
            {
                if (_monthlyBudgetWithdrawals == value)
                    return;

                _monthlyBudgetWithdrawals = value;
                OnPropertyChanged();
            }
        }

        private decimal _monthlyBudgetDifference;

        public decimal MonthlyBudgetDifference
        {
            get => _monthlyBudgetDifference;
            set
            {
                if (_monthlyBudgetDifference == value)
                    return;

                _monthlyBudgetDifference = value;
                OnPropertyChanged();
            }
        }

        private decimal _currentMonthDeposits;

        public decimal CurrentMonthDeposits
        {
            get => _currentMonthDeposits;
            set
            {
                if (_currentMonthDeposits == value)
                    return;

                _currentMonthDeposits = value;
                OnPropertyChanged();
            }
        }

        private decimal _currentMonthWithdrawals;

        public decimal CurrentMonthWithdrawals
        {
            get => _currentMonthWithdrawals;
            set
            {
                if (_currentMonthWithdrawals == value)
                    return;

                _currentMonthWithdrawals = value;
                OnPropertyChanged();
            }
        }

        private decimal _currentMonthDifference;

        public decimal CurrentMonthDifference
        {
            get => _currentMonthDifference;
            set
            {
                if (_currentMonthDifference == value)
                    return;
                
                _currentMonthDifference = value;
                OnPropertyChanged();
            }
        }

        private decimal _currentYearDeposits;

        public decimal CurrentYearDeposits
        {
            get => _currentYearDeposits;
            set
            {
                if (_currentYearDeposits == value)
                    return;

                _currentYearDeposits = value;
                OnPropertyChanged();
            }
        }

        private decimal _currentYearWithdrawals;

        public decimal CurrentYearWithdrawals
        {
            get => _currentYearWithdrawals;
            set
            {
                if (_currentYearWithdrawals == value)
                    return;

                _currentYearWithdrawals = value;
                OnPropertyChanged();
            }
        }

        private decimal _currentYearDifference;

        public decimal CurrentYearDifference
        {
            get => _currentYearDifference;
            set
            {
                if (_currentYearDifference == value)
                    return;

                _currentYearDifference = value;
                OnPropertyChanged();
            }
        }

        private string _notes;

        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes == value)
                    return;

                _notes = value;
                OnPropertyChanged();
            }
        }


        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override BankAccount PopulatePrimaryKeyControls(BankAccount newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
            var bankAccount = AppGlobals.DataRepository.GetBankAccount(Id);
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, bankAccount.Description);
            return bankAccount;
        }

        protected override void LoadFromEntity(BankAccount entity)
        {
            CurrentBankBalance = entity.CurrentBalance;
        }

        protected override BankAccount GetEntityData()
        {
            var bankAccount = new BankAccount
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
                CurrentBalance = CurrentBankBalance
            };

            return bankAccount;
        }

        protected override void ClearData()
        {
            Id = 0;
            CurrentBankBalance = 0;
        }

        protected override bool SaveEntity(BankAccount entity)
        {
            return AppGlobals.DataRepository.SaveBankAccount(entity);
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DataRepository.DeleteBankAccount(Id);
        }
    }
}
