using System;
using System.Windows;
using System.Windows.Controls;
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
        private BankAccountViewModel _bankAccountViewModel;
        private BankAccountRegisterItem _registerItem;
        public BankAccountMiscWindow(BankAccountViewModel bankAccountViewModel, BankAccountRegisterItem registerItem, ViewModelInput viewModelInput)
        {
            _bankAccountViewModel = bankAccountViewModel;
            _registerItem = registerItem;
            InitializeComponent();

            ViewModel.OnViewLoaded(this, bankAccountViewModel, registerItem, viewModelInput);
            CancelButton.Click += (sender, args) => Close();
            Loaded += (sender, args) =>
            {
                if (ViewModel.ItemType == BudgetItemTypes.Transfer)
                {
                    if (ViewModel.RegisterItem.BankAccountId != _bankAccountViewModel.Id)
                    {
                        SetReadOnlyMode(true);
                    }
                }
            };
        }

        public override void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (control == CancelButton && readOnlyValue == true)
            {
                return;
            }
            base.SetControlReadOnlyMode(control, readOnlyValue);
        }

        public void SetViewType()
        {
            TransferFromBankLabel.Visibility = ViewModel.TransferToVisible ? Visibility.Visible : Visibility.Collapsed;
            TransferFromBankControl.Visibility = ViewModel.TransferToVisible ? Visibility.Visible : Visibility.Collapsed;
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
                case ValidationFocusControls.Amount:
                    AmountControl.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(control), control, null);
            }

            MessageBox.Show(this, message, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
