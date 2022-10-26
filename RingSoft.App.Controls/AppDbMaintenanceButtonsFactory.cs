using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.App.Controls
{
    public class AppDbMaintenanceButtonsFactory : DbMaintenanceButtonsFactory
    {
        public override Control GetButtonsControl()
        {
            return new DbMaintenanceTopHeaderControl();
        }
        public override Control GetAdvancedFindButtonsControl(AdvancedFindViewModel viewModel)
        {
            var result = new DbMaintenanceTopHeaderControl();
            var additionalButtons = new AdvancedFindAdditionalButtons();
            result.Loaded += (sender, args) =>
            {
                additionalButtons.Loaded += (o, eventArgs) =>
                {
                    additionalButtons.ImportDefaultLookupButton.Command = viewModel.ImportDefaultLookupCommand;
                    additionalButtons.ApplyToLookupButton.Command = viewModel.ApplyToLookupCommand;
                    additionalButtons.SqlViewerButton.Command = viewModel.ShowSqlCommand;
                    additionalButtons.RefreshSettingsButton.Command = viewModel.RefreshSettingsCommand;
                };
                result.CustomDockPanel.Children.Add(additionalButtons);
                result.UpdateLayout();
            
            };
            return result;
        }

        public override Control GetRecordLockingButtonsControl(RecordLockingViewModel viewModel)
        {
            return new DbMaintenanceTopHeaderControl();
        }
    }
}
