using System.Windows;
using System.Windows.Controls;

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
    ///     <MyNamespace:DbMaintenanceTopHeader/>
    ///
    /// </summary>
    [TemplatePart(Name = "PreviousButton", Type = typeof(DbMaintenanceButton))]
    [TemplatePart(Name = "SaveButton", Type = typeof(DbMaintenanceButton))]
    [TemplatePart(Name = "SaveSelectButton", Type = typeof(DbMaintenanceButton))]
    [TemplatePart(Name = "DeleteButton", Type = typeof(DbMaintenanceButton))]
    [TemplatePart(Name = "FindButton", Type = typeof(DbMaintenanceButton))]
    [TemplatePart(Name = "NewButton", Type = typeof(DbMaintenanceButton))]
    [TemplatePart(Name = "CloseButton", Type = typeof(DbMaintenanceButton))]
    [TemplatePart(Name = "CustomStackPanel", Type = typeof(StackPanel))]
    [TemplatePart(Name = "NextButton", Type = typeof(DbMaintenanceButton))]
    public class DbMaintenanceTopHeaderControl : Control
    {
        public static readonly DependencyProperty CustomPanelProperty =
            DependencyProperty.RegisterAttached(nameof(CustomPanel), typeof(DbMaintenanceCustomPanel),
                typeof(DbMaintenanceTopHeaderControl),
                new FrameworkPropertyMetadata(null, CustomPanelChangedCallback));

        public DbMaintenanceCustomPanel CustomPanel
        {
            get => (DbMaintenanceCustomPanel)GetValue(CustomPanelProperty);
            set => SetValue(CustomPanelProperty, value);
        }

        private static void CustomPanelChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var topHeaderControl = (DbMaintenanceTopHeaderControl)obj;
            topHeaderControl.SetCustomPanel();
        }

        public DbMaintenanceButton PreviousButton { get; set; }
        public DbMaintenanceButton SaveButton { get; set; }
        public DbMaintenanceButton SaveSelectButton { get; set; }
        public DbMaintenanceButton DeleteButton { get; set; }
        public DbMaintenanceButton FindButton { get; set; }
        public DbMaintenanceButton NewButton { get; set; }
        public DbMaintenanceButton CloseButton { get; set; }
        public StackPanel CustomStackPanel { get; set; }
        public DbMaintenanceButton NextButton { get; set; }

        static DbMaintenanceTopHeaderControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DbMaintenanceTopHeaderControl), new FrameworkPropertyMetadata(typeof(DbMaintenanceTopHeaderControl)));
        }

        public override void OnApplyTemplate()
        {
            PreviousButton = GetTemplateChild(nameof(PreviousButton)) as DbMaintenanceButton;

            SaveButton = GetTemplateChild(nameof(SaveButton)) as DbMaintenanceButton;
            SaveSelectButton = GetTemplateChild(nameof(SaveSelectButton)) as DbMaintenanceButton;
            DeleteButton = GetTemplateChild(nameof(DeleteButton)) as DbMaintenanceButton;
            FindButton = GetTemplateChild(nameof(FindButton)) as DbMaintenanceButton;
            NewButton = GetTemplateChild(nameof(NewButton)) as DbMaintenanceButton;
            CloseButton = GetTemplateChild(nameof(CloseButton)) as DbMaintenanceButton;

            CustomStackPanel = GetTemplateChild(nameof(CustomStackPanel)) as StackPanel;

            NextButton = GetTemplateChild(nameof(NextButton)) as DbMaintenanceButton;

            SetCustomPanel();

            base.OnApplyTemplate();
        }

        private void SetCustomPanel()
        {
            if (CustomStackPanel != null && CustomPanel != null)
            {
                CustomStackPanel.Children.Clear();
                CustomStackPanel.Children.Add(CustomPanel);
                UpdateLayout();
            }
        }
    }
}
