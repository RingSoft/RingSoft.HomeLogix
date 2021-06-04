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
using System.Diagnostics;
using System.Linq;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public interface IBankAccountView : IDbMaintenanceView
    {
        void EnableRegisterGrid(bool value);

        DateTime? GetGenerateToDate(DateTime nextGenerateToDate);

        void ShowActualAmountDetailsWindow(ActualAmountCellProps actualAmountCellProps);

        bool ShowBankAccountMiscWindow(BankAccountRegisterItem registerItem, ViewModelInput viewModelInput);

        object OwnerWindow { get; }
    }

    public class CompletedRegisterData
    {
        public List<BudgetItem> BudgetItems { get; set; } = new List<BudgetItem>();

        public List<BankAccountRegisterItem> CompletedRegisterItems { get; set; } = new List<BankAccountRegisterItem>();

        public List<History> NewHistoryRecords { get; set; } = new List<History>();

        public List<SourceHistory> NewSourceHistoryRecords { get; set; } = new List<SourceHistory>();

        public List<BudgetPeriodHistory> BudgetPeriodHistoryRecords { get; set; } = new List<BudgetPeriodHistory>();

        public List<BankAccountPeriodHistory> BankAccountPeriodHistoryRecords { get; set; } =
            new List<BankAccountPeriodHistory>();
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

        private decimal _newProjectedEndingBalance;

        public decimal NewProjectedEndingBalance
        {
            get => _newProjectedEndingBalance;
            set
            {
                if (_newProjectedEndingBalance == value)
                    return;


                _newProjectedEndingBalance = value;
                OnPropertyChanged(nameof(NewProjectedEndingBalance), false);
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
                OnPropertyChanged(nameof(ProjectedEndingBalanceDifference), false);
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
                OnPropertyChanged(nameof(ProjectedLowestBalanceDate), false);
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
                OnPropertyChanged(nameof(ProjectedLowestBalanceAmount), false);
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
                OnPropertyChanged(nameof(MonthlyBudgetDeposits), false);
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
                OnPropertyChanged(nameof(MonthlyBudgetWithdrawals), false);
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
                OnPropertyChanged(nameof(MonthlyBudgetDifference), false);
            }
        }

        private LookupDefinition<BudgetItemLookup, BudgetItem> _budgetItemsLookupDefinition;

        public LookupDefinition<BudgetItemLookup, BudgetItem> BudgetItemsLookupDefinition
        {
            get => _budgetItemsLookupDefinition;
            set
            {
                if (_budgetItemsLookupDefinition == value)
                    return;

                _budgetItemsLookupDefinition = value;
                OnPropertyChanged(nameof(BudgetItemsLookupDefinition), false);
            }
        }

        private LookupCommand _budgetItemsLookupCommand;

        public LookupCommand BudgetItemsLookupCommand
        {
            get => _budgetItemsLookupCommand;
            set
            {
                if (_budgetItemsLookupCommand == value)
                    return;

                _budgetItemsLookupCommand = value;
                OnPropertyChanged(nameof(BudgetItemsLookupCommand), false);
            }
        }

        private LookupDataSourceChanged _budgetItemsDataSourceChanged;

        public LookupDataSourceChanged BudgetItemsDataSourceChanged
        {
            get => _budgetItemsDataSourceChanged;
            set
            {
                if (_budgetItemsDataSourceChanged == value)
                    return;

                _budgetItemsDataSourceChanged = value;
                OnPropertyChanged(nameof(BudgetItemsDataSourceChanged), false);
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

        private DateTime? _lastGenerationDate;

        public DateTime? LastGenerationDate
        {
            get => _lastGenerationDate;
            set
            {
                if (value == null)
                {
                    var newDate = DateTime.Today;
                    _lastGenerationDate =
                        newDate.AddDays(DateTime.DaysInMonth(newDate.Year, newDate.Month) - newDate.Day);
                }
                else
                {
                    _lastGenerationDate = value;
                }
                OnPropertyChanged(nameof(LastGenerationDate), false);
            }
        }

        private LookupDefinition<BankAccountPeriodHistoryLookup, BankAccountPeriodHistory> _monthlyLookupDefinition;

        public LookupDefinition<BankAccountPeriodHistoryLookup, BankAccountPeriodHistory> MonthlyLookupDefinition
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

        private LookupDefinition<BankAccountPeriodHistoryLookup, BankAccountPeriodHistory> _yearlyLookupDefinition;

        public LookupDefinition<BankAccountPeriodHistoryLookup, BankAccountPeriodHistory> YearlyLookupDefinition
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

        #endregion

        public ViewModelInput ViewModelInput { get; set; }

        public RelayCommand AddNewRegisterItemCommand { get; }

        public RelayCommand GenerateRegisterItemsFromBudgetCommand { get; }

        public RelayCommand BudgetItemsAddModifyCommand { get; }

        public List<BankAccountRegisterItemAmountDetail> RegisterDetails { get; } =
            new List<BankAccountRegisterItemAmountDetail>();

        private bool _loading;
        private decimal _dbCurrentBalance;

        private LookupDefinition<BankAccountPeriodHistoryLookup, BankAccountPeriodHistory> _periodHistoryLookupDefinition;

        public BankAccountViewModel()
        {
            AddNewRegisterItemCommand = new RelayCommand(AddNewRegisterItem);

            GenerateRegisterItemsFromBudgetCommand = new RelayCommand(GenerateRegisterItemsFromBudget);

            BudgetItemsAddModifyCommand = new RelayCommand(OnAddModifyBudgetItems);

            LastGenerationDate = null;
        }

        protected override void Initialize()
        {
            BankAccountView = View as IBankAccountView;
            if (BankAccountView == null)
                throw new Exception($"Bank Account View interface must be of type '{nameof(IBankAccountView)}'.");

            BankAccountView.EnableRegisterGrid(false);

            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is ViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
            }
            else
            {
                ViewModelInput = new ViewModelInput();
            }
            ViewModelInput.BankAccountViewModels.Add(this);

            _periodHistoryLookupDefinition =
                new LookupDefinition<BankAccountPeriodHistoryLookup, BankAccountPeriodHistory>(AppGlobals.LookupContext
                    .BankAccountPeriodHistory);

            _periodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.PeriodEndingDate, p => p.PeriodEndingDate);
            _periodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.TotalDeposits, p => p.TotalDeposits);
            _periodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.TotalWithdrawals, p => p.TotalWithdrawals);

            var table = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(AppGlobals.LookupContext
                .BankAccountPeriodHistory.TableName);
            var depositField = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(AppGlobals
                .LookupContext.BankAccountPeriodHistory.GetFieldDefinition(p => p.TotalDeposits).FieldName);
            var withdrawalsField = AppGlobals.LookupContext.DataProcessor.SqlGenerator.FormatSqlObject(AppGlobals
                .LookupContext.BankAccountPeriodHistory.GetFieldDefinition(p => p.TotalWithdrawals).FieldName);

            var formula = $"{table}.{depositField} - {table}.{withdrawalsField}";
            _periodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.Difference, formula)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            _periodHistoryLookupDefinition.InitialOrderByType = OrderByTypes.Descending;

            MonthlyLookupDefinition = _periodHistoryLookupDefinition.Clone();
            YearlyLookupDefinition = _periodHistoryLookupDefinition.Clone();
            HistoryLookupDefinition = AppGlobals.LookupContext.HistoryLookup.Clone();

            BudgetItemsLookupDefinition = AppGlobals.LookupContext.BudgetItemsLookup.Clone();
            BudgetItemsLookupDefinition.AddVisibleColumnDefinition(p => p.MonthlyAmount, p => p.MonthlyAmount);

            RegisterGridManager = new BankAccountRegisterGridManager(this);

            base.Initialize();
        }

        protected override void ClearData()
        {
            _loading = true;

            Id = 0;
            CurrentProjectedEndingBalance = 0;
            _dbCurrentBalance = CurrentBalance = 0;
            NewProjectedEndingBalance = 0;
            ProjectedEndingBalanceDifference = 0;
            ProjectedLowestBalanceDate = null;
            ProjectedLowestBalanceAmount = 0;
            
            MonthlyBudgetDeposits = 0;
            MonthlyBudgetWithdrawals = 0;
            MonthlyBudgetDifference = 0;
            
            Notes = string.Empty;
            LastGenerationDate = null;

            RegisterGridManager.SetupForNewRecord();
            BankAccountView.EnableRegisterGrid(false);

            MonthlyLookupCommand = GetLookupCommand(LookupCommands.Clear);
            YearlyLookupCommand = GetLookupCommand(LookupCommands.Clear);
            HistoryLookupCommand = GetLookupCommand(LookupCommands.Clear);
            BudgetItemsLookupCommand = GetLookupCommand(LookupCommands.Clear);

            AddNewRegisterItemCommand.IsEnabled = GenerateRegisterItemsFromBudgetCommand.IsEnabled = false;

            _loading = false;
        }

        protected override BankAccount PopulatePrimaryKeyControls(BankAccount newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
            var bankAccount = AppGlobals.DataRepository.GetBankAccount(Id);
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, bankAccount.Description);

            MonthlyLookupDefinition.FilterDefinition.ClearFixedFilters();
            MonthlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.PeriodType, 
                Conditions.Equals, (int) PeriodHistoryTypes.Monthly);
            MonthlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BankAccountId, 
                Conditions.Equals, Id);

            MonthlyLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            YearlyLookupDefinition.FilterDefinition.ClearFixedFilters();
            YearlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.PeriodType,
                Conditions.Equals, (int)PeriodHistoryTypes.Yearly);
            YearlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BankAccountId,
                Conditions.Equals, Id);

            YearlyLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            HistoryLookupDefinition.FilterDefinition.ClearFixedFilters();
            HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BankAccountId, Conditions.Equals, Id);

            HistoryLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            BudgetItemsLookupDefinition.FilterDefinition.ClearFixedFilters();
            BudgetItemsLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BankAccountId, Conditions.Equals,
                Id).SetEndLogic(EndLogics.Or);
            BudgetItemsLookupDefinition.FilterDefinition.AddFixedFilter(p => p.TransferToBankAccountId,
                Conditions.Equals, Id);

            BudgetItemsLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput);

            ReadOnlyMode = ViewModelInput.BankAccountViewModels.Any(a => a != this && a.Id == Id);
            AddNewRegisterItemCommand.IsEnabled = GenerateRegisterItemsFromBudgetCommand.IsEnabled = !ReadOnlyMode;

            _dbCurrentBalance = bankAccount.CurrentBalance;

            return bankAccount;
        }

        protected override void LoadFromEntity(BankAccount entity)
        {
            _loading = true;

            CurrentProjectedEndingBalance = entity.ProjectedEndingBalance;
            CurrentBalance = entity.CurrentBalance;
            MonthlyBudgetDeposits = entity.MonthlyBudgetDeposits;
            MonthlyBudgetWithdrawals = entity.MonthlyBudgetWithdrawals;
            
            Notes = entity.Notes;
            LastGenerationDate = entity.LastGenerationDate;
            if (LastGenerationDate == DateTime.MinValue)
                LastGenerationDate = null;

            RegisterGridManager.LoadGrid(entity.RegisterItems);
            BankAccountView.EnableRegisterGrid(RegisterGridManager.Rows.Any());

            _loading = false;
            CalculateTotals();

            if (ReadOnlyMode)
                ControlsGlobals.UserInterface.ShowMessageBox(
                    "This Bank Account is being modified in another window.  Editing not allowed.", "Editing not allowed",
                    RsMessageBoxIcons.Exclamation);
        }

        public void CalculateTotals(bool calculateProjectedBalanceData = true)
        {
            if (_loading)
                return;

            if (calculateProjectedBalanceData)
                RegisterGridManager.CalculateProjectedBalanceData();

            ProjectedEndingBalanceDifference = NewProjectedEndingBalance - CurrentProjectedEndingBalance;
            
            MonthlyBudgetDifference = MonthlyBudgetDeposits - MonthlyBudgetWithdrawals;
        }

        private void RefreshBudgetTotals()
        {
            var bankAccount = AppGlobals.DataRepository.GetBankAccount(Id, false);

            MonthlyBudgetDeposits = bankAccount.MonthlyBudgetDeposits;
            MonthlyBudgetWithdrawals = bankAccount.MonthlyBudgetWithdrawals;

            var newBalance = CurrentBalance - _dbCurrentBalance;
            NewProjectedEndingBalance = bankAccount.ProjectedEndingBalance + newBalance;

            ProjectedLowestBalanceAmount = bankAccount.ProjectedLowestBalanceAmount;
            ProjectedLowestBalanceDate = bankAccount.ProjectedLowestBalanceDate;

            CalculateTotals(false);
        }

        private void AddNewRegisterItem()
        {
            var registerItem = new BankAccountRegisterItem{BankAccountId = Id};
            if (BankAccountView.ShowBankAccountMiscWindow(registerItem, ViewModelInput))
            {
                RegisterGridManager.AddGeneratedRegisterItems(new List<BankAccountRegisterItem> {registerItem});
                CalculateTotals();
            }
        }

        private void GenerateRegisterItemsFromBudget()
        {
            var lastGenerationDate = LastGenerationDate;
            if (lastGenerationDate == null)
            {
                lastGenerationDate = DateTime.Today;
            }
            var generateToDate = BankAccountView.GetGenerateToDate(lastGenerationDate.Value.AddMonths(1));

            if (generateToDate == null)
                return;
            
            var budgetItems = AppGlobals.DataRepository.GetBudgetItemsForBankAccount(Id);
            if (budgetItems == null)
                return;

            var newBankAccount = GetEntityData();

            var registerItems = new List<BankAccountRegisterItem>();

            foreach (var budgetItem in budgetItems)
            {
                registerItems.AddRange(
                    BudgetItemProcessor.GenerateBankAccountRegisterItems(Id, budgetItem, generateToDate.Value));
            }

            var bankAccount = AppGlobals.DataRepository.GetBankAccount(Id, false);
            LastGenerationDate = bankAccount.LastGenerationDate = generateToDate.Value;
            if (!AppGlobals.DataRepository.SaveGeneratedRegisterItems(registerItems,
                budgetItems, null, bankAccount))
                return;

            foreach (var registerItem in registerItems)
            {
                registerItem.BudgetItem = budgetItems.FirstOrDefault(f => f.Id == registerItem.BudgetItemId);
            }

            RegisterGridManager.AddGeneratedRegisterItems(registerItems.Where(w => w.BankAccountId == Id));
            CalculateTotals();
        }

        protected override BankAccount GetEntityData()
        {
            Debug.Assert(LastGenerationDate != null, nameof(LastGenerationDate) + " != null");
            var bankAccount = new BankAccount
            {
                Id = Id,
                Description = KeyAutoFillValue.Text,
                ProjectedEndingBalance = NewProjectedEndingBalance,
                CurrentBalance = CurrentBalance,
                ProjectedLowestBalanceDate = ProjectedLowestBalanceDate,
                ProjectedLowestBalanceAmount = ProjectedLowestBalanceAmount,
                MonthlyBudgetDeposits = MonthlyBudgetDeposits,
                MonthlyBudgetWithdrawals = MonthlyBudgetWithdrawals,
                Notes = Notes,
                LastGenerationDate = (DateTime)LastGenerationDate
            };

            return bankAccount;
        }

        protected override bool SaveEntity(BankAccount entity)
        {
            var completedRows = RegisterGridManager.Rows.OfType<BankAccountRegisterGridRow>().Where(w => w.Completed).ToList();

            var completedRegisterData = new CompletedRegisterData();
            if (!ProcessCompletedRows(completedRegisterData, completedRows))
                return false;

            if (AppGlobals.DataRepository.SaveBankAccount(entity, completedRegisterData))
            {
                if (completedRows.Any())
                {
                    var currentRowIndex = RegisterGridManager.Grid.CurrentRowIndex;
                    BankAccountRegisterGridRow currentRow = null;
                    if (currentRowIndex >= 0)
                    {
                        currentRow = (BankAccountRegisterGridRow) RegisterGridManager.Rows[currentRowIndex];
                        while (currentRow.Completed && currentRowIndex > 0)
                        {
                            currentRowIndex--;
                            currentRow = (BankAccountRegisterGridRow)RegisterGridManager.Rows[currentRowIndex];
                        }
                    }
                    foreach (var completedRow in completedRows)
                    {
                        RegisterGridManager.InternalRemoveRow(completedRow);
                    }

                    CalculateTotals();
                    if (RegisterGridManager.Rows.Any())
                    {
                        var newRowIndex = RegisterGridManager.Rows.IndexOf(currentRow);
                        if (newRowIndex < 0)
                            newRowIndex = 0;
                        RegisterGridManager.Grid.GotoCell(
                            RegisterGridManager.Rows[newRowIndex],
                            RegisterGridManager.Grid.CurrentColumnId);
                    }
                }

                foreach (var budgetItemViewModel in ViewModelInput.BudgetItemViewModels)
                {
                    budgetItemViewModel.RecalculateBudgetItem();
                }
                if (AppGlobals.MainViewModel != null)
                    AppGlobals.MainViewModel.RefreshView();
                return true;
            }

            return false;
        }

        private bool ProcessCompletedRows(CompletedRegisterData completedRegisterData, List<BankAccountRegisterGridRow> completedRows)
        {
            foreach (var completedRow in completedRows)
            {
                var amountDetails = new List<BankAccountRegisterItemAmountDetail>();
                var registerItem = new BankAccountRegisterItem();
                completedRow.SaveToEntity(registerItem, 0, amountDetails);

                var monthEndDate = new DateTime(registerItem.ItemDate.Year, registerItem.ItemDate.Month,
                    DateTime.DaysInMonth(registerItem.ItemDate.Year, registerItem.ItemDate.Month));
                var yearEndDate = new DateTime(registerItem.ItemDate.Year, 12, 31);

                ProcessCompletedBankMonth(completedRegisterData, monthEndDate, completedRow);

                ProcessCompletedBankYear(completedRegisterData, yearEndDate, completedRow);

                int? transferToBankAccountId = null;
                var processBudgetRow = true;
                //var addToHistory = true;
                switch (completedRow.LineType)
                {
                    case BankAccountRegisterItemTypes.BudgetItem:
                        break;
                    case BankAccountRegisterItemTypes.Miscellaneous:
                        break;
                    case BankAccountRegisterItemTypes.TransferToBankAccount:
                        if (completedRow is BankAccountRegisterGridTransferRow transferRow)
                        {
                            var transferRegisterItem =
                                AppGlobals.DataRepository.GetTransferRegisterItem(transferRow.TransferRegisterGuid);

                            if (transferRegisterItem == null)
                            {
                                processBudgetRow = false;
                                //addToHistory = false;
                            }
                            else
                            {
                                if (completedRow.TransactionType == TransactionTypes.Deposit)
                                    transferToBankAccountId = registerItem.BankAccountId;
                                else
                                    transferToBankAccountId = transferRegisterItem.BankAccountId;
                            }
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (processBudgetRow)
                {
                    ProcessCompletedBudget(completedRegisterData, registerItem, monthEndDate, completedRow,
                        yearEndDate);
                }
                completedRegisterData.CompletedRegisterItems.Add(registerItem);

                //if (addToHistory)
                {
                    AddCompletedToHistory(completedRegisterData, registerItem, transferToBankAccountId, completedRow, amountDetails);
                }

                //RegisterDetails.AddRange(amountDetails);
            }
            return true;
        }

        private static void ProcessCompletedBudget(CompletedRegisterData completedRegisterData,
            BankAccountRegisterItem registerItem, DateTime monthEndDate, BankAccountRegisterGridRow completedRow,
            DateTime yearEndDate)
        {
            if (registerItem.BudgetItemId != null && registerItem.BudgetItemId != 0)
            {
                var budgetItem =
                    completedRegisterData.BudgetItems.FirstOrDefault(f => f.Id == registerItem.BudgetItemId);

                if (budgetItem == null)
                {
                    budgetItem = AppGlobals.DataRepository.GetBudgetItem(registerItem.BudgetItemId.Value);
                    completedRegisterData.BudgetItems.Add(budgetItem);
                }

                ProcessCompletedBudgetMonth(completedRegisterData, budgetItem, monthEndDate, completedRow);

                ProcessCompletedBudgetYear(completedRegisterData, budgetItem, yearEndDate, completedRow);
            }
        }

        private static void ProcessCompletedBudgetMonth(CompletedRegisterData completedRegisterData, BudgetItem budgetItem,
            DateTime monthEndDate, BankAccountRegisterGridRow completedRow)
        {
            if (budgetItem.CurrentMonthEnding < monthEndDate || budgetItem.LastCompletedDate == null)
            {
                budgetItem.CurrentMonthEnding = monthEndDate;
                budgetItem.CurrentMonthAmount = 0;
            }

            if (completedRow.ItemDate.Month >= budgetItem.CurrentMonthEnding.Month)
                budgetItem.CurrentMonthAmount += completedRow.ActualAmount.GetValueOrDefault(0);

            if (budgetItem.LastCompletedDate == null ||
                completedRow.ItemDate > budgetItem.LastCompletedDate.GetValueOrDefault(DateTime.Today))
            {
                budgetItem.LastCompletedDate = completedRow.ItemDate;
            }

            var budgetMonthHistory = completedRegisterData.BudgetPeriodHistoryRecords.FirstOrDefault(f =>
                f.BudgetItemId == budgetItem.Id &&
                f.PeriodType == (byte) PeriodHistoryTypes.Monthly && f.PeriodEndingDate == monthEndDate);

            if (budgetMonthHistory == null)
            {
                budgetMonthHistory = AppGlobals.DataRepository.GetBudgetPeriodHistory(budgetItem.Id,
                    PeriodHistoryTypes.Monthly, monthEndDate) ?? new BudgetPeriodHistory
                {
                    BudgetItemId = budgetItem.Id,
                    PeriodType = (byte) PeriodHistoryTypes.Monthly,
                    PeriodEndingDate = monthEndDate
                };

                completedRegisterData.BudgetPeriodHistoryRecords.Add(budgetMonthHistory);
            }

            budgetMonthHistory.ProjectedAmount += completedRow.ProjectedAmount;
            budgetMonthHistory.ActualAmount += completedRow.ActualAmount.GetValueOrDefault(0);
        }

        private static void ProcessCompletedBudgetYear(CompletedRegisterData completedRegisterData, BudgetItem budgetItem,
            DateTime yearEndDate, BankAccountRegisterGridRow completedRow)
        {
            var budgetYearHistory = completedRegisterData.BudgetPeriodHistoryRecords.FirstOrDefault(f =>
                f.BudgetItemId == budgetItem.Id &&
                f.PeriodType == (byte)PeriodHistoryTypes.Yearly && f.PeriodEndingDate == yearEndDate);

            if (budgetYearHistory == null)
            {
                budgetYearHistory = AppGlobals.DataRepository.GetBudgetPeriodHistory(budgetItem.Id,
                    PeriodHistoryTypes.Yearly, yearEndDate) ?? new BudgetPeriodHistory
                {
                    BudgetItemId = budgetItem.Id,
                    PeriodType = (byte)PeriodHistoryTypes.Yearly,
                    PeriodEndingDate = yearEndDate
                };

                completedRegisterData.BudgetPeriodHistoryRecords.Add(budgetYearHistory);
            }

            budgetYearHistory.ProjectedAmount += completedRow.ProjectedAmount;
            budgetYearHistory.ActualAmount += completedRow.ActualAmount.GetValueOrDefault(0);
        }

        private void ProcessCompletedBankMonth(CompletedRegisterData completedRegisterData,
            DateTime monthEndDate, BankAccountRegisterGridRow completedRow)
        {
            var bankMonthHistory = completedRegisterData.BankAccountPeriodHistoryRecords.FirstOrDefault(f =>
                f.BankAccountId == Id && f.PeriodType == (byte) PeriodHistoryTypes.Monthly &&
                f.PeriodEndingDate == monthEndDate);

            if (bankMonthHistory == null)
            {
                bankMonthHistory = AppGlobals.DataRepository.GetBankPeriodHistory(Id,
                    PeriodHistoryTypes.Monthly, monthEndDate) ?? new BankAccountPeriodHistory
                    {
                        BankAccountId = Id,
                        PeriodType = (byte)PeriodHistoryTypes.Monthly,
                        PeriodEndingDate = monthEndDate
                    };

                completedRegisterData.BankAccountPeriodHistoryRecords.Add(bankMonthHistory);
            }

            switch (completedRow.TransactionType)
            {
                case TransactionTypes.Deposit:
                    bankMonthHistory.TotalDeposits += completedRow.ActualAmount.GetValueOrDefault(0);
                    break;
                case TransactionTypes.Withdrawal:
                    bankMonthHistory.TotalWithdrawals += completedRow.ActualAmount.GetValueOrDefault(0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessCompletedBankYear(CompletedRegisterData completedRegisterData,
            DateTime yearEndDate, BankAccountRegisterGridRow completedRow)
        {
            var bankYearHistory = completedRegisterData.BankAccountPeriodHistoryRecords.FirstOrDefault(f =>
                f.BankAccountId == Id && f.PeriodType == (byte)PeriodHistoryTypes.Yearly &&
                f.PeriodEndingDate == yearEndDate);

            if (bankYearHistory == null)
            {
                bankYearHistory = AppGlobals.DataRepository.GetBankPeriodHistory(Id,
                    PeriodHistoryTypes.Yearly, yearEndDate) ?? new BankAccountPeriodHistory
                {
                    BankAccountId = Id,
                    PeriodType = (byte)PeriodHistoryTypes.Yearly,
                    PeriodEndingDate = yearEndDate
                };

                completedRegisterData.BankAccountPeriodHistoryRecords.Add(bankYearHistory);
            }

            switch (completedRow.TransactionType)
            {
                case TransactionTypes.Deposit:
                    bankYearHistory.TotalDeposits += completedRow.ActualAmount.GetValueOrDefault(0);
                    break;
                case TransactionTypes.Withdrawal:
                    bankYearHistory.TotalWithdrawals += completedRow.ActualAmount.GetValueOrDefault(0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddCompletedToHistory(CompletedRegisterData completedRegisterData, BankAccountRegisterItem registerItem,
            int? transferToBankAccountId, BankAccountRegisterGridRow completedRow, List<BankAccountRegisterItemAmountDetail> amountDetails)
        {
            var historyItem = new History
            {
                BankAccountId = Id,
                Date = registerItem.ItemDate,
                ItemType = registerItem.ItemType,
                BudgetItemId = registerItem.BudgetItemId,
                TransferToBankAccountId = transferToBankAccountId,
                Description = registerItem.Description,
                ProjectedAmount = completedRow.ProjectedAmount,
                ActualAmount = completedRow.ActualAmount.GetValueOrDefault(0)
            };
            completedRegisterData.NewHistoryRecords.Add(historyItem);
            var detailId = 1;
            foreach (var amountDetail in amountDetails)
            {
                completedRegisterData.NewSourceHistoryRecords.Add(new SourceHistory
                {
                    HistoryItem = historyItem,
                    DetailId = detailId,
                    SourceId = amountDetail.SourceId,
                    Date = amountDetail.Date,
                    Amount = amountDetail.Amount
                });
                detailId++;
            }
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DataRepository.DeleteBankAccount(Id);
        }

        public void OnAddModifyBudgetItems()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                BudgetItemsLookupCommand = GetLookupCommand(LookupCommands.AddModify);
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                ViewModelInput.BankAccountViewModels.Remove(this);
        }

        public bool IsBeingReconciled(int budgetItemId)
        {
            if (RegisterGridManager.Rows
                .OfType<BankAccountRegisterGridRow>().Any(a => a.Completed
                || a.ActualAmount != null && a.BudgetItemId == budgetItemId))
                return true;

            return false;
        }

        public bool IsBudgetItemInRegisterGrid(int budgetItemId)
        {
            return RegisterGridManager.Rows.OfType<BankAccountRegisterGridRow>()
                .Any(a => a.BudgetItemId == budgetItemId
                && a.LineType != BankAccountRegisterItemTypes.Miscellaneous);
        }

        public void RefreshAfterBudgetItemSave(BudgetItem budgetItem,
            List<BankAccountRegisterItem> newRegisterItems, DateTime? startDate)
        {
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
            var bankAccount = AppGlobals.DataRepository.GetBankAccount(Id);
            RegisterGridManager.LoadGrid(bankAccount.RegisterItems);
            RefreshBudgetTotals();
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
        }

        public void DeleteBudgetItem(int budgetItemId)
        {
            var budgetRows = RegisterGridManager.Rows.OfType<BankAccountRegisterGridRow>()
                .Where(w => w.BudgetItemId == budgetItemId).ToList();

            foreach (var budgetRow in budgetRows)
            {
                RegisterGridManager.InternalRemoveRow(budgetRow);
            }
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
