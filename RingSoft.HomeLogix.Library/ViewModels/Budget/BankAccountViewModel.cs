using System;
using System.Linq;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public interface IBankAccountView : IDbMaintenanceView
    {
        void EnableRegisterGrid(bool value);
    }

    public class NewRegisterItem
    {
    }

    public class BankAccountViewModel : AppDbMaintenanceViewModel<BankAccount>
    {
        public override TableDefinition<BankAccount> TableDefinition => AppGlobals.LookupContext.BankAccounts;

        public IBankAccountView BankAccountView { get; private set; }

        #region Properties

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

        private decimal _currentProjectedEndingBalance;

        public decimal CurrentProjectedEndingBalance
        {
            get => _currentProjectedEndingBalance;
            set
            {
                if (_currentProjectedEndingBalance == value)
                    return;


                _currentProjectedEndingBalance = value;
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
                    return;

                _currentBalance = value;

                CalculateTotals();
                OnPropertyChanged();
            }
        }

        private BankAccountRegisterGridManager _registerGridManager;

        public BankAccountRegisterGridManager RegisterGridManager
        {
            get => _registerGridManager;
            set
            {
                if (_registerGridManager == value)
                    return;

                _registerGridManager = value;
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


        private decimal _newProjectedEndingBalance;

        public decimal NewProjectedEndingBalance
        {
            get => _newProjectedEndingBalance;
            set
            {
                if (_newProjectedEndingBalance == value)
                    return;


                _newProjectedEndingBalance = value;
                OnPropertyChanged();
            }
        }

        private decimal _projectedEndingBalanceDifference;

        public decimal ProjectedEndingBalanceDifference
        {
            get => _projectedEndingBalanceDifference;
            set
            {
                if (_projectedEndingBalanceDifference == value)
                    return;

                _projectedEndingBalanceDifference = value;
                OnPropertyChanged();
            }
        }


        private DateTime? _projectedLowestBalanceDate;

        public DateTime? ProjectedLowestBalanceDate
        {
            get => _projectedLowestBalanceDate;
            set
            {
                if (_projectedLowestBalanceDate == value)
                    return;

                _projectedLowestBalanceDate = value;
                OnPropertyChanged();
            }
        }

        private decimal _projectedLowestBalanceAmount;

        public decimal ProjectedLowestBalanceAmount
        {
            get => _projectedLowestBalanceAmount;
            set
            {
                if (_projectedLowestBalanceAmount == value)
                    return;

                _projectedLowestBalanceAmount = value;
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

        private decimal _previousMonthDeposits;

        public decimal PreviousMonthDeposits
        {
            get => _previousMonthDeposits;
            set
            {
                if (_previousMonthDeposits == value)
                    return;

                _previousMonthDeposits = value;
                OnPropertyChanged();
            }
        }

        private decimal _previousMonthWithdrawals;

        public decimal PreviousMonthWithdrawals
        {
            get => _previousMonthWithdrawals;
            set
            {
                if (_previousMonthWithdrawals == value)
                    return;

                _previousMonthWithdrawals = value;
                OnPropertyChanged();
            }
        }

        private decimal _previousMonthDifference;

        public decimal PreviousMonthDifference
        {
            get => _previousMonthDifference;
            set
            {
                if (_previousMonthDifference == value)
                    return;

                _previousMonthDifference = value;
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

        private decimal _previousYearDeposits;

        public decimal PreviousYearDeposits
        {
            get => _previousYearDeposits;
            set
            {
                if (_previousYearDeposits == value)
                    return;

                _previousYearDeposits = value;
                OnPropertyChanged();
            }
        }


        private decimal _previousYearWithdrawals;   

        public decimal PreviousYearWithdrawals
        {
            get => _previousYearWithdrawals;
            set
            {
                if (_previousYearWithdrawals == value)
                    return;

                _previousYearWithdrawals = value;
                OnPropertyChanged();
            }
        }

        private decimal _previousYearDifference;

        public decimal PreviousYearDifference
        {
            get => _previousYearDifference;
            set
            {
                if (_previousYearDifference == value)
                    return;

                _previousYearDifference = value;
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

        private int? _escrowDayOfMonth;

        public int? EscrowDayOfMonth
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

        #endregion

        public RelayCommand AddNewRegisterItemCommand { get; }

        public RelayCommand GenerateRegisterItemsFromBudgetCommand { get; }

        private bool _loading;

        public BankAccountViewModel()
        {
            AddNewRegisterItemCommand = new RelayCommand(AddNewRegisterItem);

            GenerateRegisterItemsFromBudgetCommand = new RelayCommand(GenerateRegisterItemsFromBudget);
        }

        protected override void Initialize()
        {
            BankAccountView = View as IBankAccountView;
            if (BankAccountView == null)
                throw new Exception($"Bank Account View interface must be of type '{nameof(IBankAccountView)}'.");

            BankAccountView.EnableRegisterGrid(false);


            EscrowBankAccountAutoFillSetup =
                new AutoFillSetup(
                    AppGlobals.LookupContext.BankAccounts.GetFieldDefinition(p => p.EscrowToBankAccountId));

            RegisterGridManager = new BankAccountRegisterGridManager(this);

            base.Initialize();
        }

        protected override void ClearData()
        {
            _loading = true;

            Id = 0;
            CurrentProjectedEndingBalance = 0;
            CurrentBalance = 0;
            NewProjectedEndingBalance = 0;
            ProjectedEndingBalanceDifference = 0;
            EscrowBalance = null;
            ProjectedLowestBalanceDate = null;
            ProjectedLowestBalanceAmount = 0;
            
            MonthlyBudgetDeposits = 0;
            MonthlyBudgetWithdrawals = 0;
            MonthlyBudgetDifference = 0;
            
            CurrentMonthDeposits = 0;
            CurrentMonthWithdrawals = 0;
            CurrentMonthDifference = 0;
            PreviousMonthDeposits = 0;
            PreviousMonthWithdrawals = 0;
            PreviousMonthDifference = 0;
            CurrentYearDeposits = 0;
            CurrentYearWithdrawals = 0;
            CurrentYearDifference = 0;

            EscrowBankAccountAutoFillValue = null;
            EscrowDayOfMonth = 1;
            Notes = string.Empty;

            RegisterGridManager.SetupForNewRecord();
            BankAccountView.EnableRegisterGrid(false);

            _loading = false;
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
            _loading = true;

            CurrentProjectedEndingBalance = entity.ProjectedEndingBalance;
            CurrentBalance = entity.CurrentBalance;
            EscrowBalance = entity.EscrowBalance;
            MonthlyBudgetDeposits = entity.MonthlyBudgetDeposits;
            MonthlyBudgetWithdrawals = entity.MonthlyBudgetWithdrawals;

            CurrentMonthDeposits = entity.CurrentMonthDeposits;
            CurrentMonthWithdrawals = entity.CurrentMonthWithdrawals;
            PreviousMonthDeposits = entity.PreviousMonthDeposits;
            PreviousMonthWithdrawals = entity.PreviousMonthWithdrawals;
            CurrentYearDeposits = entity.CurrentYearDeposits;
            CurrentYearWithdrawals = entity.CurrentYearWithdrawals;
            
            EscrowBankAccountAutoFillValue = null;
            if (entity.EscrowToBankAccount != null)
            {
                var escrowPrimaryKeyValue =
                    AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(entity.EscrowToBankAccount);
                EscrowBankAccountAutoFillValue =
                    new AutoFillValue(escrowPrimaryKeyValue, entity.EscrowToBankAccount.Description);
            }
            
            EscrowDayOfMonth = entity.EscrowDayOfMonth;
            Notes = entity.Notes;

            RegisterGridManager.LoadGrid(entity.RegisterItems);
            BankAccountView.EnableRegisterGrid(RegisterGridManager.Rows.Any());

            _loading = false;
            CalculateTotals();
        }

        private void CalculateTotals()
        {
            if (_loading)
                return;

            //NewProjectedEndingBalance = 0;
            ProjectedEndingBalanceDifference = NewProjectedEndingBalance - CurrentProjectedEndingBalance;
            //ProjectedLowestBalanceDate = null;
            //ProjectedLowestBalanceAmount = 0;
            
            MonthlyBudgetDifference = MonthlyBudgetDeposits - MonthlyBudgetWithdrawals;
            CurrentMonthDifference = CurrentMonthDeposits - CurrentMonthWithdrawals;
            PreviousMonthDifference = PreviousMonthDeposits - PreviousMonthWithdrawals;
            CurrentYearDifference = CurrentYearDeposits - CurrentYearWithdrawals;
        }

        public void RefreshBudgetTotals()
        {
            //MonthlyBudgetDeposits = 0;
            //MonthlyBudgetWithdrawals = 0;
            //EscrowBalance = entity.EscrowBalance;
            //RegisterGridManager.Refresh
            CalculateTotals();
        }

        private void AddNewRegisterItem()
        {
            //TODO
        }

        private void GenerateRegisterItemsFromBudget()
        {
            //TODO
        }

        protected override BankAccount GetEntityData()
        {
            var bankAccount = new BankAccount
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
                ProjectedEndingBalance = NewProjectedEndingBalance,
                CurrentBalance = CurrentBalance,
                EscrowBalance = EscrowBalance,
                ProjectedLowestBalanceDate = ProjectedLowestBalanceDate,
                ProjectedLowestBalanceAmount = ProjectedLowestBalanceAmount,
                MonthlyBudgetDeposits = MonthlyBudgetDeposits,
                MonthlyBudgetWithdrawals = MonthlyBudgetWithdrawals,
                CurrentMonthDeposits = CurrentMonthDeposits,
                CurrentMonthWithdrawals = CurrentMonthWithdrawals,
                PreviousMonthDeposits = PreviousMonthDeposits,
                PreviousMonthWithdrawals = PreviousMonthWithdrawals,
                CurrentYearDeposits = CurrentYearDeposits,
                CurrentYearWithdrawals = CurrentYearWithdrawals,
                EscrowDayOfMonth = EscrowDayOfMonth,
                Notes = Notes
            };

            if (EscrowBankAccountAutoFillValue != null)
            {
                bankAccount.EscrowToBankAccountId = AppGlobals.LookupContext.BankAccounts
                    .GetEntityFromPrimaryKeyValue(EscrowBankAccountAutoFillValue.PrimaryKeyValue).Id;
            }

            
            return bankAccount;
        }

        protected override AutoFillValue GetAutoFillValueForNullableForeignKeyField(FieldDefinition fieldDefinition)
        {
            if (fieldDefinition ==
                AppGlobals.LookupContext.BankAccounts.GetFieldDefinition(p => p.EscrowToBankAccountId))
                return EscrowBankAccountAutoFillValue;

            return base.GetAutoFillValueForNullableForeignKeyField(fieldDefinition);
        }

        protected override bool SaveEntity(BankAccount entity)
        {
            //var registerItems = RegisterGridManager.GetEntityList();
            return AppGlobals.DataRepository.SaveBankAccount(entity);
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DataRepository.DeleteBankAccount(Id);
        }
    }
}
