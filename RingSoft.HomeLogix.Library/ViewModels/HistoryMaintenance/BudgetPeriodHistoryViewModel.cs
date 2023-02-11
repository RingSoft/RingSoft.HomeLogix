using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance
{
    public class BudgetPeriodHistoryViewModel : AppDbMaintenanceViewModel<BudgetPeriodHistory>
    {

        private string _budgetItem;

        public string BudgetItem
        {
            get => _budgetItem;
            set
            {
                if (_budgetItem == value)
                {
                    return;
                }
                _budgetItem = value;

                OnPropertyChanged();
            }
        }

        private DateTime _periodEndingDate;

        public DateTime PeriodEndingDate
        {
            get => _periodEndingDate;
            set
            {
                if (_periodEndingDate == value)
                {
                    return;
                }
                _periodEndingDate = value;
                OnPropertyChanged();
            }
        }

        private decimal _projectedAmount;

        public decimal ProjectedAmount
        {
            get => _projectedAmount;
            set
            {
                if (_projectedAmount == value)
                {
                    return;
                }
                _projectedAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _actualAmount;

        public decimal ActualAmount
        {
            get => _actualAmount;
            set
            {
                if (_actualAmount == value)
                {
                    return;
                }
                _actualAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _difference;

        public decimal Difference
        {
            get => _difference;
            set
            {
                if (_difference == value)
                    return;

                _difference = value;
                OnPropertyChanged();
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

        public ViewModelInput ViewModelInput { get; set; }

        public override TableDefinition<BudgetPeriodHistory> TableDefinition =>
            AppGlobals.LookupContext.BudgetPeriodHistory;

        private PeriodHistoryTypes _mode;
        private BudgetPeriodHistory _budgetPeriodHistory;

        protected override void Initialize()
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is ViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
                _mode = PeriodHistoryTypes.Monthly;
            }
            else if (LookupAddViewArgs != null &&
                     LookupAddViewArgs.InputParameter is YearlyHistoryFilter yearlyHistoryFilter)
            {
                ViewModelInput = yearlyHistoryFilter.ViewModelInput;
                _mode = PeriodHistoryTypes.Yearly;
            }
            else
            {
                ViewModelInput = new ViewModelInput();
                _mode = PeriodHistoryTypes.Monthly;
            }

            if (Processor is IAppDbMaintenanceProcessor appDbMaintenanceProcessor)
            {
                appDbMaintenanceProcessor.WindowReadOnlyMode = true;
            }

            FindButtonLookupDefinition.InitialOrderByType = OrderByTypes.Descending;

            HistoryLookupDefinition = AppGlobals.LookupContext.HistoryLookup.Clone();

            base.Initialize();

            SelectButtonEnabled = SaveButtonEnabled = DeleteButtonEnabled = NewButtonEnabled = false;
        }

        protected override BudgetPeriodHistory PopulatePrimaryKeyControls(BudgetPeriodHistory newEntity, PrimaryKeyValue primaryKeyValue)
        {
            _budgetPeriodHistory = AppGlobals.DataRepository.GetBudgetPeriodHistory(newEntity.BudgetItemId,
                (PeriodHistoryTypes)newEntity.PeriodType, newEntity.PeriodEndingDate);

            var budgetItem = AppGlobals.DataRepository.GetBudgetItem(_budgetPeriodHistory.BudgetItemId);

            BudgetItem = budgetItem.Description;
            
            PeriodEndingDate = _budgetPeriodHistory.PeriodEndingDate;

            FindButtonLookupDefinition.ReadOnlyMode = false;

            HistoryLookupDefinition.FilterDefinition.ClearFixedFilters();
            HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BudgetItemId, Conditions.Equals,
                _budgetPeriodHistory.BudgetItemId);

            var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;
            var table = AppGlobals.LookupContext.History;
            var sql = string.Empty;
            var yearSql = string.Empty;
            switch (AppGlobals.DbPlatform)
            {
                case DbPlatforms.SqlServer:
                    sql =
                        $"MONTH({sqlGenerator.FormatSqlObject(table.TableName)}.";
                    sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)})";
                    //sql += $"'{budgetPeriodHistory.PeriodEndingDate.Month:D2}'";
                    yearSql =
                        $"YEAR({sqlGenerator.FormatSqlObject(table.TableName)}.";
                    yearSql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)})";
                    break;
                case DbPlatforms.Sqlite:
                case DbPlatforms.MySql:
                    sql =
                        $"strftime('%m', {sqlGenerator.FormatSqlObject(table.TableName)}.";
                    sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)})";
                    //sql += $"'{budgetPeriodHistory.PeriodEndingDate.Month:D2}'";
                    yearSql =
                        $"strftime('%Y', {sqlGenerator.FormatSqlObject(table.TableName)}.";
                    yearSql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)})";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (_mode == PeriodHistoryTypes.Monthly)
            {
                HistoryLookupDefinition.FilterDefinition.AddFixedFilter("Month", Conditions.Equals, $"{_budgetPeriodHistory.PeriodEndingDate.Month:D2}", sql);
            }
            
            HistoryLookupDefinition.FilterDefinition.AddFixedFilter("Year", Conditions.Equals, $"{_budgetPeriodHistory.PeriodEndingDate.Year:D4}", yearSql);

            _budgetPeriodHistory = _budgetPeriodHistory;
            ViewModelInput.HistoryFilterBudgetPeriodItem = _budgetPeriodHistory;
                
            HistoryLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput);

            return _budgetPeriodHistory;
        }

        protected override void LoadFromEntity(BudgetPeriodHistory entity)
        {
            ProjectedAmount = entity.ProjectedAmount;
            ActualAmount = entity.ActualAmount;
            var budgetItem = AppGlobals.DataRepository.GetBudgetItem(entity.BudgetItemId);

            if (budgetItem.Type == BudgetItemTypes.Income)
            {
                Difference = ActualAmount - ProjectedAmount;
            }
            else
            {
                Difference = ProjectedAmount - ActualAmount;
            }
        }

        protected override BudgetPeriodHistory GetEntityData()
        {
            throw new NotImplementedException();
        }

        protected override void ClearData()
        {
            BudgetItem = string.Empty;
            PeriodEndingDate = DateTime.Today;
            ProjectedAmount = ActualAmount = Difference = 0;

            HistoryLookupCommand = GetLookupCommand(LookupCommands.Clear);
        }

        protected override bool SaveEntity(BudgetPeriodHistory entity)
        {
            throw new NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new NotImplementedException();
        }

        public override void OnWindowClosing(CancelEventArgs e)
        {
            ViewModelInput.HistoryFilterBudgetPeriodItem = null;
            base.OnWindowClosing(e);
        }

        protected override void PrintOutput()
        {
            var printerSetup = CreatePrinterSetupArgs();

            var callBack = new HistoryPrintFilterCallBack();
            callBack.FilterDate = PeriodEndingDate;

            callBack.PrintOutput += (sender, model) =>
            {
                if (printerSetup.LookupDefinition is LookupDefinition<BudgetPeriodHistoryLookup, BudgetPeriodHistory> historyLookup)
                {
                    historyLookup.FilterDefinition.ClearFixedFilters();

                    historyLookup.FilterDefinition.AddFixedFilter(
                        TableDefinition.GetFieldDefinition(p => p.BudgetItemId),
                        Conditions.Equals, _budgetPeriodHistory.BudgetItemId);

                    historyLookup.FilterDefinition.AddFixedFilter(
                        TableDefinition.GetFieldDefinition(p => p.PeriodType), Conditions.Equals,
                        _budgetPeriodHistory.PeriodType);

                    if (model.BeginningDate.HasValue)
                    {
                        historyLookup.FilterDefinition.AddFixedFilter(p => p.PeriodEndingDate,
                            Conditions.GreaterThanEquals,
                            model.BeginningDate.Value);
                    }

                    if (model.EndingDate.HasValue)
                    {
                        historyLookup.FilterDefinition.AddFixedFilter(p => p.PeriodEndingDate,
                            Conditions.LessThanEquals,
                            model.EndingDate.Value);
                    }
                }

                Processor.PrintOutput(printerSetup);
            };

            AppGlobals.MainViewModel.View.ShowHistoryPrintFilterWindow(callBack);
        }
    }
}
