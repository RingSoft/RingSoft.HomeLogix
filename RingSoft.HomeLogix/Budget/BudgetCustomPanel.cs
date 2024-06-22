using RingSoft.App.Controls;
using System.Windows;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.HomeLogix.Budget"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.HomeLogix.Budget;assembly=RingSoft.HomeLogix.Budget"
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
    ///     <MyNamespace:BudgetCustomPanel/>
    ///
    /// </summary>
    [TemplatePart(Name = "AddButton", Type = typeof(DbMaintenanceButton))]
    [TemplatePart(Name = "ClearRecurButton", Type = typeof(DbMaintenanceButton))]

    public class BudgetCustomPanel : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton AddButton { get; set; }

        public DbMaintenanceButton ClearRecurButton { get; set; }


        static BudgetCustomPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BudgetCustomPanel), new FrameworkPropertyMetadata(typeof(BudgetCustomPanel)));
        }

        public override void OnApplyTemplate()
        {
            AddButton = GetTemplateChild(nameof(AddButton)) as DbMaintenanceButton;
            ClearRecurButton = GetTemplateChild(nameof(ClearRecurButton)) as DbMaintenanceButton;


            if (AddButton != null)
            {
                AddButton.ToolTip.HeaderText = "Add New Adjustment (Alt + A)";
                AddButton.ToolTip.DescriptionText = "Add an adjustment to history and the statistics.";
            }

            if (ClearRecurButton != null)
            {
                ClearRecurButton.ToolTip.HeaderText = "Clear Recurring (Alt + R)";
                ClearRecurButton.ToolTip.DescriptionText = "Clear recurring values.";
            }


            base.OnApplyTemplate();
        }
    }
}
