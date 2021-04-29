using RingSoft.DbLookup.Controls.WPF;
using System;
using System.Windows;
using System.Windows.Input;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.HomeLogix"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.HomeLogix;assembly=RingSoft.HomeLogix"
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
    ///     <MyNamespace:RegisterGridBudgetItemAutoFillControl/>
    ///
    /// </summary>
    public class RegisterGridBudgetItemAutoFillControl : AutoFillControl
    {
        public event EventHandler ShowBudgetWindow;

        static RegisterGridBudgetItemAutoFillControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RegisterGridBudgetItemAutoFillControl), new FrameworkPropertyMetadata(typeof(RegisterGridBudgetItemAutoFillControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TextBox.PreviewTextInput += (sender, args) =>
            {
                args.Handled = true;
                ShowLookupWindow();
            };

            TextBox.PreviewKeyDown += (sender, args) =>
            {
                if (args.Key == Key.F5)
                    ShowLookupWindow();
            };
            Button.Click += (sender, args) => ShowLookupWindow();
        }

        protected override void ShowLookupWindow()
        {
            ShowBudgetWindow?.Invoke(this, EventArgs.Empty);
        }
    }
}
