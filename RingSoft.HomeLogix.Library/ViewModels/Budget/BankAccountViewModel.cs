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
                RefreshBudgetTotals();
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


        #endregion

        public ViewModelInput ViewModelInput { get; set; }

        public RelayCommand AddNewRegisterItemCommand { get; }

        public RelayCommand GenerateRegisterItemsFromBudgetCommand { get; }

        public RelayCommand BudgetItemsAddModifyCommand { get; }

        public List<BankAccountRegisterItemAmountDetail> RegisterDetails { get; } =
            new List<BankAccountRegisterItemAmountDetail>();

        private bool _loading;

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

            EscrowBankAccountAutoFillSetup =
                new AutoFillSetup(
                    AppGlobals.LookupContext.BankAccounts.GetFieldDefinition(p => p.EscrowToBankAccountId));

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
            CurrentBalance = 0;
            NewProjectedEndingBalance = 0;
            ProjectedEndingBalanceDifference = 0;
            EscrowBalance = null;
            ProjectedLowestBalanceDate = null;
            ProjectedLowestBalanceAmount = 0;
            
            MonthlyBudgetDeposits = 0;
            MonthlyBudgetWithdrawals = 0;
            MonthlyBudgetDifference = 0;
            
            EscrowBankAccountAutoFillValue = null;
            EscrowDayOfMonth = 1;
            Notes = string.Empty;
            LastGenerationDate = null;

            RegisterGridManager.SetupForNewRecord();
            BankAccountView.EnableRegisterGrid(false);

            BudgetItemsLookupCommand = GetLookupCommand(LookupCommands.Clear);

            _loading = false;
        }

        protected override BankAccount PopulatePrimaryKeyControls(BankAccount newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
            var bankAccount = AppGlobals.DataRepository.GetBankAccount(Id);
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, bankAccount.Description);

            BudgetItemsLookupDefinition.FilterDefinition.ClearFixedFilters();
            BudgetItemsLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BankAccountId, Conditions.Equals,
                Id).SetEndLogic(EndLogics.Or);
            BudgetItemsLookupDefinition.FilterDefinition.AddFixedFilter(p => p.TransferToBankAccountId,
                Conditions.Equals, Id);

            BudgetItemsLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput);

            ReadOnlyMode = ViewModelInput.BankAccountViewModels.Any(a => a != this && a.Id == Id);
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
            LastGenerationDate = entity.LastGenerationDate;
            if (LastGenerationDate == DateTime.MinValue)
                LastGenerationDate = null;

            RegisterGridManager.LoadGrid(entity.RegisterItems.OrderBy(o => o.ItemDate)
                .ThenByDescending(t => t.ProjectedAmount));
            BankAccountView.EnableRegisterGrid(RegisterGridManager.Rows.Any());

            _loading = false;
            CalculateTotals();

            if (ReadOnlyMode)
                ControlsGlobals.UserInterface.ShowMessageBox(
                    "This Bank Account is being modified in another window.  Editing not allowed.", "Editing not allowed",
                    RsMessageBoxIcons.Exclamation);
        }

        public void CalculateTotals()
        {
            if (_loading)
                return;

            RegisterGridManager.CalculateProjectedBalanceData();
            ProjectedEndingBalanceDifference = NewProjectedEndingBalance - CurrentProjectedEndingBalance;
            
            MonthlyBudgetDifference = MonthlyBudgetDeposits - MonthlyBudgetWithdrawals;
        }

        public void RefreshBudgetTotals()
        {
            var bankAccount = AppGlobals.DataRepository.GetBankAccount(Id, false);

            MonthlyBudgetDeposits = bankAccount.MonthlyBudgetDeposits;
            MonthlyBudgetWithdrawals = bankAccount.MonthlyBudgetWithdrawals;
            EscrowBalance = bankAccount.EscrowBalance;
            CalculateTotals();
        }

        private void AddNewRegisterItem()
        {
            //TODO
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

            var escrowOutput = BudgetItemProcessor.GenerateEscrowRegisterItems(GetEntityData(), generateToDate.Value);

            var registerItems = new List<BankAccountRegisterItem>();
            registerItems.AddRange(
                // ReSharper disable once PossibleInvalidOperationException
                escrowOutput.RegisterItems);

            foreach (var budgetItem in budgetItems)
            {
                registerItems.AddRange(
                    BudgetItemProcessor.GenerateBankAccountRegisterItems(Id, budgetItem, generateToDate.Value));
            }

            var bankAccount = AppGlobals.DataRepository.GetBankAccount(Id, false);
            LastGenerationDate = bankAccount.LastGenerationDate = generateToDate.Value;
            if (!AppGlobals.DataRepository.SaveGeneratedRegisterItems(registerItems, escrowOutput.RegisterItemEscrows,
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
                EscrowBalance = EscrowBalance,
                ProjectedLowestBalanceDate = ProjectedLowestBalanceDate,
                ProjectedLowestBalanceAmount = ProjectedLowestBalanceAmount,
                MonthlyBudgetDeposits = MonthlyBudgetDeposits,
                MonthlyBudgetWithdrawals = MonthlyBudgetWithdrawals,
                EscrowDayOfMonth = EscrowDayOfMonth,
                Notes = Notes,
                LastGenerationDate = (DateTime)LastGenerationDate
            };

            if (EscrowBankAccountAutoFillValue != null && EscrowBankAccountAutoFillValue.PrimaryKeyValue != null &&
                EscrowBankAccountAutoFillValue.PrimaryKeyValue.IsValid)
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
            RegisterDetails.Clear();

            entity.RegisterItems = RegisterGridManager.GetEntityList();

            var completedRows = RegisterGridManager.Rows.OfType<BankAccountRegisterGridRow>().Where(w => w.Completed).ToList();

            var completedRegisterData = new CompletedRegisterData();
            if (!ProcessCompletedRows(completedRegisterData, completedRows))
                return false;

            if (AppGlobals.DataRepository.SaveBankAccount(entity, RegisterDetails, completedRegisterData))
            {
                if (completedRows.Any())
                {
                    RegisterGridManager.Grid?.TakeCellSnapshot(true, true);
                    foreach (var completedRow in completedRows)
                    {
                        RegisterGridManager.RemoveRow(completedRow);
                    }

                    CalculateTotals();
                    RegisterGridManager.Grid?.RestoreCellSnapshot(true, true);
                }

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
                var addToHistory = true;
                switch (completedRow.LineType)
                {
                    case BankAccountRegisterItemTypes.BudgetItem:
                        break;
                    case BankAccountRegisterItemTypes.Miscellaneous:
                        break;
                    case BankAccountRegisterItemTypes.TransferToBankAccount:
                    case BankAccountRegisterItemTypes.MonthlyEscrow:
                        if (completedRow is BankAccountRegisterGridTransferRow transferRow)
                        {
                            var transferRegisterItem =
                                AppGlobals.DataRepository.GetTransferRegisterItem(transferRow.TransferRegisterGuid);

                            if (transferRegisterItem == null)
                            {
                                processBudgetRow = false;
                                addToHistory = false;
                            }
                            else
                            {
                                if (completedRow.TransactionType == TransactionTypes.Deposit)
                                    transferToBankAccountId = registerItem.BankAccountId;
                                else
                                    transferToBankAccountId = transferRegisterItem.BankAccountId;
                            }
                        }

                        if (processBudgetRow && completedRow.LineType == BankAccountRegisterItemTypes.MonthlyEscrow &&
                            completedRow is BankAccountRegisterGridEscrowRow escrowRow)
                        {
                            ProcessRegisterItemEscrowRow(completedRegisterData, registerItem, escrowRow);
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

                if (addToHistory)
                {
                    AddCompletedToHistory(completedRegisterData, registerItem, transferToBankAccountId, completedRow, amountDetails);
                }

                //RegisterDetails.AddRange(amountDetails);
            }
            return true;
        }

        private static void ProcessRegisterItemEscrowRow(CompletedRegisterData completedRegisterData,
            BankAccountRegisterItem registerItem, BankAccountRegisterGridEscrowRow escrowRow)
        {
            var escrowBudgetItems = AppGlobals.DataRepository.GetEscrowsOfRegisterItem(registerItem.Id);
            foreach (var escrowBudgetItem in escrowBudgetItems)
            {
                var budgetItem =
                    completedRegisterData.BudgetItems.FirstOrDefault(f =>
                        f.Id == escrowBudgetItem.BudgetItemId);
                if (budgetItem == null)
                {
                    budgetItem = escrowBudgetItem.BudgetItem;
                    completedRegisterData.BudgetItems.Add(budgetItem);
                }

                if (escrowRow.IsEscrowFrom)
                {
                    budgetItem.EscrowBalance -= escrowBudgetItem.Amount;
                }
                else
                {
                    budgetItem.EscrowBalance += escrowBudgetItem.Amount;
                }
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
            if (budgetItem.CurrentMonthEnding < monthEndDate)
                budgetItem.CurrentMonthEnding = monthEndDate;

            budgetItem.CurrentMonthAmount += completedRow.ActualAmount.GetValueOrDefault(0);

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

        protected override void OnPropertyChanged(string propertyName = null, bool raiseDirtyFlag = true)
        {
            if (raiseDirtyFlag)
            {

            }
            base.OnPropertyChanged(propertyName, raiseDirtyFlag);
        }
    }
}
