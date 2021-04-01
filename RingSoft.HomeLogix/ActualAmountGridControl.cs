using System.Media;
using RingSoft.DataEntryControls.WPF;
using System.Windows;
using System.Windows.Controls;
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
    ///     <MyNamespace:ActualAmountGridControl/>
    ///
    /// </summary>
    
    [TemplatePart(Name = "DetailsButton", Type = typeof(Button))]
    public class ActualAmountGridControl : DecimalEditControl
    {
        public Button DetailsButton { get; private set; }

        static ActualAmountGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ActualAmountGridControl), new FrameworkPropertyMetadata(typeof(ActualAmountGridControl)));
        }

        public override void OnApplyTemplate()
        {
            DetailsButton = GetTemplateChild(nameof(DetailsButton)) as Button;
            
            base.OnApplyTemplate();

            TextBox.IsReadOnly = true;
            TextBox.IsReadOnlyCaretVisible = true;
            DropDownButton.IsEnabled = false;
        }

        protected override bool ProcessKeyChar(char keyChar)
        {
            switch (keyChar)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '-':
                case '.':
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("This contains multiple values.  Would you like to edit?", "Multiple Details",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    return false;
                    break;
            }
            return base.ProcessKeyChar(keyChar);
        }

        protected override bool ProcessKey(Key key)
        {
            return base.ProcessKey(key);
        }
    }
}
