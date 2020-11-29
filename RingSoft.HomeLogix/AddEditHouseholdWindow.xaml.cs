using System;
using System.IO;
using Microsoft.Win32;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.Library.ViewModels;
using RingSoft.HomeLogix.MasterData;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for AddEditHouseholdWindow.xaml
    /// </summary>
    public partial class AddEditHouseholdWindow : IAddEditHouseholdView
    {
        public Households Household { get; }

        public AddEditHouseholdWindow(Households household)
        {
            Household = household;

            InitializeComponent();
            
            ViewModel.OnViewLoaded(this);
        }

        public void CloseWindow(bool dialogResult)
        {
            DialogResult = dialogResult;
            Close();
        }

        public string ShowFileDialog()
        {
            var fileName = string.Empty;
            var directory = string.Empty;
            if (!ViewModel.FileName.IsNullOrEmpty())
            {
                fileName = Path.GetFileName(ViewModel.FileName);
                directory = new FileInfo(ViewModel.FileName ?? string.Empty).DirectoryName;
            }
            var saveFileDialog = new SaveFileDialog
            {
                FileName = fileName ?? string.Empty,
                InitialDirectory = directory ?? string.Empty,
                DefaultExt = "sqlite",
                Filter = "SQLite Files|*.sqlite"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            return string.Empty;
        }

        public void SetFocus(SetFocusControls control)
        {
            switch (control)
            {
                case SetFocusControls.HouseholdName:
                    HouseholdNameTextBox.Focus();
                    break;
                case SetFocusControls.FileName:
                    FileNameTextBox.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(control), control, null);
            }
        }
    }
}
