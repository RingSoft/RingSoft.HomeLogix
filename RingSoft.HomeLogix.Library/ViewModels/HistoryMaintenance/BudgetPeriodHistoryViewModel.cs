using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using System;
using System.ComponentModel;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance
{
    public class BudgetPeriodHistoryViewModel : DbMaintenanceViewModel<BudgetPeriodHistory>
    {
        #region Properties

        private AutoFillSetup _budgetAutoFillSetup;

        public AutoFillSetup BudgetAutoFillSetup
        {
            get { return _budgetAutoFillSetup; }
            set
            {
                if (_budgetAutoFillSetup == value)
                {
                    return;
                }
                _budgetAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _budgetAutoFillValue;

        public AutoFillValue BudgetAutoFillValue
        {
            get { return _budgetAutoFillValue; }
            set
            {
                if (_budgetAutoFillValue == value)
                    return;

                _budgetAutoFillValue = value;
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

        #endregion

        public ViewModelInput ViewModelInput { get; set; }

        private PeriodHistoryTypes _mode;
        private BudgetPeriodHistory _budgetPeriodHistory;
        private bool _noFilters;

        public BudgetPeriodHistoryViewModel()
        {
            BudgetAutoFillSetup = new AutoFillSetup(TableDefinition
                .GetFieldDefinition(p => p.BudgetItemId));
        }
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

            PeriodEndingDate = newEntity.PeriodEndingDate;
        }

        protected override BudgetPeriodHistory GetEntityFromDb(BudgetPeriodHistory newEntity, PrimaryKeyValue primaryKeyValue)
        {
            _budgetPeriodHistory = AppGlobals.DataRepository.GetBudgetPeriodHistory(newEntity.BudgetItemId,
                (PeriodHistoryTypes)newEntity.PeriodType, newEntity.PeriodEndingDate);

            return _budgetPeriodHistory;
        }

        protected override void LoadFromEntity(BudgetPeriodHistory entity)
        {
            HistoryLookupDefinition.FilterDefinition.ClearFixedFilters();
            HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BudgetItemId, Conditions.Equals,
                entity.BudgetItemId);

            if (_mode == PeriodHistoryTypes.Monthly)
            {
                var beginDate = new DateTime(entity.PeriodEndingDate.Year
                    , entity.PeriodEndingDate.Month, 1);
                HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Date
                    , Conditions.GreaterThanEquals, beginDate);
                HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Date
                    , Conditions.LessThanEquals, entity.PeriodEndingDate);
            }
            else
            {
                var beginDate = new DateTime(entity.PeriodEndingDate.Year
                    , 1, 1);
                var endDate = new DateTime(entity.PeriodEndingDate.Year
                    , 12, 31);

                HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Date
                    , Conditions.GreaterThanEquals, beginDate);
                HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.Date
                    , Conditions.LessThanEquals, endDate);
            }
            HistoryLookupDefinition.SetCommand(
                GetLookupCommand(LookupCommands.Refresh
                    , TableDefinition.GetPrimaryKeyValueFromEntity(entity)
                    , ViewModelInput));

            BudgetAutoFillValue = entity.BudgetItem.GetAutoFillValue();
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
            BudgetAutoFillValue = null;
            PeriodEndingDate = DateTime.Today;
            ProjectedAmount = ActualAmount = Difference = 0;

            HistoryLookupDefinition.SetCommand(
                GetLookupCommand(LookupCommands.Clear));
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
