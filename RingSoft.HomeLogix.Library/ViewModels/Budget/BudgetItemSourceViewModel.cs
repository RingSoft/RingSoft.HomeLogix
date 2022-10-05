using System;
using System.Collections.Generic;
using System.Text;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BudgetItemSourceViewModel : AppDbMaintenanceViewModel<BudgetItemSource>
    {
        public override TableDefinition<BudgetItemSource> TableDefinition => AppGlobals.LookupContext.BudgetItemSources;

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


        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                    return;

                _name = value;
                OnPropertyChanged();
            }
        }


        private LookupDefinition<SourceHistoryLookup, SourceHistory> _sourceHistoryLookupDefinition;

        public LookupDefinition<SourceHistoryLookup, SourceHistory> SourceHistoryLookupDefinition
        {
            get => _sourceHistoryLookupDefinition;
            set
            {
                if (_sourceHistoryLookupDefinition == value)
                    return;

                _sourceHistoryLookupDefinition = value;
                OnPropertyChanged();
            }
        }

        private LookupCommand _sourceHistoryLookupCommand;

        public LookupCommand SourceHistoryLookupCommand
        {
            get => _sourceHistoryLookupCommand;
            set
            {
                if (_sourceHistoryLookupCommand == value)
                    return;

                _sourceHistoryLookupCommand = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalAmount;

        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                if (_totalAmount == value)
                {
                    return;
                }
                _totalAmount = value;
                OnPropertyChanged();
            }
        }


        private bool _isIncome;

        protected override void Initialize()
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.InputParameter is ViewModelInput viewModelInput)
            {
                _isIncome = viewModelInput.SourceHistoryIsIncome;
                viewModelInput.SourceHistoryIsIncome = false;
            }

            var sourceHistoryLookupDefinition =
                new LookupDefinition<SourceHistoryLookup, SourceHistory>(AppGlobals.LookupContext.SourceHistory);
            sourceHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.Date, p => p.Date);
            sourceHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.Amount, p => p.Amount);
            sourceHistoryLookupDefinition.AddHiddenColumn(p => p.HistoryId, p => p.HistoryId);
            sourceHistoryLookupDefinition.InitialOrderByType = OrderByTypes.Descending;

            SourceHistoryLookupDefinition = sourceHistoryLookupDefinition;

            base.Initialize();
        }


        protected override BudgetItemSource PopulatePrimaryKeyControls(BudgetItemSource newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
            var budgetItemSource = AppGlobals.DataRepository.GetBudgetItemSource(Id);
            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, budgetItemSource.Name);

            SourceHistoryLookupDefinition.FilterDefinition.ClearFixedFilters();
            SourceHistoryLookupDefinition.FilterDefinition.AddFixedFilter(p => p.SourceId, Conditions.Equals, Id);
            SourceHistoryLookupCommand = GetLookupCommand(LookupCommands.Refresh, primaryKeyValue);

            return budgetItemSource;
        }

        protected override void LoadFromEntity(BudgetItemSource entity)
        {
            TotalAmount = AppGlobals.DataRepository.GetSourceTotal(entity.Id);
        }

        protected override BudgetItemSource GetEntityData()
        {
            return new BudgetItemSource
            {
                Id = Id,
                IsIncome = _isIncome,
                Name = KeyAutoFillValue.Text
            };
        }

        protected override void ClearData()
        {
            SourceHistoryLookupCommand = GetLookupCommand(LookupCommands.Clear);
            TotalAmount = 0;
        }

        protected override bool SaveEntity(BudgetItemSource entity)
        {
            return AppGlobals.DataRepository.SaveBudgetItemSource(entity);
        }

        protected override bool DeleteEntity()
        {
            return AppGlobals.DataRepository.DeleteBudgetItemSource(Id);
        }

        protected override PrimaryKeyValue GetAddViewPrimaryKeyValue(PrimaryKeyValue addViewPrimaryKeyValue)
        {
            if (LookupAddViewArgs.LookupData.LookupDefinition.TableDefinition == AppGlobals.LookupContext.SourceHistory)
            {
                var sourceHistory = AppGlobals.LookupContext.SourceHistory.GetEntityFromPrimaryKeyValue(
                    addViewPrimaryKeyValue);

                sourceHistory =
                    AppGlobals.DataRepository.GetSourceHistory(sourceHistory.HistoryId, sourceHistory.DetailId);

                return AppGlobals.LookupContext.BudgetItemSources.GetPrimaryKeyValueFromEntity(sourceHistory.Source);
            }
            return base.GetAddViewPrimaryKeyValue(addViewPrimaryKeyValue);
        }
    }
}
