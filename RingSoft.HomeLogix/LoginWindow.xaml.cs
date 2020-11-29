using System.ComponentModel;
using System.Windows;
using RingSoft.HomeLogix.Library.ViewModels;
using RingSoft.HomeLogix.MasterData;

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

        public bool ShowAddEditHousehold(Households household)
        {
            var addEditHouseholdWindow = new AddEditHouseholdWindow(household);
            addEditHouseholdWindow.Owner = this;
            return addEditHouseholdWindow.ShowDialog() == true;
        }

        public string GetHouseholdDataFile()
        {
            throw new System.NotImplementedException();
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
