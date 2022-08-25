using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.App.Controls
{
    public class AppDbMaintenanceButtonsFactory : DbMaintenanceButtonsFactory
    {
        public override Control GetButtonsControl()
        {
            return new DbMaintenanceTopHeaderControl();
        }

    }
}
