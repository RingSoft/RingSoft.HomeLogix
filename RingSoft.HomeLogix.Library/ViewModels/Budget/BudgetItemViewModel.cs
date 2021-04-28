using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using System.ComponentModel;
using System.Linq;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.LookupModel;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public interface IBudgetItemView : IDbMaintenanceView
    {
        void SetViewType();

        void ShowMonthlyStatsControls(bool show = true);
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

        private bool _budgetItemTypeEnabled;

        public bool BudgetItemTypeEnabled
        {
            get => _budgetItemTypeEnabled;
            set
            {
                if (_budgetItemTypeEnabled == value)
                    return;

                _budgetItemTypeEnabled = value;
                OnPropertyChanged(nameof(BudgetItemTypeEnabled), false);
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

                SetViewMode(true);
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

        private DateTime? _startingDate;

        public DateTime? StartingDate
        {
            get => _startingDate;
            set
            {
                if (_startingDate == value)
                    return;

                _startingDate = value;
                SetViewMode(true);
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

        private bool _dateControlsEnabled;

        public bool DateControlsEnabled
        {
            get => _dateControlsEnabled;
            set
            {
                if (_dateControlsEnabled == value)
                    return;

                _dateControlsEnabled = value;
                OnPropertyChanged(nameof(DateControlsEnabled), false);
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
                OnPropertyChanged(nameof(EscrowBalance), false);
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

        private DateTime _currentMonthEnding;

        public DateTime CurrentMonthEnding
        {
            get => _currentMonthEnding;
            set
            {
                if (_currentMonthEnding == value)
                    return;

                _currentMonthEnding = value;
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

        private LookupDefinition<BudgetPeriodHistoryLookup, BudgetPeriodHistory> _monthlyLookupDefinition;

        public LookupDefinition<BudgetPeriodHistoryLookup, BudgetPeriodHistory> MonthlyLookupDefinition
        {
            get => _monthlyLookupDefinition;
            set
            {
                if (_monthlyLookupDefinition == value)
                    return;

                _monthlyLookupDefinition = value;
                OnPropertyChanged(nameof(MonthlyLookupDefinition), false);
            }
        }

        private LookupCommand _monthlyLookupCommand;

        public LookupCommand MonthlyLookupCommand
        {
            get => _monthlyLookupCommand;
            set
            {
                if (_monthlyLookupCommand == value)
                    return;

                _monthlyLookupCommand = value;
                OnPropertyChanged(nameof(MonthlyLookupCommand), false);
            }
        }

        private LookupDefinition<BudgetPeriodHistoryLookup, BudgetPeriodHistory> _yearlyLookupDefinition;

        public LookupDefinition<BudgetPeriodHistoryLookup, BudgetPeriodHistory> YearlyLookupDefinition
        {
            get => _yearlyLookupDefinition;
            set
            {
                if (_yearlyLookupDefinition == value)
                    return;

                _yearlyLookupDefinition = value;
                OnPropertyChanged(nameof(YearlyLookupDefinition), false);
            }
        }

        private LookupCommand _yearlyLookupCommand;

        public LookupCommand YearlyLookupCommand
        {
            get => _yearlyLookupCommand;
            set
            {
                if (_yearlyLookupCommand == value)
                    return;

                _yearlyLookupCommand = value;
                OnPropertyChanged(nameof(YearlyLookupCommand), false);
            }
        }

        private LookupDefinition<HistoryLookup, History> _historyLookupDefinition;

        public LookupDefinition<HistoryLookup, History> HistoryLookupDefinition
        {
            get => _historyLookupDefinition;
            set
            {
                if (_historyLookupDefinition == value)
                    return;

                _historyLookupDefinition = value;
                OnPropertyChanged(nameof(HistoryLookupDefinition), false);
            }
        }

        private LookupCommand _historyLookupCommand;

        public LookupCommand HistoryLookupCommand
        {
            get => _historyLookupCommand;
            set
            {
                if (_historyLookupCommand == value)
                    return;

                _historyLookupCommand = value;
                OnPropertyChanged(nameof(HistoryLookupCommand), false);
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

        private DateTime? _lastCompletedDate;

        public DateTime? LastCompletedDate
        {
            get => _lastCompletedDate;
            set
            {
                if (_lastCompletedDate == value)
                    return;

                _lastCompletedDate = value;
                OnPropertyChanged(nameof(LastCompletedDate), false);
            }
        }


        #endregion

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
        private decimal? _dbEscrowBalance;
        private BankAccount _escrowToBankAccount;
        private BankAccount _dbEscrowToBankAccount;
        private bool _dbDoEscrow;

        private LookupDefinition<BudgetPeriodHistoryLookup, BudgetPeriodHistory>
            _periodHistoryLookupDefinition =
                new LookupDefinition<BudgetPeriodHistoryLookup, BudgetPeriodHistory>(AppGlobals.LookupContext
                    .BudgetPeriodHistory);

        public BudgetItemViewModel()
        {
            DateControlsEnabled = BudgetItemTypeEnabled = true;

            _periodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.PeriodEndingDate, p => p.PeriodEndingDate);
            _periodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.ProjectedAmount, p => p.ProjectedAmount);
            _periodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.ActualAmount, p => p.ActualAmount);

            var table = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(AppGlobals.LookupContext
                .BudgetPeriodHistory.TableName);
            var projectedAmountField = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(AppGlobals
                .LookupContext.BudgetPeriodHistory.GetFieldDefinition(p => p.ProjectedAmount).FieldName);
            var actualAmountField = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(AppGlobals
                .LookupContext.BudgetPeriodHistory.GetFieldDefinition(p => p.ActualAmount).FieldName);

            var formula = $"{table}.{projectedAmountField} - {table}.{actualAmountField}";
            _periodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.Difference, formula)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            _periodHistoryLookupDefinition.InitialOrderByType = OrderByTypes.Descending;
        }

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

            BankAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BankAccountsLookup)
            {
                AddViewParameter = ViewModelInput
            };

            MonthlyLookupDefinition = _periodHistoryLookupDefinition.Clone();
            YearlyLookupDefinition = _periodHistoryLookupDefinition.Clone();
            HistoryLookupDefinition = AppGlobals.LookupContext.HistoryLookup.Clone();

            _loading = false;

            base.Initialize();
        }

        protected override void ClearData()
        {
            _loading = true;

            Id = 0;
            BudgetItemType = BudgetItemTypes.Expense;

            BankAutoFillValue = null;
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition ==
                    AppGlobals.LookupContext.BankAccounts)
                {
                    var bankAccount =
                        AppGlobals.LookupContext.BankAccounts.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                            .ParentWindowPrimaryKeyValue);
                    bankAccount = AppGlobals.DataRepository.GetBankAccount(bankAccount.Id, false);

                    BankAutoFillValue =
                        new AutoFillValue(LookupAddViewArgs.ParentWindowPrimaryKeyValue, bankAccount.Description);
                }
            }


            DbBankAccountId = 0;
            DoEscrow = _dbDoEscrow = false;
            Amount = 0;
            RecurringPeriod = 1;
            RecurringType = BudgetItemRecurringTypes.Months;
            StartingDate = DateTime.Today;
            EndingDate = null;
            _dbMonthlyAmount = MonthlyAmount = 0;
            _dbEscrowBalance = null;
            _escrowToBankAccount = null;

            TransferToBankAccountAutoFillValue = null;
            DbTransferToBankId = 0;
            DateControlsEnabled = BudgetItemTypeEnabled = true;

            CurrentMonthEnding = new DateTime(DateTime.Today.Year, DateTime.Today.Month,
                DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

            LastCompletedDate = null;

            MonthlyLookupCommand = GetLookupCommand(LookupCommands.Clear);
            YearlyLookupCommand = GetLookupCommand(LookupCommands.Clear);
            HistoryLookupCommand = GetLookupCommand(LookupCommands.Clear);

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
                    DoEscrow = false;
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
                                    SetEscrow();
                            }
                            else
                            {
                                EscrowVisible = false;
                                DoEscrow = false;
                            }
                            break;
                        case BudgetItemRecurringTypes.Years:
                            EscrowVisible = true;
                            if (!fromSetEscrow)
                                SetEscrow();
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

            switch (RecurringType)
            {
                case BudgetItemRecurringTypes.Days:
                case BudgetItemRecurringTypes.Weeks:
                    _view.ShowMonthlyStatsControls();
                    break;
                case BudgetItemRecurringTypes.Months:
                case BudgetItemRecurringTypes.Years:
                    _view.ShowMonthlyStatsControls(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            RecalculateBudgetItem(GetBudgetItem(), calculateEscrowBalance);
        }

        public void RecalculateBudgetItem()
        {
            var budgetItem = AppGlobals.DataRepository.GetBudgetItem(Id);
            EscrowBalance = budgetItem.EscrowBalance;
            CurrentMonthAmount = budgetItem.CurrentMonthAmount;
            LastCompletedDate = budgetItem.LastCompletedDate;
            CurrentMonthEnding = budgetItem.CurrentMonthEnding;

            RecalculateBudgetItem(budgetItem, false);
        }

        private void RecalculateBudgetItem(BudgetItem budgetItem, bool calculateEscrowBalance)
        {
            var budgetItemData = new BudgetItemProcessorData
            {
                BudgetItem = budgetItem
            };

            if (EscrowVisible && BankAutoFillValue?.PrimaryKeyValue != null && BankAutoFillValue.PrimaryKeyValue.IsValid)
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

            if (MaintenanceMode == DbMaintenanceModes.AddMode && StartingDate == null)
                EscrowBalance = 0;

            MonthlyAmount = budgetItemData.BudgetItem.MonthlyAmount;
            YearlyAmount = budgetItemData.YearlyAmount;
            CurrentMonthPercent = budgetItemData.CurrentMonthPercent;
            MonthToDatePercent = budgetItemData.MonthToDatePercent;
            MonthlyPercentDifference = budgetItemData.MonthlyPercentDifference;
            MonthlyAmountRemaining = budgetItemData.MonthlyAmountRemaining;
        }

        private void SetEscrow()
        {
            DoEscrow = true;
            StartingDate = null;
            EscrowBalance = 0;
        }

        protected override BudgetItem PopulatePrimaryKeyControls(BudgetItem newEntity, PrimaryKeyValue primaryKeyValue)
        {
            _loading = true;
            Id = newEntity.Id;
            var budgetItem = AppGlobals.DataRepository.GetBudgetItem(Id);
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, budgetItem.Description);

            _dbMonthlyAmount = budgetItem.MonthlyAmount;
            DbBankAccountId = budgetItem.BankAccountId;
            DbTransferToBankId = budgetItem.TransferToBankAccountId;
            _dbEscrowBalance = budgetItem.EscrowBalance;
            _dbDoEscrow = budgetItem.DoEscrow;

            ReadOnlyMode = ViewModelInput.BudgetItemViewModels.Any(a => a != this && a.Id == Id);
            BudgetItemTypeEnabled = false;
            StartingDate = budgetItem.StartingDate;

            if (StartingDate == null)
            {
                DateControlsEnabled = false;
            }

            MonthlyLookupDefinition.FilterDefinition.ClearFixedFilters();
            MonthlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.PeriodType,
                Conditions.Equals, (int)PeriodHistoryTypes.Monthly);
            MonthlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BudgetItemId,
                Conditions.Equals, Id);

            MonthlyLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            YearlyLookupDefinition.FilterDefinition.ClearFixedFilters();
            YearlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.PeriodType,
                Conditions.Equals, (int)PeriodHistoryTypes.Yearly);
            YearlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BudgetItemId,
                Conditions.Equals, Id);

            YearlyLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            HistoryLookupDefinition.FilterDefinition.ClearFixedFilters();
            HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BudgetItemId, Conditions.Equals, Id);

            HistoryLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            _loading = false;
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
            EndingDate = entity.EndingDate;
            DoEscrow = entity.DoEscrow;
            MonthlyAmount = entity.MonthlyAmount;
            CurrentMonthAmount = entity.CurrentMonthAmount;
            CurrentMonthEnding = entity.CurrentMonthEnding;
            EscrowBalance = entity.EscrowBalance;
            Notes = entity.Notes;
            LastCompletedDate = entity.LastCompletedDate;

            if (entity.TransferToBankAccount != null)
            {
                TransferToBankAccountAutoFillValue = new AutoFillValue(
                    AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(entity.TransferToBankAccount),
                    entity.TransferToBankAccount.Description);
            }

            _loading = false;
            SetViewMode(true, false);

            if (ReadOnlyMode)
            {
                ControlsGlobals.UserInterface.ShowMessageBox("This Budget Item is being modified in another window.  Editing not allowed.", "Editing Not Allowed", RsMessageBoxIcons.Exclamation);
            }
            else
            {
                if (StartingDate != null)
                {
                    DateControlsEnabled = true;
                }
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
                            if (!DoEscrow)
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
                    var escrowBalance = (decimal)0;
                    var dbEscrowBalance = (decimal)0;
                    var dbMonthlyAmount = _dbMonthlyAmount;

                    if (EscrowBalance != null)
                        escrowBalance = (decimal)EscrowBalance;

                    if (_dbEscrowBalance != null)
                        dbEscrowBalance = (decimal)_dbEscrowBalance;

                    var escrowToBank = newBankAccount;
                    _escrowToBankAccount = newBankAccount.EscrowToBankAccount;
                    var newMonthlyBudgetWithdrawals = MonthlyAmount - _dbMonthlyAmount;

                    if (newBankAccountId != DbBankAccountId && DbBankAccountId != 0)
                    {
                        if (_escrowToBankAccount != null && DbBankAccount.EscrowToBankAccount != null &&
                            _escrowToBankAccount.Id != DbBankAccount.EscrowToBankAccount.Id)
                        {
                            var dbEscrowToBank = DbBankAccount;
                            _dbEscrowToBankAccount = DbBankAccount.EscrowToBankAccount;
                            if (_dbEscrowToBankAccount != null)
                            {
                                dbEscrowToBank = _dbEscrowToBankAccount;
                                _dbEscrowToBankAccount.MonthlyBudgetDeposits -= dbMonthlyAmount;
                            }

                            dbEscrowToBank.EscrowBalance -= dbEscrowBalance;
                            dbMonthlyAmount = dbEscrowBalance = 0;
                        }

                        DbBankAccount.MonthlyBudgetWithdrawals -= _dbMonthlyAmount;
                        newMonthlyBudgetWithdrawals = MonthlyAmount;
                    }

                    if (_escrowToBankAccount != null)
                    {
                        escrowToBank = _escrowToBankAccount;
                        _escrowToBankAccount.MonthlyBudgetDeposits += MonthlyAmount - dbMonthlyAmount;
                        newBankAccount.MonthlyBudgetWithdrawals += newMonthlyBudgetWithdrawals;
                    }

                    var escrowToBankEscrowBalance = (decimal)0;
                    if (escrowToBank.EscrowBalance != null)
                        escrowToBankEscrowBalance = (decimal)escrowToBank.EscrowBalance;

                    escrowToBankEscrowBalance += escrowBalance - dbEscrowBalance;
                    escrowToBank.EscrowBalance = escrowToBankEscrowBalance;
                }
                else if (_dbDoEscrow && newBankAccount != null)
                {
                    decimal dbEscrowBalance = 0;
                    if (_dbEscrowBalance != null)
                        dbEscrowBalance = (decimal)_dbEscrowBalance;

                    var dbEscrowToBank = newBankAccount;
                    if (DbBankAccount != null && DbBankAccount.Id != newBankAccountId)
                    {
                        dbEscrowToBank = DbBankAccount;
                    }
                    _dbEscrowToBankAccount = dbEscrowToBank.EscrowToBankAccount;
                    if (_dbEscrowToBankAccount != null)
                    {
                        dbEscrowToBank = _dbEscrowToBankAccount;
                        _dbEscrowToBankAccount.MonthlyBudgetDeposits -= _dbMonthlyAmount;
                    }

                    dbEscrowToBank.EscrowBalance -= dbEscrowBalance;
                    EscrowBalance = 0;
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
                            if (newTransferToBankAccount.Id == DbTransferToBankId || DbTransferToBankAccount == null)
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
                MonthlyAmount = MonthlyAmount,
                CurrentMonthAmount = CurrentMonthAmount,
                CurrentMonthEnding = CurrentMonthEnding,
                Notes = Notes,
                LastCompletedDate = LastCompletedDate
            };
            return budgetItem;
        }

        protected override bool ValidateEntity(BudgetItem entity)
        {
            foreach (var bankAccountViewModel in ViewModelInput.BankAccountViewModels)
            {
                if (bankAccountViewModel.IsBudgetItemDirty(Id, StartingDate))
                {
                    var message =
                        "This budget item is currently being reconciled.  If you continue, you will loose all your changes to this item in the Register.  Do you wish to continue?";

                    if (!View.ShowYesNoMessage(message, "Budget Item Being Modified", true))
                        return false;
                }
            }
            return base.ValidateEntity(entity);
        }

        protected override bool SaveEntity(BudgetItem entity)
        {
            var result = AppGlobals.DataRepository.SaveBudgetItem(entity, DbBankAccount, DbTransferToBankAccount,
                _escrowToBankAccount, _dbEscrowToBankAccount);

            if (result)
            {
                DbBankAccount = DbTransferToBankAccount = _escrowToBankAccount = _dbEscrowToBankAccount = null;
                if (entity.BankAccountId != DbBankAccountId && LookupAddViewArgs != null)
                    PopulatePrimaryKeyControls(entity,
                        AppGlobals.LookupContext.BudgetItems.GetPrimaryKeyValueFromEntity(entity));
            }

            return result;
        }

        protected override bool DeleteEntity()
        {
            DbBankAccount = AppGlobals.DataRepository.GetBankAccount(DbBankAccountId, false);
            switch (BudgetItemType)
            {
                case BudgetItemTypes.Income:
                    DbBankAccount.MonthlyBudgetDeposits -= _dbMonthlyAmount;
                    break;
                case BudgetItemTypes.Expense:
                    if (_dbDoEscrow)
                    {
                        var dbEscrowToBank = DbBankAccount;
                        _dbEscrowToBankAccount = dbEscrowToBank.EscrowToBankAccount;
                        if (_dbEscrowToBankAccount != null)
                        {
                            dbEscrowToBank = _dbEscrowToBankAccount;
                            _dbEscrowToBankAccount.MonthlyBudgetDeposits -= _dbMonthlyAmount;
                            DbBankAccount.MonthlyBudgetWithdrawals -= _dbMonthlyAmount;
                        }
                        decimal dbEscrowBalance = 0;
                        if (_dbEscrowBalance != null)
                            dbEscrowBalance = (decimal)_dbEscrowBalance;

                        dbEscrowToBank.EscrowBalance -= dbEscrowBalance;
                    }
                    else
                    {
                        DbBankAccount.MonthlyBudgetWithdrawals -= _dbMonthlyAmount;
                    }
                    break;
                case BudgetItemTypes.Transfer:
                    DbBankAccount.MonthlyBudgetWithdrawals -= _dbMonthlyAmount;
                    if (DbTransferToBankId != null)
                    {
                        DbTransferToBankAccount =
                            AppGlobals.DataRepository.GetBankAccount((int)DbTransferToBankId, false);
                        DbTransferToBankAccount.MonthlyBudgetDeposits -= _dbMonthlyAmount;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var result = AppGlobals.DataRepository.DeleteBudgetItem(Id, DbBankAccount, DbTransferToBankAccount, _dbEscrowToBankAccount);

            if (result)
                DbBankAccount = DbTransferToBankAccount = _dbEscrowToBankAccount = null;

            return result;
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
