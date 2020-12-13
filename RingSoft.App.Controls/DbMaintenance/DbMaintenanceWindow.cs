using RingSoft.DataEntryControls.WPF;
using System.Windows;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
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
    ///     <MyNamespace:DbMaintenanceWindow/>
    ///
    /// </summary>
    public abstract class DbMaintenanceWindow : BaseWindow
    {
        public abstract DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl { get; }

        static DbMaintenanceWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DbMaintenanceWindow), new FrameworkPropertyMetadata(typeof(DbMaintenanceWindow)));
        }

        public DbMaintenanceWindow()
        {
            Loaded += (sender, args) => OnLoaded();
        }

        protected virtual void OnLoaded()
        {
            DbMaintenanceTopHeaderControl.SaveSelectButton.Visibility = Visibility.Collapsed;

            DbMaintenanceTopHeaderControl.SaveSelectButton.Click += (sender, args) => MessageBox.Show("Save/Select");
        }
    }
}
