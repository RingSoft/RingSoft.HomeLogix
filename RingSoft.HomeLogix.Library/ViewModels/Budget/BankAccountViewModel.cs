using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.MobileInterop.PhoneModel;
using RingSoft.Printing.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public enum BankProcesses
    {
        Loading = 0,
        Posting = 1,
        Generating = 2,
    }

    public interface IBankAccountView : IDbMaintenanceView
    {
        void EnableRegisterGrid(bool value);

        DateTime? GetGenerateToDate(DateTime nextGenerateToDate);

        void ShowActualAmountDetailsWindow(ActualAmountCellProps actualAmountCellProps);

        bool ShowBankAccountMiscWindow(BankAccountRegisterItem registerItem, ViewModelInput viewModelInput);

        object OwnerWindow { get; }

        bool ImportFromBank(BankAccountViewModel bankAccountViewModel);

        void LoadBank(BankAccount entity);

        void GenerateTransactions(DateTime generateToDate);

        void PostRegister(CompletedRegisterData completedRegisterData, List<BankAccountRegisterGridRow> completedRows);

        void UpdateStatus(string status);

        void ShowMessageBox(string message, string caption, RsMessageBoxIcons icon);

        void SetInitGridFocus(BankAccountRegisterGridRow row, int columnId);

        void RestartApp();

        void RefreshGrid(BankAccount bankAccount);
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

    public class BankAccountViewModel : DbMaintenanceViewModel<BankAccount>
    {
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

        private TextComboBoxControlSetup _typeSetup;

        public TextComboBoxControlSetup TypeSetup
        {
            get => _typeSetup;
            set
            {
                if (_typeSetup == value)
                {
                    return;
                }
                _typeSetup = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxItem _typeItem;

        public TextComboBoxItem TypeItem
        {
            get => _typeItem;
            set
            {
                if (_typeItem == value)
                {
                    return;
                }
                _typeItem = value;
                CalculateTotals();
                OnPropertyChanged();
            }
        }

        public BankAccountTypes AccountType
        {
            get => (BankAccountTypes) TypeItem.NumericValue;
            set => TypeItem = TypeSetup.GetItem((int) value);
        }

        private bool _typeEnabled;

        public bool TypeEnabled
        {
            get => _typeEnabled;
            set
            {
                if (_typeEnabled == value)
                {
                    return;
                }
                _typeEnabled = value;
                OnPropertyChanged();
            }
        }


        private double _currentProjectedEndingBalance;

        public double CurrentProjectedEndingBalance
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

        private double _currentBalance;

        public double CurrentBalance
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

        private double _newProjectedEndingBalance;

        public double NewProjectedEndingBalance
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

        private double _projectedEndingBalanceDifference;

        public double ProjectedEndingBalanceDifference
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

        private double _projectedLowestBalanceAmount;

        public double ProjectedLowestBalanceAmount
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


        private double _monthlyBudgetDeposits;

        public double MonthlyBudgetDeposits
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

        private double _monthlyBudgetWithdrawals;

        public double MonthlyBudgetWithdrawals
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

        private double _monthlyBudgetDifference;

        public double MonthlyBudgetDifference
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

        private bool _completeAll;
        
        public bool CompleteAll
        {
            get => _completeAll;
            set
            {
                if (_completeAll == value)
                    return;

                _completeAll = value;
                OnPropertyChanged();

                if (_completeGrid && RegisterGridManager != null)
                    RegisterGridManager.CompleteAll(CompleteAll);
            }
        }

        private DateTime? _lastCompletedDate;

        public DateTime? LastCompleteDate
        {
            get => _lastCompletedDate;
            set
            {
                if (_lastCompletedDate == value)
                {
                    return;
                }
                _lastCompletedDate = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public int InitRegisterId { get; private set; }

        public ViewModelInput ViewModelInput { get; set; }

        public RelayCommand AddNewRegisterItemCommand { get; }

        public RelayCommand GenerateRegisterItemsFromBudgetCommand { get; }

        public RelayCommand BudgetItemsAddModifyCommand { get; }

        public RelayCommand ImportTransactionsCommand { get; }

        public List<BankAccountRegisterItemAmountDetail> RegisterDetails { get; } =
            new List<BankAccountRegisterItemAmountDetail>();

        private bool _loading;
        private double _dbCurrentBalance;
        private bool _completeGrid = true;
        private bool _processCompletedRows = true;
        private YearlyHistoryFilter _yearlyHistoryFilter = new YearlyHistoryFilter();
        private bool _recordSaved;
        private DateTime _firstRegisterDate = DateTime.Now;
        private bool _saveEntity = true;
        private bool _doProcessCompletedRows;

        private LookupDefinition<BankAccountPeriodHistoryLookup, BankAccountPeriodHistory> _periodHistoryLookupDefinition;

        public BankAccountViewModel()
        {
            AddNewRegisterItemCommand = new RelayCommand(AddNewRegisterItem);

            GenerateRegisterItemsFromBudgetCommand = new RelayCommand(GenerateRegisterItemsFromBudget);

            BudgetItemsAddModifyCommand = new RelayCommand(OnAddModifyBudgetItems);

            ImportTransactionsCommand = new RelayCommand(ImportTransactions);

            LastGenerationDate = null;
            TypeEnabled = true;

            TablesToDelete.Add(AppGlobals.LookupContext.BankAccountRegisterItems);
            TablesToDelete.Add(AppGlobals.LookupContext.BankAccountRegisterItemAmountDetails);

            PrintProcessingHeader += BankAccountViewModel_PrintProcessingHeader;

            RegisterGridManager = new BankAccountRegisterGridManager(this);
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
            _yearlyHistoryFilter.ViewModelInput = ViewModelInput;
            AppGlobals.MainViewModel.BankAccountViewModels.Add(this);

            TypeSetup = new TextComboBoxControlSetup();
            TypeSetup.LoadFromEnum<BankAccountTypes>();

            _periodHistoryLookupDefinition = AppGlobals.LookupContext.BankPeriodHistoryLookup.Clone();

            _periodHistoryLookupDefinition.InitialOrderByType = OrderByTypes.Descending;

            MonthlyLookupDefinition = _periodHistoryLookupDefinition.Clone();
            YearlyLookupDefinition = _periodHistoryLookupDefinition.Clone();
            
            HistoryLookupDefinition = AppGlobals.LookupContext.HistoryLookup.Clone();
            
            BudgetItemsLookupDefinition = AppGlobals.LookupContext.BudgetItemsLookup.Clone();
            BudgetItemsLookupDefinition.AddVisibleColumnDefinition(p => p.MonthlyAmount, p => p.MonthlyAmount);

            base.Initialize();
        }

        protected override void ClearData()
        {
            _loading = true;

            Id = 0;
            AccountType = BankAccountTypes.Checking;
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

            _completeGrid = false;
            CompleteAll = false;
            _completeGrid = true;

            MonthlyLookupDefinition.SetCommand(GetLookupCommand(LookupCommands.Clear));
            YearlyLookupDefinition.SetCommand(GetLookupCommand(LookupCommands.Clear));
            BudgetItemsLookupDefinition.SetCommand(GetLookupCommand(LookupCommands.Clear));
            HistoryLookupDefinition.SetCommand(GetLookupCommand(LookupCommands.Clear));

            AddNewRegisterItemCommand.IsEnabled = GenerateRegisterItemsFromBudgetCommand.IsEnabled =
                ImportTransactionsCommand.IsEnabled = false;

            TypeEnabled = true;

            _loading = false;
        }

        protected override void PopulatePrimaryKeyControls(BankAccount newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
            //var bankAccount = AppGlobals.DataRepository.GetBankAccount(Id);


            MonthlyLookupDefinition.FilterDefinition.ClearFixedFilters();
            MonthlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.PeriodType, 
                Conditions.Equals, (int) PeriodHistoryTypes.Monthly);
            MonthlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BankAccountId, 
                Conditions.Equals, Id);

            MonthlyLookupDefinition.SetCommand(GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput));

            YearlyLookupDefinition.FilterDefinition.ClearFixedFilters();
            YearlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.PeriodType,
                Conditions.Equals, (int)PeriodHistoryTypes.Yearly);
            YearlyLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BankAccountId,
                Conditions.Equals, Id);

            YearlyLookupDefinition.SetCommand(
                GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, _yearlyHistoryFilter));

            HistoryLookupDefinition.FilterDefinition.ClearFixedFilters();
            HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BankAccountId, Conditions.Equals, Id);

            HistoryLookupDefinition.SetCommand(
                GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput));

            BudgetItemsLookupDefinition.FilterDefinition.ClearFixedFilters();
            BudgetItemsLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BankAccountId, Conditions.Equals,
                Id).SetEndLogic(EndLogics.Or).SetLeftParenthesesCount(1);
            BudgetItemsLookupDefinition.FilterDefinition.AddFixedFilter(p => p.TransferToBankAccountId,
                Conditions.Equals, Id).SetRightParenthesesCount(1);

            BudgetItemsLookupDefinition.SetCommand(
                GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput));
            
            CompleteAll = false;
            TypeEnabled = false;
        }

        protected override BankAccount GetEntityFromDb(BankAccount newEntity, PrimaryKeyValue primaryKeyValue)
        {
            IQueryable<BankAccount> query = SystemGlobals.DataRepository.GetDataContext().GetTable<BankAccount>();
            query = query.Include(i => i.RegisterItems.OrderBy(o => o.ItemDate)
                    .ThenByDescending(t => t.ProjectedAmount))
                .Include(i => i.RegisterItems)
                .ThenInclude(ti => ti.AmountDetails)
                .Include(i => i.RegisterItems)
                .ThenInclude(ti => ti.BudgetItem)
                .ThenInclude(ti => ti.TransferToBankAccount);
            //var bankAccount = AppGlobals.DataRepository.GetEntity(query, p => p.Id == Id);
            var bankAccount = query.FirstOrDefault(p => p.Id == Id);

            ViewModelInput.HistoryFilterBankAccount = bankAccount;

            CurrentProjectedEndingBalance = bankAccount.ProjectedEndingBalance;

            if (_processCompletedRows)
            {
                _dbCurrentBalance = bankAccount.CurrentBalance;
            }

            CalculateTotals();

            //ReadOnlyMode = AppGlobals.MainViewModel.BankAccountViewModels.Any(a => a != this && a.Id == Id);
            AddNewRegisterItemCommand.IsEnabled = GenerateRegisterItemsFromBudgetCommand.IsEnabled =
                ImportTransactionsCommand.IsEnabled = !ReadOnlyMode;

            if (bankAccount.LastCompletedDate.HasValue)
            {
                LastCompleteDate = bankAccount.LastCompletedDate.Value;
            }
            else
            {
                LastCompleteDate = new DateTime().MinDate();
            }

            return bankAccount;

        }

        protected override void LoadFromEntity(BankAccount entity)
        {
            _loading = true;

            if (SystemGlobals.UnitTestMode)
            {
                LoadFromEntityProcedure(entity);
            }
            else
            {
                BankAccountView.LoadBank(entity);
            }
            //LoadFromEntityProcedure(entity);
        }

        public void RefreshFromDb()
        {
            var bankAccount = AppGlobals.DataRepository.GetBankAccount(Id);
            if (bankAccount != null)
            {
                BankAccountView.RefreshGrid(bankAccount);
            }
        }

        public void LoadFromEntityProcedure(BankAccount entity)
        {
            AccountType = (BankAccountTypes) entity.AccountType;
            CurrentBalance = entity.CurrentBalance;
            MonthlyBudgetDeposits = entity.MonthlyBudgetDeposits;
            MonthlyBudgetWithdrawals = entity.MonthlyBudgetWithdrawals;

            Notes = entity.Notes;
            LastGenerationDate = entity.LastGenerationDate;
            if (LastGenerationDate == DateTime.MinValue)
                LastGenerationDate = null;

            RegisterGridManager.LoadGrid(entity.RegisterItems);
            if (RegisterGridManager.Rows.Any())
            {
                var rows = RegisterGridManager.Rows.OfType<BankAccountRegisterGridRow>().OrderBy(p => p.ItemDate);
                _firstRegisterDate = rows.FirstOrDefault().ItemDate;
            }
            BankAccountView.EnableRegisterGrid(RegisterGridManager.Rows.Any());

            _loading = false;
            CalculateTotals();

            if (RegisterGridManager.MonthlyBudgetDeposits != 0)
            {
                MonthlyBudgetDeposits = RegisterGridManager.MonthlyBudgetDeposits;
            }

            if (RegisterGridManager.MonthlyBudgetWithdrawals != 0)
            {
                MonthlyBudgetWithdrawals = RegisterGridManager.MonthlyBudgetWithdrawals;
            }

            MonthlyBudgetDifference = MonthlyBudgetDeposits - MonthlyBudgetWithdrawals;

            if (ReadOnlyMode)
                BankAccountView.ShowMessageBox(
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

        public void RefreshBudgetTotals()
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
            var keyDown = Processor.IsMaintenanceKeyDown(MaintenanceKey.Alt);
            var registerItem = new BankAccountRegisterItem{BankAccountId = Id};
            if (BankAccountView.ShowBankAccountMiscWindow(registerItem, ViewModelInput))
            {
                RegisterGridManager.AddGeneratedRegisterItems(new List<BankAccountRegisterItem> {registerItem});
                CalculateTotals();
                AppGlobals.MainViewModel.RefreshView();
            }

            if (!keyDown)
            {
                View.ResetViewForNewRecord();
            }
        }

        private void GenerateRegisterItemsFromBudget()
        {
            var keyDown = Processor.IsMaintenanceKeyDown(MaintenanceKey.Alt);
            var lastGenerationDate = LastGenerationDate;
            if (lastGenerationDate == null)
            {
                lastGenerationDate = DateTime.Today;
            }
            var generateToDate = BankAccountView.GetGenerateToDate(lastGenerationDate.Value.AddMonths(1));

            //GenerateTransactions(generateToDate);
            if (generateToDate != null) BankAccountView.GenerateTransactions(generateToDate.Value);
            if (!keyDown)
            {
                View.ResetViewForNewRecord();
            }
        }

        public void GenerateTransactions(DateTime? generateToDate)
        {
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

            foreach (var registerItem in registerItems)
            {
                registerItem.BudgetItem = budgetItems.FirstOrDefault(f => f.Id == registerItem.BudgetItemId);
            }

            RegisterGridManager.AddGeneratedRegisterItems(registerItems.Where(w => w.BankAccountId == Id));
            CalculateTotals();
            SetTotals(bankAccount);
            AppGlobals.DataRepository.SaveGeneratedRegisterItems(registerItems,
                budgetItems, null, bankAccount);

            //Peter Ringering - 07/10/2024 04:15:16 PM - E-67
            var bankAccount1 = AppGlobals.DataRepository.GetBankAccount(Id);
            RegisterGridManager.LoadGrid(bankAccount1.RegisterItems);
        }

        private void SetTotals(BankAccount bankAccount)
        {
            bankAccount.CurrentBalance = CurrentBalance;
            bankAccount.ProjectedLowestBalanceDate = ProjectedLowestBalanceDate;
            bankAccount.ProjectedLowestBalanceAmount = ProjectedLowestBalanceAmount;
            bankAccount.MonthlyBudgetDeposits = MonthlyBudgetDeposits;
            bankAccount.MonthlyBudgetWithdrawals = MonthlyBudgetWithdrawals;
            bankAccount.LastGenerationDate = (DateTime)LastGenerationDate;
            bankAccount.ProjectedEndingBalance = NewProjectedEndingBalance;
        }

        protected override BankAccount GetEntityData()
        {
            Debug.Assert(LastGenerationDate != null, nameof(LastGenerationDate) + " != null");
            var bankAccount = new BankAccount
            {
                Id = Id,
                AccountType = (byte)AccountType,
                Description = KeyAutoFillValue.Text,
                CurrentBalance = CurrentBalance,
                ProjectedLowestBalanceDate = ProjectedLowestBalanceDate,
                ProjectedLowestBalanceAmount = ProjectedLowestBalanceAmount,
                MonthlyBudgetDeposits = MonthlyBudgetDeposits,
                MonthlyBudgetWithdrawals = MonthlyBudgetWithdrawals,
                Notes = Notes,
                LastGenerationDate = (DateTime)LastGenerationDate,
                LastCompletedDate = LastCompleteDate
            };

            if (_processCompletedRows)
            {
                bankAccount.ProjectedEndingBalance = NewProjectedEndingBalance;
            }
            else
            {
                bankAccount.ProjectedEndingBalance = CurrentProjectedEndingBalance;
            }

            return bankAccount;
        }

        private async void CheckProcessCompletedData(List<BankAccountRegisterGridRow> completedRows
        , CompletedRegisterData completedRegisterData)
        {
            _saveEntity = true;
            _doProcessCompletedRows = false;
            if (completedRows.Any() && _processCompletedRows)
            {
                var message = "Do you wish to post the Completed rows to History and delete them from the Register?";
                if (await ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, "Post Completed") ==
                    MessageBoxButtonsResult.Yes)
                    _doProcessCompletedRows = true;
            }

            if (_doProcessCompletedRows)
                if (!ProcessCompletedRows(completedRegisterData, completedRows))
                    _saveEntity = false;
        }

        protected override bool SaveEntity(BankAccount entity)
        {
            var completedRows = RegisterGridManager.Rows.OfType<BankAccountRegisterGridRow>()
                .Where(w => w.Completed).ToList();

            var completedRegisterData = new CompletedRegisterData();

            CheckProcessCompletedData(completedRows, completedRegisterData);
            if (!_saveEntity)
            {
                return false;
            }

            var context = SystemGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                if (!context.SaveNoCommitEntity(entity, $"Saving Bank Account '{entity.Description}'"))
                    return false;
                if (completedRegisterData != null)
                {
                    foreach (var bankAccountPeriodHistoryRecord in completedRegisterData.BankAccountPeriodHistoryRecords)
                    {
                        var bankAccountPeriodHistory = context.GetTable<BankAccountPeriodHistory>();
                        if (bankAccountPeriodHistory.Any(a =>
                                a.PeriodType == bankAccountPeriodHistoryRecord.PeriodType &&
                                a.BankAccountId == bankAccountPeriodHistoryRecord.BankAccountId &&
                                a.PeriodEndingDate == bankAccountPeriodHistoryRecord.PeriodEndingDate))
                        {
                            if (!context.SaveNoCommitEntity(bankAccountPeriodHistoryRecord,
                                    $"Saving Bank Account Period Ending '{bankAccountPeriodHistoryRecord.PeriodEndingDate.ToString(CultureInfo.InvariantCulture)}'")
                               )
                                return false;
                        }
                        else
                        {
                            if (!context.AddNewNoCommitEntity<BankAccountPeriodHistory>(bankAccountPeriodHistoryRecord,
                                    $"Saving Bank Account Period Ending '{bankAccountPeriodHistoryRecord.PeriodEndingDate.ToString(CultureInfo.InvariantCulture)}'")
                               )
                                return false;
                        }
                    }


                    foreach (var bankAccountRegisterItem in completedRegisterData.CompletedRegisterItems)
                    {
                        bankAccountRegisterItem.BudgetItem = null;
                        bankAccountRegisterItem.BankAccount = null;
                    }

                    if (completedRegisterData != null)
                    {
                        foreach (var budgetItem in completedRegisterData.BudgetItems)
                        {
                            budgetItem.BankAccount = null;
                            budgetItem.TransferToBankAccount = null;
                            if (!context.SaveNoCommitEntity(budgetItem,
                                    $"Saving Budget Item '{budgetItem.Description}'"))
                                return false;
                        }


                        foreach (var budgetPeriodHistoryRecord in completedRegisterData.BudgetPeriodHistoryRecords)
                        {
                            budgetPeriodHistoryRecord.BudgetItem = null;
                            if (!AppGlobals.DataRepository.SaveBudgetPeriodRecord(context, budgetPeriodHistoryRecord)) return false;
                        }

                        context.RemoveRange<BankAccountRegisterItem>(completedRegisterData.CompletedRegisterItems);

                        context.AddRange<History>(completedRegisterData.NewHistoryRecords);

                        foreach (var newSourceHistoryRecord in completedRegisterData.NewSourceHistoryRecords)
                        {
                            newSourceHistoryRecord.HistoryId = newSourceHistoryRecord.HistoryItem.Id;
                        }

                        context.AddRange<SourceHistory>(completedRegisterData.NewSourceHistoryRecords);
                    }
                }

            }
            var result = context.Commit($"Saving Bank Account '{entity.Description}");
            if (result)
            {
                _recordSaved = true;
            }

            if (_doProcessCompletedRows)
            {
                BankAccountRegisterGridRow currentRow = null;
                if (RegisterGridManager.Grid != null)
                {
                    var currentRowIndex = RegisterGridManager.Grid.CurrentRowIndex;
                    
                    if (currentRowIndex >= 0)
                    {
                        currentRow = (BankAccountRegisterGridRow)RegisterGridManager.Rows[currentRowIndex];
                        while (currentRow.Completed && currentRowIndex > 0)
                        {
                            currentRowIndex--;
                            currentRow = (BankAccountRegisterGridRow)RegisterGridManager.Rows[currentRowIndex];
                        }
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
                    if (RegisterGridManager.Grid != null)
                        RegisterGridManager.Grid.GotoCell(
                            RegisterGridManager.Rows[newRowIndex],
                            RegisterGridManager.Grid.CurrentColumnId);
                }
            }

            foreach (var budgetItemViewModel in AppGlobals.MainViewModel.BudgetItemViewModels)
            {
                budgetItemViewModel.RecalculateBudgetItem();
            }
            if (AppGlobals.MainViewModel != null)
                AppGlobals.MainViewModel.RefreshView();
            result = true;
            if (RegisterGridManager.Rows.Any())
            {
                var rows = RegisterGridManager.Rows.OfType<BankAccountRegisterGridRow>().OrderBy(p => p.ItemDate);
                var newFirstDate = rows.FirstOrDefault().ItemDate;
                if (newFirstDate.Month != _firstRegisterDate.Month)
                {
                    GenerateRegisterItemsFromBudgetCommand.Execute(null);
                    AppGlobals.MainViewModel.SetCurrentMonthEnding(newFirstDate);
                }
            }
            return result;
        }

        private bool ProcessCompletedRows(CompletedRegisterData completedRegisterData, List<BankAccountRegisterGridRow> completedRows)
        {
            if (SystemGlobals.UnitTestMode)
            {
                PostTransactions(completedRegisterData, completedRows);
            }
            else
            {
                BankAccountView.PostRegister(completedRegisterData, completedRows);
            }

            return true;
        }

        public void PostTransactions(CompletedRegisterData completedRegisterData, List<BankAccountRegisterGridRow> completedRows)
        {
            var count = completedRows.Count;
            var rowIndex = 0;
            foreach (var completedRow in completedRows)
            {
                rowIndex++;
                BankAccountView.UpdateStatus($"Processing Row {rowIndex} of {count}");
                var amountDetails = new List<BankAccountRegisterItemAmountDetail>();
                var registerItem = new BankAccountRegisterItem();
                completedRow.SaveToEntity(registerItem, 0, amountDetails);

                //if (!amountDetails.Any())
                //{
                //    registerItem.BankText = completedRow.BankText;
                //}

                var monthEndDate = new DateTime(registerItem.ItemDate.Year, registerItem.ItemDate.Month,
                    DateTime.DaysInMonth(registerItem.ItemDate.Year, registerItem.ItemDate.Month));
                var yearEndDate = new DateTime(registerItem.ItemDate.Year, 12, 31);

                ProcessCompletedBankPeriod(completedRegisterData, monthEndDate, completedRow, PeriodHistoryTypes.Monthly);

                ProcessCompletedBankPeriod(completedRegisterData, yearEndDate, completedRow, PeriodHistoryTypes.Yearly);

                int? transferToBankAccountId = null;
                var processBudgetRow = true;
                var addToBudgetHistory = true;
                switch (completedRow.LineType)
                {
                    case DataAccess.Model.BankAccountRegisterItemTypes.BudgetItem:
                        break;
                    case DataAccess.Model.BankAccountRegisterItemTypes.Miscellaneous:
                        //completedRow.ProjectedAmount = 0;
                        break;
                    case DataAccess.Model.BankAccountRegisterItemTypes.TransferToBankAccount:
                        if (completedRow is BankAccountRegisterGridTransferRow transferRow)
                        {
                            var transferRegisterItem =
                                AppGlobals.DataRepository.GetTransferRegisterItem(transferRow.TransferRegisterGuid);

                            if (transferRegisterItem == null)
                            {
                                //processBudgetRow = false;
                                //addToHistory = false;
                            }
                            else
                            {
                                transferRegisterItem.BankText = completedRow.BankText;
                                if (completedRow.TransactionType == TransactionTypes.Deposit)
                                {
                                    transferToBankAccountId = registerItem.BankAccountId;
                                }
                                else
                                {
                                    processBudgetRow = false;
                                    addToBudgetHistory = false;
                                    transferToBankAccountId = transferRegisterItem.BankAccountId;
                                }
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
                    AddCompletedToHistory(completedRegisterData, registerItem, transferToBankAccountId, completedRow,
                        amountDetails, addToBudgetHistory);
                }

                //RegisterDetails.AddRange(amountDetails);
            }
        }

        private static void ProcessCompletedBudget(CompletedRegisterData completedRegisterData,
            BankAccountRegisterItem registerItem, DateTime monthEndDate, BankAccountRegisterGridRow completedRow,
            DateTime yearEndDate)
        {
            if (registerItem.BudgetItemId != null)
            {
                var budgetItem =
                    completedRegisterData.BudgetItems.FirstOrDefault(f => f.Id == registerItem.BudgetItemId);

                if (budgetItem == null)
                {
                    budgetItem = AppGlobals.DataRepository.GetBudgetItem(registerItem.BudgetItemId);

                    if (budgetItem != null)
                        completedRegisterData.BudgetItems.Add(budgetItem);
                }

                ProcessCompletedBudgetMonth(completedRegisterData, budgetItem, monthEndDate, completedRow);

                ProcessCompletedBudgetPeriod(completedRegisterData, budgetItem, yearEndDate, completedRow, PeriodHistoryTypes.Yearly);
            }
        }

        private static void ProcessCompletedBudgetMonth(CompletedRegisterData completedRegisterData, BudgetItem budgetItem,
            DateTime monthEndDate, BankAccountRegisterGridRow completedRow)
        {
            if (budgetItem == null)
                return;
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

            ProcessCompletedBudgetPeriod(completedRegisterData, budgetItem, monthEndDate, completedRow, PeriodHistoryTypes.Monthly);
        }

        private static void ProcessCompletedBudgetPeriod(CompletedRegisterData completedRegisterData, BudgetItem budgetItem,
            DateTime periodEndDate, BankAccountRegisterGridRow completedRow, PeriodHistoryTypes periodHistoryType)
        {
            if (budgetItem == null)
                return;

            var budgetPeriodHistory = completedRegisterData.BudgetPeriodHistoryRecords.FirstOrDefault(f =>
                f.BudgetItemId == budgetItem.Id &&
                f.PeriodType == (byte)periodHistoryType && f.PeriodEndingDate == periodEndDate);

            if (budgetPeriodHistory == null)
            {
                budgetPeriodHistory = AppGlobals.DataRepository.GetBudgetPeriodHistory(budgetItem.Id,
                    periodHistoryType, periodEndDate) ?? new BudgetPeriodHistory
                {
                    BudgetItemId = budgetItem.Id,
                    PeriodType = (byte)periodHistoryType,
                    PeriodEndingDate = periodEndDate
                };

                completedRegisterData.BudgetPeriodHistoryRecords.Add(budgetPeriodHistory);
            }

            if (completedRow.IsNegative)
            {
                budgetPeriodHistory.ProjectedAmount -= completedRow.ProjectedAmount;
                budgetPeriodHistory.ActualAmount -= completedRow.ActualAmount.GetValueOrDefault(0);
            }
            else
            {
                budgetPeriodHistory.ProjectedAmount += completedRow.ProjectedAmount;
                budgetPeriodHistory.ActualAmount += completedRow.ActualAmount.GetValueOrDefault(0);
            }
        }

        private void ProcessCompletedBankPeriod(CompletedRegisterData completedRegisterData,
            DateTime periodEndDate, BankAccountRegisterGridRow completedRow, PeriodHistoryTypes periodHistoryType)
        {
            var bankPeriodHistory = completedRegisterData.BankAccountPeriodHistoryRecords.FirstOrDefault(f =>
                f.BankAccountId == Id && f.PeriodType == (byte)periodHistoryType &&
                f.PeriodEndingDate == periodEndDate);

            if (bankPeriodHistory == null)
            {
                bankPeriodHistory = AppGlobals.DataRepository.GetBankPeriodHistory(Id,
                    periodHistoryType, periodEndDate) ?? new BankAccountPeriodHistory
                {
                    BankAccountId = Id,
                    PeriodType = (byte)periodHistoryType,
                    PeriodEndingDate = periodEndDate
                };

                completedRegisterData.BankAccountPeriodHistoryRecords.Add(bankPeriodHistory);
            }

            switch (completedRow.TransactionType)
            {
                case TransactionTypes.Deposit:
                    bankPeriodHistory.TotalDeposits += completedRow.ActualAmount.GetValueOrDefault(0);
                    break;
                case TransactionTypes.Withdrawal:
                    bankPeriodHistory.TotalWithdrawals += completedRow.ActualAmount.GetValueOrDefault(0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddCompletedToHistory(CompletedRegisterData completedRegisterData, BankAccountRegisterItem registerItem,
            int? transferToBankAccountId, BankAccountRegisterGridRow completedRow, List<BankAccountRegisterItemAmountDetail> amountDetails, bool addToBudgetHistory)
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
                ActualAmount = completedRow.ActualAmount.GetValueOrDefault(0),
                BankText = registerItem.BankText
            };
            if (!addToBudgetHistory)
            {
                historyItem.BudgetItemId = null;
            }
            if (registerItem.IsNegative)
            {
                historyItem.ProjectedAmount = -historyItem.ProjectedAmount;
                historyItem.ActualAmount = -historyItem.ActualAmount;
            }

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
                    Amount = amountDetail.Amount,
                    BankText = amountDetail.BankText,
                });
                detailId++;
            }
        }

        protected override DbMaintenanceResults OnPreDeleteChildren()
        {
            var context = AppGlobals.DataRepository.GetDataContext();

            var table = context.GetTable<BankAccountRegisterItemAmountDetail>();
            var filter = new TableFilterDefinition<BankAccountRegisterItemAmountDetail>(AppGlobals.LookupContext.BankAccountRegisterItemAmountDetails);
            filter.Include(p => p.RegisterItem)
                .AddFixedFilter(p => p.BankAccountId, Conditions.Equals, Id);

            var param = GblMethods.GetParameterExpression<BankAccountRegisterItemAmountDetail>();
            var expr = filter.GetWhereExpresssion<BankAccountRegisterItemAmountDetail>(param);
            var query = FieldFilterDefinition.FilterQuery(table, param, expr);
            if (query.Any())
            {
                context.RemoveRange(query);
            }

            var table1 = context.GetTable<BankAccountRegisterItem>();

            var filter1 = new TableFilterDefinition<BankAccountRegisterItem>(AppGlobals.LookupContext.BankAccountRegisterItems);
            filter1.AddFixedFilter(p => p.BankAccountId, Conditions.Equals, Id);

            var param1 = GblMethods.GetParameterExpression<BankAccountRegisterItem>();
            var expr1 = filter1.GetWhereExpresssion<BankAccountRegisterItem>(param1);
            var query1 = FieldFilterDefinition.FilterQuery(table1, param1, expr1);
            if (query1.Any())
            {
                context.RemoveRange(query1);
            }

            if (!context.Commit("Deleting Register"))
            {
                return DbMaintenanceResults.ValidationError;
            }
            return base.OnPreDeleteChildren();
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<BankAccount>();
            var bankAccount = query.FirstOrDefault(p => p.Id == Id);
            var result = bankAccount != null && context.DeleteEntity(bankAccount, $"Deleting Bank Account '{bankAccount.Description}'");
            if (result)
            {
                AppGlobals.MainViewModel.RefreshView();
            }
            return result;
        }

        public void OnAddModifyBudgetItems()
        {
            if (ExecuteAddModifyCommand() == DbMaintenanceResults.Success)
                BudgetItemsLookupDefinition.SetCommand(GetLookupCommand(LookupCommands.AddModify));
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            base.OnWindowClosing(e);
            if (!e.Cancel)
                AppGlobals.MainViewModel.BankAccountViewModels.Remove(this);
        }

        public bool IsBeingReconciled(int budgetItemId)
        {
            var registerRows = RegisterGridManager.Rows
                .OfType<BankAccountRegisterGridRow>().Where(p => p.BudgetItemId == budgetItemId);
            if (registerRows.Any(a => a.Completed || a.ActualAmount != null))
                return true;

            return false;
        }

        public bool IsBudgetItemInRegisterGrid(int budgetItemId)
        {
            return RegisterGridManager.Rows.OfType<BankAccountRegisterGridRow>()
                .Any(a => a.BudgetItemId == budgetItemId
                && a.LineType != HomeLogix.DataAccess.Model.BankAccountRegisterItemTypes.Miscellaneous);
        }

        public void RefreshAfterBudgetItemSave()
        {
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
            var bankAccount = AppGlobals.DataRepository.GetBankAccount(Id);
            RegisterGridManager.LoadGrid(bankAccount.RegisterItems);
            RefreshBudgetTotals();
            CalculateTotals();
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

        private void ImportTransactions()
        {
            var keyDown = Processor.IsMaintenanceKeyDown(MaintenanceKey.Alt);
            if (BankAccountView.ImportFromBank(this))
            {
                CalculateTotals();
                AppGlobals.MainViewModel.RefreshView();
            }

            if (!keyDown)
            {
                View.ResetViewForNewRecord();
            }
        }

        protected override void OnPropertyChanged(string propertyName = null, bool raiseDirtyFlag = true)
        {
            if (raiseDirtyFlag)
            {

            }
            base.OnPropertyChanged(propertyName, raiseDirtyFlag);
        }

        public void SaveNoPost()
        {
            _processCompletedRows = false;
            DoSave();
            _processCompletedRows = true;
        }

        protected override PrimaryKeyValue GetAddViewPrimaryKeyValue(PrimaryKeyValue addViewPrimaryKeyValue)
        {
            if (addViewPrimaryKeyValue.TableDefinition == AppGlobals.LookupContext.BankAccountRegisterItems)
            {
                var bankRegisterItem =
                    AppGlobals.LookupContext.BankAccountRegisterItems.GetEntityFromPrimaryKeyValue(addViewPrimaryKeyValue);

                var context = AppGlobals.DataRepository.GetDataContext();
                bankRegisterItem = context.GetTable<BankAccountRegisterItem>()
                    .FirstOrDefault(p => p.Id == bankRegisterItem.Id);

                if (bankRegisterItem != null)
                {
                    InitRegisterId = bankRegisterItem.Id;
                    var bankAccount = new BankAccount() { Id = bankRegisterItem.BankAccountId };
                    return TableDefinition.GetPrimaryKeyValueFromEntity(bankAccount);
                }
            }

            return base.GetAddViewPrimaryKeyValue(addViewPrimaryKeyValue);
        }

        protected override void SetupPrinterArgs(PrinterSetupArgs printerSetupArgs, int stringFieldIndex = 1, int numericFieldIndex = 1,
            int memoFieldIndex = 1)
        {
            printerSetupArgs.PrintingProperties.ReportType = ReportTypes.Custom;
            printerSetupArgs.PrintingProperties.CustomReportPathFileName =
                $"{RingSoftAppGlobals.AssemblyDirectory}\\BankAccount.rpt";

            base.SetupPrinterArgs(printerSetupArgs, stringFieldIndex, numericFieldIndex, memoFieldIndex);
        }

        public override void ProcessPrintOutputData(PrinterSetupArgs printerSetupArgs)
        {
            base.ProcessPrintOutputData(printerSetupArgs);
            var customProperties = new List<PrintingCustomProperty>();
            customProperties.Add(new PrintingCustomProperty
            {
                Name = "intRecordCount",
                Value = printerSetupArgs.TotalRecords.ToString(),
            });
            PrintingInteropGlobals.PropertiesProcessor.CustomProperties = customProperties;
        }

        private void BankAccountViewModel_PrintProcessingHeader(object sender, PrinterDataProcessedEventArgs e)
        {
            var bankAccountId = TableDefinition.GetEntityFromPrimaryKeyValue(e.PrimaryKey).Id;

            var bankAccount = AppGlobals.DataRepository.GetBankAccount(bankAccountId);
            var registerItems = bankAccount.RegisterItems
                .OrderBy(p => p.ItemDate)
                .ThenBy(p => p.ItemType);

            var index = 0;
            var detailsChunk = new List<PrintingInputDetailsRow>();
            var newBalance = bankAccount.CurrentBalance;

            var tranTypeEnum = new EnumFieldTranslation();
            tranTypeEnum.LoadFromEnum<MobileInterop.PhoneModel.TransactionTypes>();
            var numFormat = GblMethods.GetNumFormat(2, true);

            e.HeaderRow.NumberField01 = bankAccount.CurrentBalance.ToString(numFormat);

            var lowestBalance = bankAccount.CurrentBalance;
            DateTime? lowestDate = null;

            foreach (var bankAccountRegisterItem in registerItems)
            {
                if (lowestDate == null)
                {
                    lowestDate = bankAccountRegisterItem.ItemDate;
                }
                var detailsRow = new PrintingInputDetailsRow();
                detailsRow.HeaderRowKey = e.HeaderRow.RowKey;
                detailsRow.TablelId = 1;

                detailsRow.StringField01 =
                    bankAccountRegisterItem.ItemDate.FormatDateValue(DbDateTypes.DateOnly, false);
                
                var registerData = GetRegisterData(bankAccountRegisterItem);

                detailsRow.StringField02 = registerData.Description;

                var tranTypeField =
                    tranTypeEnum.TypeTranslations.FirstOrDefault(p =>
                        p.NumericValue == (int)registerData.TransactionType);
                detailsRow.StringField03 = tranTypeField.TextValue;

                newBalance = CalcNewBalance((BankAccountTypes)bankAccount.AccountType, registerData, newBalance);

                if (newBalance < lowestBalance)
                {
                    lowestBalance = newBalance;
                    lowestDate = bankAccountRegisterItem.ItemDate;
                }

                detailsRow.NumberField01 = registerData.ProjectedAmount.ToString(numFormat);
                detailsRow.NumberField02 = newBalance.ToString(numFormat);

                detailsChunk.Add(detailsRow);

                if (index % 10 == 0)
                {
                    PrintingInteropGlobals.DetailsProcessor.AddChunk(detailsChunk, e.PrinterSetup.PrintingProperties);
                    detailsChunk.Clear();
                }

                index++;
            }

            e.HeaderRow.NumberField02 = newBalance.ToString(numFormat);
            if (lowestDate == null)
            {
                lowestDate = DateTime.Now;
            }
            e.HeaderRow.StringField03 = lowestDate.Value.FormatDateValue(DbDateTypes.DateOnly);

            e.HeaderRow.NumberField03 = lowestBalance.ToString(numFormat);
            PrintingInteropGlobals.DetailsProcessor.AddChunk(detailsChunk, e.PrinterSetup.PrintingProperties);
        }

        public static RegisterData GetRegisterData(BankAccountRegisterItem register)
        {
            var registerData = new RegisterData();
            registerData.BankAccountId = register.BankAccountId;
            registerData.Description = register.Description;
            registerData.Completed = register.Completed;
            registerData.ProjectedAmount = Math.Abs(register.ProjectedAmount);
            registerData.ItemDate = register.ItemDate;
            registerData.IsNegative = register.IsNegative;
            //registerData.RegisterItemType =
            //    (MobileInterop.PhoneModel.BankAccountRegisterItemTypes)register.ItemType;
            registerData.RegisterItemType = MobileInterop.PhoneModel.BankAccountRegisterItemTypes.BudgetItem;

            if (register.BudgetItem == null)
            {
                if (register.ProjectedAmount < 0)
                {
                    registerData.TransactionType = MobileInterop.PhoneModel.TransactionTypes.Withdrawal;
                }
                else
                {
                    registerData.TransactionType = MobileInterop.PhoneModel.TransactionTypes.Deposit;
                }
            }
            else
            {
                var budgetType = (BudgetItemTypes)register.BudgetItem.Type;
                switch (budgetType)
                {
                    case BudgetItemTypes.Income:
                        registerData.TransactionType = MobileInterop.PhoneModel.TransactionTypes.Deposit;
                        break;
                    case BudgetItemTypes.Expense:
                        registerData.TransactionType = MobileInterop.PhoneModel.TransactionTypes.Withdrawal;
                        break;
                    case BudgetItemTypes.Transfer:
                        if (register.ProjectedAmount < 0)
                        {
                            registerData.TransactionType = MobileInterop.PhoneModel.TransactionTypes.Withdrawal;
                        }
                        else
                        {
                            registerData.TransactionType = MobileInterop.PhoneModel.TransactionTypes.Deposit;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return registerData;
        }

        public static double CalcNewBalance(BankAccountTypes accountType, RegisterData registerData, double balance)
        {
            if (registerData.Completed)
            {
                return balance;
            }
            var amount = registerData.ProjectedAmount;
            if (registerData.ActualAmount > 0)
            {
                if (registerData.ProjectedAmount > registerData.ActualAmount)
                {
                    amount = registerData.ProjectedAmount - registerData.ActualAmount;
                }
                else
                {
                    return balance;
                }
            }
            switch (accountType)
            {
                case BankAccountTypes.Checking:
                case BankAccountTypes.Savings:
                    switch (registerData.TransactionType)
                    {
                        case MobileInterop.PhoneModel.TransactionTypes.Deposit:
                            balance += amount;
                            break;
                        case MobileInterop.PhoneModel.TransactionTypes.Withdrawal:
                            balance -= amount;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                case BankAccountTypes.CreditCard:
                    switch (registerData.TransactionType)
                    {
                        case MobileInterop.PhoneModel.TransactionTypes.Deposit:
                            balance -= amount;
                            break;
                        case MobileInterop.PhoneModel.TransactionTypes.Withdrawal:
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
    }
}
