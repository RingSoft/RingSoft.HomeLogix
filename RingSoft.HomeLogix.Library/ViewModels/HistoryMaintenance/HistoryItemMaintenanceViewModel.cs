using System;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance
{
    public class HistoryItemMaintenanceViewModel : AppDbMaintenanceViewModel<History>
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

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date == value)
                {
                    return;
                }
                _date = value;
                OnPropertyChanged();
            }
        }

        private BankAccountRegisterItemTypes _itemType;

        public BankAccountRegisterItemTypes ItemType
        {
            get => _itemType;
            set
            {
                if (_itemType == value)
                {
                    return;
                }
                _itemType = value;
                OnPropertyChanged();
            }
        }


        public override TableDefinition<History> TableDefinition => AppGlobals.LookupContext.History;

        protected override void Initialize()
        {
            FindButtonLookupDefinition = AppGlobals.LookupContext.HistoryLookup.Clone();
            FindButtonLookupDefinition.ReadOnlyMode = false;
            base.Initialize();
        }

        protected override History PopulatePrimaryKeyControls(History newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var historyItem = AppGlobals.DataRepository.GetHistoryItem(newEntity.Id);
            Id = newEntity.Id;

            return historyItem;
        }

        protected override void LoadFromEntity(History entity)
        {
            
        }

        protected override History GetEntityData()
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearData()
        {
            
        }

        protected override bool SaveEntity(History entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool DeleteEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}
