using RingSoft.App.Controls;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BudgetItemWindow.xaml
    /// </summary>
    public partial class BudgetItemWindow : IBudgetItemView
    {
        public override Control MaintenanceButtonsControl => TopHeaderControl;
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Budget Item";
        public override DbMaintenanceViewModelBase ViewModel => BudgetItemViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public BudgetItemWindow()
        {
            InitializeComponent();


            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is BudgetCustomPanel budgetCustomPanel)
                {
                    budgetCustomPanel.AddButton.Command = BudgetItemViewModel.AddAdjustmentCommand;
                    budgetCustomPanel.ClearRecurButton.Command = BudgetItemViewModel.ClearRecurringCommand;
                }
            };

        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(DescriptionControl);

            base.OnLoaded();

            if (BudgetItemViewModel.FromRegisterGrid)
                TopHeaderControl.SaveSelectButton.Visibility = Visibility.Collapsed;

            DbMaintenanceTopHeaderControl.SaveSelectButton.Command = BudgetItemViewModel.SaveSelectButtonCommand;

            DescriptionControl.SetReadOnlyMode(false);
        }

        public void SetViewType()
        {
            TransferToStackPanel.Visibility =
                BudgetItemViewModel.TransferToBankVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public void ShowMonthlyStatsControls(bool show = true)
        {
        }

        public bool AddAdjustment(BudgetItem budgetItem)
        {
            var win = new BudgetItemAdjustmentWindow(budgetItem);
            win.ShowInTaskbar = false;
            win.Owner = this;
            return win.ShowDialog();
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
