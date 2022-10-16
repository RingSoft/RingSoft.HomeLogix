using System.Windows;
using System.Windows.Controls;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.App.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.App.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.App.Controls;assembly=RingSoft.App.Controls"
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
    ///     <MyNamespace:TwoTierProgressWindow/>
    ///
    /// </summary>
    public class TwoTierProgressWindow : Window, ISplashWindow
    {
        public Border Border { get; set; }
        public TwoTierProgressViewModel ViewModel { get; set; }

        public bool IsDisposed => false;
        public bool Disposing => false;

        static TwoTierProgressWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TwoTierProgressWindow), new FrameworkPropertyMetadata(typeof(TwoTierProgressWindow)));
        }

        public TwoTierProgressWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("TwoTierViewModel") as TwoTierProgressViewModel;

            base.OnApplyTemplate();
        }

        public void SetProgress(string progressText)
        {
        }

        public void ShowError(string message, string title)
        {
            Dispatcher.Invoke(() => MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error));
        }

        public void CloseSplash()
        {
            Dispatcher.Invoke(() => Close());
        }

        public void UpdateTopTier(string text, int maxCount, int currentItem)
        {
            Dispatcher.Invoke(() =>
            {
                ViewModel.TopTierText = text;
                ViewModel.TopTierMaximum =maxCount;
                ViewModel.TopTierProgress = currentItem;
            });
        }

        public void SetWindowText(string text)
        {
            Dispatcher.Invoke(() => Title = text);
        }
    }
}
