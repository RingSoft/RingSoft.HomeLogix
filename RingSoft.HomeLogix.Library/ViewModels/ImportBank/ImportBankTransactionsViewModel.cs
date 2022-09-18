using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public interface IImportTransactionView
    {
        bool ShowImportBankBudgetWindow(ImportTransactionGridRow row);

        void CloseWindow(bool dialogResult);
    }
    public class ImportBankTransactionsViewModel :INotifyPropertyChanged
    {
        private string _bankAccountText;

        public string BankAccountText
        {
            get => _bankAccountText;
            set
            {
                if (_bankAccountText == value)
                {
                    return;
                }
                _bankAccountText = value;
                OnPropertyChanged();
            }
        }

        private ImportTransactionsGridManager _manager;

        public ImportTransactionsGridManager Manager
        {
            get => _manager;
            set
            {
                if (_manager == value)
                {
                    return;
                }
                _manager = value;
                OnPropertyChanged();
            }
        }


        public BankAccountViewModel BankViewModel { get; set; }

        public IImportTransactionView View { get; set; }

        public RelayCommand OkCommand { get; set; }

        public ImportBankTransactionsViewModel()
        {
            OkCommand = new RelayCommand(OnOk);
        }

        public void Initialize(BankAccountViewModel bankAccountViewModel, IImportTransactionView view)
        {
            BankViewModel = bankAccountViewModel;
            View = view;
            BankAccountText = bankAccountViewModel.KeyAutoFillValue.Text;
            Manager = new ImportTransactionsGridManager(this);
        }

        private void OnOk()
        {
            View.CloseWindow(true);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
