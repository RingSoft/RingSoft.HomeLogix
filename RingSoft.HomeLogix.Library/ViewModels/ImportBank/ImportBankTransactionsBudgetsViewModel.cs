using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public class ImportBankTransactionsBudgetsViewModel : INotifyPropertyChanged
    {
        private string _bankText;

        public string BankText
        {
            get => _bankText;
            set
            {
                if (_bankText == value)
                {
                    return;
                }
                _bankText = value;
                OnPropertyChanged();
            }
        }

        public ImportTransactionGridRow Row { get; set; }

        public void Initialize(ImportTransactionGridRow row)
        {
            Row = row;
            BankText = row.Manager.ViewModel.BankAccountText;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
