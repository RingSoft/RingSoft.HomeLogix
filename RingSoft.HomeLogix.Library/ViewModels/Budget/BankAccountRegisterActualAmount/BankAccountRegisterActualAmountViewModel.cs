using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;

// ReSharper disable once CheckNamespace
namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public interface IBankAccountRegisterActualAmountView
    {
        void OnOkButtonCloseWindow();
    }

    public class BankAccountRegisterActualAmountViewModel : INotifyPropertyChanged
    {
        private DateTime _date;

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date == value)
                    return;

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

        private BankAccountRegisterActualAmountGridManager _gridManager;

        public BankAccountRegisterActualAmountGridManager GridManager
        {
            get => _gridManager;
            set
            {
                if (_gridManager == value)
                    return;

                _gridManager = value;
                OnPropertyChanged();
            }
        }

        private decimal _projectedAmount;

        public decimal ProjectedAmount
        {
            get => _projectedAmount;
            set
            {
                if (_projectedAmount == value)
                    return;

                _projectedAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalActualAmount;

        public decimal TotalActualAmount
        {
            get => _totalActualAmount;
            set
            {
                if (_totalActualAmount == value)
                    return;

                _totalActualAmount = value;
                OnPropertyChanged();
            }
        }

        private decimal _difference;

        public decimal Difference
        {
            get => _difference;
            set
            {
                if (_difference == value)
                    return;

                _difference = value;
                OnPropertyChanged();
            }
        }


        public IBankAccountRegisterActualAmountView View { get; private set; }
        public ActualAmountCellProps ActualAmountCellProps { get; private set; }

        public RelayCommand OkButtonCommand { get; }

        public BankAccountRegisterActualAmountViewModel()
        {
            OkButtonCommand = new RelayCommand(OnOkButton);
        }

        public void OnViewLoaded(IBankAccountRegisterActualAmountView view, ActualAmountCellProps actualAmountCellProps)
        {
            View = view;
            ActualAmountCellProps = actualAmountCellProps;

            GridManager = new BankAccountRegisterActualAmountGridManager(this);

            Date = actualAmountCellProps.RegisterGridRow.ItemDate;
            Description = actualAmountCellProps.RegisterGridRow.Description;
            ProjectedAmount = actualAmountCellProps.RegisterGridRow.ProjectedAmount;

            GridManager.LoadGrid(actualAmountCellProps.RegisterGridRow.ActualAmountDetails);

            CalculateTotals();
        }

        public void CalculateTotals()
        {
            TotalActualAmount = GridManager.GetTotalAmount();
            Difference = ProjectedAmount - TotalActualAmount;
        }

        private void OnOkButton()
        {
            GridManager.SaveData();
            ActualAmountCellProps.Value = TotalActualAmount;
            View.OnOkButtonCloseWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
