using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class BudgetItemSourceViewModel : DbMaintenanceViewModel<BudgetItemSource>
    {
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

        private double _totalAmount;

        public double TotalAmount
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

        #endregion

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
            RegisterLookup(SourceHistoryLookupDefinition);

            base.Initialize();
        }

        protected override void PopulatePrimaryKeyControls(BudgetItemSource newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
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
            Id = 0;
            TotalAmount = 0;
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
