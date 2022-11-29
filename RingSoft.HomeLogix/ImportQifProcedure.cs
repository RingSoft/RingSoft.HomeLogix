using System;
using System.Windows;
using RingSoft.App.Controls;
using RingSoft.App.Library;
using RingSoft.HomeLogix.Library.ViewModels.ImportBank;

namespace RingSoft.HomeLogix
{
    public class ImportQifProcedure : AppProcedure
    {
        public override ISplashWindow SplashWindow => _splashWindow;

        public ImportBankTransactionsViewModel ViewModel { get; }

        public string QifFile { get; set; }

        private ProcessingSplashWindow _splashWindow;
        private ImportProcedures _procedure;

        public ImportQifProcedure(ImportBankTransactionsViewModel viewModel, ImportProcedures procedure)
        {
            ViewModel = viewModel;
            _procedure = procedure;
        }
        protected override void ShowSplash()
        {
            switch (_procedure)
            {
                case ImportProcedures.ImportingQif:
                    _splashWindow = new ProcessingSplashWindow("Importing QIF File");
                    break;
                case ImportProcedures.PostingQif:
                    _splashWindow = new ProcessingSplashWindow("Posting Transactions to Register");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _splashWindow.ShowDialog();

        }

        protected override bool DoProcess()
        {
            switch (_procedure)
            {
                case ImportProcedures.ImportingQif:
                    ViewModel.ImportQifFile(QifFile);
                    break;
                case ImportProcedures.PostingQif:
                    ViewModel.Manager.PostTransactions();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _splashWindow.CloseSplash();
            return true;
        }

        public bool ShowYesNoMessageBox(string message, string caption)
        {
            var result = false;
            var window = _splashWindow as Window;
            if (window != null)
            {
                window.Dispatcher.Invoke(() =>
                {
                    if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        result = true;
                    }
                });
            }
            return result;
        }
    }
}
