using RingSoft.DataEntryControls.WPF;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.ImportBank
{
    /// <summary>
    /// Interaction logic for ImportBankTransactionsWindow.xaml
    /// </summary>
    public partial class ImportBankTransactionsWindow : BaseWindow
    {
        public ImportBankTransactionsWindow(BankAccountViewModel bankAccountViewModel)
        {
            InitializeComponent();
            ViewModel.Initialize(bankAccountViewModel);
            ContentRendered += (sender, args) => DataEntryGrid.Focus();
        }
    }
}
