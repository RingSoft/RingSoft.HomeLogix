using System.Linq;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public class QifMapsViewModel : AppDbMaintenanceViewModel<QifMap>
    {
        public override TableDefinition<QifMap> TableDefinition => AppGlobals.LookupContext.QifMaps;

        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _budgetItemAutoFillSetup;

        public AutoFillSetup BudgetItemAutoFillSetup
        {
            get => _budgetItemAutoFillSetup;
            set
            {
                if (_budgetItemAutoFillSetup == value)
                    return;

                _budgetItemAutoFillSetup = value;
                OnPropertyChanged();
            }
        }


        private AutoFillValue _budgetItemAutoFillValue;

        public AutoFillValue BudgetItemAutoFillValue
        {
            get => _budgetItemAutoFillValue;
            set
            {
                if (_budgetItemAutoFillValue == value)
                    return;

                _budgetItemAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _sourceAutoFillSetup;

        public AutoFillSetup SourceAutoFillSetup
        {
            get => _sourceAutoFillSetup;
            set
            {
                if (_sourceAutoFillSetup == value)
                    return;

                _sourceAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _sourceAutoFillValue;

        public AutoFillValue SourceAutoFillValue
        {
            get => _sourceAutoFillValue;
            set
            {
                if (_sourceAutoFillValue == value)
                    return;

                _sourceAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        protected override void Initialize()
        {
            BudgetItemAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.QifMaps.GetFieldDefinition(p => p.BudgetId));

            SourceAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.QifMaps.GetFieldDefinition(p => p.SourceId));

            base.Initialize();
        }

        protected override QifMap PopulatePrimaryKeyControls(QifMap newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var qifMap = AppGlobals.LookupContext.QifMaps.GetEntityFromPrimaryKeyValue(primaryKeyValue);
            var query = AppGlobals.DataRepository.GetDataContext().GetTable<QifMap>();
            qifMap = query.FirstOrDefault(p => p.Id == qifMap.Id);
            if (qifMap != null)
            {
                Id = qifMap.Id;
                KeyAutoFillValue =
                    AppGlobals.LookupContext.QifMaps.GetAutoFillValue(Id.ToString());

                return qifMap;
            }

            return qifMap;
        }

        protected override void LoadFromEntity(QifMap entity)
        {
            BudgetItemAutoFillValue =
                AppGlobals.LookupContext.BudgetItems.GetAutoFillValue(entity.BudgetId.ToString());

            if (entity.SourceId > 0)
            {
                SourceAutoFillValue = AppGlobals.LookupContext.BudgetItemSources.GetAutoFillValue(entity.SourceId.ToString());
            }
        }

        protected override QifMap GetEntityData()
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearData()
        {
            Id = 0;
            BudgetItemAutoFillValue = null;
            SourceAutoFillValue = null;
        }

        protected override bool SaveEntity(QifMap entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}
