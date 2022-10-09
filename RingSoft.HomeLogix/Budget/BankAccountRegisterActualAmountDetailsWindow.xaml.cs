using System.Windows.Input;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BankAccountRegisterActualAmountDetailsWindow.xaml
    /// </summary>
    public partial class BankAccountRegisterActualAmountDetailsWindow : IBankAccountRegisterActualAmountView
    {
        public BankAccountRegisterActualAmountDetailsWindow(ActualAmountCellProps actualAmountCellProps)
        {
            InitializeComponent();

            ContentRendered += (sender, args) =>
            {
                Grid.Focus();
            };

            CancelButton.Click += (sender, args) => Close();
            Loaded += (sender, args) =>
            {
                ViewModel.OnViewLoaded(this, actualAmountCellProps);
                Grid.PreviewKeyDown += (o, eventArgs) =>
                {
                    if (eventArgs.Key == Key.Enter)
                    {
                        if (Grid.EditingControlHost != null && Grid.EditingControlHost.HasDataChanged())
                        {
                            Grid.EditingControlHost.Row.IsNew = false;
                            Grid.EditingControlHost.Row.SetCellValue(Grid.EditingControlHost
                                .GetCellValue());
                        }

                        ViewModel.OkButtonCommand.Execute(null);
                        eventArgs.Handled = true;
                    }
                };
            };
        }

        public void OnOkButtonCloseWindow()
        {
            Close();
        }
    }
}
