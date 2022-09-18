using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;
using RingSoft.HomeLogix.Library.ViewModels.ImportBank;

namespace RingSoft.HomeLogix.ImportBank
{
    /// <summary>
    /// Interaction logic for ImportBankTransactionsBudgetsWindow.xaml
    /// </summary>
    public partial class ImportBankTransactionsBudgetsWindow : BaseWindow, IImportBankBudgetsView
    {
        public ImportBankTransactionsBudgetsWindow(ImportTransactionGridRow row)
        {
            InitializeComponent();
            ViewModel.Initialize(row, this);

            Loaded += (sender, args) =>
            {
                TransactionDateControl.SetReadOnlyMode(true);
                Grid.KeyDown += (sender, args) =>
                {
                    if (args.Key == Key.Enter)
                    {

                    }
                };

            };
            ContentRendered += (sender, args) => Grid.Focus();
            CancelButton.Click += (sender, args) => Close();
        }

        public void CloseWindow(bool dialogResult)
        {
            DialogResult = dialogResult;
            Close();
        }
    }
}
