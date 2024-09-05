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
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance
{
    public class BudgetPeriodHistoryViewModel : DbMaintenanceViewModel<BudgetPeriodHistory>
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

        private double _projectedAmount;

        public double ProjectedAmount
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

        private double _actualAmount;

        public double ActualAmount
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

        private double _difference;

        public double Difference
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

        private PeriodHistoryTypes _mode;
        private BudgetPeriodHistory _budgetPeriodHistory;
        private bool _noFilters;

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
                _noFilters = true;

                MakeFilters(FindButtonLookupDefinition);
            }

            ReadOnlyMode = true;

            FindButtonLookupDefinition.InitialOrderByType = OrderByTypes.Descending;

            HistoryLookupDefinition = AppGlobals.LookupContext.HistoryLookup.Clone();

            base.Initialize();

            SelectButtonEnabled = SaveButtonEnabled = DeleteButtonEnabled = NewButtonEnabled = false;
        }

        protected override void PopulatePrimaryKeyControls(BudgetPeriodHistory newEntity, PrimaryKeyValue primaryKeyValue)
        {
            FindButtonLookupDefinition.ReadOnlyMode = false;

            HistoryLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue, ViewModelInput);
        }

        protected override BudgetPeriodHistory GetEntityFromDb(BudgetPeriodHistory newEntity, PrimaryKeyValue primaryKeyValue)
        {
            _budgetPeriodHistory = AppGlobals.DataRepository.GetBudgetPeriodHistory(newEntity.BudgetItemId,
                (PeriodHistoryTypes)newEntity.PeriodType, newEntity.PeriodEndingDate);

            var budgetItem = AppGlobals.DataRepository.GetBudgetItem(_budgetPeriodHistory.BudgetItemId);

            BudgetItem = budgetItem.Description;

            PeriodEndingDate = _budgetPeriodHistory.PeriodEndingDate;

            HistoryLookupDefinition.FilterDefinition.ClearFixedFilters();
            HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BudgetItemId, Conditions.Equals,
                _budgetPeriodHistory.BudgetItemId);

            var table = AppGlobals.LookupContext.History;

            if (_mode == PeriodHistoryTypes.Monthly)
            {
                var beginDate = new DateTime(_budgetPeriodHistory.PeriodEndingDate.Year
                    , _budgetPeriodHistory.PeriodEndingDate.Month, 1);
                HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Date
                    , Conditions.GreaterThanEquals, beginDate);
                HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Date
                    , Conditions.LessThanEquals, _budgetPeriodHistory.PeriodEndingDate);
            }
            else
            {
                var beginDate = new DateTime(_budgetPeriodHistory.PeriodEndingDate.Year
                    , 1, 1);
                var endDate = new DateTime(_budgetPeriodHistory.PeriodEndingDate.Year
                    , 12, 31);

                HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Date
                    , Conditions.GreaterThanEquals, beginDate);
                HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Date
                    , Conditions.LessThanEquals, endDate);
            }

            return _budgetPeriodHistory;
        }

        protected override void LoadFromEntity(BudgetPeriodHistory entity)
        {
            ProjectedAmount = entity.ProjectedAmount;
            ActualAmount = entity.ActualAmount;
            var budgetItem = AppGlobals.DataRepository.GetBudgetItem(entity.BudgetItemId);

            if ((BudgetItemTypes)budgetItem.Type == BudgetItemTypes.Income)
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

                    MakeFilters(printerSetup.LookupDefinition);

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

        private void MakeFilters(LookupDefinitionBase lookupDefinition)
        {
            if (!_noFilters)
            {
                LookupDefinition<BudgetPeriodHistoryLookup, BudgetPeriodHistory> historyLookup;
                lookupDefinition.FilterDefinition.AddFixedFilter(
                    TableDefinition.GetFieldDefinition(p => p.BudgetItemId),
                    Conditions.Equals, _budgetPeriodHistory.BudgetItemId);

                lookupDefinition.FilterDefinition.AddFixedFilter(
                    TableDefinition.GetFieldDefinition(p => p.PeriodType), Conditions.Equals,
                    _budgetPeriodHistory.PeriodType);
            }
        }
    }
}
