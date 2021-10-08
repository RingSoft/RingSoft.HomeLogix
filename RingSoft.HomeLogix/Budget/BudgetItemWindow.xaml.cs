﻿using RingSoft.App.Controls;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.DataAccess.Model;

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

            _monthlyStatsControls.Add(MonthlyAmountPercentLabel);
            _monthlyStatsControls.Add(MonthlyAmountPercentControl);
            _monthlyStatsControls.Add(MonthToDatePercentLabel);
            _monthlyStatsControls.Add(MonthToDatePercentControl);
            _monthlyStatsControls.Add(MonthlyPercentDifferenceLabel);
            _monthlyStatsControls.Add(MonthlyPercentDifferenceControl);

            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is BudgetCustomPanel budgetCustomPanel)
                {
                    budgetCustomPanel.AddButton.Command = BudgetItemViewModel.AddAdjustmentCommand;
                }
            };

        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(DescriptionControl);

            base.OnLoaded();

            if (BudgetItemViewModel.FromRegisterGrid)
                TopHeaderControl.SaveSelectButton.Visibility = Visibility.Collapsed;
        }

        public void SetViewType()
        {
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

        public bool AddAdjustment(BudgetItem budgetItem)
        {
            ControlsGlobals.UserInterface.ShowMessageBox("Show Adjustment Window", "Nub", RsMessageBoxIcons.Information);
            //var win = new BankAccountRegisterActualAmountDetailsWindow(actualAmountCellProps);
            //win.ShowInTaskbar = false;
            //win.Owner = this;
            //win.ShowDialog();

            return true;
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
