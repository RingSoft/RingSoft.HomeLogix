using System;
using System.Collections.Generic;
using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.HomeLogix.Library;

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

        private List<Control> _monthlyStatsControls = new List<Control>();

        public BudgetItemWindow()
        {
            InitializeComponent();

            _monthlyStatsControls.Add(LastCompletedDateLabel);
            _monthlyStatsControls.Add(LastCompletedDateControl);
            _monthlyStatsControls.Add(MonthToDateAmountLabel);
            _monthlyStatsControls.Add(MonthToDateAmountControl);
            _monthlyStatsControls.Add(CurrentMonthLabel);
            _monthlyStatsControls.Add(CurrentMonthControl);
            _monthlyStatsControls.Add(MonthlyAmountPercentLabel);
            _monthlyStatsControls.Add(MonthlyAmountPercentControl);
            _monthlyStatsControls.Add(MonthToDatePercentLabel);
            _monthlyStatsControls.Add(MonthToDatePercentControl);
            _monthlyStatsControls.Add(MonthlyPercentDifferenceLabel);
            _monthlyStatsControls.Add(MonthlyPercentDifferenceControl);
            _monthlyStatsControls.Add(MonthlyAmountRemainingLabel);
            _monthlyStatsControls.Add(MonthlyAmountRemainingControl);
        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(DescriptionControl);

            base.OnLoaded();
        }

        public void SetViewType()
        {
            EscrowStackPanel.Visibility = BudgetItemViewModel.EscrowVisible ? Visibility.Visible : Visibility.Collapsed;

            EscrowLabel.Visibility = EscrowBox.Visibility =
                BudgetItemViewModel.DoEscrow ? Visibility.Visible : Visibility.Collapsed;

            TransferToStackPanel.Visibility =
                BudgetItemViewModel.TransferToBankVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public void ShowMonthlyStatsControls(bool show = true)
        {
            foreach (var control in _monthlyStatsControls)
            {
                control.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public override void ResetViewForNewRecord()
        {
            TabControl.SelectedIndex = 0;
            DescriptionControl.Focus();

            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = AppGlobals.LookupContext.BudgetItems;

            if (fieldDefinition == table.GetFieldDefinition(p => p.Description))
                DescriptionControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.BankAccountId))
                BankAccountControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.TransferToBankAccountId))
                TransferToBankAccount.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
