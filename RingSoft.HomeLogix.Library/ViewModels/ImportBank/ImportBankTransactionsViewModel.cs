using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public interface IImportTransactionView
    {
        bool ShowImportBankBudgetWindow(ImportTransactionGridRow row);

        void CloseWindow(bool dialogResult);

        string GetQifFile();
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

        public RelayCommand ImportQifCommand { get; set; }

        public ImportBankTransactionsViewModel()
        {
            OkCommand = new RelayCommand(OnOk);
            ImportQifCommand = new RelayCommand(ImportQif);
        }

        public void Initialize(BankAccountViewModel bankAccountViewModel, IImportTransactionView view)
        {
            BankViewModel = bankAccountViewModel;
            View = view;
            BankAccountText = bankAccountViewModel.KeyAutoFillValue.Text;
            Manager = new ImportTransactionsGridManager(this);
            Manager.LoadGrid();
        }

        private void OnOk()
        {
            if (Manager.Rows.FirstOrDefault(p => p.IsNew == false) != null)
            {
                var message = "Do you wish to post the transactions to the register?";
                var caption = "Post To Register?";
                var result = ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption, true);
                if (result == MessageBoxButtonsResult.Yes)
                {
                    Manager.PostTransactions();
                    return;
                }

                if (Manager.SaveTransactions())
                {
                    View.CloseWindow(true);
                }
            }
            else
            {
                if (AppGlobals.DataRepository.DeleteTransactions(BankViewModel.Id))
                    View.CloseWindow(false);
            }
        }

        private void ImportQif()
        {
            var qifText = View.GetQifFile();
            if (qifText.IsNullOrEmpty())
            {
                return;
            }

            var registerStartDate = DateTime.MinValue;
            var startDate = DateTime.MinValue;
            var incompleteRows = BankViewModel.RegisterGridManager.Rows.OfType<BankAccountRegisterGridRow>().Where(p => !p.Completed);
            if (incompleteRows.Any())
            {
                registerStartDate = incompleteRows.Min(p => p.ItemDate);
            }

            if (BankViewModel.LastCompleteDate == DateTime.MinValue)
            {
                startDate = registerStartDate;
            }
            else
            {
                
                startDate = BankViewModel.LastCompleteDate;
            }
            
            var importRows = new List<ImportTransactionGridRow>();

            var columnPos = qifText.IndexOf("C*");

            while (columnPos >= 0)
            {
                var row = GetRowValue(qifText, columnPos, startDate);
                if (row == null)
                {
                    FinishImport(importRows);
                    return;
                }
                importRows.Add(row);
                //columnPos = qifText.IndexOf("^", columnPos);
                columnPos = qifText.IndexOf("C*", columnPos + 2);
            }
            FinishImport(importRows);
        }

        private void FinishImport(List<ImportTransactionGridRow> importRows)
        {
            importRows = importRows.OrderBy(p => p.Date).ToList();
            Manager.ImportFromQif(importRows);
            if (importRows.Any())
            {
                Manager.Grid?.GotoCell(importRows[0], ImportTransactionGridRow.BudgetItemColumnId);
            }
            else
            {
                ShowNothingToDo();
            }
        }

        private static void ShowNothingToDo()
        {
            var message = "Nothing was found to import.";
            var caption = "Nothing To Do";
            ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
        }

        private ImportTransactionGridRow GetRowValue(string qifText, int columnPos, DateTime startDate)
        {
            var date = GetQifValue(qifText, columnPos, "D");
            if (date.IsNullOrEmpty())
                return null;

            var rowDate = DateTime.MinValue;
            if (!DateTime.TryParse(date, out rowDate))
                return null;

            if (rowDate > startDate)
            {
                var text = GetQifValue(qifText, columnPos, "P");
                var amountText = GetQifValue(qifText, columnPos, "T");

                var amount = (decimal) 0;
                if (!decimal.TryParse(amountText, out amount))
                    return null;

                var row = new ImportTransactionGridRow(Manager);
                if (amount < 0)
                {
                    row.TransactionTypes = TransactionTypes.Withdrawal;
                }
                else
                {
                    row.TransactionTypes = TransactionTypes.Deposit;
                }
                amount = Math.Abs(amount);
                row.Amount = amount;
                row.Date = rowDate;
                row.BankText = text;
                row.MapTransaction = true;
                return row;
            }
            else
            {
                return null;
            }
        }

        private static string GetQifValue(string qifText, int columnPos, string prefix)
        {
            var crlfText = "\n";
            var checkPrefix = $"{crlfText}{prefix}";
            var valuePos = qifText.IndexOf(checkPrefix, columnPos);
            if (valuePos < 0)
            {
                crlfText = "\r\n";
                checkPrefix = $"{crlfText}{prefix}";
                valuePos = qifText.IndexOf(checkPrefix, columnPos);
                if (valuePos < 0)
                {
                    return string.Empty;
                }
            }
            var crLfPos = qifText.IndexOf(crlfText, valuePos + checkPrefix.Length);
            var result = qifText.MidStr(valuePos, crLfPos - valuePos).Trim();
            if (result.StartsWith(prefix))
            {
                result = result.RightStr(result.Length - 1);
            }
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
