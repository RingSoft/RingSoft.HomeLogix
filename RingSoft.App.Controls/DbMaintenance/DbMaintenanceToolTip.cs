using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;

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
    ///     <MyNamespace:DbMaintenanceToolTip/>
    ///
    /// </summary>
    [TemplatePart (Name = "HeaderTextBlock", Type = typeof(TextBlock))]
    public class DbMaintenanceToolTip : ToolTip
    {
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.RegisterAttached(nameof(HeaderText), typeof(string), typeof(DbMaintenanceToolTip),
                new FrameworkPropertyMetadata(null, HeaderTextChangedCallback));

        public string HeaderText
        {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        private static void HeaderTextChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dbMaintenanceToolTip = (DbMaintenanceToolTip)obj;
            dbMaintenanceToolTip.SetHeaderText();
        }
        public TextBlock HeaderTextBlock { get; set; }

        static DbMaintenanceToolTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DbMaintenanceToolTip), new FrameworkPropertyMetadata(typeof(DbMaintenanceToolTip)));
        }

        public override void OnApplyTemplate()
        {
            HeaderTextBlock = GetTemplateChild(nameof(HeaderTextBlock)) as TextBlock;

            SetHeaderText();
            base.OnApplyTemplate();
        }

        private void SetHeaderText()
        {
            if (HeaderTextBlock != null && !HeaderText.IsNullOrEmpty())
                HeaderTextBlock.Text = HeaderText;
        }
    }
}
