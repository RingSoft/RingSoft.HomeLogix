using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.App.Controls
{
    public class AppDbMaintenanceProcessorFactory : RingSoft.DbLookup.Controls.WPF.DbMaintenanceProcessorFactory
    {
        public override IDbMaintenanceProcessor GetProcessor()
        {
            return new AppDbMaintenanceWindowProcessor();
        }
    }
}
