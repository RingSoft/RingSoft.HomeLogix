using System;
using System.Windows;
using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BudgetItemWindow.xaml
    /// </summary>
    public partial class BudgetItemWindow : IBudgetItemView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Budget Item";
        public override DbMaintenanceViewModelBase ViewModel => BudgetItemViewModel;

        public BudgetItemWindow()
        {
            InitializeComponent();
        }

        public void SetViewType(RecurringViewTypes viewType)
        {
            TransferToLabel.Visibility = Visibility.Hidden;
            TransferToBankAccount.Visibility = Visibility.Hidden;
            TransferToStackPanel.Visibility = Visibility.Collapsed;
            EscrowCheckBox.Visibility = Visibility.Collapsed;

            SpendingDayOfWeekLabel.Visibility = Visibility.Hidden;
            SpendingDayOfWeekComboBoxControl.Visibility = Visibility.Hidden;
            SpendingTypeStackPanel.Visibility = Visibility.Collapsed;

            switch (viewType)
            {
                case RecurringViewTypes.Transfer:
                    TransferToStackPanel.Visibility = Visibility.Visible;
                    SetTransferToBankAccountVisibility();
                    break;
                case RecurringViewTypes.Escrow:
                    EscrowCheckBox.Visibility = Visibility.Visible;
                    TransferToStackPanel.Visibility = Visibility.Visible;
                    SetTransferToBankAccountVisibility();
                    break;
                case RecurringViewTypes.DayOrWeek:
                    break;
                case RecurringViewTypes.MonthlySpendingMonthly:
                    SpendingTypeStackPanel.Visibility = Visibility.Visible;
                    break;
                case RecurringViewTypes.MonthlySpendingWeekly:
                    SpendingTypeStackPanel.Visibility = Visibility.Visible;
                    SpendingDayOfWeekLabel.Visibility = Visibility.Visible;
                    SpendingDayOfWeekComboBoxControl.Visibility = Visibility.Visible;
                    break;
                case RecurringViewTypes.Income:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }

        private void SetTransferToBankAccountVisibility()
        {
            if (TransferToStackPanel.Visibility == Visibility.Visible)
            {
                var transferToBankAccountVisibility = Visibility.Hidden;
                if (BudgetItemViewModel.DoEscrow != null && (bool)BudgetItemViewModel.DoEscrow)
                    transferToBankAccountVisibility = Visibility.Visible;

                TransferToLabel.Visibility = transferToBankAccountVisibility;
                TransferToBankAccount.Visibility = transferToBankAccountVisibility;
            }
        }
    }
}
