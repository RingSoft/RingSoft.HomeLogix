using RingSoft.App.Controls;
using RingSoft.App.Library;
using RingSoft.HomeLogix.Library.ViewModels.ImportBank;

namespace RingSoft.HomeLogix
{
    public class ImportQifProcedure : AppProcedure
    {
        public override ISplashWindow SplashWindow => _splashWindow;

        public ImportBankTransactionsViewModel ViewModel { get; }

        private ProcessingSplashWindow _splashWindow;
        private string _qifText;

        public ImportQifProcedure(ImportBankTransactionsViewModel viewModel, string qifText)
        {
            ViewModel = viewModel;
            _qifText = qifText;
        }
        protected override void ShowSplash()
        {
            _splashWindow = new ProcessingSplashWindow("Importing QIF File");
            _splashWindow.ShowDialog();

        }

        protected override bool DoProcess()
        {
            ViewModel.ImportQifFile(_qifText);
            _splashWindow.CloseSplash();
            return true;
        }
    }
}
