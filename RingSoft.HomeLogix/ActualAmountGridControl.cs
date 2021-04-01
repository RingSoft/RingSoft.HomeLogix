using System;
using System.Media;
using RingSoft.DataEntryControls.WPF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RingSoft.HomeLogix
{
    public enum ActualAmountMode
    {
        Value = 0,
        Details = 1
    }
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

        private ActualAmountMode _amountMode;

        public ActualAmountMode AmountMode
        {
            get => _amountMode;
            set
            {
                if (_amountMode == value)
                    return;

                _amountMode = value;
                SetAmountMode();
            }
        }

        public event EventHandler ShowDetailsWindow;

        static ActualAmountGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ActualAmountGridControl), new FrameworkPropertyMetadata(typeof(ActualAmountGridControl)));
        }

        public override void OnApplyTemplate()
        {
            DetailsButton = GetTemplateChild(nameof(DetailsButton)) as Button;

            if (DetailsButton != null) 
                DetailsButton.Click += (_, _) => OnDetailsButtonClick();

            base.OnApplyTemplate();

            SetAmountMode();
        }

        private void SetAmountMode()
        {
            switch (AmountMode)
            {
                case ActualAmountMode.Value:
                    TextBox.IsReadOnly = false;
                    TextBox.IsReadOnlyCaretVisible = true;
                    DropDownButton.IsEnabled = true;
                    break;
                case ActualAmountMode.Details:
                    TextBox.IsReadOnly = true;
                    TextBox.IsReadOnlyCaretVisible = true;
                    DropDownButton.IsEnabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override bool ProcessKeyChar(char keyChar)
        {
            switch (AmountMode)
            {
                case ActualAmountMode.Value:
                    break;
                case ActualAmountMode.Details:
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
                            if (MessageBox.Show("This contains multiple values.  Would you like to edit?", "Multiple Details",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) 
                                OnDetailsButtonClick();
                            return false;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.ProcessKeyChar(keyChar);
        }

        protected override bool ProcessKey(Key key)
        {
            if (key == Key.F5)
            {
                OnDetailsButtonClick();
                return false;
            }

            return base.ProcessKey(key);
        }

        private void OnDetailsButtonClick()
        {
            ShowDetailsWindow?.Invoke(this, EventArgs.Empty);
        }
    }
}
