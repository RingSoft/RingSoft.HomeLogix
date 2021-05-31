using System;
using System.Windows;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BankAccountMiscWindow.xaml
    /// </summary>
    public partial class BankAccountMiscWindow : IBankAccountMiscView
    {
        public BankAccountMiscWindow(BankAccountRegisterItem registerItem, ViewModelInput viewModelInput)
        {
            InitializeComponent();

            ViewModel.OnViewLoaded(this, registerItem, viewModelInput);
        }

        public void SetViewType()
        {
            TransferToBankLabel.Visibility = ViewModel.TransferToVisible ? Visibility.Visible : Visibility.Collapsed;
            TransferToBankControl.Visibility = ViewModel.TransferToVisible ? Visibility.Visible : Visibility.Collapsed;
            BudgetItemLabel.Visibility = ViewModel.BudgetItemVisible ? Visibility.Visible : Visibility.Collapsed;
            BudgetItemControl.Visibility = ViewModel.BudgetItemVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public void OnOkButtonCloseWindow()
        {
            DialogResult = true;
            Close();
        }

        public void OnValidationFail(string message, string caption, ValidationFocusControls control)
        {
            switch (control)
            {
                case ValidationFocusControls.BudgetItem:
                    BudgetItemControl.Focus();
                    break;
                case ValidationFocusControls.TransferToBank:
                    TransferToBankControl.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(control), control, null);
            }

            MessageBox.Show(this, message, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
