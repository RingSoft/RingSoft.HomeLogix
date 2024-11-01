using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.ImportBank;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BankAccountMaintenanceUserControl.xaml
    /// </summary>
    public partial class BankAccountMaintenanceUserControl : IBankAccountView
    {
        private BankProcedure _bankProcedure;

        public BankAccountMaintenanceUserControl()
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
            var hotKey = new HotKey(BankAccountViewModel.ImportTransactionsCommand);
            hotKey.AddKey(Key.B);
            hotKey.AddKey(Key.I);
            AddHotKey(hotKey);

            hotKey = new HotKey(BankAccountViewModel.AddNewRegisterItemCommand);
            hotKey.AddKey(Key.B);
            hotKey.AddKey(Key.A);
            AddHotKey(hotKey);

            hotKey = new HotKey(BankAccountViewModel.GenerateRegisterItemsFromBudgetCommand);
            hotKey.AddKey(Key.B);
            hotKey.AddKey(Key.G);
            AddHotKey(hotKey);

            RegisterFormKeyControl(BankAccountControl);
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return BankAccountViewModel;
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
            return "Bank Account";
        }

        public void EnableRegisterGrid(bool value)
        {
            
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
            LookupControlsGlobals.WindowRegistry.ShowDialog(win);
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
            bankAccountMiscWindow.Owner = base.OwnerWindow;
            bankAccountMiscWindow.ShowInTaskbar = false;
            return bankAccountMiscWindow.ShowDialog().GetValueOrDefault(false);
        }

        public object OwnerWindow { get; }

        public bool ImportFromBank(BankAccountViewModel bankAccountViewModel)
        {
            var importTransactionsWindow = new ImportBankTransactionsWindow(bankAccountViewModel);
            importTransactionsWindow.Owner = base.OwnerWindow;
            importTransactionsWindow.ShowInTaskbar = false;
            importTransactionsWindow.ShowDialog();
            return importTransactionsWindow.DialogResult != null && importTransactionsWindow.DialogResult.Value;
        }

        public void LoadBank(BankAccount entity)
        {
            _bankProcedure = new BankProcedure(BankAccountViewModel, BankProcesses.Loading) { BankAccount = entity };
            _bankProcedure.Start();
            base.OwnerWindow.Activate();
        }

        public void GenerateTransactions(DateTime generateToDate)
        {
            _bankProcedure = new BankProcedure(BankAccountViewModel, BankProcesses.Generating)
                { GenerateToDate = generateToDate };
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
    }
}
