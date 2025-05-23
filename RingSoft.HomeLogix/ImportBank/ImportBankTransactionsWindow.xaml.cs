﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;
using RingSoft.App.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using RingSoft.HomeLogix.Library.ViewModels.ImportBank;

namespace RingSoft.HomeLogix.ImportBank
{
    /// <summary>
    /// Interaction logic for ImportBankTransactionsWindow.xaml
    /// </summary>
    public partial class ImportBankTransactionsWindow : BaseWindow , IImportTransactionView
    {
        public ImportBankTransactionsWindow(BankAccountViewModel bankAccountViewModel)
        {
            InitializeComponent();
            ViewModel.Initialize(bankAccountViewModel, this);
            ContentRendered += (sender, args) => DataEntryGrid.Focus();
            Loaded += (sender, args) =>
            {
                DataEntryGrid.PreviewKeyDown += (o, eventArgs) =>
                {
                    if (eventArgs.Key == Key.Enter)
                    {
                        if (DataEntryGrid.EditingControlHost != null)
                        {
                            if (DataEntryGrid.EditingControlHost.HasDataChanged())
                            {
                                DataEntryGrid.EditingControlHost.Row.IsNew = false;
                                DataEntryGrid.EditingControlHost.Row.SetCellValue(DataEntryGrid.EditingControlHost
                                    .GetCellValue());
                            }
                        }

                        if (DataEntryGrid.EditingControlHost != null &&
                            !DataEntryGrid.EditingControlHost.IsDropDownOpen)
                        {
                            ViewModel.OkCommand.Execute(null);
                            eventArgs.Handled = true;
                        }
                    }
                };
            };
            CancelButton.Click += (sender, args) => Close();
        }

        public bool ShowImportBankBudgetWindow(ImportTransactionGridRow row)
        {
            var window = new ImportBankTransactionsBudgetsWindow(row);
            window.Owner = this;
            window.ShowInTaskbar = false;
            window.ShowDialog();
            return window.DialogResult != null && window.DialogResult.Value;
        }

        public void CloseWindow(bool dialogResult)
        {
            DialogResult = dialogResult;
            Close();
        }

        public string GetQifFile()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = folder,
                DefaultExt = "qif",
                Filter = "Quicken/Microsoft Money QIF Files(*.qif)|*.qif"
            };

            var file = string.Empty;
            if (openFileDialog.ShowDialog() == true)
            {
                file = openFileDialog.FileName;
            }

            if (file.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var qifText = OpenTextFile(file);

            return qifText;
        }

        public void ShowQifMaintenanceWindow()
        {
            var window = new QifMapMaintenanceWindow();
            window.ShowInTaskbar = false;
            window.Owner = this;
            window.ShowDialog();
        }

        public void ShowExpiredWindow(List<BankAccountRegisterGridRow> expiredItems)
        {
            var window = new ImportExpiredWindow(expiredItems);
            window.Owner = this;
            window.ContentRendered += (sender, args) =>
            {
                window.Activate();
            };
            window.ShowDialog();
        }

        public static string OpenTextFile(string fileName)
        {
            var result = string.Empty;
            try
            {
                var openFile = new StreamReader(fileName);
                result = openFile.ReadToEnd();
            }
            catch (Exception e)
            {
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error Opening Text File", RsMessageBoxIcons.Error);
            }

            return result;
        }
    }
}
