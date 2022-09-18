using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RingSoft.DataEntryControls.WPF;
using RingSoft.HomeLogix.Library.ViewModels.ImportBank;

namespace RingSoft.HomeLogix.ImportBank
{
    /// <summary>
    /// Interaction logic for ImportBankTransactionsBudgetsWindow.xaml
    /// </summary>
    public partial class ImportBankTransactionsBudgetsWindow : BaseWindow
    {
        public ImportBankTransactionsBudgetsWindow(ImportTransactionGridRow row)
        {
            InitializeComponent();
            ViewModel.Initialize(row);

            Loaded += (sender, args) => TransactionDateControl.SetReadOnlyMode(true);
            ContentRendered += (sender, args) => Grid.Focus();
        }
    }
}
