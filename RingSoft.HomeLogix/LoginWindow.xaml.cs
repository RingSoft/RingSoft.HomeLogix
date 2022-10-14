using Microsoft.Win32;
using RingSoft.HomeLogix.Library.ViewModels;
using RingSoft.HomeLogix.MasterData;
using System.ComponentModel;
using System.Windows;
using RingSoft.HomeLogix.Library;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : ILoginView
    {
        public LoginWindow()
        {
            InitializeComponent();
            
            ViewModel.OnViewLoaded(this);
            ListBox.MouseDoubleClick += (sender, args) => ViewModel.LoginCommand.Execute(null);
            ListBox.GotKeyboardFocus += (sender, args) => ListBox.SelectedItem ??= ListBox.Items[0];
        }

        public bool LoginToHousehold(Household household)
        {
            var loginProcedure = new LoginProcedure(household);
            return loginProcedure.Start();
        }

        public Household ShowAddHousehold()
        {
            var addEditHouseholdWindow = new AddEditHouseholdWindow()
            {
                Owner = this,
                HouseholdProcess = HouseholdProcesses.Add
            };
            return addEditHouseholdWindow.ShowDialog();
        }

        public void EditHousehold(Household household)
        {
            var addEditHouseholdWindow = new AddEditHouseholdWindow(household)
            {
                Owner = this,
                HouseholdProcess = HouseholdProcesses.Edit
            };
            addEditHouseholdWindow.ShowDialog();
            if (addEditHouseholdWindow.DataCopied)
            {
                var message = "You must restart the application in order to continue.";
                var caption = "Restart Application";
                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
            }
        }

        public string GetHouseholdDataFile()
        {
            var openFileDialog = new OpenFileDialog()
            {
                DefaultExt = "sqlite",
                Filter = "HomeLogix SQLite Files(*.sqlite)|*.sqlite"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return string.Empty;
        }

        public void CloseWindow()
        {
            DialogResult = ViewModel.DialogResult;
            Close();
        }

        public void ShutDownApplication()
        
        {
            Application.Current.Shutdown(0);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = ViewModel.DoCancelClose();
            base.OnClosing(e);
        }
    }
}
