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

            hotKey = new HotKey(BankAccountViewModel.ShowBankOptionsCommand);
            hotKey.AddKey(Key.B);
            hotKey.AddKey(Key.O);
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
            bankAccountMiscWindow.Owner = WPFControlsGlobals.ActiveWindow;
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
            Dispatcher.Invoke(() =>
            {
                BankAccountViewModel.LoadFromEntityProcedure(bankAccount);
            });
        }

        public void ToggleCompleteAll(bool completeAll)
        {
            if (completeAll)
            {
                CompleteAllButton.Content = "Uncomplete All";
            }
            else
            {
                CompleteAllButton.Content = "Complete All";
            }
        }

        public void ShowBankOptionsWindow(BankOptionsData bankOptionsData)
        {
            var dialog = new BankOptionsWindow(bankOptionsData);
            LookupControlsGlobals.WindowRegistry.ShowDialog(dialog);
        }

        public void SetBankOptionsButtonCaption(string caption)
        {
            ShowBankOptionsButton.Content = $"{caption} Options";
            ShowBankOptionsButton.ToolTip.HeaderText = $"Show {caption} Options (Ctrl + B, Ctrl + O)";
            ShowBankOptionsButton.ToolTip.DescriptionText = $"Show {caption.ToLower()} options.";
        }
    }
}
