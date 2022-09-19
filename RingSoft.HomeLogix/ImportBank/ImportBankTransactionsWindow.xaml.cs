using System.Linq;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using RingSoft.HomeLogix.Library.ViewModels.ImportBank;

namespace RingSoft.HomeLogix.ImportBank
{
    /// <summary>
    /// Interaction logic for ImportBankTransactionsWindow.xaml
    /// </summary>
    public partial class ImportBankTransactionsWindow : BaseWindow , IImportTransactionView
    {
        public ImportBankTransactionsWindow(BankAccountViewModel bankAccountViewModel)
        {
            InitializeComponent();
            ViewModel.Initialize(bankAccountViewModel, this);
            ContentRendered += (sender, args) => DataEntryGrid.Focus();
            Loaded += (sender, args) =>
            {
                DataEntryGrid.PreviewKeyDown += (o, eventArgs) =>
                {
                    if (eventArgs.Key == Key.Enter)
                    {
                        if (DataEntryGrid.EditingControlHost != null && DataEntryGrid.EditingControlHost.HasDataChanged())
                        {
                            DataEntryGrid.EditingControlHost.Row.IsNew = false;
                            DataEntryGrid.EditingControlHost.Row.SetCellValue(DataEntryGrid.EditingControlHost
                                .GetCellValue());
                        }

                        ViewModel.OkCommand.Execute(null);
                        eventArgs.Handled = true;
                    }
                };
            };
        }

        public bool ShowImportBankBudgetWindow(ImportTransactionGridRow row)
        {
            var window = new ImportBankTransactionsBudgetsWindow(row);
            window.Owner = this;
            window.ShowInTaskbar = false;
            window.ShowDialog();
            return window.DialogResult != null && window.DialogResult.Value;
        }

        public void CloseWindow(bool dialogResult)
        {
            DialogResult = dialogResult;
            Close();
        }
    }
}
