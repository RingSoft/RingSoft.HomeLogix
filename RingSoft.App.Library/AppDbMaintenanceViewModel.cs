using RingSoft.DbMaintenance;

namespace RingSoft.App.Library
{
    public abstract class AppDbMaintenanceViewModel<TEntity> : DbMaintenanceViewModel<TEntity>
        where TEntity : new()
    {
        public override void OnSelectButton()
        {
            if (SaveButtonEnabled)
                if (DoSave() != DbMaintenanceResults.Success)
                    return;
            
            base.OnSelectButton();
        }
    }
}
