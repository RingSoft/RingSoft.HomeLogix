using Microsoft.Win32;
using RingSoft.HomeLogix.Library.ViewModels;
using RingSoft.HomeLogix.MasterData;
using System.ComponentModel;
using System.Windows;
using RingSoft.App.Library;
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
            var addEditHouseholdWindow = new AddEditHouseholdWindow(DbLoginProcesses.Add)
            {
                Owner = this,
                ShowInTaskbar = false
            };
            addEditHouseholdWindow.ShowDialog();
            return addEditHouseholdWindow.ViewModel.Object;
        }

        public bool EditHousehold(ref Household household)
        {
            var addEditHouseholdWindow = new AddEditHouseholdWindow(DbLoginProcesses.Edit, household)
            {
                Owner = this,
                ShowInTaskbar = false
            };
            addEditHouseholdWindow.ShowDialog();
            //if (addEditHouseholdWindow.DataCopied)
            //{
            //    var message = "You must restart the application in order to continue.";
            //    var caption = "Restart Application";
            //    MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
            //    Application.Current.Shutdown();
            //}

            if (addEditHouseholdWindow.ViewModel.DialogResult)
            {
                if (addEditHouseholdWindow.ViewModel.Object != null)
                    household = addEditHouseholdWindow.ViewModel.Object;
            }

            return household != null;
        }

        public AddEditHouseholdViewModel GetHouseholdConnection()
        {
            var addEditHouseholdWindow = new AddEditHouseholdWindow(DbLoginProcesses.Connect)
            {
                Owner = this,
                ShowInTaskbar = false
            };
            addEditHouseholdWindow.ShowDialog();
            return addEditHouseholdWindow.ViewModel;
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
