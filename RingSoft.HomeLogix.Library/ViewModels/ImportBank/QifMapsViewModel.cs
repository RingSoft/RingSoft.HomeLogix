using System.Linq;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public class QifMapsViewModel : DbMaintenanceViewModel<QifMap>
    {
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

        protected override void PopulatePrimaryKeyControls(QifMap newEntity, PrimaryKeyValue primaryKeyValue)
        {
            Id = newEntity.Id;
        }

        protected override QifMap GetEntityFromDb(QifMap newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var qifMap = AppGlobals.LookupContext.QifMaps.GetEntityFromPrimaryKeyValue(primaryKeyValue);
            var query = AppGlobals.DataRepository.GetDataContext().GetTable<QifMap>();
            qifMap = query.FirstOrDefault(p => p.Id == qifMap.Id);
            return qifMap; }

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
            var result = new QifMap();
            result.Id = Id;
            result.BankText = KeyAutoFillValue.Text;

            if (BudgetItemAutoFillValue.IsValid())
            {
                var budgetItem =
                    AppGlobals.LookupContext.BudgetItems.GetEntityFromPrimaryKeyValue(BudgetItemAutoFillValue
                        .PrimaryKeyValue);
                result.BudgetId = budgetItem.Id;
            }

            if (SourceAutoFillValue.IsValid())
            {
                var sourceItem =
                    AppGlobals.LookupContext.BudgetItemSources.GetEntityFromPrimaryKeyValue(SourceAutoFillValue
                        .PrimaryKeyValue);
                result.SourceId = sourceItem.Id;
            }

            return result;
        }

        protected override void ClearData()
        {
            Id = 0;
            BudgetItemAutoFillValue = null;
            SourceAutoFillValue = null;
        }

        protected override AutoFillValue GetAutoFillValueForNullableForeignKeyField(FieldDefinition fieldDefinition)
        {
            if (fieldDefinition == AppGlobals.LookupContext.QifMaps.GetFieldDefinition(p => p.SourceId))
            {
                return SourceAutoFillValue;
            }
            return base.GetAutoFillValueForNullableForeignKeyField(fieldDefinition);
        }

        protected override bool SaveEntity(QifMap entity)
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                var result = context.SaveEntity(entity, "Saving Qif Map");
                return result;
            }

            return false;
        }

        protected override bool DeleteEntity()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                var table = context.GetTable<QifMap>();
                var qifMap = table.FirstOrDefault(p => p.Id == Id);
                if (qifMap != null)
                {
                    return context.DeleteEntity(qifMap, "Deleting QifMap");
                }
            }
            return false;
        }
    }
}
