using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public interface IBudgetItemAdjustmentView
    {
        void OnOkButtonCloseWindow();
    }

    public class BudgetItemAdjustmentViewModel : INotifyPropertyChanged
    {
        private string _budgetItemDescription;

        public string BudgetItemDescription
        {
            get => _budgetItemDescription;
            set
            {
                if (_budgetItemDescription == value)
                    return;

                _budgetItemDescription = value;
                OnPropertyChanged();
            }
        }

        private string _type;

        public string Type
        {
            get => _type;
            set
            {
                if (_type == value)
                    return;

                _type = value;
                OnPropertyChanged();
            }
        }

        private DateTime _date;

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date == value)
                    return;
                ;

                _date = value;
                OnPropertyChanged();
            }
        }

        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                if (_description == value)
                    return;
                
                _description = value;
                OnPropertyChanged();
            }
        }

        private decimal _amount;

        public decimal Amount
        {
            get => _amount;
            set
            {
                if (_amount == value)
                    return;

                _amount = value;
                OnPropertyChanged();
            }
        }

        public IBudgetItemAdjustmentView View { get; set; }

        public RelayCommand OkButtonCommand { get; }

        public bool DialogResult { get; set; }

        private BudgetItem _budgetItem;

        public BudgetItemAdjustmentViewModel()
        {
            OkButtonCommand = new RelayCommand(OnOkButton);
        }

        public void OnViewLoaded(BudgetItem budgetItem)
        {
            _budgetItem = budgetItem;
            BudgetItemDescription = budgetItem.Description;
        }

        private void OnOkButton()
        {
            ControlsGlobals.UserInterface.ShowMessageBox("Post Adjustment.", "Nub", RsMessageBoxIcons.Information);
            DialogResult = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
