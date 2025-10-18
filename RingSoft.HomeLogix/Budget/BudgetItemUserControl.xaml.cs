using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BudgetItemUserControl.xaml
    /// </summary>
    public partial class BudgetItemUserControl : IBudgetItemView
    {

        private VmUiControl _genTranControl;
        public BudgetItemUserControl()
        {
            InitializeComponent();
            RegisterFormKeyControl(DescriptionControl);

            _genTranControl = new VmUiControl(GenTranCheckBox, BudgetItemViewModel.GenTranUiCommand);

            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is BudgetCustomPanel budgetCustomPanel)
                {
                    budgetCustomPanel.AddButton.Command = BudgetItemViewModel.AddAdjustmentCommand;
                    budgetCustomPanel.ClearRecurButton.Command = BudgetItemViewModel.ClearRecurringCommand;
                }
            };

            var hotKey = new HotKey(BudgetItemViewModel.AddAdjustmentCommand);
            hotKey.AddKey(Key.B);
            hotKey.AddKey(Key.A);
            AddHotKey(hotKey);

            hotKey = new HotKey(BudgetItemViewModel.ClearRecurringCommand);
            hotKey.AddKey(Key.B);
            hotKey.AddKey(Key.C);
            AddHotKey(hotKey);
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return BudgetItemViewModel;
        }

        protected override Control OnGetMaintenanceButtons()
        {
            return TopHeaderControl;
        }

        protected override DbMaintenanceStatusBar OnGetStatusBar()
        {
            return StatusBar;
        }

        protected override string GetTitle()
        {
            return "Budget Item";
        }

        public void SetViewType(bool isCC = false)
        {
            switch (BudgetItemViewModel.RecurringType)
            {
                case BudgetItemRecurringTypes.Months:
                    OnDayCheckbox.Visibility = Visibility.Visible;
                    OnDayEditControl.Visibility = Visibility.Visible;
                    break;
                default:
                    OnDayCheckbox.Visibility = Visibility.Collapsed;
                    OnDayEditControl.Visibility = Visibility.Collapsed;
                    break;
            }

            OnDayEditControl.IsEnabled = BudgetItemViewModel.OnDay;
            
            TransferToStackPanel.Visibility =
                BudgetItemViewModel.TransferToBankVisible ? Visibility.Visible : Visibility.Collapsed;

            if (isCC)
            {
                PayCCBalanceCheckBox.Visibility = Visibility.Visible;
            }
            else
            {
                PayCCBalanceCheckBox.Visibility = Visibility.Collapsed;
            }

            if (BudgetItemViewModel.PayCCBalance && PayCCBalanceCheckBox.Visibility == Visibility.Visible)
            {
                PayCCDayLabel.Visibility = Visibility.Visible;
                PayCCDatControl.Visibility = Visibility.Visible;
            }
            else
            {
                PayCCDayLabel.Visibility = Visibility.Collapsed;
                PayCCDatControl.Visibility = Visibility.Collapsed;
            }

            if (BudgetItemViewModel.PayCCBalance)
            {
                AmountControl.IsEnabled = false;
            }
            else
            {
                AmountControl.IsEnabled = true;
            }
        }

        public void ShowMonthlyStatsControls(bool show = true)
        {
            
        }

        public bool AddAdjustment(BudgetItem budgetItem)
        {
            var win = new BudgetItemAdjustmentWindow(budgetItem);
            win.ShowInTaskbar = false;
            win.Owner = OwnerWindow;
            return win.ShowDialog();
        }

        public void HandleValFail(ValFailControls control)
        {
            switch (control)
            {
                case ValFailControls.Bank:
                    BankAccountControl.HandleValFail("Bank Account");
                    break;
                case ValFailControls.TransFerToBank:
                    TransferToBankAccount.HandleValFail("Transfer To Bank Account");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(control), control, null);
            }
        }
    }
}
