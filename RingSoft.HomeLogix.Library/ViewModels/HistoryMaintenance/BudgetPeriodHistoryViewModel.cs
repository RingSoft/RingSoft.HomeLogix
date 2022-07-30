using System;
using System.Collections.Generic;
using System.Text;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance
{
    public class BudgetPeriodHistoryViewModel : AppDbMaintenanceViewModel<BudgetPeriodHistory>
    {
        private AutoFillSetup _budgetAutoFillSetup;

        public AutoFillSetup BudgetAutoFillSetup
        {
            get => _budgetAutoFillSetup;
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
            get => _budgetAutoFillValue;
            set
            {
                if (_budgetAutoFillValue == value)
                {
                    return;
                }
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

        protected override void Initialize()
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is ViewModelInput viewModelInput)
            {
                ViewModelInput = viewModelInput;
            }

            BudgetAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BudgetItemsLookup);
            ReadOnlyMode = true;

            BudgetAutoFillSetup = new AutoFillSetup(TableDefinition.GetFieldDefinition(p => p.BudgetItemId))
            {
                AddViewParameter = ViewModelInput,
                //AllowLookupAdd = false
            };

            FindButtonLookupDefinition.InitialOrderByType = OrderByTypes.Descending;

            HistoryLookupDefinition = AppGlobals.LookupContext.HistoryLookup.Clone();

            base.Initialize();

            SelectButtonEnabled = SaveButtonEnabled = DeleteButtonEnabled = NewButtonEnabled = false;
        }

        protected override BudgetPeriodHistory PopulatePrimaryKeyControls(BudgetPeriodHistory newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var budgetPeriodHistory = AppGlobals.DataRepository.GetBudgetPeriodHistory(newEntity.BudgetItemId,
                (PeriodHistoryTypes)newEntity.PeriodType, newEntity.PeriodEndingDate);

            var budgetItem = budgetPeriodHistory.BudgetItem;

            BudgetAutoFillValue = new AutoFillValue(
                AppGlobals.LookupContext.BudgetItems.GetPrimaryKeyValueFromEntity(budgetItem),
                budgetItem.Description);
            
            PeriodEndingDate = budgetPeriodHistory.PeriodEndingDate;

            FindButtonLookupDefinition.FilterDefinition.AddFixedFilter(
                TableDefinition.GetFieldDefinition(p => p.BudgetItemId),
                Conditions.Equals, budgetPeriodHistory.BudgetItemId);

            FindButtonLookupDefinition.FilterDefinition.AddFixedFilter(
                TableDefinition.GetFieldDefinition(p => p.PeriodType),
                Conditions.Equals, budgetPeriodHistory.PeriodType);

            FindButtonLookupDefinition.ReadOnlyMode = false;

            HistoryLookupDefinition.FilterDefinition.ClearFixedFilters();
            HistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.BudgetItemId, Conditions.Equals,
                budgetPeriodHistory.BudgetItemId);

            var sqlGenerator = AppGlobals.LookupContext.DataProcessor.SqlGenerator;
            var table = AppGlobals.LookupContext.History;
            var sql =
                $"strftime('%m', {sqlGenerator.FormatSqlObject(table.TableName)}.";
            sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)}) = ";
            sql += $"'{budgetPeriodHistory.PeriodEndingDate.Month:D2}'";

            HistoryLookupDefinition.FilterDefinition.AddFixedFilter(sql);

            sql =
                $"strftime('%Y', {sqlGenerator.FormatSqlObject(table.TableName)}.";
            sql += $"{sqlGenerator.FormatSqlObject(table.GetFieldDefinition(p => p.Date).FieldName)}) = ";
            sql += $"'{budgetPeriodHistory.PeriodEndingDate.Year:D4}'";

            HistoryLookupDefinition.FilterDefinition.AddFixedFilter(sql);

            HistoryLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            return budgetPeriodHistory;
        }

        protected override void LoadFromEntity(BudgetPeriodHistory entity)
        {
            ProjectedAmount = entity.ProjectedAmount;
            ActualAmount = entity.ActualAmount;

            if (entity.BudgetItem.Type == BudgetItemTypes.Income)
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
    }
}
