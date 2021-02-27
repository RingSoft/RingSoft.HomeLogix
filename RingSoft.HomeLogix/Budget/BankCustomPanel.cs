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
    ///     <MyNamespace:BankCustomPanel/>
    ///
    /// </summary>
    [TemplatePart(Name = "GenerateButton", Type = typeof(DbMaintenanceButton))]
    [TemplatePart(Name = "AddButton", Type = typeof(DbMaintenanceButton))]
    public class BankCustomPanel : DbMaintenanceCustomPanel
    {
        public DbMaintenanceButton GenerateButton { get; set; }
        public DbMaintenanceButton AddButton { get; set; }

        static BankCustomPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BankCustomPanel), new FrameworkPropertyMetadata(typeof(BankCustomPanel)));
        }

        public override void OnApplyTemplate()
        {
            GenerateButton = GetTemplateChild(nameof(GenerateButton)) as DbMaintenanceButton;
            AddButton = GetTemplateChild(nameof(AddButton)) as DbMaintenanceButton;

            GenerateButton.ToolTip.HeaderText = "Generate Register Items From Budget (Alt + R)";
            GenerateButton.ToolTip.DescriptionText = "Generate register items from budget.";

            if (AddButton != null)
            {
                AddButton.ToolTip.HeaderText = "Add New Register Item (Alt + I)";
                AddButton.ToolTip.DescriptionText = "Add a new miscellaneous item to the register.";
            }

            base.OnApplyTemplate();
        }
    }
}
