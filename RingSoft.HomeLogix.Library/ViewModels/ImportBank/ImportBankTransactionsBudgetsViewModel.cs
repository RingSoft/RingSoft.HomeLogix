using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup;

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

        private decimal _transactionAmountDecimal;

        public decimal TransactionAmount
        {
            get => _transactionAmountDecimal;
            set
            {
                if (_transactionAmountDecimal == value)
                {
                    return;
                }
                _transactionAmountDecimal = value;
                OnPropertyChanged();
            }
        }

        private string _source;

        public string Source
        {
            get => _source;
            set
            {
                if (_source == value)
                {
                    return;
                }
                _source = value;

                OnPropertyChanged();
            }
        }

        private DateTime _transactionDate;

        public DateTime TransactionDate
        {
            get => _transactionDate;
            set
            {
                if (_transactionDate == value)
                {
                    return;
                }
                _transactionDate = value;
                OnPropertyChanged();
            }
        }

        private ImportBankTransactionsBudgetManager _gridManager;

        public ImportBankTransactionsBudgetManager GridManager
        {
            get => _gridManager;
            set
            {
                if (_gridManager == value)
                {
                    return;
                }
                _gridManager = value;
                OnPropertyChanged();
            }
        }


        public ImportTransactionGridRow Row { get; set; }

        public void Initialize(ImportTransactionGridRow row)
        {
            Row = row;
            BankText = row.Manager.ViewModel.BankAccountText;
            if (row.SourceAutoFillValue != null && row.SourceAutoFillValue.IsValid())
            {
                Source = row.SourceAutoFillValue.Text;
            }
            TransactionAmount = row.Amount;
            TransactionDate = row.Date;
            GridManager = new ImportBankTransactionsBudgetManager(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
