using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RingSoft.App.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.App.Controls.DbMaintenance"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.App.Controls.DbMaintenance;assembly=RingSoft.App.Controls.DbMaintenance"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:AdvancedFindAdditionalButtons/>
    ///
    /// </summary>
    public class AdvancedFindAdditionalButtons : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton ImportDefaultLookupButton { get; set; }

        public DbMaintenanceButton ApplyToLookupButton { get; set; }

        public DbMaintenanceButton SqlViewerButton { get; set; }

        public DbMaintenanceButton RefreshSettingsButton { get; set; }

        static AdvancedFindAdditionalButtons()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedFindAdditionalButtons), new FrameworkPropertyMetadata(typeof(AdvancedFindAdditionalButtons)));
        }

        public override void OnApplyTemplate()
        {
            ImportDefaultLookupButton = GetTemplateChild(nameof(ImportDefaultLookupButton)) as DbMaintenanceButton;
            ApplyToLookupButton = GetTemplateChild(nameof(ApplyToLookupButton)) as DbMaintenanceButton;
            SqlViewerButton = GetTemplateChild(nameof(SqlViewerButton)) as DbMaintenanceButton;
            RefreshSettingsButton = GetTemplateChild(nameof(RefreshSettingsButton)) as DbMaintenanceButton;

            ImportDefaultLookupButton.ToolTip.HeaderText = "Import Default Lookup (Alt + I)";
            ImportDefaultLookupButton.ToolTip.DescriptionText =
                "Import the default lookup for this table.";

            ApplyToLookupButton.ToolTip.HeaderText = "Apply To Lookup (Alt + T)";
            ApplyToLookupButton.ToolTip.DescriptionText =
                "Apply this advanced find to the default lookup.";

            SqlViewerButton.ToolTip.HeaderText = "View SQL Statement (Alt + Q)";
            SqlViewerButton.ToolTip.DescriptionText =
                "Show the SQL statement that this advanced find generates.";

            RefreshSettingsButton.ToolTip.HeaderText = "Refresh Settings (Alt + R)";
            RefreshSettingsButton.ToolTip.DescriptionText =
                "Set up properties to define when this advanced find automatically refreshes and set up alert levels.";

            base.OnApplyTemplate();
        }
    }
}
