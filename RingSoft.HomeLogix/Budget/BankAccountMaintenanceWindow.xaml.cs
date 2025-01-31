using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using RingSoft.App.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.ImportBank;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels;
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
        public override Control MaintenanceButtonsControl => TopHeaderControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        private BankProcedure _bankProcedure;

        public object OwnerWindow => this;
        public bool ImportFromBank(BankAccountViewModel bankAccountViewModel)
        {
            var importTransactionsWindow = new ImportBankTransactionsWindow(bankAccountViewModel);
            importTransactionsWindow.Owner = this;
            importTransactionsWindow.ShowInTaskbar = false;
            importTransactionsWindow.ShowDialog();
            return importTransactionsWindow.DialogResult != null && importTransactionsWindow.DialogResult.Value;
        }

        public void LoadBank(BankAccount entity)
        {
            _bankProcedure = new BankProcedure(BankAccountViewModel, BankProcesses.Loading){BankAccount = entity};
            _bankProcedure.Start();
            Activate();
        }

        public void GenerateTransactions(DateTime generateToDate)
        {
            _bankProcedure = new BankProcedure(BankAccountViewModel, BankProcesses.Generating)
                {GenerateToDate = generateToDate};
            _bankProcedure.Start();
        }

        public void PostRegister(CompletedRegisterData completedRegisterData, List<BankAccountRegisterGridRow> completedRows)
        {
            _bankProcedure = new BankProcedure(BankAccountViewModel, BankProcesses.Posting)
            {
                CompletedRegisterData = completedRegisterData,
                CompletedRows = completedRows
            };
            _bankProcedure.Start();
        }

        public void UpdateStatus(string status)
        {
            _bankProcedure.SplashWindow.SetProgress(status);
        }

        public void ShowMessageBox(string message, string caption, RsMessageBoxIcons icon)
        {
            if (_bankProcedure != null)
            {
                _bankProcedure.ShowMessageBox(message, caption, icon);
            }
            else
            {
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, icon);
            }
        }

        public void SetInitGridFocus(BankAccountRegisterGridRow row, int columnId)
        {
            TabControl.SelectedIndex = 0;
            RegisterGrid.Focus();
            RegisterGrid.GotoCell(row, columnId);

        }

        public void RestartApp()
        {
            var path = Process.GetCurrentProcess().MainModule.FileName;
            Application.Current.Shutdown(0);
            Process.Start(path);
        }

        public void RefreshGrid(BankAccount bankAccount)
        {
            
        }

        public BankAccountMaintenanceWindow()
        {
            InitializeComponent();

            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is BankCustomPanel bankCustomPanel)
                {
                    bankCustomPanel.GenerateButton.Command =
                        BankAccountViewModel.GenerateRegisterItemsFromBudgetCommand;
                    bankCustomPanel.AddButton.Command = BankAccountViewModel.AddNewRegisterItemCommand;
                    bankCustomPanel.ImportButton.Command = BankAccountViewModel.ImportTransactionsCommand;
                }
            };
        }

        protected override void OnLoaded()
        {
            RegisterFormKeyControl(BankAccountControl);
            base.OnLoaded();
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

        public DateTime? GetGenerateToDate(DateTime nextGenerateToDate)
        {
            var generateToWindow = new BankAccountGenerateToWindow
            {
                GenerateToDate = nextGenerateToDate
            };
            if (generateToWindow.ShowDialog() == true)
                return generateToWindow.GenerateToDate;

            return null;
        }

        public void ShowActualAmountDetailsWindow(ActualAmountCellProps actualAmountCellProps)
        {
            var win = new BankAccountRegisterActualAmountDetailsWindow(actualAmountCellProps);
            win.ShowInTaskbar = false;
            win.Owner = this;
            win.ShowDialog();
        }

        public bool ShowBankAccountMiscWindow(BankAccountRegisterItem registerItem, ViewModelInput viewModelInput)
        {
            if (registerItem != null &&
                !registerItem.TransferRegisterGuid.IsNullOrEmpty())
            {
                var transferRegisterItem =
                    AppGlobals.DataRepository.GetTransferRegisterItem(registerItem.TransferRegisterGuid);
                if (transferRegisterItem == null)
                {
                    var message = "Missing related Transfer Register Item.";
                    var caption = "Missing Relation";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return false;
                }
            }

            var bankAccountMiscWindow = new BankAccountMiscWindow(BankAccountViewModel, registerItem, viewModelInput);
            bankAccountMiscWindow.Owner = this;
            bankAccountMiscWindow.ShowInTaskbar = false;
            return bankAccountMiscWindow.ShowDialog().GetValueOrDefault(false);
        }
    }
}
