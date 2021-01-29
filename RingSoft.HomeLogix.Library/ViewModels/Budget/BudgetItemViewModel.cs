using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using System.ComponentModel;
using System.Linq;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public interface IBudgetItemView : IDbMaintenanceView
    {
        void SetViewType();
    }

    public class BudgetItemViewModel : AppDbMaintenanceViewModel<BudgetItem>
    {
        public override TableDefinition<BudgetItem> TableDefinition => AppGlobals.LookupContext.BudgetItems;

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

        private TextComboBoxControlSetup _budgetItemTypeComboBoxControlSetup;

        public TextComboBoxControlSetup BudgetItemTypeComboBoxControlSetup
        {
            get => _budgetItemTypeComboBoxControlSetup;
            set
            {
                if (_budgetItemTypeComboBoxControlSetup == value)
                    return;

                _budgetItemTypeComboBoxControlSetup = value;
                OnPropertyChanged();
            }
        }


        private TextComboBoxItem _budgetItemTypeComboBoxItem;

        public TextComboBoxItem BudgetItemTypeComboBoxItem
        {
            get => _budgetItemTypeComboBoxItem;
            set
            {
                if (_budgetItemTypeComboBoxItem == value)
                    return;

                _budgetItemTypeComboBoxItem = value;
                SetViewMode();
                OnPropertyChanged();
            }
        }

        public BudgetItemTypes BudgetItemType
        {
            get => (BudgetItemTypes)BudgetItemTypeComboBoxItem.NumericValue;
            set => BudgetItemTypeComboBoxItem = BudgetItemTypeComboBoxControlSetup.GetItem((int)value);
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

                SetViewMode();
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

        private TextComboBoxControlSetup _recurringTypeComboBoxControlSetup;

        public TextComboBoxControlSetup RecurringTypeComboBoxControlSetup
        {
            get => _recurringTypeComboBoxControlSetup;
            set
            {
                if (_recurringTypeComboBoxControlSetup == value)
                    return;

                _recurringTypeComboBoxControlSetup = value;
                OnPropertyChanged();
            }
        }


        private TextComboBoxItem _recurringTypeComboBoxItem;

        public TextComboBoxItem RecurringTypeComboBoxItem
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

        public BudgetItemRecurringTypes RecurringType
        {
            get => (BudgetItemRecurringTypes)RecurringTypeComboBoxItem.NumericValue;
            set => RecurringTypeComboBoxItem = RecurringTypeComboBoxControlSetup.GetItem((int)value);
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
                SetViewMode();
                OnPropertyChanged();
            }
        }

        private DateTime? _endingDate;

        public DateTime? EndingDate
        {
            get => _endingDate;
            set
            {
                if (_endingDate == value)
                    return;

                _endingDate = value;
                SetViewMode();
                OnPropertyChanged();
            }
        }

        private bool _doEscrow;

        public bool DoEscrow
        {
            get => _doEscrow;
            set
            {
                if (_doEscrow == value)
                    return;

                _doEscrow = value;
                SetViewMode(true);
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


        private decimal _monthlyAmount;

        public decimal MonthlyAmount
        {
            get => _monthlyAmount;
            set
            {
                if (_monthlyAmount == value)
                    return;

                _monthlyAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _yearlyAmount;

        public decimal YearlyAmount
        {
            get => _yearlyAmount;
            set
            {
                if (_yearlyAmount == value)
                    return;

                _yearlyAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _currentMonthAmount;

        public decimal CurrentMonthAmount
        {
            get => _currentMonthAmount;
            set
            {
                if (_currentMonthAmount == value)
                    return;

                _currentMonthAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _previousMonthAmount;

        public decimal PreviousMonthAmount
        {
            get => _previousMonthAmount;
            set
            {
                if (_previousMonthAmount == value)
                    return;

                _previousMonthAmount = value;
                OnPropertyChanged();
            }
        }


        private decimal _currentYearAmount;

        public decimal CurrentYearAmount
        {
            get => _currentYearAmount;
            set
            {
                if (_currentYearAmount == value)
                    return;

                _currentYearAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _previousYearAmount;

        public decimal PreviousYearAmount
        {
            get => _previousYearAmount;
            set
            {
                if (_previousYearAmount == value)
                    return;

                _previousYearAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _currentMonthPercent;

        public decimal CurrentMonthPercent
        {
            get => _currentMonthPercent;
            set
            {
                if (_currentMonthPercent == value)
                    return;

                _currentMonthPercent = value;
                OnPropertyChanged();
            }
        }


        private decimal _monthToDatePercent;

        public decimal MonthToDatePercent
        {
            get => _monthToDatePercent;
            set
            {
                if (_monthToDatePercent == value)
                    return;

                _monthToDatePercent = value;
                OnPropertyChanged();
            }
        }

        private decimal _monthlyPercentDifference;

        public decimal MonthlyPercentDifference
        {
            get => _monthlyPercentDifference;
            set
            {
                if (_monthlyPercentDifference == value)
                    return;

                _monthlyPercentDifference = value;
                OnPropertyChanged();
            }
        }

        private decimal _monthlyAmountRemaining;

        public decimal MonthlyAmountRemaining
        {
            get => _monthlyAmountRemaining;
            set
            {
                if (_monthlyAmountRemaining == value)
                    return;

                _monthlyAmountRemaining = value;
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

        public DateTime? LastCompletedDate { get; private set; }

        public DateTime NextTransactionDate { get; private set; }

        public int DbBankAccountId { get; private set; }

        public BankAccount DbBankAccount { get; private set; }

        public int? DbTransferToBankId { get; private set; }

        public BankAccount DbTransferToBankAccount { get; private set; }

        public bool EscrowVisible { get; set; }
        public bool TransferToBankVisible { get; set; }

        public ViewModelInput ViewModelInput { get; set; }


        private IBudgetItemView _view;
        private bool _loading;
        private decimal _dbMonthlyAmount;
        private bool _dbDoEscrow;
        private decimal? _dbEscrowBalance;
        private BankAccount _escrowToBankAccount;

        protected override void Initialize()
        {
            if (View is IBudgetItemView budgetExpenseView)
                _view = budgetExpenseView;

            _loading = true;

            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is ViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
            }
            else
            {
                ViewModelInput = new ViewModelInput();
            }
            ViewModelInput.BudgetItemViewModels.Add(this);

            BudgetItemTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            BudgetItemTypeComboBoxControlSetup.LoadFromEnum<BudgetItemTypes>();

            RecurringTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            RecurringTypeComboBoxControlSetup.LoadFromEnum<BudgetItemRecurringTypes>();

            BankAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BankAccountsLookup);

            _loading = false;

            base.Initialize();
        }

        protected override void ClearData()
        {
            _loading = true;

            Id = 0;
            BudgetItemType = BudgetItemTypes.Expense;
            BankAutoFillValue = null;
            DbBankAccountId = 0;
            DoEscrow = false;
            Amount = 0;
            RecurringPeriod = 1;
            RecurringType = BudgetItemRecurringTypes.Months;
            StartingDate = DateTime.Today;
            EndingDate = null;
            _dbMonthlyAmount = MonthlyAmount = 0;
            _dbEscrowBalance = null;
            _dbDoEscrow = false;

            TransferToBankAccountAutoFillValue = null;
            DbTransferToBankId = 0;

            _loading = false;

            SetViewMode();
        }

        private void SetViewMode(bool fromSetEscrow = false, bool calculateEscrowBalance = true)
        {
            if (_loading)
                return;

            EscrowVisible = TransferToBankVisible = false;

            switch (BudgetItemType)
            {
                case BudgetItemTypes.Income:
                    EscrowVisible = false;
                    break;
                case BudgetItemTypes.Expense:
                    switch (RecurringType)
                    {
                        case BudgetItemRecurringTypes.Days:
                        case BudgetItemRecurringTypes.Weeks:
                            EscrowVisible = false;
                            DoEscrow = false;
                            break;
                        case BudgetItemRecurringTypes.Months:
                            if (RecurringPeriod > 1)
                            {
                                EscrowVisible = true;
                                if (!fromSetEscrow)
                                    DoEscrow = true;
                            }
                            else
                            {
                                EscrowVisible = false;
                            }
                            break;
                        case BudgetItemRecurringTypes.Years:
                            EscrowVisible = true;
                            if (!fromSetEscrow)
                                DoEscrow = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case BudgetItemTypes.Transfer:
                    EscrowVisible = false;
                    DoEscrow = false;
                    TransferToBankVisible = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _view.SetViewType();

            var budgetItemData = new BudgetItemProcessorData
            {
                BudgetItem = GetBudgetItem()
            };

            if (EscrowVisible)
            {
                budgetItemData.BudgetItem.BankAccountId = AppGlobals.LookupContext.BankAccounts
                    .GetEntityFromPrimaryKeyValue(BankAutoFillValue.PrimaryKeyValue).Id;
            }
            else
            {
                budgetItemData.BudgetItem.DoEscrow = false;
            }

            BudgetItemProcessor.CalculateBudgetItem(budgetItemData);

            if (calculateEscrowBalance)
                EscrowBalance = budgetItemData.BudgetItem.EscrowBalance;

            MonthlyAmount = budgetItemData.BudgetItem.MonthlyAmount;
            YearlyAmount = budgetItemData.YearlyAmount;
            CurrentMonthPercent = budgetItemData.CurrentMonthPercent;
            MonthToDatePercent = budgetItemData.MonthToDatePercent;
            MonthlyPercentDifference = budgetItemData.MonthlyPercentDifference;
            MonthlyAmountRemaining = budgetItemData.MonthlyAmountRemaining;
        }

        protected override BudgetItem PopulatePrimaryKeyControls(BudgetItem newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
            var budgetItem = AppGlobals.DataRepository.GetBudgetItem(Id);
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, budgetItem.Description);

            _dbMonthlyAmount = budgetItem.MonthlyAmount;
            DbBankAccountId = budgetItem.BankAccountId;
            DbTransferToBankId = budgetItem.TransferToBankAccountId;
            _dbDoEscrow = budgetItem.DoEscrow;
            _dbEscrowBalance = budgetItem.EscrowBalance;

            ReadOnlyMode = ViewModelInput.BudgetItemViewModels.Any(a => a != this && a.Id == Id);
            return budgetItem;
        }

        protected override void LoadFromEntity(BudgetItem entity)
        {
            _loading = true;

            BudgetItemType = entity.Type;
            BankAutoFillValue =
                new AutoFillValue(
                    AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(entity.BankAccount),
                    entity.BankAccount.Description);

            Amount = entity.Amount;
            RecurringPeriod = entity.RecurringPeriod;
            RecurringType = entity.RecurringType;
            StartingDate = entity.StartingDate;
            EndingDate = entity.EndingDate;
            DoEscrow = entity.DoEscrow;
            LastCompletedDate = entity.LastCompletedDate;
            NextTransactionDate = entity.NextTransactionDate;
            MonthlyAmount = entity.MonthlyAmount;
            CurrentMonthAmount = entity.CurrentMonthAmount;
            PreviousMonthAmount = entity.PreviousMonthAmount;
            CurrentYearAmount = entity.CurrentYearAmount;
            PreviousYearAmount = entity.PreviousYearAmount;
            EscrowBalance = entity.EscrowBalance;
            Notes = entity.Notes;

            if (entity.TransferToBankAccount != null)
            {
                TransferToBankAccountAutoFillValue = new AutoFillValue(
                    AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(entity.TransferToBankAccount),
                    entity.TransferToBankAccount.Description);
            }

            _loading = false;
            SetViewMode(false, false);

            if (ReadOnlyMode)
            {
                ControlsGlobals.UserInterface.ShowMessageBox("This Budget Item is being modified in another window.  Editing not allowed.", "Editing Not Allowed", RsMessageBoxIcons.Exclamation);
            }
        }

        protected override BudgetItem GetEntityData()
        {
            var newBankAccountId = 0;
            if (BankAutoFillValue != null && BankAutoFillValue.PrimaryKeyValue.IsValid)
                newBankAccountId = AppGlobals.LookupContext.BankAccounts
                    .GetEntityFromPrimaryKeyValue(BankAutoFillValue.PrimaryKeyValue).Id;

            int? newTransferToBankAccountId = null;
            if (TransferToBankAccountAutoFillValue != null &&
                TransferToBankAccountAutoFillValue.PrimaryKeyValue.IsValid)
                newTransferToBankAccountId = AppGlobals.LookupContext.BankAccounts
                    .GetEntityFromPrimaryKeyValue(TransferToBankAccountAutoFillValue.PrimaryKeyValue).Id;

            BankAccount newBankAccount = null;
            BankAccount newTransferToBankAccount = null;
            if (newBankAccountId != 0)
            {
                newBankAccount = AppGlobals.DataRepository.GetBankAccount(newBankAccountId, false);

                if (newTransferToBankAccountId != null)
                {
                    newTransferToBankAccount = AppGlobals.DataRepository.GetBankAccount((int)newTransferToBankAccountId, false);
                }

                if (newBankAccountId == DbBankAccountId || DbBankAccountId == 0)
                {
                    switch (BudgetItemType)
                    {
                        case BudgetItemTypes.Income:
                            if (newBankAccount != null)
                                newBankAccount.MonthlyBudgetDeposits += MonthlyAmount - _dbMonthlyAmount;
                            break;
                        case BudgetItemTypes.Expense:
                            if (newBankAccount != null && !DoEscrow)
                            {
                                newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount - _dbMonthlyAmount;
                            }

                            break;
                        case BudgetItemTypes.Transfer:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    DbBankAccount = AppGlobals.DataRepository.GetBankAccount(DbBankAccountId, false);
                    
                    switch (BudgetItemType)
                    {
                        case BudgetItemTypes.Income:
                            DbBankAccount.MonthlyBudgetDeposits -= _dbMonthlyAmount;
                            if (newBankAccount != null) 
                                newBankAccount.MonthlyBudgetDeposits += MonthlyAmount;
                            break;
                        case BudgetItemTypes.Expense:
                            if (!_dbDoEscrow)
                                DbBankAccount.MonthlyBudgetWithdrawals -= _dbMonthlyAmount;

                            if (newBankAccount != null && !DoEscrow)
                            {
                                newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount;
                            }

                            break;
                        case BudgetItemTypes.Transfer:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (DoEscrow && newBankAccount != null)
                {
                    var escrowBalance = (decimal) 0;
                    var dbEscrowBalance = (decimal)0;

                    if (EscrowBalance != null)
                        escrowBalance = (decimal)EscrowBalance;

                    if (_dbEscrowBalance != null)
                        dbEscrowBalance = (decimal)_dbEscrowBalance;

                    _escrowToBankAccount = newBankAccount.EscrowToBankAccount;

                    var escrowToBank = newBankAccount;
                    if (_escrowToBankAccount != null)
                    {
                        escrowToBank = _escrowToBankAccount;
                        _escrowToBankAccount.MonthlyBudgetDeposits += escrowBalance - dbEscrowBalance;
                        newBankAccount.MonthlyBudgetWithdrawals += escrowBalance - dbEscrowBalance;
                    }

                    var escrowToBankBalance = (decimal) 0;
                    if (escrowToBank.EscrowBalance != null)
                        escrowToBankBalance = (decimal) escrowToBank.EscrowBalance;

                    escrowToBankBalance += escrowBalance - dbEscrowBalance;
                    escrowToBank.EscrowBalance = escrowToBankBalance;
                }

                if (BudgetItemType == BudgetItemTypes.Transfer)
                {
                    if (newTransferToBankAccount != null && newBankAccount != null)
                    {
                        //DbBankAccount is Old Transfer From Bank Account.
                        DbBankAccount = AppGlobals.DataRepository.GetBankAccount(DbBankAccountId, false);

                        if (DbTransferToBankId != null)
                            DbTransferToBankAccount = AppGlobals.DataRepository.GetBankAccount((int)DbTransferToBankId, false);

                        if (newBankAccountId == DbBankAccountId || DbBankAccountId == 0)
                        {
                            //Same transfer from (new) bank account
                            newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount - _dbMonthlyAmount;
                            if (DbTransferToBankAccount == null)
                            {
                                //Same new transfer to bank account.
                                newTransferToBankAccount.MonthlyBudgetDeposits += MonthlyAmount - _dbMonthlyAmount;
                            }
                            else
                            {
                                //Different transfer to bank account.
                                DbTransferToBankAccount.MonthlyBudgetDeposits -= _dbMonthlyAmount;
                                newTransferToBankAccount.MonthlyBudgetDeposits += MonthlyAmount;
                            }
                        }
                        else
                        {
                            //Different transfer from (new) bank account
                            newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount;
                            var swap = false;
                            if (newTransferToBankAccount.Id != DbTransferToBankAccount.Id)
                            {
                                //Different transfer to bank account.
                                newTransferToBankAccount.MonthlyBudgetDeposits += MonthlyAmount;
                                if (newBankAccount.Id == DbTransferToBankAccount.Id)
                                {
                                    //Swap.
                                    swap = true;
                                    newTransferToBankAccount.MonthlyBudgetWithdrawals -= _dbMonthlyAmount;
                                    newBankAccount.MonthlyBudgetDeposits -= _dbMonthlyAmount;
                                }
                                else if (DbTransferToBankAccount.Id != newTransferToBankAccount.Id)
                                {
                                    DbTransferToBankAccount.MonthlyBudgetDeposits -= _dbMonthlyAmount;
                                }
                            }
                            if (DbBankAccount.Id != newBankAccountId && !swap)
                            {
                                DbBankAccount.MonthlyBudgetWithdrawals -= _dbMonthlyAmount;
                            }
                        }
                    }
                }
            }

            var budgetItem = GetBudgetItem();
            if (DbBankAccountId == newTransferToBankAccountId || DbBankAccountId == newBankAccountId)
            {
                DbBankAccount = null;
            }
            budgetItem.BankAccountId = newBankAccountId;
            budgetItem.BankAccount = newBankAccount;

            if (DbTransferToBankId == newBankAccountId || DbTransferToBankId == newTransferToBankAccountId)
            {
                DbTransferToBankAccount = null;
            }
            budgetItem.TransferToBankAccountId = newTransferToBankAccountId;
            budgetItem.TransferToBankAccount = newTransferToBankAccount;

            return budgetItem;
        }

        private BudgetItem GetBudgetItem()
        {
            var description = string.Empty;
            if (KeyAutoFillValue != null)
                description = KeyAutoFillValue.Text;

            var budgetItem = new BudgetItem
            {
                Id = Id,
                Description = description,
                Type = BudgetItemType,
                Amount = Amount,
                RecurringPeriod = RecurringPeriod == 0 ? 1 : RecurringPeriod,
                RecurringType = RecurringType,
                StartingDate = StartingDate,
                EndingDate = EndingDate,
                DoEscrow = DoEscrow,
                EscrowBalance = EscrowBalance,
                LastCompletedDate = LastCompletedDate,
                NextTransactionDate = NextTransactionDate,
                MonthlyAmount = MonthlyAmount,
                CurrentMonthAmount = CurrentMonthAmount,
                PreviousMonthAmount = PreviousMonthAmount,
                CurrentYearAmount = CurrentYearAmount,
                PreviousYearAmount = PreviousYearAmount,
                Notes = Notes
            };
            return budgetItem;
        }

        protected override bool SaveEntity(BudgetItem entity)
        {
            var result = AppGlobals.DataRepository.SaveBudgetItem(entity, DbBankAccount, DbTransferToBankAccount, _escrowToBankAccount);

            DbBankAccount = DbTransferToBankAccount = null;

            return result;
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DataRepository.DeleteBudgetItem(Id);
        }

        public override bool ValidateEntityProperty(FieldDefinition fieldDefinition, string valueToValidate)
        {
            return base.ValidateEntityProperty(fieldDefinition, valueToValidate);
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                ViewModelInput.BudgetItemViewModels.Remove(this);
        }
    }
}
