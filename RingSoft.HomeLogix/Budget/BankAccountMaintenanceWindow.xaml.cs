﻿using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BankAccountMaintenanceWindow.xaml
    /// </summary>
    public partial class BankAccountMaintenanceWindow : IBankAccountView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Bank Account";
        public override DbMaintenanceViewModelBase ViewModel => BankAccountViewModel;

        public BankAccountMaintenanceWindow()
        {
            InitializeComponent();

            RegisterFormKeyControl(BankAccountControl);

            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is BankCustomPanel bankCustomPanel)
                {
                    bankCustomPanel.GenerateButton.Command =
                        BankAccountViewModel.GenerateRegisterItemsFromBudgetCommand;
                    bankCustomPanel.AddButton.Command = BankAccountViewModel.AddNewRegisterItemCommand;
                }
            };
        }

        public override void ResetViewForNewRecord()
        {
            BankAccountControl.Focus();
            base.ResetViewForNewRecord();
        }

        public void EnableRegisterGrid(bool value)
        {
            //RegisterGrid.IsEnabled = value;
        }
    }
}
