using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using RingSoft.HomeLogix.Library.ViewModels.ImportBank;
using MessageBox = System.Windows.Forms.MessageBox;

namespace RingSoft.HomeLogix.ImportBank
{
    /// <summary>
    /// Interaction logic for ImportExpiredWindow.xaml
    /// </summary>
    public partial class ImportExpiredWindow : IImportExpiredView
    {
        public ImportExpiredWindow(List<BankAccountRegisterGridRow> rows)
        {
            InitializeComponent();

            ViewModel.Initialize(this, rows);
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
        }


        public void CloseWindow()
        {
            Close();
        }
    }
}
