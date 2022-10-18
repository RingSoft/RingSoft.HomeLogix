using System;
using System.Collections.Generic;
using RingSoft.App.Controls;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix
{
    public class BankProcedure : AppProcedure
    {
        public override ISplashWindow SplashWindow => _splashWindow;

        public BankAccountViewModel ViewModel { get; }

        public BankProcesses Process { get; }

        public BankAccount BankAccount { get; set; }

        public DateTime GenerateToDate { get; set; }

        public CompletedRegisterData CompletedRegisterData { get; set; }

        public List<BankAccountRegisterGridRow> CompletedRows { get; set; }

        public bool IsRunning { get; private set; }

        private ProcessingSplashWindow _splashWindow;
        
        public BankProcedure(BankAccountViewModel viewModel, BankProcesses process)
        {
            ViewModel = viewModel;
            Process = process;
        }
        protected override void ShowSplash()
        {
            switch (Process)
            {
                case BankProcesses.Loading:
                    _splashWindow = new ProcessingSplashWindow("Loading Bank Account");
                    break;
                case BankProcesses.Posting:
                    _splashWindow = new ProcessingSplashWindow("Posting Register");
                    break;
                case BankProcesses.Generating:
                    _splashWindow = new ProcessingSplashWindow("Generating Transactions");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _splashWindow.ShowDialog();

        }

        protected override bool DoProcess()
        {
            IsRunning = true;
            switch (Process)
            {
                case BankProcesses.Loading:
                    ViewModel.LoadFromEntityProcedure(BankAccount);
                    break;
                case BankProcesses.Posting:
                    ViewModel.PostTransactions(CompletedRegisterData, CompletedRows);
                    break;
                case BankProcesses.Generating:
                    ViewModel.GenerateTransactions(GenerateToDate);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _splashWindow.CloseSplash();
            IsRunning = false;
            return true;
        }

        public void ShowMessageBox(string message, string caption, RsMessageBoxIcons icon)
        {
            if (IsRunning)
            {
                _splashWindow.Dispatcher.Invoke(() =>
                {
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, icon);
                });
            }
            else
            {
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, icon);
            }
        }
    }
}
