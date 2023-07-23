using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public interface IBudgetItemView : IDbMaintenanceView
    {
        void SetViewType();

        void ShowMonthlyStatsControls(bool show = true);

        bool AddAdjustment(BudgetItem budgetItem);
    }

    public class YearlyHistoryFilter
    {
        public ViewModelInput ViewModelInput { get; set; }
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

        private double _amount;

        public double Amount
        {
            get => _amount;
            set
            {
                if (_amount == value)
                    return;

                _amount = value;

                SetViewMode();
                _registerAffected = true;
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
                _registerAffected = true;
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
                _registerAffected = true;
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

        private AutoFillValue _transferToBankAccountAutoFillValue;

        public AutoFillValue TransferToBankAccountAutoFillValue
        {
            get => _transferToBankAccountAutoFillValue;
            set
            {
                if (_transferToBankAccountAutoFillValue == value)
                    return;

                _transferToBankAccountAutoFillValue = value;
                OnPropertyChanged(null, false);
            }
        }


        private double _monthlyAmount;

        public double MonthlyAmount
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

        private double _yearlyAmount;

        public double YearlyAmount
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

        private double _currentMonthAmount;

        public double CurrentMonthAmount
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


        private double _currentMonthPercent;

        public double CurrentMonthPercent
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


        private double _monthToDatePercent;

        public double MonthToDatePercent
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

        private double _monthlyPercentDifference;

        public double MonthlyPercentDifference
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

        private double _monthlyAmountRemaining;

        public double MonthlyAmountRemaining
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

        public bool TransferToBankVisible { get; set; }

        public ViewModelInput ViewModelInput { get; set; }

        public bool FromRegisterGrid { get; private set; }

        public RelayCommand AddAdjustmentCommand { get; }

        private IBudgetItemView _view;
        private bool _loading;
        private double _dbMonthlyAmount;
        private DateTime _dbStartDate;
        private bool _registerAffected;
        private BankAccount _newTransferToBankAccount;
        private List<BankAccountRegisterItem> _newBankAccountRegisterItems;
        private List<BankAccountRegisterItem> _bankAccountRegisterItemsToDelete;
        private BudgetItemTypes? _lockBudgetItemType;
        private BudgetItem _budgetItemHistoryFilter;
        private YearlyHistoryFilter _yearlyHistoryFilter = new YearlyHistoryFilter();

        private LookupDefinition<BudgetPeriodHistoryLookup, BudgetPeriodHistory>
            _periodHistoryLookupDefinition =
                AppGlobals.LookupContext.BudgetPeriodLookup.Clone();
        public BudgetItemViewModel()
        {
            DateControlsEnabled = BudgetItemTypeEnabled = true;

            AddAdjustmentCommand = new RelayCommand(AddAdjustment);

            //_periodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.PeriodEndingDate, p => p.PeriodEndingDate);
            //_periodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.ProjectedAmount, p => p.ProjectedAmount);
            //_periodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.ActualAmount, p => p.ActualAmount);

            //var table = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(AppGlobals.LookupContext
            //    .BudgetPeriodHistory.TableName);
            //var projectedAmountField = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(AppGlobals
            //    .LookupContext.BudgetPeriodHistory.GetFieldDefinition(p => p.ProjectedAmount).FieldName);
            //var actualAmountField = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(AppGlobals
            //    .LookupContext.BudgetPeriodHistory.GetFieldDefinition(p => p.ActualAmount).FieldName);

            //var formula = $"{table}.{projectedAmountField} - {table}.{actualAmountField}";
            //_periodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.Difference, formula)
            //    .HasDecimalFieldType(DecimalFieldTypes.Currency)
            //    .DoShowNegativeValuesInRed();

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
                FromRegisterGrid = viewModelInput.FromRegisterGrid;
                viewModelInput.FromRegisterGrid = false;
            }
            else
            {
                ViewModelInput = new ViewModelInput();
            }

            _yearlyHistoryFilter.ViewModelInput = ViewModelInput;
            
            AppGlobals.MainViewModel.BudgetItemViewModels.Add(this);
            _lockBudgetItemType = ViewModelInput.LockBudgetItemType;
            ViewModelInput.LockBudgetItemType = null;

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

            //if (LookupAddViewArgs != null && LookupAddViewArgs.LookupReadOnlyMode
            
            _loading = false;

            base.Initialize();
        }

        protected override void ClearData()
        {
            _loading = true;

            Id = 0;

            if (_lockBudgetItemType == null)
            {
                BudgetItemTypeEnabled = true;
                BudgetItemType = BudgetItemTypes.Expense;
            }
            else
            {
                BudgetItemTypeEnabled = false;
                BudgetItemType = _lockBudgetItemType.Value;
            }

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
            Amount = 0;
            RecurringPeriod = 1;
            RecurringType = BudgetItemRecurringTypes.Months;
            StartingDate = _dbStartDate = DateTime.Today;
            EndingDate = null;
            _dbMonthlyAmount = MonthlyAmount = 0;

            TransferToBankAccountAutoFillValue = null;
            DbTransferToBankId = 0;
            DateControlsEnabled = true;

            CurrentMonthEnding = new DateTime(DateTime.Today.Year, DateTime.Today.Month,
                DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            CurrentMonthAmount = 0;
            CurrentMonthPercent = 0;

            LastCompletedDate = null;

            MonthlyLookupCommand = GetLookupCommand(LookupCommands.Clear);
            YearlyLookupCommand = GetLookupCommand(LookupCommands.Clear);
            HistoryLookupCommand = GetLookupCommand(LookupCommands.Clear);

            AddAdjustmentCommand.IsEnabled = false;

            _loading = false;

            SetViewMode();
            _registerAffected = false;
        }

        private void SetViewMode()
        {
            if (_loading)
                return;

            TransferToBankVisible = false;

            switch (BudgetItemType)
            {
                case BudgetItemTypes.Income:
                case BudgetItemTypes.Expense:
                    break;
                case BudgetItemTypes.Transfer:
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

            RecalculateBudgetItem(GetBudgetItem());
        }

        public void RecalculateBudgetItem()
        {
            var budgetItem = AppGlobals.DataRepository.GetBudgetItem(Id);

            if (budgetItem != null)
            {
                var existingDirtyFlag = RecordDirty;

                CurrentMonthAmount = budgetItem.CurrentMonthAmount;
                LastCompletedDate = budgetItem.LastCompletedDate;
                CurrentMonthEnding = budgetItem.CurrentMonthEnding;
                StartingDate = budgetItem.StartingDate;
                if (StartingDate != null)
                    _dbStartDate = StartingDate.Value;

                RecalculateBudgetItem(budgetItem);
                RecordDirty = existingDirtyFlag;
            }
        }

        private void RecalculateBudgetItem(BudgetItem budgetItem)
        {
            var budgetItemData = new BudgetItemProcessorData
            {
                BudgetItem = budgetItem
            };

            BudgetItemProcessor.CalculateBudgetItem(budgetItemData);

            MonthlyAmount = budgetItemData.BudgetItem.MonthlyAmount;
            YearlyAmount = budgetItemData.YearlyAmount;
            CurrentMonthPercent = budgetItemData.CurrentMonthPercent;
            MonthToDatePercent = budgetItemData.MonthToDatePercent;
            MonthlyPercentDifference = budgetItemData.MonthlyPercentDifference;
            MonthlyAmountRemaining = budgetItemData.MonthlyAmountRemaining;
        }

        protected override BudgetItem PopulatePrimaryKeyControls(BudgetItem newEntity, PrimaryKeyValue primaryKeyValue)
        {
            _loading = true;
            Id = newEntity.Id;
            IQueryable<BudgetItem> query = AppGlobals.DataRepository.GetDataContext().GetTable<BudgetItem>();

            query = query.Include(i => i.BankAccount)
                .Include(i => i.TransferToBankAccount);

            var budgetItem = query.FirstOrDefault(p => p.Id == Id);
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, budgetItem.Description);

            Amount = budgetItem.Amount;
            _dbMonthlyAmount = budgetItem.MonthlyAmount;
            DbBankAccountId = budgetItem.BankAccountId;
            DbTransferToBankId = budgetItem.TransferToBankAccountId;

            _budgetItemHistoryFilter = budgetItem;
            ViewModelInput.HistoryFilterBudgetItem = budgetItem;

            //ReadOnlyMode = AppGlobals.MainViewModel.BudgetItemViewModels.Any(a => a != this && a.Id == Id);
            BudgetItemTypeEnabled = false;
            StartingDate = budgetItem.StartingDate;
            if (StartingDate != null)
                _dbStartDate = StartingDate.Value;

            if (StartingDate == null)
            {
                DateControlsEnabled = false;
            }

            MonthlyLookupDefinition.FilterDefinition.ClearFixedFilters();
            MonthlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.PeriodType,
                Conditions.Equals, (int)PeriodHistoryTypes.Monthly);
            MonthlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BudgetItemId,
                Conditions.Equals, Id);

            MonthlyLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput);

            YearlyLookupDefinition.FilterDefinition.ClearFixedFilters();
            YearlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.PeriodType,
                Conditions.Equals, (int)PeriodHistoryTypes.Yearly);
            YearlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BudgetItemId,
                Conditions.Equals, Id);

            YearlyLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, _yearlyHistoryFilter);

            HistoryLookupDefinition.FilterDefinition.ClearFixedFilters();
            HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BudgetItemId, Conditions.Equals, Id);

            HistoryLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput);

            AddAdjustmentCommand.IsEnabled = !ReadOnlyMode;

            _loading = false;
            _registerAffected = false;
            return budgetItem;
        }

        private void AddAdjustment()
        {
            var keyDown = Processor.IsMaintenanceKeyDown(MaintenanceKey.Alt);
            if (RecordDirty)
                if (DoSave() != DbMaintenanceResults.Success)
                    return;
            
            var budgetItem = GetBudgetItem();
            budgetItem.BankAccountId = DbBankAccountId;

            if (_view.AddAdjustment(budgetItem))
            {
                RecalculateBudgetItem();
                var primaryKeyValue = AppGlobals.LookupContext.BudgetItems.GetPrimaryKeyValueFromEntity(budgetItem);
                MonthlyLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);
                YearlyLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);
                HistoryLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

                if (AppGlobals.MainViewModel != null)
                    AppGlobals.MainViewModel.RefreshView();
            }

            if (!keyDown)
            {
                View.ResetViewForNewRecord();
            }
        }

        protected override void LoadFromEntity(BudgetItem entity)
        {
            _loading = true;

            BudgetItemType = (BudgetItemTypes)entity.Type;
            BankAutoFillValue =
                new AutoFillValue(
                    AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(entity.BankAccount),
                    entity.BankAccount.Description);

            RecurringPeriod = entity.RecurringPeriod;
            RecurringType = (BudgetItemRecurringTypes)entity.RecurringType;
            EndingDate = entity.EndingDate;
            MonthlyAmount = entity.MonthlyAmount;
            CurrentMonthAmount = entity.CurrentMonthAmount;
            CurrentMonthEnding = entity.CurrentMonthEnding;
            Notes = entity.Notes;
            LastCompletedDate = entity.LastCompletedDate;

            if (entity.TransferToBankAccount != null)
            {
                TransferToBankAccountAutoFillValue = new AutoFillValue(
                    AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(entity.TransferToBankAccount),
                    entity.TransferToBankAccount.Description);
            }

            _loading = false;
            SetViewMode();
            _registerAffected = false;

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
            DbBankAccount = DbTransferToBankAccount = null;
            _newBankAccountRegisterItems = null;
            _bankAccountRegisterItemsToDelete = null;

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
            if (newBankAccountId != 0)
            {
                newBankAccount = AppGlobals.DataRepository.GetBankAccount(newBankAccountId, false);

                if (newTransferToBankAccountId != null)
                {
                    _newTransferToBankAccount = AppGlobals.DataRepository.GetBankAccount((int)newTransferToBankAccountId, false);
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
                            if (newBankAccount != null)
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
                            DbBankAccount.MonthlyBudgetWithdrawals -= _dbMonthlyAmount;

                            if (newBankAccount != null)
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


                if (BudgetItemType == BudgetItemTypes.Transfer)
                {
                    if (_newTransferToBankAccount != null && newBankAccount != null)
                    {
                        //DbBankAccount is Old Transfer From Bank Account.
                        DbBankAccount = AppGlobals.DataRepository.GetBankAccount(DbBankAccountId, false);

                        if (DbTransferToBankId != null)
                            DbTransferToBankAccount = AppGlobals.DataRepository.GetBankAccount((int)DbTransferToBankId, false);

                        if (newBankAccountId == DbBankAccountId || DbBankAccountId == 0)
                        {
                            //Same transfer from (new) bank account
                            newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount - _dbMonthlyAmount;
                            if (_newTransferToBankAccount.Id == DbTransferToBankId || DbTransferToBankAccount == null)
                            {
                                //Same new transfer to bank account.
                                _newTransferToBankAccount.MonthlyBudgetDeposits += MonthlyAmount - _dbMonthlyAmount;
                            }
                            else
                            {
                                //Different transfer to bank account.
                                DbTransferToBankAccount.MonthlyBudgetDeposits -= _dbMonthlyAmount;
                                _newTransferToBankAccount.MonthlyBudgetDeposits += MonthlyAmount;
                            }
                        }
                        else
                        {
                            //Different transfer from (new) bank account
                            newBankAccount.MonthlyBudgetWithdrawals += MonthlyAmount;
                            var swap = false;
                            if (DbTransferToBankAccount != null && _newTransferToBankAccount.Id != DbTransferToBankAccount.Id)
                            {
                                //Different transfer to bank account.
                                _newTransferToBankAccount.MonthlyBudgetDeposits += MonthlyAmount;
                                if (newBankAccount.Id == DbTransferToBankAccount.Id)
                                {
                                    //Swap.
                                    swap = true;
                                    _newTransferToBankAccount.MonthlyBudgetWithdrawals -= _dbMonthlyAmount;
                                    newBankAccount.MonthlyBudgetDeposits -= _dbMonthlyAmount;
                                }
                                else if (DbTransferToBankAccount.Id != _newTransferToBankAccount.Id)
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
            budgetItem.TransferToBankAccountId = newTransferToBankAccountId;
            budgetItem.TransferToBankAccount = _newTransferToBankAccount;
            budgetItem.BankAccountId = newBankAccountId;
            if (RecordDirty)
            {
                if (DbBankAccountId == 0)
                {
                    _bankAccountRegisterItemsToDelete = new List<BankAccountRegisterItem>();
                }
                else
                {
                    var existingBankAccount = AppGlobals.DataRepository.GetBankAccount(DbBankAccountId);
                    if (existingBankAccount.RegisterItems == null)
                    {
                        _bankAccountRegisterItemsToDelete = new List<BankAccountRegisterItem>();
                    }
                    else
                    {
                        if (StartingDate == null)
                        {
                            _bankAccountRegisterItemsToDelete = existingBankAccount.RegisterItems
                                .Where(w => w.BudgetItemId == Id && (BankAccountRegisterItemTypes)w.ItemType == BankAccountRegisterItemTypes.BudgetItem).ToList();
                        }
                        else
                        {
                            _bankAccountRegisterItemsToDelete = existingBankAccount.RegisterItems
                                .Where(w => w.BudgetItemId == Id && w.ItemDate >= budgetItem.StartingDate).ToList();
                        }
                    }

                    if (DbTransferToBankId != null)
                    {
                        var existingDbTransferBankAccount =
                        AppGlobals.DataRepository.GetBankAccount(DbTransferToBankId.Value);

                        if (StartingDate == null)
                        {
                            _bankAccountRegisterItemsToDelete.AddRange(existingDbTransferBankAccount.RegisterItems
                                .Where(w => w.BudgetItemId == Id && (BankAccountRegisterItemTypes)w.ItemType == BankAccountRegisterItemTypes.BudgetItem));

                        }
                        else
                        {
                            _bankAccountRegisterItemsToDelete.AddRange(existingDbTransferBankAccount.RegisterItems
                                .Where(w => w.BudgetItemId == Id && w.ItemDate >= budgetItem.StartingDate));
                        }
                    }
                    foreach (var registerItem in _bankAccountRegisterItemsToDelete)
                    {
                        registerItem.BankAccount = null;
                        registerItem.BudgetItem = null;
                    }
                }

                if (newBankAccount != null)
                {
                    var existingRegisterItems =
                        AppGlobals.DataRepository.GetRegisterItemsForBankAccount(newBankAccount.Id);

                    if (existingRegisterItems.Any())
                    {

                        _newBankAccountRegisterItems =
                            BudgetItemProcessor.GenerateBankAccountRegisterItems(budgetItem.BankAccountId, budgetItem,
                                newBankAccount.LastGenerationDate).ToList();

                        if (_newBankAccountRegisterItems.Any())
                        {
                            StartingDate = budgetItem.StartingDate;
                        }
                    }
                }
            }

            if (DbBankAccountId == newTransferToBankAccountId || DbBankAccountId == newBankAccountId)
            {
                DbBankAccount = null;
            }

            if (newBankAccount != null)
            {
                newBankAccount.RegisterItems = null;
                budgetItem.BankAccount = newBankAccount;
            }

            if (DbTransferToBankId == newBankAccountId || DbTransferToBankId == newTransferToBankAccountId)
            {
                DbTransferToBankAccount = null;
            }

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
                Type = (byte)BudgetItemType,
                Amount = Amount,
                RecurringPeriod = RecurringPeriod == 0 ? 1 : RecurringPeriod,
                RecurringType = (byte)RecurringType,
                StartingDate = StartingDate,
                EndingDate = EndingDate,
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
            if (BudgetItemType == BudgetItemTypes.Transfer)
            {
                if (!TransferToBankAccountAutoFillValue.IsValid())
                {
                    var message = "Transfer To Bank Account must be a valid Bank Account.";
                    OnValidationFail(
                        AppGlobals.LookupContext.BudgetItems.GetFieldDefinition(p => p.TransferToBankAccountId),
                        message,
                        "Invalid Transfer To Bank Account");
                    return false;
                }
                else if (AppGlobals.LookupContext.BankAccounts
                    .GetEntityFromPrimaryKeyValue(TransferToBankAccountAutoFillValue.PrimaryKeyValue).Id == AppGlobals
                    .LookupContext.BankAccounts.GetEntityFromPrimaryKeyValue(BankAutoFillValue.PrimaryKeyValue).Id)
                {
                    var message = "Transfer To Bank Account cannot be the same as the Bank Account.";
                    OnValidationFail(
                        AppGlobals.LookupContext.BudgetItems.GetFieldDefinition(p => p.TransferToBankAccountId),
                        message,
                        "Invalid Transfer To Bank Account");
                    return false;
                }
            }

            if (!BankAutoFillValue.IsValid())
            {
                var message = "Invalid Bank Account";
                OnValidationFail(
                    AppGlobals.LookupContext.BudgetItems.GetFieldDefinition(p => p.BankAccountId),
                    message,
                    "Invalid Bank Account");
                return false;

            }

            var reconciledMessageShown = false;
            if (_newBankAccountRegisterItems != null && !_newBankAccountRegisterItems.Any() &&
                entity.StartingDate == _dbStartDate && _registerAffected)
            {
                var message =
                    $"No register items will be updated for this Budget Item.  You must set the Starting Date to be earlier than {entity.StartingDate} in order to update the Bank Register Grid.  Do you wish to continue?";

                if (!Processor.ShowYesNoMessage(message, "Budget Item Being Modified", true))
                    return false;
            }

            foreach (var bankAccountViewModel in AppGlobals.MainViewModel.BankAccountViewModels)
            {
                if (!reconciledMessageShown && bankAccountViewModel.IsBeingReconciled(Id))
                {
                    reconciledMessageShown = true;
                    var message =
                        "This budget item is currently being reconciled.  If you continue, you will loose all your changes to this budget item in the Register.  Do you wish to continue?";

                    if (!Processor.ShowYesNoMessage(message, "Budget Item Being Modified", true))
                        return false;
                }
            }

            return base.ValidateEntity(entity);
        }

        protected override bool SaveEntity(BudgetItem entity)
        {
            var result = AppGlobals.DataRepository.SaveBudgetItem(entity, DbBankAccount, DbTransferToBankAccount, 
                _newBankAccountRegisterItems, _bankAccountRegisterItemsToDelete);

            if (result && RecalcRegister(entity.BankAccount))
            {
                foreach (var bankAccountViewModel in AppGlobals.MainViewModel.BankAccountViewModels)
                {
                    bankAccountViewModel.RefreshAfterBudgetItemSave(entity, _newBankAccountRegisterItems, StartingDate);
                }

                if (entity.BankAccountId != DbBankAccountId && LookupAddViewArgs != null)
                    PopulatePrimaryKeyControls(entity,
                        AppGlobals.LookupContext.BudgetItems.GetPrimaryKeyValueFromEntity(entity));

            }

            if (AppGlobals.MainViewModel != null)
                    AppGlobals.MainViewModel.RefreshView();

            return result;
        }

        private bool RecalcRegister(BankAccount bankAccount)
        {
            if (_newBankAccountRegisterItems == null && _bankAccountRegisterItemsToDelete == null)
                return true;

            if (!RecalcBankAccountRegister(bankAccount))
                return false;

            if (!RecalcBankAccountRegister(DbBankAccount))
                return false;

            if (!RecalcBankAccountRegister(_newTransferToBankAccount))
                return false;

            if (!RecalcBankAccountRegister(DbTransferToBankAccount))
                return false;

            return true;
        }

        private bool RecalcBankAccountRegister(BankAccount bankAccount)
        {
            if (bankAccount == null)
                return true;

            bankAccount = AppGlobals.DataRepository.GetBankAccount(bankAccount.Id, false);

            var registerItems = AppGlobals.DataRepository.GetRegisterItemsForBankAccount(bankAccount.Id).ToList();

            if (!registerItems.Any())
                return true;

            var newBalance = bankAccount.CurrentBalance;
            var lowestBalance = newBalance;
            DateTime? lowestBalanceDate = null;

            foreach (var registerItem in registerItems)
            {
                if (registerItem.ItemDate.Month == 1 && registerItem.ItemDate.Day == 3)
                {
                    
                }
                if (lowestBalanceDate == null)
                    lowestBalanceDate = registerItem.ItemDate.AddDays(-1);

                newBalance += registerItem.ProjectedAmount;

                if (newBalance < lowestBalance)
                {
                    lowestBalance = newBalance;
                    lowestBalanceDate = registerItem.ItemDate;
                }
            }

            bankAccount.ProjectedEndingBalance = newBalance;
            bankAccount.ProjectedLowestBalanceAmount = lowestBalance;
            bankAccount.ProjectedLowestBalanceDate = lowestBalanceDate;

            return AppGlobals.DataRepository.SaveBankAccount(bankAccount);
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
                    DbBankAccount.MonthlyBudgetWithdrawals -= _dbMonthlyAmount;
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
            var result = AppGlobals.DataRepository.DeleteBudgetItem(Id, DbBankAccount, DbTransferToBankAccount);

            if (result)
            {
                foreach (var bankAccountViewModel in AppGlobals.MainViewModel.BankAccountViewModels)
                {
                    bankAccountViewModel.DeleteBudgetItem(Id);
                }
                DbBankAccount = DbTransferToBankAccount = null;
            }

            return result;
        }

        public override void OnTablesDeleted(DeleteTables deleteTables)
        {
            var bankRegisterTable = deleteTables.Tables.FirstOrDefault(p =>
                p.ChildField.TableDefinition == AppGlobals.LookupContext.BankAccountRegisterItems);

            if (bankRegisterTable != null)
            {
                foreach (var bankAccountViewModel in AppGlobals.MainViewModel.BankAccountViewModels)
                {
                    bankAccountViewModel.RefreshFromDb();
                }
            }
            base.OnTablesDeleted(deleteTables);
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                AppGlobals.MainViewModel.BudgetItemViewModels.Remove(this);
        }

        protected override void OnPropertyChanged(string propertyName = null, bool raiseDirtyFlag = true)
        {
            if (raiseDirtyFlag)
            {
                
            }
            base.OnPropertyChanged(propertyName, raiseDirtyFlag);
        }
    }
}
