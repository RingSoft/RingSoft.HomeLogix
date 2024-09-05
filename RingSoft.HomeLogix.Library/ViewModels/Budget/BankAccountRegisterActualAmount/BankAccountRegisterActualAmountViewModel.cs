using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.HomeLogix.DataAccess.Model;

// ReSharper disable once CheckNamespace
namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public interface IBankAccountRegisterActualAmountView
    {
        void OnOkButtonCloseWindow();
    }

    public class BankAccountRegisterActualAmountViewModel : INotifyPropertyChanged
    {
        #region Properties

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

        private AutoFillSetup _budgetAutoFillSetup;

        public AutoFillSetup BudgetAutoFillSetup
        {
            get => _budgetAutoFillSetup;
            set
            {
                if (_budgetAutoFillSetup == value)
                {
                    return;
                }
                _budgetAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _budgetAutoFillValue;

        public AutoFillValue BudgetAutoFillValue
        {
            get => _budgetAutoFillValue;
            set
            {
                if (_budgetAutoFillValue == value)
                {
                    return;
                }
                _budgetAutoFillValue = value;
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

        private double _projectedAmount;

        public double ProjectedAmount
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

        private double _totalActualAmount;

        public double TotalActualAmount
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

        private double _difference;

        public double Difference
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

        #endregion

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
            BudgetAutoFillSetup =
                new AutoFillSetup(
                    AppGlobals.LookupContext.BankAccountRegisterItems.GetFieldDefinition(p => p.BudgetItemId));

            BudgetAutoFillValue = actualAmountCellProps.RegisterGridRow.BudgetItemValue;
            ProjectedAmount = actualAmountCellProps.RegisterGridRow.ProjectedAmount;

            GridManager.LoadGrid(
                AppGlobals.DataRepository.GetBankAccountRegisterItemDetails(actualAmountCellProps.RegisterGridRow
                    .RegisterId));

            CalculateTotals();
        }

        public void CalculateTotals()
        {
            TotalActualAmount = GridManager.GetTotalAmount();
            Difference = ProjectedAmount - TotalActualAmount;
            switch (ActualAmountCellProps.RegisterGridRow.TransactionType)
            {
                case TransactionTypes.Deposit:
                    Difference = -Difference;
                    break;
                case TransactionTypes.Withdrawal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnOkButton()
        {
            var invalidSources = GridManager.Rows.OfType<ActualAmountGridRow>()
                .Where(p => p.IsNew == false && (p.SourceAutoFillValue == null || !p.SourceAutoFillValue.IsValid())).ToList();
            if (invalidSources.Any())
            {
                var message = "You must select a valid source.";
                var caption = "Invalid Source";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                GridManager.Grid?.GotoCell(invalidSources[0], BankAccountRegisterActualAmountGridManager.SourceColumnId);
                return;
            }
            var amountDetails = GridManager.SaveData();
            ActualAmountCellProps.Value = TotalActualAmount;
            var registerItem = new BankAccountRegisterItem();
            ActualAmountCellProps.RegisterGridRow.SaveToEntity(registerItem, 0);
            registerItem.ActualAmount = TotalActualAmount;

            if (AppGlobals.DataRepository.SaveRegisterItem(registerItem, amountDetails))
            {
                View.OnOkButtonCloseWindow();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
