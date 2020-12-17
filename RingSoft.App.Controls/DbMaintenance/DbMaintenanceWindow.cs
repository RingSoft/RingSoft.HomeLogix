using RingSoft.App.Library;
using RingSoft.DataEntryControls.WPF;
using System.Windows;

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

        public abstract string ItemText { get; }

        static DbMaintenanceWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DbMaintenanceWindow), new FrameworkPropertyMetadata(typeof(DbMaintenanceWindow)));
        }

        public DbMaintenanceWindow()
        {
            Loaded += (sender, args) =>
            {
                DbMaintenanceTopHeaderControl.PreviousButton.ToolTip.HeaderText = "Previous (Alt + Left Arrow)";
                DbMaintenanceTopHeaderControl.PreviousButton.ToolTip.DescriptionText =
                    $"Go to the previous {ItemText} in the database.";

                DbMaintenanceTopHeaderControl.SaveButton.ToolTip.HeaderText = "Save (Alt + S)";
                DbMaintenanceTopHeaderControl.SaveButton.ToolTip.DescriptionText =
                    $"Save this {ItemText} to the database.";

                DbMaintenanceTopHeaderControl.SaveSelectButton.ToolTip.HeaderText = "Save/Select (Alt + L)";
                DbMaintenanceTopHeaderControl.SaveSelectButton.ToolTip.DescriptionText =
                    $"Save and select this {ItemText}.";

                DbMaintenanceTopHeaderControl.DeleteButton.ToolTip.HeaderText = "Delete (Alt + D)";
                DbMaintenanceTopHeaderControl.DeleteButton.ToolTip.DescriptionText =
                    $"Delete this {ItemText} from the database.";

                DbMaintenanceTopHeaderControl.FindButton.ToolTip.HeaderText = "Find (Alt + F)";
                DbMaintenanceTopHeaderControl.FindButton.ToolTip.DescriptionText =
                    $"Find {ItemText.GetArticle()} {ItemText} in the database.";

                DbMaintenanceTopHeaderControl.NewButton.ToolTip.HeaderText = "New (Alt + N)";
                DbMaintenanceTopHeaderControl.NewButton.ToolTip.DescriptionText =
                    $"Clear existing {ItemText} data in this window and create a new {ItemText}.";

                DbMaintenanceTopHeaderControl.CloseButton.ToolTip.HeaderText = "Close (Alt + C)";
                DbMaintenanceTopHeaderControl.CloseButton.ToolTip.DescriptionText = "Close this window.";

                DbMaintenanceTopHeaderControl.NextButton.ToolTip.HeaderText = "Next (Alt + Right Arrow)";
                DbMaintenanceTopHeaderControl.NextButton.ToolTip.DescriptionText =
                    $"Go to the next {ItemText} in the database.";

                OnLoaded();
            };
        }

        protected virtual void OnLoaded()
        {
            DbMaintenanceTopHeaderControl.SaveSelectButton.Visibility = Visibility.Collapsed;

            DbMaintenanceTopHeaderControl.SaveSelectButton.Click += (sender, args) => MessageBox.Show("Save/Select");
        }
    }
}
