using Microsoft.Win32;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.Library.ViewModels;
using System;
using System.IO;
using RingSoft.HomeLogix.MasterData;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for AddEditHouseholdWindow.xaml
    /// </summary>
    public partial class AddEditHouseholdWindow : IAddEditHouseholdView
    {
        public AddEditHouseholdWindow()
        {
            InitializeComponent();
            
            ViewModel.OnViewLoaded(this);
        }

        public new Households ShowDialog()
        {
            base.ShowDialog();
            return ViewModel.Household;
        }

        public void CloseWindow() => Close();

        public string ShowFileDialog()
        {
            var fileName = Path.GetFileName(ViewModel.FileName);
            var directory = new FileInfo(ViewModel.FileName ?? string.Empty).DirectoryName;
            var saveFileDialog = new SaveFileDialog
            {
                FileName = fileName ?? string.Empty,
                InitialDirectory = directory ?? string.Empty,
                DefaultExt = "sqlite",
                Filter = "HomeLogix SQLite Files(*.sqlite)|*.sqlite"
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
