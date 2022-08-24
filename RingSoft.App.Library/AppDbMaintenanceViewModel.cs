using RingSoft.DataEntryControls.Engine;
using RingSoft.DbMaintenance;

namespace RingSoft.App.Library
{
    public abstract class AppDbMaintenanceViewModel<TEntity> : DbMaintenanceViewModel<TEntity>
        where TEntity : new()
    {
        public RelayCommand SaveSelectButtonCommand { get; set; }

        public AppDbMaintenanceViewModel()
        {
            SaveSelectButtonCommand = new RelayCommand(OnSelectButton);
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (LookupAddViewArgs != null && LookupAddViewArgs.LookupReadOnlyMode)
                SaveSelectButtonCommand.IsEnabled = false;
        }

        public override void OnSelectButton()
        {
            if (SaveButtonEnabled)
            {
                var result = DoSave();
                if (result != DbMaintenanceResults.Success)
                    return;
            }

            base.OnSelectButton();
        }
    }
}
