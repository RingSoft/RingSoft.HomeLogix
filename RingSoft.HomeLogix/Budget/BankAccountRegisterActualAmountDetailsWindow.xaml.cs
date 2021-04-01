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

            ViewModel.OnViewLoaded(this, actualAmountCellProps);

            ContentRendered += (sender, args) =>
            {
                Grid.Focus();
            };

            CancelButton.Click += (sender, args) => Close();
        }

        public void OnOkButtonCloseWindow()
        {
            Close();
        }
    }
}
